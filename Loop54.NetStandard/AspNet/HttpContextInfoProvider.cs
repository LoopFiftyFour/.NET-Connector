

using Loop54.User;
using Microsoft.AspNetCore.Http;

namespace Loop54.AspNet
{
    /// <summary>
    /// Provides <see cref="IRemoteClientInfo"/> based on the current <see cref="HttpContext"/> found in the <see cref="IHttpContextAccessor"/>.
    /// </summary>
    public class HttpContextInfoProvider : IRemoteClientInfoProvider
    {
        private readonly IHttpContextAccessor _contextAccessor;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="contextAccessor">IHttpContextAccessor that can return the current <see cref="HttpContext"/></param>
        public HttpContextInfoProvider(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        /// <summary>
        /// Returns a <see cref="IRemoteClientInfo"/> based on the current <see cref="HttpContext"/>
        /// </summary>
        /// <returns>A <see cref="IRemoteClientInfo"/> based on the current <see cref="HttpContext"/></returns>
        public IRemoteClientInfo GetRemoteClientInfo()
        {
            if (_contextAccessor.HttpContext == null)
                throw Utils.CreateNullHttpContextException<HttpContextInfoProvider>("IHttpContextAccessor.HttpContext");

            return new HttpContextInfo(_contextAccessor.HttpContext);
        }
    }
}