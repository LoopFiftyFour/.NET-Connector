using Loop54.Model.Request.Parameters;

namespace Loop54.Model.Request
{
    /// <summary>
    /// Used to perform a request to get personalized recommendations.
    /// </summary>
    public class GetRecommendedEntitiesRequest : Request
    {
        /// <summary>
        /// Parameters for specifying which recommendations results to retrieve and how to format them.
        /// </summary>
        public EntityCollectionParameters ResultsOptions { get; set; } = new EntityCollectionParameters();
    }
}
