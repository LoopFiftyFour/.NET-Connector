using System;
using System.Collections.Generic;
using System.Text;

namespace Loop54.Model.Request.Parameters.Filters
{
    /// <summary>
    /// How values should be compared to the attribute values of the entities being filtered.
    /// </summary>
    public enum FilterComparisonMode
    {
        Equals,
        GreaterThan,
        GreaterThanOrEquals,
        LessThan,
        LessThanOrEquals,
        Contains
    }
}
