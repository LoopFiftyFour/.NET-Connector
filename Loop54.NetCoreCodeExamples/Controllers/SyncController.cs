using Loop54.Http;
using Loop54.Model.Request;
using Loop54.Model.Response;
using Microsoft.AspNetCore.Mvc;

namespace Loop54.NetCoreCodeExamples.Controllers
{
    public class SyncController : Controller
    {
        public IActionResult Index()
        {
            SyncExample();

            return View();
        }

        private void SyncExample()
        {
            // CODE SAMPLE sync BEGIN
            //create a client with a null client info provider (because we don't need user context when syncing)
            ILoop54Client client = new Loop54Client(new RequestManager(new Loop54Settings("https://helloworld.54proxy.com", "TestApiKey")),
                new NullClientInfoProvider());

            Response response = client.Sync();
            // CODE SAMPLE END
        }
    }
}
