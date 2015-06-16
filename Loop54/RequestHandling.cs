using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Loop54.Exceptions;
using Loop54.Public;

namespace Loop54
{

    
    
    public static class RequestHandling
    {
        public static Response GetResponse(string url, Request request)
        {
            return GetResponses(url, new[] { request })[request];
        }

        public static Response GetResponse(string url, Request request, int timeout)
        {
            return GetResponses(url, new[] { request }, timeout)[request];
        }

        public static Response GetResponse(string url, Request request, int timeout, bool measureResponse)
        {
            return GetResponses(url, new[] { request }, timeout,measureResponse)[request];
        }

        public static Response GetResponse(string url, Request request, int timeout, bool measureResponse, int numFailuresToFallBack, int minutesOnFallBack)
        {
            return GetResponses(url, new[] { request }, timeout, measureResponse,numFailuresToFallBack,minutesOnFallBack)[request];
        }

        public static IDictionary<Request, Response> GetResponses(string url, IEnumerable<Request> requests, int timeout = 5000, bool measureResponseTime = false, int numFailuresToFallBack = 5, int minutesOnFallBack = 5)
        {
            Stopwatch watch=null;
            long serializationTime=0;
            if (measureResponseTime)
            {
                watch=new Stopwatch();
                watch.Start();
            }

            url = Utils.Strings.FixEngineUrl(url);

            var ret = new Dictionary<Request, Response>();

            
            var requestData = "{";

            foreach (var request in requests)
            {
                requestData += request.Serialized + ",";
            }

            requestData = requestData.Trim(',') + "}";

            
            if (measureResponseTime)
            {
                watch.Stop();
                serializationTime = watch.ElapsedMilliseconds;
            }

            var httpResponse = Utils.Http.GetEngineResponse(url, "POST", requestData, timeout, measureResponseTime, numFailuresToFallBack,minutesOnFallBack);

            if (measureResponseTime)
            {
                watch.Reset(); //tillagt i 2.0.19
                watch.Start();
            }

            var json = JSON.JsonDecode(httpResponse.Content) as Hashtable;

            if (json == null)
                throw new DeserializationException();

            
            var responseObj = Serialization.DeserializePersistent<Response>(json);

            var data = json["Data"] as Hashtable;

            foreach (var request in requests)
            {
                var newResp = responseObj.CreateClone();

                if (data != null)
                    newResp.Data = data[request.Name] as Hashtable;

                newResp.ContentLength = httpResponse.ContentLength;

                ret.Add(request, newResp);
            }

            if (measureResponseTime)
            {
                watch.Stop();
                
                foreach (var response in ret.Values)
                {
                    response.SerializationTime = serializationTime;
                    response.DeserializationTime = watch.ElapsedMilliseconds;

                    response.ResponseTime = httpResponse.ResponseTime;
                    response.ReadDataTime = httpResponse.ReadDataTime;
                    response.RequestTime = httpResponse.RequestTime;
                    response.EngineTime = httpResponse.EngineTime;
                    response.RoundtripTime = httpResponse.RoundtripTime;
                    response.AddHeadersTime = httpResponse.AddHeadersTime;
                    response.CreateRequestTime = httpResponse.CreateRequestTime;
                    response.SetUpSPMTime = httpResponse.SetUpSPMTime;

                    
                }
            }

            return ret;
        }

        
    }

    

}
