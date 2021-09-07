using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Loop54.AspNet;
using Loop54.Model.Request;
using Loop54.Model.Request.Parameters;
using Loop54.Model.Response;
using Loop54.Test.AspNetMvc.Models;

namespace Loop54.Test.AspNetMvc.Controllers
{
    public class GetRelatedEntitiesController : Controller
    {
        private readonly ILoop54Client _loop54Client = Loop54ClientManager.Client();
        
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string id, string type, string relationKind)
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

            // set relation kind
            RelationKinds relKind;
            if(Enum.TryParse(relationKind, true, out relKind))
                request.RelationKind = relKind;
            
            GetRelatedEntitiesResponse response = _loop54Client.GetRelatedEntities(request);

            return View(new GetRelatedEntitiesViewModel
            {
                Count = response.Results.Count,
                Results = ModelUtils.GetViewModelFromEntities(response.Results.Items),
                Facets = response.Results.Facets.Where(f => f.HasValues).Select(ModelUtils.CreateFacet).ToList(),
            });
        }
    }
}
