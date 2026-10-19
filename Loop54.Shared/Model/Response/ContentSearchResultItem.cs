namespace Loop54.Model.Response
{
    /// <summary>
    /// The Content Search results for one content entity type.
    /// </summary>
    public class ContentSearchResultItem
    {
        /// <summary>
        /// The content entity type these results are for, e.g. "Brand".
        /// </summary>
        public string EntityType { get; set; }

        /// <summary>
        /// The results for this entity type, in the same format as the product results.
        /// </summary>
        public EntityCollection Results { get; set; }
    }
}
