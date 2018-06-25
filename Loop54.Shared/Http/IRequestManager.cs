using Loop54.Model.Response;
using Loop54.Model.Request;
using System.Threading.Tasks;
using Loop54.User;

namespace Loop54.Http
{
    /// <summary>
    /// Handles the HTTP request to the Loop54 api.
    /// </summary>
    public interface IRequestManager
    {
        /// <summary>
        /// Calls the loop54 search engine and returns a deserialized response object. 
        /// </summary>
        /// <typeparam name="TResponse">The expected response type. E.g. if making a search call the expected type would be <see cref="SearchResponse"/></typeparam>
        /// <typeparam name="TRequest">The request type. E.g. if making a search call use <see cref="SearchRequest"/> </typeparam>
        /// <param name="action">The type of request. This will translate into the resource on the actual HTTP request i.e. "endpoint/action". Must not be null.</param>
        /// <param name="requestData">The query data to send to the engine. Must not be null.</param>
        /// <param name="metaData">Data about the requesting user. Must not be null.</param>
        /// <returns>The desired response</returns>
        Task<TResponse> CallEngineAsync<TRequest, TResponse>(string action, TRequest requestData, UserMetaData metaData) where TResponse : Response where TRequest : Request;
    }
}
