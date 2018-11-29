using Loop54.User;
using System;
using System.Collections.Generic;

namespace Loop54
{
    /// <summary>
    /// A client info that returns null values for all user-specific metadata such as Id, Referrer, etc. 
    /// Useful for implementing engine requests where there is no context, such as batch jobs.
    /// </summary>
    public class NullClientInfo : IRemoteClientInfo
    {
        public Dictionary<string, string> Headers { get; set; } = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        public string GetRequestHeader(string name)
        {
            if (Headers.TryGetValue(name, out string value))
                return value;

            return null;
        }

        private Dictionary<string, string> _cookies = new Dictionary<string, string>();

        public string GetCookie(string name)
        {
            if (_cookies.TryGetValue(name, out string value))
                return value;

            return null;
        }

        public void SetCookie(string name, string value, DateTime expiryTime)
        {
            _cookies[name] = value;
        }

        public string Referrer { get; set; } = null;

        public string GetReferrer() => Referrer;

        public string UserAgent { get; set; } = null;

        public string GetUserAgent() => UserAgent;

        public string RemoteIp { get; set; } = null;

        public string GetRemoteIp() => RemoteIp;
    }
}
