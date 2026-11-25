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
        // Writing is off by default, so entities in events and recommendation requests are sent like before (attribute names only).
        // Full attributes are only written for SyncDeltaPush, through SyncEntityJsonConverter.
        public override bool CanWrite => false;

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(EntityAttribute);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            // Date values stay strings (the serializer uses DateParseHandling.None), so they are sent back unchanged in a SyncDeltaPush request.
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
            throw new NotImplementedException();
        }
    }
}
