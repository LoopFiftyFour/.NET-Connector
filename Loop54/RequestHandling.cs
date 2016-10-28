using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Loop54.Exceptions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Loop54
{

    
    /// <summary>
    /// Used to create HTTP requests to an engine based on a Request object.
    /// </summary>
    public static class RequestHandling
    {

        
        /// <summary>
        /// Sends the serialized request to the server and retrieves and deserializes the response.
        /// </summary>
        /// <param name="url">The engine endpoint.</param>
        /// <param name="request">The request object to serialize and send.</param>
        /// <returns>A deserialized response in the form of a Response object.</returns>
        public static Response GetResponse(string url, Request request)
        {
            Stopwatch watch=null;
            long serializationTime=0;
            if (request.Options.MeasureTime)
            {
                watch=new Stopwatch();
                watch.Start();
            }

            url = Utils.Strings.FixEngineUrl(url);
            url += request.QuestName;
            
            var requestData = request.Serialized;

            if (request.Options.MeasureTime)
            {
                watch.Stop();
                serializationTime = watch.ElapsedMilliseconds;
            }

            var httpResponse = Utils.Http.GetEngineResponse(url, "POST", requestData, request.Options.Timeout, request.Options.MeasureTime);

            if (request.Options.MeasureTime)
            {
                watch.Reset();
                watch.Start();
            }

            var json = JObject.Parse(httpResponse.Content);

            if (json == null)
                throw new DeserializationException();

            if(json["Data"]==null)
                throw new DataNotFoundException("Data");

            var data = json["Data"].Value<JObject>();
            
            var response = new Response();
            response.Success = json["Success"].ToObject<bool>();
            response.RequestId = json["HeroId"].ToObject<string>();

            foreach (var key in data.Properties())
                response.Data[key.Name] = data[key.Name];



            response.ContentLength = httpResponse.ContentLength;



            if (request.Options.MeasureTime)
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
