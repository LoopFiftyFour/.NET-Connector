namespace Loop54.Model.Request.Parameters.Filters
{
    /// <summary>
    /// Used for filtering entities that have a certain attribute, regardless of the value.
    /// </summary>
    public class AttributeExistsFilterParameter : FilterParameter
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="attributeName">The attribute to check whether it exists or not. For instance "category". Note that the names of the attributes vary depending on setup.</param>
        public AttributeExistsFilterParameter(string attributeName)
        {
            AttributeName = attributeName;
        }

        /// <summary>
        /// The attribute to look for. For instance "category".
        /// </summary>
        public string AttributeName { get; set; }
    }
}
