namespace Loop54.Model.Request
{
    /// <summary>Request to the getIndexedAttributeValues API method of the Loop54 e-commerce search engine.</summary>
    public class GetIndexedAttributeValuesRequest : Request
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="attributeName">The indexed attribute for which to fetch unique values. Can at maximum be 200 chars long.</param>
        public GetIndexedAttributeValuesRequest(string attributeName)
        {
            AttributeName = attributeName;
        }

        /// <summary>
        /// The search query from the end-user. Can at maximum be 200 chars long.
        /// </summary>
        public string AttributeName { get; set; }
    }
}
