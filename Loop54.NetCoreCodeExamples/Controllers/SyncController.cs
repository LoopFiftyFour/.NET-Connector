using System.Diagnostics;
using Loop54.Http;
using Loop54.Model;
using Loop54.Model.Request;
using Loop54.Model.Response;
using Loop54.Model.Response.Sync;
using Microsoft.AspNetCore.Mvc;

namespace Loop54.NetCoreCodeExamples.Controllers
{
    public class SyncController : Controller
    {
        public IActionResult Index()
        {
            SyncExample();
            SyncDeltaPushExample();
            GetSyncStateExample();

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

        private void SyncDeltaPushExample()
        {
            // CODE SAMPLE sync-delta-push BEGIN
            //create a client with a null client info provider (because we don't need user context when syncing)
            ILoop54Client client = new Loop54Client(new RequestManager(new Loop54Settings("https://helloworld.54proxy.com", "TestApiKey")),
                new NullClientInfoProvider());

            var request = new SyncDeltaPushRequest();

            //add a new product, or update the given attributes of an existing one (other attributes are left unchanged)
            request.ToAddOrUpdate.Add(new Entity("Product", "12")
                .SetAttribute("Price", 13.37)
                .SetAttribute("Category", "Meat", "Pork"));

            //add a new product, or completely replace an existing one
            request.ToReplaceOrAdd.Add(new Entity("Product", "13")
                .SetAttribute("Title", "Smoked pork belly")
                .SetAttribute("InStock", true));

            //remove a product (only the type and id are needed)
            request.ToDelete.Add(new Entity("Product", "14"));

            //remove attributes from a product (only the attribute names are needed)
            request.ToRemoveAttributesFrom.Add(new Entity("Product", "15").SetAttribute<string>("Campaign"));

            //the changes are queued in the engine and applied asynchronously, use GetSyncState to follow their progress
            Response response = client.SyncDeltaPush(request);
            // CODE SAMPLE END
        }

        private void GetSyncStateExample()
        {
            // CODE SAMPLE get-sync-state BEGIN
            //create a client with a null client info provider (because we don't need user context when syncing)
            ILoop54Client client = new Loop54Client(new RequestManager(new Loop54Settings("https://helloworld.54proxy.com", "TestApiKey")),
                new NullClientInfoProvider());

            GetSyncStateResponse response = client.GetSyncState();

            //the latest outcome of the pushes made with SyncDeltaPush (null if none has been applied or failed since the engine started)
            var pushState = response.GetByCategory(SyncTriggeredFrom.ExternalEndpointSyncDeltaPush);

            if (pushState?.LatestSuccess != null)
                Debug.WriteLine($"Latest push applied at {pushState.LatestSuccess.AppliedAtUtc:u}: " +
                    $"{pushState.LatestSuccess.ChangedOrAddedCount} changed or added, {pushState.LatestSuccess.RemovedCount} removed");

            if (pushState?.LatestFailure != null)
                Debug.WriteLine($"Latest push failed at {pushState.LatestFailure.FailedAtUtc:u}");

            //all categories, for instance the pulls (with source IDs such as "full" or "delta") started by the engine itself or by Sync
            foreach (var state in response.ByCategory)
                Debug.WriteLine($"{state.Category.TriggeredFrom} {state.Category.PullSourceId}: " +
                    $"latest success {state.LatestSuccess?.AppliedAtUtc:u}, latest failure {state.LatestFailure?.FailedAtUtc:u}");

            foreach (var sync in response.InProgress)
                Debug.WriteLine($"Sync from {sync.Category.TriggeredFrom} started at {sync.StartedAtUtc:u} is {sync.State}");

            Debug.WriteLine($"{response.InProgressCount} syncs in progress");
            // CODE SAMPLE END
        }
    }
}
