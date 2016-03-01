
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web.Script.Serialization;
using System.Xml;
using Akumina.WebParts.SiteSummaryList.Properties;
using Microsoft.SharePoint;

namespace Akumina.WebParts.SiteSummaryList.SiteSummaryList
{
    [ToolboxItem(false)]
    public partial class SiteSummaryList : SiteSummaryListBaseWebPart
    {
        ArrayList _newItems = new ArrayList();
        ArrayList _recommendedItems = new ArrayList();
        private const string _tabRecommendedListName = "AkuminaSiteRecommendation";
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public SiteSummaryList()
        {
        }

        private Dictionary<string, string> _oTabDictionary = new Dictionary<string, string>();

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
        }

        protected void Page_Load(object sender, EventArgs e)
        {


            StringBuilder sb = new StringBuilder();
            sb.AppendLine(WriteInitialScript());
            litTop.Text = sb.ToString();

        //    if (this.Title == "") { this.ChromeType = PartChromeType.None; }
        //    else { this.ChromeType = PartChromeType.Default; }


        }

        /// <summary>
        ///     Writes out the initialization script which contains the necessary css, js, and "global" (banner-wide) settings.
        /// </summary>
        /// <returns>String containing script element that defines the necessary settings and includes the necessary files.</returns>
        private string WriteInitialScript()
        {
            string resourcePathValue = RootResourcePath;
            if (string.IsNullOrEmpty(RootResourcePath))
            {
                string rootFolder = "";
                if (SPContext.Current.Web.RootFolder.ServerRelativeUrl.TrimEnd('/') != "")
                {
                    rootFolder = SPContext.Current.Web.RootFolder.ParentWeb.Url.Replace(SPContext.Current.Web.RootFolder.ServerRelativeUrl.TrimEnd('/'), "");
                }
                else 
                {
                    rootFolder = SPContext.Current.Web.Url.TrimEnd('/');
                
                }

                resourcePathValue = rootFolder + "/_layouts/15/Akumina.WebParts.SiteSummaryList";                
            }

            

            var sb = new StringBuilder();

            var styleSheets = Resource.pdl_StyleSheets.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var sheet in styleSheets)
            {
                sb.AppendLine("<link rel=\"stylesheet\" href=\"" + resourcePathValue + sheet + "\" />");
            }

            var jsFiles = Resource.pdl_JSFiles.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
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


            sb.AppendLine("<script type=\"text/javascript\">");
            sb.AppendLine("var scriptbase = \"" + Resource.val_ScriptBase + "\";");
            sb.AppendLine("</script>");

            var d = new Templates { ControlTemplate = Resource.ControlTemplate, ItemTemplate = Resource.ItemTemplate };

            var memStream = new MemoryStream();
            var ser = new DataContractJsonSerializer(d.GetType());
            ser.WriteObject(memStream, d);
            memStream.Position = 0;
            var sr = new StreamReader(memStream);
            var templateJson = sr.ReadToEnd();

            JavaScriptSerializer java = new JavaScriptSerializer();

            var controlTemplate = java.Deserialize<Templates>(templateJson).ControlTemplate;
            var itemTemplate = java.Deserialize<Templates>(templateJson).ItemTemplate;
            controlHtml.Text = controlTemplate;
            itemHtml.Text = itemTemplate;

            sb.AppendLine("<script type=\"text/javascript\">");
            sb.AppendLine("var scriptbase = \"" + "/_layouts/15/" + "\";");
            sb.AppendLine("var resourcePathValue =" + (string.IsNullOrEmpty(resourcePathValue) ? "\"\"" : "\"" + resourcePathValue + "\"") + ";");            
            sb.AppendLine("var numberOfDaysPopular =" + (NumberOfDaysPopular > 0 ? "\"" + NumberOfDaysPopular + "\"" : "\"\"") + ";");
            sb.AppendLine("</script>");

            var tabsName = TabList.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            //litTemplates.Text = templateJson;

            
            GetRecommendedListItems();
            using (SPSite siteCollection = new SPSite(SPContext.Current.Site.RootWeb.Url))
            {
                //get current web
                SPWeb oWeb = siteCollection.OpenWeb() ;
                SPUser user = SPContext.Current.Web.CurrentUser;
                var latestAccessLog = GetLatestWebAccess();
                string lastWebAccessforCurrentUser = GetLastAccessWebSite(oWeb.ID.ToString(), latestAccessLog, user.ID.ToString());                
                int popularWebSites = GetTotalAccessWebSite(oWeb.ID.ToString(), latestAccessLog);
                _newItems.Add(new { Site = ( oWeb.ServerRelativeUrl !="/" ? oWeb.ServerRelativeUrl.Split('/').Last() :oWeb.ToString()), Date = oWeb.Created.ToShortDateString(), Url = oWeb.Url, latestAccess = lastWebAccessforCurrentUser, TotalAccessed = popularWebSites });                
                GetUserSites(SPContext.Current.Site.Url, oWeb.Name, latestAccessLog);
            }

