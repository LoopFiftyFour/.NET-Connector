using Loop54.Model.Request.Parameters;
using System;

namespace Loop54.Model.Request
{
    /// <summary>
    /// Used to perform a request to get entities complementary to given selected entity.
    /// </summary>
    public class GetComplementaryEntitiesRequest : Request
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="entityType">Type of the entity to query</param>
        /// <param name="entityId">Id of the entity to query</param>
        public GetComplementaryEntitiesRequest(string entityType, string entityId)
            : this(new Entity(entityType ?? throw new ArgumentNullException(nameof(entityType)), entityId ?? throw new ArgumentNullException(nameof(entityId))))
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="entity">The entity to use in the query.</param>
        public GetComplementaryEntitiesRequest(Entity entity)
        {
            Entity = entity ?? throw new ArgumentNullException(nameof(entity));
        }

        /// <summary>
        /// The entity whose complementary entities we want to get. Only needs the type and id set.
        /// </summary>
        public Entity Entity { get; set; }

        /// <summary>
        /// Parameters for specifying which results to retrieve. Such as filtering, faceting, sorting and paging. 
        /// </summary>
        public EntityCollectionParameters ResultsOptions { get; set; } = new EntityCollectionParameters();
    }
}
