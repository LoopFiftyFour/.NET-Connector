using System.Threading.Tasks;
using Loop54.Model.Request;
using Loop54.Model.Response;

namespace Loop54
{
    /// <summary>
    /// This is the main client interface for calling the Loop54 e-commerce search engine.
    /// </summary>
    public interface ILoop54Client
    {
        #region AutoComplete

        /// <summary>
        /// Make an autocomplete call to the engine
        /// </summary>
        /// <param name="request">Contains the request data to send to the engine.</param>
        /// <returns>The result of the autocomplete query</returns>
        AutoCompleteResponse AutoComplete(AutoCompleteRequest request);

        /// <summary>
        /// Make an asynchronous autocomplete call to the engine
        /// </summary>
        /// <param name="request">Contains the request data to send to the engine.</param>
        /// <returns>The result of the asynchronous autocomplete query</returns>
        Task<AutoCompleteResponse> AutoCompleteAsync(AutoCompleteRequest request);

        /// <summary>
        /// Make an autocomplete call to the engine
        /// </summary>
        /// <param name="request">Contains the request data to send to the engine. Wrapped in a RequestContainer with optional user data overrides.</param>
        /// <returns>The result of the autocomplete query</returns>
        AutoCompleteResponse AutoComplete(RequestContainer<AutoCompleteRequest> request);

        /// <summary>
        /// Make an asynchronous autocomplete call to the engine
        /// </summary>
        /// <param name="request">Contains the request data to send to the engine. Wrapped in a RequestContainer with optional user data overrides.</param>
        /// <returns>The result of the asynchronous autocomplete query</returns>
        Task<AutoCompleteResponse> AutoCompleteAsync(RequestContainer<AutoCompleteRequest> request);

        #endregion

        #region CreateEvents

        /// <summary>
        /// Make an createEvent call to the engine. Adding an interaction.
        /// </summary>
        /// <param name="request">Contains the request data to send to the engine.</param>
        /// <returns>Empty default response. Could contain custom data if that is set up for you by support.</returns>
        Response CreateEvents(CreateEventsRequest request);

        /// <summary>
        /// Make an asynchronous createEvent call to the engine. Adding an interaction.
        /// </summary>
        /// <param name="request">Contains the request data to send to the engine.</param>
        /// <returns>Async task of the request.</returns>
        Task<Response> CreateEventsAsync(CreateEventsRequest request);

        /// <summary>
        /// Make an createEvent call to the engine. Adding an interaction.
        /// </summary>
        /// <param name="request">Contains the request data to send to the engine. Wrapped in a RequestContainer with optional user data overrides.</param>
        /// <returns>Empty default response. Could contain custom data if that is set up for you by support.</returns>
        Response CreateEvents(RequestContainer<CreateEventsRequest> request);

        /// <summary>
        /// Make an asynchronous createEvent call to the engine. Adding an interaction.
        /// </summary>
        /// <param name="request">Contains the request data to send to the engine. Wrapped in a RequestContainer with optional user data overrides.</param>
        /// <returns>Async task of the request.</returns>
        Task<Response> CreateEventsAsync(RequestContainer<CreateEventsRequest> request);

        #endregion

        #region GetEntities

        /// <summary>
        /// Make a getEntities call to the engine. Will return all entities matching the query provided.
        /// </summary>
        /// <param name="request">Contains the request data to send to the engine.</param>
        /// <returns>The result of the get entities query</returns>
        GetEntitiesResponse GetEntities(GetEntitiesRequest request);

        /// <summary>
        /// Make an asynchronous getEntities call to the engine. Will return all entities matching the query provided.
        /// </summary>
        /// <param name="request">Contains the request data to send to the engine.</param>
        /// <returns>The result of the get entities query</returns>
        Task<GetEntitiesResponse> GetEntitiesAsync(GetEntitiesRequest request);

