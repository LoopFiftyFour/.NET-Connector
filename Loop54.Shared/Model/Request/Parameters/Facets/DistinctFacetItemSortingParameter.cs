namespace Loop54.Model.Request.Parameters.Facets
{
    /// <summary>
    /// Describes how to sort the items returned in the facet.
    /// </summary>
    public class DistinctFacetItemSortingParameter
    {
        /// <summary>
        /// How the sorting is done.
        /// </summary>
        public Types Type { get; set; }

        /// <summary>
        /// Whether to sort items descending or ascending.
        /// </summary>
        public SortOrders Order { get; set; }

        public enum Types
        {
            Item,
            Count,
            Selected
        }
    }
}
