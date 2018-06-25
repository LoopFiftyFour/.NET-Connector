namespace Loop54.Model.Request.Parameters.Filters
{
    /// <summary>
    /// Used for filtering entities that have a certain attribute value.
    /// </summary>
    /// <typeparam name="T">The type of the entity that we want to filter.</typeparam>
    public class AttributeFilterParameter<T> : FilterParameter
    {
        /// <summary>
        /// Type of the filter. If the type of the filter is <see cref="FilterParameterType.Attribute"/>, the name of the 
        /// attribute needs to be specified in the <see cref="AttributeName"/> property.
        /// </summary>
        public FilterParameterType Type { get; set; }

        /// <summary>
        /// The name of attribute to filter on.
        /// </summary>
        public string AttributeName { get; set; }

        /// <summary>
        /// The value to test the attributes values against.
        /// </summary>
        public T Value { get; set; }

        /// <summary>
        /// How to compare the attribute value against the provided value.
        /// </summary>
        public FilterComparisonMode ComparisonMode { get; set; }
    }
}
