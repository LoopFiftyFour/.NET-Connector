using System;

namespace Loop54.Model.Response.Sync
{
    /// <summary>
    /// The result of a failed catalog sync.
    /// </summary>
    public class SyncFailure
    {
        /// <summary>
        /// The UTC time the failure was recorded.
        /// </summary>
        public DateTime FailedAtUtc { get; set; }

        /// <summary>
        /// Number of sync events (such as SyncDeltaPush requests) included in the failed sync.
        /// </summary>
        public int FailedEventCount { get; set; }
    }
}
