using Loop54.Model.Request.Parameters.Facets;
using Loop54.Serialization;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace Loop54.Model.Response
{
    /// <summary>
    /// Base class containing facet information returned by the Loop54 e-commerce search engine.
    /// </summary>
    public abstract class Facet
    {
        /// <summary>
        /// The name of the facet (as set in the request). If name is not specified in the request parameter this will be the requested attributeName.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The type of the facet (as set in the request).
        /// </summary>
        public abstract FacetType Type { get; }

        /// <summary>
        /// Whether or not the facet have any options.
        /// </summary>
        public abstract bool HasValues { get; }

        /// <summary>
        /// Tries to cast the facet to a <see cref="DistinctFacet"/>. If the Facet is not of type <see cref="FacetType.Distinct"/> it will return null.
        /// </summary>
        /// <returns>The facet as a <see cref="DistinctFacet"/>. If the Facet is not of type <see cref="FacetType.Distinct"/> it will return null.</returns>
        public DistinctFacet AsDistinct() => this as DistinctFacet;

        /// <summary>
        /// Tries to cast the facet to a <see cref="RangeFacet"/>. If the Facet is not of type <see cref="FacetType.Range"/> it will return null.
        /// </summary>
        /// <returns>The facet as a <see cref="RangeFacet"/>. If the Facet is not of type <see cref="FacetType.Range"/> it will return null.</returns>
        public RangeFacet AsRange() => this as RangeFacet;

        internal static T GetValueOrDefault<T>(JToken token)
        {
            if (token == null)
                return default;

            return token.ToObject<T>();
        }
    }

    /// <summary>
    /// This class provides results for a distinct facet. A distinct facet consists of a finite number of 
    /// options with the number of connected entities as the values. Only entities connected to selected 
    /// facet options are returned. Or all if none are selected.
    /// </summary>
    public class DistinctFacet : Facet
    {
        /// <summary>
        /// The type of the facet (as set in the request).
        /// </summary>
        public override FacetType Type => FacetType.Distinct;

        /// <summary>
        /// Whether or not the facet have any options.
        /// </summary>
        public override bool HasValues => Items.Count > 0;

        /// <summary>
        /// The options found for this facet.
        /// </summary>
        public IList<DistinctFacetItem> Items { get; set; }
        
        /// <summary>
        /// Represents a facet option found in the result set.
        /// </summary>
        public class DistinctFacetItem
        {
            internal JToken Item { get; set; }

            /// <summary>
            /// Gets the facet option as the type provided.
            /// </summary>
            /// <typeparam name="T">The type of the expected facet value.</typeparam>
            /// <returns>The facet option as the type provided.</returns>
            public T GetItem<T>() => GetValueOrDefault<T>(Item);

            /// <summary>
            /// Number of entities belonging to this option.
            /// </summary>
            public int Count { get; set; }

            /// <summary>
            /// Whether or not this option was marked selected in the request.
            /// </summary>
            public bool Selected { get; set; }
        }
    }

    /// <summary>
    /// This class provides results for a range facet. A facet that has a min and max value and only entities 
    /// in between them are returned.
    /// </summary>
    public class RangeFacet : Facet
    {
        internal JToken Min { get; set; }

        internal JToken Max { get; set; }

        internal JToken SelectedMin { get; set; }

        internal JToken SelectedMax { get; set; }

        /// <summary>
        /// The type of the facet (as set in the request).
        /// </summary>
        public override FacetType Type => FacetType.Range;

        /// <summary>
        /// Whether or not the facet have any options.
        /// </summary>
        public override bool HasValues => Min != null || Max != null;

        /// <summary>
        /// Gets the minimum value of the facet.
        /// </summary>
        /// <typeparam name="T">The type of the expected facet value.</typeparam>
        /// <returns>Minimum value of the facet.</returns>
        public T GetMin<T>() => GetValueOrDefault<T>(Min);

        /// <summary>
        /// Gets the maximum value of the facet.
        /// </summary>
        /// <typeparam name="T">The type of the expected facet value.</typeparam>
        /// <returns>Maximum value of the facet.</returns>
        public T GetMax<T>() => GetValueOrDefault<T>(Max);

        /// <summary>
        /// Gets the minimum selected value of the facet, as provided in the request.
        /// </summary>
        /// <typeparam name="T">The type of the expected facet value.</typeparam>
        /// <returns>Minimum selected value of the facet.</returns>
        public T GetSelectedMin<T>() => GetValueOrDefault<T>(SelectedMin);

        /// <summary>
        /// Gets the maximum selected value of the facet, as provided in the request.
        /// </summary>
        /// <typeparam name="T">The type of the expected facet value.</typeparam>
        /// <returns>Maximum selected value of the facet.</returns>
        public T GetSelectedMax<T>() => GetValueOrDefault<T>(SelectedMax);
    }
}
