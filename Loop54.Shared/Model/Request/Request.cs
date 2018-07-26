using System;
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
        /// <remarks>If setting the dictionary manually, make sure it's case-sensitive.</remarks>
        public IDictionary<string, object> CustomData { get; set; }

        /// <summary>
        /// Adds the object value using the provided key to the custom data of the request.
        /// </summary>
        /// <param name="key">Key to set the data on. The keys will be treated as case-sensitive.</param>
        /// <param name="value">Value to add to the custom data.</param>
        public void AddCustomData(string key, object value)
        {
            if (CustomData == null)
                CustomData = new Dictionary<string, object>(StringComparer.Ordinal);

            CustomData[key] = value;
        }
    }
}
