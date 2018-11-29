using System;

namespace Loop54
{
    internal static class Utils
    {
        /// <summary>
        /// Trims and validates a URL
        /// </summary>
        internal static string FixEngineUrl(string url)
        {
            string ret = url.ToLower().Trim().Replace("\\", "/");

            if (!url.StartsWith("http://") && !url.StartsWith("https://"))
                throw new FormatException("Invalid url: Url must use protocol http or https.");
            
            return ret.TrimEnd('/');
        }

        internal static Exception CreateNullHttpContextException<T>(string property)
        {
            return new ClientInfoException($"The {typeof(T)} cannot be used when {property} is null. " +
                    "Either make sure {property} is not null or use another IRemoteClientInfoProvider, for instance Loop54.NullClientInfoProvider.");
        }

        /// <summary>
        /// Checks whether an URL is using HTTPS
        /// </summary>
        internal static bool UrlIsHttps(string url) => url.StartsWith("https://");

        /// <summary>
        /// Generates a new random UserId
        /// </summary>
        internal static string GenerateUserId() => Guid.NewGuid().ToString();
    }
}
