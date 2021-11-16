using System;

namespace Loop54.User
{
    /// <summary>
    /// This interface contains methods used to get information about the end-user client request. If not running Asp.Net or Asp.Net Core this needs to be implemented by the user of the Loop54 library.
    /// </summary>
    public interface IRemoteClientInfo
    {
        /// <summary>
        /// Get a named header from the request made by the end-user.
        /// </summary>
        /// <returns>If the header isn't set return null, otherwise the string value of the header.</returns>
        string GetRequestHeader(string name);

        /// <summary>
        /// Get the Referer(sic) header sent by the end-user. 
        /// </summary>
        /// <returns>Referer header or null if it doesn't exist.</returns>
        string GetReferrer();

        /// <summary>
        /// Get the User-Agent header sent by the end-user.
        /// </summary>
        /// <returns>User-Agent header or null if it doesn't exist.</returns>
        string GetUserAgent();

        /// <summary>
        /// Get the IP address of the end-user.
        /// </summary>
        /// <returns>IP address or null if it doesn't exist.</returns>
        string GetRemoteIp();

        /// <summary>
        /// Gets the value of a cookie with a given name. 
        /// This needs to be able return cookies set with <see cref="SetCookie(string, string, DateTime)"/>.
        /// </summary>
        /// <returns>Return the cookie data or null if it doesn't exist.</returns>
        string GetCookie(string name);

        /// <summary>
        /// Sets a cookie with a given name, value and expiryTime. Cookies set by this method needs to be accessible with <see cref="GetCookie(string)"/>.
        /// </summary>
        void SetCookie(string name, string value, DateTime expiryTime);
    }
}
