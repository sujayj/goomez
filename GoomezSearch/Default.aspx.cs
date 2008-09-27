using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using GoomezSearchHelper;
using System.Threading;
using GoomezSearch.App_GlobalResources;
using System.Globalization;

namespace GoomezSearch
{
    public partial class Default : System.Web.UI.Page
    {
        IndexHelper m_searcher = new IndexHelper(HttpContext.Current.Server.MapPath(Constants.SearchIndexName), HttpContext.Current.Server.MapPath(Constants.HistoryIndexName));
        PagedDataSource m_pager;

        protected void Page_Load(object sender, EventArgs e)
        {
            txtSearch.Focus();

            if (!IsPostBack)
            {
                int browseVersion  = 0;
                if (Request.Browser.Version.Contains("."))
                    browseVersion = int.Parse(Request.Browser.Version.Substring(0, Request.Browser.Version.IndexOf(".")));
                else
                    browseVersion = int.Parse(Request.Browser.Version);

                if (Request.Browser.Browser == "IE")
                {
                    if (browseVersion >= 7)
                        ie7Label.Visible = true;

                    if (browseVersion >= 8)
                        ie8Label.Visible = true;
                }
                else if (Request.Browser.Browser == "Firefox")
                {
                    ffLablel.Visible = true;
                }

                string searchedText = Page.Request["search"];
                if (searchedText != null && searchedText.Length != 0)
                {
                    IndexSearch(searchedText);
                    txtSearch.Text = searchedText;
                }
                string move = Page.Request["move"];
                int index;
                if (int.TryParse(move, out index))
                    Navigate(index);

                string manya = ConfigurationManager.AppSettings["manya"];
                if (string.IsNullOrEmpty(manya))
                    return;

                if (manya.ToLower() != Boolean.TrueString.ToLower() && manya.ToLower() != Boolean.FalseString.ToLower())
                    return;

                if (Boolean.Parse(manya))
                {
                    imageEscudo.Visible = true;
                    imageLogo.ImageUrl = "~/images/manyaGoomez.jpg";
                }

                string title = ConfigurationManager.AppSettings["title"];
                if (string.IsNullOrEmpty(title))
                    return;

                this.Title = title;

            }
        }

        private void Navigate(int pageIndex)
        {
            List<IndexedFile> list = (List<IndexedFile>)Session["resultList"];

            if (list == null)
                return;

            m_pager = new PagedDataSource();
            m_pager.AllowPaging = true;
            m_pager.DataSource = list;
            m_pager.PageSize = Constants.PageSize;
            m_pager.CurrentPageIndex = pageIndex;

            SetPager();
            listResult.DataSource = m_pager;
            listResult.DataBind();
            string resultText = (string)Session["resultText"];
            txtSearch.Text = (string)Session["pattern"];
            lblResult.Text = resultText;
            lblResult.Visible = true;
            tableHeader.Visible = true;
            tableFooter.Visible = true;
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string search = txtSearch.Text.Trim();

            if (search.Length == 0)
                return;

            //IndexSearch(search);
            Response.Redirect("~/Default.aspx?search=" + search);
        }

        private void IndexSearch(string pattern)
        {
            DateTime started = DateTime.Now;

            List<IndexedFile> list = m_searcher.Search(pattern);

            int count = list.Count;

            if (count != 0)
            {
                Thread saveThread = new Thread(SaveSearch);
                saveThread.Start(pattern);
            }
            else
            {
                Response.Redirect("NoResults.aspx?search=" + pattern);
            }

            m_pager = new PagedDataSource();
            m_pager.AllowPaging = true;
            m_pager.DataSource = list;
            m_pager.PageSize = Constants.PageSize;
            m_pager.CurrentPageIndex = 0;

            if (m_pager.PageCount > 1)
                Session["resultList"] = list;

            SetPager();
            listResult.DataSource = m_pager;
            listResult.DataBind();


            DateTime ended = DateTime.Now;
            TimeSpan diff = ended - started;

            string resultText = String.Format(Messages.results, count.ToString(), pattern, diff.TotalSeconds.ToString("#0.##"));
            Session["resultText"] = resultText;
            Session["pattern"] = pattern;
            lblResult.Text = resultText;
            lblResult.Visible = true;
            tableHeader.Visible = true;
            tableFooter.Visible = true;
        }

        private void SetPager()
        {
            lblCurrent.Visible = true;
            lblFirst.Visible = true;
            lblPrevious.Visible = true;
            lblNext.Visible = true;
            lblLast.Visible = true;

            lblCurrent.Text = string.Format(Messages.pager, m_pager.CurrentPageIndex + 1, m_pager.PageCount);
            lblFirst.Text = "<a href='Default.aspx?move=0'>" + Messages.first + "</a>";
            lblPrevious.Text = "<a href='Default.aspx?move=" + (m_pager.CurrentPageIndex - 1).ToString() + "'>" + Messages.previous + "</a>";
            lblNext.Text = "<a href='Default.aspx?move=" + (m_pager.CurrentPageIndex + 1).ToString() + "'>" + Messages.next + "</a>";
            lblLast.Text = "<a href='Default.aspx?move=" + (m_pager.PageCount - 1).ToString() + "'>" + Messages.last + "</a>";

            if (m_pager.PageCount == 1)
            {
                lblFirst.Text = "";
                lblPrevious.Text = "";
                lblNext.Text = "";
                lblLast.Text = "";

                lblCurrent.Visible = false;
                lblFirst.Visible = false;
                lblPrevious.Visible = false;
                lblNext.Visible = false;
                lblLast.Visible = false;

                return;
            }

            if (m_pager.CurrentPageIndex == 0)
            {
                lblFirst.Text = Messages.first;
                lblPrevious.Text = Messages.previous;
                return;
            }

            if (m_pager.CurrentPageIndex == (m_pager.PageCount - 1))
            {
                lblNext.Text = Messages.next;
                lblLast.Text = Messages.last;
                return;
            }
        }

        private void SaveSearch(Object searchedText)
        {
            m_searcher.SaveSearchHistory((string)searchedText);
        }
    }
}
