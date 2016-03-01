using System.ComponentModel;
using System.Web.UI.WebControls.WebParts;
namespace Akumina.WebParts.Documents.DocumentGrid
{
    public class DocumentGridBaseWebPart : DocumentBaseWebPart
    {
        //[Category("Akumina InterAction"), WebDisplayName("Enter the Header Text"), WebBrowsable(true), Personalizable(PersonalizationScope.Shared)]
        //public string HeaderName { get; set; }
        //public string DisplayOrder { get; set; }

        public string _Excludelist = "Converted Forms,Documents,Form Templates,Images,List Template Gallery,Master Page Gallery,Pages,Site Assets,Site Collection Documents,Site Collection Images,Site Pages,Solution Gallery,Style Library,Theme Gallery,Translation Packages,Web Part Gallery";
        [Category("Akumina InterAction"),
        Personalizable(PersonalizationScope.Shared),
        WebBrowsable(true),
        WebDisplayName("Exclude List"),
        WebDescription("Exclude List")]
        public string Excludelist
        {
            get
            {
                return _Excludelist;
            }
            set
            {
                _Excludelist = value;
            }
        }
        public string _documentListpage = "SparkLibraryListing.aspx";
        [Category("Akumina InterAction"),
        Personalizable(PersonalizationScope.Shared),
        WebBrowsable(true),
        WebDisplayName("Document List page"),
        WebDescription("Document List page")]
        public string DocumentListPage
        {
            get
            {
                return _documentListpage;
            }
            set
            {
                _documentListpage = value;
            }
        }

        public string MenuProperty { get; set; }
        public string MoreMenuProperty { get; set; }

        public string DocumentListOptions { get; set; } 
        //public string _ContextMenuOperations = "Converted Forms,Documents,Form Templates,Images,List Template Gallery,Master Page Gallery,Pages,Site Assets,Site Collection Documents,Site Collection Images,Site Pages,Solution Gallery,Style Library,Theme Gallery,Translation Packages,Web Part Gallery";
        //[Category("Akumina InterAction"),
        //Personalizable(PersonalizationScope.Shared),
        //WebBrowsable(true),
        //WebDisplayName("Exclude List"),
        //WebDescription("Exclude List")]
        //public string Excludelist
        //{
        //    get
        //    {
        //        return _Excludelist;
        //    }
        //    set
        //    {
        //        _Excludelist = value;
        //    }
        //}


        //public override EditorPartCollection CreateEditorParts()
        //{
        //    ArrayList editorArray = new ArrayList();
        //    DocumentGridConfigurableEditorPart edPart = new DocumentGridConfigurableEditorPart();
        //    edPart.ID = this.ID + "_MenuProperty";
        //    edPart.Title = "Configuration Options";
            
        //    //edPart.DisplayTitle = "Context Menu Options";

        //    editorArray.Add(edPart);
        //    EditorPartCollection editorParts = new EditorPartCollection(editorArray);
        //    return editorParts;
        //}
    }
}