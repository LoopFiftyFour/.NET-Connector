using Loop54.Model;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Linq;
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
            var request = new Loop54.Model.Request.SyncDeltaPushRequest
            {
                ToAddOrUpdate = { new Entity("Product", "1").SetAttribute("Price", 12d, 13.37d) }
            };

            var json = Newtonsoft.Json.Linq.JObject.Parse(Loop54.Serialization.Serializer.BytesToString(Loop54.Serialization.Serializer.SerializeToBytes(request)));
            string attributeJson = json["toAddOrUpdate"][0]["attributes"][0].ToString(Formatting.None);
            Assert.AreEqual("{\"name\":\"Price\",\"type\":\"number\",\"values\":[12.0,13.37]}", attributeJson);

            var deserialized = JsonConvert.DeserializeObject<EntityAttribute>(attributeJson, new Loop54.Serialization.EntityAttributeJsonConverter());
            Assert.AreEqual("Price", deserialized.Name);
            Assert.AreEqual(EntityAttributeType.Number, deserialized.Type);
            CollectionAssert.AreEqual(new[] { 12d, 13.37d }, deserialized.GetValues<double>());
        }

        [Test]
        public void SetValuesAcceptsNullAndMixedTypes()
        {
            var attribute = new EntityAttribute { Name = "a" };
            attribute.SetValue<string>(null);
            Assert.AreEqual(EntityAttributeType.String, attribute.Type);
            Assert.IsNull(attribute.GetValue<string>());

            // Mixed types used to be accepted by SetValues, so they still are. Only sending them in a SyncDeltaPush request fails.
            attribute.SetValues<object>(new object[] { "x", 1 });
            Assert.IsNull(attribute.Type);
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
        public void SetValueAndCreateConvertValuesTheSameWay()
        {
            var attribute = new EntityAttribute { Name = "a" };
            attribute.SetValue(TestKind.SecondKind);
            Assert.AreEqual(EntityAttributeType.String, attribute.Type);
            Assert.AreEqual("secondKind", attribute.GetValue<string>());
            Assert.AreEqual(TestKind.SecondKind, attribute.GetValue<TestKind>());

            attribute.SetValue(new TestPoco { FooBar = 1, Kind = TestKind.SecondKind });
            Assert.AreEqual("{\"fooBar\":1,\"kind\":\"secondKind\"}", attribute.ValuesInternal[0].ToString(Formatting.None));
            Assert.AreEqual(1, attribute.GetValue<TestPoco>().FooBar);
        }

        [Test]
        public void DeserializedDateLikeStringIsSentBackUnchanged()
        {
            string json = "{\"type\": \"Product\", \"id\": \"1\", \"attributes\": [" +
                          "{\"name\": \"Code\", \"type\": \"string\", \"values\": [\"2020-01-01T10:00:00.000Z\", \"2020-01-01T10:00:00+02:00\"]}]}";
            var entity = Loop54.Serialization.Serializer.DeserializeString<Entity>(json);

            Assert.AreEqual("2020-01-01T10:00:00.000Z", entity.GetAttributeValueOrDefault<string>("Code"));

            var request = new Loop54.Model.Request.SyncDeltaPushRequest { ToReplaceOrAdd = { entity } };
            string sent = Loop54.Serialization.Serializer.BytesToString(Loop54.Serialization.Serializer.SerializeToBytes(request));
            StringAssert.Contains("\"values\":[\"2020-01-01T10:00:00.000Z\",\"2020-01-01T10:00:00+02:00\"]", sent);
        }

        [Test]
        public void DeserializedDateAttributeKeepsStringAndConvertsToUtcDateTime()
        {
            string json = "{\"name\": \"Launch\", \"type\": \"date\", \"values\": [\"2020-01-01T10:00:00Z\"]}";
            var attribute = Loop54.Serialization.Serializer.DeserializeString<EntityAttribute>(json);

            Assert.AreEqual("2020-01-01T10:00:00Z", attribute.GetValue<string>());
            var value = attribute.GetValue<DateTime>();
            Assert.AreEqual(DateTimeKind.Utc, value.Kind);
            Assert.AreEqual(new DateTime(2020, 1, 1, 10, 0, 0, DateTimeKind.Utc), value);
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
