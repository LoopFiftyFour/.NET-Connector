using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Loop54.Test.AspNetCore.Models
{
    public class FacetViewModel
    {
        public string Name { get; set; }
        public virtual bool IsDistinct => false;
    }

    public class DistinctFacetViewModel : FacetViewModel
    {
        public override bool IsDistinct => true;
        public Dictionary<string, int> Options { get; set; }
    }

    public class RangeFacetViewModel : FacetViewModel
    {
        public string Min { get; set; }
        public string Max { get; set; }
    }
}
