using Loop54.Model;
using Loop54.Model.Request;
using Loop54.Model.Request.Parameters;
using Loop54.User;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;

namespace Loop54.NetCoreCodeExamples.Controllers
{
    public class EventTrackingController : Controller
    {
        private readonly ILoop54Client _loop54Client;

        public EventTrackingController(ILoop54Client loop54Client)
        {
            _loop54Client = loop54Client;
        }

        [HttpGet]
        public ActionResult Index()
        {
            //Setup
            string productId = "1";

            // Code examples
            Entity clickedEntity = CreateEventsExample(productId);
            CustomerUserIDExample(clickedEntity);

            return View();
        }

        #region CodeExamples
        private Entity CreateEventsExample(string productId)
        {
            Debug.WriteLine("create-events: " + Environment.NewLine);

            // CODE SAMPLE create-events BEGIN
            // Code examples

            //click event (can be called on the product page)
            Entity clickedEntity = new Entity("Product", productId);
            _loop54Client.CreateEvents(new CreateEventsRequest(new ClickEvent(clickedEntity)));

            //addtocart event (call this when a customer adds a product to cart)
            Entity addToCartEntity = new Entity("Product", productId);
            _loop54Client.CreateEvents(new CreateEventsRequest(new AddToCartEvent(addToCartEntity)));

            //purchase events (can be called when an order is processed, or on the "thank you" page)  
            Entity purchasedEntity = new Entity("Product", productId);
            _loop54Client.CreateEvents(new CreateEventsRequest(new PurchaseEvent(purchasedEntity)
            {
                OrderId = "13t09j1g", //Optional but recommended
                Quantity = 5, //Optional
                Revenue = 249d //Optional
            }
            ));
            // CODE SAMPLE END

            Debug.WriteLine("create-events (end) " + Environment.NewLine);

            return clickedEntity;
        }

        private void CustomerUserIDExample(Entity clickedEntity)
        {
            Debug.WriteLine("create-events-custom-user-id: " + Environment.NewLine);

            // CODE SAMPLE create-events-custom-user-id BEGIN
            // How it works

            //click event with a custom user ID
            _loop54Client.CreateEvents(new CreateEventsRequest(new ClickEvent(clickedEntity)).Wrap(new UserMetaData("testUserID")));
            // CODE SAMPLE END

            Debug.WriteLine("create-events-custom-user-id (end) " + Environment.NewLine);
        }
        #endregion

        #region HelperMethods
        #endregion
    }
}
