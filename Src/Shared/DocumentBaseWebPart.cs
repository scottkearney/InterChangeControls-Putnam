using System.ComponentModel;
using System.Web.UI.WebControls.WebParts;
using Akumina.InterAction;
using Microsoft.SharePoint;

namespace Akumina.WebParts.Documents
{
    public class DocumentBaseWebPart : AkuminaBaseWebPart
    {
        public string _docListName;

        [Category("Akumina InterAction"), WebDisplayName("Enter the List Name"), WebBrowsable(true),
         Personalizable(PersonalizationScope.Shared)]
        public string ListName
        {
            get
            {
                if (string.IsNullOrEmpty(_docListName))
                    _docListName = "Documents";
                return _docListName;
            }
            set { _docListName = value; }
        }


        protected void SetRootResourcePathSandbox()
        {
            if (string.IsNullOrEmpty(RootResourcePath))
            {
                RootResourcePath = SPContext.Current.Web.Url + "/Akumina.WebParts.DocumentsSandbox";
            }
        }
    }
}