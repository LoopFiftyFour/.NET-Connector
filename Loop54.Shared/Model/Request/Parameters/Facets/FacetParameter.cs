namespace Loop54.Model.Request.Parameters.Facets
{
    /// <summary>
    /// Used for faceting an entity collection based on the attributes of the entities
    /// </summary>
    public abstract class FacetParameter
    {
        protected FacetParameter(string attributeName)
        {
            AttributeName = attributeName;
        }

        /// <summary>
        /// Name of the facet to return to the client. Can be a 'friendly' name to show to the user. If not set the <see cref="AttributeName"/> will be used.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The name of the attribute on the entities to facet the result on. If the attribute does not exist on any entity, all entities will pass and an empty facet will be returned.
        /// </summary>
        public string AttributeName { get; set;}

        /// <summary>
        /// Type of the facet. Range or Distinct.
        /// </summary>
        public virtual FacetType Type { get; }
    }
}
