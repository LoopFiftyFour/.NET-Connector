namespace Loop54.Model.Request.Parameters
{
    /// <summary>
    /// This class specifies how queries should be sorted when making a request.
    /// </summary>
    public class QuerySortingParameter
    {
        /// <summary>
        /// How the queries should be sorted.
        /// </summary>
        public Types Type { get; set; }
        
        /// <summary>
        /// In what order to sort the queries.
        /// </summary>
        public SortOrders Order { get; set; }

        /// <summary>
        /// The types of sorting available when sorting queries.
        /// </summary>
        public enum Types
        {
            Relevance,
            Popularity,
            Alphabetic
        }
    }
}
