using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Akumina.ListDefinition.Provision.ManagedMetadataSolution;
using Akumina.ListDefinition.Provision.Properties;
using Microsoft.SharePoint.Administration;
using System.IO;
using Microsoft.SharePoint.Taxonomy;
using System.Collections.Generic;

namespace Akumina.ListDefinition.Provision.Features.AkuminaListDefinitionFeature
{
    /// <summary>
    /// This class handles events raised during feature activation, deactivation, installation, uninstallation, and upgrade.
    /// </summary>
    /// <remarks>
    /// The GUID attached to this class may be used during packaging and should not be modified.
    /// </remarks>

    [Guid("d0859e50-580e-4841-8c5a-2ecb02f48c50")]
    public class AkuminaListDefinitionFeatureEventReceiver : SPFeatureReceiver
    {
        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            string TermGroupName = "Akumina";
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
            string groupName = TermGroupName;//properties.Feature.Properties[TermGroupName].Value;
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
                    provisionIDSList(oweb);
                    provisionBannerImage(oweb);
                    provisionQuickLinks(oweb);
                    provisionEnforcingUniqueValues(oweb);
                    AllowEveryOnePermission(oweb);
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

        private void AllowEveryOnePermission(SPWeb web)
        {
            try
            {
                var list = web.Lists.TryGetList("AkuminaEventLogs");
                if (list != null)
                {
                    web.AllowUnsafeUpdates = true;
                    SPUser allUsers = web.AllUsers[@"c:0(.s|true"];
                    SPRoleDefinitionCollection roleCollection = web.RoleDefinitions;
                    SPRoleDefinition roleDefinition = new SPRoleDefinition()
                    {
                        Name = "Everyone Add List Item",
                        BasePermissions = SPBasePermissions.AddListItems
                    };
                    roleDefinition.BasePermissions = SPBasePermissions.AddListItems;
                    SPRoleAssignment roleAssignment = new SPRoleAssignment((SPPrincipal)allUsers);
                    web.RoleDefinitions.Add(roleDefinition);
                    roleAssignment.RoleDefinitionBindings.Add(web.RoleDefinitions["Everyone Add List Item"]);
                    list.BreakRoleInheritance(true);
                    list.RoleAssignments.Add(roleAssignment);
                    list.Update();
                    web.AllowUnsafeUpdates = false;
                }
            }
            catch (Exception ex)
            {

            }
        }
        private void provisionBannerImage(SPWeb web)
        {
            try
            {
                var list = web.Lists.TryGetList("AkuminaBanner");
                if (list != null)
                {
                    web.AllowUnsafeUpdates = true;
                    SPListItemCollection BannerListitem = list.Items;
                    string imgUrl = string.Empty;
                    foreach (SPListItem listitem in BannerListitem)
                    {
                        if (listitem != null)
                        {
                            imgUrl = web.Url + "/AkuminaImages/Sparkbanner-" + listitem["BannerItemOrder"] + ".jpg";
                            listitem["BannerImage"] = imgUrl;
                            listitem.Update();
                        }
                    }
                    web.AllowUnsafeUpdates = false;

                }
            }
            catch { }
        }
        private void provisionIDSList(SPWeb web)
        {
            string siteId = web.Site.RootWeb.ID.ToString();
            var IdsList = web.Lists.TryGetList("AkuminaIDS");
            if (IdsList != null)
            {
                web.AllowUnsafeUpdates = true;
                List<string> idslist = new List<string>();
                idslist.Add("AkuminaAnnouncementsIDS");
                idslist.Add("AkuminaBannerIDS");
                idslist.Add("AkuminaContentBlockIDS");
                idslist.Add("AkuminaDiscussionsIDS");
                idslist.Add("AkuminaDocumentsIDS");
                idslist.Add("AkuminaMiscIDS");
                //idslist.Add("AkuminaSiteLinksIDS");

                foreach (var idslistitem in idslist)
                {
                    SPListItem itemToAdd = IdsList.Items.Add();

                    itemToAdd["Title"] = idslistitem;
                    itemToAdd["ReferenceList"] = idslistitem;

                    itemToAdd["SiteId"] = siteId;

                    itemToAdd.Update();

                }
                web.AllowUnsafeUpdates = false;
            }

        }

        private void provisionQuickLinks(SPWeb web)
        {
            try
            {
                var QuickLinksList = web.Lists.TryGetList("AkuminaQuickLinks");
                if (QuickLinksList != null)
                {
                    web.AllowUnsafeUpdates = true;
                    SPFieldLookup primaryColumn = (SPFieldLookup)QuickLinksList.Fields.GetFieldByInternalName("ParentItem");
                    string strProjectedCol = QuickLinksList.Fields.AddDependentLookup("TempCol", primaryColumn.Id);
                    SPFieldLookup projectedCol = (SPFieldLookup)QuickLinksList.Fields.GetFieldByInternalName(strProjectedCol);
                    projectedCol.LookupField = QuickLinksList.Fields["Title"].InternalName;
                    projectedCol.Hidden = true;
                    projectedCol.Update();

                    SPView view = QuickLinksList.DefaultView;
                    if (view.ViewFields.Exists("TempCol"))
                    {
                        view.ViewFields.Delete("TempCol");
                        view.Update();
                    }
                    web.AllowUnsafeUpdates = false;
                }
            }
            catch
            {

            }
        }

