using Loop54.Model.Request.Parameters;
using System.Collections.Generic;

namespace Loop54.Model.Request
{
    /// <summary>
    /// This class is used to make requests to create user behaviour needed for the Loop54 e-commerce search engine to learn.
    /// 
    /// If implementing Loop54 you should send click, addtocart and purchase events.
    /// </summary>
    public class CreateEventsRequest : Request
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public CreateEventsRequest()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="evt">An event to add to the request. Can be of type <see cref="ClickEvent"/>, <see cref="AddToCartEvent"/>, <see cref="PurchaseEvent"/> or <see cref="CustomEvent"/>.</param>
        public CreateEventsRequest(Event evt)
        {
            Events.Add(evt);
        }

        /// <summary>
        /// The events to send in the request. Usually clicks and addtocarts are sent by themselves but multiple purchase events is usually sent as one request.
        /// </summary>
        public IList<Event> Events { get; set; } = new List<Event>();
    }
}
