using System.Collections.Generic;

namespace Loop54.Model
{
    public class Entity
    {
      
        /// <summary>
        /// Type of entity
        /// </summary>
        public string EntityType { get; private set; }

        /// <summary>
        /// Id of entity
        /// </summary>
        public string ExternalId { get; private set; }

        /// <summary>
        /// All attribute data stored on the entity. Each attribute has a key and a list of values.
        /// </summary>
        public Dictionary<string, List<object>> Attributes { get; private set; }

        public Entity(string entityType, string externalId)
        {
            EntityType = entityType;
            ExternalId = externalId;

            Attributes = new Dictionary<string, List<object>>();
        }

        public override string ToString()
        {
            return "{" + EntityType + ":" + ExternalId + "}";
        }
    }

}
