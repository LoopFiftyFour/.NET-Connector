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
        public void SerializeEntityAttributeRoundTrips()
        {
            var attribute = EntityAttribute.Create("Price", 12d, 13.37d);

            string json = JsonConvert.SerializeObject(attribute, new Loop54.Serialization.EntityAttributeJsonConverter(),
                new Newtonsoft.Json.Converters.StringEnumConverter(new Newtonsoft.Json.Serialization.CamelCaseNamingStrategy()));
            Assert.AreEqual("{\"name\":\"Price\",\"type\":\"number\",\"values\":[12.0,13.37]}", json);

            var deserialized = JsonConvert.DeserializeObject<EntityAttribute>(json, new Loop54.Serialization.EntityAttributeJsonConverter());
            Assert.AreEqual("Price", deserialized.Name);
            Assert.AreEqual(EntityAttributeType.Number, deserialized.Type);
            CollectionAssert.AreEqual(new[] { 12d, 13.37d }, deserialized.GetValues<double>());
        }

        [Test]
        public void SetValuesInfersAttributeType()
        {
            Assert.AreEqual(EntityAttributeType.String, EntityAttribute.Create("a", "x").Type);
            Assert.AreEqual(EntityAttributeType.String, EntityAttribute.Create<string>("a").Type);
            Assert.AreEqual(EntityAttributeType.Boolean, EntityAttribute.Create("a", true).Type);
            Assert.AreEqual(EntityAttributeType.Integer, EntityAttribute.Create("a", 1).Type);
            Assert.AreEqual(EntityAttributeType.Integer, EntityAttribute.Create("a", 1L).Type);
            Assert.AreEqual(EntityAttributeType.Integer, EntityAttribute.Create<int?>("a", 1).Type);
            Assert.AreEqual(EntityAttributeType.Number, EntityAttribute.Create("a", 1.5).Type);
            Assert.AreEqual(EntityAttributeType.Number, EntityAttribute.Create("a", 1.5m).Type);
            Assert.AreEqual(EntityAttributeType.Date, EntityAttribute.Create("a", DateTime.UtcNow).Type);
            Assert.AreEqual(EntityAttributeType.Integer, EntityAttribute.Create<object>("a", 7).Type);

            var attribute = EntityAttribute.Create("a", "x");
            attribute.SetValue(2.5);
            Assert.AreEqual(EntityAttributeType.Number, attribute.Type);
            Assert.AreEqual(2.5, attribute.GetValue<double>());
        }

        [Test]
        public void SetValuesInfersAttributeTypeFromValues()
        {
            var dictionary = EntityAttribute.Create("a", new Dictionary<string, string> { { "Key", "value" } });
            Assert.AreEqual(EntityAttributeType.Object, dictionary.Type);
            Assert.AreEqual(EntityAttributeType.Object, EntityAttribute.Create<Dictionary<string, string>>("a").Type);
            Assert.AreEqual(EntityAttributeType.Array, EntityAttribute.Create("a", new List<int> { 1, 2 }).Type);

            Assert.AreEqual(EntityAttributeType.Number, EntityAttribute.Create<object>("a", 12, 13.5).Type);
            Assert.AreEqual(EntityAttributeType.Integer, EntityAttribute.Create<IConvertible>("a", 1, 2L).Type);
            Assert.AreEqual(EntityAttributeType.String, EntityAttribute.Create<object>("a", null, "x").Type);
            Assert.AreEqual(EntityAttributeType.String, EntityAttribute.Create<object>("a").Type);
            Assert.Throws<ArgumentException>(() => EntityAttribute.Create<object>("a", "x", 1));
        }

        [Test]
        public void SetValuesUsesConnectorSerializerSettings()
        {
            var poco = EntityAttribute.Create("a", new TestPoco { FooBar = 1, Kind = TestKind.SecondKind });
            Assert.AreEqual(EntityAttributeType.Object, poco.Type);
            Assert.AreEqual("{\"fooBar\":1,\"kind\":\"secondKind\"}", poco.ValuesInternal[0].ToString(Formatting.None));

            var enumAttribute = EntityAttribute.Create("a", TestKind.SecondKind);
            Assert.AreEqual(EntityAttributeType.String, enumAttribute.Type);
            Assert.AreEqual("secondKind", enumAttribute.GetValue<string>());
        }

        private class TestPoco
        {
            public int FooBar { get; set; }
            public TestKind Kind { get; set; }
        }

        private enum TestKind
        {
            FirstKind,
            SecondKind
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
