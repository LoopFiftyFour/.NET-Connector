using Loop54.Model.Response;
using Loop54.Model.Response.Sync;
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
            "\"enumData\": \"wednesday\", " +
            "\"complexData\": { \"count\": 3, \"facets\": [], \"items\": [{\"id\": \"sku-123\", \"type\": \"product\"}] } } }";

        private const string ResponseJsonWithoutCustomData = "{}";

        [Test]
        public void GetCustomDataOrDefault()
        {
            var responseObject = GetResponse(ResponseJsonWithCustomData);

            Assert.AreEqual("Hjalmar Söderberg", responseObject.GetCustomDataOrDefault<string>("stringData"));
            Assert.AreEqual(13.37d, responseObject.GetCustomDataOrDefault<double>("doubleData"));
            Assert.AreEqual(DayOfWeek.Wednesday, responseObject.GetCustomDataOrDefault<DayOfWeek>("enumData"));
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
        public void GetCustomDataDateKeepsStringAndConvertsToUtcDateTime()
        {
            var responseObject = GetResponse("{ \"customData\": { \"date\": \"2020-01-01T10:00:00Z\", " +
                                             "\"dates\": [\"2020-01-01T10:00:00Z\"] } }");

            Assert.AreEqual("2020-01-01T10:00:00Z", responseObject.GetCustomDataOrDefault<string>("date"));
            var date = responseObject.GetCustomDataOrDefault<DateTime>("date");
            Assert.AreEqual(DateTimeKind.Utc, date.Kind);
            Assert.AreEqual(new DateTime(2020, 1, 1, 10, 0, 0, DateTimeKind.Utc), date);
            Assert.AreEqual(new DateTimeOffset(2020, 1, 1, 10, 0, 0, TimeSpan.Zero), responseObject.GetCustomDataOrDefault<DateTimeOffset?>("date"));

            var dates = responseObject.GetCustomDataOrDefault<List<DateTime>>("dates");
            Assert.AreEqual(DateTimeKind.Utc, dates[0].Kind);
        }

        [Test]
        public void GetCustomDataOrDefaultNoCustomData()
        {
            var responseObject = GetResponse(ResponseJsonWithoutCustomData);

            Assert.IsNull(responseObject.GetCustomDataOrDefault<string>("stringData"));
            Assert.AreEqual(default(double), responseObject.GetCustomDataOrDefault<double>("doubleData"));
            Assert.AreEqual(default(DayOfWeek), responseObject.GetCustomDataOrDefault<DayOfWeek>("enumData"));
            Assert.IsNull(responseObject.GetCustomDataOrDefault<EntityCollection>("complexData"));
        }
        
        [Test]
        public void GetCustomDataOrThrow()
        {
            var responseObject = GetResponse(ResponseJsonWithCustomData);

            Assert.AreEqual("Hjalmar Söderberg", responseObject.GetCustomDataOrThrow<string>("stringData"));
            Assert.AreEqual(13.37d, responseObject.GetCustomDataOrThrow<double>("doubleData"));
            Assert.AreEqual(DayOfWeek.Wednesday, responseObject.GetCustomDataOrThrow<DayOfWeek>("enumData"));
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
            Assert.Throws<CustomDataException>(() => responseObject.GetCustomDataOrThrow<double>("enumData"));
            Assert.Throws<CustomDataException>(() => responseObject.GetCustomDataOrThrow<EntityCollection>("complexData"));
        }

        private const string ResponseWithContent =
            "{ \"makesSense\": true, \"results\": { \"count\": 0, \"facets\": [], \"items\": [] }, " +
            "\"contentSearchResults\": [ { \"entityType\": \"Brand\", \"results\": { \"count\": 1, \"facets\": [], " +
            "\"items\": [ { \"id\": \"b-1\", \"type\": \"Brand\" } ] } } ] }";

        private static IEnumerable<TestCaseData> ContentResponseDeserializers()
        {
            yield return new TestCaseData((Func<byte[], ContentResponse>)Serializer.DeserializeBytes<SearchResponse>)
                .SetArgDisplayNames(nameof(SearchResponse));
            yield return new TestCaseData((Func<byte[], ContentResponse>)Serializer.DeserializeBytes<SuggestResponse>)
                .SetArgDisplayNames(nameof(SuggestResponse));
        }

        [TestCaseSource(nameof(ContentResponseDeserializers))]
        public void ContentResponseDeserializesContentSearchResults(Func<byte[], ContentResponse> deserialize)
        {
            var response = deserialize(Encoding.UTF8.GetBytes(ResponseWithContent));
            Assert.AreEqual(1, response.ContentSearchResults.Count);
            var brands = response.GetContentSearchResults("Brand");
            Assert.AreEqual(1, brands.Count);
            Assert.AreEqual("b-1", brands.Items[0].Id);
            Assert.IsNull(response.GetContentSearchResults("Article"));
        }

        [TestCaseSource(nameof(ContentResponseDeserializers))]
        public void ContentResponseWithoutContentSearchResults(Func<byte[], ContentResponse> deserialize)
        {
            var response = deserialize(Encoding.UTF8.GetBytes("{ \"makesSense\": true }"));
            Assert.IsNull(response.ContentSearchResults);
            Assert.IsNull(response.GetContentSearchResults("Brand"));
        }

        [Test]
        public void GetSyncStateResponseDeserializes()
        {
            const string json = "{ \"inProgress\": [ " +
                "{ \"startedAtUtc\": \"2026-11-20T11:00:00Z\", \"category\": { \"triggeredFrom\": \"externalEndpointSyncDeltaPush\" }, " +
                "\"state\": \"beingProcessed\" }, " +
                "{ \"startedAtUtc\": \"2026-11-20T11:05:00Z\", \"category\": { \"triggeredFrom\": \"internal\", \"pullSourceId\": \"delta\" }, " +
                "\"state\": \"receivingData\" } ], " +
                "\"inProgressCount\": 2, " +
                "\"byCategory\": [ " +
                "{ \"category\": { \"triggeredFrom\": \"externalEndpointSyncDeltaPush\" }, " +
                "\"latestSuccess\": { \"appliedAtUtc\": \"2026-11-20T10:15:00Z\", \"earliestChangeFromUtc\": \"2026-11-20T10:00:00Z\", " +
                "\"changedOrAddedCount\": 12, \"removedCount\": 3, \"appliedEventCount\": 4 }, " +
                "\"latestFailure\": { \"failedAtUtc\": \"2026-11-19T08:00:00Z\", \"failedEventCount\": 2 } }, " +
                "{ \"category\": { \"triggeredFrom\": \"internal\", \"pullSourceId\": \"full\" }, " +
                "\"latestFailure\": { \"failedAtUtc\": \"2026-11-18T08:00:00Z\", \"failedEventCount\": 1 } } ] }";

            var response = Serializer.DeserializeBytes<GetSyncStateResponse>(Encoding.UTF8.GetBytes(json));

            Assert.AreEqual(2, response.InProgressCount);
            Assert.AreEqual(2, response.InProgress.Count);
            Assert.AreEqual(new DateTime(2026, 11, 20, 11, 0, 0, DateTimeKind.Utc), response.InProgress[0].StartedAtUtc.ToUniversalTime());
            Assert.AreEqual(SyncTriggeredFrom.ExternalEndpointSyncDeltaPush, response.InProgress[0].Category.TriggeredFrom);
            Assert.IsNull(response.InProgress[0].Category.PullSourceId);
            Assert.AreEqual(SyncInProgressState.BeingProcessed, response.InProgress[0].State);
            Assert.AreEqual(SyncTriggeredFrom.Internal, response.InProgress[1].Category.TriggeredFrom);
            Assert.AreEqual("delta", response.InProgress[1].Category.PullSourceId);
            Assert.AreEqual(SyncInProgressState.ReceivingData, response.InProgress[1].State);

            Assert.AreEqual(2, response.ByCategory.Count);

            var push = response.GetByCategory(SyncTriggeredFrom.ExternalEndpointSyncDeltaPush);
            Assert.AreSame(response.ByCategory[0], push);
            Assert.AreEqual(new DateTime(2026, 11, 20, 10, 15, 0, DateTimeKind.Utc), push.LatestSuccess.AppliedAtUtc.ToUniversalTime());
            Assert.AreEqual(new DateTime(2026, 11, 20, 10, 0, 0, DateTimeKind.Utc), push.LatestSuccess.EarliestChangeFromUtc.ToUniversalTime());
            Assert.AreEqual(12, push.LatestSuccess.ChangedOrAddedCount);
            Assert.AreEqual(3, push.LatestSuccess.RemovedCount);
            Assert.AreEqual(4, push.LatestSuccess.AppliedEventCount);
            Assert.AreEqual(new DateTime(2026, 11, 19, 8, 0, 0, DateTimeKind.Utc), push.LatestFailure.FailedAtUtc.ToUniversalTime());
            Assert.AreEqual(2, push.LatestFailure.FailedEventCount);

            var fullPull = response.GetByCategory(SyncTriggeredFrom.Internal, "full");
            Assert.AreSame(response.ByCategory[1], fullPull);
            Assert.IsNull(fullPull.LatestSuccess);
            Assert.AreEqual(1, fullPull.LatestFailure.FailedEventCount);

            Assert.IsNull(response.GetByCategory(SyncTriggeredFrom.Internal));
            Assert.IsNull(response.GetByCategory(SyncTriggeredFrom.Internal, "delta"));
            Assert.IsNull(response.GetByCategory(SyncTriggeredFrom.ExternalEndpointSync));
        }

        [Test]
        public void GetSyncStateResponseWithoutSyncs()
        {
            var response = Serializer.DeserializeBytes<GetSyncStateResponse>(
                Encoding.UTF8.GetBytes("{ \"inProgress\": [], \"inProgressCount\": 0, \"byCategory\": [] }"));

            Assert.AreEqual(0, response.InProgress.Count);
            Assert.AreEqual(0, response.InProgressCount);
            Assert.AreEqual(0, response.ByCategory.Count);
            Assert.IsNull(response.GetByCategory(SyncTriggeredFrom.ExternalEndpointSyncDeltaPush));
        }

        [Test]
        public void GetSyncStateResponseUnknownEnumValuesDeserializeAsUnknown()
        {
            const string json = "{ \"inProgress\": [ { \"startedAtUtc\": \"2026-11-20T11:00:00Z\", " +
                "\"category\": { \"triggeredFrom\": \"somethingNew\" }, \"state\": \"alsoNew\" } ], \"inProgressCount\": 1, " +
                "\"byCategory\": [ { \"category\": { \"triggeredFrom\": \"externalEndpointCustom\" } } ] }";

            var response = Serializer.DeserializeBytes<GetSyncStateResponse>(Encoding.UTF8.GetBytes(json));

            Assert.AreEqual(SyncTriggeredFrom.Unknown, response.InProgress[0].Category.TriggeredFrom);
            Assert.AreEqual(SyncInProgressState.Unknown, response.InProgress[0].State);
            Assert.AreEqual(SyncTriggeredFrom.ExternalEndpointCustom, response.ByCategory[0].Category.TriggeredFrom);
        }

        [Test]
        public void GetSyncStateResponseNonStringEnumValuesAreRejected()
        {
            const string json = "{ \"inProgress\": [ { \"startedAtUtc\": \"2026-11-20T11:00:00Z\", " +
                "\"category\": { \"triggeredFrom\": 3 }, \"state\": \"queued\" } ], \"inProgressCount\": 1, \"byCategory\": [] }";

            Assert.Throws<SerializationException>(() => Serializer.DeserializeBytes<GetSyncStateResponse>(Encoding.UTF8.GetBytes(json)));
        }

        private static Loop54.Model.Response.Response GetResponse(string responseJson)
        {
            return Serializer.DeserializeBytes<Loop54.Model.Response.Response>(Encoding.UTF8.GetBytes(responseJson));
        }
    }
}
