using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Loop54.Model.Request;
using Loop54.Model.Request.Parameters;
using Loop54.Model.Response;
using Loop54.Test.AspNetCore.Models;
using Microsoft.AspNetCore.Mvc;

namespace Loop54.Test.AspNetCore.Controllers
{
    public class GetRelatedEntitiesController : Controller
    {
        private readonly ILoop54Client _loop54Client;

        public GetRelatedEntitiesController(ILoop54Client loop54Client)
        {
            _loop54Client = loop54Client;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(string id, string type)
        {
            GetRelatedEntitiesRequest request = new GetRelatedEntitiesRequest(type, id);

            //Add custom sorting to the entities. Will default to relevance.
            request.ResultsOptions.SortBy = new List<EntitySortingParameter>
            {
                new EntitySortingParameter("Price"), //Use this constructor if you wish to sort by an attribute
                new EntitySortingParameter(EntitySortingParameter.Types.Popularity)
                {
                    Order = SortOrders.Desc
                }
            };

            request.ResultsOptions.Skip = 0;
            request.ResultsOptions.Take = 20;

            GetRelatedEntitiesResponse response = await _loop54Client.GetRelatedEntitiesAsync(request);

            return View(new GetRelatedEntitiesViewModel
            {
                Count = response.Results.Count,
                Results = ModelUtils.GetViewModelFromEntities(response.Results.Items),
                Facets = response.Results.Facets.Where(f => f.HasValues).Select(ModelUtils.CreateFacet).ToList(),
            });
        }
    }
}
