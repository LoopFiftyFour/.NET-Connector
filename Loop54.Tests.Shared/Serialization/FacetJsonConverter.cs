using Loop54.Model;
using Loop54.Model.Request.Parameters.Facets;
using Loop54.Model.Response;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Loop54.Tests.Serialization
{
    [TestFixture]
    class FacetJsonConverter
    {
        [Test]
        public void DeserializeEmptyDistinctFacet()
        {
            string facetString = "{\"name\": \"category1\", \"type\": \"distinct\", \"items\": []}";
            var facet = JsonConvert.DeserializeObject<Facet>(facetString, new Loop54.Serialization.FacetJsonConverter());
            var distinctFacet = facet.AsDistinct();

            Assert.AreEqual("category1", distinctFacet.Name);
            Assert.AreEqual(FacetType.Distinct, distinctFacet.Type);
            Assert.AreEqual(false, distinctFacet.HasValues);
            Assert.AreEqual(0, distinctFacet.Items.Count);
        }

        [Test]
        public void DeserializeDistinctStringFacet()
        {
            string facetString = "{\"name\": \"category1\", \"type\": \"distinct\", \"items\": [{\"item\": \"Toys\", \"count\": 3, \"selected\": false}, {\"item\": \"Teddy bears\", \"count\": 2, \"selected\": true}]}";
            var facet = JsonConvert.DeserializeObject<Facet>(facetString, new Loop54.Serialization.FacetJsonConverter());
            var distinctFacet = facet.AsDistinct();

            Assert.AreEqual("category1", distinctFacet.Name);
            Assert.AreEqual(FacetType.Distinct, distinctFacet.Type);
            Assert.AreEqual(true, distinctFacet.HasValues);
            Assert.AreEqual(2, distinctFacet.Items.Count);
            AssertFacetOption(distinctFacet.Items[0], "Toys", 3, false);
            AssertFacetOption(distinctFacet.Items[1], "Teddy bears", 2, true);
        }

        [Test]
        public void DeserializeDistinctDoubleFacet()
        {
            string facetString = "{\"name\": \"vatrate\", \"type\": \"distinct\", \"items\": [{\"item\": 25, \"count\": 10, \"selected\": false}, {\"item\": 13.37, \"count\": 3, \"selected\": true}, {\"item\": 12.5, \"count\": 2, \"selected\": false}]}";
            var facet = JsonConvert.DeserializeObject<Facet>(facetString, new Loop54.Serialization.FacetJsonConverter());
            var distinctFacet = facet.AsDistinct();

            Assert.AreEqual("vatrate", distinctFacet.Name);
            Assert.AreEqual(FacetType.Distinct, distinctFacet.Type);
            Assert.AreEqual(true, distinctFacet.HasValues);
            Assert.AreEqual(3, distinctFacet.Items.Count);
            AssertFacetOption(distinctFacet.Items[0], 25d, 10, false);
            AssertFacetOption(distinctFacet.Items[1], 13.37d, 3, true);
            AssertFacetOption(distinctFacet.Items[2], 12.5d, 2, false);
        }

        private void AssertFacetOption<T>(DistinctFacet.DistinctFacetItem item, T value, int count, bool selected)
        {
            Assert.AreEqual(value, item.GetItem<T>());
            Assert.AreEqual(count, item.Count);
            Assert.AreEqual(selected, item.Selected);
        }

        [Test]
        public void DeserializeEmptyRangeFacet()
        {
            string facetString = "{\"name\": \"category1\", \"type\": \"range\"}";
            var facet = JsonConvert.DeserializeObject<Facet>(facetString, new Loop54.Serialization.FacetJsonConverter());
            var rangeFacet = facet.AsRange();

            Assert.AreEqual("category1", rangeFacet.Name);
            Assert.AreEqual(FacetType.Range, rangeFacet.Type);
            Assert.AreEqual(false, rangeFacet.HasValues);
            Assert.AreEqual(null, rangeFacet.GetMin<string>());
            Assert.AreEqual(null, rangeFacet.GetMax<string>());
            Assert.AreEqual(null, rangeFacet.GetSelectedMin<string>());
            Assert.AreEqual(null, rangeFacet.GetSelectedMax<string>());
        }

        [Test]
        public void DeserializeRangeFacet()
        {
            string facetString = "{\"name\": \"price\", \"type\": \"range\", \"min\": 3, \"max\": 246 }";
            var facet = JsonConvert.DeserializeObject<Facet>(facetString, new Loop54.Serialization.FacetJsonConverter());
            var rangeFacet = facet.AsRange();

            Assert.AreEqual("price", rangeFacet.Name);
            Assert.AreEqual(FacetType.Range, rangeFacet.Type);
            Assert.AreEqual(true, rangeFacet.HasValues);

            //Get the values as int
            Assert.AreEqual(3, rangeFacet.GetMin<int>());
            Assert.AreEqual(246, rangeFacet.GetMax<int>());
            Assert.AreEqual(0, rangeFacet.GetSelectedMin<int>());
            Assert.AreEqual(0, rangeFacet.GetSelectedMax<int>());

            //Get the values as double
            Assert.AreEqual(3d, rangeFacet.GetMin<double>());
            Assert.AreEqual(246d, rangeFacet.GetMax<double>());
            Assert.AreEqual(0d, rangeFacet.GetSelectedMin<double>());
            Assert.AreEqual(0d, rangeFacet.GetSelectedMax<double>());
        }

        [Test]
        public void DeserializeSelectedRangeFacet()
        {
            string facetString = "{\"name\": \"price\", \"type\": \"range\", \"min\": 3, \"max\": 246, \"selectedMin\": 5, \"selectedMax\": 100.5}";
            var facet = JsonConvert.DeserializeObject<Facet>(facetString, new Loop54.Serialization.FacetJsonConverter());
            var rangeFacet = facet.AsRange();

            Assert.AreEqual("price", rangeFacet.Name);
            Assert.AreEqual(FacetType.Range, rangeFacet.Type);
            Assert.AreEqual(true, rangeFacet.HasValues);

            //Get the values as int
            Assert.AreEqual(3, rangeFacet.GetMin<int>());
            Assert.AreEqual(246, rangeFacet.GetMax<int>());
            Assert.AreEqual(5, rangeFacet.GetSelectedMin<int>());
            Assert.AreEqual(100, rangeFacet.GetSelectedMax<int>());

            //Get the values as double
            Assert.AreEqual(3d, rangeFacet.GetMin<double>());
            Assert.AreEqual(246d, rangeFacet.GetMax<double>());
            Assert.AreEqual(5d, rangeFacet.GetSelectedMin<double>());
            Assert.AreEqual(100.5d, rangeFacet.GetSelectedMax<double>());
        }
    }
}
