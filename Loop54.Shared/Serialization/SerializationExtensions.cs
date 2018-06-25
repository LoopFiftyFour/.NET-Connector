using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Loop54.Serialization
{
    internal static class SerializationExtensions
    {
        internal static T GetRequiredProperty<T>(this JObject jObject, string name)
        {
            if (jObject.TryGetValue(name, StringComparison.Ordinal, out JToken typeValue))
            {
                return typeValue.ToObject<T>();
            }

            throw new ApplicationException($"Required property '{name}' is missing");
        }

        internal static T GetPropertyOrDefault<T>(this JObject jObject, string name)
        {
            if (jObject.TryGetValue(name, StringComparison.Ordinal, out JToken typeValue))
            {
                return typeValue.ToObject<T>();
            }

            return default;
        }        
    }
}
