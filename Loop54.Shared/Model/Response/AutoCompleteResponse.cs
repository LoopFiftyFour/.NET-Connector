

namespace Loop54.Model.Response
{
    /// <summary>
    /// The result of a autocomplete request.
    /// </summary>
    public class AutoCompleteResponse : Response
    {
        /// <summary>
        /// The query the engine deemed most relevant, will get a result with scopes.
        /// </summary>
        public ScopedQueryResult ScopedQuery { get; set; }

        /// <summary>
        /// A collection of query suggestions for the request query.
        /// </summary>
        public QueryCollection Queries { get; set; }
    }
}
