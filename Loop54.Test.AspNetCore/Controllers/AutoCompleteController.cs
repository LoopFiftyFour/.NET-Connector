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
    public class AutoCompleteController : Controller
    {
        private readonly ILoop54Client _loop54Client;

        public AutoCompleteController(ILoop54Client loop54Client)
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
            AutoCompleteRequest request = new AutoCompleteRequest(query);
            request.QueriesOptions.Skip = 0;
            request.QueriesOptions.Take = 10;

            //Add sorting to the autocomplete queries. This is not mandatory.
            request.QueriesOptions.SortBy = new List<QuerySortingParameter>
            {
                new QuerySortingParameter(QuerySortingParameter.Types.Alphabetic, SortOrders.Asc)
            };
            
            AutoCompleteResponse response = await _loop54Client.AutoCompleteAsync(request);

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
