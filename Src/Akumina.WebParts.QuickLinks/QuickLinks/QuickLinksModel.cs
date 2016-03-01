using System.Collections.Generic;

namespace Akumina.WebParts.QuickLinks
{

    public class QuickLinksModel
    {
        public QuickLinksModel()
        {
            Items = new List<QuickLinksModel>();
            DisplayOrder = -1;
            OpenInNewWindow = false;
        }

        public int Id { get; set; }
        public int ParentId { get; set; }
        public string Title { get; set; }
        public string WebPartIcon { get; set; }
        public QuickLinksNodeType NodeType {get;set;}
        public string Link {get;set;}
        public string Target { get; set; }
        public List<QuickLinksModel> Items { get; set; }
        public int DisplayOrder { get; set; }
        public bool OpenInNewWindow { get; set; }
        public bool ShowHeader { get; set; }
        public bool HasCategories { get; set; }
        public bool HasLinks { get; set; }
        public bool IsFolder { get; set; }
        public bool IsItem { get; set; }
    }

    public enum QuickLinksNodeType
    {
        Root,
        Category,
        Item
    }

}