        private void provisionEnforcingUniqueValues(SPWeb web)
        {
            try
            {
                web.AllowUnsafeUpdates = true;
                List<string> idslist = new List<string>();
                idslist.Add("AkuminaAnnouncementsIDS");
                idslist.Add("AkuminaBannerIDS");
                idslist.Add("AkuminaContentBlockIDS");
                idslist.Add("AkuminaDiscussionsIDS");
                idslist.Add("AkuminaDocumentsIDS");
                idslist.Add("AkuminaMiscIDS");
                idslist.Add("AkuminaSiteLinksIDS");

                foreach (var idslistitem in idslist)
                {
                    var custList = web.Lists.TryGetList(idslistitem);
                    if (custList != null)
                    {
                        SPField custTitle = custList.Fields["Title"];

                        custTitle.Indexed = true;
                        custTitle.EnforceUniqueValues = true;
                        custTitle.Update();
                    }
                }
                web.AllowUnsafeUpdates = false;
            }
            catch
            {

            }

        }

        //private static void ConnectTaxonomyField(SPSite site, Guid fieldId, string termSetName, string groupName, bool required)
        //{
        //    if (site.RootWeb.Fields.Contains(fieldId))
        //    {
        //        TermStore termStore = AkuminaTaxonomyExtensions.GetDefaultTermStore(site);
        //        Group group = termStore.Groups.GetByName(groupName);
        //        TermSet termSet = group.TermSets[termSetName];

        //        TaxonomyField field = site.RootWeb.Fields[fieldId] as TaxonomyField;

        //        // Connect to MMS 
        //        field.SspId = termSet.TermStore.Id;
        //        field.TermSetId = termSet.Id;
        //        field.TargetTemplate = string.Empty;
        //        field.AnchorId = Guid.Empty;
        //        field.IsPathRendered = true;
        //        field.Required = required;
        //        field.Update(true);
        //    }
        //    else
        //    {
        //        throw new ArgumentException(string.Format("Field {0} not found in site {1}", fieldId, site.Url), "fieldId");
        //    }
        //}


        // Uncomment the method below to handle the event raised before a feature is deactivated.

        //public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        //{
        //    //
        //    // Obtain reference to current site collection
        //    //
        //    SPSite site = properties.Feature.Parent as SPSite;
        //    if (site == null)
        //    {
        //        return;
        //    }
        //    //
        //    // Get the term group name from properties
        //    //
        //    string groupName = properties.Feature.Properties["TermGroupName"].Value;
        //    if (string.IsNullOrEmpty(groupName))
        //    {
        //        return;
        //    }
        //    try
        //    {
        //        SPSecurity.RunWithElevatedPrivileges(delegate()
        //        {
        //            DeleteMetadata(site, groupName);
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        WriteError(ex);
        //        throw;
        //    }
        //}

        #region private methods

        private void ImportMetadata(SPSite site, string groupName)
        {
            //
            // Create ManagedMetadataImporterLogic instance
            //
            ManagedMetadataImporterLogic importer = new ManagedMetadataImporterLogic(site);
            //
            // Import the term sets
            //
            using (StringReader reader = new StringReader(Resources.AkuminaTaxonomy))
            {
                importer.ImportTermSet(groupName, reader);
            }

        }

        /* private void DeleteMetadata(SPSite site, string groupName)
         {
             //
             // Create ManagedMetadataImporterLogic instance
             //
             ManagedMetadataImporterLogic importer = new ManagedMetadataImporterLogic(site);
             //
             // Delete term group and all terms sets below it
             //
             importer.DeleteGroup(groupName);
         }*/

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
        #endregion

        public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        {

            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    SPWeb oweb = (SPWeb)properties.Feature.Parent;
                    ////using (SPSite osite = properties.Feature.Parent as SPSite)
                    ////{
                    ////    using (SPWeb oweb = osite.OpenWeb())
                    ////    {
                    deleteIDSLists(oweb);
                    DeleteEveryOnePermission(oweb);
                    ////    }
                    ////}
                });
            }
            catch (Exception ex)
            {
                WriteError(ex);
                throw;
            }
        }
        private void DeleteEveryOnePermission(SPWeb web)
        {
            var list = web.Lists.TryGetList("AkuminaEventLogs");
            if (list != null)
            {
                bool RdefExists = false;
                SPRoleDefinitionCollection roleCollection = web.RoleDefinitions;
                foreach (SPRoleDefinition item in roleCollection)
                {
                    if (item.Name == "Everyone Add List Item")
                    {

                        RdefExists = true;
                        break;
                    }
                    else
                    {
                        RdefExists = false;
                    }
                }

                if (RdefExists)
                    web.RoleDefinitions.Delete("Everyone Add List Item");
            }
        }
        private void deleteIDSLists(SPWeb web)
        {


            var IdsList = web.Lists.TryGetList("AkuminaIDS");
            if (IdsList != null)
            {
                web.AllowUnsafeUpdates = true;
                SPListItemCollection IdsListitem = IdsList.Items;
                foreach (SPListItem listitem in IdsListitem)
                {
                    var Idslistitem = web.Lists.TryGetList(listitem["Title"].ToString());

                    if (Idslistitem != null)
                    {
                        Idslistitem.Delete();
                        IdsList.Items.DeleteItemById(listitem.ID);
                    }
                }
                IdsList.Delete();
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
