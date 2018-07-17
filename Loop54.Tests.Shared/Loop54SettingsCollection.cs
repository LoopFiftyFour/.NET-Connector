using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Loop54.Tests
{
    [TestFixture]
    class Loop54SettingsCollection
    {
        [Test]
        public void Create()
        {
            var result = Loop54.Loop54SettingsCollection.Create();
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count());
        }

        [Test]
        public void CreateAndAdd()
        {
            var result = Loop54.Loop54SettingsCollection.Create()
                .Add("Test", "https://test-1.54proxy.com");
            
            var setting = result.Single();
            Assert.AreEqual("Test", setting.Key);
            Assert.AreEqual("https://test-1.54proxy.com", setting.Value.Endpoint);
        }

        [Test]
        public void AddNullShouldThrow()
        {
            var result = Loop54.Loop54SettingsCollection.Create();

            var exception = Assert.Throws<ArgumentNullException>(() => result.Add("Test1", (string)null));
            StringAssert.Contains("endpoint", exception.Message);

            exception = Assert.Throws<ArgumentNullException>(() => result.Add(null, new Loop54Settings("https://test-1.54proxy.com")));
            StringAssert.Contains("instanceName", exception.Message);

            exception = Assert.Throws<ArgumentNullException>(() => result.Add("Test1", (Loop54Settings)null));
            StringAssert.Contains("settings", exception.Message);

            //Assert the collection is unchanged after the failure
            Assert.AreEqual(0, result.Count());
        }

        [Test]
        public void AddMultipleAndEnumerate()
        {
            var result = Loop54.Loop54SettingsCollection.Create()
                .Add("Test1", "https://test-1.54proxy.com")
                .Add("Test2", "https://test-2.54proxy.com")
                .Add("Test3", new Loop54Settings("https://test-3.54proxy.com") { RequestTimeoutMs = 1337 });

            var settings = result.OrderBy(kv => kv.Key).ToList(); //We cannot be sure of the internal order

            Assert.AreEqual(3, settings.Count);
            Assert.AreEqual("Test1", settings[0].Key);
            Assert.AreEqual("https://test-1.54proxy.com", settings[0].Value.Endpoint);
            Assert.AreEqual("Test2", settings[1].Key);
            Assert.AreEqual("https://test-2.54proxy.com", settings[1].Value.Endpoint);
            Assert.AreEqual("Test3", settings[2].Key);
            Assert.AreEqual("https://test-3.54proxy.com", settings[2].Value.Endpoint);
            Assert.AreEqual(1337, settings[2].Value.RequestTimeoutMs);
        }

        [Test]
        public void AddSameNameShouldThrow()
        {
            var result = Loop54.Loop54SettingsCollection.Create()
                .Add("Test1", "https://test-1.54proxy.com");

            var exception = Assert.Throws<ApplicationException>(() => result.Add("Test1", "https://test-1-2.54proxy.com"));
            StringAssert.Contains("There's already a 'Test1", exception.Message);

            //Assert the collection is unchanged after the failure
            var settings = result.ToList();
            Assert.AreEqual(1, settings.Count);
            Assert.AreEqual("Test1", settings[0].Key);
            Assert.AreEqual("https://test-1.54proxy.com", settings[0].Value.Endpoint);
        }
    }
}