        /// <summary>
        /// Make a getEntities call to the engine. Will return all entities matching the query provided.
        /// </summary>
        /// <param name="request">Contains the request data to send to the engine. Wrapped in a RequestContainer with optional user data overrides.</param>
        /// <returns>The result of the get entities query</returns>
        GetEntitiesResponse GetEntities(RequestContainer<GetEntitiesRequest> request);

        /// <summary>
        /// Make an asynchronous getEntities call to the engine. Will return all entities matching the query provided.
        /// </summary>
        /// <param name="request">Contains the request data to send to the engine. Wrapped in a RequestContainer with optional user data overrides.</param>
        /// <returns>The result of the get entities query</returns>
        Task<GetEntitiesResponse> GetEntitiesAsync(RequestContainer<GetEntitiesRequest> request);

        #endregion

        #region GetEntitiesByAttribute

        /// <summary>
        /// Make a getEntitiesByAttribute call to the engine. Will return entities that have a give attribute value. Note that this attribute need to be indexed by Loop54 explicitly.
        /// </summary>
        /// <param name="request">Contains the request data to send to the engine.</param>
        /// <returns>The result of the get entities query</returns>
        GetEntitiesByAttributeResponse GetEntitiesByAttribute(GetEntitiesByAttributeRequest request);

        /// <summary>
        /// Make an asynchronous getEntitiesByAttribute call to the engine. Will return entities that have a give attribute value. Note that this attribute need to be indexed by Loop54 explicitly.
        /// </summary>
        /// <param name="request">Contains the request data to send to the engine.</param>
        /// <returns>The result of the get entities query</returns>
        Task<GetEntitiesByAttributeResponse> GetEntitiesByAttributeAsync(GetEntitiesByAttributeRequest request);

        /// <summary>
        /// Make a getEntitiesByAttribute call to the engine. Will return entities that have a give attribute value. Note that this attribute need to be indexed by Loop54 explicitly.
        /// </summary>
        /// <param name="request">Contains the request data to send to the engine. Wrapped in a RequestContainer for user data overrides.</param>
        /// <returns>The result of the get entities query</returns>
        GetEntitiesByAttributeResponse GetEntitiesByAttribute(RequestContainer<GetEntitiesByAttributeRequest> request);

        /// <summary>
        /// Make an asynchronous getEntitiesByAttribute call to the engine. Will return entities that have a give attribute value. Note that this attribute need to be indexed by Loop54 explicitly.
        /// </summary>
        /// <param name="request">Contains the request data to send to the engine. Wrapped in a RequestContainer for user data overrides.</param>
        /// <returns>The result of the get entities query</returns>
        Task<GetEntitiesByAttributeResponse> GetEntitiesByAttributeAsync(RequestContainer<GetEntitiesByAttributeRequest> request);

        #endregion

        #region GetRelatedEntities

        /// <summary>
        /// Make a getRelatedEntities call to the engine. Will return entities that are contextually close to the given product.
        /// </summary>
        /// <param name="request">Contains the request data to send to the engine.</param>
        /// <returns>The related entities</returns>
        GetRelatedEntitiesResponse GetRelatedEntities(GetRelatedEntitiesRequest request);

        /// <summary>
        /// Make an asynchronous getRelatedEntities call to the engine. Will return entities that are contextually close to the given product.
        /// </summary>
        /// <param name="request">Contains the request data to send to the engine.</param>
        /// <returns>The related entities</returns>
        Task<GetRelatedEntitiesResponse> GetRelatedEntitiesAsync(GetRelatedEntitiesRequest request);

        /// <summary>
        /// Make a getRelatedEntities call to the engine. Will return entities that are contextually close to the given product.
        /// </summary>
        /// <param name="request">Contains the request data to send to the engine. Wrapped in a RequestContainer for user data overrides.</param>
        /// <returns>The related entities</returns>
        GetRelatedEntitiesResponse GetRelatedEntities(RequestContainer<GetRelatedEntitiesRequest> request);

