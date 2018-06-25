using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Loop54.Model;
using Loop54.Model.Request;
using Loop54.Model.Request.Parameters;
using Loop54.Model.Response;
using Loop54.Serialization;
using Loop54.Test.AspNetCore.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Loop54.Test.AspNetCore.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILoop54Client _loop54Client;

        public HomeController(ILoop54Client loop54Client)
        {
            _loop54Client = loop54Client;
        }

        // GET: /<controller>/
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(string query)
        {
            SearchRequest request = new SearchRequest(query);
            request.ResultsOptions.AddDistinctFacet<string>("Manufacturer");
            request.ResultsOptions.AddDistinctFacet<string>("Organic");
            request.ResultsOptions.AddDistinctFacet<string>("Category");
            request.ResultsOptions.AddRangeFacet<double>("Price");
            request.ResultsOptions.Skip = 0;
            request.ResultsOptions.Take = 10;

            SearchResponse response = await _loop54Client.SearchAsync(request);

            return View(new SearchViewModel
            {
                Query = response.Query,
                MakesSense = response.MakesSense,
                Count = response.Results.Count,
                Results = GetViewModelFromEntities(response.Results.Items),
                Facets = response.Results.Facets.Where(f => f.HasValues).Select(CreateFacet).ToList(),
                RelatedCount = response.RelatedResults.Count,
                RelatedResults = GetViewModelFromEntities(response.RelatedResults.Items),
                SpellingSuggestions = response.SpellingSuggestions.Count > 0 ? response.SpellingSuggestions.Items.Select(i => i.Query).ToList() : null,
                RelatedQueries = response.RelatedQueries.Count > 0 ? response.RelatedQueries.Items.Select(i => i.Query).ToList() : null,
            });
        }
        
        private FacetViewModel CreateFacet(Facet responseFacet)
        {
            if(responseFacet.Type == FacetType.Distinct)
            {
                DistinctFacet dist = responseFacet.AsDistinct();
                return new DistinctFacetViewModel
                {
                    Name = dist.Name,
                    Options = dist.Items.ToDictionary(key => key.GetItem<string>(), value => value.Count)
                };
            }
            else
            {
                RangeFacet range = responseFacet.AsRange();
                return new RangeFacetViewModel
                {
                    Name = range.Name,
                    Min = range.GetMin<string>(),
                    Max = range.GetMax<string>()
                };
            }
        }

        private IList<ProductViewModel> GetViewModelFromEntities(IList<Entity> entities)
        {
            return entities.Select(e => new ProductViewModel
            {
                ImageUrl = e.GetAttributeValueOrDefault<string>("ImageUrl"),
                Name = e.GetAttributeValueOrDefault<string>("Title"),
                Price = e.GetAttributeValueOrDefault<double>("Price"),
                Manufacturer = e.GetAttributeValueOrDefault<string>("Manufacturer"),
                Category = e.GetAttributeValueOrDefault<string>("Category")
            }).ToList();
        }

        [HttpGet]
        public IActionResult Error()
        {
            return View(new ErrorViewModel(Activity.Current?.Id ?? HttpContext.TraceIdentifier));
        }
    }
}
