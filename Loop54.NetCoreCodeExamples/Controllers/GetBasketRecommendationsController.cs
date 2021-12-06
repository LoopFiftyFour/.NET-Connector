using Loop54.Model;
using Loop54.Model.Request;
using Loop54.Model.Response;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;
using System.Linq;

namespace Loop54.NetCoreCodeExamples.Controllers
{
    public class GetBasketRecommendationsController : Controller
    {
        private readonly ILoop54Client _loop54Client;

        public GetBasketRecommendationsController(ILoop54Client loop54Client)
        {
            _loop54Client = loop54Client;
        }

        [HttpGet]
        public ActionResult Index()
        {
            // Code examples
            GetBasketRecommendations();

            return View();
        }

        #region CodeExamples
        private void GetBasketRecommendations()
        {
            Debug.WriteLine("get-basket-recommendations: " + Environment.NewLine);

            // CODE SAMPLE get-basket-recommendations-full BEGIN
            GetBasketRecommendationsRequest request = new GetBasketRecommendationsRequest(new [] 
            {
                new Entity("Product", "13"),
                new Entity("Product", "14"),
            });

            //specify number of response items
            request.ResultsOptions.Skip = 0;
            request.ResultsOptions.Take = 10;

            //fetch response from engine
            GetBasketRecommendationsResponse response = _loop54Client.GetBasketRecommendations(request);

            foreach (Entity recommendation in response.Results.Items)
            {
                var productId = recommendation.Id;
                var productTitle = recommendation.GetAttributeValueOrDefault<string>("Title");
                Debug.WriteLine(productId + " " + productTitle); //render a product on the search results page
            }
            // CODE SAMPLE END

            Debug.WriteLine("get-basket-recommendations (end) " + Environment.NewLine);
        }
        #endregion
    }
}
