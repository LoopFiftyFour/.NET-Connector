using Loop54.AspNet;
using System.Web.Mvc;
using System.Web.Routing;

namespace Loop54.Test.AspNetMvc
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            //Will configure the client to point to the provided endpoint. Will thereafter serve a singleton 
            //instance when calling Client method. If calling StartUp multiple times, a new instance of the 
            //client will be created each time.
            Loop54ClientManager.StartUp("https://helloworld.54proxy.com");

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}
