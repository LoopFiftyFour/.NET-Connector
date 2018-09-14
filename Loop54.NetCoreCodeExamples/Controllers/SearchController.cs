using Loop54.Model.Request;
using Loop54.Model.Response;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;
using System.Linq;

namespace Loop54.NetCoreCodeExamples.Controllers
{
    public class SearchController : Controller
    {
        private readonly ILoop54Client _loop54Client;

        public SearchController(ILoop54Client loop54Client)
        {
            _loop54Client = loop54Client;
        }

        [HttpGet]
        public ActionResult Index()
        {
            // Setup
            string query1 = "a";
            string query2 = "b";

            // Code examples
            SearchExample(query1);
            SearchCheckResultExample(query2);

            return View();
        }

        #region CodeExamples
        private void SearchExample(string query)
        {
            Debug.WriteLine("search-full: " + Environment.NewLine);

            // CODE SAMPLE search-full BEGIN
            // The search field
            //initialize "Search" request and set search query
            SearchRequest request = new SearchRequest(query);

            //specify number of response items
            request.ResultsOptions.Skip = 0;
            request.ResultsOptions.Take = 10;
            request.RelatedResultsOptions.Skip = 0;
            request.RelatedResultsOptions.Take = 9;

            //fetch response from engine
            SearchResponse response = _loop54Client.Search(request);

            // INJECT SAMPLE search-check-results BEGIN
            CheckResults(response);
            // INJECT SAMPLE END

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

            //render recommended results
            var relatedResults = response.RelatedResults.Items;
            if (relatedResults.Any())
                Debug.WriteLine("Maybe you also want these?");

            foreach (var resultItem in relatedResults)
            {
                var productId = resultItem.Id;
                var productTitle = resultItem.GetAttributeValueOrDefault<string>("Title");
                Debug.WriteLine(productId + " " + productTitle); //render a product on the search results page
            }
            // CODE SAMPLE END

            Debug.WriteLine("search-full (end) " + Environment.NewLine);
        }

        private void SearchCheckResultExample(string query)
        {
            Debug.WriteLine("search-check-result: " + Environment.NewLine);
            //initialize "Search" request and set search query
            SearchRequest request = new SearchRequest(query);

            //specify number of response items
            request.ResultsOptions.Skip = 0;
            request.ResultsOptions.Take = 10;
            request.RelatedResultsOptions.Skip = 0;
            request.RelatedResultsOptions.Take = 9;

            //fetch response from engine
            SearchResponse response = _loop54Client.Search(request);
            
            CheckResults(response);

            Debug.WriteLine("search-check-result (end) " + Environment.NewLine);
        }
        #endregion

        #region HelperMethods
        private void CheckResults(SearchResponse response)
        {
            // CODE SAMPLE search-check-results BEGIN
            // Check the search results
            //if the result does not make sense, show error message
            //(note that there may still be results!)
            if (!response.MakesSense)
                Debug.WriteLine("We did not understand your query.");

            //render spelling suggestions
            if (response.SpellingSuggestions.Count > 0)
            {
                var queries = response.SpellingSuggestions.Items.Select(o => o.Query);
                var suggestions = string.Join(", ", queries);

                Debug.WriteLine("Did you mean: " + suggestions + "?");
            }
            // CODE SAMPLE END
        }
        #endregion
    }
}
