using Loop54.Model.Request.Parameters;
using Newtonsoft.Json.Linq;
using System;
using System.Xml.Linq;

namespace Loop54.Model.Request
{
    /// <summary>
    /// Used to perform a request to get entities with an indexed string attribute. For attributes 
    /// that are not indexed, use GetEntities with a filter instead. To find out what attributes 
    /// are indexed in the engine call /getIndexedAttributes.
    /// </summary>
    public class GetEntitiesByAttributeRequest : Request
    {
        public GetEntitiesByAttributeRequest(string attributeName, string attributeValue)
        {
            Attribute = new AttributeNameValuePairSingle()
            {
                Name = attributeName,
                Value = attributeValue
            };
        }

        public GetEntitiesByAttributeRequest(string attributeName, string[] attributeValue)
        {
            Attribute = new AttributeNameValuePairMultiple()
            {
                Name = attributeName,
                Value = attributeValue
            };
        }

        /// <summary>
        /// The attribute name-value-pair to find entities connected to. Note: this attribute needs 
        /// to be indexed in the engine. See the endpoint /getIndexedAttributes.
        /// </summary>
        public AttributeNameValuePair Attribute { get; set; }

        /// <summary>
        /// Parameters for specifying which results to retrieve and how to format them.
        /// </summary>
        public EntityCollectionParameters ResultsOptions { get; set; } = new EntityCollectionParameters();

        public abstract class AttributeNameValuePair
        {
            /// <summary>
            /// Name of the attribute.
            /// </summary>
            public string Name { get; set; }
        }

        /// <summary>
        /// Name-value-pair to identify an attribute to filter by. Supports a single value for the attribute.
        /// </summary>
        public class AttributeNameValuePairSingle : AttributeNameValuePair
        {
            //The client should not be able to create objects of this type.
            internal AttributeNameValuePairSingle()
            {
            }

            /// <summary>
            /// Value to filter by.
            /// </summary>
            public string Value { get; set; }
        }

        /// <summary>
        /// Name-value-pair to identify an attribute to filter by. Supports multiple values for the attribute.
        /// </summary>
        public class AttributeNameValuePairMultiple : AttributeNameValuePair
        {
            //The client should not be able to create objects of this type.
            internal AttributeNameValuePairMultiple()
            {
            }

            /// <summary>
            /// Value to filter by.
            /// </summary>
            public string[] Value { get; set; }
        }
    }
}
