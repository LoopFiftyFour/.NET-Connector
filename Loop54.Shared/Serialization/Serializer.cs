using Loop54.Engine.Response;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Loop54.Serialization
{
    internal class Serializer
    {
        private static Encoding _encoding = new UTF8Encoding(false);

        private static JsonSerializer _serializer = new JsonSerializer
        {
            ContractResolver = new CamelCaseExceptDictionaryKeysResolver(),
            NullValueHandling = NullValueHandling.Ignore,

            Formatting = Formatting.None,
            Converters = {
                new StringEnumConverter
                {
                    CamelCaseText = true,
                    AllowIntegerValues = false
                },
                new EntityAttributeJsonConverter(),
                new FacetJsonConverter()
            }
        };

        internal static byte[] SerializeToBytes(object data)
        {
            try
            {
                using (MemoryStream outputstream = new MemoryStream())
                using (StreamWriter streamWriter = new StreamWriter(outputstream, _encoding))
                {
                    _serializer.Serialize(streamWriter, data);
                    streamWriter.Flush();
                    return outputstream.ToArray();
                }
            }
            catch (Exception e)
            {
                throw new SerializationException($"Could not serialize object of type {data.GetType().Name}", e);
            }
        }

        internal static T Deserialize<T>(byte[] data)
        {
            try
            {
                using (MemoryStream stream = new MemoryStream(data))
                using (StreamReader streamReader = new StreamReader(stream))
                using (JsonTextReader jsonReader = new JsonTextReader(streamReader))
                {
                    return _serializer.Deserialize<T>(jsonReader);
                }
            }
            catch (Exception e)
            {
                throw new SerializationException($"Could not deserialize object of type {data.GetType().Name}", e);
            }
        }
    }
}
