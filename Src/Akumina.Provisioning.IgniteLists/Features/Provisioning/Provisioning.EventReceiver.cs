using System;
using System.Runtime.InteropServices;
using Microsoft.SharePoint;
//using Akumina.ListDefinition.Provision.ManagedMetadataSolution;
//using Akumina.ListDefinition.Provision.Properties;
using Microsoft.SharePoint.Administration;
//using Microsoft.SharePoint.Taxonomy;

namespace Akumina.Provisioning.IgniteLists.Features
{
    /// <summary>
    /// This class handles events raised during feature activation, deactivation, installation, uninstallation, and upgrade.
    /// </summary>
    /// <remarks>
    /// The GUID attached to this class may be used during packaging and should not be modified.
    /// </remarks>

    [Guid("8771f045-68ea-4b29-be4a-6dcc4b38b066")]
    public class AkListDefinitionFeatureEventReceiver : SPFeatureReceiver
    {
        Provision _provision = new Provision();
        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            
            string termGroupName = "Akumina";
            _provision.IsIgnite = true;
            //
            // Obtain reference to current site collection
            //
            ////SPSite site = properties.Feature.Parent as SPSite;
            SPWeb web = (SPWeb)properties.Feature.Parent;
            if (web == null)
            {
                return;
            }
            //
            // Get the term group name from properties
            //
            string groupName = termGroupName;//properties.Feature.Properties[TermGroupName].Value;
            if (string.IsNullOrEmpty(groupName))
            {
                return;
            }
            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    SPWeb oweb = (SPWeb)properties.Feature.Parent;
                    ////using (SPSite osite = properties.Feature.Parent as SPSite)
                    ////{
                    ////    using (SPWeb oweb = osite.OpenWeb())
                    ////    {
                    //ImportMetadata(site, groupName);

                    var xmlfilePath = properties.Definition.RootDirectory;
                    _provision.CreateContentTypes(oweb, properties);
                    _provision.CreateLists(oweb, xmlfilePath);                    
                    _provision.ProvisionIdsList(oweb);                    
                    _provision.ProvisionBannerImage(oweb);
                    _provision.ProvisionQuickLinks(oweb);
                    //_provision.ProvisionEnforcingUniqueValues(oweb);
                    _provision.AllowEveryOnePermission(oweb);
                    ////}
                    ////}
                });
            }
            catch (Exception ex)
            {
                WriteError(ex);
                throw;
            }


            /*SPSite site = properties.Feature.Parent as SPSite;
              SPWeb web = site.RootWeb;

              try
              {
                  string taxonomygroupName = "Akumina";
                  string termSetName = "Category";

                  SPSecurity.RunWithElevatedPrivileges(delegate()
                  {
                      using (SPSite elevatedSite = new SPSite(web.Site.ID))
                      {
                          Guid fieldId = new Guid("{DD02CB92-A716-4209-99D2-4C5EF8AE8816}");
                          ConnectTaxonomyField(elevatedSite, fieldId, termSetName, taxonomygroupName, true);
                      }
                  });
              }
              catch (Exception ex)
              {
              }*/

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
        

        public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        {

            //try
            //{
            //    SPSecurity.RunWithElevatedPrivileges(delegate()
            //    {
            //        SPWeb oweb = (SPWeb)properties.Feature.Parent;
            //        ////using (SPSite osite = properties.Feature.Parent as SPSite)
            //        ////{
            //        ////    using (SPWeb oweb = osite.OpenWeb())
            //        ////    {
            //        deleteIDSLists(oweb);
            //        DeleteEveryOnePermission(oweb);
            //        ////    }
            //        ////}
            //    });
            //}
            //catch (Exception ex)
            //{
            //    WriteError(ex);
            //    throw;
            //}
        }
        private void DeleteEveryOnePermission(SPWeb web)
        {
            var list = web.Lists.TryGetList("AkEventLogs");
            if (list != null)
            {
                bool rdefExists = false;
                SPRoleDefinitionCollection roleCollection = web.RoleDefinitions;
                foreach (SPRoleDefinition item in roleCollection)
                {
                    if (item.Name == "Everyone Add List Item")
                    {

                        rdefExists = true;
                        break;
                    }
                    else
                    {
                        rdefExists = false;
                    }
                }

                if (rdefExists)
                    web.RoleDefinitions.Delete("Everyone Add List Item");
            }
        }
        private void DeleteIdsLists(SPWeb web)
        {


            var idsList = web.Lists.TryGetList("Ak");
            if (idsList != null)
            {
                web.AllowUnsafeUpdates = true;
                SPListItemCollection idsListitem = idsList.Items;
                foreach (SPListItem listitem in idsListitem)
                {
                    var idslistitem = web.Lists.TryGetList(listitem["Title"].ToString());

                    if (idslistitem != null)
                    {
                        idslistitem.Delete();
                        idsList.Items.DeleteItemById(listitem.ID);
                    }
                }
                idsList.Delete();
                web.AllowUnsafeUpdates = false;
            }

        }

        // Uncomment the method below to handle the event raised after a feature has been activated.

        //public override void FeatureActivated(SPFeatureReceiverProperties properties)
        //{
        //}


        // Uncomment the method below to handle the event raised before a feature is deactivated.

        //public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        //{
        //}


        // Uncomment the method below to handle the event raised after a feature has been installed.

        //public override void FeatureInstalled(SPFeatureReceiverProperties properties)
        //{
        //}


        // Uncomment the method below to handle the event raised before a feature is uninstalled.

        //public override void FeatureUninstalling(SPFeatureReceiverProperties properties)
        //{
        //}

        // Uncomment the method below to handle the event raised when a feature is upgrading.

        //public override void FeatureUpgrading(SPFeatureReceiverProperties properties, string upgradeActionName, System.Collections.Generic.IDictionary<string, string> parameters)
        //{
        //}
    }
}
