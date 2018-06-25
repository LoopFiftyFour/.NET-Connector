namespace Loop54.Model.Request.Parameters.Filters
{
    /// <summary>
    /// Used for inverting a filter.
    /// </summary>
    public class InverseFilterParameter : FilterParameter
    {
        /// <summary>
        /// The filter that should be inversed.
        /// </summary>
        public FilterParameter Not { get; set; }
    }
}
