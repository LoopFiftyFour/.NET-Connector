using System;
using System.Collections.Generic;
using System.Linq;
using Loop54.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Loop54.Serialization
{
    /// <summary>
    /// Writes an entity with full attributes (name, type and values).
    /// </summary>
    /// <remarks>Only used for the entity collections where it's important to also have entity attributes (like SyncDeltaPush request).</remarks>
    internal class SyncEntityJsonConverter : JsonConverter
    {
        internal static readonly SyncEntityJsonConverter Instance = new SyncEntityJsonConverter();

        public override bool CanRead => false;
        public override bool CanWrite => true;

        public override bool CanConvert(Type objectType) => objectType == typeof(Entity);

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var entity = (Entity)value;

            writer.WriteStartObject();

            writer.WritePropertyName("type");
            writer.WriteValue(entity.Type);

            writer.WritePropertyName("id");
            writer.WriteValue(entity.Id);

            var attributes = entity.Attributes;
            if (attributes != null)
            {
                writer.WritePropertyName("attributes");
                writer.WriteStartArray();
                foreach (var attribute in attributes)
                    WriteFullAttribute(writer, attribute, serializer);
                writer.WriteEndArray();
            }

            writer.WriteEndObject();
        }

        /// <summary>Writes the attribute with its name, type and values, as the engine expects it in a SyncDeltaPush request.</summary>
        private static void WriteFullAttribute(JsonWriter writer, EntityAttribute attribute, JsonSerializer serializer)
        {
            if (attribute == null)
                throw new InvalidOperationException("An entity's attribute list must not contain null.");
            if (attribute.Type == null)
                throw new InvalidOperationException($"Could not determine the type of attribute \"{attribute.Name}\". " +
                                                    "All values of an attribute must have the same type.");

            writer.WriteStartObject();

            writer.WritePropertyName("name");
            writer.WriteValue(attribute.Name);

            writer.WritePropertyName("type");
            serializer.Serialize(writer, attribute.Type.Value);

            writer.WritePropertyName("values");
            writer.WriteStartArray();
            foreach (var v in attribute.ValuesInternal ?? new JToken[0])
            {
                if (IsNonFiniteNumber(v) || (v is JContainer container && container.Descendants().Any(IsNonFiniteNumber)))
                    throw new InvalidOperationException($"Attribute \"{attribute.Name}\" has a NaN or infinite value, which the engine does not accept.");
                v.WriteTo(writer);
            }
            writer.WriteEndArray();

            writer.WriteEndObject();
        }

        private static bool IsNonFiniteNumber(JToken token)
        {
            switch ((token as JValue)?.Value)
            {
                case double d: return double.IsNaN(d) || double.IsInfinity(d);
                case float f: return float.IsNaN(f) || float.IsInfinity(f);
                default: return false;
            }
        }
    }

    /// <summary>
    /// Writes a list of entities with full attributes. Fails on null entities, which the engine would reject together with the whole request.
    /// </summary>
    internal class SyncEntityListJsonConverter : JsonConverter
    {
        public override bool CanRead => false;
        public override bool CanWrite => true;

        public override bool CanConvert(Type objectType) => typeof(IEnumerable<Entity>).IsAssignableFrom(objectType);

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteStartArray();
            foreach (var entity in (IEnumerable<Entity>)value)
            {
                if (entity == null)
                    throw new InvalidOperationException($"The entity list \"{writer.Path}\" must not contain null entities.");
                SyncEntityJsonConverter.Instance.WriteJson(writer, entity, serializer);
            }
            writer.WriteEndArray();
        }
    }
}
