namespace Loop54.Model.Response
{
    /// <summary>
    /// The result of a suggest request.
    /// </summary>
    public class SuggestResponse : Response
    {
        /// <summary>
        /// Whether the engine understood the search query.
        /// </summary>
        public bool MakesSense { get; set; }

        /// <summary>
        /// The (normalized) original query typed by the user.
        /// </summary>
        public string OriginalQuery { get; set; }
        
        /// <summary>
        /// The query used by the engine for the search request. I may either be the original query, or the top query suggestion.
        /// </summary>
        public string QueryUsedForSearch { get; set; }
        
        /// <summary>
        /// The query the engine deemed most relevant, will get a result with scopes.
        /// </summary>
        public ScopedQueryResult ScopedQuery { get; set; }

        /// <summary>
        /// A collection of query suggestions for the request query.
        /// </summary>
        public QueryCollection Queries { get; set; }
        
        /// <summary>
        /// The results that match the query.
        /// </summary>
        public EntityCollection Results { get; set; }
    }
}
