using Newtonsoft.Json.Serialization;
using System;

namespace Loop54.Engine.Response
{
    //used to serialize properties with camelCase but keep original casing for dictionaries (for instance entity attributes)
    internal class CamelCaseExceptDictionaryKeysResolver : CamelCasePropertyNamesContractResolver
    {
        protected override JsonDictionaryContract CreateDictionaryContract(Type objectType)
        {
            JsonDictionaryContract contract = base.CreateDictionaryContract(objectType);

            contract.DictionaryKeyResolver = propertyName => propertyName;

            return contract;
        }
    }
}
