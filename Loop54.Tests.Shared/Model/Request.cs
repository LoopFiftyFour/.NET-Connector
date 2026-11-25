using Loop54.Model.Request;
using Loop54.Model.Request.Parameters;
using Loop54.Serialization;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

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
        public void SyncDeltaPushRequestSerializesEntitiesWithAttributes()
        {
            var request = new SyncDeltaPushRequest
            {
                ToAddOrUpdate = { new Loop54.Model.Entity("Product", "1").SetAttribute("Price", 13.37).SetAttribute("Category", "Meat", "Pork") },
                ToReplaceOrAdd = { new Loop54.Model.Entity("Product", "2").SetAttribute("InStock", true).SetAttribute("Stock", 5) },
                ToDelete = { new Loop54.Model.Entity("Product", "3") },
                ToRemoveAttributesFrom = { new Loop54.Model.Entity("Product", "4").SetAttribute<string>("Color") },
            };

            var json = JObject.Parse(Serializer.BytesToString(Serializer.SerializeToBytes(request)));

            var updated = (JObject)json["toAddOrUpdate"].Single();
            Assert.AreEqual("Product", (string)updated["type"]);
            Assert.AreEqual("1", (string)updated["id"]);
            var updatedAttributes = (JArray)updated["attributes"];
            Assert.AreEqual(2, updatedAttributes.Count);
            Assert.IsTrue(JToken.DeepEquals(JObject.Parse("{\"name\": \"Price\", \"type\": \"number\", \"values\": [13.37]}"), updatedAttributes[0]));
            Assert.IsTrue(JToken.DeepEquals(JObject.Parse("{\"name\": \"Category\", \"type\": \"string\", \"values\": [\"Meat\", \"Pork\"]}"),
                updatedAttributes[1]));

            var replacedAttributes = (JArray)json["toReplaceOrAdd"].Single()["attributes"];
            Assert.IsTrue(JToken.DeepEquals(JObject.Parse("{\"name\": \"InStock\", \"type\": \"boolean\", \"values\": [true]}"), replacedAttributes[0]));
            Assert.IsTrue(JToken.DeepEquals(JObject.Parse("{\"name\": \"Stock\", \"type\": \"integer\", \"values\": [5]}"), replacedAttributes[1]));

            Assert.IsTrue(JToken.DeepEquals(JObject.Parse("{\"type\": \"Product\", \"id\": \"3\"}"), json["toDelete"].Single()));

            var removed = json["toRemoveAttributesFrom"].Single();
            Assert.IsTrue(JToken.DeepEquals(JObject.Parse("{\"name\": \"Color\", \"type\": \"string\", \"values\": []}"), removed["attributes"].Single()));
        }

        [Test]
        public void SyncDeltaPushRequestSerializesEmptyAndNullCollections()
        {
            var request = new SyncDeltaPushRequest { ToReplaceOrAdd = null };
            request.ToDelete.Add(new Loop54.Model.Entity("Product", "1"));

            var json = JObject.Parse(Serializer.BytesToString(Serializer.SerializeToBytes(request)));

            Assert.AreEqual(0, ((JArray)json["toAddOrUpdate"]).Count);
            Assert.IsNull(json["toReplaceOrAdd"]);
            Assert.AreEqual(1, ((JArray)json["toDelete"]).Count);
            Assert.AreEqual(0, ((JArray)json["toRemoveAttributesFrom"]).Count);
        }

        [Test]
        public void SyncDeltaPushRequestThrowsForAttributeWithMixedValueTypes()
        {
            var attribute = new Loop54.Model.EntityAttribute { Name = "Mixed" };
            attribute.SetValues<object>(new object[] { "x", 1 });
            var entity = new Loop54.Model.Entity("Product", "1") { Attributes = new List<Loop54.Model.EntityAttribute> { attribute } };

            var request = new SyncDeltaPushRequest { ToAddOrUpdate = { entity } };

            var exception = Assert.Throws<SerializationException>(() => Serializer.SerializeToBytes(request));
            StringAssert.Contains("Mixed", exception.InnerException.Message);
        }

        [Test]
        public void SyncDeltaPushRequestThrowsForNullEntity()
        {
            var request = new SyncDeltaPushRequest();
            request.ToDelete.Add(null);

            var exception = Assert.Throws<SerializationException>(() => Serializer.SerializeToBytes(request));
            StringAssert.Contains("toDelete", exception.InnerException.Message);
        }

        [TestCase(double.NaN)]
        [TestCase(double.PositiveInfinity)]
        [TestCase(double.NegativeInfinity)]
        public void SyncDeltaPushRequestThrowsForNonFiniteNumber(double value)
        {
            var request = new SyncDeltaPushRequest { ToAddOrUpdate = { new Loop54.Model.Entity("Product", "1").SetAttribute("Price", value) } };

            var exception = Assert.Throws<SerializationException>(() => Serializer.SerializeToBytes(request));
            StringAssert.Contains("Price", exception.InnerException.Message);
        }

        [Test]
        public void AttributesAddedToEntityAttributesListAreSent()
        {
            var entity = new Loop54.Model.Entity("Product", "1").SetAttribute("a", 1);
            entity.Attributes.Add(Loop54.Model.EntityAttribute.Create("b", 2));

            Assert.AreEqual(2, entity.Attributes.Count);
            Assert.AreEqual(2, entity.GetAttributeValueOrDefault<int>("B"));

            var request = new SyncDeltaPushRequest { ToAddOrUpdate = { entity } };
            var json = JObject.Parse(Serializer.BytesToString(Serializer.SerializeToBytes(request)));
            CollectionAssert.AreEqual(new[] { "a", "b" }, json["toAddOrUpdate"][0]["attributes"].Select(a => (string)a["name"]));
        }

        [Test]
        public void SetAttributeReplacesExistingAttributeInPlace()
        {
            var entity = new Loop54.Model.Entity("Product", "1").SetAttribute("a", 1).SetAttribute("b", 2).SetAttribute("A", 3);

            Assert.AreEqual(2, entity.Attributes.Count);
            Assert.AreEqual("A", entity.Attributes[0].Name);
            Assert.AreEqual(3, entity.GetAttributeValueOrDefault<int>("a"));
        }

        [Test]
        public void SettingAttributesWithDuplicateNamesThrows()
        {
            var entity = new Loop54.Model.Entity("Product", "1");
            Assert.Throws<ArgumentException>(() => entity.Attributes = new List<Loop54.Model.EntityAttribute>
            {
                Loop54.Model.EntityAttribute.Create("a", 1), Loop54.Model.EntityAttribute.Create("A", 2)
            });
        }

        [Test]
        public void GetAttributeValueOnEntityWithoutAttributesReturnsDefault()
        {
            Assert.IsNull(new Loop54.Model.Entity("Product", "1").GetAttributeValueOrDefault<string>("Title"));
        }

        [Test]
        public void EntitiesOutsideSyncDeltaPushAreSentWithAttributeNamesOnly()
        {
            var entity = new Loop54.Model.Entity("Product", "1").SetAttribute("Price", 13.37);
            var request = new GetRelatedEntitiesRequest(entity);

            var json = JObject.Parse(Serializer.BytesToString(Serializer.SerializeToBytes(request)));

            Assert.IsTrue(JToken.DeepEquals(JObject.Parse("{\"id\": \"1\", \"type\": \"Product\", \"attributes\": [{\"name\": \"Price\"}]}"),
                json["entity"]));
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
