namespace Loop54.Model.Response.Sync
{
    /// <summary>
    /// The state of the catalog syncs of the same category, for instance the pulls with the "delta" source ID triggered through the Sync endpoint.
    /// </summary>
    public class SyncStateByCategory
    {
        /// <summary>
        /// The category of the syncs.
        /// </summary>
        public SyncCategory Category { get; set; }

        /// <summary>
        /// The result of the most recently applied of these syncs, or null if none has succeeded yet. The entity counts cover the whole catalog
        /// update the syncs were applied in, which can include syncs of other categories. The event count only covers the syncs of this category.
        /// </summary>
        public SyncSuccess LatestSuccess { get; set; }

        /// <summary>
        /// The result of the most recently failed of these syncs, or null if none has failed yet. The event count only covers the syncs of
        /// this category.
        /// </summary>
        public SyncFailure LatestFailure { get; set; }
    }
}
