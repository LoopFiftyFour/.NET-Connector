using System.Web.Mvc;
using Loop54.AspNet;
using Loop54.Model.Request;
using Loop54.Test.AspNetMvc.Models;

namespace Loop54.Test.AspNetMvc.Controllers
{
    public class GetIndexedAttributeValuesController : Controller
    {
        private readonly ILoop54Client _loop54Client = Loop54ClientManager.Client();

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string attributeName)
        {
            var response = _loop54Client.GetIndexedAttributeValues(new GetIndexedAttributeValuesRequest(attributeName));

            return View(new GetIndexedAttributeValuesViewModel
            {
                Values = response.Values
            });
        }
    }
}
