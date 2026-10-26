using Loop54.Model.Request;
using Loop54.Model.Request.Parameters;
using Loop54.Serialization;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Loop54.Tests.Model
{
    [TestFixture]
    public class Request
    {
        [Test]
        public void AddCustomData()
        {
            var request = new Loop54.Model.Request.Request();

            string stringValue = "Hjalmar Söderberg";
            double doubleValue = 13.37d;
            var complexValue = new EntityCollectionParameters()
            {
                Skip = 0,
                Take = 10
            };

            request.AddCustomData("stringKey", stringValue);
            request.AddCustomData("doubleKey", doubleValue);
            request.AddCustomData("complexKey", complexValue);

            Assert.AreEqual(stringValue, request.CustomData["stringKey"]);
            Assert.AreEqual(doubleValue, request.CustomData["doubleKey"]);
            Assert.AreSame(complexValue, request.CustomData["complexKey"]);
        }

        [Test]
        public void AddCustomDataCaseSensitivity()
        {
            var request = new Loop54.Model.Request.Request();

            string stringValue = "Hjalmar Söderberg";
            string stringValue2 = "August Strindberg";

            request.AddCustomData("stringKey", stringValue);
            request.AddCustomData("STRINGKey", stringValue2);

            Assert.AreEqual(stringValue, request.CustomData["stringKey"]);
            Assert.AreEqual(stringValue2, request.CustomData["STRINGKey"]);
        }

        [Test]
        public void SearchRequestSerializesContentResultsOptions()
        {
            var request = new Loop54.Model.Request.SearchRequest("shoes");
            request.AddContentResultsOptions("Brand").Take = 3;
            request.AddContentResultsOptions("Article").Skip = 5;

            var json = JObject.Parse(Serializer.BytesToString(Serializer.SerializeToBytes(request)));
            var options = (JArray)json["contentResultsOptions"];
            Assert.AreEqual(2, options.Count);
            Assert.AreEqual("Brand", (string)options[0]["entityType"]);
            Assert.AreEqual(3, (int)options[0]["take"]);
            Assert.AreEqual("Article", (string)options[1]["entityType"]);
            Assert.AreEqual(5, (int)options[1]["skip"]);
        }

        [Test]
        public void SearchRequestWithoutContentOptionsOmitsField()
        {
            var json = JObject.Parse(Serializer.BytesToString(Serializer.SerializeToBytes(new Loop54.Model.Request.SearchRequest("shoes"))));
            Assert.IsNull(json["contentResultsOptions"]);
        }

        [Test]
        public void TypedEntityCollectionParametersRequiresEntityType()
        {
            Assert.Throws<ArgumentException>(() => new TypedEntityCollectionParameters(null));
            Assert.Throws<ArgumentException>(() => new TypedEntityCollectionParameters(" "));
        }

        private static IEnumerable<TestCaseData> ContentRequestFactories()
        {
            yield return new TestCaseData((Func<string, ContentRequest>)(query => new SearchRequest(query)))
                .SetArgDisplayNames(nameof(SearchRequest));
            yield return new TestCaseData((Func<string, ContentRequest>)(query => new SuggestRequest(query)))
                .SetArgDisplayNames(nameof(SuggestRequest));
        }

        [TestCaseSource(nameof(ContentRequestFactories))]
        public void AddContentResultsOptionsTwiceReturnsSameEntry(Func<string, ContentRequest> createRequest)
        {
            var request = createRequest("summit");
            request.AddContentResultsOptions("Brand").Take = 2;
            request.AddContentResultsOptions("Brand").Skip = 1;
            request.AddContentResultsOptions("Article");

            Assert.AreEqual(2, request.ContentResultsOptions.Count, "Brand should not be added twice");
            Assert.AreEqual("Brand", request.ContentResultsOptions[0].EntityType);
            Assert.AreEqual(2, request.ContentResultsOptions[0].Take);
            Assert.AreEqual(1, request.ContentResultsOptions[0].Skip);
            Assert.AreEqual("Article", request.ContentResultsOptions[1].EntityType);
        }

        [Test]
        public void AddContentResultsOptionsRequiresEntityType()
        {
            var request = new Loop54.Model.Request.SearchRequest("summit");
            var options = request.AddContentResultsOptions("Brand");
            options.EntityType = null;

            Assert.Throws<ArgumentException>(() => request.AddContentResultsOptions(null));
            Assert.Throws<ArgumentException>(() => request.AddContentResultsOptions(" "));
        }
    }
}
