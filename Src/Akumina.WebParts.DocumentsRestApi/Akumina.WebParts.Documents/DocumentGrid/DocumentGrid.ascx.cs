using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Akumina.InterAction;
using Akumina.WebParts.Documents.Properties;
using Microsoft.Office.Server.UserProfiles;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using System.Text;
using System.Web.UI.HtmlControls;
using Microsoft.SharePoint.WebControls;


namespace Akumina.WebParts.Documents.DocumentGrid
{
    [ToolboxItem(false)]
    public partial class DocumentGrid : DocumentGridBaseWebPart
    {
        #region Constants

        // Visual Studio might automatically update this path when you change the Visual Web Part project item.
        private const string AscxPath = @"~/_CONTROLTEMPLATES/DocumentGrid/DocumentGrid.ascx";

        #endregion

        #region Fields

        private const string QueryParam = "docLibName";
        private DataTable _dt;
        private StringBuilder _sbTaxonomy = new StringBuilder();
        private Dictionary<string, string> _listofTax = new Dictionary<string, string>();
        private List<string> _strLibraries = new List<string>();       
        private bool _sorting;
        private static readonly string MetaField = "Category";

        public string CategoryName = "category";
        public const string ExpireDateColumn = "End Date";
        public static string[] CloneColumns = { "Editor", "LinkFileName" };
        public ICurrentPath GetCurrentPath;
        #endregion

        #region Editor Part collection

        [ConnectionConsumer("the current Path Value", "GetPathConsumer")]
        public void GetPathConsumer(ICurrentPath getPath)
        {
            GetCurrentPath = getPath;
        }

        #endregion

        /// <summary>
        ///   Render the style sheets
        /// </summary>           
        protected override void Render(HtmlTextWriter writer)
        {

            //HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.Public);
            //HttpContext.Current.Response.Cache.SetMaxAge(TimeSpan.FromMinutes(30));
            //HttpContext.Current.Response.Cache.SetSlidingExpiration(true);
            // Set the cache response expiration to 3600 seconds (use your own value here).
            //HttpContext.Current.Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(30));

            // Set both server and browser caching.

            //HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.ServerAndPrivate);

            // Prevent browser's default max-age=0 header on first request
            // from invalidating the server cache on each request.
            //HttpContext.Current.Response.Cache.SetValidUntilExpires(true);
            try
            {
                string libName = "/js/vendor/jquery-2.1.3.min.js";//"/js/controls/ia-caml-query.js","/js/controls/ia-kql.js",
                string[] jsLinks =
            {
                "/js/vendor/modernizr.js","/js/vendor/picker.js", "/js/vendor/picker.date.js", "/js/vendor/tablesaw.stackonly.js","/js/vendor/jquery.sticky.js","/js/vendor/mustache.min.js","/js/vendor/jquery-done-typing.js",
                "/js/vendor/jquery.magnific-popup.js", "/js/components/modal.js", "/js/vendor/quicksearch.js", "/js/controls/ia-document-list.js","/js/vendor/jstree.js","/js/components/datepicker.js",
                "/js/vendor/jquery.dropdown.js", "/js/idle.js", 
                "/js/controls/ia-drag-drop.js",
                "/js/components/accordion.js",  "/js/controls/ia-document-grid.js", "/js/controls/ia-query.js",
                "/js/controls/ia-context-menu.js",  "/js/controls/ia-document-filters.js","/js/components/ia-docLib-modal.js","/js/controls/ia-library-search.js", 
                "/js/components/tabs.js","/js/controls/ia-common.js","/js/vendor/chosen.jquery.min.js","/js/components/ia-search-picker.js","/js/vendor/jquery.treeview.js","/js/components/ia-treeview.js"
            };
                string spJs = "/_layouts/15/sp.js";
                string[] cssLinks =
            {
                "/css/ia-interaction-controls.css", "/css/ia-document-grid.css","/css/jquery.treeview.css"
            };
                if (!string.IsNullOrEmpty(RootResourcePath) && !RootResourcePath.ToLower().Contains("http:"))
                {
                    var cWeb = SPContext.Current.Web;
                    SPList oList = null;
                    oList = cWeb.Lists.TryGetList(RootResourcePath);
                    if (oList != null && oList.BaseType == SPBaseType.DocumentLibrary)
                    {
                        var docLib = (SPDocumentLibrary)oList;
                        var oQuery = new SPQuery
                        {
                            Query = "<OrderBy><FieldRef Name='Order0' Ascending='True'  /></OrderBy>",
                            ViewAttributes = "Scope=\"Recursive\""
                        };
                        var collListItemsAvailable =
                            docLib.GetItems(oQuery);
                        var cssFiles = new List<string>();
                        var jsFiles = new List<string>();
                        foreach (SPListItem oListItemAvailable in collListItemsAvailable)
                        {
                            if (oListItemAvailable["File_x0020_Type"].ToString() == "css")
                                cssFiles.Add(oListItemAvailable.File.ServerRelativeUrl);

                            if (oListItemAvailable["File_x0020_Type"].ToString() == "js")
                                jsFiles.Add(oListItemAvailable.File.ServerRelativeUrl);
                        }
                        //bind script operation
                        for (int i = 0; i < jsFiles.Count; i++)
                        {
                            if (i != 0)
                                writer.Write(BindScript(jsFiles[i].ToLower(), true));
                            else
                            {
                                writer.Write("<script type=\"text/javascript\">");
                                writer.Write("if (typeof jQuery == 'undefined') { document.write(unescape(\"%3Cscript src='" + SPUrlUtility.CombineUrl(SPContext.Current.Site.RootWeb.Url, jsFiles[i]) + "' type='text/javascript'%3E%3C/script%3E\")); }");
                                writer.Write("</script>");

                            }
                        }

                        //bind style operation
                        foreach (var cssfile in cssFiles)
                            writer.Write(BindStyle(cssfile.ToLower(), true));
                    }
                }
                else if (!string.IsNullOrEmpty(RootResourcePath) && RootResourcePath.ToLower().Contains("http:"))
                {
                    writer.Write("<script type=\"text/javascript\">");
                    writer.Write("if (typeof jQuery == 'undefined') { document.write(unescape(\"%3Cscript src='" + RootResourcePath + libName + "' type='text/javascript'%3E%3C/script%3E\")); }");
                    writer.Write("</script>");
                    foreach (var jsfile in jsLinks)
                        writer.Write(@"<script type=""text/javascript"" src=""{0}{1}""></script>", RootResourcePath, jsfile.ToLower());
                    foreach (var csfile in cssLinks)
                        writer.Write(@"<link rel=""stylesheet"" href=""{0}{1}"" type=""text/css"" />", RootResourcePath, csfile.ToLower());
                }

                base.Render(writer);
            }
            catch (Exception ex)
            {
                Logging.LogError(ex);
            }
        }

