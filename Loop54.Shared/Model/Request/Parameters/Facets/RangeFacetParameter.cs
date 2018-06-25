namespace Loop54.Model.Request.Parameters.Facets
{
    /// <summary>
    /// A range facet has a min and max value and only entities in between them are returned.
    /// </summary>
    /// <typeparam name="T">Type of the attribute to facet on.</typeparam>
    public class RangeFacetParameter<T> : FacetParameter
    {
        public RangeFacetParameter(string attributeName) : base(attributeName)
        {
        }

        /// <summary>
        /// Returns the type of the facet parameter. Will always be <see cref="FacetType.Range"/>.
        /// </summary>
        public override FacetType Type => FacetType.Range;

        /// <summary>
        /// The min and max values selected by the user.
        /// </summary>
        public RangeFacetSelectedParameter<T> Selected { get; set; }
    }

    /// <summary>
    /// The selected min and max for the facet.
    /// </summary>
    /// <typeparam name="T">Type of the attribute to facet on.</typeparam>
    public class RangeFacetSelectedParameter<T>
    {
        /// <summary>
        /// Selected minimum.
        /// </summary>
        public T Min { get; set; }

        /// <summary>
        /// Selected maximum.
        /// </summary>
        public T Max { get; set; }
    }
}
