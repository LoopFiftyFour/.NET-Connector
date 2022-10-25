namespace Loop54.Model.Request
{
    /// <summary>Request to the getIndexedAttributeValues API method of the Loop54 e-commerce search engine.</summary>
    public class GetIndexedAttributeValuesRequest : Request
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="attributeName">The indexed attribute for which to fetch unique values.</param>
        public GetIndexedAttributeValuesRequest(string attributeName)
        {
            AttributeName = attributeName;
        }

        /// <summary>
        /// The indexed attribute for which to fetch unique values.
        /// </summary>
        public string AttributeName { get; set; }
    }
}
