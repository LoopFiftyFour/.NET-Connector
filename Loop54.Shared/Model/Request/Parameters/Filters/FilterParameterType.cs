namespace Loop54.Model.Request.Parameters.Filters
{
    /// <summary>
    /// Specifies the type of a attribute filter.
    /// </summary>
    public enum FilterParameterType
    {
        /// <summary>
        /// Compares against an attribute on the entity
        /// </summary>
        Attribute,

        /// <summary>
        /// Compares against the type of the entity
        /// </summary>
        Type,

        /// <summary>
        /// Compares against the id of the entity
        /// </summary>
        Id
    }
}
