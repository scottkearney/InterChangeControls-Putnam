using System.ComponentModel;
using System.Web.UI.WebControls.WebParts;
using Akumina.InterAction;
using Akumina.WebParts.Announcement.AnnouncementItems;

namespace Akumina.WebParts.Announcement
{
    public class AnnouncementItemsBaseWebPart : AkuminaBaseWebPart
    {
        public AnnouncementItemsBaseWebPart()
        {
            //ListName = "CompanyNews_AK";
            DisplayTemplate = DisplayTemplate.WidgetTemplate;
            ViewAllLink = "NewsList.aspx";
        }
        public string UniqueId { get; set; }

        [Category("Akumina InterAction"), WebDisplayName("Enter the List Name"), WebBrowsable(true), Personalizable(PersonalizationScope.Shared), DefaultValue("CompanyNews_AK")]
        public string ListName { get; set; }

        [Category("Akumina InterAction"), WebDisplayName("Enter link for View All page"), WebBrowsable(true), Personalizable(PersonalizationScope.Shared), DefaultValue("")]
        public string ViewAllLink { get; set; }

        [Category("Akumina InterAction"), WebDisplayName("Display Template"), WebBrowsable(true), Personalizable(PersonalizationScope.Shared), DefaultValue(DisplayTemplate.WidgetTemplate)]
        public DisplayTemplate DisplayTemplate { get; set; }

        [Category("Akumina InterAction"), WebDisplayName("Enter the maximum number of items to display"), WebBrowsable(true), Personalizable(PersonalizationScope.Shared), DefaultValue(5)]
        public int ItemsToDisplay { get; set; }

        [Category("Akumina InterAction"), WebDisplayName("Icon"), WebBrowsable(true), Personalizable(PersonalizationScope.Shared)]
        public Icons Icon { get; set; }

        protected void MapInstructionSetToProperties(InstructionResponse response, AnnouncementItems.AnnouncementItems webPart)
        {
            webPart.ListName = response.GetValue("ListName", webPart.ListName);
            webPart.ViewAllLink = response.GetValue("ViewAllLink", webPart.ViewAllLink);
            webPart.DisplayTemplate = ParseEnum<DisplayTemplate>(response.GetValue("DisplayTemplate", webPart.DisplayTemplate.ToString()));
            webPart.ItemsToDisplay = response.GetValue("ItemsToDisplay", webPart.ItemsToDisplay);
            webPart.ItemsToDisplay = webPart.ItemsToDisplay > 0 ? webPart.ItemsToDisplay : 500;
            webPart.RootResourcePath = response.GetValue("RootResourcePath", webPart.RootResourcePath);
        }
    }
}
