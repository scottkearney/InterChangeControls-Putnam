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
                string libName = "/js/vendor/jquery-2.1.3.min.js";
                string[] jsLinks =
            {
                "/js/vendor/modernizr.js","/js/vendor/picker.js", "/js/vendor/picker.date.js", "/js/vendor/tablesaw.stackonly.js","/js/vendor/jquery.sticky.js",
                "/js/vendor/jquery.magnific-popup.js", "/js/components/modal.js", "/js/vendor/quicksearch.js", "/js/controls/ia-document-list.js","/js/vendor/jstree.js","/js/components/datepicker.js",
                "/js/vendor/jquery.dropdown.js", "/js/idle.js", 
                "/js/controls/ia-drag-drop.js",
                "/js/components/accordion.js",  "/js/controls/ia-document-grid.js",
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
                            BindData();
                        }

                        else if (GetCurrentPath != null)
                        {

                            if (GetCurrentPath != null && !string.IsNullOrEmpty(GetCurrentPath.CurrentPath))// &&                                     currentFolderPath.Value != GetCurrentPath.CurrentPath)
                            {
                                currentFolderPath.Value = GetCurrentPath.CurrentPath;
                                BindData();
                            }

                            else
                                BindData();
                        }
                        else
                            BindData();
                    }
                    else
                        _sorting = false;
                }
                else if (grid != null && grid.HeaderRow != null)
                {
                    grid.HeaderRow.TableSection = TableRowSection.TableHeader;
                    grid.HeaderRow.CssClass = "sticky";
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

        private void BindListOfLibraries()
        {
            try
            {
                _strLibraries = new List<string>();
                if (HttpContext.Current.Request.QueryString[QueryParam] != null && !string.IsNullOrEmpty(HttpContext.Current.Request.QueryString[QueryParam]))
                {
                    string strQueryLibraries = HttpContext.Current.Request.QueryString[QueryParam].Trim();
                    if (!strQueryLibraries.Equals("all"))
                    {
                        var queryLibraries = strQueryLibraries.Split(',');
                        _strLibraries = (from library in queryLibraries where !string.IsNullOrWhiteSpace(library) select library.Trim('#')).ToList();
                    }
                }

                if (_strLibraries.Count == 0)
                {
                    if (!string.IsNullOrWhiteSpace(ListName))
                        _strLibraries.Add(ListName);
                }
                BindRequiredFields();
            }
            catch (Exception ex)
            {
                Logging.LogError(ex);
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

                string[] defaultMenuOptions = { Resources.btnCheckIn, Resources.btnOpen, Resources.btnCheckOutAll, Resources.btnDiscardChkOutAll, Resources.btnDownload, Resources.btnDelete, Resources.btnMore };
                string[] defaultMoreOptions = { Resources.btnCheckIn, Resources.btnDiscardCheckOut, Resources.btnCheckOut, Resources.btnViewProperties, Resources.btnEditProperties, Resources.btnShare, Resources.btnShareWith, Resources.btnFollow, Resources.btnCompliance, Resources.btnWorkflow };

                hdnContextMenuOptions.Value = string.Join(";", defaultMenuOptions);
                hdnContextMenuMoreOptions.Value = string.Join(";", defaultMoreOptions);

                if (refreshIdleGrid.Value != "true")
                {
                    if (!Page.IsPostBack)
                    {
                        var j = 0;

                        var arrProperties = Resources.Grid_Header_ColumnNames.Split('|');

                        for (var i = 2; i <= (arrProperties.Length + 1); i++)
                        {
                            grid.Columns[i].HeaderText = arrProperties[j];
                            i++;
                            j++;
                        }
                    }
                    else
                    {
                        UploadFile(sender, e);
                    }
                }

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
        ///     Bind the values to DataTable whenever server side operations are performed 
        /// </summary>       
        private DataTable GetDataTable(bool recursive = false)
        {
            try
            {
                _dt = new DataTable();
                if (_strLibraries.Count > 0)
                {
                    SPSecurity.RunWithElevatedPrivileges(delegate
                    {
                        var viewFields =
                            "<FieldRef Name='ID' /><FieldRef Name='File_x0020_Type' /><FieldRef Name='LinkFilename' /><FieldRef Name='Modified' /><FieldRef Name='Editor' />";
                        var currentWeb = SPContext.Current.Web;
                        recursive = true;

                        string documentListUrl = "#";
                        if (_strLibraries.Count == 1)
                        {

                            recursive = false;

                            if (string.IsNullOrEmpty(currentFolderPath.Value) || (!string.IsNullOrEmpty(currentFolderPath.Value) && currentFolderPath.Value.Split('/').Length == 1))
                                ltlbreadcrumb.Text = string.Format("<a title='Document Libraries' style='color:#fff;' href='{0}'>Document Libraries</a> > {1}", documentListUrl, _strLibraries.FirstOrDefault());
                            else
                            {
                                string previousFolderName = string.Empty, previousFolderPath = string.Empty, currentFolderName = string.Empty;
                                string[] folderPath = currentFolderPath.Value.Split('/');

                                previousFolderName = folderPath[folderPath.Length - 2];
                                currentFolderName = folderPath[folderPath.Length - 1];
                                if (folderPath.Length == 2)
                                    previousFolderName = _strLibraries.FirstOrDefault();
                                previousFolderPath = string.Join("/", folderPath.Take(folderPath.Length - 1));
                                ltlbreadcrumb.Text = string.Format("<a title=\"{0}\" style='color:#fff;' href=\"#\" onclick=\"getfolder('{1}');return false;\">{0}</a> > {2}", previousFolderName, previousFolderPath, currentFolderName);
                            }

                            uploadCreateFolder.Visible = true;
                            singleDoumentBreadCrumb.Visible = true;
                        }
                        else
                        {
                            ltlbreadcrumb.Text = string.Format("<a title='Document Libraries' style='color:#fff;' href='{0}'>Document Libraries</a> > Search", documentListUrl);
                            uploadCreateFolder.Visible = false;
                            singleDoumentBreadCrumb.Visible = false;
                        }
                        
                        foreach (string strLibrary in _strLibraries)
                        {
                            var docLib = (SPDocumentLibrary)currentWeb.Lists[strLibrary];//(SPDocumentLibrary)currentWeb.Lists[this.ListName];

                            listName.Value = docLib.RootFolder.Name;
                            listDisplayName.Value = docLib.Title;
                            listGuidId.Value = docLib.ID.ToString();
                            hdnMinorCheckInEnable.Value = (docLib.EnableVersioning && docLib.EnableMinorVersions) ? "minor" : docLib.EnableVersioning ? "major" : "no";
                            webUrl.Value = currentWeb.Url;

                            if (hdnMinorCheckInEnable.Value.Equals("major"))
                                dd_fileCheckInMajorOption.Visible = true;

                            var folderPath = "";
                            webServerUrl.Value = currentWeb.ServerRelativeUrl.Equals("/") ? "" : currentWeb.ServerRelativeUrl;

                            lstForceCheckout.Value = docLib.ForceCheckout.ToString().ToLower();
                            var sQuery = new SPQuery
                            {
                                Query = "<OrderBy><FieldRef Name='LinkFilename' Ascending='True'  /></OrderBy>"
                            };

                            if (docLib.Fields.ContainsField(MetaField))
                                viewFields += "<FieldRef Name='" + MetaField + "' />";
                            sQuery.ViewFields = viewFields;
                            sQuery.ViewFieldsOnly = true;

                            if (recursive)
                                sQuery.ViewAttributes = "Scope='Recursive'";
                            else if (ddlSelect.SelectedValue != "This Folder")
                                sQuery.ViewAttributes = "Scope='Recursive'";

                            if (!recursive)

                                folderPath = currentFolderPath.Value;

                            //
                            var gTable = new GridDataTable
                            {
                                ListName = docLib.Title,
                                Stylelib = RootResourcePath,
                                ListofTax = _listofTax,
                                ListDisplayName = docLib.Title,
                                ListId = docLib.ID.ToString(),
                                ForceCheckOut = docLib.ForceCheckout.ToString().ToLower()
                            };
                            if (!string.IsNullOrEmpty(folderPath))
                            {
                                var folder = currentWeb.GetFolder(folderPath);
                                if (folder != null)
                                {
                                    sQuery.Folder = folder;

                                    try
                                    {
                                        gTable.ListPermission = folder.EffectiveRawPermissions != null ? folder.EffectiveRawPermissions.HasFlag(SPBasePermissions.EditListItems) : false;
                                    }
                                    catch
                                    {
                                        gTable.ListPermission = docLib.DoesUserHavePermissions(SPBasePermissions.EditListItems);
                                    }
                                }
                                else
                                    gTable.ListPermission = docLib.DoesUserHavePermissions(SPBasePermissions.EditListItems);
                            }
                            else
                                gTable.ListPermission = docLib.DoesUserHavePermissions(SPBasePermissions.EditListItems);
                            createpermission.Value = gTable.ListPermission ? "yes" : "no";
                            var myColl = docLib.GetItems(sQuery);
                            DataTable currentLibTable = gTable.BindSpListItemstoTable(myColl);
                            if (currentLibTable != null)
                            {
                                if (_dt.Rows.Count > 0)
                                {
                                    _dt.Merge(currentLibTable);
                                }
                                else
                                    _dt = currentLibTable;
                            }
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                Logging.LogError(ex);
            }

            return _dt;
        }

        /// <summary>
        ///     Performs data bind operation whenever required
        /// </summary>       
        private void BindData()
        {
            try
            {
                _dt = GetDataTable();
                var filterView = new DataView(_dt);

                ViewState["Data"] = _dt;
                if (filterView.Count > 0)
                    _dt = filterView.ToTable();
                grid.DataSource = _dt;
                grid.PageSize = 100000;
                grid.DataBind();
                grid.HeaderRow.TableSection = TableRowSection.TableHeader;
                grid.HeaderRow.CssClass = "sticky";
            }
            catch (Exception ex)
            {
                Logging.LogError(ex);
            }
        }

        /// <summary>
        ///     RowDataBound event is used to assign the up and down arrow for the Document Header.
        /// </summary>
        protected void grid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    var headerStr = new[] { "Name", "Modified", "Modified By" };

                    if ((Label)e.Row.FindControl("headerlbl1") != null)
                        ((Label)e.Row.FindControl("headerlbl1")).Text = headerStr[0];
                    ((Label)e.Row.FindControl("headerlbl2")).Text = headerStr[1];
                    ((Label)e.Row.FindControl("headerlbl3")).Text = headerStr[2];
                }
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

        /// <summary>
        ///     Set the datatable value in Viewstate
        /// </summary>
        protected void viewSateReset_Click(object sender, EventArgs e)
        {
            ViewState["Data"] = null;
            pnlRequiredFields.Controls.Clear();
            BindRequiredFields();
        }

        /// <summary>
        ///     GridDataTable forms the needed DataTable structure for Bind the items
        /// </summary>
        private class GridDataTable
        {
            private readonly string[] _columns =
            {
                "", "TypeUrl", "LinkFilename", "Modified", "Editor", "ID",
                "File_x0020_Type", "Path", "Status", "ModifiedTime","EditorClone","LinkFilenameClone","EditPermission","PopularCount","ListId","ListDisplayName","ForceCheckOut","DatenTime"
            };

            private readonly DataTable _dTable = new DataTable();
            private readonly UserProfileManager _profileManager = new UserProfileManager();
            public bool ListPermission = false;
            public string ListName = string.Empty;
            private int _auditCount = 0;
            private SPAuditQuery _wssQuery;
            private SPAuditEntryCollection _auditCol;
            private SPWeb _elevated;
            private SPList _listCommon;
            public string Stylelib;
            private string _weburl = SPContext.Current.Web.Url;
            public Dictionary<string, string> ListofTax = new Dictionary<string, string>();
            public string ListDisplayName;
            public string ListId;
            public string ForceCheckOut;


            /// <summary>
            ///     Assign the table Column fields
            /// </summary>
            public GridDataTable()
            {
                foreach (var columnName in _columns)
                {
                    _dTable.Columns.Add(columnName, typeof(string));
                }
                _dTable.Columns.Add(MetaField);
            }


            /// <summary>
            ///     Bind the SPListItems into a DataTable
            /// </summary>
            /// <param name="spItemsCollection">Items need to be binded</param>
            /// <returns>Datatable after binding the Items</returns>
            public DataTable BindSpListItemstoTable(SPListItemCollection spItemsCollection)
            {
                try
                {
                    SPSecurity.RunWithElevatedPrivileges(delegate
                    {
                        var elevatedSiteColl = new SPSite(SPContext.Current.Site.ID);
                        _wssQuery = new SPAuditQuery(elevatedSiteColl);
                        _elevated = elevatedSiteColl.OpenWeb();
                        _listCommon = _elevated.Lists[ListName];

                    });
                    string checkOutIcon = "<img class='ia-doc-checkedOut' src='../_layouts/15/Akumina.WebParts.Documents/images/icons/checkoutoverlay.gif'>";
                    string iconUrl = string.Empty;
                    string filetype = string.Empty;
                    string notebook = "OneNote.Notebook";
                    string userName = string.Empty;
                    string fileName = string.Empty;
                    string id = string.Empty;
                    DateTime modifiedTime = DateTime.Now;
                    string colNameTypeUrl = _dTable.Columns[2].ColumnName;
                    string colNameFileType = _dTable.Columns[6].ColumnName;
                    string colNameFileName = _dTable.Columns[2].ColumnName;
                    string colNameModified = _dTable.Columns[3].ColumnName;
                    string colNameEditor = _dTable.Columns[4].ColumnName;
                    string metaDataValue = string.Empty;
                    foreach (SPListItem item in spItemsCollection)
                    {
                        try
                        {
                            userName = item[colNameEditor].ToString().Split('#')[1];
                            modifiedTime = Convert.ToDateTime(item[colNameModified].ToString());
                            fileName = Path.GetFileNameWithoutExtension(item[colNameFileName].ToString());
                            var itemIsFile = item.File != null;
                            try
                            {
                                metaDataValue = (item.Fields.ContainsField(MetaField) && item[MetaField] != null && !string.IsNullOrEmpty(item[MetaField].ToString())) ? getMetaField(item[MetaField].ToString()) : string.Empty;
                            }
                            catch
                            {
                                metaDataValue = string.Empty;
                            }
                            id = item[_dTable.Columns[5].ColumnName].ToString();
                            if (item.Folder != null && item.Folder.ProgID != notebook)
                            {
                                iconUrl = GetIconUrl(Path.GetExtension(item[colNameTypeUrl].ToString()));
                                filetype = item[colNameFileType] != null ? item[colNameFileType].ToString() : string.Empty;
                            }
                            else if (item.Folder != null)
                            {
                                iconUrl = GetIconUrl(".one");
                                filetype = "one";
                            }
                            else
                            {
                                iconUrl = GetIconUrl(Path.GetExtension(item[colNameTypeUrl].ToString()));
                                filetype = item[colNameFileType] != null ? item[colNameFileType].ToString() : string.Empty;
                            }

                            _dTable.Rows.Add("", iconUrl, fileName, string.Format("{0:MM/dd/yyyy}", modifiedTime), GetUserDetail(item.Web, userName), id
                                , filetype,
                                itemIsFile ? item.File.Url : item.Folder.Url,
                                itemIsFile ? ((item.File.CheckOutType.ToString().ToLower() == "online") ? checkOutIcon : string.Empty) : string.Empty,
                                string.Format("{0:MM/dd/yyyy 'at' hh:mmtt}", modifiedTime).ToLower(),
                                userName,
                                fileName,
                                !ListPermission ? "no:no" : (item.DoesUserHavePermissions(SPBasePermissions.EditListItems) && (item.Level.ToString().ToLower() == "checkout" || item.DoesUserHavePermissions(SPBasePermissions.CancelCheckout))) ? "yes:yes" : item.DoesUserHavePermissions(SPBasePermissions.EditListItems) ? "yes:no" : "no:no",
                                itemIsFile ? GetAduitCountForListItem(id) : "0",
                               ListId, ListDisplayName + ";" + ListName, ForceCheckOut, string.Format("{0:MM/dd/yyyy HH:mm:ss}", modifiedTime).ToLower(),
                                metaDataValue

                                );
                        }
                        catch (Exception ex)
                        {
                            Logging.LogError(ex);
                        }
                    }
                    _elevated.Dispose();
                    _elevated.Close();
                }
                catch (Exception ex)
                {
                    Logging.LogError(ex);
                }

                return _dTable;
            }


            private string getMetaField(string metaValue)
            {
                var metaArr = new List<string>();
                try
                {
                    var multiValues = metaValue.Split(';').ToList();
                    foreach (var single in multiValues)
                    {
                        var value = single.Split('|');
                        if (value.Length > 1 && ListofTax.ContainsKey(value[1]))
                        {
                            var metasingleValue = ListofTax[value[1]];
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

            private string GetAduitCountForListItem(string id)
            {
                try
                {
                    _auditCount = 0;
                    SPListItem oItem = _listCommon.GetItemById(int.Parse(id));
                    _wssQuery.RestrictToListItem(oItem);
                    _auditCol = _elevated.Audit.GetEntries(_wssQuery);

                    if (_auditCol != null && _auditCol.Count > 0)
                        _auditCount = _auditCol.Count;
                }
                catch (Exception ex)
                {
                    Logging.LogError(ex);
                    return _auditCount.ToString();
                }

                return _auditCount.ToString();
            }


            /// <summary>
            ///     Bind the User Link text into a DataTable
            /// </summary>
            /// <param name="web">web is used to get the user details</param>
            /// <param name="userName">Form the UserLink as it is SharePoint</param>
            /// <returns>User Link string </returns>
            private string GetUserDetail(SPWeb web, string userName)
            {
                var genLink = "<a class='userlink' href='{2}Person.aspx?accountname={0}' target='_blank'>{1}</a>";
                try
                {
                    var user = (from SPUser c in web.AllUsers
                                where c.Name == userName
                                select c).FirstOrDefault();
                    genLink = string.Format(genLink,
                        user.LoginName.Contains('|') ? user.LoginName.Split('|')[1] : user.LoginName, user.Name,
                        _profileManager.MySiteHostUrl);
                }
                catch (Exception ex)
                {
                    Logging.LogError(ex);
                }

                return genLink;
            }

            /// <summary>
            ///     Bind the Icon image into a DataTable
            /// </summary>
            /// <param name="icon">string is used to get the relevant icon image</param>
            /// <returns>Icon Image string </returns>
            private string GetIconUrl(string icon)
            {
                var imagePAth = string.Empty;
                try
                {
                    var docLibName = "";
                    if (!string.IsNullOrEmpty(Stylelib))
                    {
                        if (Stylelib.Contains("http"))
                            docLibName = Stylelib;
                        else if (!Stylelib.StartsWith("/"))
                            docLibName = _weburl + "/" + Stylelib;
                    }
                    var iconVal = !string.IsNullOrEmpty(icon) ? icon.Substring(1).ToLower() : string.Empty;
                    switch (iconVal)
                    {
                        case "doc":
                        case "docx":
                            imagePAth = docLibName + "/images/icons/icdocx.png";
                            break;
                        case "gif":
                            imagePAth = docLibName + "/images/icons/icgif.gif";
                            break;
                        case "jpeg":
                        case "jpg":
                            imagePAth = docLibName + "/images/icons/icjpg.gif";
                            break;
                        case "notebk":
                        case "one":
                        case "onetoc2":
                            imagePAth = docLibName + "/images/icons/icnotebk.png";
                            break;
                        case "pdf":
                            imagePAth = docLibName + "/images/icons/icpdf.png";
                            break;
                        case "png":
                            imagePAth = docLibName + "/images/icons/icpng.gif";
                            break;
                        case "ppt":
                        case "pptx":
                            imagePAth = docLibName + "/images/icons/icpptx.png";
                            break;
                        case "mpp":
                        case "pub":
                            imagePAth = docLibName + "/images/icons/icpub.png";
                            break;
                        case "vsx":
                        case "vsdx":
                            imagePAth = docLibName + "/images/icons/icvsx.png";
                            break;
                        case "xls":
                        case "xsl":
                        case "xslx":
                        case "xlsx":
                            imagePAth = docLibName + "/images/icons/icxlsx.png";
                            break;
                        case "txt":
                            imagePAth = docLibName + "/images/icons/ictxt.gif";
                            break;
                        case "zip":
                            imagePAth = docLibName + "/images/icons/iczip.gif";
                            break;
                        case "":
                            imagePAth = "folder";
                            break;
                        default:
                            imagePAth = docLibName + "/images/icons/icjpg.gif";
                            break;
                    }

                }
                catch (Exception ex)
                {
                    Logging.LogError(ex);
                }
                return imagePAth;
            }
        }
    }
}