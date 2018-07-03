using System.Collections.Generic;

namespace Loop54.Test.AspNetMvc.Models
{
    public class GetEntitiesViewModel
    {
        public int Count { get; set; }
        public IList<ProductViewModel> Results { get; set; }
        public IList<FacetViewModel> Facets { get; internal set; }
    }
}
