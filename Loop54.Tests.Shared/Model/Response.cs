using Loop54.Model.Response;
using Loop54.Serialization;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Loop54.Tests.Model
{
    [TestFixture]
    public class Response
    {
        private const string ResponseJsonWithCustomData = "{ \"customData\": { \"stringData\": \"Hjalmar Söderberg\", \"doubleData\": 13.37, " +
            "\"complexData\": { \"count\": 3, \"facets\": [], \"items\": [{\"id\": \"sku-123\", \"type\": \"product\"}] } } }";

        private const string ResponseJsonWithoutCustomData = "{}";

        [Test]
        public void GetCustomDataOrDefault()
        {
            var responseObject = GetResponse(ResponseJsonWithCustomData);

            Assert.AreEqual("Hjalmar Söderberg", responseObject.GetCustomDataOrDefault<string>("stringData"));
            Assert.AreEqual(13.37d, responseObject.GetCustomDataOrDefault<double>("doubleData"));
            var complex = responseObject.GetCustomDataOrDefault<EntityCollection>("complexData");
            Assert.AreEqual(3, complex.Count);
            Assert.AreEqual(0, complex.Facets.Count);
            Assert.AreEqual(1, complex.Items.Count);
            Assert.AreEqual("sku-123", complex.Items[0].Id);
            Assert.AreEqual("product", complex.Items[0].Type);

            Assert.IsNull(responseObject.GetCustomDataOrDefault<string>("nonExistingData")); //Should not throw, but return null
            Assert.Throws<CustomDataException>(() => responseObject.GetCustomDataOrDefault<int>("complexData"));//Should fail to deserialize
            Assert.Throws<CustomDataException>(() => responseObject.GetCustomDataOrDefault<int>("stringData"));//Should fail to cast
        }
        
        [Test]
        public void GetCustomDataOrDefaultNoCustomData()
        {
            var responseObject = GetResponse(ResponseJsonWithoutCustomData);

            Assert.IsNull(responseObject.GetCustomDataOrDefault<string>("stringData"));
            Assert.AreEqual(default(double), responseObject.GetCustomDataOrDefault<double>("doubleData"));
            Assert.IsNull(responseObject.GetCustomDataOrDefault<EntityCollection>("complexData"));
        }
        
        [Test]
        public void GetCustomDataOrThrow()
        {
            var responseObject = GetResponse(ResponseJsonWithCustomData);

            Assert.AreEqual("Hjalmar Söderberg", responseObject.GetCustomDataOrThrow<string>("stringData"));
            Assert.AreEqual(13.37d, responseObject.GetCustomDataOrThrow<double>("doubleData"));
            var complex = responseObject.GetCustomDataOrThrow<EntityCollection>("complexData");
            Assert.AreEqual(3, complex.Count);
            Assert.AreEqual(0, complex.Facets.Count);
            Assert.AreEqual(1, complex.Items.Count);
            Assert.AreEqual("sku-123", complex.Items[0].Id);
            Assert.AreEqual("product", complex.Items[0].Type);

            Assert.Throws<CustomDataException>(() => responseObject.GetCustomDataOrThrow<string>("nonExistingData")); //Should throw because it doesn't exist
            Assert.Throws<CustomDataException>(() => responseObject.GetCustomDataOrThrow<int>("complexData")); //Should fail to deserialize
            Assert.Throws<CustomDataException>(() => responseObject.GetCustomDataOrThrow<int>("stringData")); //Should fail to cast
        }

        [Test]
        public void GetCustomDataOrThrowNoCustomData()
        {
            var responseObject = GetResponse(ResponseJsonWithoutCustomData);

            Assert.Throws<CustomDataException>(() => responseObject.GetCustomDataOrThrow<string>("stringData"));
            Assert.Throws<CustomDataException>(() => responseObject.GetCustomDataOrThrow<double>("doubleData"));
            Assert.Throws<CustomDataException>(() => responseObject.GetCustomDataOrThrow<EntityCollection>("complexData"));
        }

        private static Loop54.Model.Response.Response GetResponse(string responseJson)
        {
            return Serializer.DeserializeBytes<Loop54.Model.Response.Response>(Encoding.UTF8.GetBytes(responseJson));
        }
    }
}
