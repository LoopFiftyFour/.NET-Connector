using Loop54.AspNet;
using Loop54.Model.Request;
using Loop54.Model.Response;
using Loop54.Serialization;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Loop54.Test.AspNet
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Loop54ClientManager.StartUp("https://helloworld.54proxy.com");
        }

        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            Search(QueryText.Text);
        }

        private void Search(string query)
        {
            RequestLabel.Text = "";
            ResponseLabel.Text = "";

            SearchRequest request = new SearchRequest(query);

            try
            { 
                SearchResponse response = Loop54ClientManager.Client().Search(request);
                ResponseLabel.Text = response.Results.Count + " direct results found.";
            }
            catch(Exception e)
            {
                ResponseLabel.Text = "Error! " + e.ToString();
            }
        }
    }
}
