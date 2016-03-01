using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.UI;
using Microsoft.SharePoint;

namespace Akumina.WebParts.ImportantDates
{
    [ToolboxItemAttribute(false)]
    public partial class ImportantDates : ImportantDatesBaseWebPart
    {
        private string _customList = "";
        private const string StartDate = "StartDate";
        private const string TitleLink = "DetailsLink";
        private const string SortField = "StartDate";
        private const string MoreWindow = "MoreWindow";
        private const string SubText = "Subtext";
        private const string ExpiresField = "Expires";

        //public ImportantDates()
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
            DisplayList();
        }

        #region private

        private void DisplayList()
        {
            try
            {
                var items = new ImportantDatesModel();
                List<ImportantDatesModel.ImportantDatesItemsModel> list = GetList();
                items.Items = list;
                items.WebPartTitle = Title;
                items.WebPartIcon = GetIcon(Icon);
                items.ShowHeader = Title != "" || GetIcon(Icon) != "none";
                ContentListItem.Text = Serialize(items);

                ItemListTemplate.Text = Serialize(new List<string> { Resources.ItemListTemplate });

                if (IsSearchCrawler())
                {
                    var html = new StringBuilder();
                    items.Items.ForEach(x => html.AppendFormat(@" <a href=""{0}"">{1}</a> {2} ", x.LinkSrc, x.LinkText, x.SubText));
                    __searchCrawlFeed.Text = html.ToString();
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }

        private List<ImportantDatesModel.ImportantDatesItemsModel> GetList()
        {
            var list = new List<ImportantDatesModel.ImportantDatesItemsModel>();
            try
            {
                SPList spList = SPContext.Current.Web.Lists[_customList];
                if (spList != null)
                {
                    var moderation = spList.EnableModeration && !IsPreview;
                    var query = new SPQuery
                    {
                        Query = GetQuery(moderation && !spList.EnableVersioning),
                        RowLimit = (uint)ItemsToDisplay
                    };
                    var result = spList.GetItems(query);
                    var items = from SPListItem dates in result select dates;
                    IEnumerable<SPListItem> rows = items as IList<SPListItem> ?? items.ToList();
                    if (rows.Any())
                    {
                        DataColumnCollection columns = result.GetDataTable().Columns;

                        if (columns.Contains("DoNotDisplayThisItem"))
                        {
                            foreach (var row in rows)
                            {
                                if ((bool)row["DoNotDisplayThisItem"]) continue;
                                if (spList.EnableVersioning)
                                {
                                    var versionedRow = GetPreview(IsPreview, row);
                                    if (versionedRow.Count == 0) continue;
                                    string startDate = GetValue(StartDate, versionedRow, columns);

                                    string titleLink =
                                        !string.IsNullOrEmpty(GetValue(TitleLink, versionedRow, columns))
                                            ? new SPFieldUrlValue(GetValue(TitleLink, versionedRow, columns)).Url
                                            : string.Empty;
                                    string title = GetValue("Title", versionedRow, columns);
                                    string target = GetValue(MoreWindow, versionedRow, columns) == "New Window"
                                        ? "_blank"
                                        : "_self";
                                    string subtext = GetValue(SubText, versionedRow, columns);
                                    string expires = GetValue(ExpiresField, versionedRow, columns);
                                    var item = new ImportantDatesModel.ImportantDatesItemsModel()
                                    {
                                        LinkText = title,
                                        LinkSrc = titleLink,
                                        Target = target,
                                        SubText = subtext,
                                        HasLink = titleLink != string.Empty
                                    };

                                    DateTime today = DateTime.Now;
                                    if (!string.IsNullOrEmpty(startDate))
                                    {
                                        DateTime dtCreated = Convert.ToDateTime(startDate);
                                        item.FullDate = dtCreated.ToString("yyyy-MM-dd");
                                        item.DateMonth = dtCreated.ToString("MMMM").Substring(0, 3);
                                        item.DateDay = dtCreated.Day.ToString("00");
                                    }

                                    if (!string.IsNullOrEmpty(expires))
                                    {
                                        if (Convert.ToDateTime(expires) <= today)
                                            continue;
                                    }
                                    list.Add(item);
                                }
                                else
                                {
                                    if (moderation && GetValue(row, "_ModerationStatus") != "0")
                                    {
                                        continue;
                                    }

                                    string startDate = GetValue(StartDate, row, columns);

                                    string titleLink =
                                        !string.IsNullOrEmpty(GetValue(TitleLink, row, columns))
                                            ? new SPFieldUrlValue(GetValue(TitleLink, row, columns)).Url
                                            : string.Empty;
                                    string title = GetValue("Title", row, columns);
                                    string target = GetValue(MoreWindow, row, columns) == "New Window"
                                        ? "_blank"
                                        : "_self";
                                    string subtext = GetValue(SubText, row, columns);
                                    string expires = GetValue(ExpiresField, row, columns);
                                    var item = new ImportantDatesModel.ImportantDatesItemsModel()
                                    {
                                        LinkText = title,
                                        LinkSrc = titleLink,
                                        Target = target,
                                        SubText = subtext,
                                        HasLink = titleLink != string.Empty
                                    };

                                    DateTime today = DateTime.Now;
                                    if (!string.IsNullOrEmpty(startDate))
                                    {
                                        DateTime dtCreated = Convert.ToDateTime(startDate);
                                        item.FullDate = dtCreated.ToString("yyyy-MM-dd");
                                        item.DateMonth = dtCreated.ToString("MMMM").Substring(0, 3);
                                        item.DateDay = dtCreated.Day.ToString("00");
                                    }

                                    if (!string.IsNullOrEmpty(expires))
                                    {
                                        if (Convert.ToDateTime(expires) <= today)
                                            continue;
                                    }
                                    list.Add(item);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                // ignored
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
                ? string.Format(@"<OrderBy><FieldRef Name='{0}' Ascending='True'/></OrderBy>{1}", SortField, whereClause)
                : "";
        }
        protected override void RenderContents(HtmlTextWriter writer)
        {
            if (string.IsNullOrEmpty(RootResourcePath))
                RootResourcePath = "/_layouts/15/Akumina.WebParts.ImportantDates";

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

            var styles = Resources.CssLibrary.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var style in styles)
            {
                WriteCss(writer, style);
            }
        }

        #endregion
    }
}