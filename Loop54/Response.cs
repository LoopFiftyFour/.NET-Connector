using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Loop54.Exceptions;
using Newtonsoft.Json;

namespace Loop54
{

    public class Response
    {

        public bool Success { get; private set; }


        public int Error_Code { get; private set; }


        public string Error_Message { get; private set; }


        public string RequestId { get; private set; }

        public long ContentLength { get; internal set; }

        public long SerializationTime { get; internal set; }
        public long DeserializationTime { get; internal set; }
        public long RoundtripTime { get; internal set; }
        public long RequestTime { get; internal set; }
        public long EngineTime { get; internal set; }
        public long ResponseTime { get; internal set; }
        public long ReadDataTime { get; internal set; }

        internal Dictionary<string,string> Data = new Dictionary<string, string>();
        public long AddHeadersTime;
        public long CreateRequestTime;
        public long SetUpSPMTime;

        internal Response()
        {
            
        }

        internal Response CreateClone()
        {
            return new Response()
                {
                    Success = Success,
                    Error_Code = Error_Code,
                    Error_Message = Error_Message,
                    RequestId = RequestId
                };
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
                    return JsonConvert.DeserializeObject<T>(Data[key]);
                }
                catch (Exception ex)
                {
                    throw new SerializationException("Could not deserialize data in slot " + key,ex);
                }
                
            }
        }
    }
}
