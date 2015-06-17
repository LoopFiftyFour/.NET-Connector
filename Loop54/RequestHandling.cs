using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Loop54.Exceptions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Loop54
{

    
    
    public static class RequestHandling
    {

        #region Overloads
        public static Response GetResponse(string url, Request request)
        {
            return GetResponse(url,  request );
        }

        public static Response GetResponse(string url, Request request, int timeout)
        {
            return GetResponse(url, request, timeout,false);
        }

        public static Response GetResponse(string url, Request request, int timeout, bool measureResponse)
        {
            return GetResponse(url, request, timeout,measureResponse,5,5);
        }
        #endregion


        private static Response GetResponse(string url, Request request, int timeout, bool measureResponseTime, int numFailuresToFallBack, int minutesOnFallBack)
        {
            Stopwatch watch=null;
            long serializationTime=0;
            if (measureResponseTime)
            {
                watch=new Stopwatch();
                watch.Start();
            }

            url = Utils.Strings.FixEngineUrl(url);

            
            var requestData = "{";
            requestData += request.Serialized + ",";
            requestData = requestData.Trim(',') + "}";

            
            if (measureResponseTime)
            {
                watch.Stop();
                serializationTime = watch.ElapsedMilliseconds;
            }

            var httpResponse = Utils.Http.GetEngineResponse(url, "POST", requestData, timeout, measureResponseTime, numFailuresToFallBack,minutesOnFallBack);

            if (measureResponseTime)
            {
                watch.Reset();
                watch.Start();
            }

            var json = JObject.Parse(httpResponse.Content);

            if (json == null)
                throw new DeserializationException();

            //If data is wrapped in quest name, use the data within
            if (json[request.Name] != null)
                json = json[request.Name].Value<JObject>();



            var response = json.ToObject<Response>();



            response.ContentLength = httpResponse.ContentLength;

            

            if (measureResponseTime)
            {
                watch.Stop();
                
                response.SerializationTime = serializationTime;
                response.DeserializationTime = watch.ElapsedMilliseconds;

                response.ResponseTime = httpResponse.ResponseTime;
                response.ReadDataTime = httpResponse.ReadDataTime;
                response.RequestTime = httpResponse.RequestTime;
                response.EngineTime = httpResponse.EngineTime;
                response.RoundtripTime = httpResponse.RoundtripTime;
                response.AddHeadersTime = httpResponse.AddHeadersTime;
                response.CreateRequestTime = httpResponse.CreateRequestTime;

            }

            return response;
        }

        
    }

    

}
