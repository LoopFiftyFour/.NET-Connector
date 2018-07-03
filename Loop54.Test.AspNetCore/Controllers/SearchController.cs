using System.Linq;
using System.Threading.Tasks;
using Loop54.Model.Request;
using Loop54.Model.Response;
using Loop54.Test.AspNetCore.Models;
using Microsoft.AspNetCore.Mvc;

namespace Loop54.Test.AspNetCore.Controllers
{
    public class SearchController : Controller
    {
        private readonly ILoop54Client _loop54Client;

        public SearchController(ILoop54Client loop54Client)
        {
            _loop54Client = loop54Client;
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
