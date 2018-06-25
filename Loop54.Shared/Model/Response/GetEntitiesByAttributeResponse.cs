
namespace Loop54.Model.Response
{
    /// <summary>
    /// The result of a request to get entities with an attribute (for instance category).
    /// </summary>
    public class GetEntitiesByAttributeResponse : Response
    {
        /// <summary>
        /// The entities that are connected to the attribute.
        /// </summary>
        public EntityCollection Results { get; set; }
    }
}
