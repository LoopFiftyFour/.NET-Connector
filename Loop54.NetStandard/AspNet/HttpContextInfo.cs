using System;
using Loop54.User;
using Microsoft.AspNetCore.Http;

namespace Loop54.AspNet
{
    /// <summary>
    /// Wrapper around a <see cref="HttpContext"/> that supplies the Loop54 library with client info.
    /// </summary>
    public class HttpContextInfo : IRemoteClientInfo
    {
        private readonly HttpContext _context;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context">The current HttpContext</param>
        public HttpContextInfo(HttpContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        
        public string GetRequestHeader(string name)
        {
            return _context.Request.Headers[name];
        }

        public string GetCookie(string name)
        {
            return _context.Request.Cookies[name];
        }

        public void SetCookie(string name, string value, DateTime expiryTime)
        {
            _context.Response.Cookies.Append(name, value, new CookieOptions { Expires = new DateTimeOffset(expiryTime) });
        }

        public string GetReferrer()
        {
            return _context.Request.Headers["Referer"];
        }

        public string GetUserAgent()
        {
            return _context.Request.Headers["User-Agent"];
        }

        public string GetRemoteIp()
        {
            return _context.Connection.RemoteIpAddress?.ToString();
        }
    }
}