        /// <summary>
        /// Make an asynchronous getRelatedEntities call to the engine. Will return entities that are contextually close to the given product.
        /// </summary>
        /// <param name="request">Contains the request data to send to the engine. Wrapped in a RequestContainer for user data overrides.</param>
        /// <returns>The related entities</returns>
        Task<GetRelatedEntitiesResponse> GetRelatedEntitiesAsync(RequestContainer<GetRelatedEntitiesRequest> request);

        #endregion
        
        #region GetComplementaryEntities

        /// <summary>
        /// Make a getComplementaryEntities call to the engine. Will return entities that are contextually close to the given product.
        /// </summary>
        /// <param name="request">Contains the request data to send to the engine.</param>
        /// <returns>The complementary entities</returns>
        GetComplementaryEntitiesResponse GetComplementaryEntities(GetComplementaryEntitiesRequest request);

        /// <summary>
        /// Make an asynchronous getComplementaryEntities call to the engine. Will return entities that are contextually close to the given product.
        /// </summary>
        /// <param name="request">Contains the request data to send to the engine.</param>
        /// <returns>The complementary entities</returns>
        Task<GetComplementaryEntitiesResponse> GetComplementaryEntitiesAsync(GetComplementaryEntitiesRequest request);

        /// <summary>
        /// Make a getComplementaryEntities call to the engine. Will return entities that are contextually close to the given product.
        /// </summary>
        /// <param name="request">Contains the request data to send to the engine. Wrapped in a RequestContainer for user data overrides.</param>
        /// <returns>The complementary entities</returns>
        GetComplementaryEntitiesResponse GetComplementaryEntities(RequestContainer<GetComplementaryEntitiesRequest> request);

        /// <summary>
        /// Make an asynchronous getComplementaryEntities call to the engine. Will return entities that are contextually close to the given product.
        /// </summary>
        /// <param name="request">Contains the request data to send to the engine. Wrapped in a RequestContainer for user data overrides.</param>
        /// <returns>The complementary entities</returns>
        Task<GetComplementaryEntitiesResponse> GetComplementaryEntitiesAsync(RequestContainer<GetComplementaryEntitiesRequest> request);

        #endregion

        #region GetBasketRecommendations

        /// <summary>
        /// Make a getBasketRecommendations call to the engine.
        /// </summary>
        /// <param name="request">Contains the request data to send to the engine.</param>
        /// <returns>The recommended entities</returns>
        GetBasketRecommendationsResponse GetBasketRecommendations(GetBasketRecommendationsRequest request);

        /// <summary>
        /// Make an asynchronous getBasketRecommendations call to the engine.
        /// </summary>
        /// <param name="request">Contains the request data to send to the engine.</param>
        /// <returns>The recommended entities</returns>
        Task<GetBasketRecommendationsResponse> GetBasketRecommendationsAsync(GetBasketRecommendationsRequest request);

        /// <summary>
        /// Make a getBasketRecommendations call to the engine.
        /// </summary>
        /// <param name="request">Contains the request data to send to the engine. Wrapped in a RequestContainer for user data overrides.</param>
        /// <returns>The recommended entities</returns>
        GetBasketRecommendationsResponse GetBasketRecommendations(RequestContainer<GetBasketRecommendationsRequest> request);

        /// <summary>
        /// Make an asynchronous getBasketRecommendations call to the engine.
        /// </summary>
        /// <param name="request">Contains the request data to send to the engine. Wrapped in a RequestContainer for user data overrides.</param>
        /// <returns>The recommended entities</returns>
        Task<GetBasketRecommendationsResponse> GetBasketRecommendationsAsync(RequestContainer<GetBasketRecommendationsRequest> request);

        #endregion

        #region Search

        /// <summary>
        /// Make a search call to the Loop54 e-commerce search engine. Will return entities and other search data based on the supplied query.
        /// </summary>
        /// <param name="request">Contains the request data to send to the engine.</param>
        /// <returns>The direct results of the query and products related to the result.</returns>
        SearchResponse Search(SearchRequest request);

