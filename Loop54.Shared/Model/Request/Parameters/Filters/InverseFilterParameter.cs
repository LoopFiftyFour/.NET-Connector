namespace Loop54.Model.Request.Parameters.Filters
{
    /// <summary>
    /// Used for inverting a filter.
    /// </summary>
    public class InverseFilterParameter : FilterParameter
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="not">The filter to inverse. Meaning if 
        /// that filter results in false the inverse will be true.</param>
        public InverseFilterParameter(FilterParameter not)
        {
            Not = not;
        }

        /// <summary>
        /// The filter that should be inversed.
        /// </summary>
        public FilterParameter Not { get; set; }
    }
}
