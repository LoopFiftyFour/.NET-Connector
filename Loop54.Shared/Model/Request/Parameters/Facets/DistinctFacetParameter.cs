using System.Collections.Generic;

namespace Loop54.Model.Request.Parameters.Facets
{
    /// <summary>
    /// Used for distinct faceting of entities. A distinct facet consists of a finite number of 
    /// options with the number of connected entities as the values. Only entities connected to selected 
    /// facet options are returned. Or all if none are selected.
    /// </summary>
    /// <typeparam name="T">Type of the attribute to facet on.</typeparam>
    public class DistinctFacetParameter<T> : FacetParameter
    {
        public DistinctFacetParameter(string attributeName) : base(attributeName)
        {
        }

        /// <summary>
        /// Returns the type of the facet parameter. Will always be <see cref="FacetType.Distinct"/>.
        /// </summary>
        public override FacetType Type => FacetType.Distinct;

        /// <summary>
        /// What options the user has selected. Only entities belonging to those options will be returned.
        /// </summary>
        public IList<T> Selected { get; set; }

        /// <summary>
        /// These sorting parameters specify how the options are to be sorted in the response.
        /// </summary>
        public IList<DistinctFacetItemSortingParameter> SortBy { get; set; }
    }
}
