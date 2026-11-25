using System;

namespace Loop54.Model.Response.Sync
{
    /// <summary>
    /// The result of a successful catalog sync.
    /// </summary>
    public class SyncSuccess
    {
        /// <summary>
        /// The UTC time the sync was applied.
        /// </summary>
        public DateTime AppliedAtUtc { get; set; }

        /// <summary>
        /// The earliest change timestamp covered by the sync.
        /// </summary>
        public DateTime EarliestChangeFromUtc { get; set; }

        /// <summary>
        /// Number of entities added or changed by the sync.
        /// </summary>
        public int ChangedOrAddedCount { get; set; }

        /// <summary>
        /// Number of entities removed by the sync.
        /// </summary>
        public int RemovedCount { get; set; }

        /// <summary>
        /// Number of sync events (such as SyncDeltaPush requests) included in the sync.
        /// </summary>
        public int AppliedEventCount { get; set; }
    }
}
