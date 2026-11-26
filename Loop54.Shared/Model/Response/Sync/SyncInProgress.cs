using System;
using Loop54.Serialization;
using Newtonsoft.Json;

namespace Loop54.Model.Response.Sync
{
    /// <summary>
    /// A catalog sync that the engine has received, but not yet applied or failed.
    /// </summary>
    public class SyncInProgress
    {
        /// <summary>
        /// The UTC time the sync was received.
        /// </summary>
        public DateTime StartedAtUtc { get; set; }

        /// <summary>
        /// The category of the sync: where it was triggered from, and what it contains.
        /// </summary>
        public SyncCategory Category { get; set; }

        /// <summary>
        /// How far the sync has come.
        /// </summary>
        [JsonConverter(typeof(LenientStringEnumConverter))]
        public SyncInProgressState State { get; set; }
    }

    /// <summary>
    /// How far a catalog sync in progress has come.
    /// </summary>
    public enum SyncInProgressState
    {
        /// <summary>The state is unknown, or not supported by this version of the connector.</summary>
        Unknown = 0,
        /// <summary>The catalog data is still being read.</summary>
        ReceivingData = 1,
        /// <summary>The sync is queued for processing.</summary>
        Queued = 2,
        /// <summary>The sync is being processed.</summary>
        BeingProcessed = 3,
    }
}
