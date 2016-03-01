using Akumina.InterAction;
using System.ComponentModel;
using System.Web.UI.WebControls.WebParts;

namespace Akumina.WebParts.ImportantDates
{
    public class ImportantDatesBaseWebPart : AkuminaBaseWebPart
    {
        public ImportantDatesBaseWebPart()
        {
            ListName = "ImportantDates_AK";
        }

        [Category("Akumina InterAction"), WebDisplayName("Enter the List Name"), WebBrowsable(true), Personalizable(PersonalizationScope.Shared)]
        public string ListName { get; set; }

        [Category("Akumina InterAction"), WebDisplayName("Enter the Maximum Number"), WebBrowsable(true), Personalizable(PersonalizationScope.Shared)]
        public int ItemsToDisplay { get; set; }

        /// <summary>
        ///    
        /// </summary>
        [Category("Akumina InterAction"), WebDisplayName("Icon"), WebBrowsable(true), Personalizable(PersonalizationScope.Shared)]
        public Icons Icon { get; set; }

        protected void MapInstructionSetToProperties(InstructionResponse response, ImportantDates webPart)
        {
            webPart.ListName = response.GetValue("ListName", webPart.ListName);
            webPart.ItemsToDisplay = response.GetValue("ItemsToDisplay", webPart.ItemsToDisplay);
            webPart.ItemsToDisplay = webPart.ItemsToDisplay > 0 ? webPart.ItemsToDisplay : 500;
            webPart.RootResourcePath = response.GetValue("RootResourcePath", webPart.RootResourcePath);
        }
    }
}
