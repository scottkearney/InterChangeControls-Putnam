using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.Administration;

namespace Akumina.Provisioning.IgniteLists.WebCreation
{
    /// <summary>
    /// Web Events
    /// </summary>
    public class WebCreation : SPWebEventReceiver
    {
        /// <summary>
        /// A site was provisioned.
        /// </summary>
        public override void WebProvisioned(SPWebEventProperties properties)
        {
            base.WebProvisioned(properties);
            Provision provision = new Provision();
            provision.IsIgnite = true;
            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    var xmlfilePath = SPUtility.GetGenericSetupPath(@"TEMPLATE\Features\Akumina.Provisioning.IgniteLists_Provisioning");
                    xmlfilePath = xmlfilePath.Replace("14", "15");
                    provision.CreateLists(properties.Web, xmlfilePath,false);
                    provision.ProvisionIdsList(properties.Web, false);
                });
            }
            catch (Exception ex)
            {
                WriteError(ex);
                throw;
            }
        }
        private void WriteError(Exception exception)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                SPDiagnosticsService.Local.WriteTrace(0,
                                                      new SPDiagnosticsCategory("Managed Metadata Feature", TraceSeverity.Unexpected, EventSeverity.Error),
                                                      TraceSeverity.Unexpected,
                                                      exception.Message,
                                                      exception.StackTrace);
            });
        }

    }
}