        /// <summary>
        /// Make an asynchronous search call to the Loop54 e-commerce search engine. Will return entities and other search data based on the supplied query.
        /// </summary>
        /// <param name="request">Contains the request data to send to the engine.</param>
        /// <returns>The direct results of the query and products related to the result.</returns>
        Task<SearchResponse> SearchAsync(SearchRequest request);

        /// <summary>
        /// Make a search call to the Loop54 e-commerce search engine. Will return entities and other search data based on the supplied query.
        /// </summary>
        /// <param name="request">Contains the request data to send to the engine. Wrapped in a RequestContainer with optional user data overrides.</param>
        /// <returns>The direct results of the query and products related to the result.</returns>
        SearchResponse Search(RequestContainer<SearchRequest> request);

        /// <summary>
        /// Make an asynchronous search call to the Loop54 e-commerce search engine. Will return entities and other search data based on the supplied query.
        /// </summary>
        /// <param name="request">Contains the request data to send to the engine. Wrapped in a RequestContainer with optional user data overrides.</param>
        /// <returns>The direct results of the query and products related to the result.</returns>
        Task<SearchResponse> SearchAsync(RequestContainer<SearchRequest> request);

        #endregion

        #region Sync

        /// <summary>
        /// Make a sync call to the engine, telling it to re-sync the catalog.
        /// </summary>
        /// <param name="request">Contains the request data to send to the engine.</param>
        /// <returns>The results of a sync request.</returns>
        Response Sync(Request request = null);

        /// <summary>
        /// Make an asynchronous sync call to the engine, telling it to re-sync the catalog.
        /// </summary>
        /// <param name="request">Contains the request data to send to the engine.</param>
        /// <returns>The results of a sync request.</returns>
        Task<Response> SyncAsync(Request request = null);

        /// <summary>
        /// Make a sync call to the engine, telling it to re-sync the catalog.
        /// </summary>
        /// <param name="request">Contains the request data to send to the engine. Wrapped in a RequestContainer for user data overrides.</param>
        /// <returns>The results of a sync request.</returns>
        Response Sync(RequestContainer<Request> request);

        /// <summary>
        /// Make an asynchronous sync call to the engine, telling it to re-sync the catalog.
        /// </summary>
        /// <param name="request">Contains the request data to send to the engine. Wrapped in a RequestContainer for user data overrides.</param>
        /// <returns>The results of a sync request.</returns>
        Task<Response> SyncAsync(RequestContainer<Request> request);

        #endregion

        #region CustomCall

        /// <summary>
        /// Make a custom api call to the Loop54 e-commerce search engine. Contact customer support to get more information.
        /// </summary>
        /// <param name="name">Name of the api endpoint. Must be lower camel-case.</param>
        /// <param name="request">The request data.</param>
        /// <returns>A base response object with custom data.</returns>
        Response CustomCall(string name, Request request);

        /// <summary>
        /// Make an asynchronous custom api call to the Loop54 e-commerce search engine. Contact customer support to get more information.
        /// </summary>
        /// <param name="name">Name of the api endpoint. Must be lower camel-case.</param>
        /// <param name="request">The request data.</param>
        /// <returns>A base response object with custom data.</returns>
        Task<Response> CustomCallAsync(string name, Request request);

        /// <summary>
        /// Make a custom api call to the Loop54 e-commerce search engine. Contact customer to get more information.
        /// </summary>
        /// <param name="name">Name of the api endpoint. Must be lower camel-case.</param>
        /// <param name="request">The request data wrapped in a RequestContainer with optional user data overrides.</param>
        /// <returns>A base response object with custom data.</returns>
        Response CustomCall(string name, RequestContainer<Request> request);

        /// <summary>
        /// Make an asynchronous custom api call to the Loop54 e-commerce search engine. Contact customer to get more information.
        /// </summary>
        /// <param name="name">Name of the api endpoint. Must be lower camel-case.</param>
        /// <param name="request">The request data wrapped in a RequestContainer with optional user data overrides.</param>
        /// <returns>A base response object with custom data.</returns>
        Task<Response> CustomCallAsync(string name, RequestContainer<Request> request);

        #endregion
    }
}
