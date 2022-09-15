using Newtonsoft.Json.Linq;
using System;
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
        public void SetValue<T>(T value)
        {
            ValuesInternal = new[] { JToken.FromObject(value) };
        }

        /// <summary>
        /// Sets the values of the attribute. Will overwrite any existing value or values.
        /// </summary>
        public void SetValues<T>(IList<T> values)
        {
            ValuesInternal = values.Select(v => JToken.FromObject(v)).ToArray();
        }
    }
}
