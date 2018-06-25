using Loop54.Model.Request;
using Loop54.User;

namespace Loop54
{
    /// <summary>
    /// Provides extension methods for the Request classes
    /// </summary>
    public static class RequestExtensions
    {
        /// <summary>
        /// Will wrap the request in a requestContainer to provide overriding options for the api call.
        /// </summary>
        /// <typeparam name="T">The type of the request data to add overrides to.</typeparam>
        /// <param name="requestData">The request data to wrap in a container. For instance a <see cref="SearchRequest"/>.</param>
        /// <param name="metaDataOverrides">Use these overrides to force certain values to take effect in the api call. For instance 
        /// if you set the <see cref="UserMetaData.UserId"/> it will trump any data from a <see cref="IRemoteClientInfo"/>! This 
        /// could be useful if you want to use an internal customer id of a logged in user.</param>
        /// <returns>A <see cref="RequestContainer{T}"/> wrapping the <see cref="Request" /></returns>
        public static RequestContainer<T> Wrap<T>(this T requestData, UserMetaData metaDataOverrides = null) where T : Request
        {
            return new RequestContainer<T>(requestData)
            {
                MetaDataOverrides = metaDataOverrides
            };
        }
    }
}
