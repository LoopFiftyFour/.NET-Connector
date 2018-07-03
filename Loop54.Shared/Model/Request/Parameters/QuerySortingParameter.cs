namespace Loop54.Model.Request.Parameters
{
    /// <summary>
    /// This class specifies how queries should be sorted when making a request.
    /// </summary>
    public class QuerySortingParameter
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public QuerySortingParameter()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="type">How to sort the queries</param>
        public QuerySortingParameter(Types type)
        {
            Type = type;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="type">How to sort the queries</param>
        /// <param name="order">In what order to sort the queries</param>
        public QuerySortingParameter(Types type, SortOrders order)
            : this(type)
        {
            Order = order;
        }
        
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
