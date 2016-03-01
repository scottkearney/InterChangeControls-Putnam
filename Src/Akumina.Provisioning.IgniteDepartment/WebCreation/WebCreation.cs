using Microsoft.SharePoint;

namespace Akumina.Provisioning.IgniteDepartment.WebCreation
{
    /// <summary>
    /// Web Events
    /// </summary>
    public class WebCreation : SPWebEventReceiver
    {
        //Add to the Pages List
        private string propVal = "Pages";

        /// <summary>
        /// A site was provisioned.
        /// </summary>
        public override void WebProvisioned(SPWebEventProperties properties)
        {
            base.WebProvisioned(properties);
            if (properties.Web.WebTemplate == "IGNITESUBSITEDEFINITION")
            {
                //ProvisionDepartment(properties);
            }
        }
    }
}