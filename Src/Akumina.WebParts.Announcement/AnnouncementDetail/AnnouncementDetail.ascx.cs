using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using Microsoft.SharePoint;
using System.Web.UI.WebControls;
using Akumina.InterAction;

namespace Akumina.WebParts.Announcement.AnnouncementDetail
{
    [ToolboxItem(false)]
    public partial class AnnouncementDetail : AnnouncementDetailBaseWebPart
    {
        private string _customList = "";
        private const string BodyField = "Body";
        private const string TitleField = "AnnouncementTitle";
        private const string SeoTitleField = "Seo_x002d_Title";
        private const string SeoDescriptionField = "Seo_x002d_Description";
        private const string SeoKeywordsField = "SEO_x002d_Keywords";
        private const string ExpiresField = "Expires";
        private const string StartDateField = "Start_x0020_Date";
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        //public AnnouncementDetail()
        //{
        //}

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack) return;
            if (!SPContext.Current.IsDesignTime && !string.IsNullOrWhiteSpace(InstructionSet))
            {
                try
                {
                    MapInstructionSetToProperties(GetInstructionSet(InstructionSet), this);
                }
                catch (Exception)
                {
                    // ignored
                }
            }

            _customList = ListName;

            var item = GetItem();

            if (item == null) return;
            ContentListItem.Text = Serialize(item);
            ItemDetailTemplate.Text = Serialize(new List<string> { Resources.ItemDetailTemplate });
            SetSeoMeta(item);

            if (IsSearchCrawler())
            {
                __searchCrawlFeed.Text = item.Body;
            }
        }

        private void SetSeoMeta(AnnouncementDetailModel model)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(model.SEOTitle) || !string.IsNullOrWhiteSpace(model.Title))
                {
                    var metaTitle = new System.Web.UI.HtmlControls.HtmlMeta
                    {
                        Name = "title",
                        Content = model.SEOTitle ?? model.Title
                    };
                    Page.Header.Controls.AddAt(1, metaTitle);
                }
                if (!string.IsNullOrWhiteSpace(model.SEODescription))
                {
                    var metaDescription = new System.Web.UI.HtmlControls.HtmlMeta
                    {
                        Name = "description",
                        Content = model.SEODescription
                    };
                    Page.Header.Controls.AddAt(1, metaDescription);
                }
                if (!string.IsNullOrWhiteSpace(model.SEOKeywords))
                {
                    var metaKeywords = new System.Web.UI.HtmlControls.HtmlMeta
                    {
                        Name = "keywords",
                        Content = model.SEOKeywords
                    };
                    Page.Header.Controls.AddAt(1, metaKeywords);
                }
            }
            catch (Exception ex)
            {
                Logging.LogError(ex);
            }
        }
        private AnnouncementDetailModel GetItem()
        {
            var item = new AnnouncementDetailModel();

            try
            {
                string queryTitle = GetTitle();
                SPList spList = SPContext.Current.Web.Lists[_customList];

                if (spList != null)
                {
                    var listItem = GetItem(_customList, queryTitle, IsPreview, true);
                    if (listItem.Count > 0)
                    {
                        string title = "";
                        string body = "";

                        string seoTitle = "";
                        string seoDescription = "";
                        string seoKeywords = "";
                        string expires = "";
                        string startDate = "";
                        if (listItem.ContainsKey(ExpiresField))
                        {
                            expires = listItem[ExpiresField].ToString();
                        }
                        if (listItem.ContainsKey(StartDateField))
                        {
                            startDate = listItem[StartDateField].ToString();
                        }                        
                        DateTime today = DateTime.Now;
                        if (!string.IsNullOrEmpty(expires))
                        {
                            if (Convert.ToDateTime(expires) <= today )
                                return null;
                        }
                        if (!string.IsNullOrEmpty(startDate))
                        {
                            if (Convert.ToDateTime(startDate) > today)
                                return null;
                        }
                        if (listItem.ContainsKey(TitleField))
                        {
                            title = listItem[TitleField].ToString();
                        }
                        if (listItem.ContainsKey(BodyField))
                        {
                            body = listItem[BodyField].ToString();
                        }

                        if (listItem.ContainsKey(SeoTitleField))
                        {
                            seoTitle = listItem[SeoTitleField].ToString();
                        }
                        if (listItem.ContainsKey(SeoDescriptionField))
                        {
                            seoDescription = listItem[SeoDescriptionField].ToString();
                        }
                        if (listItem.ContainsKey(SeoKeywordsField))
                        {
                            seoKeywords = listItem[SeoKeywordsField].ToString();
                        }
                        item.Body = body;
                        item.Title = title;
                        if (string.IsNullOrEmpty(seoTitle))
                            seoTitle = title;
                        item.SEOTitle = seoTitle;
                        item.SEODescription = Regex.Replace(seoDescription, @"<[^>]+>|&nbsp;", "").Trim();
                        item.SEOKeywords = seoKeywords;

                        SetPageTitles(seoTitle);
                    }
                    else
                    {
                        item = null;
                    }
                }
            }
            catch (Exception)
            {
                item = null;
            }

            return item;
        }


        private static string GetTitle()
        {
            var title = HttpContext.Current.Request.RawUrl.Split('/').Last();

            if (string.IsNullOrEmpty(title)) return title;
            var queryPart = title.IndexOf("?", StringComparison.Ordinal);
            if (queryPart > -1)
            {
                title = title.Substring(0, queryPart);
            }
            return title;
        }

        protected void SetPageTitles(string pageTitle)
        {
            try
            {
                if (Page.Master == null) return;
                var contentPlaceHolder = (ContentPlaceHolder)Page.Master.FindControl("PlaceHolderPageTitle");
                contentPlaceHolder.Controls.Clear();
                var title = new LiteralControl { Text = pageTitle };
                contentPlaceHolder.Controls.Add(title);

                contentPlaceHolder = (ContentPlaceHolder)Page.Master.FindControl("PlaceHolderPageTitleInTitleArea");
                contentPlaceHolder.Controls.Clear();
                title = new LiteralControl { Text = pageTitle };
                contentPlaceHolder.Controls.Add(title);
            }
            catch
            {
                // ignored
            }
        }

        protected override void RenderContents(HtmlTextWriter writer)
        {
            if (string.IsNullOrEmpty(RootResourcePath))
                RootResourcePath = "/_layouts/15/Akumina.WebParts.Announcement";

            var scripts = Resources.JsLibrary.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var script in scripts)
            {
                if (script.ToLower().Contains("jquery"))
                {
                    writer.WriteLine("<script type=\"text/javascript\">");
                    writer.WriteLine("if (typeof jQuery == 'undefined') { document.write(unescape(\"%3Cscript src='" + RootResourcePath + script + "' type='text/javascript'%3E%3C/script%3E\")); }");
                    writer.WriteLine("</script>");
                }
                else
                {
                    WriteJs(writer, script);
                }
            }
            base.RenderContents(writer);
            var styles = Resources.CssDetail.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var style in styles)
            {
                WriteCss(writer, style);
            }
        }
    }
}
