using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Akumina.InterAction;
using Akumina.WebParts.Documents.DocumentGrid;
using Akumina.WebParts.DocumentsSandbox.Properties;
using Microsoft.SharePoint;

namespace Akumina.WebParts.DocumentsSandbox.DocumentGrid
{
    [ToolboxItem(false)]
    public class DocumentGrid : DocumentGridBaseWebPart
    {
        /// <summary>
        ///     Global Declaration
        /// </summary>
        private Button _btnSearch = new Button();

        private readonly GridView _grid = new GridView();
        private readonly HiddenField _hfieldlistName = new HiddenField();
        private readonly HiddenField _hfieldlistDisplayName = new HiddenField();
        private readonly HiddenField _hfieldlistGuidId = new HiddenField();
        private readonly HiddenField _hfieldwebUrl = new HiddenField();
        private readonly HiddenField _hfieldcurrentFolderPath = new HiddenField();
        private readonly HiddenField _hfieldwebServerUrl = new HiddenField();
        private readonly HiddenField _hfieldcreateperHiddenField = new HiddenField();
        private readonly HiddenField _hfieldcolumnToHidden = new HiddenField();
        private readonly HiddenField _hfieldcolumnToForceCheckout = new HiddenField();
        private readonly HiddenField _hfieldcolumnNames = new HiddenField();


        private readonly TextBox _txtSearch = new TextBox();
        private readonly HtmlGenericControl _lblBreadCrum = new HtmlGenericControl("label");
        private readonly DropDownList _ddlselect = new DropDownList();
        private string webUrl = string.Empty;

        private static readonly string MetaField = Resources.MetaField;

        #region WebPart Properties


        //public override EditorPartCollection CreateEditorParts()
        //{
        //    var editorParts = new List<EditorPart>();
        //    var oWpEditor = new WpEditor();
        //    oWpEditor.ID = ID + "_akuminaEditorPart";
        //    oWpEditor.Title = "Akumina InterAction";
        //    editorParts.Add(oWpEditor);
        //    return new EditorPartCollection(base.CreateEditorParts(), editorParts);
        //}

        #endregion

        private readonly string[] _cloneColumns = Resources._cloneColumns.Split('|');

        protected override void OnInit(EventArgs e)
        {
            try
            {

                var upload = HttpContext.Current.Request.QueryString["upload"];
                if (string.IsNullOrEmpty(upload))
                {
                    loadInstructionSet();
                    //Bind all Child Controls
                    BindChildControlstoPage();
                    Page.Load += Page_Load;
                }
                else
                    Page.Load += Page_Load;
            }
            catch (Exception ex)
            {
                Logging.LogError(ex);
            }
        }

        protected override void CreateChildControls()
        {
        }

        protected override void Render(HtmlTextWriter writer)
        {

            try
            {
                string jquerylibName = Resources.JquerylibraryPath;
                string[] jsLinks = Resources.JS_Links.Split('|');
                string[] cssLinks = Resources.CSS_Links.Split('|');

                if (!string.IsNullOrEmpty(RootResourcePath) && !RootResourcePath.ToLower().Contains("http"))
                {
                    var cWeb = SPContext.Current.Web;
                    SPList oList = null;
                    oList = cWeb.Lists.TryGetList(RootResourcePath);
                    if (oList != null && oList.BaseType == SPBaseType.DocumentLibrary)
                    {
                        var docLib = (SPDocumentLibrary)oList;
                        var oQuery = new SPQuery();
                        oQuery.Query = "<OrderBy><FieldRef Name='Order0' Ascending='True'  /></OrderBy>";
                        oQuery.ViewAttributes = "Scope=\"Recursive\"";
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
                            if (i == 0)
                            {
                                writer.Write("<script type=\"text/javascript\">");
                                writer.Write("if (typeof jQuery == 'undefined') { document.write(unescape(\"%3Cscript src='" + jsFiles[i] + "' type='text/javascript'%3E%3C/script%3E\")); }");
                                writer.Write("</script>");
                            }
                            else
                                writer.Write(BindScript(jsFiles[i].ToLower(), false));
                        }


                        //bind style operation
                        foreach (var cssfile in cssFiles)
                            writer.Write(BindStyle(cssfile.ToLower(), false));

                        writer.Write("<div id='styleUrl' style:'display:none'>" + docLib.RootFolder.ServerRelativeUrl + "/images/icons/" + "</div>");

                    }
                }
                else if (!string.IsNullOrEmpty(RootResourcePath) && RootResourcePath.ToLower().Contains("http"))
                {
                    writer.Write("<script type=\"text/javascript\">");
                    writer.Write("if (typeof jQuery == 'undefined') { document.write(unescape(\"%3Cscript src='" + RootResourcePath + jquerylibName + "' type='text/javascript'%3E%3C/script%3E\")); }");
                    writer.Write("</script>");
                    foreach (var jsfile in jsLinks)
                        writer.Write(@"<script type=""text/javascript"" src=""{0}{1}""></script>", RootResourcePath, jsfile.ToLower());
                    foreach (var csfile in cssLinks)
                        writer.Write(@"<link rel=""stylesheet"" href=""{0}{1}"" type=""text/css"" />", RootResourcePath, csfile.ToLower());

                    writer.Write("<div id='styleUrl' style='display:none'>" + RootResourcePath.ToLower() + "/images/icons/</div>");
                }

                writer.Write(Resources.DMSTemplate);

                base.Render(writer);
            }
            catch (Exception ex)
            {
                Logging.LogError(ex);
            }
        }

        #region Event Handlers functions

