using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web.UI;
using Akumina.InterAction;
using Microsoft.SharePoint;

namespace Akumina.WebParts.ContentBlock.ContentBlock
{
    [ToolboxItem(false)]
    public partial class ContentBlock : ContentBlockBaseWebPart
    {
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        //public ContentBlock()
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
            if (string.IsNullOrEmpty(RootResourcePath))
            {
                RootResourcePath = SPContext.Current.Web.Site.RootWeb.Url + "/Akumina.WebParts.ContentBlock";
            }
            string isAlert = string.Empty;
            string itemTitle = string.Empty;
            string itemHtml = string.Empty;
            string expires = string.Empty;
            int itemId = 0;

            string contentIdQueryString = string.Empty;
            if (Page.Request.QueryString[DynamicParam] != null)
            {
                contentIdQueryString = Page.Request.QueryString[DynamicParam];
            }

            if (!string.IsNullOrWhiteSpace(QueryPart))
            {
                var parts = QueryPart.Split(new[] { '.' }, 3);
                Dictionary<string, object> listItem;
                string itemTitleQueryPart;
                if (parts.Length == 3 && parts[0].Equals("~"))
                {
                    itemTitleQueryPart = parts[1];
                    if (DynamicPreview && !string.IsNullOrEmpty(contentIdQueryString))
                    {
                        itemTitleQueryPart = contentIdQueryString;
                    }
                    listItem = GetItem(true, itemTitleQueryPart, parts[2]);
                }
                else
                {
                    itemTitleQueryPart = parts[1];
                    if (DynamicPreview && !string.IsNullOrEmpty(contentIdQueryString))
                    {
                        itemTitleQueryPart = contentIdQueryString;
                    }
                    listItem = GetItem(parts[0], itemTitleQueryPart, IsPreview, true);
                }
                if (listItem == null || listItem.Count<1)
                {
                    Logging.LogError(string.Format("No items found for {0}", QueryPart));
                    return;
                }

                if (listItem.ContainsKey("Expires") && listItem["Expires"] != null)
                {
                    expires = listItem["Expires"].ToString();
                }
                isAlert = (string)listItem["IsAlert"];
                itemId =  Convert.ToInt32(listItem["ID"]);
                itemTitle = listItem["Title"] as string;
                itemHtml = (string)listItem["Html"];
            }
            DateTime today = DateTime.Now;
            DateTime dtExpires = today.AddDays(1);


            if (Expires != DateTime.MinValue)
            {
                  dtExpires = Convert.ToDateTime(Expires);
            }
            else if (!string.IsNullOrEmpty(expires))
            {
                dtExpires = Convert.ToDateTime(expires);
            }            
        
            var uniqueId = GetUniqueId();
            ContentListItemId.Text = uniqueId;

            if (DisableAlert || (Convert.ToDateTime(dtExpires) < today))
            {
                ContentListItem.Text = Serialize(new ContentBlockModel
                {
                    UniqueId = uniqueId,
                    Id = itemId,
                    Title = "",
                    Html = ""
                });
                ContentListItemTemplate.Text = Serialize(new List<string> { "<div></div>" });
            }
            else
            {
                ContentListItem.Text = Serialize(new ContentBlockModel
                {
                    UniqueId = uniqueId,
                    Id = itemId,
                    Title = itemTitle,
                    Html = string.IsNullOrWhiteSpace(SiteWideAlertMessage) ? itemHtml : SiteWideAlertMessage,
                    ColorTheme = (isAlert.ToLower() == "1" || !string.IsNullOrWhiteSpace(SiteWideAlertMessage)) ? Themes.Alert.ToString().ToLower() : ColorTheme.ToString().ToLower(),
                    WebPartTitle = Title,
                    WebPartIcon = GetIcon(Icon),
                    ShowHeader = Title != "" || GetIcon(Icon) != "none",

                });
                if (isAlert.ToLower() == "1" || !string.IsNullOrWhiteSpace(SiteWideAlertMessage))
                {
                    
                    ContentListItemTemplate.Text = Serialize(new List<string> { Resources.AlertTemplate });
                }
                else
                {
                    ContentListItemTemplate.Text = Serialize(new List<string> { Resources.ItemTemplate });
                }
            }
            if (IsSearchCrawler())
            {
                __searchCrawlFeed.Text = string.Format("{0} {1}", itemTitle, (string.IsNullOrWhiteSpace(SiteWideAlertMessage) ? itemHtml : SiteWideAlertMessage));
            }
        }

        protected override void RenderContents(HtmlTextWriter writer)
        {
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
    }
}
