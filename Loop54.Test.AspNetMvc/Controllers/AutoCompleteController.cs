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
    public class AutoCompleteController : Controller
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
            AutoCompleteRequest request = new AutoCompleteRequest(query);
            request.QueriesOptions.Skip = 0;
            request.QueriesOptions.Take = 10;

            //Add sorting to the autocomplete queries. This is not mandatory.
            request.QueriesOptions.SortBy = new List<QuerySortingParameter>
            {
                new QuerySortingParameter(QuerySortingParameter.Types.Alphabetic, SortOrders.Asc)
            };
            
            AutoCompleteResponse response = _loop54Client.AutoComplete(request);

            return View(new AutoCompleteViewModel
            {
                Count = response.Queries.Count,
                Results = response.Queries.Items.Select(q => q.Query).ToList(),
                ScopedQuery = response.ScopedQuery?.Query,
                ScopeAttribute = response.ScopedQuery?.ScopeAttributeName,
                Scopes = response.ScopedQuery?.Scopes ?? new List<string>()
            });
        }
    }
}
