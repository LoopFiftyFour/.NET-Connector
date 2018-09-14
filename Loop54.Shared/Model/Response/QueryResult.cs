using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Loop54.Model.Response
{
    /// <summary>
    /// An instance of a query. Wraps a string, with optional scopes.
    /// </summary>
    public class QueryResult
    {
        /// <summary>
        /// The query.
        /// </summary>
        public string Query { get; set; }
    }

    /// <summary>
    /// A query with scopes. Scopes can be categories, brands or another attribute of the entities related to the query to do faceted autocomplete.
    /// </summary>
    public class ScopedQueryResult : QueryResult
    {
        /// <summary>
        /// Scopes where this query is relevant. Based on which entity attribute values will be present in the search results. Use together with faceting of search results for this query.
        /// </summary>
        public IList<string> Scopes { get; set; }

        /// <summary>
        /// Which attribute the scopes were built using.
        /// </summary>
        public string ScopeAttributeName { get; set; }
    }
}
