
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
using Akumina.WebParts.DocumentSummaryList.Properties;
using Microsoft.SharePoint;
using Microsoft.Office.Server.Search.Query;
using Microsoft.Office.Server.Search.Administration;
using Akumina.InterAction;

namespace Akumina.WebParts.DocumentSummaryList.DocumentSummaryList
{
     [ToolboxItem(false)]
    public partial class DocumentSummaryList : DocumentSummaryListBaseWebPart
    {
        private string targetLibrary = "";
        private string tabList = "";
        private int NewestNumber;
        private int RecentNumber;
        private int PopularNumber;
        private int RecommendedNumber;
        private string NewestInfo = "";
        private string RecentInfo = "";
        private string PopularInfo = "";
        private string RecommendedInfo = "";
        private int PopularDays;
        private ArrayList _newItems = new ArrayList();
        private string  _logDateOcurred = "";
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public DocumentSummaryList()
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

            InitializeInstructions();
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(WriteInitialScript());
            litTop.Text = sb.ToString();

          //  if (this.Title == "") { this.ChromeType = PartChromeType.None; }
          //  else { this.ChromeType = PartChromeType.Default; }

        }

        //Get the Instruction Set
        //If the Instruction Set is not null, assign values to webpart
        //edit pane properties and appropriate variables
        private void InitializeInstructions()
        {
            if (!Page.IsPostBack)
            {
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
                getIDSValues();
            }
        }

