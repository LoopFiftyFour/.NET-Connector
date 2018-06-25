using System.Collections.Generic;

namespace Loop54.Model.Request.Parameters.Filters
{
    /// <summary>
    /// Used for combining two or more filters using AND-logic
    /// </summary>
    public class AndFilterParameter : FilterParameter
    {
        /// <summary>
        /// The filters that should be combined.
        /// </summary>
        public List<FilterParameter> And { get; set; } = new List<FilterParameter>();
    }
}
