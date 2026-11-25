using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Loop54.Model
{
    /// <summary>
    /// An entity represents one Product (or document). It is identified by the id and type and contains named attributes.
    /// </summary>
    public class Entity
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="type">The type of the entity. This is usually "Product"</param>
        /// <param name="id">The unique id of the entity. Could for instance be a SKU id.</param>
        public Entity(string type, string id)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
            Type = type ?? throw new ArgumentNullException(nameof(type));
        }

        /// <summary>
        /// The unique id of the entity. Could for instance be a SKU id.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// The type of the entity. This is usually "Product". This is used to distinguish between products and non-product entities (such as content).
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// All attributes available on the entity. When sending an Entity with a CreateEvent call these are not necessary to be set.
        /// </summary>
        public List<EntityAttribute> Attributes
        {
            get
            {
                return _internalAttributeMapping?.Select(kv => kv.Value).ToList();
            }
            set
            {
                _internalAttributeMapping = value?.ToDictionary(k => k.Name, v => v, EntityAttribute.NameComparer);
            }
        }

        private Dictionary<string, EntityAttribute> _internalAttributeMapping;

        /// <summary>
        /// Adds an attribute with the specified name and values to the entity, replacing any existing attribute with the same name.
        /// The attribute type sent to the engine is determined from <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">Type of the attribute values, for instance string, int, double, bool or DateTime.</typeparam>
        /// <param name="name">Name of the attribute.</param>
        /// <param name="values">The attribute values. May be empty, for instance when removing the attribute in a SyncDeltaPush request.</param>
        /// <returns>This entity, to allow chaining.</returns>
        public Entity SetAttribute<T>(string name, params T[] values)
        {
            var attribute = EntityAttribute.Create(name, values);

            if (_internalAttributeMapping == null)
                _internalAttributeMapping = new Dictionary<string, EntityAttribute>(EntityAttribute.NameComparer);

            _internalAttributeMapping[name] = attribute;
            return this;
        }

        /// <summary>
        /// Gets the attribute value with the desired name and type.
        /// </summary>
        /// <typeparam name="T">Type of the attribute. This needs to match what the library has deserialized or an exception will be thrown.</typeparam>
        /// <param name="name">Name of the attribute.</param>
        /// <returns>The attribute value. If more than one attribute exist the first one will be returned. If the attribute does not exist null will be returned.</returns>
        public T GetAttributeValueOrDefault<T>(string name)
        {
            IList<T> attributeValues = GetAttributeValuesOrDefault<T>(name);

            if (attributeValues == null || attributeValues.Count == 0)
                return default;

            return attributeValues[0];
        }

        /// <summary>
        /// Gets the attribute values with the desired name and type.
        /// </summary>
        /// <typeparam name="T">Type of the attribute. This needs to match what the library has deserialized or an exception will be thrown.</typeparam>
        /// <param name="name">Name of the attribute.</param>
        /// <returns>All values related to the attribute as an array or null if the attribute does not exist.</returns>
        public IList<T> GetAttributeValuesOrDefault<T>(string name)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            if (_internalAttributeMapping.TryGetValue(name, out EntityAttribute attribute))
                return attribute.GetValues<T>();

            return null;
        }
    }
}
