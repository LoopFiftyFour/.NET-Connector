using System.Collections.Generic;

namespace Loop54.Model.Request
{
    /// <summary>
    /// The base request class used for issuing custom requests to the Loop54 e-commerce search engine.
    /// </summary>
    public class Request
    {
        /// <summary>
        /// Any additional, non-standard, data. Contact support for information about how and when to use this.
        /// </summary>
        public IDictionary<string, object> CustomData { get; set; }
    }
}
