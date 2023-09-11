using Loop54.Http;
using Loop54.Model;
using Loop54.Model.Request;
using Loop54.Model.Request.Parameters;
using Loop54.Model.Response;
using Loop54.User;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace Loop54.Tests
{
    [TestFixture]
    public class Loop54Client
    {
        public class TestRequestManager : IRequestManager
        {
            public string CalledAction { get; private set; }
            public object CalledRequestData { get; private set; }
            public UserMetaData CalledMetaData { get; private set; }

            public Task<TResponse> CallEngineAsync<TRequest, TResponse>(string action, TRequest requestData, UserMetaData metaData)
                where TRequest : Request
                where TResponse : Response
            {
                CalledAction = action;
                CalledRequestData = requestData;
                CalledMetaData = metaData;

                return Task.FromResult<TResponse>(null);
            }
        }

        [Test]
        public void SearchWithoutOverrides()
        {
            TestRequestManager requestManager = new TestRequestManager();
            NullClientInfoProvider clientInfoProvider = new NullClientInfoProvider();
            var client = new Loop54.Loop54Client(requestManager, clientInfoProvider);

            var search = new SearchRequest("pork belly");
            var response = client.Search(search);

            Assert.AreEqual("search", requestManager.CalledAction);
            Assert.AreSame(search, requestManager.CalledRequestData);
            Assert.IsNotNull(requestManager.CalledMetaData);
            Assert.IsNotNull(requestManager.CalledMetaData.UserId);
        }
        
        [Test]
        public void SearchWithoutOverridesAndWithFaultyClientInfoProvider()
        {
            TestRequestManager requestManager = new TestRequestManager();
            var client = new Loop54.Loop54Client(requestManager, new NullClientInfoProvider() { ClientInfo = null });

            var search = new SearchRequest("pork belly");

            //Should throw error to tell us to implement the IRemoteClientInfoProvider correctly
            var exception = Assert.Throws<ClientInfoException>(() => client.Search(search));
            StringAssert.Contains($"returned a null {nameof(IRemoteClientInfo)}", exception.Message);
        }

        [Test]
        public void SearchWithMetaDataOverrides()
        {
            TestRequestManager requestManager = new TestRequestManager();
            NullClientInfoProvider clientInfoProvider = new NullClientInfoProvider();
            var client = new Loop54.Loop54Client(requestManager, clientInfoProvider);

            var search = new SearchRequest("pork belly");
            var response = client.Search(search.Wrap(metaDataOverrides: new UserMetaData("joakim.borgstrom")));

            Assert.AreEqual("search", requestManager.CalledAction);
            Assert.AreSame(search, requestManager.CalledRequestData);
            Assert.IsNotNull(requestManager.CalledMetaData);
            Assert.AreEqual("joakim.borgstrom", requestManager.CalledMetaData.UserId);
        }

        [Test]
        public void SearchWithMetaDataAndClientInfoOverrides() =>
            CallWithOverrides((c, r) => c.Search(r), new SearchRequest("pork belly"), "search");

        [Test]
        public void AutoCompleteWithMetaDataAndClientInfoOverrides() =>
            CallWithOverrides((c, r) => c.AutoComplete(r), new AutoCompleteRequest("pork bel"), "autoComplete");

        [Test]
        public void CreateEventsWithMetaDataAndClientInfoOverrides() =>
            CallWithOverrides((c, r) => c.CreateEvents(r), new CreateEventsRequest(new ClickEvent(new Entity("product", "1337"))), "createEvents");

        [Test]
        public void GetEntitiesWithMetaDataAndClientInfoOverrides() =>
            CallWithOverrides((c, r) => c.GetEntities(r), new GetEntitiesRequest(), "getEntities");

        [Test]
        public void GetEntitiesByAttributeWithMetaDataAndClientInfoOverrides() =>
            CallWithOverrides((c, r) => c.GetEntitiesByAttribute(r), new GetEntitiesByAttributeRequest("category", "meats"), "getEntitiesByAttribute");

        [Test]
        public void GetPopularEntitiesWithMetaDataAndClientInfoOverrides() =>
            CallWithOverrides((c, r) => c.GetPopularEntities(r), new GetPopularEntitiesRequest("click", null, null), "getPopularEntities");

        [Test]
        public void GetRecentEntitiesWithMetaDataAndClientInfoOverrides() =>
            CallWithOverrides((c, r) => c.GetRecentEntities(r), new GetRecentEntitiesRequest("purchase", new [] {"Pr"}, "U1"), "getRecentEntities");

        [Test]
        public void GetRelatedEntitiesWithMetaDataAndClientInfoOverrides() =>
            CallWithOverrides((c, r) => c.GetRelatedEntities(r), new GetRelatedEntitiesRequest("category", "meats"), "getRelatedEntities");

        [Test]
        public void CustomCallWithMetaDataAndClientInfoOverrides() =>
            CallWithOverrides((c, r) => c.CustomCall("customerspecific", r), new Request(), "customerspecific");

        private void CallWithOverrides<TResponse, TRequest>(Func<ILoop54Client, RequestContainer<TRequest>, TResponse> call, TRequest request,
            string expectedAction) 
            where TRequest : Request
            where TResponse : Response
        {
            const string remoteIp = "127.0.0.1";
            const string referer = "referer.se";
            const string userAgent = "testcase";

            TestRequestManager requestManager = new TestRequestManager();
            NullClientInfoProvider clientInfoProvider = new NullClientInfoProvider();
            clientInfoProvider.ClientInfo = new NullClientInfo()
            {
                RemoteIp = remoteIp,
                Referrer = referer,
                UserAgent = userAgent
            };

            var client = new Loop54.Loop54Client(requestManager, clientInfoProvider);
            
            var response = call(client, request.Wrap(new UserMetaData("joakim.borgstrom")));

            Assert.AreEqual(expectedAction, requestManager.CalledAction);
            Assert.AreSame(request, requestManager.CalledRequestData);
            Assert.IsNotNull(requestManager.CalledMetaData);
            Assert.AreEqual("joakim.borgstrom", requestManager.CalledMetaData.UserId);
            Assert.AreEqual(remoteIp, requestManager.CalledMetaData.IpAddress);
            Assert.AreEqual(referer, requestManager.CalledMetaData.Referer);
            Assert.AreEqual(userAgent, requestManager.CalledMetaData.UserAgent);
        }

        [Test]
        public void CustomCallWithoutOverridesWackyActionName()
        {
            TestRequestManager requestManager = new TestRequestManager();
            NullClientInfoProvider clientInfoProvider = new NullClientInfoProvider();
            var client = new Loop54.Loop54Client(requestManager, clientInfoProvider);

            var request = new Request();
            Assert.Throws<ArgumentNullException>(() => client.CustomCall(null, request));
            Assert.Throws<ArgumentException>(() => client.CustomCall("", request));
        }
    }
}
