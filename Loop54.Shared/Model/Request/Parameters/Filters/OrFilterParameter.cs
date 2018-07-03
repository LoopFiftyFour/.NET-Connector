using System.Collections.Generic;

namespace Loop54.Model.Request.Parameters.Filters
{
    /// <summary>
    /// Used for combining two or more filters using or logic
    /// </summary>
    public class OrFilterParameter : FilterParameter
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public OrFilterParameter()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="filters">Filters to combine using OR logic</param>
        public OrFilterParameter(params FilterParameter[] filters)
        {
            foreach (FilterParameter filter in filters)
                Or.Add(filter);
        }

        /// <summary>
        /// The filters that should be combined.
        /// </summary>
        public List<FilterParameter> Or { get; set; } = new List<FilterParameter>();
    }
}
