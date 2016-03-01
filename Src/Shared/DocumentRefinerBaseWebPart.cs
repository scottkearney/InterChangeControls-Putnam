using System.ComponentModel;
using System.Web.UI.WebControls.WebParts;
namespace Akumina.WebParts.Documents.DocumentRefiner
{
    public class DocumentRefinerBaseWebPart : DocumentBaseWebPart
    {
        public string _categoryName;
        [Category("Akumina InterAction"), WebDisplayName("Enter the Category Term Set Name"), WebBrowsable(true), Personalizable(PersonalizationScope.Shared)]
        public string CategoryName
        {
            get
            {
                if (string.IsNullOrEmpty(_categoryName))
                    _categoryName = "Category";
                return _categoryName;
            }
            set
            {
                _categoryName = value;
            }
        }
        public string FilterOptions { get; set; } 
        //public override EditorPartCollection CreateEditorParts()
        //{
        //    ArrayList editorArray = new ArrayList();
        //    DocumentRefinerConfigurableEditorPart edPart = new DocumentRefinerConfigurableEditorPart();
        //    edPart.ID = this.ID + "_FilterProperty";
        //    edPart.Title = "Filter Options";
        //    editorArray.Add(edPart);
        //    EditorPartCollection editorParts = new EditorPartCollection(editorArray);
        //    return editorParts;
        //}
    }
}