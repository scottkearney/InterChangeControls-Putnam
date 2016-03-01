#region

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Akumina.InterAction;
using Akumina.WebParts.Documents.DocumentTab;
using Akumina.WebParts.DocumentsSandbox.Properties;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;

#endregion

namespace Akumina.WebParts.DocumentsSandbox.DocumentTab
{
    [ToolboxItem(false)]
    public class DocumentTab : DocumentTabBaseWebPart
    {
        private readonly HtmlGenericControl _allFiles = new HtmlGenericControl("a");
        private readonly HiddenField _lblId = new HiddenField();
        private readonly HtmlGenericControl _myFiles = new HtmlGenericControl("a");
        private readonly HtmlGenericControl _recentFiles = new HtmlGenericControl("a");
        private readonly HiddenField _tabStr = new HiddenField();
        private DataTable _odtPopular;
        private string _strFolderName = string.Empty;
        public IGetCurrentPath GetCurrentPath;

        private string BindScript(string scriptUrl, bool pickFromSiteCollection)
        {
            if (pickFromSiteCollection)
                scriptUrl = SPUrlUtility.CombineUrl(SPContext.Current.Site.RootWeb.Url, scriptUrl);
            else
                scriptUrl = SPUrlUtility.CombineUrl(SPContext.Current.Web.Url, scriptUrl);

            return string.Format(@"<script type=""text/javascript"" src=""{0}""></script>", scriptUrl);
        }

        protected override void CreateChildControls()
        {
            try
            {
                var reset = HttpContext.Current.Request.QueryString["tabreset"];
                if (string.IsNullOrEmpty(reset) || reset == "true")
                {
                    BindChildControls();
                }
            }
            catch (Exception ex)
            {
                Logging.LogError(ex);
            }
        }

        protected void BindChildControls()
        {
            try
            {
                var controlDiv = new HtmlGenericControl("DIV");
                controlDiv.Attributes["class"] = "interAction";


                _lblId.ClientIDMode = ClientIDMode.Static;
                _lblId.ID = "lblID";

                _tabStr.ClientIDMode = ClientIDMode.Static;
                _tabStr.ID = "tabStr";

                var mainDiv = new HtmlGenericControl("DIV");
                mainDiv.Attributes["class"] = "ia-tabs";

                var ul = new HtmlGenericControl("ul");
                ul.ClientIDMode = ClientIDMode.Static;
                ul.Attributes["class"] = "ia-tabs-nav";


                // *All Files li* //
                var liallfiles = new HtmlGenericControl("li");

                //Linkbutton
                _allFiles.ID = "allFiles";
                _allFiles.Attributes["onclick"] = "tabHighlight('0',this);";
                _allFiles.Attributes["href"] = "#";
                liallfiles.Controls.Add(_allFiles);
                ul.Controls.Add(liallfiles);

                // *My Files li* //
                var limyfiles = new HtmlGenericControl("li");
                _myFiles.ID = "myFiles";
                _myFiles.Attributes["class"] = "myFiles";
                _myFiles.Attributes["onclick"] = "tabHighlight('1',this);";
                _myFiles.Attributes["href"] = "#";
                limyfiles.Controls.Add(_myFiles);
                ul.Controls.Add(limyfiles);

                // *Recent Files li* //
                var lirecentfiles = new HtmlGenericControl("li");

                _recentFiles.ID = "recentFiles";
                _recentFiles.ClientIDMode = ClientIDMode.Static;
                _recentFiles.Attributes["onclick"] = "tabHighlight('2',this);";
                _recentFiles.Attributes["href"] = "#";
                lirecentfiles.Controls.Add(_recentFiles);
                ul.Controls.Add(lirecentfiles);

                mainDiv.Controls.Add(ul);

                controlDiv.Controls.Add(_lblId);
                controlDiv.Controls.Add(_tabStr);
                controlDiv.Controls.Add(mainDiv);

                Page.Load += Page_Load;
                Controls.Add(controlDiv);
            }
            catch (Exception ex)
            {
                Logging.LogError(ex);
            }
        }

