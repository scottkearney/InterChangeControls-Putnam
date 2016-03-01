using Microsoft.SharePoint.WebPartPages;
using System.Collections;
using System.ComponentModel;
using System.Web.UI.WebControls.WebParts;
namespace Akumina.WebParts.Documents.DocumentGrid
{
    public class DocumentGridBaseWebPart : DocumentBaseWebPart
    {
        public string MenuProperty { get; set; }
        public string MoreMenuProperty { get; set; }
        public string DocumentListOptions { get; set; }


        public string _rowLimit;
        [Category("Akumina InterAction"), WebDisplayName("Enter the MaxResult"), WebBrowsable(true), Personalizable(PersonalizationScope.Shared)]
        public string RowLimit
        {
            get
            {
                if (string.IsNullOrEmpty(_rowLimit))
                    _rowLimit = "100";
                return _rowLimit;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    int i;
                    if (!int.TryParse(value, out i))
                        throw new WebPartPageUserException("The item \"" + value + "\" is not a valid number");
                }
                _rowLimit = value;
            }
        }

        public enum _JSRenderMode
        {
            Caml,
            KQL
        }

        //public string _JSRenderMode;
        [Category("Akumina InterAction"), WebDisplayName("Select the JS Render Mode"), WebBrowsable(true), Personalizable(PersonalizationScope.Shared)]
        public _JSRenderMode JSRenderMode
        {
            get;
            set;
        }

        public override EditorPartCollection CreateEditorParts()
        {
            ArrayList editorArray = new ArrayList();
            DocumentGridConfigurableEditorPart edPart = new DocumentGridConfigurableEditorPart();
            edPart.ID = this.ID + "_MenuProperty";
            edPart.Title = "Configuration Options";
            editorArray.Add(edPart);
            EditorPartCollection editorParts = new EditorPartCollection(editorArray);
            return editorParts;
        }

    }
}