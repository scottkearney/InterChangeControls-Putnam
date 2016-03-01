using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.UI;
using Microsoft.SharePoint;

namespace Akumina.WebParts.Announcement.AnnouncementItems
{
    [ToolboxItem(false)]
    public partial class AnnouncementItems : AnnouncementItemsBaseWebPart
    {
        private string _customList = "";
        private const string SummaryField = "Summary";
        private const string UrlField = "FriendlyUrl";
        private const string SortField = "Created";
        private const string ExpiresField = "Expires";
        private const string CreatedField = "Created";
        private const string StartDateField = "Start_x0020_Date";

        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        //public AnnouncementItems()
        //{
        //}

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            UniqueId = ID.ToLower().Replace(" ", "");
            targetList.Attributes["class"] = UniqueId;

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
            DisplayList();
        }


        #region private

        private void DisplayList()
        {
            var items = new AnnouncementItemsListModel();
            List<AnnouncementItemsModel> list = GetList();
            items.Items = list;
            items.WebPartTitle = Title;
            items.WebPartIcon = GetIcon(Icon);
            items.ShowHeader = Title != "" || GetIcon(Icon) != "none";
            items.ViewAllLink = GetLink(ViewAllLink);
            ContentListItem.Text = Serialize(items);

            switch (DisplayTemplate)
            {
                case DisplayTemplate.WidgetTemplate:
                    ItemListTemplate.Text = Serialize(new List<string> { Resources.ItemListTemplate });
                    break;
                case DisplayTemplate.PageTemplate:
                    ItemListTemplate.Text = Serialize(new List<string> { Resources.PageItemListTemplate });
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (IsSearchCrawler())
            {
                var html = new StringBuilder();
                list.ForEach(x => html.AppendFormat(@" <a href=""{2}"">{0}</a> {1} ", x.Title, x.Summary, x.Url));
                __searchCrawlFeed.Text = html.ToString();
            }
        }

        private List<AnnouncementItemsModel> GetList()
        {
            var list = new List<AnnouncementItemsModel>();
            try
            {
                SPList spList = SPContext.Current.Web.Lists[_customList];
                if (spList != null)
                {
                    var moderation = spList.EnableModeration && !IsPreview;
                    var maxList = new AnnouncementItemsListModel { RowLimit = ItemsToDisplay };
                    var query = new SPQuery { Query = GetQuery(moderation && !spList.EnableVersioning) };
                    if (maxList.RowLimit > 0)
                    {
                        query.RowLimit = (uint)(maxList.RowLimit);
                    }
                    SPListItemCollection items = spList.GetItems(query);

                    foreach (SPListItem item in items)
                    {
                        string title = "";
                        string summary = "";
                        string url = "";
                        string expires = "";
                        string created = "";
                        string startDate = "";
                        if (items.List.EnableVersioning)
                        {
                            var listItem = GetPreview(IsPreview, item);
                            if (listItem.Count > 0)
                            {
                                title = GetValue(listItem, "AnnouncementTitle");
                                summary = GetValue(listItem, SummaryField);
                                url = GetValue(listItem, UrlField).Replace("string;#", "");
                                expires = GetValue(listItem, ExpiresField);
                                created = GetValue(listItem, CreatedField);
                                startDate = GetValue(listItem, StartDateField);
                            }
                        }
                        else
                        {
                            if (moderation && GetValue(item, "_ModerationStatus") != "0")
                            {
                                continue;
                            }
                            title = GetValue(item, "AnnouncementTitle");
                            summary = GetValue(item, SummaryField);
                            url = GetValue(item, UrlField).Replace("string;#", "");
                            expires = GetValue(item, ExpiresField);
                            created = GetValue(item, CreatedField);
                            startDate = GetValue(item, StartDateField);
                        }
                        var itemAnnoucement = new AnnouncementItemsModel()
                        {
                            Title = title,
                            Summary = summary,
                            Url = url
                        };
                        if (!string.IsNullOrEmpty(created))
                        {
                            DateTime dtCreated = Convert.ToDateTime(created);
                            itemAnnoucement.Created = dtCreated.ToString("MMMM dd, yyyy");
                        }

                        if (!string.IsNullOrEmpty(expires))
                        {
                            DateTime dtExpires = Convert.ToDateTime(expires);
                            if (dtExpires <= DateTime.Now)
                                continue;
                        }
                        if (!string.IsNullOrEmpty(startDate))
                        {
                            if (Convert.ToDateTime(startDate) > DateTime.Now)
                                continue;
                        }
                        list.Add(itemAnnoucement);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return list;
        }

        private static string GetQuery(bool moderation)
        {
            var whereClause = "";
            if (moderation)
            {
                whereClause = @"<Where><Eq><FieldRef Name='_ModerationStatus' /><Value Type='Number'>0</Value></Eq></Where>";
            }

            return !string.IsNullOrEmpty(SortField)
                ? string.Format(@"<OrderBy><FieldRef Name='{0}' Ascending='False'/></OrderBy>{1}", SortField, whereClause)
                : "";
        }

        private string GetLink(string pageUrl)
        {
            string currentUrl = HttpContext.Current.Request.Url.ToString();
            string redirectUrl = currentUrl.Substring(0, (currentUrl.LastIndexOf("/", StringComparison.Ordinal)));
            pageUrl = redirectUrl + "/" + pageUrl;
            return pageUrl;
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

        #endregion
    }
}
