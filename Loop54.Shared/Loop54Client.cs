using Loop54.AspNet;
using Loop54.Http;
using Loop54.Model.Request;
using Loop54.Model.Response;
using Loop54.User;
using System;
using System.Threading.Tasks;

namespace Loop54
{
    /// <summary>
    /// Default implementation of the ILoop54Client interface.
    /// 
    /// When issuing a request the user data will be used in this order of priority:
    /// 1. The <see cref="UserMetaData"/> provided in <see cref="RequestContainer{T}"/> in the method call.
    /// 2. The <see cref="IRemoteClientInfo"/> gotten from the <see cref="IRemoteClientInfoProvider"/>.
    /// </summary>
    public class Loop54Client : ILoop54Client
    {
        private readonly IRequestManager _requestManager;
        private readonly IRemoteClientInfoProvider _remoteClientInfoProvider;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="requestManager">The <see cref="IRequestManager"/> to use when making calls to the api. Must not be null.</param>
        /// <param name="remoteClientInfoProvider">The <see cref="IRemoteClientInfoProvider"/></param>
        public Loop54Client(IRequestManager requestManager, IRemoteClientInfoProvider remoteClientInfoProvider)
        {
            _requestManager = requestManager ?? throw new ArgumentNullException(nameof(requestManager));
            _remoteClientInfoProvider = remoteClientInfoProvider ?? throw new ArgumentNullException(nameof(remoteClientInfoProvider));
        }

        public SearchResponse Search(SearchRequest request) => Search(request.Wrap());
        public SearchResponse Search(RequestContainer<SearchRequest> request) => UnwrapAggregateException(() => SearchAsync(request).Result);
        public async Task<SearchResponse> SearchAsync(SearchRequest request) => await SearchAsync(request.Wrap());
        public async Task<SearchResponse> SearchAsync(RequestContainer<SearchRequest> request) => await CallEngineWithinClientContext<SearchRequest, SearchResponse>("search", request);

        public AutoCompleteResponse AutoComplete(AutoCompleteRequest request) => AutoComplete(request.Wrap());
        public AutoCompleteResponse AutoComplete(RequestContainer<AutoCompleteRequest> request) => UnwrapAggregateException(() => AutoCompleteAsync(request).Result);
        public async Task<AutoCompleteResponse> AutoCompleteAsync(AutoCompleteRequest request) => await AutoCompleteAsync(request.Wrap());
        public async Task<AutoCompleteResponse> AutoCompleteAsync(RequestContainer<AutoCompleteRequest> request) => await CallEngineWithinClientContext<AutoCompleteRequest, AutoCompleteResponse>("autoComplete", request);

        public GetEntitiesResponse GetEntities(GetEntitiesRequest request) => GetEntities(request.Wrap());
        public GetEntitiesResponse GetEntities(RequestContainer<GetEntitiesRequest> request) => UnwrapAggregateException(() => GetEntitiesAsync(request).Result);
        public async Task<GetEntitiesResponse> GetEntitiesAsync(GetEntitiesRequest request) => await GetEntitiesAsync(request.Wrap());
        public async Task<GetEntitiesResponse> GetEntitiesAsync(RequestContainer<GetEntitiesRequest> request) => await CallEngineWithinClientContext<GetEntitiesRequest, GetEntitiesResponse>("getEntities", request);

        public GetEntitiesByAttributeResponse GetEntitiesByAttribute(GetEntitiesByAttributeRequest request) => GetEntitiesByAttribute(request.Wrap());
        public GetEntitiesByAttributeResponse GetEntitiesByAttribute(RequestContainer<GetEntitiesByAttributeRequest> request) => UnwrapAggregateException(() => GetEntitiesByAttributeAsync(request).Result);
        public async Task<GetEntitiesByAttributeResponse> GetEntitiesByAttributeAsync(GetEntitiesByAttributeRequest request) => await GetEntitiesByAttributeAsync(request.Wrap());
        public async Task<GetEntitiesByAttributeResponse> GetEntitiesByAttributeAsync(RequestContainer<GetEntitiesByAttributeRequest> request) => await CallEngineWithinClientContext<GetEntitiesByAttributeRequest, GetEntitiesByAttributeResponse>("getEntitiesByAttribute", request);

        public GetRelatedEntitiesResponse GetRelatedEntities(GetRelatedEntitiesRequest request) => GetRelatedEntities(request.Wrap());
        public GetRelatedEntitiesResponse GetRelatedEntities(RequestContainer<GetRelatedEntitiesRequest> request) => UnwrapAggregateException(() => GetRelatedEntitiesAsync(request).Result);
        public async Task<GetRelatedEntitiesResponse> GetRelatedEntitiesAsync(GetRelatedEntitiesRequest request) => await GetRelatedEntitiesAsync(request.Wrap());
        public async Task<GetRelatedEntitiesResponse> GetRelatedEntitiesAsync(RequestContainer<GetRelatedEntitiesRequest> request) => await CallEngineWithinClientContext<GetRelatedEntitiesRequest, GetRelatedEntitiesResponse>("getRelatedEntities", request);

        public Response CreateEvents(CreateEventsRequest request) => CreateEvents(request.Wrap());
        public Response CreateEvents(RequestContainer<CreateEventsRequest> request) => UnwrapAggregateException(() => CreateEventsAsync(request).Result);
        public async Task<Response> CreateEventsAsync(CreateEventsRequest request) => await CreateEventsAsync(request.Wrap());
        public async Task<Response> CreateEventsAsync(RequestContainer<CreateEventsRequest> request) => await CallEngineWithinClientContext<CreateEventsRequest, Response>("createEvents", request);

        public Response CustomCall(string name, Request request) => CustomCall(name, request.Wrap());
        public Response CustomCall(string name, RequestContainer<Request> request) => UnwrapAggregateException(() => CustomCallAsync(name, request).Result);
        public async Task<Response> CustomCallAsync(string name, Request request) => await CustomCallAsync(name, request.Wrap());
        public async Task<Response> CustomCallAsync(string name, RequestContainer<Request> request) => await CallEngineWithinClientContext<Request, Response>(name, request);

        private async Task<TResponse> CallEngineWithinClientContext<TRequest, TResponse>(string requestName, RequestContainer<TRequest> request) where TResponse : Response where TRequest : Request
        {
            if (requestName == null)
                throw new ArgumentNullException(nameof(requestName));

            if (requestName.Length == 0 || char.IsUpper(requestName[0]))
                throw new ArgumentException($"The {nameof(requestName)} must be in lower camel case with a length over 0.", nameof(requestName));

            UserMetaData metaData = GetOrCreateMetaData(request);

            return await _requestManager.CallEngineAsync<TRequest, TResponse>(requestName, request.Request, metaData);
        }

        private UserMetaData GetOrCreateMetaData<TRequest>(RequestContainer<TRequest> request) where TRequest : Request
        {
            UserMetaData metaData = request.MetaDataOverrides ?? new UserMetaData();
            
            IRemoteClientInfo remoteClientInfo = _remoteClientInfoProvider.GetRemoteClientInfo();

            if (remoteClientInfo == null)
                throw new ClientInfoException($"The {nameof(IRemoteClientInfoProvider)} returned a null {nameof(IRemoteClientInfo)}. Make sure you've implemented it correctly!");
            
            metaData.SetFromClientInfo(remoteClientInfo);
            return metaData;
        }

        private T UnwrapAggregateException<T>(Func<T> throwingFunc)
        {
            try
            {
                return throwingFunc();
            }
            catch(AggregateException ae)
            {
                throw ae.InnerException;
            }
        }
    }
}
