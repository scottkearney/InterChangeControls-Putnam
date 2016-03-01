using System.Collections.Generic;

namespace Akumina.WebParts.ImportantDates
{
    public class ImportantDatesModel
    {
        public string WebPartTitle { get; set; }
        public string WebPartIcon { get; set; }
        public bool ShowHeader { get; set; }
        public List<ImportantDatesItemsModel> Items { get; set; }

        public class ImportantDatesItemsModel
        {
            
            public string FullDate { get; set; }
            public string DateMonth { get; set; }
            public string DateDay { get; set; }
            public string LinkSrc { get; set; }
            public string LinkText { get; set; }
            public string Target { get; set; }
            public string SubText { get; set; }
            public bool HasLink { get; set; }
        }
    }
}
