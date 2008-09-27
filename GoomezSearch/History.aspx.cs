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
    public partial class History : System.Web.UI.Page
    {
        IndexHelper searcher = new IndexHelper(HttpContext.Current.Server.MapPath(Constants.SearchIndexName), HttpContext.Current.Server.MapPath(Constants.HistoryIndexName));

        protected void Page_Load(object sender, EventArgs e)
        {
            searchText.Focus();

            if (!IsPostBack)
            {
                DisplayDay(DateTime.Today);
                lblResult.Text = String.Format(Messages.searchHistoryFor, Page.User.Identity.Name);
            }
        }

        protected void Calendar1_SelectionChanged(object sender, EventArgs e)
        {
            DisplayDay(((Calendar)sender).SelectedDate);
        }

        private void DisplayDay(DateTime date)
        {
            DataList1.DataSource = searcher.GetHistoryByUserDate(date);
            DataList1.DataBind();
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string search = searchText.Text.Trim().Replace("'", "");
            if (search.Length == 0)
                return;

            Response.Redirect("Default.aspx?search=" + search);
        }

        private void SearchHistory(string pattern)
        {
            DataList1.DataSource = searcher.SearchHistoryByUserPattern(pattern);
            DataList1.DataBind();
        }
        protected void btnSearchHistory_Click(object sender, EventArgs e)
        {
            string search = searchText.Text.Trim().Replace("'", "");
            if (search.Length == 0)
                return;

            SearchHistory(search);

        }
    }
}
