using System.Collections.Generic;

namespace Loop54.Model.Request.Parameters
{
    /// <summary>
    /// This class is used to specify which queries to get in a request. Used for paging and sorting.
    /// </summary>
    public class QueryCollectionParameters
    {
        /// <summary>
        /// How many queries to skip. If null, the engine defaults to 0.
        /// </summary>
        public int? Skip { get; set; }

        /// <summary>
        /// How many queries to take. If null, the engine defaults to 5.
        /// </summary>
        public int? Take { get; set; }

        /// <summary>
        /// How to sort the queries. Will default to relevance descending.
        /// </summary>
        public IList<QuerySortingParameter> SortBy { get; set; }
    }
}
