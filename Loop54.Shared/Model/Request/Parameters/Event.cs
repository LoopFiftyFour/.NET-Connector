namespace Loop54.Model.Request.Parameters
{
    /// <summary>
    /// Base event class. This represents a user interaction on a product (or as we call it, entity).
    /// </summary>
    public abstract class Event
    {
        /// <summary>
        /// The string value corresponding to the click-event
        /// </summary>
        public const string Click = "click";

        /// <summary>
        /// The string value corresponding to the addtocart-event
        /// </summary>
        public const string AddToCart = "addtocart";

        /// <summary>
        /// The string value corresponding to the purchase-event
        /// </summary>
        public const string Purchase = "purchase";
        
        internal Event(string type, Entity entity)
        {
            Type = type;
            Entity = entity;
        }

        /// <summary>
        /// Type of the event.
        /// </summary>
        public string Type { get; }
        
        /// <summary>
        /// The entity that the user has interacted with.
        /// </summary>
        public Entity Entity { get; set; }
    }

    /// <summary>
    /// Event for when a user has clicked on a product.
    /// </summary>
    public class ClickEvent : Event
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Entity">The entity that the user has clicked on.</param>
        public ClickEvent(Entity Entity)
            : base(Click, Entity)
        {
        }
    }

    /// <summary>
    /// Event for when a user has added a product to the shopping cart.
    /// </summary>
    public class AddToCartEvent : Event
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Entity">The entity that the user has added to the cart.</param>
        public AddToCartEvent(Entity Entity)
            : base(AddToCart, Entity)
        {
        }
    }

    /// <summary>
    /// Event for when a user has purchased a product.
    /// </summary>
    public class PurchaseEvent : Event
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Entity">The entity that the user has purchased.</param>
        public PurchaseEvent(Entity Entity)
            : base(Purchase, Entity)
        {
        }

        /// <summary>
        /// An identifier of the purchase order. Optional.
        /// </summary>
        public string OrderId { get; set; }

        /// <summary>
        /// How much revenue the purchase may have resulted in. Optional.
        /// </summary>
        public double? Revenue { get; set; }

        /// <summary>
        /// How many items of the entity that has been purchased. Optional.
        /// </summary>
        public int? Quantity { get; set; }
    }

    /// <summary>
    /// Custom event type. Contact support before using this!
    /// </summary>
    public class CustomEvent : Event
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="type">Custom type string of the event.</param>
        /// <param name="Entity">The entity that has been </param>
        public CustomEvent(string type, Entity Entity)
            : base(type, Entity)
        {
        }
    }
}
