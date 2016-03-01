using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.SharePoint;
//using Microsoft.SharePoint.WebPartPages;
using System.Web.UI.WebControls.WebParts;
using System.Xml;
using System.Globalization;
using System.Reflection;
using System.Collections.Generic;
//using Microsoft.SharePoint.WebPartPages;  

namespace Akumina.SiteDefinition.Provision.Features.SiteDefinition_Feature
{
    /// <summary>
    /// This class handles events raised during feature activation, deactivation, installation, uninstallation, and upgrade.
    /// </summary>
    /// <remarks>
    /// The GUID attached to this class may be used during packaging and should not be modified.
    /// </remarks>

    [Guid("76870955-afa2-47d1-8ffc-d9a7a76b6045")]
    public class SiteDefinition_FeatureEventReceiver : SPFeatureReceiver
    {
        // Uncomment the method below to handle the event raised after a feature has been activated.
        private string propVal = "Pages";
        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    SPWeb web = (SPWeb)properties.Feature.Parent;

                    //if (SPContext.Current.Web.Properties.ContainsKey("AkuminaPageSetting"))
                    //    propVal = SPContext.Current.Web.Properties["AkuminaPageSetting"];
                    //if (string.IsNullOrEmpty(propVal))


                    //try
                    //{

                    //    SPFolder akumina = web.Folders["_catalogs"].SubFolders["masterpage"].SubFolders["AkuminaSpark"].SubFolders["Pages"];
                    //    //propVal = propVal.Replace(" ", "");
                    //    //SPList sourceDocLib = web.Lists["Pages"];
                    //    //SPListItem spItem = sourceDocLib.Items[2];
                    //    foreach (SPFile srcFile in akumina.Files)
                    //    {

                    //        srcFile.CopyTo(propVal + "/" + srcFile.Name, true);
                    //    }
                    //}
                    //catch (Exception ex)
                    //{ }
                    try
                    {
                        var list = web.Lists.TryGetList("Device Channels");
                        if (list != null)
                        {
                            var item = list.Items.Add();
                            if (item != null)
                            {
                                item["Name"] = "Devices";
                                item["Alias"] = "Devices";
                                item["Description"] = "Device Channel";
                                item["Device Inclusion Rules"] = "Android\niPad\niPod\niPhone\nBlackBerry\nIEMobile\nWebOS\nx64";
                                item["Active"] = true;
                                item.Update();
                            }
                        }
                    }
                    catch (Exception ex)
                    { }


                    //string[] dmsWebparts = { "AkuminaInterActionDocumentFolderTree.webpart", "AkuminaInterActionDocumentRefiner.webpart", "AkuminaInterActionDocumentTab.webpart", "AkuminaInterActionDocumentGrid.webpart" };
                    //string[] dmszones = { "TreeContent", "FilterContent", "TabContent", "GridContent" };

                    string[] dmsWebparts = { "AkuminaInterActionDocumentRefiner.webpart", "AkuminaInterActionDocumentTab.webpart", "AkuminaInterActionDocumentGrid.webpart" };
                    string[] dmszones = { "FilterContent", "TabContent", "GridContent" };
                   

                    string[] homeZones = { "BannerItem", "DynamicControls", "DocumentsSummmary", "ContentAlert", "Announcements", "Department", "QuickLinks", "WelcomeContent" };
                    string[] homeWebparts = { "AkuminaInterActionBanner.webpart", "Akumina.WebParts.Miscellaneous_PlaceHolder.webpart", "Akumina.WebParts.DocumentSummaryList_DocumentSummaryList.webpart", "Akumina.WebParts.ContentBlock_ListItem.webpart", "Akumina.WebParts.Announcement_AnnouncementItems.webpart", "Akumina.WebParts.SiteSummaryList_SiteSummaryList.webpart", "Akumina.WebParts.SiteLinks_SiteLinks.webpart", "Akumina.WebParts.ContentBlock_ListItem.webpart" };

