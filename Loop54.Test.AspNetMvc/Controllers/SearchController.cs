using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Loop54.AspNet;
using Loop54.Model.Request;
using Loop54.Model.Response;
using Loop54.Test.AspNetMvc.Models;

namespace Loop54.Test.AspNetMvc.Controllers
{
    public class SearchController : Controller
    {
        private readonly ILoop54Client _loop54Client = Loop54ClientManager.Client();
        
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string query)
        {
            SearchRequest request = new SearchRequest(query);
            request.ResultsOptions.Skip = 0;
            request.ResultsOptions.Take = 10;

            //Add facets to the search request
            request.ResultsOptions.AddDistinctFacet<string>("Manufacturer");
            request.ResultsOptions.AddDistinctFacet<string>("Organic");
            request.ResultsOptions.AddDistinctFacet<string>("Category");
            request.ResultsOptions.AddRangeFacet<double>("Price");
            
            SearchResponse response = _loop54Client.Search(request);

            return View(new SearchViewModel
            {
                Query = response.Query,
                MakesSense = response.MakesSense,
                Count = response.Results.Count,
                Results = ModelUtils.GetViewModelFromEntities(response.Results.Items),
                Facets = response.Results.Facets.Where(f => f.HasValues).Select(ModelUtils.CreateFacet).ToList(),
                RelatedCount = response.RelatedResults.Count,
                RelatedResults = ModelUtils.GetViewModelFromEntities(response.RelatedResults.Items),
                SpellingSuggestions = response.SpellingSuggestions.Count > 0 ? response.SpellingSuggestions.Items.Select(i => i.Query).ToList() : null,
                RelatedQueries = response.RelatedQueries.Count > 0 ? response.RelatedQueries.Items.Select(i => i.Query).ToList() : null,
            });
        }

        [HttpGet]
        public ActionResult WithCustomData()
        {
            return View("Index");
        }

        [HttpPost]
        public ActionResult WithCustomData(string query)
        {
            SearchRequest request = new SearchRequest(query);
            request.ResultsOptions.Skip = 0;
            request.ResultsOptions.Take = 10;

            //Add facets to the search request
            request.ResultsOptions.AddDistinctFacet<string>("Manufacturer");
            request.ResultsOptions.AddDistinctFacet<string>("Organic");
            request.ResultsOptions.AddDistinctFacet<string>("Category");
            request.ResultsOptions.AddRangeFacet<double>("Price");

            //Add custom data to the request
            //What custom data that is available to you may vary depending on your price package.
            //Please contact customer support for more information.
            request.AddCustomData("message", "ping");

            SearchResponse response = _loop54Client.Search(request);

            //Get the custom data from the response
            //This method can deserialize complex types as well, for instance a EntityCollection if doing content search. 
            //Again, contact customer support for more information.
            string responseMessage = response.GetCustomDataOrDefault<string>("responseMessage");

            return View("Index", new SearchViewModel
            {
                ResponseMessage = responseMessage,
                Query = response.Query,
                MakesSense = response.MakesSense,
                Count = response.Results.Count,
                Results = ModelUtils.GetViewModelFromEntities(response.Results.Items),
                Facets = response.Results.Facets.Where(f => f.HasValues).Select(ModelUtils.CreateFacet).ToList(),
                RelatedCount = response.RelatedResults.Count,
                RelatedResults = ModelUtils.GetViewModelFromEntities(response.RelatedResults.Items),
                SpellingSuggestions = response.SpellingSuggestions.Count > 0 ? response.SpellingSuggestions.Items.Select(i => i.Query).ToList() : null,
                RelatedQueries = response.RelatedQueries.Count > 0 ? response.RelatedQueries.Items.Select(i => i.Query).ToList() : null,
            });
        }
    }
}
