using System.ComponentModel;
using System.Web.UI.WebControls.WebParts;
using Akumina.InterAction;

namespace Akumina.WebParts.Announcement
{
    public class AnnouncementDetailBaseWebPart : AkuminaBaseWebPart
    {
        [Category("Akumina InterAction"), WebDisplayName("Enter the List Name"), WebBrowsable(true), Personalizable(PersonalizationScope.Shared), DefaultValue("")]
        public string ListName { get; set; }

        protected void MapInstructionSetToProperties(InstructionResponse response, AnnouncementDetail.AnnouncementDetail webPart)
        {
            webPart.ListName = response.GetValue("ListName", webPart.ListName);
        }
    }
}
