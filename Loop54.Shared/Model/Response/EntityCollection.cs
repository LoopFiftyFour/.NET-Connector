using System.Collections.Generic;
using System.Linq;

namespace Loop54.Model.Response
{
    /// <summary>
    /// A collection of entities returned by the Loop54 e-commerce search engine.
    /// </summary>
    public class EntityCollection
    {
        /// <summary>
        /// The total number of entities that are available to return after filtering and faceting but before paging.
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// The facets calculated for the result set.
        /// </summary>
        public IList<Facet> Facets { get; set; }

        /// <summary>
        /// The filtered, faceted, sorted and paged entities.
        /// </summary>
        public IList<Entity> Items { get; set; }

        public DistinctFacet GetDistinctFacetByName(string name)
        {
            return Facets?.FirstOrDefault(f => f.Name.Equals(name))?.AsDistinct();
        }

        public RangeFacet GetRangeFacetByName(string name)
        {
            return Facets?.FirstOrDefault(f => f.Name.Equals(name))?.AsRange();
        }
    }
}
