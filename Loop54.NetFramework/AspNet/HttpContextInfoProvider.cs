using Loop54.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            return new HttpContextInfo(HttpContext.Current);
        }
    }
}
