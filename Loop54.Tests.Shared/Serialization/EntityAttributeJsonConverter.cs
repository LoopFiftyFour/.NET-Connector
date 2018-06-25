using Loop54.Model;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Loop54.Tests.Serialization
{
    [TestFixture]
    class EntityAttributeJsonConverter
    {
        [Test]
        public void DeserializeStringEntityAttribute()
        {
            string entityAttributeString = "{\"name\": \"category1\", \"type\": \"string\", \"values\": [\"Toys\"]}";
            
            var attribute = JsonConvert.DeserializeObject<EntityAttribute>(entityAttributeString, new Loop54.Serialization.EntityAttributeJsonConverter());

            Assert.AreEqual("category1", attribute.Name);
            Assert.AreEqual(EntityAttributeType.String, attribute.Type);
            Assert.AreEqual("Toys", attribute.GetValue<string>());
        }

        [Test]
        public void DeserializeEmptyStringEntityAttribute()
        {
            string entityAttributeString = "{\"name\": \"category1\", \"type\": \"string\", \"values\": []}";

            var attribute = JsonConvert.DeserializeObject<EntityAttribute>(entityAttributeString, new Loop54.Serialization.EntityAttributeJsonConverter());

            Assert.AreEqual("category1", attribute.Name);
            Assert.AreEqual(EntityAttributeType.String, attribute.Type);
            Assert.AreEqual(null, attribute.GetValue<string>());
        }

        [Test]
        public void DeserializeDoubleEntityAttribute()
        {
            string entityAttributeString = "{\"name\": \"Price\", \"type\": \"number\", \"values\": [12, 13.37]}";

            var attribute = JsonConvert.DeserializeObject<EntityAttribute>(entityAttributeString, new Loop54.Serialization.EntityAttributeJsonConverter());

            Assert.AreEqual("Price", attribute.Name);
            Assert.AreEqual(EntityAttributeType.Number, attribute.Type);
            Assert.AreEqual(12d, attribute.GetValue<double>());
            Assert.AreEqual(12d, attribute.GetValues<double>()[0]);
            Assert.AreEqual(13.37d, attribute.GetValues<double>()[1]);
        }

        [Test]
        public void DeserializeEntityAttributeMissingRequired()
        {
            string entityAttributeString = "{\"type\": \"number\", \"values\": [12, 13.37]}";
            ApplicationException exception = Assert.Throws<ApplicationException>(() => JsonConvert.DeserializeObject<EntityAttribute>(entityAttributeString, new Loop54.Serialization.EntityAttributeJsonConverter()));
            StringAssert.Contains("name", exception.Message);

            entityAttributeString = "{\"name\": \"Price\", \"values\": [12, 13.37]}";
            exception = Assert.Throws<ApplicationException>(() => JsonConvert.DeserializeObject<EntityAttribute>(entityAttributeString, new Loop54.Serialization.EntityAttributeJsonConverter()));
            StringAssert.Contains("type", exception.Message);

            entityAttributeString = "{\"type\": \"number\", \"name\": \"Price\"}";
            exception = Assert.Throws<ApplicationException>(() => JsonConvert.DeserializeObject<EntityAttribute>(entityAttributeString, new Loop54.Serialization.EntityAttributeJsonConverter()));
            StringAssert.Contains("values", exception.Message);
        }
    }
}
