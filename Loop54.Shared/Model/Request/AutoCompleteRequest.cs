using Loop54.Model.Request.Parameters;

namespace Loop54.Model.Request
{
    /// <summary>
    /// This class is used for performing auto-complete requests to the Loop54 e-commerce search engine. 
    /// </summary>
    public class AutoCompleteRequest : Request
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="query">The partial query that the user has entered</param>
        public AutoCompleteRequest(string query)
        {
            Query = query; 
        }

        /// <summary>
        /// The partial query that the user has entered
        /// </summary>
        public string Query { get; set; }

        /// <summary>
        /// Parameters for specifying how the auto-complete results should be retrieved. Contains paging and sorting options.
        /// </summary>
        public QueryCollectionParameters QueriesOptions { get; set; }
    }
}
