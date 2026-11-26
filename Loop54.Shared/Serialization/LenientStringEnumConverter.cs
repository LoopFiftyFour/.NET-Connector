using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace Loop54.Serialization
{
    /// <summary>
    /// Like the default enum converter, but deserializes values this version of the connector does not know about to the default enum value
    /// (which should be named "Unknown") instead of throwing. Used for response enums that the engine may add new values to.
    /// </summary>
    internal class LenientStringEnumConverter : StringEnumConverter
    {
        public LenientStringEnumConverter()
        {
            NamingStrategy = new CamelCaseNamingStrategy();
            AllowIntegerValues = false;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            // Only unrecognized strings are tolerated. Nulls, numbers and other tokens fail exactly like with the default converter.
            if (reader.TokenType != JsonToken.String)
                return base.ReadJson(reader, objectType, existingValue, serializer);

            try
            {
                return base.ReadJson(reader, objectType, existingValue, serializer);
            }
            catch (JsonSerializationException)
            {
                Type enumType = Nullable.GetUnderlyingType(objectType) ?? objectType;
                return Activator.CreateInstance(enumType);
            }
        }
    }
}
