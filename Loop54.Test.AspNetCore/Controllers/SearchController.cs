using System.Linq;
using System.Threading.Tasks;
using Loop54.AspNet;
using Loop54.Model.Request;
using Loop54.Model.Response;
using Loop54.Test.AspNetCore.Models;
using Microsoft.AspNetCore.Mvc;

namespace Loop54.Test.AspNetCore.Controllers
{
    public class SearchController : Controller
    {
        private readonly ILoop54Client _loop54Client;

        public SearchController(ILoop54ClientProvider loop54ClientProvider)
        {
            _loop54Client = loop54ClientProvider.GetNamed("English"); //'English' is the same name we gave the instance in startup.cs
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(string query)
        {
            SearchRequest request = new SearchRequest(query);
            request.ResultsOptions.Skip = 0;
            request.ResultsOptions.Take = 10;

            //Add facets to the search request
            request.ResultsOptions.AddDistinctFacet<string>("Manufacturer");
            request.ResultsOptions.AddDistinctFacet<string>("Organic");
            request.ResultsOptions.AddDistinctFacet<string>("Category");
            request.ResultsOptions.AddRangeFacet<double>("Price");
            
            SearchResponse response = await _loop54Client.SearchAsync(request);

            return View(new SearchViewModel
            {
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
        public IActionResult WithCustomData()
        {
            return View("Index");
        }

        [HttpPost]
        public async Task<IActionResult> WithCustomData(string query)
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

            SearchResponse response = await _loop54Client.SearchAsync(request);

            //Get the custom data from the response
            //This method can deserialize complex types as well, for instance a EntityCollection if doing content search. 
            //Again, contact customer support for more information.
            string responseMessage = response.GetCustomDataOrDefault<string>("responseMessage");
            
            return View("Index", new SearchViewModel
            {
                ResponseMessage = responseMessage,
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
