using Loop54.Model.Request.Parameters;

namespace Loop54.Model.Request
{
    /// <summary>
    /// Used to perform a request to get entities with an indexed string attribute. For attributes 
    /// that are not indexed, use GetEntities with a filter instead. To find out what attributes 
    /// are indexed in the engine call /getIndexedAttributes.
    /// </summary>
    public class GetEntitiesByAttributeRequest : Request
    {
        public GetEntitiesByAttributeRequest(string attributeName, string attributeValue, RequestAliasData requestAlias = null)
        {
            Attribute.Name = attributeName;
            Attribute.Value = attributeValue;
            RequestAlias = requestAlias;
        }
        
        /// <summary>
        /// The attribute name-value-pair to find entities connected to. Note: this attribute needs 
        /// to be indexed in the engine. See the endpoint /getIndexedAttributes.
        /// </summary>
        public AttributeNameValuePair Attribute { get; set; } = new AttributeNameValuePair();

        /// <summary>
        /// Parameters for specifying which results to retrieve and how to format them.
        /// </summary>
        public EntityCollectionParameters ResultsOptions { get; set; } = new EntityCollectionParameters();

        /// <summary>
        /// Provides human-readable labels in the Portal.
        /// </summary>
        public RequestAliasData RequestAlias { get; set; }

        /// <summary>
        /// Provides human-readable labels in the Portal.
        /// </summary>
        public class RequestAliasData
        {
            /// <summary>
            /// Specify an alias for this attribute name.
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// Specify an alias for this attribute value.
            /// </summary>
            public string Value { get; set; }

            /// <summary>
            /// Specify a more detailed description for this attribute name–value pair. Will be shown in the Portal.
            /// </summary>
            public string Details { get; set; }
        }

        /// <summary>
        /// Name-value-pair to identify an attribute to filter by.
        /// </summary>
        public class AttributeNameValuePair
        {
            //The client should not be able to create objects of this type.
            internal AttributeNameValuePair()
            {
            }

            /// <summary>
            /// Name of the attribute.
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// Value to filter by.
            /// </summary>
            public string Value { get; set; }
        }
    }
}
