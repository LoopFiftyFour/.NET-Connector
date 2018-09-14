using Loop54.Model.Request;
using Loop54.Model.Request.Parameters;
using Loop54.Model.Request.Parameters.Facets;
using Loop54.Model.Request.Parameters.Filters;
using Loop54.Model.Response;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Loop54.NetCoreCodeExamples.Controllers
{
    public class CategoryListingController : Controller
    {
        private readonly ILoop54Client _loop54Client;

        public CategoryListingController(ILoop54Client loop54Client)
        {
            _loop54Client = loop54Client;
        }

        [HttpGet]
        public ActionResult Index()
        {
            // Setup
            string categoryName = "meat";
            string specificManufacturer = "MeatNStuff";

            // Code examples
            CategoryListingExample(categoryName);
            CategoryListingFacetsExample(categoryName);
            CategoryListingDistinctFacetExample(categoryName, specificManufacturer);
            CategoryListingRangeFacetExample(categoryName);
            CategoryListingSortingExample(categoryName);
            CategoryListingFilterExample(categoryName);

            return View();
        }

        #region CodeExamples
        private void CategoryListingExample(string categoryName)
        {
            Debug.WriteLine("categorylisting-full: " + Environment.NewLine);
            Debug.WriteLine("items: ");

            // CODE SAMPLE categorylisting-full BEGIN
            // Below is an example of a request - response cycle of a category listing request
            var request = new GetEntitiesByAttributeRequest("Category", categoryName);
            request.ResultsOptions.Skip = 0;
            request.ResultsOptions.Take = 9;

            var response = _loop54Client.GetEntitiesByAttribute(request);

            // INJECT SAMPLE render-items BEGIN
            RenderItems(response);
            // INJECT SAMPLE END
            // CODE SAMPLE END

            Debug.WriteLine("categorylisting-full (end) " + Environment.NewLine);
        }

        private void CategoryListingFacetsExample(string categoryName)
        {
            Debug.WriteLine("categorylisting-facets: " + Environment.NewLine);
            Debug.WriteLine("items: ");

            // CODE SAMPLE categorylisting-facets BEGIN
            // Category listing with facets
            var request = new GetEntitiesByAttributeRequest("Category", categoryName);

            //Add facets to the request 
            request.ResultsOptions.AddDistinctFacet<string>("Manufacturer");
            request.ResultsOptions.AddDistinctFacet<string>("Category");
            request.ResultsOptions.AddDistinctFacet<string>("Organic");
            request.ResultsOptions.AddRangeFacet<double>("Price");

            var response = _loop54Client.GetEntitiesByAttribute(request);

            // INJECT SAMPLE render-items BEGIN
            RenderItems(response);
            // INJECT SAMPLE END
            // INJECT SAMPLE render-facets BEGIN
            RenderFacets(response);
            // INJECT SAMPLE END
            // CODE SAMPLE END

            Debug.WriteLine("categorylisting-facets (end) " + Environment.NewLine);
        }

        private void CategoryListingDistinctFacetExample(string categoryName, string specificManufacturer)
        {
            Debug.WriteLine("categorylisting-distinct-facet: " + Environment.NewLine);
            Debug.WriteLine("items: ");

            // CODE SAMPLE categorylisting-distinct-facet BEGIN
            // Category listing with a distinct facet applied
            // The use-case here is e.g. when the user clicks on a specific manufacturer in the category listing facet list
            var request = new GetEntitiesByAttributeRequest("Category", categoryName);

            //Add facets to the request 
            //And select a specific facet value to filter on
            request.ResultsOptions.AddDistinctFacet<string>("Manufacturer", new List<string>() { specificManufacturer });
            request.ResultsOptions.AddDistinctFacet<string>("Category");
            request.ResultsOptions.AddDistinctFacet<string>("Organic");
            request.ResultsOptions.AddRangeFacet<double>("Price");

            var response = _loop54Client.GetEntitiesByAttribute(request);
            // CODE SAMPLE END

            RenderItemsExtended(response);
            RenderFacets(response);
            Debug.WriteLine("categorylisting-distinct-facet (end) " + Environment.NewLine);
        }

        private void CategoryListingRangeFacetExample(string categoryName)
        {
            Debug.WriteLine("categorylisting-range-facet: " + Environment.NewLine);
            Debug.WriteLine("items: ");

            // CODE SAMPLE categorylisting-range-facet BEGIN
            // Category listing with a range facet
            // The use-case here is e.g. when the user selects a specific price range in the category listing facet list
            var request = new GetEntitiesByAttributeRequest("Category", categoryName);

            //Add facets to the request 
            //And select a specific range for a certain facet
            request.ResultsOptions.AddDistinctFacet<string>("Manufacturer");
            request.ResultsOptions.AddDistinctFacet<string>("Category");
            request.ResultsOptions.AddDistinctFacet<string>("Organic");
            request.ResultsOptions.AddRangeFacet<double>("Price", new RangeFacetSelectedParameter<double>() { Min = 10, Max = 60 });

            var response = _loop54Client.GetEntitiesByAttribute(request);
            // CODE SAMPLE END

            RenderItemsExtended(response);
            RenderFacets(response);

            Debug.WriteLine("categorylisting-range-facet (end) " + Environment.NewLine);
        }

        private void CategoryListingSortingExample(string categoryName)
        {
            Debug.WriteLine("categorylisting-sorting: " + Environment.NewLine);
            Debug.WriteLine("items: ");

            // CODE SAMPLE categorylisting-sorting BEGIN
            // Category listing with sorting
            var request = new GetEntitiesByAttributeRequest("Category", categoryName);

            //Set the sort order of the products in the category
            request.ResultsOptions.SortBy = new List<EntitySortingParameter>{
                new EntitySortingParameter("Price")
                    { Order = SortOrders.Asc}, // Primary sorting: Sort on attribute Price, ascending order
                new EntitySortingParameter(EntitySortingParameter.Types.Popularity)
                    { Order = SortOrders.Desc} // Secondary sorting: Sort on popularity, descending order
            };

            var response = _loop54Client.GetEntitiesByAttribute(request);
            // CODE SAMPLE END

            RenderItemsExtended(response);

            Debug.WriteLine("categorylisting-sorting (end) " + Environment.NewLine);
        }

        private void CategoryListingFilterExample(string categoryName)
        {
            Debug.WriteLine("categorylisting-filter: " + Environment.NewLine);
            Debug.WriteLine("items: ");

            // CODE SAMPLE categorylisting-filter BEGIN
            // Category listing with filters
            var request = new GetEntitiesByAttributeRequest("Category", categoryName);

            //Filter the products in the category
            //In this case, we only want products that have got
            //the price attribute, and where the organic attribute is set to "True"
            request.ResultsOptions.Filter = new AndFilterParameter(
                new AttributeExistsFilterParameter("Price"),
                //Because the organic attribute is stored as a string in the engine we need to filter with that type.                
                //If it would have been stored as a boolean we would have used bool instead.
                new AttributeFilterParameter<string>("Organic", "True")
            );

            var response = _loop54Client.GetEntitiesByAttribute(request);
            // CODE SAMPLE END

            RenderItemsExtended(response);

            Debug.WriteLine("categorylisting-filter (end) " + Environment.NewLine);
        }
        #endregion

        #region HelperMethods
        private void RenderItems(GetEntitiesByAttributeResponse response)
        {
            // CODE SAMPLE render-items BEGIN
            var results = response.Results.Items;

            if (!results.Any())
                Debug.WriteLine("There were no items in this category.");

            foreach (var resultItem in results)
            {
                var productId = resultItem.GetAttributeValueOrDefault<string>("Id");
                var productTitle = resultItem.GetAttributeValueOrDefault<string>("Title");
                Debug.WriteLine(productId + " " + productTitle);
                //render a product on the category listing page
            }
            // CODE SAMPLE END
        }

        private void RenderItemsExtended(GetEntitiesByAttributeResponse response)
        {
            var results = response.Results.Items;

            if (!results.Any())
                Debug.WriteLine("There were no items in this category.");

            foreach (var resultItem in results)
            {
                var productId = resultItem.GetAttributeValueOrDefault<string>("Id");
                var productTitle = resultItem.GetAttributeValueOrDefault<string>("Title");
                var price = resultItem.GetAttributeValueOrDefault<double>("Price");
                var organic = resultItem.GetAttributeValueOrDefault<string>("Organic");
                Debug.WriteLine(productId + " " + productTitle + " (" + price + " kr, " + organic + "), ");
                //render a product on the category listing page
            }
        }

        private void RenderFacets(GetEntitiesByAttributeResponse response)
        {
            // CODE SAMPLE render-facets BEGIN
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
            // CODE SAMPLE END
        }
        #endregion
    }
}