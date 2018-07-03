using System.Collections.Generic;

namespace Loop54.Test.AspNetMvc.Models
{
    public class AutoCompleteViewModel
    {
        public string Query { get; set; }
        public int Count { get; set; }
        public IList<string> Results { get; set; }
        public string ScopedQuery { get; internal set; }
        public IList<string> Scopes { get; internal set; }
    }
}
