using System.Collections.Generic;
using System.Linq;

namespace Loop54.Model.Response
{
    /// <summary>
    /// Base class for responses that can contain Content Search results.
    /// </summary>
    public abstract class ContentResponse : Response
    {
        /// <summary>
        /// Results from Content Search, one entry per content entity type that was searched.
        /// Only set when Content Search is enabled and content results were requested.
        /// </summary>
        public IList<ContentSearchResultItem> ContentSearchResults { get; set; }

        /// <summary>
        /// Gets the Content Search results for the given entity type, or null if that type was not searched.
        /// </summary>
        /// <param name="entityType">The content entity type, e.g. "Brand". Case-sensitive.</param>
        public EntityCollection GetContentSearchResults(string entityType)
        {
            return ContentSearchResults?.FirstOrDefault(c => c.EntityType == entityType)?.Results;
        }
    }
}
