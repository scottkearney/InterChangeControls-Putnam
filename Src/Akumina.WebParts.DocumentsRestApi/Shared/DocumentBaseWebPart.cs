using System.ComponentModel;
using System.Web.UI.WebControls.WebParts;
using Akumina.InterAction;
using Microsoft.SharePoint;

namespace Akumina.WebParts.Documents
{
    public class DocumentBaseWebPart : AkuminaBaseWebPart
    {
        public string _docListName;
        [Category("Akumina InterAction"), WebDisplayName("Enter the List Name"), WebBrowsable(true), Personalizable(PersonalizationScope.Shared)]
        public string ListName
        {
            get
            {
                if (string.IsNullOrEmpty(_docListName))
                    _docListName = "AkuminaDocuments";
                return _docListName;
            }
            set
            {
                _docListName = value;
            }
        }

        public string DocumentLibraries = "";//"Converted Forms,Documents,Form Templates,Images,List Template Gallery,Master Page Gallery,Pages,Site Assets,Site Collection Documents,Site Collection Images,Site Pages,Solution Gallery,Style Library,Theme Gallery,Translation Packages,Web Part Gallery";
        [Category("Akumina InterAction"),
        Personalizable(PersonalizationScope.Shared),
        WebBrowsable(true),
        WebDisplayName("Document Libraries"),
        WebDescription("Document Libraries")]
        public string _DocumentLibraries
        {
            get
            {
                return DocumentLibraries;
            }
            set
            {
                DocumentLibraries = value;
            }
        }

        protected void SetRootResourcePathSandbox()
        {
            if (string.IsNullOrEmpty(RootResourcePath))
            {
                RootResourcePath = SPContext.Current.Web.Url + "/_layouts/15/Akumina.WebParts.DocumentsSandbox"; 
            }
        }
    }
}