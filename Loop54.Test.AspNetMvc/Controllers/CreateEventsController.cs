using System.Threading.Tasks;
using System.Web.Mvc;
using Loop54.AspNet;
using Loop54.Model;
using Loop54.Model.Request;
using Loop54.Model.Request.Parameters;

namespace Loop54.Test.AspNetMvc.Controllers
{
    public class CreateEventsController : Controller
    {
        private readonly ILoop54Client _loop54Client = Loop54ClientManager.Client();
        
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string eventType, string entityType, string entityId)
        {
            Entity entity = new Entity(entityType, entityId);

            if (eventType.Equals(Event.Click))
                 Click(entity);
            else if (eventType.Equals(Event.AddToCart))
                 AddToCart(entity);
            else if (eventType.Equals(Event.Purchase))
                 Purchase(entity);

            return View((object)"Success!");
        }

        private void Click(Entity entity)
        {
            _loop54Client.CreateEvents(new CreateEventsRequest(new ClickEvent(entity)));
        }

        private void AddToCart(Entity entity)
        {
            _loop54Client.CreateEvents(new CreateEventsRequest(new AddToCartEvent(entity)));
        }

        private void Purchase(Entity entity)
        {
            _loop54Client.CreateEvents(new CreateEventsRequest(new PurchaseEvent(entity)
            {
                OrderId = "66", //Optional
                Quantity = 1, //Optional
                Revenue = 249d //Optional
            }));
        }
    }
}
