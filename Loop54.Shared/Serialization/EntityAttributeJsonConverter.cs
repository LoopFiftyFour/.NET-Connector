using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Linq;
using Loop54.Model.Response;
using Loop54.Model.Request.Parameters.Facets;
using Loop54.Model;
using System.Collections.Concurrent;

namespace Loop54.Serialization
{
    internal class EntityAttributeJsonConverter : JsonConverter
    {
        private ConcurrentDictionary<Type, Type> _cachedGenerics = new ConcurrentDictionary<Type, Type>();

        public override bool CanRead => true;
        public override bool CanWrite => true;

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(EntityAttribute);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject jo = JObject.Load(reader);

            return new EntityAttribute
            {
                Type = jo.GetRequiredProperty<EntityAttributeType>("type"),
                Name = jo.GetRequiredProperty<string>("name"),
                ValuesInternal = jo.GetRequiredProperty<JArray>("values").ToArray()
            };
        }
        
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var attribute = (EntityAttribute)value;

            writer.WriteStartObject();

            writer.WritePropertyName("name");
            writer.WriteValue(attribute.Name);

            writer.WritePropertyName("type");
            serializer.Serialize(writer, attribute.Type);

            writer.WritePropertyName("values");
            writer.WriteStartArray();
            foreach (var v in attribute.ValuesInternal ?? new JToken[0])
                v.WriteTo(writer);
            writer.WriteEndArray();

            writer.WriteEndObject();
        }
    }
}
