
namespace Loop54.Model.Response
{
    /// <summary>
    /// The result of a getRecommendedEntities request.
    /// </summary>
    public class GetRecommendedEntitiesResponse : Response
    {
        /// <summary>
        /// The personalized recommended entities.
        /// </summary>
        public EntityCollection Results { get; set; }
    }
}
