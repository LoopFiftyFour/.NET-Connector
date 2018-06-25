using Loop54.Http;
using Loop54.Model.Request;
using Loop54.Model.Response;
using Loop54.User;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Loop54.Tests.Http
{
    [TestFixture]
    public class RequestManager
    {
        public ResourceHoster SetupHoster(string data, int statusCode)
        {
            ResourceHoster hoster = new ResourceHoster();
            hoster.ResourceString = data;
            hoster.StatusCode = statusCode;
            hoster.Start();

            return hoster;
        }

        [Test]
        public void SuccessfulRequest()
        {
            var hoster = SetupHoster("{\"CustomData\": {}}", 200);

            Loop54.Http.RequestManager manager = new Loop54.Http.RequestManager(new Loop54Settings("http://localhost:" + hoster.Port)
            {
                RequireHttps = false
            });

            UserMetaData metaData = new UserMetaData("User.Name")
            {
                Referer = "https://www.loop54.com",
                UserAgent = "LoopBrowse",
                IpAddress = "127.0.0.1"
            };
            var response = manager.CallEngineAsync<Request, Response>("whatever", new Request(), metaData).Result;

            hoster.Stop();

            Assert.AreEqual("/whatever", hoster.CalledPath);
            Assert.AreEqual("User.Name", hoster.CalledHeaders[Headers.UserId]);
            Assert.AreEqual("https://www.loop54.com", hoster.CalledHeaders[Headers.Referer]);
            Assert.AreEqual("LoopBrowse", hoster.CalledHeaders[Headers.UserAgent]);
            Assert.AreEqual("127.0.0.1", hoster.CalledHeaders[Headers.IpAddress]);
            Assert.IsNotNull(response.CustomData);
            Assert.AreEqual(0, response.CustomData.Count);
        }

        [Test]
        public void StatusCodeError()
        {
            var hoster = SetupHoster("{\"error\": { \"code\": 400, \"status\": \"Bad request\", " +
                "\"title\": \"The request was not valid.\", \"detail\": \"Field skip is not within the allowed range.\", " +
                "\"parameter\": \"request.results.skip\"}}", 400);

            Loop54.Http.RequestManager manager = new Loop54.Http.RequestManager(new Loop54Settings("http://localhost:" + hoster.Port)
            {
                RequireHttps = false
            });

            UserMetaData metaData = new UserMetaData("User.Name");

            var error = Assert.Throws<AggregateException>(() => { var l = manager.CallEngineAsync<Request, Response>("whatever", new Request(), metaData).Result; });
            var realException = (EngineStatusCodeException)error.InnerException;

            hoster.Stop();

            Assert.AreEqual("/whatever", hoster.CalledPath);
            Assert.AreEqual("User.Name", hoster.CalledHeaders[Headers.UserId]);
            Assert.AreEqual(400, realException.Details.Code);
            Assert.AreEqual("Bad request", realException.Details.Status);
            Assert.AreEqual("The request was not valid.", realException.Details.Title);
            Assert.AreEqual("The request was not valid.", realException.Message);
            Assert.AreEqual("request.results.skip", realException.Details.Parameter);
            Assert.AreEqual("Field skip is not within the allowed range.", realException.Details.Detail);
        }

        [Test]
        public void UnreachableError()
        {
            Loop54.Http.RequestManager manager = new Loop54.Http.RequestManager(new Loop54Settings("http://localhost:1337")
            {
                RequireHttps = false,
                RequestTimeoutMs = 100
            });

            UserMetaData metaData = new UserMetaData("User.Name");

            var error = Assert.Throws<AggregateException>(() => { var l = manager.CallEngineAsync<Request, Response>("whatever", new Request(), metaData).Result; });
            Assert.IsInstanceOf(typeof(EngineNotReachableException), error.InnerException);
        }
    }
}
