using Loop54.Serialization;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace Loop54.Model.Response
{
    /// <summary>
    /// A response from the engine. Used for responses that don't return any standardized data parameters.
    /// </summary>
    public class Response
    {
        /// <summary>
        /// Any additional, non-standard, data. Contact support for information about how and when to use this.
        /// </summary>
        public IDictionary<string, object> CustomData { get; set; }

        /// <summary>
        /// Retrieve a named custom value from the response. If not found, or the value could not be deserialized or cast 
        /// to T, it will throw a <see cref="CustomDataException"/>.
        /// </summary>
        /// <typeparam name="T">Expected type of the response data to retrieve.</typeparam>
        /// <param name="key">Key of the custom data.</param>
        /// <returns>Deserialized data as the specified type. If the data did not exist it's default is returned.</returns>
        public T GetCustomDataOrThrow<T>(string key)
        {
            (bool foundData, T data) = TryGetCustomData<T>(key);
            
            if(!foundData)
                throw new CustomDataException($"No data with key '{key}' found on the response");

            return data;
        }

        /// <summary>
        /// Retrieve a named custom value from the response. If not found it will return the default value.
        /// If the value could not be deserialized or cast to T it will throw a <see cref="CustomDataException"/>.
        /// </summary>
        /// <typeparam name="T">Expected type of the response data to retrieve.</typeparam>
        /// <param name="key">Key of the custom data.</param>
        /// <returns>Deserialized data as the specified type. If the data did not exist it's default is returned.</returns>
        public T GetCustomDataOrDefault<T>(string key) => TryGetCustomData<T>(key).data;

        private (bool foundData, T data) TryGetCustomData<T>(string key)
        {
            if (CustomData == null)
                return (false, default);

            if (CustomData.TryGetValue(key, out object data))
            {
                //When Json.NET deserialize complex json to a object-instance it ends up as a JToken.
                //Which we try to deserialize to the expected type.
                if (data is JToken token)
                {
                    try
                    {
                        return (true, Serializer.Deserialize<T>(token));
                    }
                    catch(Exception e)
                    {
                        throw new CustomDataException($"The data with key '{key}' couldn't be deserialized to '{typeof(T).ToString()}'", e, data);
                    }
                }

                if (data is T tData)
                    return (true, tData);
                
                throw new CustomDataException($"The data with key '{key}' couldn't be deserialized or cast to '{typeof(T).ToString()}'", data);
            }

            return (false, default);
        }
    }
}
