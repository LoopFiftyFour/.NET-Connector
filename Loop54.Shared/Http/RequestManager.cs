using Loop54.Model.Request;
using Loop54.Model.Response;
using Loop54.Properties;
using Loop54.Serialization;
using Loop54.User;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Loop54.Http
{
    /// <summary>
    /// Handles the request to the Loop54 api. This can be used as a singleton.
    /// </summary>
    public class RequestManager : IRequestManager
    {
        private readonly HttpClient _httpClient = new HttpClient(new HttpClientHandler
        {
            AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
        });
        
        private class RequestData
        {
            public string Action { get; set; }
            public byte[] Body { get; set; }
            public UserMetaData UserMetaData { get; set; }
            public string ApiKey { get; set; }
        }
        
        private Loop54Settings _settings;

        /// <summary>
        /// Settings used by this request manager.
        /// </summary>
        internal Loop54Settings Settings => _settings;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="settings">The settings for this instance to use when calling loop54.</param>
        public RequestManager(Loop54Settings settings)
        {
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
            _httpClient.Timeout = TimeSpan.FromMilliseconds(settings.RequestTimeoutMs);
        }

        /// <summary>
        /// Calls the loop54 search engine and returns a deserialized response object. 
        /// </summary>
        /// <typeparam name="TResponse">The expected response type. E.g. if making a search call the expected type would be <see cref="SearchResponse"/></typeparam>
        /// <typeparam name="TRequest">The request type. E.g. if making a search call use <see cref="SearchRequest"/> </typeparam>
        /// <param name="action">The type of request. This will translate into the resource on the actual HTTP request i.e. "endpoint/action". Must not be null.</param>
        /// <param name="requestData">The query data to send to the engine. Must not be null.</param>
        /// <param name="metaData">Data about the requesting user. Must not be null.</param>
        /// <returns>The desired response</returns>
        public async Task<TResponse> CallEngineAsync<TRequest, TResponse>(string action, TRequest requestData, UserMetaData metaData) where TResponse : Response where TRequest : Request
        {
            if(action == null)
                throw new ArgumentNullException(nameof(action));

            if (requestData == null)
                throw new ArgumentNullException(nameof(requestData));

            if (metaData == null)
                throw new ArgumentNullException(nameof(metaData));

            RequestData request = new RequestData
            {
                UserMetaData = metaData,
                Body = Serializer.SerializeToBytes(requestData),
                Action = action,
                ApiKey = _settings.ApiKey
            };

            byte[] responseBytes = await MakeHttpRequest(request);
            return Serializer.DeserializeBytes<TResponse>(responseBytes);
        }

        private async Task<byte[]> MakeHttpRequest(RequestData request)
        {
            var content = new ByteArrayContent(request.Body);
            SetRequestHeaders(content.Headers, request.UserMetaData);
            string endpoint = GetValidatedEndpoint();

            HttpResponseMessage message;
            try
            {
                message = await _httpClient.PostAsync($"{endpoint}/{request.Action}", content);
            }
            catch (Exception e)
            {
                throw new EngineNotReachableException($"Could not make request to engine at '{endpoint}', you might have entered the wrong endpoint or there"
                    + " might be a firewall blocking the port.", e);
            }

            if (message.IsSuccessStatusCode)
                return await message.Content.ReadAsByteArrayAsync();

            throw CreateEngineErrorException(await message.Content.ReadAsByteArrayAsync());
        }

        private string GetValidatedEndpoint()
        {
            string endpoint = Utils.FixEngineUrl(_settings.Endpoint);
            if (_settings.RequireHttps && !Utils.UrlIsHttps(endpoint))
                throw new ApplicationException($"If setting '{nameof(Loop54Settings.RequireHttps)}' is true the endpoint needs to use the protocol 'https'");
            return endpoint;
        }

        protected virtual void SetRequestHeaders(HttpContentHeaders headers, UserMetaData userMetaData)
        {
            headers.ContentType = new MediaTypeHeaderValue("application/json");
            headers.Add(VersionHeaders.ApiVersionHeader, VersionHeaders.ApiVersion);
            headers.Add(VersionHeaders.LibVersionHeader, VersionHeaders.LibVersion);
            AddHeaderIfNotNull(headers, Headers.ApiKey, _settings.ApiKey);
            AddHeaderIfNotNull(headers, Headers.UserId, userMetaData.UserId);
            AddHeaderIfNotNull(headers, Headers.IpAddress, userMetaData.IpAddress);
            AddHeaderIfNotNull(headers, Headers.UserAgent, userMetaData.UserAgent);
            AddHeaderIfNotNull(headers, Headers.Referer, userMetaData.Referer);
        }
        
        private static void AddHeaderIfNotNull(HttpContentHeaders headers, string headerName, string headerValue)
        {
            if (headerValue != null)
                headers.Add(headerName, headerValue);
        }
        
        private static Exception CreateEngineErrorException(byte[] responseData)
        {
            string responseText = Serializer.GetStringFromBytes(responseData);
            try
            {
                if (string.IsNullOrEmpty(responseText))
                    throw new ApplicationException("An empty response was received.");

                ErrorResponse errorResponse = Serializer.DeserializeString<ErrorResponse>(responseText);
                if (errorResponse.Error == null)
                    throw new ApplicationException("The response JSON does not have an 'Error' property.");

                return new EngineStatusCodeException(errorResponse.Error);
            }
            catch (Exception ex)
            {
                throw new InvalidEngineResponseException(responseText, ex);
            }
        }
    }
}
