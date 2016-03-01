using Microsoft.SharePoint.WebPartPages;
using System.ComponentModel;
using System.Web.UI.WebControls.WebParts;
namespace Akumina.WebParts.Documents.DocumentGrid
{
    public class DocumentGridBaseWebPart : DocumentBaseWebPart
    {
        public string MenuProperty { get; set; }
        public string MoreMenuProperty { get; set; }
        public string DocumentListOptions { get; set; }
    }
}