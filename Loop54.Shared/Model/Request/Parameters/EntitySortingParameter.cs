namespace Loop54.Model.Request.Parameters
{
    public class EntitySortingParameter
    {
        /// <summary>
        /// How the entities should be sorted.
        /// </summary>
        public Types Type { get; set; }
        
        /// <summary>
        /// If choosing to sort by <see cref="Types.Attribute"/> this property specifies which attribute to sort by. 
        /// </summary>
        public string AttributeName { get; set; }
        
        /// <summary>
        /// In what order to sort the entities.
        /// </summary>
        public SortOrders Order { get; set; }

        public enum Types
        {
            Attribute,
            Id,
            Type,
            Relevance,
            Popularity
        }
    }
}
