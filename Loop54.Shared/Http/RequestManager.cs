using Loop54.AspNet;
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
        private HttpClient _httpClient = new HttpClient(new HttpClientHandler()
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
            return Serializer.Deserialize<TResponse>(responseBytes);
        }

        private async Task<byte[]> MakeHttpRequest(RequestData request)
        {
            var content = new ByteArrayContent(request.Body);
            SetHeadersOnRequest(content, request);
            string endpoint = GetValidatedEndpoint();

            HttpResponseMessage message = null;

            try
            {
                message = await _httpClient.PostAsync($"{endpoint}/{request.Action}", content);
            }
            catch (Exception e)
            {
                throw new EngineNotReachableException($"Could not make request to engine at '{endpoint}', you might have entered the wrong endpoint or there might " +
                    $"be a firewall blocking outgoing port 80", e);
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

        private void SetHeadersOnRequest(ByteArrayContent content, RequestData request)
        {
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            content.Headers.Add(VersionHeaders.ApiVersionHeader, VersionHeaders.ApiVersion);
            content.Headers.Add(VersionHeaders.LibVersionHeader, VersionHeaders.LibVersion);
            AddHeaderIfNotNull(content.Headers, Headers.ApiKey, _settings.ApiKey);
            AddHeaderIfNotNull(content.Headers, Headers.UserId, request.UserMetaData.UserId);
            AddHeaderIfNotNull(content.Headers, Headers.IpAddress, request.UserMetaData.IpAddress);
            AddHeaderIfNotNull(content.Headers, Headers.UserAgent, request.UserMetaData.UserAgent);
            AddHeaderIfNotNull(content.Headers, Headers.Referer, request.UserMetaData.Referer);
        }
        
        private void AddHeaderIfNotNull(HttpContentHeaders headers, string headerName, string headerValue)
        {
            if (headerValue != null)
                headers.Add(headerName, headerValue);
        }
        
        private Exception CreateEngineErrorException(byte[] responseData)
        {
            ErrorResponse errorResponse = Serializer.Deserialize<ErrorResponse>(responseData);
            return new EngineStatusCodeException(errorResponse.Error);
        }
    }
}
