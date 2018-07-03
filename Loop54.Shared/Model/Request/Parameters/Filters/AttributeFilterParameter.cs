namespace Loop54.Model.Request.Parameters.Filters
{
    /// <summary>
    /// Used for filtering entities that have a certain attribute value.
    /// </summary>
    /// <typeparam name="T">The type of the entity that we want to filter.</typeparam>
    public class AttributeFilterParameter<T> : FilterParameter
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="type">Type of the filter. If you want to use <see cref="FilterParameterType.Attribute"/> 
        /// it's easier to use the <see cref="AttributeFilterParameter(string, T)"/> constructor.</param>
        /// <param name="value">The value to compare. Either the id or type.</param>
        public AttributeFilterParameter(FilterParameterType type, T value)
        {
            Type = type;
            Value = value;
        }

        /// <summary>
        /// Constructor. Use this if you want to use the filter parameter to filter on an attribute. Will set the 
        /// type of the filter to <see cref="FilterParameterType.Attribute"/>.
        /// </summary>
        /// <param name="attributeName">Name of the attribute to filter on. For instance "category". Note that the names of the attributes vary depending on setup.</param>
        /// <param name="value">The value to compare. For instance a category name if filtering on a category attribute.</param>
        public AttributeFilterParameter(string attributeName, T value)
        {
            Type = FilterParameterType.Attribute;
            AttributeName = attributeName;
            Value = value;
        }

        /// <summary>
        /// Type of the filter. If the type of the filter is <see cref="FilterParameterType.Attribute"/>, the name of the 
        /// attribute needs to be specified in the <see cref="AttributeName"/> property.
        /// </summary>
        public FilterParameterType Type { get; set; }

        /// <summary>
        /// The name of attribute to filter on. This must be set if <see cref="Type"/> is <see cref="FilterParameterType.Attribute"/>.
        /// </summary>
        public string AttributeName { get; set; }

        /// <summary>
        /// The value to test the attributes values against.
        /// </summary>
        public T Value { get; set; }

        /// <summary>
        /// How to compare the attribute value against the provided value. Defaults to <see cref=" FilterComparisonMode.Equals"/>.
        /// </summary>
        public FilterComparisonMode ComparisonMode { get; set; } = FilterComparisonMode.Equals;
    }
}
