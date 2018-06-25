using System.Collections.Generic;

namespace Loop54.Model.Response
{
    /// <summary>
    /// A response from the engine. Used for responses that don't return any standardized data parameters.
    /// </summary>
    public class Response
    {
        /// <summary>
        /// Any additional, non-standard, data. Contact support for information about how and when to use this.
        /// </summary>
        public IDictionary<string, object> CustomData { get; set; }
    }
}
