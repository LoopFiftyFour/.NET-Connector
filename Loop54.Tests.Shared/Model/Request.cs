using Loop54.Model.Request.Parameters;
using Loop54.Model.Response;
using Loop54.Serialization;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

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
    }
}
