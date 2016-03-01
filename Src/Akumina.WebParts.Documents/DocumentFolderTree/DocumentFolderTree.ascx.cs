using System;
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Akumina.InterAction;
using Akumina.WebParts.Documents.Properties;
using Microsoft.SharePoint;

namespace Akumina.WebParts.Documents.DocumentFolderTree
{
    [ToolboxItem(false)]
    public partial class DocumentFolderTree : DocumentFolderTreeBaseWebPart, ICurrentPath
    {

        #region Fields

        private string _libraryPath = "Akumina";
        private string _path = string.Empty;

        #endregion

        /// <summary>
        /// What are these editor fields even doing?
        /// We should be using the DocumentFolderTreeBaseWebPart to create fields in Edit Pane
        /// </summary>
        #region Editor Part Collection

        //This isn't being displayed in edit pane
        public string LibraryPath
        {
            get { return _libraryPath; }
            set { _libraryPath = value; }
        }

        [Personalizable(PersonalizationScope.Shared), Browsable(false), Category("Document Library Selection")]
        public string CurrentPath
        {
            get { return _path; }
            set { _path = value; }
        }

        [ConnectionProvider("Provide the path From TextBox", "PathProvider")]
        public ICurrentPath PathProvider()
        {
            return this;
        }

        #endregion

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
        }

        protected override void OnPreRender(EventArgs e)
        {
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack && (!Page.IsPostBack || refreshIdleFT.Value == "true")) return;
            try
            {

                if (!string.IsNullOrEmpty(InstructionSet))
                {
                    IInstructionRepository clientServices = new InstructionRepository();
                    var response = clientServices.Execute(InstructionSet);
                    if (response != null && response.Dictionary != null)
                    {
                        ListName = clientServices.GetValue(response.Dictionary, Resources.colName_ListName, ListName);// response.Dynamic.ListName;
                    }
                }

                if (string.IsNullOrEmpty(RootResourcePath) && WebPartManager.DisplayMode != WebPartManager.EditDisplayMode)
                    RootResourcePath = SPContext.Current.Web.Url.TrimEnd('/') + "/_layouts/15/Akumina.WebParts.Documents";
                if (upFolderTree.Visible && ListName != null && refreshIdleFT.Value != "true")
                {

                    var wb = SPContext.Current.Web;
                    var baseUrl = wb.Url;
                    try
                    {

                        var folderTreeInfo = new StringBuilder();
                        var doclib = (SPDocumentLibrary)wb.Lists[ListName];
                        var root = doclib.RootFolder;
                        if (HttpContext.Current.Request.Cookies["akuminaNavigationCookie"] != null)
                        {
                            string folderPath = HttpContext.Current.Request.Cookies["akuminaNavigationCookie"].Value.Split('!')[2];
                            CurrentPath = folderPath;
                            folderBox.Text = folderPath;
                        }
                        else if (!Page.IsPostBack)
                        {
                            CurrentPath = root.Name;
                            folderBox.Text = root.Name;
                        }
                        else
                            CurrentPath = folderBox.Text;

                        var sb = new StringBuilder();
                        var liSb = "";
                        liSb = string.Format("<li title='{1}'>{0}|{1}{2}</li>", doclib.Title, doclib.RootFolder.Url,
                            Utility.FrameFolderTree(sb, root, baseUrl));
                        folderTreeInfo.AppendLine(liSb);


                        var finalRes = string.Format("<ul>{0}</ul>", folderTreeInfo);
                        ltlFolderInfo.Text = finalRes;
                    }
                    catch (Exception ex)
                    {
                        var errorLabel = new Label();
                        var errorType = ex.GetType().Name;
                        var errorMessage = "";
                        if (errorType == "InvalidCastException")
                        {
                            errorMessage = "Error: Please select a document library for tree view.";
                        }
                        if (errorType == "ArgumentException")
                        {
                            errorMessage = "Error: Cannot find document library " + ListName;
                        }
                        errorLabel.Text = errorMessage;
                        Controls.Add(errorLabel);
                    }
                    upFolderTree.Update();
                }
                else
                {
                    upFolderTree.Visible = false;
                    refreshIdleFT.Visible = false;
                }
            }
            catch (Exception ex)
            {
                Logging.LogError(ex);
            }
        }

        protected void button_Click(object sender, EventArgs e)
        {
            if (folderBox.Text != string.Empty)
            {
                CurrentPath = folderBox.Text;
            }
        }
    }
}