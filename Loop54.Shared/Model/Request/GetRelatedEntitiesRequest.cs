using Loop54.Model.Request.Parameters;
using System;

namespace Loop54.Model.Request
{
    /// <summary>
    /// Used to perform a request to get entities related to given selected entity. That is, being in the same context.
    /// </summary>
    public class GetRelatedEntitiesRequest : Request
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="entityType">Type of the entity to query</param>
        /// <param name="entityId">Id of the entity to query</param>
        public GetRelatedEntitiesRequest(string entityType, string entityId)
            : this(new Entity(entityType ?? throw new ArgumentNullException(nameof(entityType)), entityId ?? throw new ArgumentNullException(nameof(entityId))))
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="entity">The entity to use in the query.</param>
        public GetRelatedEntitiesRequest(Entity entity)
        {
            Entity = entity ?? throw new ArgumentNullException(nameof(entity));
        }

        /// <summary>
        /// The entity who's related entities we want to get. Only needs the type and id set.
        /// </summary>
        public Entity Entity { get; set; }

        /// <summary>
        /// Parameters for specifying which results to retrieve. Such as filtering, faceting, sorting and paging. 
        /// </summary>
        public EntityCollectionParameters ResultsOptions { get; set; } = new EntityCollectionParameters();
        
        /// <summary>
        /// The kind of relation that will be used to create resulting entities. Similar or Complementary.
        /// Defaults to Similar if not specified.
        /// </summary>
        /// <remarks>
        /// When getting related entities for 'BrandA bike tire': <br/>
        /// "similar" - will return entities that are 'more of the same' (f.e. 'BrandB bike tire')<br/>
        /// "complementary" - will return entities that are 'people also buy' (f.e. 'BrandX bike pump')
        /// </remarks>
        public RelationTypes? RelationKind { get; set; } = null;
    }
}
