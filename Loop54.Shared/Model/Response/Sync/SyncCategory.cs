using Loop54.Serialization;
using Newtonsoft.Json;

namespace Loop54.Model.Response.Sync
{
    /// <summary>
    /// A group of catalog syncs that are reported together: the pushes triggered from the same place, or the pulls triggered from the same
    /// place with the same pull source ID.
    /// </summary>
    public class SyncCategory
    {
        /// <summary>
        /// Where the syncs were triggered from.
        /// </summary>
        [JsonConverter(typeof(LenientStringEnumConverter))]
        public SyncTriggeredFrom TriggeredFrom { get; set; }

        /// <summary>
        /// The source ID of the pulls, for instance "full" or "delta". Null for pushes.
        /// </summary>
        public string PullSourceId { get; set; }
    }

    /// <summary>
    /// Where a catalog sync was triggered from.
    /// </summary>
    public enum SyncTriggeredFrom
    {
        /// <summary>Unknown, or not supported by this version of the connector.</summary>
        Unknown = 0,
        /// <summary>The sync was started by the engine itself, for instance a scheduled sync or a sync triggered manually.</summary>
        Internal = 1,
        /// <summary>The sync was requested through the Sync endpoint.</summary>
        ExternalEndpointSync = 2,
        /// <summary>The sync was requested through the SyncDeltaPush endpoint.</summary>
        ExternalEndpointSyncDeltaPush = 3,
        /// <summary>The sync was requested through a custom endpoint.</summary>
        ExternalEndpointCustom = 4,
    }
}