            ArrayList newTab = new ArrayList();
            newTab.Add(new { Description = string.IsNullOrEmpty(InfoTextNewestTab) ? "" : InfoTextNewestTab, Item = _newItems, tabIndex = 1 });
            litResultNew.Text = new JavaScriptSerializer().Serialize(newTab);
            ArrayList recommendedTab = new ArrayList();
            recommendedTab.Add(new { Item = _recommendedItems});
            litResultRecommended.Text = new JavaScriptSerializer().Serialize(recommendedTab);
            litTabNames.Text = new JavaScriptSerializer().Serialize(tabsName);
            ///Descriptions
            NewTabDescription.Text = "\"" + InfoTextNewestTab + "\"";
            RecentTabDescription.Text = "\"" + InfoTextRecentTab + "\"";
            PopularTabDescription.Text = "\"" + InfoTextPopularTab + "\"";
            RecommendedTabDescription.Text = "\"" + InfoTextRecommendedTab + "\"";
            //Total Number Items
            NewTabCount.Text = "\"" + NumberOfSitesNewest + "\"";
            RecentTabCount.Text = "\"" + NumberOfSitesMyRecent + "\"";
            PopularTabCount.Text = "\"" + NumberOfSitesPopular + "\"";
            RecommendedTabCount.Text = "\"" + NumberOfSitesRecommended + "\"";
            NewTabCount.Text = "\"" + NumberOfSitesNewest + "\"";
            
