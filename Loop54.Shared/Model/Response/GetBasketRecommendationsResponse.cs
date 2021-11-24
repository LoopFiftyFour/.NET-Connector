namespace Loop54.Model.Response
{
    /// <summary>
    /// The result of a GetBasketRecommendations request.
    /// </summary>
    public class GetBasketRecommendationsResponse : Response
    {
        /// <summary>
        /// The recommended entities.
        /// </summary>
        public EntityCollection Results { get; set; }
    }
}