        /// <summary>
        ///    Forms script tag in .aspx page
        /// </summary>     
        /// <param name="scriptUrl">Provides the URL to form the tag </param> 
        /// <param name="pickFromSiteCollection">Provides a root web URL</param> 
        private string BindScript(string scriptUrl, bool pickFromSiteCollection)
        {
            scriptUrl = SPUrlUtility.CombineUrl(pickFromSiteCollection ? SPContext.Current.Site.RootWeb.Url : SPContext.Current.Web.Url, scriptUrl);

            return string.Format(@"<script type=""text/javascript"" src=""{0}""></script>", scriptUrl);
        }

        /// <summary>
        ///    Forms CSS tag in .aspx page
        /// </summary>     
        /// <param name="styleUrl">Provides the URL to form the tag </param> 
        /// <param name="pickFromSiteCollection">Provides a root web URL</param> 
        private string BindStyle(string styleUrl, bool pickFromSiteCollection)
        {
            styleUrl = SPUrlUtility.CombineUrl(pickFromSiteCollection ? SPContext.Current.Site.RootWeb.Url : SPContext.Current.Web.Url, styleUrl);

            return string.Format(@"<link rel=""stylesheet"" href=""{0}"" type=""text/css"" />", styleUrl);
        }

        protected override void OnPreRender(EventArgs e)
        {
            try
            {
                if (ListName != null && (refreshIdleGrid.Value != "true"))
                {
                    if (!_sorting)
                    {
                        if (HttpContext.Current.Request.Cookies["akuminaNavigationCookie"] != null)
                        {
                            currentFolderPath.Value = HttpContext.Current.Request.Cookies["akuminaNavigationCookie"].Value.Split('!')[2];
                            //BindData();
                        }

                        else if (GetCurrentPath != null)
                        {

                            if (GetCurrentPath != null && !string.IsNullOrEmpty(GetCurrentPath.CurrentPath))// &&                                     currentFolderPath.Value != GetCurrentPath.CurrentPath)
                            {
                                currentFolderPath.Value = GetCurrentPath.CurrentPath;
                                //BindData();
                            }

                            //else
                            //    BindData();
                        }
                        //else
                        //    BindData();
                    }
                    else
                        _sorting = false;
                }
              
            }
            catch (Exception ex)
            {
                Logging.LogError(ex);
            }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
            DisplayTermStoreTree();
            GetTermStoreTree();
            BindListOfLibraries();
            getLibraries();
            try
            {
                if (Page.Request.Browser.Type.ToUpper().Contains("SAFARI"))
                {
                    var sm = ScriptManager.GetCurrent(this.Page);
                    if (sm != null)
                    {
                        sm.EnablePartialRendering = false;
                    }
                }
            }
            catch
            {
                // ignored
            }
        }

