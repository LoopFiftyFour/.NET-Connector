using Loop54.Model.Request.Parameters;

namespace Loop54.Model.Request
{
    /// <summary>
    /// This class is used to configure a GetEntities request to the Loop54 e-commerce search engine.
    /// </summary>
    public class GetEntitiesRequest : Request
    {
        /// <summary>
        /// Parameters for specifying which entities to retrieve. Such as filtering, faceting, sorting and paging. 
        /// 
        /// Note that filtering is advised when doing this request.
        /// </summary>
        public EntityCollectionParameters ResultsOptions { get; set; } = new EntityCollectionParameters();
    }
}
