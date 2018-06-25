
namespace Loop54.Model.Response
{
    /// <summary>
    /// The results of a getEntitites request.
    /// </summary>
    public class GetEntitiesResponse : Response
    {
        /// <summary>
        /// The entities returned in the request.
        /// </summary>
        public EntityCollection Results { get; set; }
    }
}
