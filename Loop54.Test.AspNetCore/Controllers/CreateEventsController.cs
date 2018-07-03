using System.Threading.Tasks;
using Loop54.Model;
using Loop54.Model.Request;
using Loop54.Model.Request.Parameters;
using Microsoft.AspNetCore.Mvc;

namespace Loop54.Test.AspNetCore.Controllers
{
    public class CreateEventsController : Controller
    {
        private readonly ILoop54Client _loop54Client;

        public CreateEventsController(ILoop54Client loop54Client)
        {
            _loop54Client = loop54Client;
        }
        
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(string eventType, string entityType, string entityId)
        {
            Entity entity = new Entity(entityType, entityId);

            if (eventType.Equals(Event.Click))
                await Click(entity);
            else if (eventType.Equals(Event.AddToCart))
                await AddToCart(entity);
            else if (eventType.Equals(Event.Purchase))
                await Purchase(entity);

            return View((object)"Success!");
        }

        private async Task Click(Entity entity)
        {
            await _loop54Client.CreateEventsAsync(new CreateEventsRequest(new ClickEvent(entity)));
        }

        private async Task AddToCart(Entity entity)
        {
            await _loop54Client.CreateEventsAsync(new CreateEventsRequest(new AddToCartEvent(entity)));
        }

        private async Task Purchase(Entity entity)
        {
            await _loop54Client.CreateEventsAsync(new CreateEventsRequest(new PurchaseEvent(entity)
            {
                OrderId = "66", //Optional
                Quantity = 1, //Optional
                Revenue = 249d //Optional
            }));
        }
    }
}
