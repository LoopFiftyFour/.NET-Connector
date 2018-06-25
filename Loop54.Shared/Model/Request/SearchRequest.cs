using Loop54.Model.Request.Parameters;

namespace Loop54.Model.Request
{
    /// <summary>
    /// This class is used to configure a search request to the Loop54 e-commerce search engine. 
    /// </summary>
    public class SearchRequest : Request
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="query">The search query from the end-user. Can at maximum be 200 chars long.</param>
        public SearchRequest(string query)
        {
            Query = query;
        }

        /// <summary>
        /// The search query from the end-user. Can at maximum be 200 chars long.
        /// </summary>
        public string Query { get; set; }

        /// <summary>
        /// Parameters for specifying which direct results to retrieve. Such as filtering, faceting, sorting and paging. 
        /// </summary>
        /// <remarks>Only affects the direct results, to modify the related results use <see cref="RelatedResultsOptions"/>.</remarks>
        public EntityCollectionParameters ResultsOptions { get; set; } = new EntityCollectionParameters();

        /// <summary>
        /// Parameters for specifying which related results to retrieve. Such as filtering, faceting, sorting and paging.
        /// </summary>
        /// <remarks>Only affects the related results, to modify the direct results use <see cref="ResultsOptions"/>.</remarks>
        public EntityCollectionParameters RelatedResultsOptions { get; set; } = new EntityCollectionParameters();

        /// <summary>
        /// Parameters for specifying how spelling suggestions should be retrieved. Contains paging and sorting options.
        /// </summary>
        public QueryCollectionParameters SpellingSuggestionsOptions { get; set; } = new QueryCollectionParameters();

        /// <summary>
        /// Parameters for specifying how related queries should be retrieved. Contains paging and sorting options.
        /// </summary>
        public QueryCollectionParameters RelatedQueriesOptions { get; set; } = new QueryCollectionParameters();
    }
}
