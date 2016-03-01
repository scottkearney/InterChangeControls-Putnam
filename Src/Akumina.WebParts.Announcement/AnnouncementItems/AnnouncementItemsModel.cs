using System.Collections.Generic;

namespace Akumina.WebParts.Announcement.AnnouncementItems
{
    public class AnnouncementItemsListModel
    {
        public string WebPartTitle { get; set; }
        public bool ShowHeader { get; set; }
        public string WebPartIcon { get; set; }
        public int RowLimit { get; set; }
        public string ViewAllLink { get; set; }
        public List<AnnouncementItemsModel> Items { get; set; }
    }

    public class AnnouncementItemsModel
    {
        public string Title { get; set; }
        public string Summary { get; set; }
        public string Url { get; set; }
        public string Created { get; set; }
    }
    public enum DisplayTemplate
    {
        PageTemplate,
        WidgetTemplate
    }
}
