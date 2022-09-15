using System.Threading.Tasks;
using Loop54.Model.Request;
using Loop54.Test.AspNetCore.Models;
using Microsoft.AspNetCore.Mvc;

namespace Loop54.Test.AspNetCore.Controllers
{
    public class GetIndexedAttributesController : Controller
    {
        private readonly ILoop54Client _loop54Client;

        public GetIndexedAttributesController(ILoop54Client loop54Client)
        {
            _loop54Client = loop54Client;
        }
        
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var response = await _loop54Client.GetIndexedAttributesAsync(new GetIndexedAttributesRequest());

            return View(new GetIndexedAttributesViewModel
            {
                Attributes = response.Attributes,
                IndexedAttributes = response.IndexedAttributes
            });
        }
    }
}
