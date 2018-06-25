using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Loop54.Tests.User
{
    [TestFixture]
    class UserMetaData
    {
        [Test]
        public void SetSetFromClientInfo()
        {
            var meta = new Loop54.User.UserMetaData();
            meta.SetFromClientInfo(new NullClientInfo());
        }

        [Test]
        public void SetSetFromClientInfoFillWithData()
        {
            var meta = new Loop54.User.UserMetaData();

            const string remoteIp = "127.0.0.1";
            const string referer = "referer.se";
            const string userAgent = "testcase";

            var clientInfo = new NullClientInfo()
            {
                RemoteIp = remoteIp,
                Referrer = referer,
                UserAgent = userAgent
            };

            meta.SetFromClientInfo(clientInfo);

            Assert.AreEqual(remoteIp, meta.IpAddress);
            Assert.AreEqual(referer, meta.Referer);
            Assert.AreEqual(userAgent, meta.UserAgent);
        }

        [Test]
        public void SetSetFromClientInfoFillWithDataProxyIp()
        {
            var meta = new Loop54.User.UserMetaData();

            const string remoteIp = "127.0.0.1";
            const string proxyIp = "127.0.0.2";
            const string referer = "referer.se";
            const string userAgent = "testcase";

            var clientInfo = new NullClientInfo()
            {
                RemoteIp = remoteIp,
                Referrer = referer,
                UserAgent = userAgent
            };
            clientInfo.Headers[Loop54.User.UserMetaData.ProxyIpHeaderName] = proxyIp;

            meta.SetFromClientInfo(clientInfo);

            Assert.AreEqual(proxyIp, meta.IpAddress);
            Assert.AreEqual(referer, meta.Referer);
            Assert.AreEqual(userAgent, meta.UserAgent);
        }

        [Test]
        public void SetSetFromClientInfoFillWithExistingUserCookie()
        {
            var meta = new Loop54.User.UserMetaData();

            const string remoteIp = "127.0.0.1";
            const string referer = "referer.se";
            const string userAgent = "testcase";
            const string userId = "joakim.borgstrom";

            var clientInfo = new NullClientInfo()
            {
                RemoteIp = remoteIp,
                Referrer = referer,
                UserAgent = userAgent
            };
            clientInfo.SetCookie(Loop54.User.UserMetaData.UserIdCookieKey, userId, DateTime.UtcNow.AddYears(1));

            meta.SetFromClientInfo(clientInfo);

            Assert.AreEqual(remoteIp, meta.IpAddress);
            Assert.AreEqual(referer, meta.Referer);
            Assert.AreEqual(userAgent, meta.UserAgent);
            Assert.AreEqual(userId, meta.UserId);
        }

        [Test]
        public void SetSetFromClientInfoFillWithExistingUserOnMetaData()
        {
            var meta = new Loop54.User.UserMetaData();

            const string remoteIp = "127.0.0.1";
            const string referer = "referer.se";
            const string userAgent = "testcase";
            const string userId = "joakim.borgstrom";

            var clientInfo = new NullClientInfo()
            {
                RemoteIp = remoteIp,
                Referrer = referer,
                UserAgent = userAgent
            };

            meta.UserId = userId;
            meta.SetFromClientInfo(clientInfo);

            Assert.AreEqual(remoteIp, meta.IpAddress);
            Assert.AreEqual(referer, meta.Referer);
            Assert.AreEqual(userAgent, meta.UserAgent);
            Assert.AreEqual(userId, meta.UserId);
        }
    }
}
