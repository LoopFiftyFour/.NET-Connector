using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Loop54.Tests
{
    [TestFixture]
    class Loop54ClientProvider
    {
        [Test]
        public void CreateSingleSetting()
        {
            var provider = new Loop54.Loop54ClientProvider(new NullClientInfoProvider(),
                Loop54.Loop54SettingsCollection.Create().Add("Test", "https://test.54proxy.com"));

            var namedInstance = provider.GetNamed("Test");
            var defaultInstance = provider.GetSingleOrThrow();

            Assert.AreSame(namedInstance, defaultInstance);
            AssertEndpoint(namedInstance, "https://test.54proxy.com");
        }

        [Test]
        public void CreateMultipleSettings()
        {
            var provider = new Loop54.Loop54ClientProvider(new NullClientInfoProvider(),
                Loop54.Loop54SettingsCollection.Create()
                    .Add("Test1", "https://test-1.54proxy.com")
                    .Add("Test2", "https://test-2.54proxy.com"));

            var namedInstance1 = provider.GetNamed("Test1");
            var namedInstance2 = provider.GetNamed("Test2");

            AssertEndpoint(namedInstance1, "https://test-1.54proxy.com");
            AssertEndpoint(namedInstance2, "https://test-2.54proxy.com");
            Assert.AreNotSame(namedInstance1, namedInstance2);
        }

        [Test]
        public void ConstructorShouldFailIfBadInput()
        {
            //The context accessor cannot be null
            var exception = Assert.Throws<ArgumentNullException>(() => new Loop54.Loop54ClientProvider(null, Loop54.Loop54SettingsCollection.Create()
                    .Add("Test1", "https://test-1.54proxy.com")));
            StringAssert.Contains("remoteClientInfoProvider", exception.Message);
            
            //The settingsCollection cannot be null
            exception = Assert.Throws<ArgumentNullException>(() => new Loop54.Loop54ClientProvider(new NullClientInfoProvider(), null));
            StringAssert.Contains("settingsCollection", exception.Message);

            //The settingscollection cannot be empty
            var exception2 = Assert.Throws<ArgumentException>(() => new Loop54.Loop54ClientProvider(new NullClientInfoProvider(), Loop54.Loop54SettingsCollection.Create()));
            StringAssert.Contains("settingsCollection", exception2.Message);
            StringAssert.Contains("empty", exception2.Message);
        }

        [Test]
        public void GetNamedThrowsIfMissingOrNullName()
        {
            var provider = new Loop54.Loop54ClientProvider(new NullClientInfoProvider(),
                Loop54.Loop54SettingsCollection.Create()
                    .Add("Test1", "https://test-1.54proxy.com")
                    .Add("Test2", "https://test-2.54proxy.com"));

            var exception = Assert.Throws<ApplicationException>(() => provider.GetNamed("Test3"));
            StringAssert.Contains("Test3", exception.Message);
            
            var exception2 = Assert.Throws<ArgumentNullException>(() => provider.GetNamed(null));
            StringAssert.Contains("instanceName", exception2.Message);
        }

        [Test]
        public void CreateMultipleSettingsShouldThrowSingle()
        {
            var provider = new Loop54.Loop54ClientProvider(new NullClientInfoProvider(),
                Loop54.Loop54SettingsCollection.Create()
                    .Add("Test1", "https://test-1.54proxy.com")
                    .Add("Test2", "https://test-2.54proxy.com"));

            var exception = Assert.Throws<ApplicationException>(() => provider.GetSingleOrThrow());
            StringAssert.Contains("Cannot guess a single default client", exception.Message);
        }

        private void AssertEndpoint(ILoop54Client client, string expectedEndpoint)
        {
            Loop54.Loop54Client loop54Client = (Loop54.Loop54Client)client;
            Loop54.Http.RequestManager requestManager = (Loop54.Http.RequestManager)loop54Client.RequestManager;
            string endpoint = requestManager.Settings.Endpoint;
            Assert.AreEqual(expectedEndpoint, endpoint);
        }
    }
}
