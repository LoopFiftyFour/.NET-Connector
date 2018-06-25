using System.Collections.Generic;

namespace Loop54.Model.Response
{
    /// <summary>
    /// A collection of queries (strings).
    /// </summary>
    public class QueryCollection
    {
        /// <summary>
        /// The total number of queries available before paging.
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// The queries after paging.
        /// </summary>
        public IList<QueryResult> Items { get; set; }
    }
}
