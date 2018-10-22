using Loop54.Model.Request;
using Loop54.Model.Request.Parameters.Facets;
using Loop54.Model.Response;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Loop54.NetCoreCodeExamples.Controllers
{
    public class FacetingController : Controller
    {
        private readonly ILoop54Client _loop54Client;

        public FacetingController(ILoop54Client loop54Client)
        {
            _loop54Client = loop54Client;
        }

        [HttpGet]
        public ActionResult Index()
        {
            // Setup
            string query = "a";
            string specificManufacturer = "MeatNStuff";

            // Code examples
            FacetingSingleFacetExample(query);
            FacetingMultipleFacetsExample(query);
            FacetingEngineResponseExample(query);
            FacetingDistinctFacetExample(query, specificManufacturer);
            FacetingRangeFacetExample(query);

            return View();
        }

        #region CodeExamples
        private void FacetingSingleFacetExample(string query)
        {
            Debug.WriteLine("faceting-single-facet: " + Environment.NewLine);
            Debug.WriteLine("items: ");

            // CODE SAMPLE faceting-single-facet BEGIN
            // Search with a single facet
            SearchRequest request = new SearchRequest(query);
            //Add facets to the search request 
            request.ResultsOptions.AddDistinctFacet<string>("Category");
            SearchResponse response = _loop54Client.Search(request);
            // CODE SAMPLE END

            RenderItems(response);
            RenderFacets(response);

            Debug.WriteLine("faceting-single-facet (end) " + Environment.NewLine);
        }

        private void FacetingMultipleFacetsExample(string query)
        {
            Debug.WriteLine("faceting-multiple-facets: " + Environment.NewLine);
            Debug.WriteLine("items: ");

            // CODE SAMPLE faceting-multiple-facets BEGIN
            // Search with multiple facets
            SearchRequest request = new SearchRequest(query);
            //Add facets to the search request 
            request.ResultsOptions.AddDistinctFacet<string>("Manufacturer");
            request.ResultsOptions.AddDistinctFacet<string>("Category");
            SearchResponse response = _loop54Client.Search(request);
            // CODE SAMPLE END

            RenderItems(response);
            RenderFacets(response);

            Debug.WriteLine("faceting-multiple-facet (end) " + Environment.NewLine);
        }

        private void FacetingEngineResponseExample(string query)
        {
            Debug.WriteLine("faceting-engine-response: " + Environment.NewLine);
            Debug.WriteLine("items: ");

            SearchRequest request = new SearchRequest(query);
            //Add facets to the search request 
            request.ResultsOptions.AddDistinctFacet<string>("Manufacturer");
            request.ResultsOptions.AddDistinctFacet<string>("Category");
            SearchResponse response = _loop54Client.Search(request);

            RenderItems(response);
            
            RenderFacets(response);

            Debug.WriteLine("faceting-engine-response (end) " + Environment.NewLine);
        }

        private void FacetingDistinctFacetExample(string query, string specificManufacturer)
        {
            Debug.WriteLine("faceting-distinct-facet: " + Environment.NewLine);
            Debug.WriteLine("items: ");

            // CODE SAMPLE faceting-distinct-facet BEGIN
            // Search with a distinct facet applied
            // The use-case here is e.g. when the user clicks on a specific manufacturer in the search result facet list
            SearchRequest request = new SearchRequest(query);

            //Add facets to the search request
            //And select a specific facet value to filter on
            request.ResultsOptions.AddDistinctFacet<string>("Manufacturer", new List<string>() { specificManufacturer });
            request.ResultsOptions.AddDistinctFacet<string>("Organic");
            request.ResultsOptions.AddDistinctFacet<string>("Category");
            request.ResultsOptions.AddRangeFacet<double>("Price");

            SearchResponse response = _loop54Client.Search(request);
            // CODE SAMPLE END

            RenderItems(response);
            RenderFacets(response);

            Debug.WriteLine("faceting-distinct-facet (end) " + Environment.NewLine);
        }

        private void FacetingRangeFacetExample(string query)
        {
            Debug.WriteLine("faceting-range-facet: " + Environment.NewLine);
            Debug.WriteLine("items: ");

            // CODE SAMPLE faceting-range-facet BEGIN
            // Search with a range facet
            // The use-case here is e.g. when the user selects a specific price range in the search result facet list
            SearchRequest request = new SearchRequest(query);

            //Add facets to the search request
            //And select a specific range for a certain facet
            request.ResultsOptions.AddDistinctFacet<string>("Manufacturer");
            request.ResultsOptions.AddDistinctFacet<string>("Organic");
            request.ResultsOptions.AddDistinctFacet<string>("Category");
            request.ResultsOptions.AddRangeFacet<double>("Price", new RangeFacetSelectedParameter<double>() { Min = 10, Max = 60 });

            SearchResponse response = _loop54Client.Search(request);
            // CODE SAMPLE END

            RenderItems(response);
            RenderFacets(response);

            Debug.WriteLine("faceting-range-facet (end) " + Environment.NewLine);
        }
        #endregion

        #region HelperMethods
        private void RenderFacets(SearchResponse response)
        {
            // CODE SAMPLE render-distinct-facets BEGIN
            // Render distinct facets
            List<string> distinctFacetsToDisplay = new List<string>() { "Manufacturer", "Category", "Organic" };
            foreach (string attributeName in distinctFacetsToDisplay)
            {
                var facet = response.Results.GetDistinctFacetByName(attributeName);
                if (facet != null)
                {
                    var facetItems = facet.Items;
                    if (facetItems.Any())
                        Debug.WriteLine(attributeName + ": ");
                    foreach (var facetItem in facetItems)
                    {
                        Debug.WriteLine(facetItem.GetItem<string>() + ": " + facetItem.Count); // Write the facet name and the number of products in the facet 
                    }
                }
            }
            // CODE SAMPLE END

            //if there is a price range facet
            var priceFacet = response.Results.GetRangeFacetByName("Price");
            if (priceFacet != null)
            {
                Debug.WriteLine("Price: ");
                var minPrice = priceFacet.GetMin<double>();
                var maxPrice = priceFacet.GetMax<double>();
                var minPriceSelected = priceFacet.GetSelectedMin<double>();
                var maxPriceSelected = priceFacet.GetSelectedMax<double>();
                Debug.WriteLine("min: " + minPrice.ToString() + " kr, max: " + maxPrice.ToString() + 
                                " kr, min selected: " + minPriceSelected.ToString() + " kr," +
                                " max selected: " + maxPriceSelected.ToString() + " kr.");
            }
        }

        private void RenderItems(SearchResponse response)
        {
            var results = response.Results.Items;

            if (!results.Any())
                Debug.WriteLine("There were no items.");

            foreach (var resultItem in results)
            {
                var productId = resultItem.GetAttributeValueOrDefault<string>("Id");
                var productTitle = resultItem.GetAttributeValueOrDefault<string>("Title");
                var price = resultItem.GetAttributeValueOrDefault<double>("Price");
                Debug.WriteLine(productId + " " + productTitle + " (" + price + " kr), ");
            }
        }
        #endregion
    }
}
