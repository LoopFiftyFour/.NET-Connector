namespace Loop54.Model.Request.Parameters
{
    /// <summary>
    /// Represents the type of relation that would be done when calling <see cref="GetRelatedEntitiesRequest"/>
    /// </summary>
    public enum RelationKinds
    {
        /// <summary>
        /// Return the entities that are on the same context as the requested entity 
        /// </summary>
        Similar,
        /// <summary>
        /// Return the entities that are usually 'purchased' together with the requested entity
        /// </summary>
        Complementary
    }
}
