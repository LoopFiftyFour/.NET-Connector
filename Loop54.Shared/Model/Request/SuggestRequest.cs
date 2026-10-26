using Loop54.Model.Request.Parameters;

namespace Loop54.Model.Request
{
    /// <summary>
    /// This class is used to configure a suggest request to the Loop54 e-commerce search engine. 
    /// </summary>
    public class SuggestRequest : ContentRequest
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="query">The search query from the end-user. Can at maximum be 200 chars long.</param>
        /// <param name="useTopSuggestionForSearch">When 'true', the search may run against the top suggested autocompletion for the input query.</param>
        public SuggestRequest(string query, bool useTopSuggestionForSearch = true)
        {
            Query = query;
            UseTopSuggestionForSearch = useTopSuggestionForSearch;
        }
        
        /// <summary>
        /// The search query from the end-user. Can at maximum be 200 chars long.
        /// </summary>
        public string Query { get; set; }
        
        /// <summary>
        /// When 'true', the search may run against the top suggested autocompletion for the input query.
        /// </summary>        
        public bool UseTopSuggestionForSearch { get; set; }
        
        /// <summary>
        /// Parameters for specifying which direct results to retrieve. Such as filtering, faceting, sorting and paging. 
        /// </summary>
        public EntityCollectionParameters ResultsOptions { get; set; } = new EntityCollectionParameters();
        
        /// <summary>
        /// Parameters for specifying how the auto-complete results should be retrieved. Contains paging and sorting options.
        /// </summary>
        public QueryCollectionParameters QueriesOptions { get; set; } = new QueryCollectionParameters();
    }
}