        private void getIDSValues()
        {
            //Initialize variables to values from the edit pane
            targetLibrary = TargetDocumentLibrary;
            tabList = TabList;
            NewestNumber = NumberOfSitesNewest;
            RecentNumber = NumberOfSitesMyRecent;
            PopularNumber = NumberOfSitesPopular;
            RecommendedNumber = NumberOfSitesRecommended;
            NewestInfo = InfoTextNewestTab;
            RecentInfo = InfoTextRecentTab;
            PopularInfo = InfoTextPopularTab;
            RecommendedInfo = InfoTextRecommendedTab;
            PopularDays = NumberOfDaysPopular;

            //Get value in Instruction Set
            if (!string.IsNullOrWhiteSpace(InstructionSet))
            {
                var parts = InstructionSet.Split(new[] { '.' }, 2);
                var listItem = GetItem(parts[0], parts[1]);
                if (listItem == null)
                {
                    Logging.LogError(string.Format("No items found for {0}", InstructionSet));
                }

                //If an IDS field is null or blank, do not take it
                if (!DBNull.Value.Equals(listItem["TargetDocumentLibrary"]))
                {
                    targetLibrary = (string)listItem["TargetDocumentLibrary"];
                }
                if (!DBNull.Value.Equals(listItem["TabList"]))
                {
                    tabList = (string)listItem["TabList"];
                }
                if (!DBNull.Value.Equals(listItem["NumberOfDocsNewest"]))
                {
                    NewestNumber = (int)(double)listItem["NumberOfDocsNewest"];
                }
                if (!DBNull.Value.Equals(listItem["NumberOfDocsMyRecent"]))
                {
                    RecentNumber = (int)(double)listItem["NumberOfDocsMyRecent"];
                }
                if (!DBNull.Value.Equals(listItem["NumberOfDocsPopular"]))
                {
                    PopularNumber = (int)(double)listItem["NumberOfDocsPopular"];
                }
                if (!DBNull.Value.Equals(listItem["NumberOfDocsRecommended"]))
                {
                    RecommendedNumber = (int)(double)listItem["NumberOfDocsRecommended"];
                }
                if (!DBNull.Value.Equals(listItem["InfoTextNewestTab"]))
                {
                    NewestInfo = (string)listItem["InfoTextNewestTab"];
                }
                if (!DBNull.Value.Equals(listItem["InfoTextRecentTab"]))
                {
                    RecentInfo = (string)listItem["InfoTextRecentTab"];
                }
                if (!DBNull.Value.Equals(listItem["InfoTextPopularTab"]))
                {
                    PopularInfo = (string)listItem["InfoTextPopularTab"];
                }
                if (!DBNull.Value.Equals(listItem["InfoTextRecommendedTab"]))
                {
                    RecommendedInfo = (string)listItem["InfoTextRecommendedTab"];
                }
                if (!DBNull.Value.Equals(listItem["NumberOfDaysPopular"]))
                {
                    PopularDays = (int)(double)listItem["NumberOfDaysPopular"];
                }
            }            
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
                resourcePathValue = rootFolder + "/_layouts/15/Akumina.WebParts.DocumentSummaryList";                
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

            sb.AppendLine("<script type=\"text/javascript\">");
            sb.AppendLine("var scriptbase = \"" + "/_layouts/15/" + "\";");
            sb.AppendLine("var resourcePathValue =" + (string.IsNullOrEmpty(resourcePathValue) ? "\"\"" : "\"" + resourcePathValue + "\"") + ";");            
            sb.AppendLine("var numberOfDaysPopular =" + (PopularDays > 0 ? "\"" + PopularDays + "\"" : "\"\"") + ";");
            sb.AppendLine("</script>");

            var tabsName = tabList.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            //litTemplates.Text = templateJson;
            JavaScriptSerializer java = new JavaScriptSerializer();
            var controlTemplate = java.Deserialize<Templates>(templateJson).ControlTemplate;
            var itemTemplate = java.Deserialize<Templates>(templateJson).ItemTemplate;
            controlDslHtml.Text = controlTemplate;
            itemDslHtml.Text = itemTemplate;
            SPUser user = SPContext.Current.Web.CurrentUser;

            litResultNew.Text = new JavaScriptSerializer().Serialize(GetDocumentsFromSite("new", NumberOfSitesNewest, ConvertSearchKeywordQuery(targetLibrary)));
            litResultRecent.Text = new JavaScriptSerializer().Serialize(GetDocumentsFromSite("recent", NumberOfSitesMyRecent, ConvertSearchKeywordQuery(targetLibrary) + " AND ModifiedBy:" + user.Name));
            litResultPopular.Text = new JavaScriptSerializer().Serialize(GetDocumentsFromSite("popular", NumberOfSitesPopular, ConvertSearchKeywordQuery(targetLibrary)));
            litResultRecommended.Text = new JavaScriptSerializer().Serialize(GetDocumentsFromSite("recommended", NumberOfSitesRecommended, ConvertSearchKeywordQuery(targetLibrary) + " AND owstaxidmetadataalltagsinfo:Recommended" ));
            //owstaxIdCategory:Recommended
            litTabNames.Text = new JavaScriptSerializer().Serialize(tabsName);
            ///Descriptions
            NewTabDescription.Text = "\"" + NewestInfo + "\"";
            RecentTabDescription.Text = "\"" + RecentInfo + "\"";
            PopularTabDescription.Text = "\"" + PopularInfo + "\"";
            RecommendedTabDescription.Text = "\"" + RecommendedInfo + "\"";
            //Total Number Items
            NewTabCount.Text = "\"" + NewestNumber + "\"";
            RecentTabCount.Text = "\"" + RecentNumber + "\"";
            PopularTabCount.Text = "\"" + PopularNumber + "\"";
            RecommendedTabCount.Text = "\"" + RecommendedNumber + "\"";
            litCurrentSite.Text = CurrentSite.ToString().ToLower();

            litDSL_Title.Text = Title;
            litDSL_Icon.Text = GetIcon(Icon);
            litDSL_ShowHeader.Text = Convert.ToString(Title != "" || GetIcon(Icon) != "none").ToLower() ;

            return sb.ToString();
        }
        
