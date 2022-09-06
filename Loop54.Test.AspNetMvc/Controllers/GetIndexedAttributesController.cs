using System.Web.Mvc;
using Loop54.AspNet;
using Loop54.Model.Request;
using Loop54.Test.AspNetMvc.Models;

namespace Loop54.Test.AspNetMvc.Controllers
{
    public class GetIndexedAttributesController : Controller
    {
        private readonly ILoop54Client _loop54Client = Loop54ClientManager.Client();

        [HttpGet]
        public ActionResult Index()
        {
            var response = _loop54Client.GetIndexedAttributes(new GetIndexedAttributesRequest());

            return View(new GetIndexedAttributesViewModel
            {
                Attributes = response.Attributes,
                IndexedAttributes = response.IndexedAttributes
            });
        }
    }
}
