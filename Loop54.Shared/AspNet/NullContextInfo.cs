using System;

namespace Loop54.AspNet
{
    internal class NullContextInfo : IHttpContextInfo
    {
        public string GetRequestUrl()
        {
            return null;
        }

        public string GetRequestHeader(string name)
        {
            return null;
        }

        public string GetCookie(string name)
        {
            return null;
        }

        public void SetCookie(string name, string value, DateTime expiryTime)
        {
        }

        public string GetReferrer()
        {
            return null;
        }

        public string GetUserAgent()
        {
            return null;
        }

        public string GetRemoteIp()
        {
            return null;
        }
    }
}
