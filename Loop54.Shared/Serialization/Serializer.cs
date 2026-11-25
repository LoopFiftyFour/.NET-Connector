using Loop54.Engine.Response;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Text;
using Newtonsoft.Json.Serialization;

namespace Loop54.Serialization
{
    internal class Serializer
    {
        private static readonly Encoding _encoding = new UTF8Encoding(false);

        private static readonly JsonSerializer _jsonSerializer = new JsonSerializer
        {
            ContractResolver = new CamelCaseExceptDictionaryKeysResolver(),
            NullValueHandling = NullValueHandling.Ignore,
            DateParseHandling =  DateParseHandling.None,

            Formatting = Formatting.None,
            Converters = {
                new StringEnumConverter
                {
                    NamingStrategy = new CamelCaseNamingStrategy(),
                    AllowIntegerValues = false
                },
                new EntityAttributeJsonConverter(),
                new FacetJsonConverter()
            }
        };

        /// <summary>The serializer used for all engine requests and responses, so values converted ahead of time are written the same way.</summary>
        internal static JsonSerializer JsonSerializer => _jsonSerializer;

        // Default settings, used only to convert date strings below.
        private static readonly JsonSerializer _valueSerializer = new JsonSerializer();

        /// <summary>
        /// Converts a value token to <typeparamref name="T"/>. Dates are kept as strings when deserializing (DateParseHandling.None), and
        /// <see cref="JToken.ToObject{T}()"/> would parse them with DateTime.Parse, turning UTC times into local ones. So date strings are parsed
        /// like a DateTime property would be instead, keeping the time zone as sent (for instance DateTimeKind.Utc for "Z").
        /// </summary>
        internal static T ConvertValue<T>(JToken token) =>
            token.Type == JTokenType.String && IsDateType(typeof(T)) ? token.ToObject<T>(_valueSerializer) : token.ToObject<T>();

        internal static bool IsDateType(Type type)
        {
            type = Nullable.GetUnderlyingType(type) ?? type;
            return type == typeof(DateTime) || type == typeof(DateTimeOffset);
        }

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
