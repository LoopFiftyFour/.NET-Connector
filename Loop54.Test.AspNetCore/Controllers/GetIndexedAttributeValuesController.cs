using System.Threading.Tasks;
using Loop54.Model.Request;
using Loop54.Test.AspNetCore.Models;
using Microsoft.AspNetCore.Mvc;

namespace Loop54.Test.AspNetCore.Controllers
{
    public class GetIndexedAttributeValuesController : Controller
    {
        private readonly ILoop54Client _loop54Client;

        public GetIndexedAttributeValuesController(ILoop54Client loop54Client)
        {
            _loop54Client = loop54Client;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(string attributeName)
        {
            var response = await _loop54Client.GetIndexedAttributeValuesAsync(new GetIndexedAttributeValuesRequest(attributeName));

            return View(new GetIndexedAttributeValuesViewModel
            {
                Values = response.Values,
            });
        }
    }
}
