using Microsoft.AspNetCore.Mvc;

namespace Loop54.NetCoreCodeExamples.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }     
    }
}
