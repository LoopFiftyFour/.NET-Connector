using System.Web.Mvc;

namespace Loop54.Test.AspNetMvc.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
    }
}
