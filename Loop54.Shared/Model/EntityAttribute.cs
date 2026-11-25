using Loop54.Serialization;
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

            // Convert with the same serializer as the rest of the request, so complex values get camelCased property names and enums
            // are written as strings. The public SetValue(s) methods keep their original conversion, to not change existing callers' data.
            JToken[] tokens = (values ?? new T[0])
                .Select(v => v == null ? JValue.CreateNull() : JToken.FromObject(v, Serializer.JsonSerializer))
                .ToArray();
            attribute.Type = GetAttributeType(typeof(T), tokens) ?? throw new ArgumentException(
                "All values of an attribute must have the same type, but got values of types " +
                string.Join(", ", GetValueTypes(tokens)) + ".", nameof(values));
            attribute.ValuesInternal = tokens;
            return attribute;
        }

        /// <summary>
        /// Name of the attribute. For instance "Price", "Name" or "Category".
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>The attribute type sent to the engine. Null if it could not be determined, for instance when the values have mixed types.</summary>
        internal EntityAttributeType? Type { get; set; }

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
            ValuesInternal = values.Select(v => JToken.FromObject(v)).ToArray();

            // Only needed if the attribute is sent in a SyncDeltaPush request, so failing to infer it must not break existing callers.
            try
            {
                Type = GetAttributeType(typeof(T), ValuesInternal);
            }
            catch (ArgumentException)
            {
                Type = null;
            }
        }

        // Returns null if the values have more than one type (other than a mix of integers and numbers).
        private static EntityAttributeType? GetAttributeType(Type type, JToken[] tokens)
        {
            // The values decide the type when there are any, since T may be object, an interface or a base type.
            var types = GetValueTypes(tokens);

            if (types.Count == 0)
                return GetAttributeType(type);
            if (types.Count == 1)
                return types[0];
            if (types.Count == 2 && types.Contains(EntityAttributeType.Integer) && types.Contains(EntityAttributeType.Number))
                return EntityAttributeType.Number;

            return null;
        }

        private static List<EntityAttributeType> GetValueTypes(JToken[] tokens) =>
            tokens.Where(t => t.Type != JTokenType.Null).Select(GetAttributeType).Distinct().ToList();

        private static EntityAttributeType GetAttributeType(JToken token)
        {
            switch (token.Type)
            {
                case JTokenType.String:
                case JTokenType.Guid:
                case JTokenType.Uri:
                case JTokenType.TimeSpan:
                case JTokenType.Bytes:
                    return EntityAttributeType.String;
                case JTokenType.Boolean:
                    return EntityAttributeType.Boolean;
                case JTokenType.Integer:
                    return EntityAttributeType.Integer;
                case JTokenType.Float:
                    return EntityAttributeType.Number;
                case JTokenType.Date:
                    return EntityAttributeType.Date;
                case JTokenType.Array:
                    return EntityAttributeType.Array;
                case JTokenType.Object:
                    return EntityAttributeType.Object;
                default:
                    throw new ArgumentException($"Attribute values of JSON type {token.Type} are not supported.");
            }
        }

        // Used when there are no non-null values to look at, for instance when removing an attribute in a SyncDeltaPush request.
        private static EntityAttributeType GetAttributeType(Type type)
        {
            type = Nullable.GetUnderlyingType(type) ?? type;

            if (type == typeof(string) || type == typeof(char) || type == typeof(Guid) || type.IsEnum)
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
            if (type == typeof(object))
                return EntityAttributeType.String;
            if (typeof(IDictionary).IsAssignableFrom(type) || typeof(JObject).IsAssignableFrom(type))
                return EntityAttributeType.Object;
            if (typeof(IEnumerable).IsAssignableFrom(type))
                return EntityAttributeType.Array;

            return EntityAttributeType.Object;
        }
    }
}
