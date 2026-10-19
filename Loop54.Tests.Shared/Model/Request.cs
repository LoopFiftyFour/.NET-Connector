using Loop54.Model.Request.Parameters;
using Loop54.Model.Response;
using Loop54.Serialization;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

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
    }
}
