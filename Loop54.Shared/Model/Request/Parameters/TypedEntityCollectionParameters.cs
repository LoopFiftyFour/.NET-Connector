using System;

namespace Loop54.Model.Request.Parameters
{
    /// <summary>
    /// Specifies how a collection of entities of a specific entity type should be paged, filtered, sorted and faceted.
    /// Used for Content Search, where each content entity type (e.g. "Brand", "Article") has its own options.
    /// </summary>
    public class TypedEntityCollectionParameters : EntityCollectionParameters
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="entityType">The content entity type to search, e.g. "Brand". Case-sensitive.</param>
        public TypedEntityCollectionParameters(string entityType)
        {
            if (string.IsNullOrWhiteSpace(entityType))
                throw new ArgumentException("Entity type must be set.", nameof(entityType));

            EntityType = entityType;
        }

        /// <summary>
        /// The content entity type to search, e.g. "Brand" or "Article". Case-sensitive.
        /// Search, paging, sorting, faceting and filtering are scoped to this entity type only.
        /// </summary>
        public string EntityType { get; set; }
    }
}
