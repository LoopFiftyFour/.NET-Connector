using System.Collections.Generic;

namespace Loop54.Test.AspNetMvc.Models
{
    public class SearchViewModel
    {
        public bool MakesSense { get; set; }
        public int Count { get; set; }
        public IList<ProductViewModel> Results { get; set; }
        public IList<FacetViewModel> Facets { get; internal set; }
        public int RelatedCount { get; set; }
        public IList<ProductViewModel> RelatedResults { get; set; }

        public IList<string> RelatedQueries { get; set; }
        public IList<string> SpellingSuggestions { get; set; }
        public string ResponseMessage { get; set; }
        public string Redirect { get; set; }
    }
}
