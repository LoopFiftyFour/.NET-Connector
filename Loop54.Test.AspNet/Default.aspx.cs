using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Loop54.Test.AspNet
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            Search(QueryText.Text);
        }

        private void Search(string query)
        {
            RequestLabel.Text = "";
            ResponseLabel.Text = "";

            var request = CreateRequest("Search", "QueryString", query);
            RequestLabel.Text = request.Serialized;

            var response = GetResponse(request);

            if (response.Success)
            {
                var directItems = response.GetValue<double>("DirectResults_TotalItems");
                ResponseLabel.Text = directItems + " direct results found.";
            }
            else
            {
                ResponseLabel.Text = "Error!";
            }
        }

        private static Request CreateRequest(string requestName, string key, string value)
        {
            var request = new Request(requestName);
            request.SetValue(key, value);
            return request;
        }

        private static Response GetResponse(Request request)
        {
            return RequestHandling.GetResponse("http://helloworld.54proxy.com", request);
        }
    }
}
