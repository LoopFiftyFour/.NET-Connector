using Loop54.Model.Request;
using Loop54.Model.Response;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;
using System.Linq;

namespace Loop54.NetCoreCodeExamples.Controllers
{
    public class AutoCompleteController : Controller
    {
        private readonly ILoop54Client _loop54Client;

        public AutoCompleteController(ILoop54Client loop54Client)
        {
            _loop54Client = loop54Client;
        }

        [HttpGet]
        public ActionResult Index()
        {
            // Setup
            string query = "a";

            // Code examples
            AutoCompleteExample(query);

            return View();
        }

        #region CodeExamples
        private void AutoCompleteExample(string query)
        {
            Debug.WriteLine("autocomplete-full: " + Environment.NewLine);

            // CODE SAMPLE autocomplete-full BEGIN
            // Below is an example of a request - response cycle of an autocomplete request
            AutoCompleteRequest request = new AutoCompleteRequest(query);
            request.QueriesOptions.Skip = 0;
            request.QueriesOptions.Take = 9;
            AutoCompleteResponse response = _loop54Client.AutoComplete(request);

            var queries = response.Queries.Items.Select(q => q.Query).ToList();

            //print out all suggested autocomplete queries
            Debug.WriteLine("queries: " + string.Join(", ", queries));
            // CODE SAMPLE END

            Debug.WriteLine("autocomplete-full (end) " + Environment.NewLine);
        }

        private void ScopedAutoCompleteExample(string query)
        {
            Debug.WriteLine("autocomplete-scoped: " + Environment.NewLine);

            // CODE SAMPLE autocomplete-scoped BEGIN
            // Below is an example of a request - response cycle of an autocomplete request
            // where scopes are used to provide the user with more context
            AutoCompleteRequest request = new AutoCompleteRequest(query);
            request.QueriesOptions.Skip = 0;
            request.QueriesOptions.Take = 9;
            AutoCompleteResponse response = _loop54Client.AutoComplete(request);

            //prints out the scoped suggestions
            if(response.ScopedQuery != null)
            {
                Debug.WriteLine("scoped query: " + response.ScopedQuery.Query);
                Debug.WriteLine("scopes based on: " + response.ScopedQuery.ScopeAttributeName);
                Debug.WriteLine("scopes: " + string.Join(", ", response.ScopedQuery.Scopes));
            }
            // CODE SAMPLE END

            Debug.WriteLine("autocomplete-scoped (end) " + Environment.NewLine);
        }
        #endregion
    }
}