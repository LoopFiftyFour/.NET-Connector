namespace Loop54.Model.Response
{
    /// <summary>
    /// The result of a search operation.
    /// </summary>
    public class SearchResponse : Response
    {
        /// <summary>
        /// Whether or not the engine understood the search query.
        /// </summary>
        public bool MakesSense { get; set; }

        /// <summary>
        /// A collection of suggestions for alternate spellings of the query.
        /// </summary>
        public QueryCollection SpellingSuggestions { get; set; }

        /// <summary>
        /// A collection of suggestions for queries that are related to the provided query.
        /// </summary>
        public QueryCollection RelatedQueries { get; set; }

        /// <summary>
        /// The results that match the query.
        /// </summary>
        public EntityCollection Results { get; set; }

        /// <summary>
        /// Any additional results that, while not matching, are relevant to the query.
        /// </summary>
        public EntityCollection RelatedResults { get; set; }
    }
}
