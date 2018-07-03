using System.Collections.Generic;

namespace Loop54.Model.Request.Parameters.Filters
{
    /// <summary>
    /// Used for combining two or more filters using AND-logic
    /// </summary>
    public class AndFilterParameter : FilterParameter
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public AndFilterParameter()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="filters">Filters to combine using AND logic</param>
        public AndFilterParameter(params FilterParameter[] filters)
        {
            foreach(FilterParameter filter in filters)
                And.Add(filter);
        }

        /// <summary>
        /// The filters that should be combined.
        /// </summary>
        public List<FilterParameter> And { get; set; } = new List<FilterParameter>();
    }
}
