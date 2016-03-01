using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using Akumina.InterAction;
using Akumina.WebParts.Banner.Properties;
using Microsoft.SharePoint;
using Microsoft.Office.Server.Search.Query;
using Microsoft.Office.Server.Search.Administration;
using System.Data;
using System.Web.Script.Serialization;

namespace Akumina.WebParts.Banner.Banner
{
    [ToolboxItem(false)]
    public partial class Banner : BannerBaseWebPart
    {
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        //public Banner()
        //{
        //}

        private string _uniqueId = "";
        private string _textColorTheme = "";
        private string _textLocation = "";
        private string _textAlignment = "";
        private readonly List<BannerItem> _bannerItems = new List<BannerItem>();

        #region Page Events
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
                    IInstructionRepository clientService = new InstructionRepository();
                    var response = clientService.Execute(InstructionSet);
                    if (response.Dictionary.Count > 0)
                    {
                        MapInstructionSetToProperties(response, this, clientService);
                    }
                    else 
                    {
                        GetWebPartProperties();
                    }
                }
                catch (Exception)
                {
                    // ignored
                }
            }
            else
            {
                GetWebPartProperties();
            }
            if (string.IsNullOrEmpty(RootResourcePath))
            {
                RootResourcePath = SPContext.Current.Web.Site.RootWeb.Url + "/Akumina.WebParts.Banner";
            }
            if (QueryType == QueryType.SearchQuery)
            {
                GetItemsUsingSearchQuery();
            }
            else
            {
                GetItemsUsingListQuery();
            }

            var sb = new StringBuilder();
            sb.AppendLine(WriteInitialScript());
            litProperties.Text = WriteWebPartProperties();
            litTop.Text = sb.ToString();
            _uniqueId = Guid.NewGuid().ToString().Replace("-", "").ToUpper();
            litDivID.Text = _uniqueId;
            litUniqueId.Text = _uniqueId;
            litItemsResult.Text = new JavaScriptSerializer().Serialize(_bannerItems);
            if (IsSearchCrawler())
            {
                var html = new StringBuilder();
                foreach (var item in _bannerItems)
                {
                    html.AppendFormat(@"{0} {1} {2} {3} {4} {5} {6} {7} {8}", item.BannerHeadingOWSTEXT, item.BannerImageAltTextOWSTEXT, item.BannerImageOWSURLH,
                        item.BannerItemOrderOWSNMBR, item.BannerLinkHoverTextOWSTEXT, item.BannerLinkTargetOWSCHCS, item.BannerLinkTextOWSTEXT,
                        item.BannerSubHeadingOWSTEXT, item.BannerLinkUrlOWSURLH);
                }
                __searchCrawlFeed.Text = html.ToString();
            }
        }

        private void GetItemsUsingListQuery()
        {
            try
            {
                if (string.IsNullOrEmpty(ResultSourceId)) return;
                var myWeb = SPContext.Current.Web;
                var list = myWeb.Lists[ResultSourceId];

                var orderedListItems = from SPListItem banner in list.Items
                                       orderby banner["BannerItemOrder"] ascending
                                       select banner;

                var items = orderedListItems.Take(MaxSlideCount > 0 ? MaxSlideCount : 500);

                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    var moderation = list.EnableModeration && !IsPreview;
                    DataColumnCollection columns = list.Items.GetDataTable().Columns;
                    foreach (var row in items)
                    {
                        if (row.ParentList.EnableVersioning)
                        {
                            var listItem = GetPreview(IsPreview, row);
                            if (listItem.Count <= 0 || !GetValue("BannerActive", listItem, columns).Equals("true", StringComparison.InvariantCultureIgnoreCase))
                                continue;
                            var bannerHeadingOwstext = GetValue("BannerHeading", listItem, columns);
                            var bannerImageAltTextOwstext = GetValue("BannerImageAltText", listItem, columns);
                            var bannerImageOwsurlh = GetValue("BannerImage", listItem, columns);
                            var bannerItemOrderOwsnmbr = GetValue("BannerItemOrder", listItem, columns);
                            var bannerLinkHoverTextOwstext = GetValue("BannerLinkHoverText", listItem, columns);
                            var bannerLinkTargetOwschcs = GetValue("BannerLinkTarget", listItem, columns);
                            var bannerLinkTextOwstext = GetValue("BannerLinkText", listItem, columns);
                            var bannerSubHeadingOwstext = GetValue("BannerSubHeading", listItem, columns);
                            var bannerLinkUrlOwsurlh = GetValue("BannerLinkUrl", listItem, columns);
                            var expires = GetValue("Expires", listItem, columns);
                            if (!Expired(expires))
                                AddItemToList(bannerHeadingOwstext, bannerImageAltTextOwstext, bannerImageOwsurlh, bannerItemOrderOwsnmbr, bannerLinkHoverTextOwstext, bannerLinkTargetOwschcs, bannerLinkTextOwstext, bannerSubHeadingOwstext, bannerLinkUrlOwsurlh);
                        }
                        else
                        {
                            if (moderation)
                            {
                                if (GetValue("_ModerationStatus", row, columns) != "0")
                                {
                                    continue;
                                }
                            }
                            if (!GetValue("BannerActive", row, columns).Equals("true", StringComparison.InvariantCultureIgnoreCase)) continue;
                            var bannerHeadingOwstext = GetValue("BannerHeading", row, columns);
                            var bannerImageAltTextOwstext = GetValue("BannerImageAltText", row, columns);
                            var bannerImageOwsurlh = GetValue("BannerImage", row, columns);
                            var bannerItemOrderOwsnmbr = GetValue("BannerItemOrder", row, columns);
                            var bannerLinkHoverTextOwstext = GetValue("BannerLinkHoverText", row, columns);
                            var bannerLinkTargetOwschcs = GetValue("BannerLinkTarget", row, columns);
                            var bannerLinkTextOwstext = GetValue("BannerLinkText", row, columns);
                            var bannerSubHeadingOwstext = GetValue("BannerSubHeading", row, columns);
                            var bannerLinkUrlOwsurlh = GetValue("BannerLinkUrl", row, columns);
                            var expires = GetValue("Expires", row, columns);
                            if (!Expired(expires))
                                AddItemToList(bannerHeadingOwstext, bannerImageAltTextOwstext, bannerImageOwsurlh, bannerItemOrderOwsnmbr, bannerLinkHoverTextOwstext, bannerLinkTargetOwschcs, bannerLinkTextOwstext, bannerSubHeadingOwstext, bannerLinkUrlOwsurlh);
                        }
                    }
                });
            }
            catch (Exception)
            {
                // ignored
            }
        }

        private static bool Expired(string expires)
        {
            if (string.IsNullOrEmpty(expires)) return false;
            var today = DateTime.Now;
            var dtExpires = Convert.ToDateTime(expires);
            return dtExpires <= today;
        }

        private void AddItemToList(string bannerHeadingOwstext, string bannerImageAltTextOwstext, string bannerImageOwsurlh, string bannerItemOrderOwsnmbr, string bannerLinkHoverTextOwstext, string bannerLinkTargetOwschcs, string bannerLinkTextOwstext, string bannerSubHeadingOwstext, string bannerLinkUrlOwsurlh)
        {
            _bannerItems.Add(new BannerItem
            {
                BannerHeadingOWSTEXT = bannerHeadingOwstext,
                BannerImageAltTextOWSTEXT = bannerImageAltTextOwstext,
                BannerImageOWSURLH = bannerImageOwsurlh,
                BannerItemOrderOWSNMBR = bannerItemOrderOwsnmbr,
                BannerLinkHoverTextOWSTEXT = bannerLinkHoverTextOwstext,
                BannerLinkTargetOWSCHCS = bannerLinkTargetOwschcs,
                BannerLinkTextOWSTEXT = bannerLinkTextOwstext,
                BannerSubHeadingOWSTEXT = bannerSubHeadingOwstext,
                BannerLinkUrlOWSURLH = bannerLinkUrlOwsurlh
            });
        }

        private void GetItemsUsingSearchQuery()
        {
            var bannerItems = GetBannerItems();
            if (bannerItems == null) return;
            foreach (DataRow row in bannerItems.Rows)
            {
                var bannerHeadingOwstext = row["BannerHeadingOWSTEXT"].ToString();
                var bannerImageAltTextOwstext = row["BannerImageAltTextOWSTEXT"].ToString();
                var bannerImageOwsurlh = row["BannerImageOWSURLH"].ToString();
                var bannerItemOrderOwsnmbr = row["BannerItemOrderOWSNMBR"].ToString();
                var bannerLinkHoverTextOwstext = row["BannerLinkHoverTextOWSTEXT"].ToString();
                var bannerLinkTargetOwschcs = row["BannerLinkTargetOWSCHCS"].ToString();
                var bannerLinkTextOwstext = row["BannerLinkTextOWSTEXT"].ToString();
                var bannerSubHeadingOwstext = row["BannerSubHeadingOWSTEXT"].ToString();
                var bannerLinkUrlOwsurlh = row["BannerLinkUrlOWSURLH"].ToString();

                AddItemToList(bannerHeadingOwstext, bannerImageAltTextOwstext, bannerImageOwsurlh, bannerItemOrderOwsnmbr, bannerLinkHoverTextOwstext, bannerLinkTargetOwschcs, bannerLinkTextOwstext, bannerSubHeadingOwstext, bannerLinkUrlOwsurlh);
            }
        }

        private void GetWebPartProperties()
        {
            var theme = TextColorTheme;
            switch (theme)
            {
                case ColorTheme.Black:
                    _textColorTheme = "ia-text-black";
                    break;
                case ColorTheme.White:
                    _textColorTheme = "ia-text-white";
                    break;
                case ColorTheme.Gray:
                    _textColorTheme = "ia-text-gray";
                    break;
            }
            var location = TextLocation;
            switch (location)
            {
                case Location.top_left:
                case Location.top_center:
                case Location.top_right:
                case Location.middle_left:
                case Location.middle_center:
                case Location.middle_right:
                case Location.bottom_left:
                case Location.bottom_center:
                case Location.bottom_right:
                    _textLocation = "ia-" + location;
                    break;
            }

            var alignment = TextAlignment;
            switch (alignment)
            {
                case Alignment.left:
                case Alignment.right:
                case Alignment.center:
                    _textAlignment = "ia-text-" + alignment;
                    break;
            }
        }

        #endregion

        #region private methods

        /// <summary>
        /// Map instruction set fields to web part properties. If instruction set field is null, or is of the wrong type, the web part property is left unchanged.
        /// </summary>
        /// <param name="idsData">InstructionClientResponse containing instruction set.</param>
        /// <param name="webPart">Banner web part</param>
        /// <param name="clientService"></param>
        internal void MapInstructionSetToProperties(InstructionResponse idsData, Banner webPart, IInstructionRepository clientService)
        {
            webPart.ResultSourceId = idsData.Dictionary["AkuminaInterActionDefault"].ContainsKey("ResultSourceId") ? (clientService.GetValue(idsData.Dictionary, "ResultSourceId", webPart.ResultSourceId)) : webPart.ResultSourceId;
            webPart.RootResourcePath = idsData.Dictionary["AkuminaInterActionDefault"].ContainsKey("RootResourcePath") ? clientService.GetValue(idsData.Dictionary, "RootResourcePath", webPart.RootResourcePath) : webPart.RootResourcePath;
            webPart.InfiniteLoop = idsData.Dictionary["AkuminaInterActionDefault"].ContainsKey("InfiniteLoop") ? (clientService.GetValue(idsData.Dictionary, "InfiniteLoop", "1") == "1") : webPart.InfiniteLoop;
            webPart.AutoPlay = idsData.Dictionary["AkuminaInterActionDefault"].ContainsKey("Autoplay") ? (clientService.GetValue(idsData.Dictionary, "Autoplay", "1") == "1") : webPart.AutoPlay;
            webPart.ShowNavigator = idsData.Dictionary["AkuminaInterActionDefault"].ContainsKey("SlideNavigation") ? (clientService.GetValue(idsData.Dictionary, "SlideNavigation", "1") == "1") : webPart.ShowNavigator;
            webPart.MaxSlideCount = idsData.Dictionary["AkuminaInterActionDefault"].ContainsKey("MaximumSlideCount") ? Convert.ToInt32(clientService.GetValue(idsData.Dictionary, "MaximumSlideCount", 1.0D)) : webPart.MaxSlideCount;
            webPart.TransitionEffect = idsData.Dictionary["AkuminaInterActionDefault"].ContainsKey("Animation.SlideTransition") ? GetValueEnum(idsData.Dictionary, "Animation.SlideTransition", TransitionEffect.Fade) : webPart.TransitionEffect;
            webPart.SlideDuration = idsData.Dictionary["AkuminaInterActionDefault"].ContainsKey("Animation.SlideDuration") ? Convert.ToInt32(clientService.GetValue(idsData.Dictionary, "Animation.SlideDuration", 3000.0D)) : webPart.SlideDuration;
            webPart.QueryType = idsData.Dictionary["AkuminaInterActionDefault"].ContainsKey("QueryType") ? GetValueEnum(idsData.Dictionary, "QueryType", webPart.QueryType) : webPart.QueryType;

            var color = GetValueEnum(idsData.Dictionary, "TextSettings.ColorTheme", webPart.TextColorTheme);
            switch (color)
            {
                case ColorTheme.Black:
                    _textColorTheme = "ia-text-black";
                    break;
                case ColorTheme.White:
                    _textColorTheme = "ia-text-white";
                    break;
                case ColorTheme.Gray:
                    _textColorTheme = "ia-text-gray";
                    break;
            }
            var location = GetValueEnum(idsData.Dictionary, "TextSettings.Location", webPart.TextLocation);
            switch (location)
            {
                case Location.top_left:
                case Location.top_center:
                case Location.top_right:
                case Location.middle_left:
                case Location.middle_center:
                case Location.middle_right:
                case Location.bottom_left:
                case Location.bottom_center:
                case Location.bottom_right:
                    _textLocation = "ia-" + location;
                    break;
            }
            var alignment = GetValueEnum(idsData.Dictionary, "TextSettings.Alignment", webPart.TextAlignment);
            switch (alignment)
            {
                case Alignment.left:
                case Alignment.right:
                case Alignment.center:
                    _textAlignment = "ia-text-" + alignment;
                    break;
            }
            webPart.H1MaxCharacters = Convert.ToInt32(clientService.GetValue(idsData.Dictionary, "TextSettings.MaxCharacters", 0D));
            webPart.H2MaxCharacters = Convert.ToInt32(clientService.GetValue(idsData.Dictionary, "TextSettings.SubtitleMaxCharacters", 0D));
            webPart.ButtonMaxCharacters = Convert.ToInt32(clientService.GetValue(idsData.Dictionary, "TextSettings.ButtonMaxCharacters", 0D));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private DataTable GetBannerItems()
        {
            try
            {
                var proxy = (SearchServiceApplicationProxy)SearchServiceApplicationProxy.GetProxy(SPServiceContext.GetContext(SPContext.Current.Site));
                var query = new KeywordQuery(proxy) { SourceId = new Guid(ResultSourceId) };
                query.SelectProperties.Add("BannerCssClassOWSTEXT");
                query.SelectProperties.Add("BannerHeadingOWSTEXT");
                query.SelectProperties.Add("BannerImageAltTextOWSTEXT");
                query.SelectProperties.Add("BannerImageOWSURLH");
                query.SelectProperties.Add("BannerItemOrderOWSNMBR");
                query.SelectProperties.Add("BannerLinkHoverTextOWSTEXT");
                query.SelectProperties.Add("BannerLinkTargetOWSCHCS");
                query.SelectProperties.Add("BannerLinkTextOWSTEXT");
                query.SelectProperties.Add("BannerSubHeadingOWSTEXT");
                query.SelectProperties.Add("BannerLinkUrlOWSURLH");
                query.RowLimit = MaxSlideCount > 0 ? MaxSlideCount : 500;
                query.ResultsProvider = SearchProvider.Default;
                query.EnableStemming = true;
                query.TrimDuplicates = false;
                query.SafeQueryPropertiesTemplateUrl = "spfile://webroot/queryparametertemplate.xml";
                query.AuthenticationType = QueryAuthenticationType.PluggableAuthenticatedQuery;
                query.KeywordInclusion = KeywordInclusion.AllKeywords;
                var executor = new SearchExecutor();
                var resultTableCollection = executor.ExecuteQuery(query);
                var resultTables = resultTableCollection.Filter("TableType", KnownTableTypes.RelevantResults);
                var resultTable = resultTables.FirstOrDefault();
                if (resultTable != null) return resultTable.Table;
            }
            catch (Exception ex)
            {
                Microsoft.Office.Server.Diagnostics.PortalLog.LogString("Exception Occurred in this method : {0} || {1}", ex.Message, ex.StackTrace);
            }
            return null;
        }

        /// <summary>
        ///     Writes out the initialization script which contains the necessary css, js, and "global" (banner-wide) settings.
        /// </summary>
        /// <returns>String containing script element that defines the necessary settings and includes the necessary files.</returns>
        private string WriteInitialScript()
        {
            var resourcePathValue = RootResourcePath;

            var sb = new StringBuilder();

            var styleSheets = Resources.pdl_StyleSheets.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var sheet in styleSheets)
            {
                sb.AppendLine("<link rel=\"stylesheet\" href=\"" + resourcePathValue + sheet + "\" />");
            }

            var jsFiles = Resources.pdl_JSFiles.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var jsFile in jsFiles)
            {
                if (jsFile.ToLower().Contains("jquery"))
                {
                    sb.AppendLine("<script type=\"text/javascript\">");
                    sb.AppendLine("if (typeof jQuery == 'undefined') { document.write(unescape(\"%3Cscript src='" + resourcePathValue + jsFile + "' type='text/javascript'%3E%3C/script%3E\")); }");
                    sb.AppendLine("</script>");
                }
                sb.AppendLine("<script type=\"text/javascript\" src=\"" + resourcePathValue + jsFile + "\"></script>");
            }

            var d = new Templates { ControlTemplate = Resources.ControlTemplate, ItemTemplate = Resources.ItemTemplate, TileItemTemplate = Resources.TileItemTemplate };

            var memStream = new MemoryStream();
            var ser = new DataContractJsonSerializer(d.GetType());
            ser.WriteObject(memStream, d);
            memStream.Position = 0;
            var sr = new StreamReader(memStream);
            var templateJson = sr.ReadToEnd();

            sb.AppendLine("<script type=\"text/javascript\">");
            sb.AppendLine("var scriptbase = \"" + Resources.val_ScriptBase + "\";");
            sb.AppendLine("var bannerResourcePathValue = \"" + resourcePathValue + "\";");

            sb.AppendLine("</script>");

            litTemplates.Text = templateJson;

            return sb.ToString();
        }

        /// <summary>
        ///     Writes out the necessary property values from the web parts properties collection.
        /// </summary>
        /// <returns>String containing script element that defines the necessary values specific to this instance of the web part.</returns>
        private string WriteWebPartProperties()
        {
            var sb = new StringBuilder();
            sb.AppendLine("thisBanner.InstructionSetId = \"" + InstructionSet + "\";");

            sb.AppendLine("thisBanner.webPartTheme = \"" + WebPartTheme + "\";");
            sb.AppendLine("thisBanner.bannerResultSource = \"" + ResultSourceId + "\";");
            sb.AppendLine("thisBanner.linkButtonTheme = \"" + LinkButtonTheme + "\";");
            sb.AppendLine("thisBanner.linkButtonTextTheme = \"" + LinkButtonTextTheme + "\";");
            sb.AppendLine("thisBanner.slideDuration = " + SlideDuration + ";");
            sb.AppendLine("thisBanner.transitionEffect = \"" + TransitionEffect + "\";");
            sb.AppendLine("thisBanner.showNavigator = " + (ShowNavigator ? "true" : "false") + ";");
            sb.AppendLine("thisBanner.autoPlay = " + (AutoPlay ? "true" : "false") + ";");
            sb.AppendLine("thisBanner.infiniteLoop = " + (InfiniteLoop ? "true" : "false") + ";");

            sb.AppendLine("thisBanner.MaxSlideCount = " + MaxSlideCount + ";");
            sb.AppendLine("thisBanner.TextColorTheme = \"" + _textColorTheme + "\";");
            sb.AppendLine("thisBanner.TextLocation = \"" + _textLocation.Replace("_", "-") + "\";");
            sb.AppendLine("thisBanner.TextAlignment = \"" + _textAlignment + "\";");
            sb.AppendLine("thisBanner.H1MaxCharacters = " + (H1MaxCharacters > 0 ? H1MaxCharacters.ToString() : "-1") + ";");
            sb.AppendLine("thisBanner.H2MaxCharacters = " + (H2MaxCharacters > 0 ? H2MaxCharacters.ToString() : "-1") + ";");
            sb.AppendLine("thisBanner.ButtonMaxCharacters = " + (ButtonMaxCharacters > 0 ? ButtonMaxCharacters.ToString() : "-1") + ";");
            
            sb.AppendLine("thisBanner.DisplayTiles = " + (DisplayTiles ? "true" : "false") + ";");
            return sb.ToString();
        }

        /// <summary>
        ///     return the value of given column enum
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instructionDictionary"></param>
        /// <param name="columnName"></param>
        /// <param name="defaultValue"></param>
        /// <returns>T</returns>
        private static T GetValueEnum<T>(Dictionary<string, Dictionary<string, object>> instructionDictionary, string columnName,
            T defaultValue = default(T))
        {
            try
            {
                var arr = columnName.Split(new[] { '.' }, 2);
                var firstKey = (arr.Length == 1) ? InstructionConstants.DefaultCategory : arr[0];
                var secondKey = (arr.Length == 1) ? arr[0] : arr[1];
                if (!instructionDictionary.ContainsKey(firstKey) ||
                    !instructionDictionary[firstKey].ContainsKey(secondKey)) return defaultValue;
                var columnValue = ((IDictionary<string, object>)instructionDictionary[firstKey])[secondKey];

                return (columnValue != null) ? (T)Enum.Parse(typeof(T), columnValue.ToString().Replace("-", "_")) : defaultValue;
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }
        #endregion
    }
}
