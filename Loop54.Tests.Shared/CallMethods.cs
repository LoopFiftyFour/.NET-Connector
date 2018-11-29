using System;
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
        private Loop54.Loop54Client GetClient()
        {
            return new Loop54.Loop54Client(new RequestManager(new Loop54Settings("https://helloworld.54proxy.com")
            {
                ApiKey = "TestApiKey",
                RequestTimeoutMs = 2000
            }), new NullClientInfoProvider());
        }

        private UserMetaData CreateMetaData()
        {
            return new UserMetaData
            {
                UserId = "testUser",
                IpAddress = "0.0.0.0"
            };
        }
        
        [Test]
        public void CreateEvents()
        {
            var request = new CreateEventsRequest(new ClickEvent(new Entity("123", "321")));
            GetClient().CreateEvents(request.Wrap(metaDataOverrides: CreateMetaData()));
        }
        
        [Test]
        public void AutoCompleteHasResults([Values("b", "be", "bee", "beef", "c", "ch", "chi", "chic", "chick", "chicke", "chicken")]string query)
        {
            var response = GetClient().AutoComplete(new AutoCompleteRequest(query).Wrap(metaDataOverrides: CreateMetaData()));
            Assert.Greater(response.Queries.Count, 0);
            Assert.Greater(response.Queries.Items.Count, 0);
        }

        [Test]
        public void SearchHasResults([Values("steak", "chicken breast")]string query)
        {
            var response = GetClient().Search(new SearchRequest(query).Wrap(metaDataOverrides: CreateMetaData()));
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
            var response = GetClient().Search(request.Wrap(metaDataOverrides: CreateMetaData()));
            var responseMessage = response.GetCustomDataOrDefault<string>("responseMessage");
            Assert.AreEqual("pong", responseMessage);
        }

        [Test]
        public void GetRelatedEntitiesHasResults()
        {
            //Should be a wheat flour
            var response = GetClient().GetRelatedEntities(new GetRelatedEntitiesRequest("Product", "13").Wrap(metaDataOverrides: CreateMetaData()));
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

            var response = GetClient().GetEntities(request.Wrap(metaDataOverrides: CreateMetaData()));
            Assert.Greater(response.Results.Count, 0);
            Assert.Greater(response.Results.Items.Count, 0);
        }

        [Test]
        public void GetEntitiesByAttributeHasResults()
        {
            //Should result in two flour products
            var request = new GetEntitiesByAttributeRequest("Manufacturer", "Grinders inc");

            var response = GetClient().GetEntitiesByAttribute(request.Wrap(metaDataOverrides: CreateMetaData()));
            Assert.Greater(response.Results.Count, 0);
            Assert.Greater(response.Results.Items.Count, 0);
        }

        [Test]
        public void SearchLimits([Values("steak", "chicken breast")]string query, [Values(2, 5, 10, 100)]int number)
        {
            var searchRequest = new SearchRequest(query);
            searchRequest.ResultsOptions.Skip = 0;
            searchRequest.ResultsOptions.Take = number;

            var response = GetClient().Search(searchRequest.Wrap(metaDataOverrides: CreateMetaData()));

            AssertNumber(response.Results, number);
        }

        [Test]
        public void Sync()
        {
            ILoop54Client client = GetClient();

            var request = new Request();

            var response = client.Sync(request);
        }

        private void AssertNumber(EntityCollection results, int desiredNumber)
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