        private string ConvertSearchKeywordQuery(string targetDocumentLibrary)
        {
            if (string.IsNullOrEmpty(targetDocumentLibrary)) { return ""; }
            string keywordFormat =" AND (";
            string[] lists = targetDocumentLibrary.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);;
            int index =0;
            if (this.CurrentSite)
            {
                foreach (string list in lists)
                //for (int i = 0; i < 2; i++ )
                {
                    SPWeb myWeb = SPContext.Current.Web;
                    var docList = myWeb.Lists.TryGetList(list);
                    var docListID = docList != null ? docList.ID.ToString() : "";
                    keywordFormat += index == 0 ? string.Format(" ListID:{0}", docListID) : string.Format(" OR ListID:{0}", docListID);
                    index++;
                }
            }
            else 
            { 
              SPSite oSite =  SPContext.Current.Site;
              SPWeb oWebRoot = SPContext.Current.Site.RootWeb;
              if (oWebRoot.DoesUserHavePermissions(SPBasePermissions.ViewListItems))
              {
                  foreach (string list in lists)
                  //for (int i = 0; i < 2; i++ )
                  {
                      var docList = oWebRoot.Lists.TryGetList(list);
                      var docListID = docList != null ? docList.ID.ToString() : "";
                      keywordFormat += index == 0 ? string.Format(" ListID:{0}", docListID) : string.Format(" OR ListID:{0}", docListID);
                      index++;
                  }
              }
                 
              
                  foreach (SPWeb oWeb in oSite.RootWeb.GetSubwebsForCurrentUser())
                  {
                      foreach (string list in lists)
                      //for (int i = 0; i < 2; i++ )
                      {
                          var docList = oWeb.Lists.TryGetList(list);
                          var docListID = docList != null ? docList.ID.ToString() : "";
                          keywordFormat += index == 0 ? string.Format(" ListID:{0}", docListID) : string.Format(" OR ListID:{0}", docListID);
                          index++;
                      }
                  }
             
            }
            keywordFormat += " )";
            return keywordFormat;
        }

        
        private DataTable GetDocumentsQuery(string tabName, int rowLimit, string listTarget)
        {
            try
            {
                SearchServiceApplicationProxy proxy = (SearchServiceApplicationProxy)SearchServiceApplicationProxy.GetProxy
               (SPServiceContext.GetContext(SPContext.Current.Site));
                var query = new KeywordQuery(proxy);
                string contentClass = GetContentClass();
                if (this.CurrentSite)
                {
                    query.QueryText = string.Format("webID:{0} AND " + contentClass + listTarget, SPContext.Current.Web.ID);
                }
                else
                {
                    query.QueryText = string.Format("SiteID:{0} AND " + contentClass + listTarget, SPContext.Current.Site.ID);
                }
                query.SortList.Add("LastModifiedTime", SortDirection.Descending);
                query.RowLimit = rowLimit;//max row limit is 500 for KeywordQuery
                query.ResultsProvider = SearchProvider.Default;
                query.EnableStemming = true;
                query.TrimDuplicates = false;
                query.AuthenticationType = QueryAuthenticationType.PluggableAuthenticatedQuery;
                query.KeywordInclusion = KeywordInclusion.AllKeywords;
                SearchExecutor executor = new SearchExecutor();
                ResultTableCollection resultTableCollection = executor.ExecuteQuery(query);
                var resultTables = resultTableCollection.Filter("TableType", KnownTableTypes.RelevantResults);
                var resultTable = resultTables.FirstOrDefault();
                return resultTable.Table;
            }
            catch (Exception ex)
            {
                Logging.LogError(ex);
                return null;
            }

        }

        private string GetContentClass()
        {
            StringBuilder text = new StringBuilder();
            string contentClass = text.ToString();
            text.Append("(contentclass:STS_ListItem_DocumentLibrary OR FileExtension:doc OR FileExtension:docx OR FileExtension:xls OR FileExtension:xlsx OR FileExtension:ppt OR FileExtension:pptx OR FileExtension:pdf OR FileExtension:png  OR FileExtension:gif OR FileExtension:jpeg OR FileExtension:jpg OR FileExtension:pub OR FileExtension:mpp OR FileExtension:vsx OR FileExtension:vsdx OR FileExtension:xls OR FileExtension:xsl OR FileExtension:xslx OR FileExtension:xlsx OR FileExtension:txt OR FileExtension:zip) (IsDocument:True OR contentclass:STS_ListItem)");
            return text.ToString();
        }          
    
        /// <summary>
        /// Get All Documents from a specific Web Site
        /// </summary>
        /// <param name="site"></param>
        /// <returns></returns>
        private ArrayList GetDocumentsFromSite(string tabName, int rowLimit, string listTarget)
        {
            ArrayList documentsResult = new ArrayList();
            _logDateOcurred = "";
            int totalLogsForItem = 0;
            int limit;

            //Detect what tab is being passed an adjust the number of documents
            //displayed accordingly
            if (tabName == "new")
            {
                limit = NewestNumber;
            }
            else if (tabName == "recent")
            {
                limit = RecentNumber;
            }
            else if (tabName == "popular")
            {
                limit = PopularNumber;
            }
            else if (tabName == "recommended")
            {
                limit = RecommendedNumber;
            }
            else
            {
                limit = 20;
                Logging.LogError(string.Format("Inproper tab passed {0}", tabName));
            }
            try
            {

                var searchResults = GetDocumentsQuery(tabName, rowLimit, listTarget);
                SPUser user = SPContext.Current.Web.CurrentUser;
                //foreach (DataRow row in searchResults.Rows)
                DataRow row;
                for (int i = 0; i < limit; i++ )
                {   //not folder
                    row = searchResults.Rows[i];
                    if (row["FileExtension"].ToString() != "")
                    {
                        string path = row["parentlink"].ToString().Replace("/Forms/AllItems.aspx", "").Replace("/AllItems.aspx", "").Replace("/Forms/Thumbnails.aspx", "");
                        string serverRedirectURL = row["OriginalPath"].ToString();//string.IsNullOrEmpty(row["ServerRedirectedURL"].ToString()) ? path  + "/"  + "." + row["SecondaryFileExtension"].ToString() :  path + "/" + fileName;
                        string docID = row["DocID"].ToString();
                        string title = row["Title"].ToString();
                        string author = "";
                        string authorAccount = "";
                        if (!string.IsNullOrEmpty(row["EditorOWSUSER"].ToString()))
                        {
                            var authorValues = row["EditorOWSUSER"].ToString().Split('|');
                            if (authorValues.Length > 0)
                            {
                                author = authorValues[1];
                                authorAccount = authorValues[authorValues.Length - 1];
                            }
                        }
                        string lastModifiedTime = row["LastModifiedTime"].ToString();
                        string fileType = row["SecondaryFileExtension"].ToString();
                        string siteName = path.Split('/')[path.Split('/').Count() - 2];
                        string siteUrl = path.Substring(0, path.LastIndexOf('/'));
                        string originalFile = row["OriginalPath"].ToString().Split('/')[row["OriginalPath"].ToString().Split('/').Count() - 1];
                        string fileName = originalFile.Contains("DispForm.aspx") ? title + "." + fileType : originalFile;

                        var listItem = GetItem(path, row["OriginalPath"].ToString(), fileName);
                        if (listItem != null)
                        {
                            siteName = listItem.Web.ToString();
                            siteUrl = listItem.Web.Url.ToString();
                            title = listItem.Name;
                            if (listItem.File != null)
                            {
                                lastModifiedTime = listItem.File.TimeLastModified.ToString();
                            }
                        }

                        if (tabName == "popular" && listItem != null)
                        {
                            totalLogsForItem = GetLogCountForListItem(listItem);
                        }
                        documentsResult.Add(new { SiteName = siteName, Url = siteUrl, IconSrc = GetIconUrl(fileType), FileUrl = path + "/" + fileName, FileName = title, DateModified = lastModifiedTime, ModifiedBy = author, ModifiedByAccount = authorAccount, IsCurrentUserLastEditor = author == user.Name, ISRecommended = false, TotalLogs = totalLogsForItem, LogDateOcurred = _logDateOcurred != "" ? _logDateOcurred : lastModifiedTime });
                    }
                }
            }

            catch (Exception ex)
            {
                Logging.LogError(ex);
            }
            return documentsResult;
        }

        


        /// <summary>
        ///     Bind the Icon image into a DataTable
        /// </summary>
        /// <param name="icon">string is used to get the relevant icon image</param>
        /// <returns>Icon Image string </returns>
        private string GetIconUrl(string icon)
        {
            var imagePAth = "../_layouts/15/Akumina.WebParts.DocumentSummaryList/images/icons/";                        
            switch (icon)
            {
                case "doc":
                case "docx":
                    imagePAth = imagePAth +"icdocx.png";
                    break;
                case "gif":
                    imagePAth = imagePAth +"icgif.gif";
                    break;
                case "jpeg":
                case "jpg":
                    imagePAth = imagePAth +"icjpg.gif";
                    break;
                case "notebk":
                case "one":
                case "onetoc2":
                    imagePAth = imagePAth +"icnotebk.png";
                    break;
                case "pdf":
                    imagePAth = imagePAth +"icpdf.png";
                    break;
                case "png":
                    imagePAth = imagePAth +"icpng.gif";
                    break;
                case "ppt":
                case "pptx":
                    imagePAth = imagePAth +"icpptx.png";
                    break;
                case "pub":
                case "mpp":
                    imagePAth = imagePAth +"icpub.png";
                    break;
                case "vsx":
                case "vsdx":
                    imagePAth = imagePAth +"icvsx.png";
                    break;
                case "xls":
                case "xsl":
                case "xslx":
                case "xlsx":
                    imagePAth = imagePAth +"icxlsx.png";
                    break;
                case "txt":
                    imagePAth = imagePAth +"ictxt.gif";
                    break;
                case "zip":
                    imagePAth = imagePAth +"iczip.gif";
                    break;
                case "":
                    imagePAth = "/_layouts/15/images/folder.gif?rev=23lder";
                    break;
                default:
                    imagePAth = imagePAth +"icjpg.gif";
                    break;
            }

            return imagePAth;
        }

        private SPListItem GetItem(string listName, string site, string itemName) { 
          //SPListItem oItem = new SPListItem();
          try
            {               
                    using (SPSite siteCollection = new SPSite(site))
                    {
                        SPWeb elevatedWeb = siteCollection.OpenWeb();
                        
                        SPList list = elevatedWeb.GetList(listName);

                        SPQuery query = new SPQuery();
                        query.Query = "<Where><Eq><FieldRef Name=\"FileLeafRef\" /><Value Type=\"Text\">" + itemName + "</Value></Eq></Where>";
                        query.ViewAttributes = "Scope=\"RecursiveAll\"";
                        query.RowLimit = 1;
                        
                        query.ViewFields = "";
                        SPListItemCollection items = list.GetItems(query);
                        if (items.Count > 0)
                        {
                            return items[0];

                        }
                        else { return null; }

                    }                            
             
                
          }
            catch (Exception ex)
            {
                Logging.LogError(ex);
                return null;
                
            }
        
            
        }
        /// <summary>
        /// Get the total logs for a listItem
        /// </summary>
        /// <returns></returns>
         private int GetLogCountForListItem(SPListItem oItem)
        {
            int auditCount = 0;
            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    using (SPSite siteCollection = new SPSite(SPContext.Current.Web.Url))
                    {
                       SPWeb elevatedWeb = siteCollection.OpenWeb();
                       SPAuditQuery wssQuery = new SPAuditQuery(siteCollection);
                       wssQuery.RestrictToListItem(oItem);
                       SPAuditEntryCollection auditCol = elevatedWeb.Audit.GetEntries(wssQuery);
                                            

                     var auditColFilter =  auditCol.Cast<SPAuditEntry>()
                             .Where(entry =>  
                                 entry.Occurred >= System.DateTime.Now.AddDays(-PopularDays)
                               ).OrderByDescending(I=>I.Occurred).ToArray();

                     if (auditColFilter != null && auditColFilter.Length > 0)
                     {
                         auditCount = auditColFilter.Length;
                           
                       }                                                                   
                    }                      
                });
            
               return auditCount;
            }
            catch (Exception ex)
            {
                Logging.LogError(ex);
                return 0;
            }
        }
    }
    }

