using Loop54.Engine.Response;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Text;

namespace Loop54.Serialization
{
    internal class Serializer
    {
        private static readonly Encoding _encoding = new UTF8Encoding(false);

        private static readonly JsonSerializer _jsonSerializer = new JsonSerializer
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

        internal static string BytesToString(byte[] serializedBytes) => _encoding.GetString(serializedBytes);

        internal static byte[] SerializeToBytes(object data)
        {
            try
            {
                using (MemoryStream outputstream = new MemoryStream())
                using (StreamWriter streamWriter = new StreamWriter(outputstream, _encoding))
                {
                    _jsonSerializer.Serialize(streamWriter, data);
                    streamWriter.Flush();
                    return outputstream.ToArray();
                }
            }
            catch (Exception e)
            {
                throw new SerializationException($"Could not serialize object of type {data.GetType().Name}", e);
            }
        }

        internal static T DeserializeToken<T>(JToken token)
        {
            try
            {
                return token.ToObject<T>(_jsonSerializer);
            }
            catch (Exception e)
            {
                throw new SerializationException($"Could not deserialize object of type {typeof(T)}", e);
            }
        }

        internal static string GetStringFromBytes(byte[] data)
        {
            return _encoding.GetString(data);
        }

        internal static T DeserializeBytes<T>(byte[] data)
        {
            var json = GetStringFromBytes(data);
            return DeserializeString<T>(json);
        }

        internal static T DeserializeString<T>(string json)
        {
            try
            {
                using (var stringReader = new StringReader(json))
                using (var jsonReader = new JsonTextReader(stringReader))
                {
                    return _jsonSerializer.Deserialize<T>(jsonReader);
                }
            }
            catch (Exception e)
            {
                throw new SerializationException($"Could not deserialize object of type {typeof(T)}", e);
            }
        }
    }
}
