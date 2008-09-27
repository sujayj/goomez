using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GoomezSearchHelper;
using GoomezSearch.App_GlobalResources;

namespace GoomezSearch
{
    public partial class NoResults : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.ClientScript.RegisterHiddenField("__EVENTTARGET", "btnSearch");
            txtSearch.Focus();
            if (!IsPostBack)
            {
                string searchedText = Page.Request["search"];
                if (searchedText != null && searchedText.Length != 0)
                {
                    IndexHelper index = new IndexHelper(HttpContext.Current.Server.MapPath(Constants.SearchIndexName), HttpContext.Current.Server.MapPath(Constants.HistoryIndexName));
                    string mean = index.DidYouMean(searchedText);

                    if (mean != "")
                        lblDidYouMean.Text = String.Format(Messages.didYouMean, "<a href='Default.aspx?search=" + mean + "'><i><b>" + mean + "</b></i></a>");
                    else
                        lblDidYouMean.Text = "";

                    if (searchedText == "soup")
                    {
                        lblMessage.Text = "NO SOUP FOR YOU!";
                        imageNazi.Visible = true;
                    }
                    else
                    {
                        lblMessage.Text = String.Format(Messages.noResults, searchedText);
                        imageNazi.Visible = false;
                    }
                    txtSearch.Text = searchedText;
                    searchedText = searchedText.Replace(" ", "%20");
                    lblGoogle.Text = String.Format(Messages.tryGoogle, "<a href='http://www.google.com/search?q=" + searchedText + "'><i>Google</i></a>", "<a href='http://www.google.com/search?q=" + searchedText + "&btnI=I%27m+Feeling+Lucky'><i>" + Messages.feelingLucky + "</i></a>");
                }

            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string pattern = txtSearch.Text.Trim();
            if (!string.IsNullOrEmpty(pattern))
                Response.Redirect("Default.aspx?search=" + pattern);
        }
    }
}
