using Loop54.User;
using System;
using System.Web;

namespace Loop54.AspNet
{
    /// <summary>
    /// Wrapper around a <see cref="HttpContext"/> that supplies the Loop54 library with client info.
    /// </summary>
    public class HttpContextInfo : IRemoteClientInfo
    {
        private readonly HttpContext _context;

        public HttpContextInfo(HttpContext context)
        {
            _context = context ?? throw new ArgumentNullException("context");
        }

        public string GetRequestUrl()
        {
            return _context.Request.Url.AbsoluteUri;
        }

        public string GetRequestHeader(string name)
        {
            return _context.Request.Headers[name];
        }

        public string GetCookie(string name)
        {
            var cookie = _context.Request.Cookies[name];
            return cookie == null ? null : cookie.Value;
        }

        public void SetCookie(string name, string value, DateTime expiryTime)
        {
            var newCookie = new HttpCookie(name);
            newCookie.Value = value;
            newCookie.Expires = expiryTime;

            _context.Response.Cookies.Add(newCookie);
        }

        public string GetReferrer()
        {
            return _context.Request.ServerVariables["HTTP_REFERER"];
        }

        public string GetUserAgent()
        {
            return _context.Request.ServerVariables["HTTP_USER_AGENT"];
        }

        public string GetRemoteIp()
        {
            return _context.Request.ServerVariables["REMOTE_ADDR"];
        }
    }
}