                    string[] DiscussionZones = { "FullPage" };
                    string[] newDisWebparts = { "Akumina.WebParts.DiscussionBoard_CreateNewDiscussion.webpart" };
                    string[] threadWebParts = { "Akumina.WebParts.DiscussionBoard_DiscussionThreadPage.webpart" };
                    string[] listingdisWebparts = { "Akumina.WebParts.DiscussionBoard_DiscussionBoardListing.webpart" };

                    string[] AnnouncementDetailsZones = { "FullAnnouncementDetail", "FullAnnouncements" };
                    string[] AnnouncementDetailsWebpart = { "Akumina.WebParts.Announcement_AnnouncementDetail.webpart", "Akumina.WebParts.Announcement_AnnouncementItems.webpart" };

                    string[] LibraryListingZones = { "LibraryListing"};
                    string[] LibraryListingWebpart = { "Akumina.WebParts.LibraryListing_LibraryListing.webpart" };
                    
                    try
                    {
                        AddWebPartToPage(web, propVal + "/SparkHome.aspx", homeWebparts, homeZones);
                        AddWebPartToPage(web, propVal + "/SparkDocuments.aspx", dmsWebparts, dmszones);
                        AddWebPartToPage(web, propVal + "/SparkDiscussionThreads.aspx", threadWebParts, DiscussionZones);
                        AddWebPartToPage(web, propVal + "/SparkDiscussions.aspx", listingdisWebparts, DiscussionZones);

                        AddWebPartToPage(web, propVal + "/SparkNewDiscussion.aspx", newDisWebparts, DiscussionZones);

                        AddWebPartToPage(web, propVal + "/SparkAnnouncementDetail.aspx", AnnouncementDetailsWebpart, AnnouncementDetailsZones);
                        AddWebPartToPage(web, propVal + "/SparkLibraryListing.aspx", LibraryListingWebpart, LibraryListingZones);
                        
                    }
                    catch (Exception ex)
                    { }
                    //try
                    //{
                    //    ConnectWebParts(web);

                    //}
                    //catch (Exception ex)
                    //{ }

                    try
                    {

                        if (web.GetFile(propVal + "/SparkHome.aspx") != null)
                        {
                            SPFolder rootFolder = web.RootFolder;
                            rootFolder.WelcomePage = propVal + "/SparkHome.aspx"; //custom landing page    
                            rootFolder.Update();
                        }
                    }
                    catch (Exception ex)
                    { }
                    try
                    {

                        SetAlert(web, propVal + "/SparkHome.aspx");
                    }
                    catch (Exception ex)
                    { }

