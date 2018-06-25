using System;
using System.Collections.Generic;
using System.Text;

namespace Loop54
{
    /// <summary>
    /// Global settings for the Loop54 e-commerce search engine setup
    /// </summary>
    public class Loop54Settings
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="endpoint">The endpoint of the Loop54 search engine. If you don't have this please contact customer support.</param>
        public Loop54Settings(string endpoint)
        {
            Endpoint = endpoint ?? throw new ArgumentNullException(nameof(endpoint));
        }

        /// <summary>
        /// The endpoint of the Loop54 search engine. If you don't have this please contact customer support.
        /// </summary>
        public string Endpoint { get; set; }

        /// <summary>
        /// The api key authenticating you as a trusted caller. If you don't have this please contact customer support.
        /// </summary>
        public string ApiKey { get; set; } = null;

        /// <summary>
        /// Whether to enforce the use of HTTPS. If this is true the endpoint must use the HTTPS protocol
        /// </summary>
        public bool RequireHttps { get; set; } = true;

        /// <summary>
        /// How long to wait, in milliseconds, for the Loop54 search engine before failing.
        /// </summary>
        public int RequestTimeoutMs { get; set; } = 10000;
    }
}
