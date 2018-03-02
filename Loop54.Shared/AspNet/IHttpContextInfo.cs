using System;

namespace Loop54.AspNet
{
    internal interface IHttpContextInfo
    {
        string GetRequestUrl();
        string GetRequestHeader(string name);
        string GetCookie(string name);
        void SetCookie(string name, string value, DateTime expiryTime);
        string GetReferrer();
        string GetUserAgent();
        string GetRemoteIp();
    }
}