        private void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                SetRootResourcePathSandbox();
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(InstructionSet))
                {
                    IInstructionRepository clientServices = new InstructionRepository();
                    var response = clientServices.Execute(InstructionSet);
                    if (response != null && response.Dictionary != null)
                    {
                        if (response.Dictionary.Count > 0)
                        {
                            Dictionary<string, object> values = new Dictionary<string, object>();
                            object listName = string.Empty, recentFiles = string.Empty;
                            response.Dictionary.TryGetValue("AkuminaInterActionDefault", out values);
                            if (values != null && values.Count > 0)
                                values.TryGetValue("ListName", out listName);
                            response.Dictionary.TryGetValue("Tab", out values);
                            if (values != null && values.Count > 0)
                                values.TryGetValue("NumberOfRecentFiles", out recentFiles);


                            if (listName != null && !string.IsNullOrEmpty(listName.ToString()))
                                ListName = listName.ToString();
                            if (recentFiles != null && !string.IsNullOrEmpty(recentFiles.ToString()))
                                NoOfRecentFiles = recentFiles.ToString();
                        }
                    }
                }

                if (ListName != null)
                {
                    try
                    {
                        string currFolderPath = HttpContext.Current.Request.QueryString["fd"],
                            searchterm = HttpContext.Current.Request.QueryString["searchterm"],
                            recursive = HttpContext.Current.Request.QueryString["recursive"];
                        currFolderPath = !string.IsNullOrEmpty(currFolderPath) ? currFolderPath.Trim() : string.Empty;
                        searchterm = !string.IsNullOrEmpty(searchterm) ? searchterm.Trim() : string.Empty;
                        recursive = !string.IsNullOrEmpty(recursive) ? recursive.Trim() : string.Empty;
                        //GetMenuDatasAll(currFolderPath, searchterm, recursive);
                        var username = SPContext.Current.Web.CurrentUser.Name;
                        _allFiles.InnerHtml = String.Format("{0} <span class='ia-tab-count allfilecount'>{1}</span>", Resources.Tab_AllFiles_Text,
                           "0");
                        _myFiles.Attributes["user"] = username;
                        _myFiles.InnerHtml = String.Format("{0} <span class='ia-tab-count myfilecount'>{1}</span>", Resources.Tab_MyFiles_Text, "0");
                        _recentFiles.InnerHtml = String.Format("{0}", Resources.Tab_RecentFiles_Text);
                        _recentFiles.Attributes["file-count"] = NoOfRecentFiles != null ? NoOfRecentFiles.ToString() : "0";
                    }
                    catch (Exception ex)
                    {
                        _allFiles.InnerText += ex.Message;
                    }
                }
            }
            catch (Exception ex)
            {
                Logging.LogError(ex);
            }
        }

        private void GetMenuDatasAll(string currFolderPath, string searchterm, string recursive)
        {
            try
            {
                var username = SPContext.Current.Web.CurrentUser.Name;
                DataTable odtCollection = null;
                var currentWeb = SPContext.Current.Web;

                var docLibrary = currentWeb.Lists[ListName];
                SPDocumentLibrary docLib = null;
                if (docLibrary != null && docLibrary.BaseType.ToString() == "DocumentLibrary")
                {
                    docLib = (SPDocumentLibrary)currentWeb.Lists[ListName];
                    var sQuery = new SPQuery();
                    sQuery.Query = "<OrderBy><FieldRef Name='Modified' Ascending='False'  /></OrderBy>";
                    if (!string.IsNullOrEmpty(recursive))
                        sQuery.ViewAttributes = "Scope='Recursive'";
                    sQuery.ViewFields =
                        "<FieldRef Name='ID' /><FieldRef Name='LinkFilename' /><FieldRef Name='Modified' /><FieldRef Name='Editor' /><FieldRef Name='File_x0020_Type' />";
                    sQuery.ViewFieldsOnly = true;
                    _strFolderName = currFolderPath;
                    if (!string.IsNullOrEmpty(_strFolderName))
                    {
                        var oListItem = currentWeb.GetFolder(_strFolderName);
                        if (oListItem != null)
                        {
                            sQuery.Folder = oListItem;
                        }
                    }
                    var oListItemColl = docLib.GetItems(sQuery);

                    if (oListItemColl != null)
                        odtCollection = oListItemColl.GetDataTable();

                    #region searchterm

                    if (odtCollection != null)
                    {
                        //if (!string.IsNullOrEmpty(searchterm))
                        //{
                        //    var filterView = new DataView(odtCollection);
                        //    filterView.RowFilter = "LinkFilename like '%" + searchterm + "%'";
                        //    odtCollection = filterView.ToTable();
                        //}

                        EnumerableRowCollection<DataRow> query = from order in odtCollection.AsEnumerable()
                                                                 where string.IsNullOrEmpty(order.Field<string>("File_x0020_Type"))
                                                                 select order;

                        var notebook = "OneNote.Notebook";
                        SPListItem item = null;
                        foreach (DataRow dr in query) // search whole table
                        {
                            DataRow drOne = odtCollection.Select("Id=" + dr["Id"].ToString()).FirstOrDefault();
                            item = docLib.GetItemById(Convert.ToInt32(dr["Id"]));
                            if (item.Folder.ProgID != null && item.Folder.ProgID == notebook)
                                drOne["File_x0020_Type"] = "one"; //change the file type
                        }
                    }

                    #endregion

                    var filterView1 = new DataView(odtCollection);
                    var days = 0;
                    filterView1.RowFilter = String.Format("NOT File_x0020_Type=''");
                    _allFiles.InnerHtml = String.Format("{0} <span class='ia-tab-count'>{1}</span>", Resources.Tab_AllFiles_Text,
                        filterView1.Count);
                    _allFiles.Attributes["tab-val"] = "All Files";
                    _myFiles.Attributes["tab-val"] = String.Format("Editor LIKE '%{0}%' AND NOT File_x0020_Type=''",
                        username);
                    if (odtCollection != null)
                    {
                        var filterView = new DataView(odtCollection);
                        string filter = string.Empty;
                        if (!string.IsNullOrEmpty(searchterm))
                            filter += "LinkFilename like '%" + searchterm + "%' AND ";
                        filter += String.Format("Editor LIKE '%{0}%'  AND NOT File_x0020_Type=''", username);
                        filterView.RowFilter = filter;
                        _myFiles.InnerHtml = String.Format("{0} <span class='ia-tab-count'>{1}</span>", Resources.Tab_MyFiles_Text,
                            filterView != null ? filterView.Count.ToString() : "0");
                    }
                    else
                        _myFiles.InnerHtml = String.Format("{0} <span class='ia-tab-count'>{1}</span>", Resources.Tab_MyFiles_Text, "0");
                    if (odtCollection != null)
                    {
                        _recentFiles.InnerHtml = String.Format("{0}", Resources.Tab_RecentFiles_Text);
                        var filterView = new DataView(odtCollection);
                        filterView.RowFilter = "NOT File_x0020_Type=''";
                        filterView.Sort = "Modified DESC";
                        var ltrlRecentFile = new Literal();
                        ltrlRecentFile.Text = String.Format("{0} ", Resources.Tab_RecentFiles_Text);
                        var lstResult = (from table in filterView.ToTable().AsEnumerable()
                                         select table.Field<int>("ID").ToString()).ToArray();

                        if (lstResult.Count() > 0)
                        {
                            if (!string.IsNullOrEmpty(NoOfRecentFiles) && int.TryParse(NoOfRecentFiles, out days))
                                lstResult = lstResult.Take(Convert.ToInt32(days)).ToArray();
                            //else if (!string.IsNullOrEmpty(TabNumOfRecentFiles) &&

                            //         int.TryParse(TabNumOfRecentFiles, out days))
                            lstResult = lstResult.Take(Convert.ToInt32(days)).ToArray();
                            _recentFiles.Attributes["tab-val"] = "ID IN (" + string.Join(",", lstResult) + ")|" +
                                                                 string.Join(",", lstResult);
                        }
                        else
                            _recentFiles.Attributes["tab-val"] = string.Empty;

                        //var dateAndTimeRecent = DateTime.Now.AddDays(-Convert.ToInt32(NumOfDays));
                        //var recentFileFilter = String.Format("Modified >= #{0:MM/dd/yyyy}# AND NOT File_x0020_Type=''",
                        //    dateAndTimeRecent);
                        //filterView.RowFilter = recentFileFilter;
                        //recentFiles.InnerHtml = String.Format("{0}", "Recent");
                        //recentFiles.Attributes["tab-val"] = recentFileFilter;
                    }
                    else
                        _recentFiles.InnerHtml = String.Format("{0}", Resources.Tab_RecentFiles_Text);
                }
            }
            catch (Exception ex)
            {
                _allFiles.InnerText += ex.Message;
            }
        }

        protected DataTable DataTableInitiate()
        {
            _odtPopular = new DataTable();
            var col = _odtPopular.Columns.Add("ID", typeof(string));
            col.AutoIncrement = true;
            col.AutoIncrementStep = 1;
            col.AutoIncrementSeed = 1;
            _odtPopular.Columns.Add("AuditCount", typeof(Int32));
            _odtPopular.Columns.Add("Modified", typeof(DateTime));
            return _odtPopular;
        }

        protected void tab_Click(object sender, EventArgs e)
        {
            var linkbtn = (LinkButton)(sender);

            _tabStr.Value = linkbtn.CommandArgument;
        }

        #region Web Part Properties

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
    }
}