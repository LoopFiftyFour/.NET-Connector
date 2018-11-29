using Loop54.AspNet;
using Loop54.Model.Request;
using System;
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
