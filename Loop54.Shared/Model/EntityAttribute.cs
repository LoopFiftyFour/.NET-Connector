using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Loop54.Model
{
    /// <summary>
    /// Represents an attribute on an entity in the loop54 search engine.
    /// This could for instance be "Price", "Name" or "Category" values that belong to an entity.
    /// </summary>
    public class EntityAttribute
    {
        /// <summary>Attribute names are treated as case-insensitive by the engine, though the case received should be preserved preserved.</summary>
        public static readonly StringComparer NameComparer = StringComparer.OrdinalIgnoreCase;

        /// <summary>
        /// Creates an empty attribute. Used when deserializing.
        /// </summary>
        public EntityAttribute()
        {
        }

        /// <summary>
        /// Creates an attribute with the specified name and values. The attribute type sent to the engine is determined from <typeparamref name="T"/>
        /// (or from the values, if <typeparamref name="T"/> is <see cref="object"/>).
        /// </summary>
        /// <param name="name">Name of the attribute. For instance "Price", "Name" or "Category".</param>
        /// <param name="values">The values of the attribute. May be empty, for instance when removing the attribute in a SyncDeltaPush request.</param>
        public static EntityAttribute Create<T>(string name, params T[] values)
        {
            var attribute = new EntityAttribute { Name = name ?? throw new ArgumentNullException(nameof(name)) };
            attribute.SetValues<T>(values ?? new T[0]);
            return attribute;
        }

        /// <summary>
        /// Name of the attribute. For instance "Price", "Name" or "Category".
        /// </summary>
        public string Name { get; set; }
        
        internal EntityAttributeType Type { get; set; }

        internal JToken[] ValuesInternal { get; set; } = new JToken[0];

        /// <summary>
        /// Casts the values to the specified generic type and return the first one or null.
        /// </summary>
        /// <returns>The first (or only) value or null if empty.</returns>
        public T GetValue<T>() => GetValues<T>().FirstOrDefault();

        /// <summary>
        /// Casts the values to the specified generic type and return them all as a list.
        /// </summary>
        /// <returns>All values with the desired type as a list.</returns>
        public IList<T> GetValues<T>()
        {
            return ValuesInternal.Select(v => v.ToObject<T>()).ToList();
        }

        /// <summary>
        /// Sets the value of the attribute. Will overwrite any existing value or values.
        /// </summary>
        public void SetValue<T>(T value) => SetValues<T>(new[] { value });

        /// <summary>
        /// Sets the values of the attribute. Will overwrite any existing value or values.
        /// </summary>
        public void SetValues<T>(IList<T> values)
        {
            if (values == null)
                throw new ArgumentNullException(nameof(values));

            Type = GetAttributeType(typeof(T), values.Cast<object>());
            ValuesInternal = values.Select(v => v == null ? JValue.CreateNull() : JToken.FromObject(v)).ToArray();
        }

        private static EntityAttributeType GetAttributeType(Type type, IEnumerable<object> values)
        {
            type = Nullable.GetUnderlyingType(type) ?? type;

            if (type == typeof(object))
            {
                // The static type tells us nothing, use the runtime type of the first non-null value instead.
                object first = values.FirstOrDefault(v => v != null);
                if (first == null)
                    return EntityAttributeType.String;
                type = first.GetType();
            }

            if (type == typeof(string) || type == typeof(char) || type == typeof(Guid))
                return EntityAttributeType.String;
            if (type == typeof(bool))
                return EntityAttributeType.Boolean;
            if (type == typeof(int) || type == typeof(long) || type == typeof(short) || type == typeof(byte)
                || type == typeof(uint) || type == typeof(ulong) || type == typeof(ushort) || type == typeof(sbyte))
                return EntityAttributeType.Integer;
            if (type == typeof(double) || type == typeof(float) || type == typeof(decimal))
                return EntityAttributeType.Number;
            if (type == typeof(DateTime) || type == typeof(DateTimeOffset))
                return EntityAttributeType.Date;
            if (typeof(IEnumerable).IsAssignableFrom(type))
                return EntityAttributeType.Array;

            return EntityAttributeType.Object;
        }
    }
}
