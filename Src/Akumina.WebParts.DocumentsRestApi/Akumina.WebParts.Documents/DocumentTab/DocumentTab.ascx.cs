using System;
using System.ComponentModel;
using System.Data;
using System.Web.UI.WebControls.WebParts;
using Akumina.InterAction;
using Akumina.WebParts.Documents.Properties;
using Microsoft.SharePoint;

namespace Akumina.WebParts.Documents.DocumentTab
{
    [ToolboxItem(false)]
    public partial class DocumentTab : DocumentTabBaseWebPart, ICurrentTab
    {
        #region Constants

        // Visual Studio might automatically update this path when you change the Visual Web Part project item.
        private const string AscxPath = @"~/_CONTROLTEMPLATES/DocumentGrid/DocumentTab.ascx";

        #endregion

        private String _tab = String.Empty;
        private int _auditCount;
        public ICurrentPath GetCurrentPath;
        private DataTable _odtCollection;
        private DataTable _odtPopular;
        private string _strFolderName = string.Empty;

        [Personalizable]
        public string CurrentTab
        {
            get { return _tab; }
            set { _tab = value; }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            InitializeControl();
        }

        protected override void OnPreRender(EventArgs e)
        {
            try
            {
                var username = SPContext.Current.Web.CurrentUser.Name;

                myFiles.Attributes["user"] = username;
                recentFiles.Attributes["file-count"] = NoOfRecentFiles != null ? NoOfRecentFiles.ToString() : "0";
                popularFiles.Attributes["file-count"] = NoOfPopularFiles != null ? NoOfPopularFiles.ToString() : "0";
            }
            catch (Exception ex)
            {
                Logging.LogError(ex);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {


                if (!Page.IsPostBack)
                {
                    if (!string.IsNullOrEmpty(InstructionSet))
                    {
                        InstructionRepository clientServices = new InstructionRepository();
                        var response = clientServices.Execute(InstructionSet);
                        if (response != null && response.Dictionary != null)
                        {
                            ListName = clientServices.GetValue(response.Dictionary, Resources.colName_ListName, ListName);//response.Dynamic.ListName;
                            NoOfPopularFiles = clientServices.GetValue(response.Dictionary, Resources.colName_Popular, NoOfPopularFiles);
                            NoOfRecentFiles = clientServices.GetValue(response.Dictionary, Resources.colName_Recent, NoOfRecentFiles);
                        }
                    }
                    if (string.IsNullOrEmpty(RootResourcePath))
                        RootResourcePath = SPContext.Current.Web.Url.TrimEnd('/') + "/_layouts/15/Akumina.WebParts.Documents";
                    _tab = "All Files";
                }
            }
            catch (Exception ex)
            {
                Logging.LogError(ex);
            }
        }

    }
}