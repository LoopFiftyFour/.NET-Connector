namespace Loop54.Model.Response
{
    /// <summary>The results of a getIndexedAttributes request.</summary>
    public class GetIndexedAttributesResponse : Response
    {
        /// <summary>Entity attributes that the engine currently has in memory, which are available for filtering, sorting and faceting.</summary>
        public string[] Attributes { get; set; }

        /// <summary>Attributes that are indexed by the engine and can be used in a getEntitiesByAttribute request.</summary>
        public string[] IndexedAttributes { get; set; }
    }
}
