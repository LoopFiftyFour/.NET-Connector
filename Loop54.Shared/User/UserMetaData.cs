using Loop54.AspNet;
using System;
using System.Collections.Generic;
using System.Text;

namespace Loop54.User
{
    /// <summary>
    /// Represents data about the end-user. Used by the search engine to personalize the shopping experience.
    /// Use this class to override the default behaviour of the library. For instance if you want a custom 
    /// UserId based on a logged in user.
    /// </summary>
    public class UserMetaData
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public UserMetaData()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="userId">The unique id of the user. Could be used for overriding the default, random, cookie-stored identifier.</param>
        public UserMetaData(string userId)
        {
            UserId = userId ?? throw new ArgumentNullException(nameof(userId));
        }

        /// <summary>
        /// An unique identifier of the end-user. DO NOT set this to a placeholder string. If it's not set it'll be automatically 
        /// set to a random GUID when making the request and a cookie will be stored to the IRemoteClientInfo provided in the 
        /// request so that we may identify the user later.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Ip address of the end-user. If not set it will later be set to the IP address provided by the IRemoteClientInfo.
        /// </summary>
        public string IpAddress { get; set; }

        /// <summary>
        /// User-Agent header from the end-user. If not set it will later be set to the User-Agent provided by the IRemoteClientInfo.
        /// </summary>
        public string UserAgent { get; set; }

        /// <summary>
        /// The Referer(sic) header from the end-user. If not set it will later be set to the Referer provided by the IRemoteClientInfo.
        /// </summary>
        public string Referer { get; set; }

        internal void SetFromClientInfo(IRemoteClientInfo clientInfo)
        {
            if (string.IsNullOrEmpty(UserId))
                UserId = GetOrAddUserIdCookie(clientInfo);

            AssertUserIdNotNull();

            if (string.IsNullOrEmpty(IpAddress))
                IpAddress = GetRealIp(clientInfo);

            if (string.IsNullOrEmpty(UserAgent))
                UserAgent = clientInfo.GetUserAgent();

            if (string.IsNullOrEmpty(Referer))
                Referer = clientInfo.GetReferrer();
        }

        private void AssertUserIdNotNull()
        {
            if (string.IsNullOrEmpty(UserId))
                throw new ClientInfoException($"UserId is null or empty. Make sure you've implemented the {nameof(IRemoteClientInfo)} " +
                    $"interface properly so that the cookie setting and getting works.");
        }

        private string GetOrAddUserIdCookie(IRemoteClientInfo cookieAccessor)
        {
            string cookieValue = cookieAccessor.GetCookie(UserIdCookieKey);

            if (string.IsNullOrEmpty(cookieValue))
            {
                string newUserId = Utils.GenerateUserId();
                cookieAccessor.SetCookie(UserIdCookieKey, newUserId, DateTime.Now.AddYears(1));
                cookieValue = newUserId;
            }

            return cookieValue;
        }

        internal const string UserIdCookieKey = "Loop54User";
        internal const string ProxyIpHeaderName = "X-Forwarded-For";

        private string GetRealIp(IRemoteClientInfo clientInfo)
        {
            //behind proxy?
            var forwarded = clientInfo.GetRequestHeader("X-Forwarded-For");

            if (!string.IsNullOrEmpty(forwarded))
                return forwarded;

            return clientInfo.GetRemoteIp();
        }
    }
}