        private void grid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    string[] headerStr;
                    headerStr = Resources.Grid_Header_ColumnNames.Split('|');
                    if (((Label)e.Row.FindControl("headerlbl1")) != null)
                        ((Label)e.Row.FindControl("headerlbl1")).Text = headerStr[0];
                    if (((Label)e.Row.FindControl("headerlbl2")) != null)
                        ((Label)e.Row.FindControl("headerlbl2")).Text = headerStr[1];
                    if (((Label)e.Row.FindControl("headerlbl3")) != null)
                        ((Label)e.Row.FindControl("headerlbl3")).Text = headerStr[2];
                }
            }
            catch (Exception ex)
            {
                Logging.LogError(ex);
            }
        }

        #endregion

        #region Additional Functions

        /// <summary>
        ///     Binds all the HTML and Asp Controls to the Page
        /// </summary>
        private void BindChildControlstoPage()
        {
            try
            {
                webUrl = SPContext.Current.Web.Url;
                var mainDiv = new HtmlGenericControl("div");
                mainDiv.Attributes["class"] = "interAction";

                #region Loader

                var fullloaderDiv = new HtmlGenericControl("div");
                fullloaderDiv.Attributes["class"] = "ia-loading-panel ia-hide";

                var loaderDiv = new HtmlGenericControl("div");
                loaderDiv.Attributes["class"] = "ia-loader";

                #endregion

                fullloaderDiv.Controls.Add(loaderDiv);
                mainDiv.Controls.Add(fullloaderDiv);


                var documentListDiv = new HtmlGenericControl("div");
                documentListDiv.ID = "drop_zone";
                documentListDiv.ClientIDMode = ClientIDMode.Static;
                documentListDiv.Attributes["class"] = "ia-documentList";

                var breadCrumpText = new HtmlGenericControl("p");
                breadCrumpText.InnerText = Resources.breadCrumpText;
                breadCrumpText.Attributes["class"] = "hintText";
                _lblBreadCrum.Attributes["class"] = "lblBreadCrump";
                _lblBreadCrum.ID = "lblBreadCrump";
                _lblBreadCrum.ClientIDMode = ClientIDMode.Static;

                breadCrumpText.Controls.Add(_lblBreadCrum);


                documentListDiv.Controls.Add(breadCrumpText);

                /*----------------------------------------------------------*/

                /* Button Row Div*/

                #region Create and Upload Button

                var docLibName = "";
                if (!string.IsNullOrEmpty(RootResourcePath))
                {
                    if (RootResourcePath.Contains("http"))
                        docLibName = RootResourcePath;
                    else if (!RootResourcePath.StartsWith("/"))
                        docLibName = webUrl + "/" + RootResourcePath;
                }

                var btnrowDiv = new HtmlGenericControl("DIV");
                btnrowDiv.Attributes.Add("class", "ia-button-row");
                /* Div Starts for Create & Upload */
                var anchor = new HtmlGenericControl("a");
                anchor.ClientIDMode = ClientIDMode.Static;
                anchor.Attributes.Add("data-dropdown", "#dropdown-1");
                anchor.Attributes.Add("class", "ia-button ia-button-dropdown");
                anchor.Attributes["href"] = "#";
                anchor.ID = "btnCreate";
                anchor.InnerText = Resources.btnCreate;

                var dropDownDiv = new HtmlGenericControl("DIV");
                dropDownDiv.ClientIDMode = ClientIDMode.Static;
                dropDownDiv.ID = "dropdown-1";
                dropDownDiv.Attributes.Add("class", "dropdown dropdown-tip dropdown-relative");
                var createUl = new HtmlGenericControl("ul");
                createUl.ClientIDMode = ClientIDMode.Static;
                createUl.Attributes.Add("class", "dropdown-menu");

                /* Anchor Tag Starts for Folder */
                var fldDocli = new HtmlGenericControl("li");
                fldDocli.ClientIDMode = ClientIDMode.Static;
                var ahrfldDoc = new HtmlGenericControl("a");
                ahrfldDoc.ClientIDMode = ClientIDMode.Static;
                ahrfldDoc.Attributes.Add("onclick", "fileCreateFolder();");
                ahrfldDoc.ID = "btnDoc";
                ahrfldDoc.InnerHtml = "<span class='ia-doclib-file-icon fa fa-folder fac'></span>&nbsp;" + Resources.btnDoc;
                fldDocli.Controls.Add(ahrfldDoc);
                createUl.Controls.Add(fldDocli);
                /* Anchor Tag Ends for Folder */

                /* Anchor Tag Starts for Word */
                var wordDocli = new HtmlGenericControl("li");
                wordDocli.ClientIDMode = ClientIDMode.Static;
                var ahrwordDoc = new HtmlGenericControl("a");
                ahrwordDoc.ClientIDMode = ClientIDMode.Static;
                ahrwordDoc.Attributes.Add("onclick", "fileCreateDocument(1);");
                ahrwordDoc.ID = "btnWordDoc";
                ahrwordDoc.InnerHtml = "<img src='" + docLibName + "/images/icons/icdocx.png'/>&nbsp;" + Resources.btnWordDoc;
                wordDocli.Controls.Add(ahrwordDoc);
                createUl.Controls.Add(wordDocli);

                //* Anchor Tag Ends for Word *//

                //* Anchor Tag start for Excel *//
                var exlDocli = new HtmlGenericControl("li");
                exlDocli.ClientIDMode = ClientIDMode.Static;
                var ahrexlDoc = new HtmlGenericControl("a");
                ahrexlDoc.Attributes.Add("onclick", "fileCreateDocument(2);");
                ahrexlDoc.ID = "btnExcelDoc";
                ahrexlDoc.ClientIDMode = ClientIDMode.Static;
                ahrexlDoc.InnerHtml = "<img src='" + docLibName + "/images/icons/icxlsx.png'/>&nbsp;" + Resources.btnExcelDoc;

                exlDocli.Controls.Add(ahrexlDoc);
                createUl.Controls.Add(exlDocli);
                //* Anchor Tag End for Excel *//

                //* Anchor Tag start for PPT *//
                var pptDocli = new HtmlGenericControl("li");
                pptDocli.ClientIDMode = ClientIDMode.Static;
                var ahrpptDoc = new HtmlGenericControl("a");
                ahrpptDoc.Attributes.Add("onclick", "fileCreateDocument(3);");
                ahrpptDoc.ID = "btnPPTDoc";
                ahrpptDoc.ClientIDMode = ClientIDMode.Static;
                ahrpptDoc.InnerHtml = "<img src='" + docLibName + "/images/icons/icpptx.png'/>&nbsp;" + Resources.btnPPTDoc;
                pptDocli.Controls.Add(ahrpptDoc);
                createUl.Controls.Add(pptDocli);
                //* Anchor Tag End for PPT *//

                //* Anchor Tag start for OneNote *//
                var ontDocli = new HtmlGenericControl("li");
                ontDocli.ClientIDMode = ClientIDMode.Static;
                var ahrontDoc = new HtmlGenericControl("a");
                ahrontDoc.Attributes.Add("onclick", "fileCreateDocument(4);");
                ahrontDoc.ID = "btnONoteDoc";
                ahrontDoc.InnerHtml = "<img src='" + docLibName + "/images/icons/icnotebk.png'/>&nbsp;" + Resources.btnONoteDoc;
                ahrontDoc.ClientIDMode = ClientIDMode.Static;

                ontDocli.Controls.Add(ahrontDoc);
                createUl.Controls.Add(ontDocli);

                //* Anchor Tag End for PPT *//
                dropDownDiv.Controls.Add(createUl);

                //Anchor Tag start for Upload Button *//

                var anchrUpload = new HtmlGenericControl("a");
                anchrUpload.ClientIDMode = ClientIDMode.Static;
                anchrUpload.Attributes["class"] = "ia-button";
                anchrUpload.Attributes.Add("onclick", "fileUpload();");
                anchrUpload.ID = "btnUpload";
                anchrUpload.InnerText = Resources.btnUpload;
                anchrUpload.Attributes["href"] = "#";
                btnrowDiv.Controls.Add(anchor);
                btnrowDiv.Controls.Add(dropDownDiv);
                btnrowDiv.Controls.Add(anchrUpload);


                //Anchor Tag end for Upload Button *//

                #endregion

                /*Div Starts for Search*/

                #region Search controls

                var searchDiv = new HtmlGenericControl("DIV");
                searchDiv.ClientIDMode = ClientIDMode.Static;
                searchDiv.Attributes.Add("class", "ia-search-documentList");
                searchDiv.ID = "searchZone";


                var _searchicon = new HtmlGenericControl("span");

                _searchicon.Attributes.Add("class", "ia-searchInput fa fa-search");
                searchDiv.Controls.Add(_searchicon);

                _txtSearch.ID = "txtSearch";
                _txtSearch.ClientIDMode = ClientIDMode.Static;
                _txtSearch.Attributes.Add("class", "ia-searchBox");
                //_txtSearch.Attributes.Add("onkeypress", "detect_enter(event);");
                searchDiv.Controls.Add(_txtSearch);

                _ddlselect.ID = "ddlSelect";
                _ddlselect.ClientIDMode = ClientIDMode.Static;
                _ddlselect.Attributes.Add("class", "ia-searchDropdown");
                _ddlselect.Attributes.Add("onchange", "searchterm();");
                var options = Resources.SearchDropDwonOptions.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var option in options)
                {
                    _ddlselect.Items.Add(option);
                }
                searchDiv.Controls.Add(_ddlselect);


                //var htmlSearchBtn = new HtmlGenericControl("DIV");
                //htmlSearchBtn.Attributes.Add("class", "ia-button");
                //htmlSearchBtn.Attributes.Add("onclick", "searchterm();");
                //htmlSearchBtn.InnerHtml = "<span class='fa fa-search'></span>";
                //searchDiv.Controls.Add(htmlSearchBtn);

                //Div Ends for Search*//

                #endregion

                documentListDiv.Controls.Add(btnrowDiv);
                documentListDiv.Controls.Add(searchDiv);

                #region Context Menu

                //* Div Starts for Context Menu *//


                var ctxUl = new HtmlGenericControl("ul");
                ctxUl.ID = "ulContextMenu";
                ctxUl.ClientIDMode = ClientIDMode.Static;
                ctxUl.Attributes.Add("class", "menu ia-button-group");
                //* Anchor tag starts for Check In *//
                var ctxLiChkIn = new HtmlGenericControl("li");
                ctxLiChkIn.ClientIDMode = ClientIDMode.Static;
                var anchChkIn = new HtmlGenericControl("a");
                anchChkIn.Attributes.Add("href", "#");
                anchChkIn.ID = "btnCheckIn";
                anchChkIn.ClientIDMode = ClientIDMode.Static;
                anchChkIn.Attributes.Add("class", "ia-button secondary");
                anchChkIn.InnerText = Resources.btnCheckIn;
                anchChkIn.Attributes.Add("onclick", "fileCheckIn();");
                ctxLiChkIn.Controls.Add(anchChkIn);
                ctxUl.Controls.Add(ctxLiChkIn);
                //* Anchor tag ends for Check In *//

                //* Anchor tag starts for Open *//
                var ctxLiOpen = new HtmlGenericControl("li");
                ctxLiOpen.ClientIDMode = ClientIDMode.Static;
                var anchOpn = new HtmlGenericControl("a");
                anchOpn.Attributes.Add("href", "#");
                anchOpn.ID = "btnOpen";
                anchOpn.ClientIDMode = ClientIDMode.Static;
                anchOpn.Attributes.Add("class", "ia-button secondary");
                anchOpn.InnerText = Resources.btnOpen;
                anchOpn.Attributes.Add("onclick", "fileOpen(event);");
                ctxLiOpen.Controls.Add(anchOpn);
                ctxUl.Controls.Add(ctxLiOpen);
                //* Anchor tag ends for Open *//

                //* Anchor tag starts for Check Out *//
                var ctxLiChkOut = new HtmlGenericControl("li");
                ctxLiChkOut.ClientIDMode = ClientIDMode.Static;
                var anchChkOut = new HtmlGenericControl("a");
                anchChkOut.Attributes.Add("href", "#");
                anchChkOut.ID = "btnCheckOutAll";
                anchChkOut.ClientIDMode = ClientIDMode.Static;
                anchChkOut.Attributes.Add("class", "ia-button secondary");
                anchChkOut.InnerText = Resources.btnCheckOutAll;
                anchChkOut.Attributes.Add("onclick", "fileCheckOut();");
                ctxLiChkOut.Controls.Add(anchChkOut);
                ctxUl.Controls.Add(ctxLiChkOut);
                //* Anchor tag ends for Check Out *//

                //* Anchor tag starts for All Discard Check Out *//
                var ctxLiChkOutAll = new HtmlGenericControl("li");
                ctxLiChkOutAll.ClientIDMode = ClientIDMode.Static;
                var anchChkOutAll = new HtmlGenericControl("a");
                anchChkOutAll.Attributes.Add("href", "#");
                anchChkOutAll.ID = "btnDiscardChkOutAll";
                anchChkOutAll.ClientIDMode = ClientIDMode.Static;
                anchChkOutAll.Attributes.Add("class", "ia-button secondary");
                anchChkOutAll.InnerText = Resources.btnDiscardChkOutAll;
                anchChkOutAll.Attributes.Add("onclick", "fileDisCheckOut();");
                ctxLiChkOutAll.Controls.Add(anchChkOutAll);
                ctxUl.Controls.Add(ctxLiChkOutAll);
                //* Anchor tag ends for Check Out *//

                //* Anchor tag starts for Download *//
                var ctxLiDwd = new HtmlGenericControl("li");
                ctxLiDwd.ClientIDMode = ClientIDMode.Static;
                var anchDwd = new HtmlGenericControl("a");
                anchDwd.Attributes.Add("href", "#");
                anchDwd.ID = "btnDownload";
                anchDwd.ClientIDMode = ClientIDMode.Static;
                anchDwd.Attributes.Add("class", "ia-button secondary");
                anchDwd.InnerText = Resources.btnDownload;
                anchDwd.Attributes.Add("onclick", "fileDownload();");
                ctxLiDwd.Controls.Add(anchDwd);
                ctxUl.Controls.Add(ctxLiDwd);
                //* Anchor tag ends for Download *//

                //* Anchor tag starts for Delete *//
                var ctxLiDlt = new HtmlGenericControl("li");
                ctxLiDlt.ClientIDMode = ClientIDMode.Static;
                var anchDlt = new HtmlGenericControl("a");
                anchDlt.Attributes.Add("href", "#");
                anchDlt.ID = "btnDelete";
                anchDlt.ClientIDMode = ClientIDMode.Static;
                anchDlt.Attributes.Add("class", "ia-button secondary");
                anchDlt.InnerText = Resources.btnDelete;
                anchDlt.Attributes.Add("onclick", "fileDelete();");
                ctxLiDlt.Controls.Add(anchDlt);
                ctxUl.Controls.Add(ctxLiDlt);
                //* Anchor tag ends for Delete *//

                //* Anchor tag starts for More *//
                var ctxLiMore = new HtmlGenericControl("li");
                ctxLiMore.ClientIDMode = ClientIDMode.Static;
                var anchMore = new HtmlGenericControl("a");
                anchMore.Attributes.Add("href", "#");
                anchMore.ID = "btnMore";
                anchMore.ClientIDMode = ClientIDMode.Static;
                anchMore.InnerText = Resources.btnMore;
                anchMore.Attributes.Add("data-dropdown", "#dropdown-2");
                anchMore.Attributes.Add("class", "ia-button secondary ia-button-dropdown");
                ctxLiMore.Controls.Add(anchMore);


                //* Anchor tag ends for More *//

                //* Div Starts for More Menu *//
                var moreDiv = new HtmlGenericControl("DIV");
                moreDiv.ClientIDMode = ClientIDMode.Static;
                moreDiv.Attributes.Add("class", "dropdown dropdown-tip dropdown-relative");
                moreDiv.ID = "dropdown-2";
                var moreUl = new HtmlGenericControl("ul");
                moreUl.Attributes.Add("class", "dropdown-menu");
                moreUl.ClientIDMode = ClientIDMode.Static;
                var moreCnkInLi = new HtmlGenericControl("li");
                moreCnkInLi.ClientIDMode = ClientIDMode.Static;
                //* Anchor Tag Start For Check-In *//
                var ahrChkIn = new HtmlGenericControl("a");
                ahrChkIn.Attributes.Add("href", "#");
                //ahrChkIn.Attributes.Add("type", "button");
                ahrChkIn.Attributes.Add("onclick", "fileCheckIn();");
                ahrChkIn.ID = "btnCheckInMore";
                ahrChkIn.ClientIDMode = ClientIDMode.Static;
                ahrChkIn.InnerText = Resources.btnCheckInMore;
                moreCnkInLi.Controls.Add(ahrChkIn);
                moreUl.Controls.Add(moreCnkInLi);
                //* Anchor Tag end For Check-In *//

                var moreDisChkLi = new HtmlGenericControl("li");
                moreDisChkLi.ClientIDMode = ClientIDMode.Static;
                //* Anchor Tag Start For Discard checkout *//
                var ahrDisChkOut = new HtmlGenericControl("a");
                ahrDisChkOut.Attributes.Add("href", "#");
                //ahrDisChkOut.Attributes.Add("type", "button");
                ahrDisChkOut.Attributes.Add("onclick", "fileDisCheckOut();");
                ahrDisChkOut.ID = "btnDiscardCheckOut";
                ahrDisChkOut.ClientIDMode = ClientIDMode.Static;
                ahrDisChkOut.InnerText = Resources.btnDiscardCheckOut;
                moreDisChkLi.Controls.Add(ahrDisChkOut);
                moreUl.Controls.Add(moreDisChkLi);
                //* Anchor Tag end For Discard checkout *//

                var moreChkOutLi = new HtmlGenericControl("li");
                moreChkOutLi.ClientIDMode = ClientIDMode.Static;
                //* Anchor Tag Start For Check Out *//
                var ahrChkOut = new HtmlGenericControl("a");
                ahrChkOut.Attributes.Add("href", "#");
                //ahrChkOut.Attributes.Add("type", "button");
                ahrChkOut.Attributes.Add("onclick", "fileCheckOut();");
                ahrChkOut.ID = "btnCheckOut";
                ahrChkOut.ClientIDMode = ClientIDMode.Static;
                ahrChkOut.InnerText = Resources.btnCheckOut;
                moreChkOutLi.Controls.Add(ahrChkOut);
                moreUl.Controls.Add(moreChkOutLi);
                //* Anchor Tag end For Check Out *//
                var dddivderLi = new HtmlGenericControl("li");
                dddivderLi.ClientIDMode = ClientIDMode.Static;
                dddivderLi.Attributes.Add("class", "dropdown-divider");
                moreUl.Controls.Add(dddivderLi);

                var viewPtyLi = new HtmlGenericControl("li");
                viewPtyLi.ClientIDMode = ClientIDMode.Static;
                //* Anchor Tag Start For View properties *//
                var ahrviewPty = new HtmlGenericControl("a");
                ahrviewPty.Attributes.Add("href", "#");
                //ahrviewPty.Attributes.Add("type", "button");
                ahrviewPty.Attributes.Add("onclick", "fileViewProperties();");
                ahrviewPty.ID = "btnViewProperties";
                ahrviewPty.ClientIDMode = ClientIDMode.Static;
                ahrviewPty.InnerText = Resources.btnViewProperties;
                viewPtyLi.Controls.Add(ahrviewPty);
                moreUl.Controls.Add(viewPtyLi);
                //* Anchor Tag end For View properties *//

                var editPtyLi = new HtmlGenericControl("li");
                editPtyLi.ClientIDMode = ClientIDMode.Static;
                //* Anchor Tag Start For Edit properties *//
                var ahreditPty = new HtmlGenericControl("a");
                ahreditPty.Attributes.Add("href", "#");
                //ahreditPty.Attributes.Add("type", "button");
                ahreditPty.Attributes.Add("onclick", "fileEditProperties();");
                ahreditPty.ID = Resources.btnEditProperties;
                ahreditPty.ClientIDMode = ClientIDMode.Static;
                ahreditPty.InnerText = "Edit properties";
                editPtyLi.Controls.Add(ahreditPty);
                moreUl.Controls.Add(editPtyLi);
                //* Anchor Tag end For Edit properties *//

                var dddivderLi1 = new HtmlGenericControl("li");
                dddivderLi1.ClientIDMode = ClientIDMode.Static;
                dddivderLi1.Attributes.Add("class", "dropdown-divider");
                moreUl.Controls.Add(dddivderLi1);

                var shrFileLi = new HtmlGenericControl("li");
                shrFileLi.ClientIDMode = ClientIDMode.Static;
                //* Anchor Tag Start For Share this File *//
                var ahrshareFile = new HtmlGenericControl("a");
                ahrshareFile.Attributes.Add("href", "#");
                //ahrshareFile.Attributes.Add("type", "button");
                ahrshareFile.Attributes.Add("onclick", "fileShare();");
                ahrshareFile.ID = "btnShare";
                ahrshareFile.ClientIDMode = ClientIDMode.Static;
                ahrshareFile.InnerText = Resources.btnShare;
                shrFileLi.Controls.Add(ahrshareFile);
                moreUl.Controls.Add(shrFileLi);
                //* Anchor Tag end For Share this File *//

                var shareFileli = new HtmlGenericControl("li");
                shareFileli.ClientIDMode = ClientIDMode.Static;
                //* Anchor Tag Start For Share this File *//
                var ahrshareFiletag = new HtmlGenericControl("a");
                ahrshareFiletag.Attributes.Add("href", "#");
                //ahrshareFiletag.Attributes.Add("type", "button");
                ahrshareFiletag.Attributes.Add("onclick", "fileShareWith();");
                ahrshareFiletag.ID = "btnShareWith";
                ahrshareFiletag.ClientIDMode = ClientIDMode.Static;
                ahrshareFiletag.InnerText = Resources.btnShareWith;
                shareFileli.Controls.Add(ahrshareFiletag);
                moreUl.Controls.Add(shareFileli);

                var followFileLi = new HtmlGenericControl("li");
                followFileLi.ClientIDMode = ClientIDMode.Static;
                //* Anchor Tag Start For Share this File *//
                var ahrFollowfiletag = new HtmlGenericControl("a");
                ahrFollowfiletag.Attributes.Add("href", "#");
                //ahrFollowfiletag.Attributes.Add("type", "button");
                ahrFollowfiletag.Attributes.Add("onclick", "fileFollow();");
                ahrFollowfiletag.ID = "btnFollow";
                ahrFollowfiletag.ClientIDMode = ClientIDMode.Static;
                ahrFollowfiletag.InnerText = Resources.btnFollow;
                followFileLi.Controls.Add(ahrFollowfiletag);
                moreUl.Controls.Add(followFileLi);


                var complianceLi = new HtmlGenericControl("li");
                complianceLi.ClientIDMode = ClientIDMode.Static;
                //* Anchor Tag Start For Share this File *//
                var ahrCompliancetag = new HtmlGenericControl("a");
                ahrCompliancetag.Attributes.Add("href", "#");
                //ahrCompliancetag.Attributes.Add("type", "button");
                ahrCompliancetag.Attributes.Add("onclick", "fileComplianceDetails();");
                ahrCompliancetag.ID = "btnCompliance";
                ahrCompliancetag.ClientIDMode = ClientIDMode.Static;
                ahrCompliancetag.InnerText = Resources.btnCompliance;
                complianceLi.Controls.Add(ahrCompliancetag);
                moreUl.Controls.Add(complianceLi);


                var workflowLi = new HtmlGenericControl("li");
                workflowLi.ClientIDMode = ClientIDMode.Static;
                //* Anchor Tag Start For Share this File *//
                var ahrWorkflowtag = new HtmlGenericControl("a");
                ahrWorkflowtag.Attributes.Add("href", "#");
                // ahrWorkflowtag.Attributes.Add("type", "button");
                ahrWorkflowtag.Attributes.Add("onclick", "fileWorkflow();");
                ahrWorkflowtag.ID = "btnWorkflow";
                ahrWorkflowtag.ClientIDMode = ClientIDMode.Static;
                ahrWorkflowtag.InnerText = Resources.btnWorkflow;
                workflowLi.Controls.Add(ahrWorkflowtag);
                moreUl.Controls.Add(workflowLi);


                moreDiv.Controls.Add(moreUl);
                ctxLiMore.Controls.Add(moreDiv);
                ctxUl.Controls.Add(ctxLiMore);
                documentListDiv.Controls.Add(ctxUl);

                #endregion

                #region Hidden Fields

                _hfieldcurrentFolderPath.ClientIDMode = ClientIDMode.Static;
                _hfieldcurrentFolderPath.ID = "currentFolderPath";
                _hfieldcolumnToHidden.ClientIDMode = ClientIDMode.Static;
                _hfieldcolumnToHidden.ID = "columnToHidden";


                _hfieldlistGuidId.ClientIDMode = ClientIDMode.Static;
                _hfieldlistGuidId.ID = "listGuidId";

                _hfieldlistName.ClientIDMode = ClientIDMode.Static;
                _hfieldlistName.ID = "listName";

                _hfieldlistDisplayName.ClientIDMode = ClientIDMode.Static;
                _hfieldlistDisplayName.ID = "listDisplayName";

                _hfieldwebServerUrl.ID = "webServerUrl";
                _hfieldwebServerUrl.ClientIDMode = ClientIDMode.Static;

                _hfieldwebUrl.ClientIDMode = ClientIDMode.Static;
                _hfieldwebUrl.ID = "webUrl";


                _hfieldcolumnToForceCheckout.ClientIDMode = ClientIDMode.Static;
                _hfieldcolumnToForceCheckout.ID = "lstForceCheckout";

                _hfieldcreateperHiddenField.ClientIDMode = ClientIDMode.Static;
                _hfieldcreateperHiddenField.ID = "createpermission";

                _hfieldcolumnNames.ClientIDMode = ClientIDMode.Static;
                _hfieldcolumnNames.ID = "columnNames";

                #endregion

                #region drag drop progress

                var divOverlay = new HtmlGenericControl("div");
                divOverlay.ID = "overlay";
                divOverlay.ClientIDMode = ClientIDMode.Static;
                var squaresWaveG = new HtmlGenericControl("div");
                squaresWaveG.ID = "squaresWaveG";
                squaresWaveG.ClientIDMode = ClientIDMode.Static;
                var squaresWaveG1 = new HtmlGenericControl("div");
                squaresWaveG1.ID = "squaresWaveG_1";
                squaresWaveG1.ClientIDMode = ClientIDMode.Static;
                squaresWaveG1.Attributes["class"] = "squaresWaveG";
                var squaresWaveG2 = new HtmlGenericControl("div");
                squaresWaveG2.ID = "squaresWaveG_2";
                squaresWaveG2.ClientIDMode = ClientIDMode.Static;
                squaresWaveG2.Attributes["class"] = "squaresWaveG";
                var squaresWaveG3 = new HtmlGenericControl("div");
                squaresWaveG3.ID = "squaresWaveG_3";
                squaresWaveG3.ClientIDMode = ClientIDMode.Static;
                squaresWaveG3.Attributes["class"] = "squaresWaveG";
                var squaresWaveG4 = new HtmlGenericControl("div");
                squaresWaveG4.ID = "squaresWaveG_4";
                squaresWaveG4.ClientIDMode = ClientIDMode.Static;
                squaresWaveG4.Attributes["class"] = "squaresWaveG";

                squaresWaveG.Controls.Add(squaresWaveG1);
                squaresWaveG.Controls.Add(squaresWaveG2);
                squaresWaveG.Controls.Add(squaresWaveG3);
                squaresWaveG.Controls.Add(squaresWaveG4);
                divOverlay.Controls.Add(squaresWaveG);


                var documentGrid = new HtmlGenericControl("div");

                documentGrid.Attributes["class"] = "ia-document-library";
                documentGrid.ClientIDMode = ClientIDMode.Static;
                documentGrid.ID = "mainGrid";

                #endregion

                _grid.ID = "ogrid";
                _grid.AutoGenerateColumns = false;
                _grid.ClientIDMode = ClientIDMode.Static;
                _grid.EmptyDataText = Resources.Gird_EmptyDataText;

                _grid.CssClass = "ia-document-library-list tablesaw tablesaw-stack fixed_headers";
                _grid.Attributes["data-mode"] = "stack";
                _grid.RowDataBound += grid_RowDataBound;

                //Initialize the DataSource

                documentListDiv.Controls.Add(divOverlay);
                mainDiv.Controls.Add(_hfieldcolumnToHidden);
                mainDiv.Controls.Add(_hfieldcurrentFolderPath);
                mainDiv.Controls.Add(_hfieldlistGuidId);
                mainDiv.Controls.Add(_hfieldlistName);
                mainDiv.Controls.Add(_hfieldlistDisplayName);
                mainDiv.Controls.Add(_hfieldwebUrl);
                mainDiv.Controls.Add(_hfieldwebServerUrl);
                mainDiv.Controls.Add(_hfieldcolumnToForceCheckout);
                mainDiv.Controls.Add(_hfieldcolumnNames);

                mainDiv.Controls.Add(_hfieldcreateperHiddenField);


                //documentGrid.Controls.Add(_grid);


                var documentPreviewTemplate = new HtmlGenericControl("div");

                documentPreviewTemplate.Attributes["class"] = "docModelTemplate";

                var documentPreviewTemp = "<div id='document-preview-1' class='mfp-hide interAction ia-modal-preview'>" +
                                          "<div class='ia-preview-document'></div><div class='ia-preview-details'><h1 class='ia-doc-title'></h1>" +
                                          "<p class='ia-doc-modified'>Changed by John Smith on 10/29/2014 at 12:45pm</p>" +
                                          "<p class='ia-doc-shared'>Shared with <a href='#'>lots of people</a></p>" +
                                          "<div class='ia-doc-link'>" +
                                          "<button class='ia-button secondary' onclick=\"CopyToCB();\">Copy Link</button>" +
                                          "<input id='ia-Doc-PrevLink' type='text' /></div>" +
                                          "<ul class='ia-button-group'>" +
                                          "<li><a href='#' class='ia-button secondary' onclick='fileOpen();'>Open</a></li>" +
                                          "<li><a href='#' class='ia-button secondary' onclick='fileShare();'>Share</a></li>" +
                                          "<li><a href='#' class='ia-button secondary' onclick='fileFollow();' >Follow</a></li>" +
                                          "</ul></div></div>";
                documentPreviewTemplate.InnerHtml = documentPreviewTemp;

                var noResults = new HtmlGenericControl("div");
                noResults.ID = "noresults";
                noResults.ClientIDMode = ClientIDMode.Static;

                noResults.InnerHtml = Resources.Gird_EmptyDataText;
                documentListDiv.Controls.Add(documentGrid);
                documentListDiv.Controls.Add(documentPreviewTemplate);

                documentListDiv.Controls.Add(noResults);
                mainDiv.Controls.Add(documentListDiv);


                Controls.Add(mainDiv);
            }
            catch (Exception ex)
            {
                Logging.LogError(ex);
            }
        }
        private void loadInstructionSet()
        {
            try
            {
                SetRootResourcePathSandbox();
                if (!string.IsNullOrEmpty(InstructionSet))
                {
                    IInstructionRepository clientServices = new InstructionRepository();
                    var response = clientServices.Execute(InstructionSet);
                    if (response != null && response.Dictionary != null)
                    {
                        if (response.Dictionary.Count > 0)
                        {
                            object listName = string.Empty, headerName = string.Empty, styleLibrary = string.Empty, orderColumns = string.Empty;
                            Dictionary<string, object> values = new Dictionary<string, object>();

                            response.Dictionary.TryGetValue("AkuminaInterActionDefault", out values);
                            if (values != null && values.Count > 0)
                            { values.TryGetValue("RootResourcePath", out styleLibrary); values.TryGetValue("ListName", out listName); }

                            response.Dictionary.TryGetValue("Grid", out values);
                            if (values != null && values.Count > 0)
                            {
                                values.TryGetValue("HeaderName", out headerName);
                                values.TryGetValue("DisplayOrder", out orderColumns);
                            }

                            if (listName != null && !string.IsNullOrEmpty(listName.ToString()))
                                ListName = listName.ToString();
                            ////if (headerName != null && !string.IsNullOrEmpty(headerName.ToString()))
                            ////    HeaderName = headerName.ToString();
                            ////else
                            //    HeaderName = Resources.Grid_Header_ColumnNames;

                            if (styleLibrary != null && !string.IsNullOrEmpty(styleLibrary.ToString()))
                                RootResourcePath = styleLibrary.ToString();
                            //if (orderColumns != null && !string.IsNullOrEmpty(orderColumns.ToString()))
                            //    DisplayOrder = orderColumns.ToString();
                        }
                    }
                }

                //HeaderName = Resources.Grid_Header_ColumnNames;
            }
            catch (Exception ex)
            {
                Logging.LogError(ex);
            }
        }
        private string SetFilterQuery(string currentQuery)
        {
            var filterQuery = string.Empty;
            try
            {
                var filters = GetRefineQuery(currentQuery);
                foreach (var filter in filters)
                {
                    if (filter.Key == "File_x0020_Type")
                    {
                        var tempFile = string.Empty;
                        var fileTypes = new List<string>();
                        foreach (var file in filter.Options)
                        {
                            tempFile = GetFileType.GetValue(file);
                            if (!string.IsNullOrEmpty(tempFile))
                                fileTypes.Add(tempFile);
                            else
                                fileTypes.Add(file.ToLower());
                        }
                        filterQuery += filter.Key + " IN ('" + string.Join("','", fileTypes.ToArray()) + "') AND ";
                    }
                    else if (filter.Key == "Category")
                    {
                        var key = filter.Key;
                        if (_cloneColumns.Contains(filter.Key)) key = filter.Key + "Clone";
                        List<string> filterArr = new List<string>();
                        foreach (string val in filter.Options)
                        {
                            filterArr.Add(key + " LIKE '%" + val + "%'");
                        }
                        filterQuery += "(" + string.Join(" OR ", filterArr.ToArray()) + ") AND ";
                    }
                    else if (filter.Key != "Date")
                    {
                        var key = filter.Key;
                        if (_cloneColumns.Contains(filter.Key)) key = filter.Key + "Clone";
                        filterQuery += key + " IN ('" + string.Join("','", filter.Options.ToArray()) + "') AND ";
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(filter.Options[0]) && !filter.Options[0].Equals("undefined"))
                            filterQuery += "Modified >= #" +
                                           String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(filter.Options[0])) + "# AND ";

                        if (!string.IsNullOrEmpty(filter.Options[1]) && !filter.Options[1].Equals("undefined"))
                            filterQuery += "Modified <= #" +
                                           String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(filter.Options[1])) + "# AND ";
                    }
                }
                if (filterQuery.Length > 0)
                    filterQuery = filterQuery.Remove(filterQuery.Length - 5);
            }
            catch (Exception ex)
            {
                Logging.LogError(ex);
            }

            return filterQuery;
        }



        private void CalcFunction()
        {
            try
            {
                string currFolderPath = HttpContext.Current.Request.QueryString["fd"],
                        currQuery = HttpContext.Current.Request.QueryString["qy"],
                        currTab = HttpContext.Current.Request.QueryString["tab"],
                        searchterm = HttpContext.Current.Request.QueryString["searchterm"],
                        recursive = HttpContext.Current.Request.QueryString["recursive"],
                         sortcolumn = HttpContext.Current.Request.QueryString["sortcolumn"],
                        sortorder = HttpContext.Current.Request.QueryString["sortorder"];
                currFolderPath = !string.IsNullOrEmpty(currFolderPath) ? currFolderPath.Trim() : string.Empty;
                currQuery = !string.IsNullOrEmpty(currQuery) ? currQuery.Trim() : string.Empty;
                currTab = !string.IsNullOrEmpty(currTab) ? currTab.Replace("$", "#") : string.Empty;
                searchterm = !string.IsNullOrEmpty(searchterm) ? searchterm.Trim() : string.Empty;
                recursive = !string.IsNullOrEmpty(recursive) ? recursive.Trim() : string.Empty;
                sortcolumn = !string.IsNullOrEmpty(sortcolumn) ? sortcolumn.Trim() : string.Empty;
                sortorder = !string.IsNullOrEmpty(sortorder) ? sortorder.Trim() : string.Empty;

                BindData(currFolderPath, currTab, currQuery, searchterm, recursive, sortcolumn, sortorder);
                _hfieldcolumnNames.Value = Resources.Grid_Header_ColumnNames;
            }
            catch (Exception ex)
            {
                Logging.LogError(ex);
            }
        }

        private List<Filter> GetRefineQuery(string currQuery)
        {
            var querys = new List<Filter>();
            try
            {
                if (!string.IsNullOrEmpty(currQuery))
                {
                    var filters = currQuery.Split('$');
                    string[] filterVal;
                    foreach (var filter in filters)
                    {
                        filterVal = filter.Split('@');
                        querys.Add(new Filter { Key = filterVal[0], Options = filterVal[1].Split('|').ToList() });
                    }
                }
            }
            catch (Exception ex)
            {
                Logging.LogError(ex);
            }
            return querys;
        }

        private void Page_Load(object sender, EventArgs e)
        {
            try
            {
                var upload = HttpContext.Current.Request.QueryString["upload"];
                var oSpWeb = SPContext.Current.Web;
                var docLibrary = oSpWeb.Lists.TryGetList(ListName);
                if (docLibrary != null && docLibrary.BaseType == SPBaseType.DocumentLibrary && string.IsNullOrEmpty(upload))
                    CalcFunction();
            }
            catch (Exception ex)
            {
                Logging.LogError(ex);
            }
        }

        private string BindScript(string scriptUrl, bool pickFromSiteCollection)
        {
            //if (pickFromSiteCollection)
            //    scriptUrl = SPUrlUtility.CombineUrl(SPContext.Current.Site.RootWeb.Url, scriptUrl);
            //else
            //    scriptUrl = SPUrlUtility.CombineUrl(SPContext.Current.Web.Url, scriptUrl);

            return string.Format(@"<script type=""text/javascript"" src=""{0}""></script>", scriptUrl);
        }

        private string BindStyle(string styleUrl, bool pickFromSiteCollection)
        {
            //if (pickFromSiteCollection)
            //    styleUrl = SPUrlUtility.CombineUrl(SPContext.Current.Site.RootWeb.Url, styleUrl);
            //else
            //    styleUrl = SPUrlUtility.CombineUrl(SPContext.Current.Web.Url, styleUrl);

            return string.Format(@"<link rel=""stylesheet"" href=""{0}"" type=""text/css"" />", styleUrl);
        }



        private void AssignTemplates(DataTable gTable)
        {
            try
            {
                foreach (DataColumn col in gTable.Columns)
                {
                    var tf1 = new TemplateField();
                    if (col.ColumnName == "Column1")
                    {
                        //Initalize the DataField value.                   
                        tf1.HeaderTemplate = new GridViewTemplate(ListItemType.Header, col.ColumnName);
                        tf1.HeaderStyle.CssClass = "ia-doclib-header-checkbox";
                        //Initialize the HeaderText field value.
                        tf1.ItemTemplate = new GridViewTemplate(ListItemType.Item, col.ColumnName);
                        tf1.ItemStyle.CssClass = "ia-doclib-checkbox";
                        _grid.Columns.Add(tf1);
                    }
                    else if (col.ColumnName == "LinkFilename")
                    {
                        //Initalize the DataField value.
                        tf1.HeaderTemplate = new GridViewTemplate(ListItemType.Header, col.ColumnName);
                        tf1.HeaderStyle.CssClass = "ia-doclib-header-name ia-doclib-header-sorted";
                        //Initialize the HeaderText field value.
                        tf1.ItemTemplate = new GridViewTemplate(ListItemType.Item, col.ColumnName);
                        tf1.ItemStyle.CssClass = "ia-doclist-name-icon";

                        _grid.Columns.Add(tf1);
                    }
                    else if (col.ColumnName == "Modified")
                    {
                        //Initalize the DataField value.
                        tf1.HeaderTemplate = new GridViewTemplate(ListItemType.Header, col.ColumnName);
                        tf1.HeaderStyle.CssClass = "ia-doclib-header-modified";


                        //Initialize the HeaderText field value.
                        tf1.ItemTemplate = new GridViewTemplate(ListItemType.Item, col.ColumnName);
                        tf1.ItemStyle.CssClass = "ia-doc-list-modified";
                        _grid.Columns.Add(tf1);
                    }
                    else if (col.ColumnName == "Editor")
                    {
                        //Initalize the DataField value.
                        tf1.HeaderTemplate = new GridViewTemplate(ListItemType.Header, col.ColumnName);
                        tf1.HeaderStyle.CssClass = "ia-doclib-header-modifiedBy";

                        //Initialize the HeaderText field value.
                        tf1.ItemTemplate = new GridViewTemplate(ListItemType.Item, col.ColumnName);
                        tf1.ItemStyle.CssClass = "ia-doc-list-modifiedBy";
                        _grid.Columns.Add(tf1);
                    }
                    else if (col.ColumnName == "ID")
                    {
                        //Initalize the DataField value.
                        tf1.HeaderTemplate = new GridViewTemplate(ListItemType.Header, col.ColumnName);
                        tf1.ItemStyle.CssClass = "itemhidden";
                        tf1.HeaderStyle.CssClass = "itemhidden";

                        //Initialize the HeaderText field value.
                        tf1.ItemTemplate = new GridViewTemplate(ListItemType.Item, col.ColumnName);
                        _grid.Columns.Add(tf1);
                    }
                    else if (col.ColumnName == "File_x0020_Type")
                    {
                        //Initalize the DataField value.
                        tf1.HeaderTemplate = new GridViewTemplate(ListItemType.Header, col.ColumnName);
                        tf1.ItemStyle.CssClass = "itemhiddenFileType";
                        tf1.HeaderStyle.CssClass = "itemhiddenFileType";

                        //Initialize the HeaderText field value.
                        tf1.ItemTemplate = new GridViewTemplate(ListItemType.Item, col.ColumnName);
                        _grid.Columns.Add(tf1);
                    }
                    else if (col.ColumnName == "Path")
                    {
                        //Initalize the DataField value.
                        tf1.HeaderTemplate = new GridViewTemplate(ListItemType.Header, col.ColumnName);
                        tf1.ItemStyle.CssClass = "itemhiddenPath";
                        tf1.HeaderStyle.CssClass = "itemhiddenPath";

                        //Initialize the HeaderText field value.
                        tf1.ItemTemplate = new GridViewTemplate(ListItemType.Item, col.ColumnName);
                        _grid.Columns.Add(tf1);
                    }

                    else if (col.ColumnName == "ModifiedTime")
                    {
                        //Initalize the DataField value.
                        tf1.HeaderTemplate = new GridViewTemplate(ListItemType.Header, col.ColumnName);
                        tf1.ItemStyle.CssClass = "itemhiddendate";
                        tf1.HeaderStyle.CssClass = "itemhiddendate";

                        //Initialize the HeaderText field value.
                        tf1.ItemTemplate = new GridViewTemplate(ListItemType.Item, col.ColumnName);
                        _grid.Columns.Add(tf1);
                    }
                    else if (col.ColumnName == "EditPermission")
                    {
                        //Initalize the DataField value.
                        tf1.HeaderTemplate = new GridViewTemplate(ListItemType.Header, col.ColumnName);
                        tf1.ItemStyle.CssClass = "itemEditPermission";
                        tf1.HeaderStyle.CssClass = "itemEditPermission";

                        //Initialize the HeaderText field value.
                        tf1.ItemTemplate = new GridViewTemplate(ListItemType.Item, col.ColumnName);
                        _grid.Columns.Add(tf1);
                    }
                    //Add the newly created bound field to the GridView.
                }
            }
            catch (Exception ex)
            {
                Logging.LogError(ex);
            }
        }

        public void BindData(string folderPath, string currTab, string currentQuery, string searchterm, string recursive, string sortcolumn, string sortorder)
        {
            try
            {
                //var viewFields =
                //        "<FieldRef Name='ID' /><FieldRef Name='File_x0020_Type' /><FieldRef Name='LinkFilename' /><FieldRef Name='Modified' /><FieldRef Name='Editor' />";
                //var dxTable = new DataTable();
                var currentWeb = SPContext.Current.Web;
                var docLib = (SPDocumentLibrary)currentWeb.Lists[ListName];
                _hfieldcolumnToForceCheckout.Value = docLib.ForceCheckout.ToString().ToLower();
                _hfieldlistName.Value = docLib.RootFolder.Name;
                _hfieldlistDisplayName.Value = docLib.Title;
                _hfieldlistGuidId.Value = docLib.ID.ToString();
                _hfieldwebUrl.Value = currentWeb.Url;
                _hfieldwebServerUrl.Value = currentWeb.ServerRelativeUrl.Equals("/") ? "" : currentWeb.ServerRelativeUrl;
                _hfieldcreateperHiddenField.Value = docLib.DoesUserHavePermissions(SPBasePermissions.EditListItems) ? "yes" : "no";

                //var sQuery = new SPQuery();
                //var dt = new DataTable();
                //var dataView = new DataView();
                //if (sortcolumn == "ia-doclib-header-name")
                //    sortcolumn = "LinkFilename";
                //else if (sortcolumn == "ia-doclib-header-modified")
                //    sortcolumn = "Modified";
                //else if (sortcolumn == "ia-doclib-header-modifiedBy")
                //    sortcolumn = "Editor";
                //if (string.IsNullOrEmpty(sortcolumn))
                //    sQuery.Query = "<OrderBy><FieldRef Name='LinkFilename' Ascending='True'  /></OrderBy>";
                //else
                //    sQuery.Query = "<OrderBy><FieldRef Name='" + sortcolumn + "' Ascending='" + sortorder + "'  /></OrderBy>";
                //if (MetaField != null && docLib.Fields.ContainsField(MetaField))
                //    viewFields += "<FieldRef Name='" + MetaField + "' />";
                //sQuery.ViewFields = viewFields;
                //sQuery.ViewFieldsOnly = true;
                //var myColl = docLib.GetItems(sQuery);
                //var gTable = new GridDataTable();
                //gTable.Stylelib = RootResourcePath;
                //if (!string.IsNullOrEmpty(folderPath))
                //{
                //    var folder = currentWeb.GetFolder(folderPath);
                //    if (folder != null)
                //    {
                //        sQuery.Folder = folder;
                //        gTable.ListPermission = docLib.DoesUserHavePermissions(SPBasePermissions.EditListItems);
                //        //gTable.ListPermission = folder.EffectiveRawPermissions.HasFlag(SPBasePermissions.EditListItems);
                //    }
                //    else
                //        gTable.ListPermission = docLib.DoesUserHavePermissions(SPBasePermissions.EditListItems);
                //}
                //else
                //    gTable.ListPermission = docLib.DoesUserHavePermissions(SPBasePermissions.EditListItems);
                //_hfieldcreateperHiddenField.Value = gTable.ListPermission ? "yes" : "no";
                //if (!string.IsNullOrEmpty(recursive))
                //    sQuery.ViewAttributes = "Scope='Recursive'";




                //dt = gTable.BindSpListItemstoTable(myColl);

                //#region searchterm

                //if (!string.IsNullOrEmpty(searchterm))
                //{
                //    dataView = dt.AsDataView();
                //    dataView.RowFilter = "LinkFilename like '%" + searchterm + "%'";

                //    dt = dataView.ToTable();
                //}

                //#endregion

                //#region Codes

                //if (currTab != "All Files")
                //{
                //    if (currTab.Contains("|"))
                //    {
                //        dataView = dt.AsDataView();
                //        dataView.RowFilter = currTab.Split('|')[0];
                //        var tempdt = dataView.ToTable();
                //        var indexAc = tempdt.Columns.Add("Order", typeof(Int32)).Ordinal;
                //        var indexId = tempdt.Columns["ID"].Ordinal;
                //        var ids = currTab.Split('|')[1].Split(',');
                //        for (var i = 0; i < tempdt.Rows.Count; i++)
                //            tempdt.Rows[i][indexAc] = Array.IndexOf(ids, tempdt.Rows[i][indexId]);
                //        dataView = tempdt.AsDataView();
                //        dataView.Sort = "Order ASC";
                //        dt = dataView.ToTable();
                //    }
                //    else
                //    {
                //        dataView = dt.AsDataView();
                //        dataView.RowFilter = currTab;
                //        dt = dataView.ToTable();
                //    }
                //}

                //#endregion

                //#region RefineQueyFilter

                //if (!string.IsNullOrEmpty(currentQuery))
                //{
                //    dataView = dt.AsDataView();
                //    dataView.RowFilter = SetFilterQuery(currentQuery);
                //    var reset = HttpContext.Current.Request.QueryString["qyreset"];
                //    if (reset != null && reset == "true" && !currentQuery.Contains("$"))
                //    {
                //        if (dataView.Count > 0)
                //            dt = dataView.ToTable();
                //    }
                //    else
                //        dt = dataView.ToTable();
                //}

                //#endregion

                //#region OrderChange

                ////if (!string.IsNullOrEmpty(DisplayOrder))
                ////{
                ////    try
                ////    {
                ////        var colList = gTable._columns.ToList();
                ////        colList.Add(MetaField);
                ////        var defaultColumns = colList.ToArray();
                ////        int[] columnsBind = Array.ConvertAll(DisplayOrder.Split(','), int.Parse).Distinct().ToArray();
                ////        int j = 1;
                ////        foreach (int order in columnsBind)
                ////        {
                ////            if (order < 4)
                ////            {
                ////                j = j + 1;
                ////                dt.Columns[defaultColumns[order + 1]].SetOrdinal(j);
                ////            }
                ////        }
                ////        _hfieldcolumnToHidden.Value = (3 - (j - 1)).ToString();
                ////    }
                ////    catch (Exception ex) { }
                ////}
                ////else
                ////    _hfieldcolumnToHidden.Value = "0";
                //#endregion

                //#region BindToGrid

                //_grid.Columns.Clear();
                //AssignTemplates(dt);

                //if (!string.IsNullOrEmpty(sortcolumn) && sortorder == "False")
                //{
                //    DataTable fileTable = new DataTable();
                //    var files = new DataView(dt);
                //    files.RowFilter = String.Format("NOT File_x0020_Type='' AND NOT File_x0020_Type='one'");
                //    if (files.Count > 0)
                //        fileTable = files.ToTable();
                //    var folders = new DataView(dt);
                //    folders.RowFilter = String.Format("File_x0020_Type='' OR File_x0020_Type='one'");
                //    if (folders.Count > 0)
                //    {
                //        if (fileTable.Rows.Count > 0)
                //            dt = files.ToTable().AsEnumerable()
                //                        .Union(folders.ToTable().Rows.Cast<DataRow>())
                //                        .CopyToDataTable();
                //        else
                //            dt = folders.ToTable();

                //    }
                //    else
                //        dt = fileTable;
                //}

                //_grid.DataSource = dt;
                //_grid.DataBind();
                //if (_grid.HeaderRow != null)
                //{
                //    _grid.HeaderRow.TableSection = TableRowSection.TableHeader;

                //    _grid.HeaderRow.CssClass = "sticky";
                //}
        #endregion
            }
            catch (Exception ex)
            {
                Logging.LogError(ex);
            }
        }

        public DataTable ReorderTable(DataTable table, string[] columns)
        {
            try
            {
                if (columns.Length != table.Columns.Count)
                    throw new ArgumentException("Count of columns must be equal to table.Column.Count", "columns");

                for (int i = 0; i < columns.Length; i++)
                {
                    table.Columns[columns[i]].SetOrdinal(i);
                }
            }
            catch (Exception ex)
            {
                Logging.LogError(ex);
            }
            return table;
        }

        public void BindViewState(string key, string value)
        {
            _hfieldcurrentFolderPath.Value = value;
        }

        //#endregion

        #region Aditional Classes

        private class GridDataTable
        {
            private readonly DataTable _dTable = new DataTable();

            public readonly string[] _columns =
            {
                "Column1", "TypeUrl", "LinkFilename", "Modified", "Editor", "ID",
                "File_x0020_Type", "Path", "Status", "EditorClone", "ModifiedTime","EditPermission"
            };

            public string Stylelib;
            public bool ListPermission = false;
            private string weburl = SPContext.Current.Web.Url;


            /// <summary>
            ///     Assign the table Column fields
            /// </summary>
            public GridDataTable()
            {
                foreach (var columnName in _columns)
                {
                    _dTable.Columns.Add(columnName, typeof(string));
                }
                if (MetaField != null) _dTable.Columns.Add(MetaField);
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
                    var collDataTable = spItemsCollection.GetDataTable();
                    var metaData = new Dictionary<string, string>();
                    if (collDataTable == null)
                        return _dTable;
                    if (MetaField != null && collDataTable.Columns.Contains(MetaField))
                    {
                        string value = string.Empty;
                        foreach (DataRow row in collDataTable.Rows)
                        {
                            value = string.Empty;
                            if (row[MetaField] != null)
                            {
                                value = row[MetaField].ToString();
                                if (value.Contains("#"))
                                    value = String.Join(",", row[MetaField].ToString().Split('#').Where((val, index) => index % 2 == 1).Select(s => s.Trim(';')).ToArray());

                            }
                            metaData.Add(row["ID"].ToString(), value);

                            //metaData.Add(row["ID"].ToString(),
                            //    (row[MetaField] != null && !string.IsNullOrEmpty(row[MetaField].ToString()))
                            //        ? row[MetaField].ToString()
                            //        : string.Empty);
                            //metaData.Add(row["ID"].ToString(),
                            //    (row[MetaField] != null && !string.IsNullOrEmpty(row[MetaField].ToString())) 
                            //        ?  row[MetaField].ToString().Split('#')[1]
                            //        : string.Empty);
                        }
                    }

                    var metaDataValue = string.Empty;
                    var iconUrl = string.Empty;
                    string filetype = string.Empty;

                    foreach (SPListItem item in spItemsCollection)
                    {
                        try
                        {

                            var notebook = "OneNote.Notebook";
                            if (item.Folder.ProgID != notebook)
                            {
                                iconUrl = GetIconUrl(Path.GetExtension(item[_dTable.Columns[2].ColumnName].ToString()));
                                filetype = item[_dTable.Columns[6].ColumnName] != null
                                    ? item[_dTable.Columns[6].ColumnName].ToString()
                                    : string.Empty;
                            }
                            else
                            {
                                iconUrl = GetIconUrl(".one");
                                filetype = "one";
                            }
                        }
                        catch
                        {
                            iconUrl = GetIconUrl(Path.GetExtension(item[_dTable.Columns[2].ColumnName].ToString()));
                            filetype = item[_dTable.Columns[6].ColumnName] != null
                                ? item[_dTable.Columns[6].ColumnName].ToString()
                                : string.Empty;
                        }
                        _dTable.Rows.Add("",
                            !string.IsNullOrEmpty(iconUrl)
                                ? iconUrl
                                : GetIconUrl(Path.GetExtension(item[_dTable.Columns[2].ColumnName].ToString())),
                            Path.GetFileNameWithoutExtension(item[_dTable.Columns[2].ColumnName].ToString()),
                            String.Format("{0:MM/dd/yyyy}",
                                Convert.ToDateTime(item[_dTable.Columns[3].ColumnName].ToString())),
                            GetUserDetail(item.Web, item[_dTable.Columns[4].ColumnName].ToString()),
                            item[_dTable.Columns[5].ColumnName],
                            filetype,
                            item.File != null ? item.File.Url : (item.Folder != null ? item.Folder.Url : ""),
                            item.File != null
                                ? ((item.File.CheckOutType.ToString().ToLower() == "online")
                                    ? "<span class='ia-doc-checkedOut' title='Checked Out'></span>"
                                    : string.Empty)
                                : string.Empty,
                            item[_dTable.Columns[4].ColumnName].ToString().Split('#')[1],
                            String.Format("{0:MM/dd/yyyy 'at' hh:mmtt}",
                                Convert.ToDateTime(item[_dTable.Columns[3].ColumnName].ToString())).ToLower(),
    !ListPermission ? "no:no" : (item.DoesUserHavePermissions(SPBasePermissions.EditListItems) && (item.Level.ToString().ToLower() == "checkout" || item.DoesUserHavePermissions(SPBasePermissions.CancelCheckout))) ? "yes:yes" : item.DoesUserHavePermissions(SPBasePermissions.EditListItems) ? "yes:no" : "no:no",

                            metaData.TryGetValue(item.ID.ToString(), out metaDataValue) ? metaDataValue : string.Empty
                            );

                    }
                }
                catch (Exception ex)
                {
                    Logging.LogError(ex);
                }
                return _dTable;
            }

            private string GetUserDetail(SPWeb web, string userName)
            {
                var genLink = "<a class='userlink' href='/_layouts/userdisp.aspx?ID={0}' target='_blank'>{1}</a>";
                try
                {
                    var user = (from SPUser c in web.AllUsers
                                where c.Name == userName.Split('#')[1]
                                select c).FirstOrDefault();
                    genLink = string.Format(genLink, user.ID, user.Name);
                }
                catch (Exception ex)
                {
                    Logging.LogError(ex);
                }
                return genLink;
            }

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
                            docLibName = weburl + "/" + Stylelib;
                    }
                    var iconVal = !String.IsNullOrEmpty(icon) ? icon.Substring(1).ToLower() : string.Empty;
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

        #endregion
        }
    }
}