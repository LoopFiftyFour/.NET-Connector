namespace Loop54.Model
{
    /// <summary>
    /// Represent the two types of facets available to make.
    /// </summary>
    public enum FacetType
    {
        /// <summary>
        /// A facet should consist of a finite number of options with the number of connected entities as the values.
        /// Only entities connected to selected facet options are returned. Or all if none are selected.
        /// </summary>
        Distinct,

        /// <summary>
        /// A facet that has a min and max value and only entities in between them are returned.
        /// </summary>
        Range
    }
}
