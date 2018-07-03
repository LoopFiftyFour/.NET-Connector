namespace Loop54.Model.Request.Parameters
{
    /// <summary>
    /// This class is used to specify how to sort entities in a response from the Loop54 e-commerce search engine.
    /// </summary>
    public class EntitySortingParameter
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public EntitySortingParameter()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="type">How to order the entities.</param>
        public EntitySortingParameter(Types type)
        {
            Type = type;
        }

        /// <summary>
        /// Constructor. Used when wanting to order on a certain attribute on the entities.
        /// </summary>
        /// <param name="attributeName">What attribute to sort on. For instance "category" or "price". Note that the attributes in your environment might vary.</param>
        public EntitySortingParameter(string attributeName)
        {
            Type = Types.Attribute;
            AttributeName = attributeName;
        }

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
            /// <summary>
            /// Sort the entities by a specific attribute that exist in your catalogue. For instance "price".
            /// </summary>
            Attribute,

            /// <summary>
            /// Sort the entities by id.
            /// </summary>
            Id,

            /// <summary>
            /// Sort the entities by type. Type can vary. But in most cases it is "Product" but it can also be "Content" depending on setup.
            /// </summary>
            Type,

            /// <summary>
            /// Sort the entities by their relevance. For instance the context of a search hit.
            /// </summary>
            Relevance,

            /// <summary>
            /// Sort the entities by their popularity.
            /// </summary>
            Popularity
        }
    }
}
