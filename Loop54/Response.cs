using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Loop54.Exceptions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Loop54
{

    public class Response
    {
        #region JsonData
        /// <summary>
        /// If false, the response was unsuccessful in some regard.
        /// </summary>
        public bool Success { get; internal set; }

        /// <summary>
        /// The ID of the request/response round-trip. Set by the engine. 
        /// </summary>
        public string RequestId { get; internal set; }


        internal Dictionary<string, JToken> Data = new Dictionary<string, JToken>();
        #endregion

        /// <summary>
        /// The size of the response.
        /// </summary>
        public long ContentLength { get; internal set; }


        public long SerializationTime { get; internal set; }
        public long DeserializationTime { get; internal set; }
        public long RoundtripTime { get; internal set; }
        public long RequestTime { get; internal set; }
        public long EngineTime { get; internal set; }
        public long ResponseTime { get; internal set; }
        public long ReadDataTime { get; internal set; }
        public long AddHeadersTime { get; internal set; }
        public long CreateRequestTime { get; internal set; }
        public long SetUpSPMTime { get; internal set; }

        
        

        internal Response()
        {
            
        }

        /// <summary>
        /// Returns whether the response has a given data value or not.
        /// </summary>
        /// <param name="key">The name of the data.</param>
        /// <returns>True if the data exists, otherwise false.</returns>
        public bool HasData(string key)
        {
            lock (Data)
            {
                return Data.ContainsKey(key);
            }
        }

        /// <summary>
        /// Returns a list of keys to the data available in the response.
        /// </summary>
        /// <returns>Keys to the data in the response.</returns>
        public IList<string> GetKeys()
        {
            lock (Data)
            {
                return Data.Keys.ToList();
            }
        }

        /// <summary>
        /// Gets the data associated with a key.
        /// </summary>
        /// <typeparam name="T">The type of data.</typeparam>
        /// <param name="key">The name of the data.</param>
        /// <returns>The data.</returns>
        public T GetValue<T>(string key)
        {
            lock (Data)
            {
                if (!HasData(key))
                    throw new KeyNotFoundException(key);

                try
                {
                    return Data[key].ToObject<T>();
                }
                catch (Exception ex)
                {
                    throw new SerializationException("Could not deserialize data in slot " + key,ex);
                }
                
            }
        }
    }
}
