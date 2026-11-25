using System.Collections.Generic;
using System.Linq;
using Loop54.Model.Response.Sync;

namespace Loop54.Model.Response
{
    /// <summary>
    /// The response of a GetSyncState request. Contains the catalog syncs that are still in progress, and the most recently applied and failed
    /// syncs per sync category.
    /// </summary>
    public class GetSyncStateResponse : Response
    {
        /// <summary>
        /// The syncs that have been received but not yet applied or failed, oldest first. The engine only lists a limited number of them
        /// (currently 100), see <see cref="InProgressCount"/> for the total.
        /// </summary>
        public IList<SyncInProgress> InProgress { get; set; } = new List<SyncInProgress>();

        /// <summary>
        /// The total number of syncs that have been received but not yet applied or failed.
        /// </summary>
        public int InProgressCount { get; set; }

        /// <summary>
        /// The latest success and failure of the syncs per category, for the categories that have had a sync applied or failed since the
        /// engine started.
        /// </summary>
        public IList<SyncStateByCategory> ByCategory { get; set; } = new List<SyncStateByCategory>();

        /// <summary>
        /// Gets the sync state of a category, for instance <c>GetByCategory(SyncTriggeredFrom.ExternalEndpointSyncDeltaPush)</c> for the
        /// pushes made with SyncDeltaPush.
        /// </summary>
        /// <param name="triggeredFrom">Where the syncs were triggered from.</param>
        /// <param name="pullSourceId">The source ID of the pulls, such as "full" or "delta". Null for pushes.</param>
        /// <returns>The state of the category, or null if no sync of that category has been applied or failed since the engine started.</returns>
        public SyncStateByCategory GetByCategory(SyncTriggeredFrom triggeredFrom, string pullSourceId = null) =>
            ByCategory?.FirstOrDefault(s => s.Category != null && s.Category.TriggeredFrom == triggeredFrom
                && s.Category.PullSourceId == pullSourceId);
    }
}
