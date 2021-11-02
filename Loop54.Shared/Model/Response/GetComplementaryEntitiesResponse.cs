
namespace Loop54.Model.Response
{
    /// <summary>
    /// The result of a GetComplementaryEntities request.
    /// </summary>
    public class GetComplementaryEntitiesResponse : Response
    {
        /// <summary>
        /// The entities that are complementary to the entity supplied in the request.
        /// </summary>
        public EntityCollection Results { get; set; }
    }
}
