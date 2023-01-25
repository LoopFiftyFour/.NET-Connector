using System.Collections.Generic;
using Loop54.Http;
using Loop54.Model;
using Loop54.Model.Request;
using Loop54.Model.Request.Parameters;
using Loop54.Model.Request.Parameters.Filters;
using Loop54.Model.Response;
using Loop54.User;
using NUnit.Framework;

namespace Loop54.Tests
{
    [TestFixture]
    public class CallMethods
    {
        private static Loop54.Loop54Client GetClient()
        {
            return new Loop54.Loop54Client(new RequestManager(new Loop54Settings("https://helloworld.54proxy.com", "TestApiKey")
            {
                RequestTimeoutMs = 2000
            }), new NullClientInfoProvider());
        }

        private static RequestContainer<T> WrapRequest<T>(T requestData) where T : Request
            => requestData.Wrap(new UserMetaData { UserId = "testUser", IpAddress = "0.0.0.0" });
        
        [Test]
        public void CreateEvents()
        {
            var request = new CreateEventsRequest(new ClickEvent(new Entity("123", "321")));
            GetClient().CreateEvents(WrapRequest(request));
        }
        
        [Test]
        public void AutoCompleteHasResults([Values("s", "st", "ste", "stea", "steak", "c", "ch", "chi", "chic", "chick", "chicke", "chicken")]string query)
        {
            var response = GetClient().AutoComplete(WrapRequest(new AutoCompleteRequest(query)));
            Assert.Greater(response.Queries.Count, 0);
            Assert.Greater(response.Queries.Items.Count, 0);
        }

        [Test]
        public void SearchHasResults([Values("steak", "chicken breast")]string query)
        {
            var response = GetClient().Search(WrapRequest(new SearchRequest(query)));
            Assert.Greater(response.Results.Count, 0);
            Assert.Greater(response.Results.Items.Count, 0);
            Assert.Greater(response.RelatedResults.Count, 0);
            Assert.Greater(response.RelatedResults.Items.Count, 0);
        }

        [Test]
        public void SearchWithCustomData([Values("steak", "chicken breast")]string query)
        {
            var request = new SearchRequest(query);
            request.AddCustomData("message", "ping");
            var response = GetClient().Search(WrapRequest(request));
            var responseMessage = response.GetCustomDataOrDefault<string>("responseMessage");
            Assert.AreEqual("pong", responseMessage);
        }

        [Test]
        public void GetRelatedEntitiesHasResults()
        {
            //Should be a wheat flour
            var response = GetClient().GetRelatedEntities(WrapRequest(new GetRelatedEntitiesRequest("Product", "13")));
            Assert.Greater(response.Results.Count, 0);
            Assert.Greater(response.Results.Items.Count, 0);
        }

        [Test]
        [Ignore("Not released to HelloWorld engine yet")]
        public void GetComplementaryEntitiesHasResults()
        {
            //Should be a wheat flour
            var response = GetClient().GetComplementaryEntities(WrapRequest(new GetComplementaryEntitiesRequest("Product", "13")));
            Assert.Greater(response.Results.Count, 0);
            Assert.Greater(response.Results.Items.Count, 0);
        }

        [Test]
        [Ignore("Not released to HelloWorld engine yet")]
        public void GetGetBasketRecommendationsHasResults()
        {
            var entities = new List<Entity> { new Entity("Product", "26397727") };
            var response = GetClient().GetBasketRecommendations(WrapRequest(new GetBasketRecommendationsRequest(entities)));
            Assert.Greater(response.Results.Count, 0);
            Assert.Greater(response.Results.Items.Count, 0);
        }

        [Test]
        public void GetEntitiesHasResults()
        {
            //Should result in an expensive steak
            var request = new GetEntitiesRequest();
            request.ResultsOptions.Filter = new AttributeFilterParameter<double>("Price", 100)
            {
                ComparisonMode = FilterComparisonMode.GreaterThanOrEquals
            };

            var response = GetClient().GetEntities(WrapRequest(request));
            Assert.Greater(response.Results.Count, 0);
            Assert.Greater(response.Results.Items.Count, 0);
        }

        [Test]
        public void GetEntitiesByAttributeSingleValueHasResults()
        {
            //Should result in two flour products
            var request = new GetEntitiesByAttributeRequest("Manufacturer", "Grinders inc");

            var response = GetClient().GetEntitiesByAttribute(WrapRequest(request));
            Assert.Greater(response.Results.Count, 0);
            Assert.Greater(response.Results.Items.Count, 0);
        }

        [Test]
        [Ignore("Not released to HelloWorld engine yet")]
        public void GetEntitiesByAttributeMultipleValuesHasEnoughResults()
        {
            //Get number of results in one of the categories
            var request1 = new GetEntitiesByAttributeRequest("Category", "Bakery");
            var response1 = GetClient().GetEntitiesByAttribute(WrapRequest(request1));
            var firstCount = response1.Results.Count;

            //Should result in more products than in just one category
            var request = new GetEntitiesByAttributeRequest("Category", new[] { "Bakery", "Dairy" });
            var response = GetClient().GetEntitiesByAttribute(WrapRequest(request));

            //make sure we get more results than the first request
            Assert.Greater(response.Results.Count, firstCount);
            Assert.Greater(response.Results.Items.Count, 0);
        }

        [Test]
        public void SearchLimits([Values("steak", "chicken breast")]string query, [Values(2, 5, 10, 100)]int number)
        {
            var searchRequest = new SearchRequest(query);
            searchRequest.ResultsOptions.Skip = 0;
            searchRequest.ResultsOptions.Take = number;

            var response = GetClient().Search(WrapRequest(searchRequest));

            AssertNumber(response.Results, number);
        }

        [Test]
        public void Sync()
        {
            ILoop54Client client = GetClient();

            var request = new Request();

            var response = client.Sync(request);
        }

        [Test]
        public void GetIndexedAttributesHasResults()
        {
            var response = GetClient().GetIndexedAttributes(WrapRequest(new GetIndexedAttributesRequest()));
            
            Assert.Greater(response.Attributes.Length, 10);
            Assert.Greater(response.IndexedAttributes.Length, 2);
            
            CollectionAssert.IsOrdered(response.Attributes, EntityAttribute.NameComparer);
            CollectionAssert.IsOrdered(response.IndexedAttributes, EntityAttribute.NameComparer);
        }

        [Test]
        public void GetIndexedAttributeValuesHasResults()
        {
            var response = GetClient().GetIndexedAttributeValues(WrapRequest(new GetIndexedAttributeValuesRequest("Category")));

            Assert.Greater(response.Values.Length, 5);

            CollectionAssert.IsOrdered(response.Values, EntityAttribute.NameComparer);
        }

        private static void AssertNumber(EntityCollection results, int desiredNumber)
        {
            Assert.LessOrEqual(results.Items.Count, results.Count, "The engine returned more results than the engine reported existed."); //do not return more than exist
            Assert.LessOrEqual(results.Items.Count, desiredNumber, "The engine returned more results than we asked for."); //do not return more than we asked for

            //if there are more than we asked for
            if (results.Count > desiredNumber)
                Assert.AreEqual(results.Items.Count, desiredNumber); //return exactly as many as we asked for
            else
                Assert.AreEqual(results.Items.Count, results.Count); //return exactly as many as exist
        }
    }
}