                    provisionIDSList(web);

                });
            }
            catch (Exception ex)
            { }

        }

        private void provisionIDSList(SPWeb web)
        {
            try
            {
                //SPWeb web = properties.Feature.Parent as SPWeb;
                using (SPSite site = new SPSite(web.Site.ID))
                {
                    using (SPWeb rootWeb = site.OpenWeb(site.RootWeb.ID))
                    {
                        if (web.ID.ToString() != rootWeb.ID.ToString())
                        {
                            SPSecurity.RunWithElevatedPrivileges(delegate
                            {
                                var IdsList = rootWeb.Lists.TryGetList("AkuminaIDS");
                                if (IdsList != null)
                                {
                                    rootWeb.AllowUnsafeUpdates = true;
                                    List<string> idslist = new List<string>();
                                    //idslist.Add("AkuminaAnnouncementsIDS");
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

                                        itemToAdd["SiteId"] = web.ID.ToString();

                                        itemToAdd.Update();

                                    }
                                    rootWeb.AllowUnsafeUpdates = false;
                                }
                            });
                        }
                    }
                }
            }
            catch
            {

            }

        }

        // Uncomment the method below to handle the event raised before a feature is deactivated.

        public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        {
            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    SPWeb web = (SPWeb)properties.Feature.Parent;
                    string propVal = string.Empty;
                    //if (SPContext.Current.Web.Properties.ContainsKey("AkuminaPageSetting"))
                    //    propVal = SPContext.Current.Web.Properties["AkuminaPageSetting"];

                    //if (string.IsNullOrEmpty(propVal))
                    propVal = "Pages";
                    web.AllowUnsafeUpdates = true;


                    try
                    {
                        if (web.GetFile("SitePages/Home.aspx") != null)
                        {

                            SPFolder rootFolder = web.RootFolder;
                            rootFolder.WelcomePage = "SitePages/Home.aspx"; //custom landing page    
                            rootFolder.Update();
                        }

                    }
                    catch { }
                    try
                    {
                        if (web.GetFile(propVal + "/SparkHome.aspx") != null) web.GetFile(propVal + "/SparkHome.aspx").Item.Delete();
                        if (web.GetFile(propVal + "/SparkDocuments.aspx") != null) web.GetFile(propVal + "/SparkDocuments.aspx").Item.Delete();
                        if (web.GetFile(propVal + "/SparkDiscussionThreads.aspx") != null) web.GetFile(propVal + "/SparkDiscussionThreads.aspx").Item.Delete();
                        if (web.GetFile(propVal + "/SparkDiscussions.aspx") != null) web.GetFile(propVal + "/SparkDiscussions.aspx").Item.Delete();
                        if (web.GetFile(propVal + "/SparkNewDiscussion.aspx") != null) web.GetFile(propVal + "/SparkNewDiscussion.aspx").Item.Delete();
                        if (web.GetFile(propVal + "/SparkAnnouncementDetail.aspx") != null) web.GetFile(propVal + "/SparkAnnouncementDetail.aspx").Item.Delete();
                        if (web.GetFile(propVal + "/SparkLibraryListing.aspx") != null) web.GetFile(propVal + "/SparkLibraryListing.aspx").Item.Delete();

                        web.CustomMasterUrl = web.MasterUrl;
                        web.Update();

                    }
                    catch { }
                    try
                    {
                        SPFolder mPageFolder = web.Folders["_catalogs"].SubFolders["masterpage"];
                        try
                        {
                            if (mPageFolder.Files["AkuminaSparkDMSPageLayout.aspx"] != null)
                            {
                                mPageFolder.Files["AkuminaSparkDMSPageLayout.aspx"].Delete();
                            }
                        }
                        catch (Exception ex)
                        { }
                        try
                        {
                            if (mPageFolder.Files["AkuminaSparkHomePageLayout.aspx"] != null)
                            {
                                mPageFolder.Files["AkuminaSparkHomePageLayout.aspx"].Delete();
                            }
                        }
                        catch (Exception ex)
                        { }
                        SPFolder akumina = mPageFolder.SubFolders["AkuminaSpark"];
                        if (akumina != null)
                        {
                            try
                            {
                                if (akumina.Files["akuminaspark.html"] != null)
                                {
                                    akumina.Files["akuminaspark.html"].Delete();
                                }
                            }
                            catch (Exception ex)
                            { }

                            akumina.Delete();
                        }

                    }
                    catch { }
                    web.AllowUnsafeUpdates = false;


                });
            }
            catch (Exception ex)
            { }
        }


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
        private void AddWebPartToPage(SPWeb web, string pageUrl, string[] webPartNames, string[] zoneIDs)
        {
            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    SPFile page = web.GetFile(pageUrl);
                    page.CheckOut();
                    using (Microsoft.SharePoint.WebPartPages.SPLimitedWebPartManager webPartManager = web.GetLimitedWebPartManager(
                            pageUrl, PersonalizationScope.Shared))
                    {
                        for (int i = 0; i < webPartNames.Length; i++)
                        {
                            try
                            {
                                using (WebPart webPart = CreateWebPart(web, webPartNames[i], webPartManager))
                                {

                                    webPartManager.AddWebPart(webPart, zoneIDs[i], 0);

                                }
                            }
                            catch (Exception ex)
                            {

                            }
                        }
                    }
                    page.CheckIn(String.Empty);
                    page.Publish(String.Empty);

                });

            }
            catch (Exception ex)
            {

            }
        }


        private WebPart CreateWebPart(SPWeb web, string webPartName, Microsoft.SharePoint.WebPartPages.SPLimitedWebPartManager webPartManager)
        {
            WebPart webPart = null;
            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    SPQuery qry = new SPQuery();
                    qry.Query = String.Format(CultureInfo.CurrentCulture, "<Where><Eq><FieldRef Name='FileLeafRef'></FieldRef><Value Type='File'>{0}</Value></Eq></Where>", webPartName);

                    SPList webPartGallery = null;

                    if (null == web.ParentWeb)
                    {
                        webPartGallery = web.GetCatalog(SPListTemplateType.WebPartCatalog);
                    }
                    else
                    {
                        webPartGallery = web.Site.RootWeb.GetCatalog(SPListTemplateType.WebPartCatalog);
                    }

                    SPListItemCollection webParts = webPartGallery.GetItems(qry);

                    XmlReader xmlReader = new XmlTextReader(webParts[0].File.OpenBinaryStream());
                    string errorMsg;
                    webPart = webPartManager.ImportWebPart(xmlReader, out errorMsg);


                });
            }
            catch (Exception ex)
            { }
            return webPart;
        }

        private void ConnectWebParts(SPWeb web)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                string pageUrl = propVal + "/SparkDocuments.aspx";
                try
                {

                    web.AllowUnsafeUpdates = true;
                    SPFile page = web.GetFile(pageUrl);
                    page.CheckOut();

                    using (var mgr = web.GetLimitedWebPartManager(pageUrl, PersonalizationScope.Shared))
                    {
                        WebPart provider = null;
                        WebPart consumer = null;
                        foreach (WebPart wp in mgr.WebParts)
                        {

                            if (wp.Title.ToLower().Contains("grid"))
                                consumer = wp;

                            else if (wp.Title.ToLower().Contains("tree"))
                                provider = wp;

                        }



                        if (consumer == null || provider == null)
                        {


                            return;

                        }
                        else
                        {
                            //Provider connection point

                            var providerConnPoints = mgr.GetProviderConnectionPoints(provider);

                            ProviderConnectionPoint providerConnPoint = null;

                            foreach (ProviderConnectionPoint connPoint in providerConnPoints)
                            {


                                providerConnPoint = connPoint;

                            }



                            //Consumer connection point

                            var consumerConnPoints = mgr.GetConsumerConnectionPoints(consumer);


                            ConsumerConnectionPoint consumerConnPoint = null;

                            foreach (ConsumerConnectionPoint connPoint in consumerConnPoints)
                            {


                                consumerConnPoint = connPoint;

                            }
                            if (providerConnPoint != null && consumerConnPoint != null)
                            {

                                mgr.SPConnectWebParts(provider, providerConnPoint, consumer, consumerConnPoint);



                            }
                        }

                    }
                    page.CheckIn(String.Empty);
                    page.Publish(String.Empty);

                }

                catch (Exception ex)
                {


                    web.AllowUnsafeUpdates = false;

                }


            });



        }

        private void SetAlert(SPWeb web, string pageUrl)
        {
            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    SPFile page = web.GetFile(pageUrl);
                    page.CheckOut();
                    using (Microsoft.SharePoint.WebPartPages.SPLimitedWebPartManager webPartManager = web.GetLimitedWebPartManager(
                            pageUrl, PersonalizationScope.Shared))
                    {
                        for (int i = 0; i < webPartManager.WebParts.Count; i++)
                        {
                            if (webPartManager.WebParts[i].Title.Equals("Content Block"))
                            {
                                WebPart wp = webPartManager.WebParts[i];
                                PropertyInfo[] pinfo = wp.GetType().GetProperties();
                                foreach (PropertyInfo pi in pinfo)
                                {
                                    if (pi.Name == "SiteWideAlertMessage")
                                    {
                                        pi.SetValue(wp, "Edit this webpart to configure alert or content block");
                                    }
                                }
                                webPartManager.SaveChanges(wp);
                                break;
                            }
                        }
                    }
                    page.CheckIn(String.Empty);
                    page.Publish(String.Empty);
                });

            }
            catch (Exception ex)
            { }
        }

    }


}
