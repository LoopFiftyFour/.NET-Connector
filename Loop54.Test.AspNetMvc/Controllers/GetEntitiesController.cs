using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Loop54.AspNet;
using Loop54.Model.Request;
using Loop54.Model.Request.Parameters;
using Loop54.Model.Request.Parameters.Filters;
using Loop54.Model.Response;
using Loop54.Test.AspNetMvc.Models;

namespace Loop54.Test.AspNetMvc.Controllers
{
    public class GetEntitiesController : Controller
    {
        private readonly ILoop54Client _loop54Client = Loop54ClientManager.Client();
        
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(bool? organic)
        {
            GetEntitiesRequest request = new GetEntitiesRequest();
            request.ResultsOptions.SortBy = new List<EntitySortingParameter>
            {
                new EntitySortingParameter("Price"), //Use this constructor if you wish to sort by an attribute
                new EntitySortingParameter("Title")
                {
                    Order = SortOrders.Desc
                }
            };

            request.ResultsOptions.Filter = new AndFilterParameter(
                new AttributeExistsFilterParameter("Organic"),
                //Because the organic attribute is stored as a string in the engine we need to filter with that type.
                //If it would have been stored as a boolean we would have used bool instead.
                new AttributeFilterParameter<string>("Organic", organic.HasValue && organic.Value ? "True" : "False"));

            request.ResultsOptions.AddDistinctFacet<string>("Manufacturer");
            request.ResultsOptions.AddDistinctFacet<string>("Organic");
            request.ResultsOptions.AddDistinctFacet<string>("Category");
            request.ResultsOptions.AddRangeFacet<double>("Price");
            request.ResultsOptions.Skip = 0;
            request.ResultsOptions.Take = 20;

            GetEntitiesResponse response = _loop54Client.GetEntities(request);

            return View(new GetEntitiesViewModel
            {
                Count = response.Results.Count,
                Results = ModelUtils.GetViewModelFromEntities(response.Results.Items),
                Facets = response.Results.Facets.Where(f => f.HasValues).Select(ModelUtils.CreateFacet).ToList(),
            });
        }
    }
}
