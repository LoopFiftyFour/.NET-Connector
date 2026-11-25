using System.Collections.Generic;

namespace Loop54.Model.Request
{
    /// <summary>
    /// This class is used to configure a SyncDeltaPush request to the Loop54 e-commerce search engine. It performs a partial catalog sync,
    /// adding, updating, replacing or removing entities (or attributes of entities) without the engine having to read the whole catalog.
    ///
    /// The changes are queued and applied asynchronously by the engine. Use <see cref="ILoop54Client.GetSyncState(Request)"/> to follow their
    /// progress.
    ///
    /// The engine rejects the whole request if an entity (identified by type and id) appears more than once in a collection, or in more than one
    /// collection. The only exception is that an entity may be in both <see cref="ToAddOrUpdate"/> and <see cref="ToRemoveAttributesFrom"/>, as
    /// long as no attribute is both updated and removed.
    /// </summary>
    public class SyncDeltaPushRequest : Request
    {
        /// <summary>
        /// If the entity exists, its attributes will be updated with data from the supplied entity, otherwise it will be added.
        /// Attributes not present on the supplied entity are left unchanged. Entities without attributes are ignored.
        /// </summary>
        public IList<Entity> ToAddOrUpdate { get; set; } = new List<Entity>();

        /// <summary>
        /// If the entity exists, it will be completely replaced with the supplied entity, otherwise it will be added.
        /// Entities without attributes are ignored.
        /// </summary>
        public IList<Entity> ToReplaceOrAdd { get; set; } = new List<Entity>();

        /// <summary>
        /// If the entity exists, it will be removed, otherwise nothing happens. Only the type and id of the entities are needed.
        /// </summary>
        public IList<Entity> ToDelete { get; set; } = new List<Entity>();

        /// <summary>
        /// If the entity exists and has the specified attributes, these attributes will be removed from it. Only the names of the attributes
        /// are used, so they can be added without values, for instance with <c>entity.SetAttribute&lt;string&gt;("Color")</c>.
        /// </summary>
        public IList<Entity> ToRemoveAttributesFrom { get; set; } = new List<Entity>();
    }
}
