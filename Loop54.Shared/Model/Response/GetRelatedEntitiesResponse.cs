
namespace Loop54.Model.Response
{
    /// <summary>
    /// The result of a GetRelatedEntities request.
    /// </summary>
    public class GetRelatedEntitiesResponse : Response
    {
        /// <summary>
        /// The entities that are related to the entity supplied in the request.
        /// </summary>
        public EntityCollection Results { get; set; }
    }
}
