using System;
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
                v.WriteTo(writer);
            writer.WriteEndArray();

            writer.WriteEndObject();
        }
    }
}
