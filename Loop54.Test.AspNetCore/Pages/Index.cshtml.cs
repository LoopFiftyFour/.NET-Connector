using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Loop54.Test.AspNetCore.Pages
{
    public class IndexModel : PageModel
    {
        public string RequestText { get; set; }
        public string ResponseText { get; set; }

        public void OnGet()
        {
        }

        public void OnPost()
        {
            Search(Request.Form["Query"]);
        }

        private void Search(string query)
        {
            RequestText = "";
            ResponseText = "";

            var request = CreateRequest("Search", "QueryString", query);
            RequestText = request.Serialized;

            var response = GetResponse(request);

            if (response.Success)
            {
                var directItems = response.GetValue<double>("DirectResults_TotalItems");
                ResponseText = directItems + " direct results found.";
            }
            else
            {
                ResponseText = "Error!";
            }
        }

        private Request CreateRequest(string requestName, string key, string value)
        {
            var request = new Request(requestName, HttpContext, null);
            request.SetValue(key, value);
            return request;
        }

        private static Response GetResponse(Request request)
        {
            return RequestHandling.GetResponse("http://helloworld.54proxy.com", request);
        }
    }
}
