using System.Linq;
using System.Threading.Tasks;
using Loop54.Model.Request;
using Loop54.Model.Request.Parameters.Filters;
using Loop54.Model.Response;
using Loop54.Test.AspNetCore.Models;
using Microsoft.AspNetCore.Mvc;

namespace Loop54.Test.AspNetCore.Controllers
{
    public class GetEntitiesByAttributeController : Controller
    {
        private readonly ILoop54Client _loop54Client;

        public GetEntitiesByAttributeController(ILoop54Client loop54Client)
        {
            _loop54Client = loop54Client;
        }
        
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(string category, bool organic)
        {
            GetEntitiesByAttributeRequest request = new GetEntitiesByAttributeRequest("Category", category);

            //We only want products that have the organic attribute and that is either "True" or "False"
            request.ResultsOptions.Filter = new AndFilterParameter(
                new AttributeExistsFilterParameter("Organic"),
                //Because the organic attribute is stored as a string in the engine we need to filter with that type.
                //If it would have been stored as a boolean we would have used bool instead.
                new AttributeFilterParameter<string>("Organic", organic ? "True" : "False"));

            request.ResultsOptions.AddDistinctFacet<string>("Manufacturer");
            request.ResultsOptions.AddRangeFacet<double>("Price");
            request.ResultsOptions.Skip = 0;
            request.ResultsOptions.Take = 20;

            GetEntitiesByAttributeResponse response = await _loop54Client.GetEntitiesByAttributeAsync(request);

            return View(new GetEntitiesByAttributeViewModel
            {
                Count = response.Results.Count,
                Results = ModelUtils.GetViewModelFromEntities(response.Results.Items),
                Facets = response.Results.Facets.Where(f => f.HasValues).Select(ModelUtils.CreateFacet).ToList(),
            });
        }
    }
}
