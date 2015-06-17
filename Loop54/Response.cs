using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Loop54.Exceptions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Loop54
{

    public class Response
    {
        #region JsonData
        public bool Success { get; internal set; }

        public int Error_Code { get; internal set; }
        public string Error_Message { get; internal set; }

        public string RequestId { get; internal set; }

        internal Dictionary<string, JToken> Data = new Dictionary<string, JToken>();
        #endregion


        public long ContentLength { get; internal set; }

        public long SerializationTime { get; internal set; }
        public long DeserializationTime { get; internal set; }
        public long RoundtripTime { get; internal set; }
        public long RequestTime { get; internal set; }
        public long EngineTime { get; internal set; }
        public long ResponseTime { get; internal set; }
        public long ReadDataTime { get; internal set; }
        public long AddHeadersTime;
        public long CreateRequestTime;
        public long SetUpSPMTime;

        
        

        internal Response()
        {
            
        }

      
        public bool HasData(string key)
        {
            lock(Data)
                return Data.ContainsKey(key);
        }

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