        private static string RenderLabel(string displayName, bool isRequired)
        {
            string content = "<nobr>" + displayName;
            if (isRequired)
                content += @"<span title=""This is a required field."" class=""ms-formvalidation""> *</span>";
            content += "</nobr>";
            return content;
        }
        private void getLibraries()
        {
            StringBuilder libDetails=new StringBuilder();
            StringBuilder webdocLib=new StringBuilder();
            //foreach (SPWeb web in SPContext.Current.Site.AllWebs)
            SPWeb web = SPContext.Current.Web;
            
                foreach (SPList list in web.Lists)
                {
                    if (list is SPDocumentLibrary)
                    {
                        libDetails.Append((list.Title + ":" + list.ID)+",");
                        webdocLib.Append(list.Title+",");
                    }
                }

            if (String.IsNullOrEmpty(_DocumentLibraries))
                DocumentLibraries = webdocLib.ToString();


            webdocLibrary.Value = libDetails.ToString();
            selecteddocLibrary.Value = _DocumentLibraries;

        }
        private void BindListOfLibraries()
        {
            try
            {
                _strLibraries = new List<string>();
                if (HttpContext.Current.Request.QueryString[QueryParam] != null && !string.IsNullOrEmpty(HttpContext.Current.Request.QueryString[QueryParam]))
                {
                    string strQueryLibraries = HttpContext.Current.Request.QueryString[QueryParam].Trim().ToLower();
                    if (!strQueryLibraries.Equals("all"))
                    {
                        var queryLibraries = strQueryLibraries.Split(',');
                        _strLibraries = (from library in queryLibraries where !string.IsNullOrWhiteSpace(library) select library.Trim('#')).ToList();
                    }
                }

                if (_strLibraries.Count == 0)
                {
                    if (!string.IsNullOrWhiteSpace(ListName))
                        _strLibraries.Add(ListName.ToLower());
                }
                if (_strLibraries.Count > 0)
                {
                    listName.Value = _strLibraries[0];
                }
                BindRequiredFields();
            }
            catch (Exception ex)
            {
                Logging.LogError(ex);
            }
        }

        private void GetTermStoreTree()
        {

            if (!string.IsNullOrEmpty(CategoryName))
            {
                _listofTax = new Dictionary<string, string>();
                SPSite thisSite = SPContext.Current.Site;
                Microsoft.SharePoint.Taxonomy.TaxonomySession session = new Microsoft.SharePoint.Taxonomy.TaxonomySession(thisSite);
                if (session.TermStores != null && session.TermStores.Count > 0)
                {
                    Microsoft.SharePoint.Taxonomy.TermStore termStore = session.TermStores[0];
                    Microsoft.SharePoint.Taxonomy.Group group = termStore.GetSiteCollectionGroup(thisSite);
                    foreach (Microsoft.SharePoint.Taxonomy.TermSet termSet in group.TermSets)
                    {
                        _listofTax.Add(termSet.Id.ToString().ToLower(), termSet.Name.ToLower());
                        foreach (Microsoft.SharePoint.Taxonomy.Term term in termSet.Terms)
                        {
                            StringAddTermSet(term, termSet.Name);
                        }
                    }
                }
            }
        }

        private void StringAddTermSet(Microsoft.SharePoint.Taxonomy.Term term, string termString)
        {
            string strTermString = termString + ">" + term.Name;
            _listofTax.Add(term.Id.ToString().ToLower(), strTermString.ToLower());
            if (term.Terms.Count > 0)
            {
                foreach (Microsoft.SharePoint.Taxonomy.Term t in term.Terms)
                {
                    StringAddTermSet(t, strTermString);
                }
            }
        }

