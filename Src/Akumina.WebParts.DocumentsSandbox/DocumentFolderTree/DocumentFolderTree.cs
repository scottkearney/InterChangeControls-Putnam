using System;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Akumina.InterAction;
using Akumina.WebParts.Documents.DocumentFolderTree;
using Microsoft.SharePoint;

namespace Akumina.WebParts.DocumentsSandbox.DocumentFolderTree
{
    [ToolboxItem(false)]
    public class DocumentFolderTree : DocumentFolderTreeBaseWebPart
    {
        private readonly TextBox _folderBox = new TextBox();
        private readonly Literal _ltlFolderInfo = new Literal();
     

        protected override void CreateChildControls()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            try
            {
                var reset = HttpContext.Current.Request.QueryString["fdreset"];
                if (string.IsNullOrEmpty(reset) || reset == "true")
                {
                    BindChildControls();
                    Page.Load += Page_Load;
                }
            }
            catch (Exception ex)
            {
                Logging.LogError(ex);
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
        }

        private void Page_Load(object sender, EventArgs e)
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
                            object listName = string.Empty;
                            var values = response.Dictionary.FirstOrDefault().Value;
                            if (values.Count > 0)
                            {
                                values.TryGetValue("ListName", out listName);
                            }
                            if (listName != null && !string.IsNullOrEmpty(listName.ToString()))
                                ListName = listName.ToString();
                        }
                    }
                }

                if (ListName != null)
                {
                    var wb = SPContext.Current.Web;
                    var baseUrl = wb.Url;


                    SPDocumentLibrary doclib = null;
                    SPList doclibList = wb.Lists.TryGetList(ListName);
                    if (doclibList != null && doclibList.BaseType.ToString() == "DocumentLibrary")
                    {
                        doclib = (SPDocumentLibrary)doclibList;
                        _folderBox.Text = ListName;
                        try
                        {

                            var folderTreeInfo = new StringBuilder();
                            var root = doclib.RootFolder;
                            var sb = new StringBuilder();
                            var liSb = "";
                            liSb = String.Format("<li title='{1}'>{0}|{1}{2}</li>", doclib.Title, doclib.RootFolder.Url,
                                Utility.FrameFolderTree(sb, root, baseUrl));
                            folderTreeInfo.AppendLine(liSb);
                            var finalRes = String.Format("<ul>{0}</ul>", folderTreeInfo);
                            _ltlFolderInfo.Text = finalRes;
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
                                errorMessage = "Error: There is no such document " +
                                               "library with this name: " + ListName;
                            }
                            errorLabel.Text = errorMessage;
                            Controls.Add(errorLabel);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
              Logging.LogError(ex);
            }
        }

        protected override void OnLoad(EventArgs e)
        {
        }

        protected void BindChildControls()
        {
            try
            {
                var folderzone = new HtmlGenericControl("DIV");
                folderzone.ID = "folderzone";
                folderzone.ClientIDMode = ClientIDMode.Static;
                folderzone.Style.Add("display", "none");
                _folderBox.ID = "folderBox";
                _folderBox.ClientIDMode = ClientIDMode.Static;

                folderzone.Controls.Add(_folderBox);

                var interActionZone = new HtmlGenericControl("DIV");
                interActionZone.Attributes["class"] = "interAction";

                var folderTreeZone = new HtmlGenericControl("DIV");
                folderTreeZone.Attributes["class"] = "ia-folder-tree";
                folderTreeZone.Style.Add("display", "none");

                _ltlFolderInfo.ID = "ltlFolderInfo";
                _ltlFolderInfo.ClientIDMode = ClientIDMode.Static;
                folderTreeZone.Controls.Add(_ltlFolderInfo);
                interActionZone.Controls.Add(folderTreeZone);

                Controls.Add(folderzone);
                Controls.Add(interActionZone);
            }
            catch (Exception ex)
            {
              Logging.LogError(ex);
            }
        }

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
    }
}