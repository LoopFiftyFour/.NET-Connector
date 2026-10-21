using Loop54.Model.Request.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Loop54.Model.Request
{
    /// <summary>
    /// Base class for requests that can also retrieve Content Search results.
    /// </summary>
    public abstract class ContentRequest : Request
    {
        /// <summary>
        /// Parameters for specifying which content results to retrieve (Content Search). One entry per content entity type,
        /// e.g. "Brand" or "Article", each with its own paging, sorting, faceting and filtering. If not set, no content is searched.
        /// </summary>
        /// <remarks>Does not affect the product results.</remarks>
        public IList<TypedEntityCollectionParameters> ContentResultsOptions { get; set; }

        /// <summary>
        /// Adds a content entity type to search and returns its parameters, so that paging, sorting, faceting and filtering can be set on it.
        /// If parameters for that entity type were already added, the existing parameters are returned instead of adding a duplicate.
        /// </summary>
        /// <param name="entityType">The content entity type to search, e.g. "Brand". Case-sensitive.</param>
        /// <returns>The parameters for the added content entity type.</returns>
        public TypedEntityCollectionParameters AddContentResultsOptions(string entityType)
        {
            if (string.IsNullOrWhiteSpace(entityType))
                throw new ArgumentException("Entity type must be set.", nameof(entityType));

            if (ContentResultsOptions == null)
                ContentResultsOptions = new List<TypedEntityCollectionParameters>();

            var existing = ContentResultsOptions.FirstOrDefault(o => o != null && o.EntityType == entityType);
            if (existing != null)
                return existing;

            var options = new TypedEntityCollectionParameters(entityType);
            ContentResultsOptions.Add(options);
            return options;
        }
    }
}