        private void BindRequiredFields()
        {
            try
            {
                SPWeb spWeb = SPContext.Current.Web;
                {

                    SPDocumentLibrary spDoc = null;
                    if (_strLibraries.Count == 1)
                    {
                        int i = 0;
                        SPList spList = spWeb.Lists[_strLibraries[0]];
                        if (spList != null)
                        {
                            spDoc = spList as SPDocumentLibrary;
                            if (spDoc != null)
                            {

                                listName.Value = spDoc.RootFolder.Name;
                                listDisplayName.Value = spDoc.Title;
                                GridJsMode.Value = JSRenderMode.ToString().ToLower();
                                listGuidId.Value = spDoc.ID.ToString();
                                hdnMinorCheckInEnable.Value = (spDoc.EnableVersioning && spDoc.EnableMinorVersions) ? "minor" : spDoc.EnableVersioning ? "major" : "no";
                                webUrl.Value = spWeb.Url;

                                if (hdnMinorCheckInEnable.Value.Equals("major"))
                                    dd_fileCheckInMajorOption.Visible = true;
                                createpermission.Value = spDoc.DoesUserHavePermissions(SPBasePermissions.EditListItems) ? "yes" : "no";
                                //var folderPath = "";
                                webServerUrl.Value = spWeb.ServerRelativeUrl.Equals("/") ? "" : spWeb.ServerRelativeUrl;
                                foreach (SPField field in spDoc.Fields)
                                {
                                    if (field.Required && field.CanBeDisplayedInEditForm)
                                    {
                                        var htmldiv = new HtmlGenericControl("div");
                                        htmldiv.Attributes["class"] = "ia-form-row";
                                        var lbl = new Label
                                        {
                                            CssClass = "ia-form-label",
                                            Text = RenderLabel(field.Title, true)
                                        };

                                        htmldiv.Controls.Add(lbl);
                                        var oFormField = new FormField
                                        {
                                            ControlMode = SPControlMode.New,
                                            ListId = spList.ID,
                                            FieldName = field.InternalName,
                                            ID = "DynamicCtrlField_" + field.InternalName
                                        };
                                        if (field.Type == SPFieldType.DateTime)
                                        {
                                            string defaultDate = oFormField.ItemFieldValue != null ? DateTime.Parse(oFormField.ItemFieldValue.ToString()).ToString("MMM dd, yyyy") : string.Empty;
                                            var oDateField = new TextBox
                                            {
                                                CssClass = "ia-datepicker ax-textbox",
                                                ID = "DynamicCtrlField_" + field.InternalName,
                                                Text = defaultDate
                                            };
                                            oDateField.Attributes["default-value"] = defaultDate;
                                            htmldiv.Controls.Add(oDateField);
                                        }
                                        else if (field.Type == SPFieldType.Text)
                                        {
                                            string defaultDate = oFormField.ItemFieldValue != null ? oFormField.ItemFieldValue.ToString() : string.Empty;
                                            var oTextField = new TextBox
                                            {
                                                CssClass = "ax-textbox",
                                                ID = "DynamicCtrlField_" + field.InternalName,
                                                Text = defaultDate
                                            };
                                            oTextField.Attributes["default-value"] = defaultDate;
                                            htmldiv.Controls.Add(oTextField);
                                        }
                                        else if (field.TypeDisplayName.ToLower().Equals("managed metadata"))
                                        {
                                            string oAssociatedTaxonomy = string.Empty, oAssociatedTaxonomyNames = string.Empty, oDefaultTaxonomyPathValues = string.Empty, oTaxonomoyDefualtValue = string.Empty;
                                            var oTaxonomyField = field as Microsoft.SharePoint.Taxonomy.TaxonomyField;
                                            if (oTaxonomyField != null && oTaxonomyField.IsAnchorValid)
                                            {
                                                oAssociatedTaxonomy = !oTaxonomyField.AnchorId.ToString().Equals("00000000-0000-0000-0000-000000000000") ? GetMetaField(oTaxonomyField.Title.ToLower() + "|" + oTaxonomyField.AnchorId) : GetMetaField(oTaxonomyField.Title.ToLower() + "|" + oTaxonomyField.TermSetId);
                                            }
                                            if (!string.IsNullOrEmpty(oAssociatedTaxonomy))
                                            {

                                                if (oFormField.ItemFieldValue != null && !string.IsNullOrEmpty(oFormField.ItemFieldValue.ToString()))
                                                {
                                                    string defaultTaxVal = oFormField.ItemFieldValue.ToString();
                                                    oDefaultTaxonomyPathValues = GetMetaField(defaultTaxVal);
                                                    oAssociatedTaxonomyNames = GetMetaFieldNames(defaultTaxVal);
                                                    oTaxonomoyDefualtValue = oAssociatedTaxonomyNames + "|" + oDefaultTaxonomyPathValues;

                                                }

                                                var oTaxonomyFieldInput = new TextBox
                                                {
                                                    CssClass = "ia-upload-category-input",
                                                    ID = "DynamicCtrlField_" + field.InternalName,
                                                    Text = oAssociatedTaxonomyNames
                                                };
                                                oTaxonomyFieldInput.Attributes["associated-taxonomy"] = oAssociatedTaxonomy;
                                                oTaxonomyFieldInput.Attributes["default-value"] = oTaxonomoyDefualtValue;
                                                oTaxonomyFieldInput.Attributes["selected-taxonomies"] = oDefaultTaxonomyPathValues;
                                                oTaxonomyFieldInput.Attributes["readonly"] = "";
                                                oTaxonomyFieldInput.Attributes["field-name"] = "DynamicCtrlField_" + field.InternalName;

                                                var oHdnSelectedTaxonomies = new HiddenField
                                                {
                                                    ID = "DynamicCtrlField_" + field.InternalName + "_Hidden",
                                                    ClientIDMode = ClientIDMode.Static,
                                                    Value = oDefaultTaxonomyPathValues
                                                };

                                                var oBrowse = new Literal
                                                {
                                                    Text =
                                                        @"<a class='ia-button ia-modal-inline-trigger' href='#upload-category-popup' onclick=""getCategoryModel(this)"">Browse</a>"
                                                };

                                                htmldiv.Controls.Add(oTaxonomyFieldInput);
                                                htmldiv.Controls.Add(oBrowse);
                                                htmldiv.Controls.Add(oHdnSelectedTaxonomies);
                                            }
                                        }
                                        else
                                            htmldiv.Controls.Add(oFormField);

                                        pnlRequiredFields.Controls.Add(htmldiv);
                                        ++i;

                                    }
                                }
                            }
                        }
                        if (i > 0)
                            requiredFields.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {

                Logging.LogError(ex);
            }
        }

        private string GetMetaFieldNames(string metaValue)
        {
            var metaArr = new List<string>();
            try
            {
                var multiValues = metaValue.Split(';').ToList();
                foreach (var single in multiValues)
                {
                    var value = single.Split('|');

                    if (value.Length > 1 && _listofTax.ContainsKey(value[1]))
                    {
                        var metasingleValue = value[0];
                        metaArr.Add(metasingleValue);
                    }
                }
            }
            catch (Exception ex)
            {
                Logging.LogError(ex);
            }
            return string.Join(",", metaArr.ToArray());
        }

        private string GetMetaField(string metaValue)
        {
            var metaArr = new List<string>();
            try
            {
                var multiValues = metaValue.Split(';').ToList();
                foreach (var single in multiValues)
                {
                    var value = single.Split('|');

                    if (value.Length > 1 && _listofTax.ContainsKey(value[1]))
                    {
                        var metasingleValue = _listofTax[value[1]];
                        metaArr.Add(metasingleValue);
                    }
                }
            }
            catch (Exception ex)
            {
                Logging.LogError(ex);
            }
            return string.Join(",", metaArr.ToArray());
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Server.ScriptTimeout = 1200; // specify the timeout to 20 mins
            try
            {
                if (!string.IsNullOrEmpty(InstructionSet))
                {
                    var clientServices = new InstructionRepository();
                    var response = clientServices.Execute(InstructionSet);
                    if (response != null && response.Dictionary != null)
                    {
                        ListName = clientServices.GetValue(response.Dictionary, Resources.colName_ListName, ListName);
                        RootResourcePath = clientServices.GetValue(response.Dictionary, Resources.colName_RootResourcePath, RootResourcePath);
                    }
                }
                if (string.IsNullOrEmpty(RootResourcePath) && WebPartManager.DisplayMode != WebPartManager.EditDisplayMode)
                    RootResourcePath = SPContext.Current.Web.Url.TrimEnd('/') + "/_layouts/15/Akumina.WebParts.Documents";

                styleUrl.Value = RootResourcePath.TrimEnd('/') + "/";

                string[] defaultMenuOptions = { Resources.btnCheckIn, Resources.btnOpen, Resources.btnCheckOutAll, Resources.btnDiscardChkOutAll, Resources.btnDownload, Resources.btnDelete, Resources.btnMore };
                string[] defaultMoreOptions = { Resources.btnCheckIn, Resources.btnDiscardCheckOut, Resources.btnCheckOut, Resources.btnViewProperties, Resources.btnEditProperties, Resources.btnShare, Resources.btnShareWith, Resources.btnFollow, Resources.btnCompliance, Resources.btnWorkflow };




                if (!string.IsNullOrEmpty(MenuProperty))
                    hdnContextMenuOptions.Value = MenuProperty;
                else
                    hdnContextMenuOptions.Value = string.Join(";", defaultMenuOptions);
                if (!string.IsNullOrEmpty(MoreMenuProperty))
                    hdnContextMenuMoreOptions.Value = MoreMenuProperty;
                else
                    hdnContextMenuMoreOptions.Value = string.Join(";", defaultMoreOptions);
                if (!string.IsNullOrEmpty(DocumentListOptions))
                    hdnColumnsToHide.Value = DocumentListOptions.Replace(";", ",");

                if (refreshIdleGrid.Value != "true")
                {
                    if (!Page.IsPostBack)
                    {
                        var j = 0;

                        var arrProperties = Resources.Grid_Header_ColumnNames.Split('|');
                        columnNames.Value = string.Join(",", arrProperties);
                        //for (var i = 2; i <= (arrProperties.Length + 1); i++)
                        //{
                        //    //grid.Columns[i].HeaderText = arrProperties[j];
                        //    i++;
                        //    j++;
                        //}
                    }
                    else
                    {
                        UploadFile(sender, e);
                    }
                }
                ltlDms_Template.Text = Resources.DMSTemplate;
                hdnRowlimit.Value = RowLimit;
                akuminaCookieName.Value = "akuminaNavigationCookie";


            }
            catch (Exception ex)
            {
                Logging.LogError(ex);
            }
        }

        /// <summary>
        ///     Upload the Drag and Drop files by checking file collection count into the Document Library
        /// </summary>      
        protected void UploadFile(object sender, EventArgs e)
        {
            try
            {
                using (SPLongOperation operation = new SPLongOperation(this.Page))
                {
                    ltlUploadSuccess.Text = string.Empty;
                    if (!string.IsNullOrWhiteSpace(hdnUploadedFiles.Value))
                    {
                        var fileCollection = hdnUploadedFiles.Value.Split(','); //Page.Request.Files;
                        if (fileCollection.Length > 0)
                        {
                            for (var i = 0; i < fileCollection.Length; i++)
                            {
                                var upload = fileCollection[i];
                                if (!string.IsNullOrEmpty(upload))
                                    UploadFiles(upload);
                            }
                            //if (string.IsNullOrWhiteSpace(uploadStatus.InnerHtml))
                            //{
                            //ltlUploadSuccess.Text =  "Files Uploaded Successfully.";
                            //uploadSucess.Attributes["class"] = "ia-upload-success";
                            //}
                            //else
                            //uploadSucess.Attributes["class"] = "ia-upload-error";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logging.LogError(ex);
            }
        }

        /// <summary>
        ///     Upload the Drag and Drop files into the Document Library
        /// </summary>
        /// <param name="uploadFile">uploadFile is used to get the file details</param>          
        private void UploadFiles(string strFileName)//(HttpPostedFile uploadFile)
        {
            string strFolderUrl = string.Empty;
            try
            {

                var site = SPContext.Current.Site;
                {
                    var oSpWeb = SPContext.Current.Web;
                    {
                        oSpWeb.AllowUnsafeUpdates = true;
                        var uploadLibrary = _strLibraries.Count > 0 ? _strLibraries[0] : ListName;
                        var fileName = strFileName;
                        var docLibrary = oSpWeb.Lists[uploadLibrary];
                        //var fileName = uploadFile.FileName.Contains("\\")
                        //    ? Path.GetFileName(uploadFile.FileName)
                        //    : uploadFile.FileName;

                        string folderPath = currentFolderPath.Value, url = string.Empty;
                        if (!string.IsNullOrEmpty(folderPath))
                        {
                            url = folderPath.StartsWith("/") ? folderPath : "/" + folderPath;
                        }
                        else
                            url = docLibrary.RootFolder.ServerRelativeUrl;

                        strFolderUrl = oSpWeb.Url + url + "/";
                        //Check the Status and Perform the Check Out
                        SPFile file = oSpWeb.GetFile(strFolderUrl + fileName);

                        if (file.Exists)
                        {
                            if (docLibrary.ForceCheckout && file.CheckOutStatus == SPFile.SPCheckOutStatus.None)
                            {
                                file.CheckOut();
                            }

                            SPListItem listItem = file.Item;

                            foreach (SPField field in docLibrary.Fields)
                            {

                                if (field.Required && field.CanBeDisplayedInEditForm)
                                {
                                    if (field.Type == SPFieldType.DateTime)
                                    {
                                        DateTime dateValue;
                                        var formField = (TextBox)pnlRequiredFields.FindControl("DynamicCtrlField_" + field.InternalName);
                                        if (formField == null) continue;
                                        if (DateTime.TryParse(formField.Text, out dateValue))
                                            listItem[field.InternalName] = dateValue;
                                        listItem.UpdateOverwriteVersion();

                                    }
                                    else if (field.Type == SPFieldType.Text)
                                    {

                                        var formField = (TextBox)pnlRequiredFields.FindControl("DynamicCtrlField_" + field.InternalName);
                                        if (formField == null) continue;
                                        listItem[field.InternalName] = formField.Text;
                                        listItem.UpdateOverwriteVersion();

                                    }
                                    else if (field.TypeDisplayName.ToLower().Equals("managed metadata"))
                                    {
                                        string selectedTaxonomy = string.Empty;
                                        var formField = (HiddenField)pnlRequiredFields.FindControl("DynamicCtrlField_" + field.InternalName + "_Hidden");
                                        if (formField == null) continue;
                                        selectedTaxonomy = formField.Value;

                                        var taxonomyField = field as Microsoft.SharePoint.Taxonomy.TaxonomyField;
                                        if (taxonomyField.AllowMultipleValues)
                                        {
                                            var fieldValuesCollection = new Microsoft.SharePoint.Taxonomy.TaxonomyFieldValueCollection(string.Empty);
                                            try
                                            {
                                                var assignedTaxonomies = selectedTaxonomy.Split(',');
                                                foreach (string value in assignedTaxonomies)
                                                {
                                                    var taxGuid = _listofTax.FirstOrDefault(x => x.Value == value).Key;
                                                    var taxName = value.Split('>').Last();
                                                    if (taxGuid != null && taxName != null)
                                                    {

                                                        var taxonomyFieldValue = new Microsoft.SharePoint.Taxonomy.TaxonomyFieldValue(string.Empty)
                                                        {
                                                            TermGuid = taxGuid,
                                                            Label = taxName
                                                        };
                                                        // term.Id.ToString();
                                                        //term.Name;
                                                        fieldValuesCollection.Add(taxonomyFieldValue);
                                                    }
                                                }
                                                taxonomyField.SetFieldValue(listItem, fieldValuesCollection);
                                            }
                                            catch (Exception ex)
                                            {
                                                Logging.LogError(ex);
                                            }
                                        }
                                        else
                                        {
                                            try
                                            {
                                                var assignedTaxonomies = selectedTaxonomy.Split(',');

                                                if (assignedTaxonomies.Any())
                                                {
                                                    Microsoft.SharePoint.Taxonomy.TaxonomyFieldValue taxonomyFieldValue = new Microsoft.SharePoint.Taxonomy.TaxonomyFieldValue(string.Empty);
                                                    foreach (string value in assignedTaxonomies)
                                                    {
                                                        var taxGuid = _listofTax.FirstOrDefault(x => x.Value == value).Key;
                                                        var taxName = selectedTaxonomy.Split('>').Last();
                                                        if (taxGuid != null && taxName != null)
                                                        {
                                                            taxonomyFieldValue.TermGuid = taxGuid;// term.Id.ToString();
                                                            taxonomyFieldValue.Label = taxName;//term.Name;
                                                            break;
                                                        }

                                                    }
                                                    taxonomyField.SetFieldValue(listItem, taxonomyFieldValue);
                                                }

                                            }
                                            catch (Exception ex)
                                            {
                                                Logging.LogError(ex);
                                            }
                                        }
                                        listItem.UpdateOverwriteVersion();
                                    }
                                    else
                                    {
                                        FormField formField = (FormField)pnlRequiredFields.FindControl("DynamicCtrlField_" + field.InternalName); if (formField == null) continue;
                                        listItem[field.InternalName] = formField.Value;
                                        listItem.UpdateOverwriteVersion();
                                    }

                                }
                            }
                            listItem.Update();

                            if (file.CheckedOutByUser != null)
                            {
                                try
                                {
                                    if (!hdnMinorCheckInEnable.Value.Equals("major"))
                                    {

                                        if (checkInFile.Checked || checkInCheckOutFile.Checked)
                                        {
                                            var comment = checkInFile.Checked ? checkinComment.Value : checkInCheckOutComment.Value;
                                            if (checkInFile.Checked)
                                            {
                                                file.CheckIn(comment, checkInDraft.Checked ? SPCheckinType.MinorCheckIn : SPCheckinType.MajorCheckIn);
                                            }
                                            else
                                            {
                                                file.CheckIn(comment, checkInCheckoutDraft.Checked ? SPCheckinType.MinorCheckIn : SPCheckinType.MajorCheckIn);
                                            }
                                        }
                                        if (checkInCheckOutFile.Checked)
                                            file.CheckOut();
                                    }
                                    else
                                    {
                                        string comment = dd_checkincomment.Value;
                                        file.CheckIn(comment, SPCheckinType.MajorCheckIn);
                                        if (dd_fileCheckinYes.Checked)
                                            file.CheckOut();
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Logging.LogError(ex);
                                }
                            }
                            else
                            {
                                try
                                {
                                    SPFileLevel moderationInformation = file.Item.Level;
                                    if (!hdnMinorCheckInEnable.Value.Equals("major"))
                                    {
                                        if (moderationInformation == SPFileLevel.Draft)
                                        {
                                            if (checkInFile.Checked || checkInCheckOutFile.Checked)
                                            {
                                                var comment = checkInFile.Checked ? checkinComment.Value : checkInCheckOutComment.Value;
                                                if (checkInFile.Checked)
                                                {
                                                    if (checkInPublish.Checked)
                                                        file.Publish(comment);
                                                }
                                                else
                                                {
                                                    if (checkInCheckOutPublish.Checked)
                                                        file.Publish(comment);
                                                }
                                            }
                                        }
                                        if (checkInCheckOutFile.Checked)
                                            file.CheckOut();
                                    }
                                    else
                                    {
                                        string comment = dd_checkincomment.Value;
                                        if (moderationInformation == SPFileLevel.Draft)
                                        {
                                            file.Publish(comment);
                                        }
                                        if (dd_fileCheckinYes.Checked)
                                            file.CheckOut();
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Logging.LogError(ex);
                                }
                            }
                            oSpWeb.AllowUnsafeUpdates = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ltlUploadSuccess.Text += ex.Message.Replace(strFolderUrl, "") + "<br/>";

            }
        }

        private void DisplayTermStoreTree()
        {
            _sbTaxonomy = new StringBuilder();

            SPSite thisSite = SPContext.Current.Site;

            var session = new Microsoft.SharePoint.Taxonomy.TaxonomySession(thisSite);

            if (session.TermStores != null && session.TermStores.Count > 0)
            {

                Microsoft.SharePoint.Taxonomy.TermStore termStore = session.TermStores[0];

                Microsoft.SharePoint.Taxonomy.Group group = termStore.GetSiteCollectionGroup(thisSite);

                foreach (Microsoft.SharePoint.Taxonomy.TermSet termSet in group.TermSets)
                {
                    ltlTaxonomyView.Text = string.Empty;
                    _sbTaxonomy.AppendFormat("<ul category='{0}'>", termSet.Name.ToLower());
                    foreach (Microsoft.SharePoint.Taxonomy.Term term in termSet.Terms)
                    {
                        DiplayTermSet(term, termSet.Name);
                    }
                    _sbTaxonomy.Append("</ul>");
                    ltlTaxonomyView.Text = _sbTaxonomy.ToString();
                }
            }
        }
        /// <summary>
        /// Bind the term set
        /// </summary>
        /// <param name="term"></param>
        /// <param name="termString"></param>
        private void DiplayTermSet(Microsoft.SharePoint.Taxonomy.Term term, string termString)
        {

            string strTermString = termString + ">" + term.Name;
            _sbTaxonomy.AppendFormat(
                term.IsAvailableForTagging
                    ? "<li><input type='checkbox' value='{1}' id='{0}' /><label>{0}</label>"
                    : "<li><input type='checkbox' disabled='' value='{1}' id='{0}' /><label>{0}</label>", term.Name, strTermString.ToLower());

            if (term.Terms.Count > 0)
            {
                _sbTaxonomy.Append("<ul>");
                foreach (Microsoft.SharePoint.Taxonomy.Term t in term.Terms)
                {
                    DiplayTermSet(t, strTermString);
                }
                _sbTaxonomy.Append("</ul>");
            }
            _sbTaxonomy.Append("</li>");

        }

        /// <summary>
        ///     Set the datatable value in Viewstate
        /// </summary>
        protected void viewSateReset_Click(object sender, EventArgs e)
        {
            ViewState["Data"] = null;
            pnlRequiredFields.Controls.Clear();
            BindRequiredFields();
        }


    }
}