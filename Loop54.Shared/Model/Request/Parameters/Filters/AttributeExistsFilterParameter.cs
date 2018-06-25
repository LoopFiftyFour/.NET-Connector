namespace Loop54.Model.Request.Parameters.Filters
{
    /// <summary>
    /// Used for filtering entities that have a certain attribute, regardless of the value.
    /// </summary>
    public class AttributeExistsFilterParameter : FilterParameter
    {
        /// <summary>
        /// The attribute to look for. For instance "category".
        /// </summary>
        public string AttributeName { get; set; }
    }
}
