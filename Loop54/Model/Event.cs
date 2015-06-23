
namespace Loop54.Model
{
    public class Event
    {
        /// <summary>
        /// Type of event, for instance "click", "addtocart" or "purchase". Not case-sensitive.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Use this property if the event was initiated on a string.
        /// </summary>
        public string String { get; set; }

        /// <summary>
        /// Use this property if the event was initiated on an entity.
        /// </summary>
        public Entity Entity { get; set; }

        /// <summary>
        /// The revenue from each item in the event. 
        /// </summary>
        public double Revenue { get; set; }

        /// <summary>
        /// If the event was part of an order or group, set the order Id here.
        /// </summary>
        public string OrderId{ get; set; }

        /// <summary>
        /// If the event involved multiple identical strings or entities, set the quantity here. Defaults to 1.
        /// </summary>
        public int Quantity { get; set; }

        public Event()
        {
            Quantity = 1;
        }
    }
}
