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
