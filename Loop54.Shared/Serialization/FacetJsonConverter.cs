using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Linq;
using Loop54.Model.Response;
using Loop54.Model.Request.Parameters.Facets;
using Loop54.Model;

namespace Loop54.Serialization
{
    internal class FacetJsonConverter : JsonConverter
    {
        public override bool CanRead => true;
        public override bool CanWrite => false;

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Facet);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject jo = JObject.Load(reader);

            FacetType type = jo.GetRequiredProperty<FacetType>("type");
            string name = jo.GetRequiredProperty<string>("name");

            if(type == FacetType.Distinct)
                return DeserializeDistinct(jo, name);
            else if(type == FacetType.Range)
                return DeserializeRange(jo, name);

            throw new NotSupportedException($"FacetType '{type}' cannot be deserialized");
        }
        
        private static object DeserializeDistinct(JObject jo, string name)
        {
            return new DistinctFacet
            {
                Name = name,
                Items = jo.GetRequiredProperty<JArray>("items").Cast<JObject>().Select(itemObject => new DistinctFacet.DistinctFacetItem
                {
                    Count = itemObject.GetRequiredProperty<int>("count"),
                    Selected = itemObject.GetRequiredProperty<bool>("selected"),
                    Item = itemObject.GetRequiredProperty<JToken>("item"),
                }).ToList()
            };
        }
        
        private static object DeserializeRange(JObject jo, string name)
        {
            return new RangeFacet
            {
                Name = name,
                Min = jo.GetPropertyOrDefault<JToken>("min"),
                Max = jo.GetPropertyOrDefault<JToken>("max"),
                SelectedMin = jo.GetPropertyOrDefault<JToken>("selectedMin"),
                SelectedMax = jo.GetPropertyOrDefault<JToken>("selectedMax")
            };
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
