namespace Loop54.Model.Response
{
    /// <summary>The results of a getIndexedAttributeValues request.</summary>
    public class GetIndexedAttributeValuesResponse : Response
    {
        /// <summary>The unique values that are indexed for the provided attribute.</summary>
        public string[] Values { get; set; }
    }
}
