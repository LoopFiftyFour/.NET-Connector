using System.Web;
using Loop54.AspNet;
using Loop54.Utils;

namespace Loop54
{
    public partial class Request
    {
        /// <summary>
        /// Create a new request using HttpContext.Current, if any.
        /// </summary>
        /// <param name="requestName">The name of the request. Examples: "Search", "AutoComplete", "CreateEvents".</param>
        public Request(string requestName)
            : this(requestName, HttpContext.Current, null)
        {
        }

        /// <summary>
        /// Create a new request using HttpContext.Current, if any.
        /// </summary>
        /// <param name="requestName">The name of the request. Examples: "Search", "AutoComplete", "CreateEvents".</param>
        /// <param name="options">Optional request options for compatibility with older engines and other settings. Set to null to ignore.</param>
        public Request(string requestName, RequestOptions options)
            : this(requestName, HttpContext.Current, options)
        {
        }

        /// <summary>
        /// Create a new request using the specified HttpContext, if any.
        /// </summary>
        /// <param name="requestName">The name of the request. Examples: "Search", "AutoComplete", "CreateEvents".</param>
        /// <param name="httpContext">The HttpContext of the current ASP.NET request, if any. May be null.</param>
        /// <param name="options">Optional request options for compatibility with older engines and other settings. Set to null to ignore.</param>
        public Request(string requestName, HttpContext httpContext, RequestOptions options)
        {
            QuestName = requestName;
            Options = options ?? new RequestOptions();
            // Allow HttpContext to explicitly set to null, so that the library can be used outside of ASP.NET.
            // _contextInfo is still set in this case, so the rest of the class doesn't have to check it for null every time.
            _http = new Http(httpContext == null ? (IHttpContextInfo)new NullContextInfo() : new HttpContextInfo(httpContext));
        }
    }
}
