using System;
using System.Collections;
using Loop54.Exceptions;
using Loop54.Public;

namespace Loop54
{

    [SerializableClass]
    public class Response
    {
        [SerializableField]
        public bool Success { get; private set; }

        [SerializableField]
        public int Error_Code { get; private set; }

        [SerializableField]
        public string Error_Message { get; private set; }

        [SerializableField]
        public string RequestId { get; private set; }

        public long ContentLength { get; internal set; }

        public long SerializationTime { get; internal set; }
        public long DeserializationTime { get; internal set; }
        public long RoundtripTime { get; internal set; }
        public long RequestTime { get; internal set; }
        public long EngineTime { get; internal set; }
        public long ResponseTime { get; internal set; }
        public long ReadDataTime { get; internal set; }

        internal Hashtable Data = new Hashtable();
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
                    throw new DataNotFoundException(key);

                try
                {
                    return Serialization.DeserializeObject<T>(Data[key]);
                }
                catch (Exception ex)
                {
                    throw new Exception("Could not deserialize data in slot " + key,ex);
                }
                
            }
        }

        public ItemCollection GetCollection(string key)
        {
            return GetValue<ItemCollection>(key);
        }



    }

}
