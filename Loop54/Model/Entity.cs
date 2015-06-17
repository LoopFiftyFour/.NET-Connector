using System.Collections.Generic;

namespace Loop54.Model
{
    public class Entity
    {
        public string EntityType { get; set; }
        public string ExternalId { get; set; }

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
