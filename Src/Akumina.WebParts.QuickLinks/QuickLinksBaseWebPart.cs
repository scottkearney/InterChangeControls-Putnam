using System.ComponentModel;
using System.Web.UI.WebControls.WebParts;
using Akumina.InterAction;

namespace Akumina.WebParts.QuickLinks
{
    public enum Directions
    {
        LeftRight, TopBottom
    }

    public class QuickLinksBaseWebPart : AkuminaBaseWebPart
    {

        private string _queryPart = "Quicklinks_AK";
        [Category("Akumina InterAction"), WebDisplayName("Query Part"), WebBrowsable(true), Personalizable(PersonalizationScope.Shared)]
        public string QueryPart
        {
            get
            {
                return _queryPart;
            }
            set
            {
                _queryPart = value;
            }
        }

        [Category("Akumina InterAction"), WebDisplayName("Display As Top Menu"), WebBrowsable(true), Personalizable(PersonalizationScope.Shared)]
        public bool DisplayAsTopMenu { get; set; }


        /// <summary>
        ///    
        /// </summary>
        [Category("Akumina InterAction"), WebDisplayName("Direction"), WebBrowsable(true), Personalizable(PersonalizationScope.Shared)]
        public Directions Directions { get; set; }

        /// <summary>
        ///    
        /// </summary>
        [Category("Akumina InterAction"), WebDisplayName("Icon"), WebBrowsable(true), Personalizable(PersonalizationScope.Shared)]
        public Icons Icon { get; set; }

        protected void MapInstructionSetToProperties(InstructionResponse response, QuickLinks.QuickLinks webPart)
        {
            webPart.QueryPart = response.GetValue("QueryPart", webPart.QueryPart);
            webPart.RootResourcePath = response.GetValue("RootResourcePath", webPart.RootResourcePath);
        }
    }
}