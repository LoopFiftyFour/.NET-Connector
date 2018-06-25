using System.Collections.Generic;
using Loop54.Model.Request.Parameters.Facets;
using Loop54.Model.Request.Parameters.Filters;

namespace Loop54.Model.Request.Parameters
{
    /// <summary>
    /// This class specifies how a collection of entities should be paged, filtered, sorted and faceted
    /// </summary>
    public class EntityCollectionParameters
    {
        /// <summary>
        /// How many entities to skip when paging the result. If null it defaults to 0.
        /// </summary>
        public int? Skip { get; set; }

        /// <summary>
        /// How many entities to take when paging the result. If null it defaults to 5.
        /// </summary>
        public int? Take { get; set; }

        /// <summary>
        /// List of sorting parameters specifying how the result should be sorted. The first sorting parameter specifies the 
        /// primary order. If items are equal, they will be sorted using the second parameter, and so on. If not set the entities will be sorted by relevance descending.
        /// </summary>
        public IList<EntitySortingParameter> SortBy { get; set; }

        /// <summary>
        /// List of facets to calculate and possibly filter the result on. If not set, no faceting is done.
        /// </summary>
        public IList<FacetParameter> Facets { get; set; }
        
        /// <summary>
        /// How the results should be filtered. If not set, no filtering is done.
        /// </summary>
        public FilterParameter Filter { get; set; }

        /// <summary>
        /// Adds a facet with the type <see cref="FacetType.Distinct"/> to the entity collection parameter. 
        /// </summary>
        /// <typeparam name="T">Type of the attribute to filter on. If you expect a string value back use <see cref="string"/> or if you expect a floating point value back, use <see cref="double"/>.</typeparam>
        /// <param name="attributeName">What attribute on the entities to facet on. This must match one of the attributes available on the entities in the search engine.</param>
        /// <param name="selected">Values selected by the user. If none are selected this can be null.</param>
        /// <param name="name">The desired name of the facet in the response. Will be the same as the attributeName if null.</param>
        /// <param name="sortBy">How to sort the facet options in the response.</param>
        public void AddDistinctFacet<T>(string attributeName, 
            IList<T> selected = null, 
            string name = null, 
            IList<DistinctFacetItemSortingParameter> sortBy = null)
        {
            if (Facets == null)
                Facets = new List<FacetParameter>();

            Facets.Add(new DistinctFacetParameter<T>(attributeName)
            {
                Selected = selected,
                Name = name,
                SortBy = sortBy
            });
        }

        /// <summary>
        /// Adds a facet with the type <see cref="FacetType.Range"/> to the entity collection parameter. 
        /// </summary>
        /// <typeparam name="T">Type of the attribute to filter on. If you expect a string value back use <see cref="string"/> or if you expect a floating point value back, use <see cref="double"/>.</typeparam>
        /// <param name="attributeName">What attribute on the entities to facet on. This must match one of the attributes available on the entities in the search engine.</param>
        /// <param name="selected">The min and max values selected by the user. Can be left to null if nothing is selected.</param>
        /// <param name="name">The desired name of the facet in the response. Will be the same as the attributeName if null.</param>
        public void AddRangeFacet<T>(string attributeName, 
            RangeFacetSelectedParameter<T> selected = null, 
            string name = null)
        {
            if (Facets == null)
                Facets = new List<FacetParameter>();
            
            Facets.Add(new RangeFacetParameter<T>(attributeName)
            {
                Selected = selected,
                Name = name
            });
        }
    }
}