            return sb.ToString();
        }

      

        /// <summary>
        /// Get Total Access for each Site
        /// </summary>
        /// <param name="siteID"></param>
        /// <param name="latestAccessLog"></param>
        /// <returns></returns>
        private int GetTotalAccessWebSite(string siteID, DataTable latestAccessLog)
        {
            int total = 0;
            //Get total access for the site in the last 30 days.
            IEnumerable<DataRow> ordered = latestAccessLog.AsEnumerable().
                Where(i => i.Field<String>("siteID") == siteID.ToString()
                    && i.Field<DateTime>("LastAccess") >= DateTime.Now.AddDays(NumberOfDaysPopular > 0 ? -NumberOfDaysPopular : -30));//.OrderByDescending(i => i.Field<String>("LastAccess"));

            total = ordered.Count() > 0 ? ordered.Count() : 0;
            return total;
        }
        /// <summary>
        /// //Get Latest Log Date for Current User
        /// </summary>
        /// <param name="siteID"></param>
        /// <param name="latestAccessLog"></param>
        /// <returns></returns>
        private string GetLastAccessWebSite(string siteID, DataTable latestAccessLog, string userID)
        {
            string lastLog = "";

            IEnumerable<DataRow> ordered = latestAccessLog.AsEnumerable().
                Where(i => i.Field<String>("SiteID") == siteID.ToString() && i.Field<String>("UserID") == userID);//.OrderByDescending(i => i.Field<String>("LastAccess"));

            foreach (DataRow dr in ordered)
            {
                lastLog = dr["LastAccess"].ToString();
                break;
            }

            return lastLog;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="site"></param>
        /// <param name="subSite"></param>
        /// <param name="newItems"></param>
        /// <param name="latestAccessLog"></param>
        private void GetUserSites(string site, string subSite, DataTable latestAccessLog)
        {

            //SPSecurity.RunWithElevatedPrivileges(delegate()
            //{
                using (SPSite oSite = new SPSite(site))
                {

                    if (string.IsNullOrEmpty(subSite))
                    {

                        foreach (SPWeb oWeb in oSite.RootWeb.GetSubwebsForCurrentUser())
                        {
                            if (!oWeb.IsAppWeb)
                            {
                             AddTabItems(oWeb.Url,  "", latestAccessLog, oWeb);                                                                    

                            }
                        }
                    }
                    //Subsites
                    else
                    {
                        SPWeb myWeb = oSite.OpenWeb(subSite);
                        if (myWeb.Exists)
                        foreach (SPWeb oWeb in myWeb.GetSubwebsForCurrentUser())
                        {
                            if (!oWeb.IsAppWeb)
                            {
                                AddTabItems(site, subSite, latestAccessLog, oWeb);
                            }
                        }

                    }
                }

            //});
        }

        private ArrayList GetRecommendedListItems()
        {           
            try
            {
                if (!string.IsNullOrEmpty(_tabRecommendedListName))
                {
                    SPSite SpSite = SPContext.Current.Site;
                    SPUser user = SPContext.Current.Web.CurrentUser;
                    SPWeb myWeb = SpSite.RootWeb;
                    var list = myWeb.Lists[_tabRecommendedListName];

                    SPSecurity.RunWithElevatedPrivileges(delegate() {
                        foreach (SPListItem oListItem in list.Items)
                        {
                        
                                var url = oListItem["Url"];
                                url = url.ToString().Split(',')[0];
                                var host = new System.Uri(url.ToString()).Host;

                                if (SPContext.Current.Web.Url.ToLower().Contains(host.ToLower()))
                                {
                                    using (SPSite oSite = new SPSite(url.ToString()))
                                    {
                                        SPWeb oWeb = oSite.OpenWeb();
                                        if (oWeb.DoesUserHavePermissions(user.LoginName, SPBasePermissions.Open))
                                        {
                                            if ((bool)oListItem["Recommended"] == true)
                                            {
                                                _recommendedItems.Add(new { Site = oListItem.Title, Url = url });
                                            }
                                        }
                                    }
                                }
                                else {
                                    if ((bool)oListItem["Recommended"] == true)
                                    {
                                        _recommendedItems.Add(new { Site = oListItem.Title, Url = url });
                                    }
                                }

                            
                        }
                    });
                }
            }
            catch (Exception)
            {
                // ignored
            }

            return _newItems;
        }
        /// <summary>
        /// Get All Documents from a specific Web Site
        /// </summary>
        /// <param name="site"></param>
        /// <returns></returns>

        private void AddTabItems(string site, string subSite, DataTable latestAccessLog, SPWeb oWebRef)
        {
            //Get Latest Log Date for Current User
            SPUser user = SPContext.Current.Web.CurrentUser;
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPSite oSite = new SPSite(site))
                {
                    SPWeb oWeb = oSite.OpenWeb(oWebRef.ID);
                    string lastWebAccessforCurrentUser = GetLastAccessWebSite(oWeb.ID.ToString(), latestAccessLog, user.ID.ToString());
                    //lastWebAccessforCurrentUser = lastWebAccessforCurrentUser != "" ? lastWebAccessforCurrentUser : oWeb.Created.ToShortDateString();
                    int popularWebSites = GetTotalAccessWebSite(oWeb.ID.ToString(), latestAccessLog);
                    
                    if (oWebRef.DoesUserHavePermissions(user.LoginName,SPBasePermissions.Open))
                    {
                        _newItems.Add(new { Site = oWeb.Name, Date = oWeb.Created.ToShortDateString(), Url = oWeb.Url, latestAccess = lastWebAccessforCurrentUser, TotalAccessed = popularWebSites });
                    }
                    if (oWeb.Webs.Count > 0 || oWeb.Url != site)
                    {

                        GetUserSites(oWeb.Url, subSite != "" ? subSite + "/" + oWeb.Name : oWeb.Name, latestAccessLog);
                    }
                }
            });
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private DataTable GetLatestWebAccess()
        {

            DataTable dt = new DataTable();
            dt.Columns.Add("SiteID", typeof(string));
            dt.Columns.Add("LastAccess", typeof(DateTime));
            dt.Columns.Add("UserID", typeof(string));

            #region AuditorTest

            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    using (SPSite siteCollection = new SPSite(SPContext.Current.Web.Url))
                    {
                        

                        //Write Log
                        //var logXml = string.Format("<LastAccess>{0}</LastAccess>", System.DateTime.Now.ToString());
                        //SPAudit audit = siteCollection.Audit;
                        //audit.WriteAuditEvent(SPAuditEventType.Custom, "recentLog", logXml);

                        //Read Log
                        SPAuditQuery auditQuery = new SPAuditQuery(siteCollection);
                        //auditQuery.RestrictToUser(currentUser.ID);            

                        SPAuditEntryCollection entries = siteCollection.Audit.GetEntries(auditQuery);
                        var entriesFiltered = entries.Cast<SPAuditEntry>().OrderByDescending(i => i.Occurred);



                        foreach (SPAuditEntry entry in entriesFiltered)
                        {
                            if (entry.SourceName == "Aku_SSL_Log")
                            {
                                string myXml = entry.EventData;
                                XmlDocument xml = new XmlDocument();
                                xml.LoadXml(myXml);
                                XmlNodeList xmlList = xml.SelectNodes("/root");
                                DataRow dr = dt.NewRow();

                                var siteID = "";
                                var lastAccess = "";

                                foreach (XmlNode node in xmlList)
                                {
                                    if (node["LastAccess"] != null)
                                    {
                                        lastAccess = node["LastAccess"].InnerText;
                                    }
                                    if (node["WebSiteID"] != null)
                                    {
                                        siteID = node["WebSiteID"].InnerText;
                                    }
                                    if (!string.IsNullOrEmpty(siteID) && !string.IsNullOrEmpty(lastAccess))
                                    {
                                        dr["SiteID"] = siteID;
                                        dr["LastAccess"] = Convert.ToDateTime(lastAccess);
                                        dr["UserID"] = entry.UserId;
                                        dt.Rows.Add(dr);
                                    }
                                }
                            }
                        }
                    }

                });
            #endregion
                return dt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
