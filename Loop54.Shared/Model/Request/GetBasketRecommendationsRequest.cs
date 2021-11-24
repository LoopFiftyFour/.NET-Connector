using Loop54.Model.Request.Parameters;
using System;
using System.Collections.Generic;

namespace Loop54.Model.Request
{
    /// <summary>
    /// Used to perform a request to get recommendations for a basket.
    /// </summary>
    public class GetBasketRecommendationsRequest : Request
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="entities">The entities to use in the query.</param>
        public GetBasketRecommendationsRequest(IList<Entity> entities)
        {
            Entities = entities;
        }

        /// <summary>
        /// The set of entities in the basket to get recommendations for.
        /// </summary>
        public IList<Entity> Entities { get; set; }

        /// <summary>
        /// Parameters for specifying which results to retrieve. Such as filtering, faceting, sorting and paging. 
        /// </summary>
        public EntityCollectionParameters ResultsOptions { get; set; } = new EntityCollectionParameters();
    }
}
