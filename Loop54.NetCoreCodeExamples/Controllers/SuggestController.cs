using Loop54.Model.Request;
using Loop54.Model.Response;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;
using System.Linq;

namespace Loop54.NetCoreCodeExamples.Controllers
{
    public class SuggestController : Controller
    {
        private readonly ILoop54Client _loop54Client;

        public SuggestController(ILoop54Client loop54Client)
        {
            _loop54Client = loop54Client;
        }

        [HttpGet]
        public ActionResult Index()
        {
            // Setup
            string query1 = "a";
            string query2 = "b";
            string query3 = "dfshdfjsdkhfsdjkf";
            string query4 = "apple";

            // Code examples
            // Query-suggestion part of the Suggest module
            SuggestQueriesExample(query1);
            SuggestScopedExample(query1);

            // Search part of the Suggest module
            SuggestExample(query1);
            SuggestExample(query2);
            SuggestExample(query3);
            SuggestExample(query4);
            SuggestOriginalQueryExample(query1);

            return View();
        }

        #region CodeExamples
        private void SuggestQueriesExample(string query)
        {
            Debug.WriteLine("suggest-queries: " + Environment.NewLine);

            // CODE SAMPLE suggest-queries BEGIN
            // Below is an example of a request - response cycle of a suggest request
            // where only the query suggestions (autocomplete) are used
            SuggestRequest request = new SuggestRequest(query)
            {
                QueriesOptions =
                {
                    Skip = 0,
                    Take = 9
                }
            };
            SuggestResponse response = _loop54Client.Suggest(request);

            var queries = response.Queries.Items.Select(q => q.Query).ToList();

            //print out all suggested queries
            Debug.WriteLine("queries: " + string.Join(", ", queries));
            // CODE SAMPLE END

            Debug.WriteLine("suggest-queries (end) " + Environment.NewLine);
        }

        private void SuggestScopedExample(string query)
        {
            Debug.WriteLine("suggest-scoped: " + Environment.NewLine);

            // CODE SAMPLE suggest-scoped BEGIN
            // Below is an example of a request - response cycle of an suggest request
            // where scopes are used to provide the user with more context
            SuggestRequest request = new SuggestRequest(query);
            request.QueriesOptions.Skip = 0;
            request.QueriesOptions.Take = 9;
            SuggestResponse response = _loop54Client.Suggest(request);

            //prints out the scoped suggestions
            if(response.ScopedQuery != null)
            {
                Debug.WriteLine("scoped query: " + response.ScopedQuery.Query);
                Debug.WriteLine("scopes based on: " + response.ScopedQuery.ScopeAttributeName);
                Debug.WriteLine("scopes: " + string.Join(", ", response.ScopedQuery.Scopes));
            }
            // CODE SAMPLE END

            Debug.WriteLine("suggest-scoped (end) " + Environment.NewLine);
        }

        private void SuggestExample(string query)
        {
            Debug.WriteLine("suggest-full: " + Environment.NewLine);

            // CODE SAMPLE suggest-full BEGIN
            // The search field
            //initialize "suggest" request and set search query
            SuggestRequest request = new SuggestRequest(query);

            //specify number of query suggestions and response items
            request.QueriesOptions.Skip = 0;
            request.QueriesOptions.Take = 9;
            request.ResultsOptions.Skip = 0;
            request.ResultsOptions.Take = 10;

            //fetch query suggestions and results from engine in a single call
            SuggestResponse response = _loop54Client.Suggest(request);

            //render query suggestions
            var queries = response.Queries.Items.Select(q => q.Query).ToList();
            Debug.WriteLine($"queries: {string.Join(", ", queries)}");

            //if the result does not make sense, show error message
            //(note that there may still be results!)
            if (!response.MakesSense)
                Debug.WriteLine("We did not understand your query.");

            //the engine may have searched for the top query suggestion instead of the typed query
            if (response.QueryUsedForSearch != response.OriginalQuery)
                Debug.WriteLine($"Showing results for: {response.QueryUsedForSearch}");

            //render direct results
            var results = response.Results.Items;
            if (!results.Any())
                Debug.WriteLine("There were no items matching your search.");

            foreach (var resultItem in results)
            {
                var productId = resultItem.Id;
                var productTitle = resultItem.GetAttributeValueOrDefault<string>("Title");
                Debug.WriteLine(productId + " " + productTitle); //render a product on the search results page
            }
            // CODE SAMPLE END

            Debug.WriteLine($"suggest-full (end) {Environment.NewLine}");
        }

        private void SuggestOriginalQueryExample(string query)
        {
            Debug.WriteLine("suggest-original-query: " + Environment.NewLine);

            // CODE SAMPLE suggest-original-query BEGIN
            // Below is an example of an suggest request where the results are always
            // based on the query typed by the user, and never on the top query suggestion
            SuggestRequest request = new SuggestRequest(query, useTopSuggestionForSearch: false);
            request.ResultsOptions.Skip = 0;
            request.ResultsOptions.Take = 10;
            SuggestResponse response = _loop54Client.Suggest(request);

            //render direct results
            foreach (var resultItem in response.Results.Items)
            {
                var productId = resultItem.Id;
                var productTitle = resultItem.GetAttributeValueOrDefault<string>("Title");
                Debug.WriteLine(productId + " " + productTitle);
            }
            // CODE SAMPLE END

            Debug.WriteLine("suggest-original-query (end) " + Environment.NewLine);
        }
        #endregion
    }
}
