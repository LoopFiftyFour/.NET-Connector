using Loop54.User;
using System.Web;

namespace Loop54.AspNet
{
    /// <summary>
    /// This provider uses <see cref="HttpContext.Current"/> to create the <see cref="IRemoteClientInfo"/>
    /// </summary>
    public class HttpContextInfoProvider : IRemoteClientInfoProvider
    {
        public IRemoteClientInfo GetRemoteClientInfo()
        {
            if (HttpContext.Current == null)
                throw Utils.CreateNullHttpContextException<HttpContextInfoProvider>("HttpContext.Current");

            return new HttpContextInfo(HttpContext.Current);
        }
    }
}