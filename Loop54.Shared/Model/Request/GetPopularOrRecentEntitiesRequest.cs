using Loop54.Model.Request.Parameters;
using System;

namespace Loop54.Model.Request
{
    /// <summary>Used to perform a request to get the most popular entities, either for a given user or globally.</summary>
    public class GetPopularEntitiesRequest : GetPopularOrRecentEntitiesRequest
    {
        public GetPopularEntitiesRequest(string behaviorType, string[] entityType, string forUserId) : base(behaviorType, entityType, forUserId)
        {
        }

        /// <summary>Creates a request to get the most popular entities for the current user.</summary>
        public static GetPopularEntitiesRequest ForCurrentUser(string behaviorType, string[] entityType) 
            => new GetPopularEntitiesRequest(behaviorType, entityType, CurrentUserPlaceholder);
    }

    /// <summary>Used to perform a request to get the most recent entities, either for a given user or globally.</summary>
    public class GetRecentEntitiesRequest : GetPopularOrRecentEntitiesRequest
    {
        public GetRecentEntitiesRequest(string behaviorType, string[] entityType, string forUserId) : base(behaviorType, entityType, forUserId)
        {
        }

        /// <summary>Creates a request to get the most recent entities for the current user.</summary>
        public static GetRecentEntitiesRequest ForCurrentUser(string behaviorType, string[] entityType)
            => new GetRecentEntitiesRequest(behaviorType, entityType, CurrentUserPlaceholder);
    }

    /// <summary>Base class for requests to get the most popular or most recent entities.</summary>
    public abstract class GetPopularOrRecentEntitiesRequest : Request
    {
        /// <summary>
        /// Represents a placeholder value used in `GetRecentEntitiesRequest` and `GetPopularEntitiesRequest` to indicate that the request should 
        /// target the current user's data.
        /// </summary>
        public const string CurrentUserPlaceholder = "(CurrentUser)";

        protected GetPopularOrRecentEntitiesRequest(string behaviorType, string[] entityType, string forUserId)
        {
            if (string.IsNullOrEmpty(behaviorType))
                throw new ArgumentNullException(nameof(behaviorType));
            if (entityType != null && entityType.Length == 0)
                throw new ArgumentException($"{nameof(entityType)} must not be an empty array (but may be null to include all entity types).");
            if (forUserId != null && forUserId.Length == 0)
                throw new ArgumentException($"{nameof(forUserId)} must not be an empty string (but may be null for all users).");

            BehaviorType = behaviorType;
            EntityType = entityType;
            ForUserId = forUserId;
        }

        /// <summary>The interaction or navigation type to include (such as "click", "purchase" or "search").</summary>
        public string BehaviorType { get; set; }

        /// <summary>The entity types to include (such as "Product" or "Query") or null for all.</summary>
        public string[] EntityType { get; set; }

        /// <summary>
        /// A user ID to retrieve the most common/recent entities for that user or null to retrieve the globally most common/recent entities.
        /// <see cref="CurrentUserPlaceholder"/> can be specified to use the user ID from the User-Id request header.
        /// </summary>
        public string ForUserId { get; set; }

        /// <summary>Parameters for specifying which results to retrieve, such as filtering, faceting, sorting and paging.</summary>
        public EntityCollectionParameters ResultsOptions { get; set; } = new EntityCollectionParameters();
    }
}
