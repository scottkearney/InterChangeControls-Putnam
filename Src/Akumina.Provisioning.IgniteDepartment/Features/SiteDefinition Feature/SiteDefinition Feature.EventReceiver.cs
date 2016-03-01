using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.SharePoint;
using System.Web.UI.WebControls.WebParts;
using System.Xml;
using System.Globalization;
using Microsoft.SharePoint.Utilities;
using Microsoft.Office.Server.Search.WebControls;
using Microsoft.Office.Server.Search;
using Microsoft.Office.Server.Search.Administration;
using Microsoft.Office.Server.Search.Administration.Query;
using Microsoft.SharePoint.Administration;

namespace Akumina.Provisioning.IgniteDepartment.Features.Feature1
{
    /// <summary>
    /// This class handles events raised during feature activation, deactivation, installation, uninstallation, and upgrade.
    /// </summary>
    /// <remarks>
    /// The GUID attached to this class may be used during packaging and should not be modified.
    /// </remarks>

    [Guid("c09235a0-74f9-4ac7-b773-67814d7edd01")]
    public class Feature1EventReceiver : SPFeatureReceiver
    {
        private string propVal = "Pages";

        // Uncomment the method below to handle the event raised after a feature has been activated.

        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate()
                    {
                        SPWeb web = (SPWeb)properties.Feature.Parent;


                        //var xmlfilePath = properties.Definition.RootDirectory + @"\IgnitePageLayoutModule\";
                        //AddModuleFilesToLibrary(web, xmlfilePath, "_catalogs/masterpage/", "*.aspx");

                        var xmlfilePath = properties.Definition.RootDirectory + @"\MasterPageModule\";
                        AddModuleFilesToLibrary(web, xmlfilePath, "_catalogs/masterpage/Ignite/", "*.*");

                        //xmlfilePath = properties.Definition.RootDirectory + @"\PagesModule\";
                        //AddModuleFilesToLibrary(web, xmlfilePath, "Pages/", "*.aspx");

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

                        try
                        {
                            ProvisionDepartment(web);
                        }
                        catch (Exception ex)
                        {

                        }
                    });
            }
            catch (Exception ex)
            { }
        }


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

        private static void AddModuleFilesToLibrary(SPWeb web, string xmlfilePath, string moveToUrl, string extensions)
        {
            string[] files = System.IO.Directory.GetFiles(xmlfilePath, extensions, System.IO.SearchOption.AllDirectories);
            var listtempID = web.Lists.Add("IgnitePageLayoutModule_Temp", "IgnitePageLayoutModule_Temp", SPListTemplateType.DocumentLibrary);
            foreach (string file in files)
            {
                SPFolder myLibrary = web.Folders["IgnitePageLayoutModule_Temp"];

                // Prepare to upload
                Boolean replaceExistingFiles = true;

                if (!System.IO.File.Exists(file))
                    throw new System.IO.FileNotFoundException("File not found.", file);
                String fileName = System.IO.Path.GetFileName(file);
                System.IO.FileStream fileStream = System.IO.File.OpenRead(file);

                // Upload document
                SPFile spfile = myLibrary.Files.Add(fileName, fileStream, replaceExistingFiles);

                // Commit 
                myLibrary.Update();
                spfile.MoveTo(moveToUrl + spfile.Name, true);
            }
            if (listtempID != null)
                web.Lists.Delete(listtempID);
        }

        private void ProvisionDepartment(SPWeb web)
        {   

            // Define web parts and web part zones so the web parts can populate the appropriate sections on pages
            //Uncomment webparts as appropriate pages are created

            string[] dmsWebparts = { "Akumina.WebParts.QuickLinks_ListItem.webpart", "AkuminaInterActionDocumentFolderTree.webpart", "AkuminaInterActionDocumentRefiner.webpart", "AkuminaInterActionDocumentGrid.webpart", "AkuminaInterActionDocumentTab.webpart" };
            string[] dmszones = { "DeptNav", "WebPartZone1", "WebPartZone2", "WebPartZone3" };


            string[] homeZones = { "DeptNav", "WebPartZone1", "WebPartZone2" };
            string[] homeWebparts = { "Akumina.WebParts.QuickLinks_ListItem.webpart", "Akumina.WebParts.DocumentSummaryList_DocumentSummaryList.webpart", "Akumina.WebParts.DiscussionBoard_DiscussionSummary.webpart", "Akumina.WebParts.Announcement_AnnouncementItems.webpart", "Akumina.WebParts.ContentBlock_ListItem.webpart", "Akumina.WebParts.ImportantDates_ImportantDates.webpart", "Akumina.WebParts.QuickLinks_ListItem.webpart" };
            string[] DiscussionZones = { "DeptNav", "FullPage" };
            string[] newDisWebparts = { "Akumina.WebParts.QuickLinks_ListItem.webpart", "Akumina.WebParts.DiscussionBoard_CreateNewDiscussion.webpart" };
            string[] threadWebParts = { "Akumina.WebParts.QuickLinks_ListItem.webpart", "Akumina.WebParts.DiscussionBoard_DiscussionThreadPage.webpart" };
            string[] listingdisWebparts = { "Akumina.WebParts.QuickLinks_ListItem.webpart", "Akumina.WebParts.DiscussionBoard_DiscussionBoardListing.webpart" };

            string[] AnnouncementDetailsZones = { "DeptNav", "FullAnnouncementDetail", "FullAnnouncements" };
            string[] AnnouncementDetailsWebpart = { "Akumina.WebParts.QuickLinks_ListItem.webpart", "Akumina.WebParts.Announcement_AnnouncementDetail.webpart", "Akumina.WebParts.Announcement_AnnouncementItems.webpart" };

            string[] AnnouncementListingZones = { "DeptNav", "Announcements" };
            string[] AnnouncementListingWebpart = { "Akumina.WebParts.QuickLinks_ListItem.webpart", "Akumina.WebParts.Announcement_AnnouncementItems.webpart" };
            //string[] LibraryListingZones = { "LibraryListing" };
            //string[] LibraryListingWebpart = { "Akumina.WebParts.LibraryListing_LibraryListing.webpart" };
            string[] SearchZones = { "DeptNav", "Results", "Refiner" };
            string[] SearchWebParts = { "Akumina.WebParts.QuickLinks_ListItem.webpart", "ResultScript.webpart", "RefinementScript.webpart" };
            //Add Web Parts to Web Part Zones
            try
            {
                AddWebPartToPage(web, propVal + "/Home.aspx", homeWebparts, homeZones, "home");
                AddWebPartToPage(web, propVal + "/Documents.aspx", dmsWebparts, dmszones, "dms");
                AddWebPartToPage(web, propVal + "/DiscussThread.aspx", threadWebParts, DiscussionZones, "discuss");
                AddWebPartToPage(web, propVal + "/DiscussList.aspx", listingdisWebparts, DiscussionZones, "discuss");

                AddWebPartToPage(web, propVal + "/DiscussNew.aspx", newDisWebparts, DiscussionZones, "discuss");

                AddWebPartToPage(web, propVal + "/NewsDetail.aspx", AnnouncementDetailsWebpart, AnnouncementDetailsZones, "newsDetail");
                AddWebPartToPage(web, propVal + "/NewsList.aspx", AnnouncementListingWebpart, AnnouncementListingZones, "newsList");
                AddWebPartToPage(web, propVal + "/Search.aspx", SearchWebParts, SearchZones, "search");
                //AddWebPartToPage(web, propVal + "/SparkLibraryListing.aspx", LibraryListingWebpart, LibraryListingZones);

            }
            catch (Exception ex)
            { }
            //Set Web Part Lists to Department Lists
            try
            {
                ConfigureWebPartLists(web, propVal + "/Home.aspx");
                ConfigureWebPartLists(web, propVal + "/Documents.aspx");
                ConfigureWebPartLists(web, propVal + "/DiscussThread.aspx");
                ConfigureWebPartLists(web, propVal + "/DiscussList.aspx");

                ConfigureWebPartLists(web, propVal + "/DiscussNew.aspx");

                ConfigureWebPartLists(web, propVal + "/NewsDetail.aspx");
                ConfigureWebPartLists(web, propVal + "/NewsList.aspx");
                ConfigureWebPartLists(web, propVal + "/Search.aspx");

            }
            catch (Exception ex)
            { }

            //Set Custom Landing Page for Subsite
            try
            {
                if (web.GetFile(propVal + "/Home.aspx") != null)
                {
                    //Get Root of subsite
                    SPFolder rootFolder = web.RootFolder;
                    rootFolder.WelcomePage = propVal + "/Home.aspx";
                    rootFolder.Update();
                }
            }
            catch (Exception ex)
            { }
            //Connect DMS Web Parts
            try
            {
                ConnectWebParts(web);
            }
            catch (Exception ex)
            { }           

            //Add to quicklinks list
            try
            {
                AddSiteToTopNav(web);
            }
            catch
            { }
        }
        
        
        private void ConfigureWebPartLists(SPWeb web, string url)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                SPFile page = web.GetFile(url);
                bool isDeptNav = true;
                page.CheckOut();
                Microsoft.SharePoint.WebPartPages.SPLimitedWebPartManager mgr = web.GetLimitedWebPartManager(url, System.Web.UI.WebControls.WebParts.PersonalizationScope.Shared);
                web.AllowUnsafeUpdates = true;

                foreach (var myWebPart in mgr.WebParts)
                {

                    if (((System.Web.UI.WebControls.WebParts.WebPart)myWebPart).Title == "Announcement Items")
                    {

                        myWebPart.GetType().GetProperty("ListName").SetValue(myWebPart, "DeptNews_AK");
                        myWebPart.GetType().GetProperty("InstructionSet").SetValue(myWebPart, "DeptNewsIDS_AK.1");

                        //Make changes specific to NewsList.aspx
                        if (page.Name == "NewsList.aspx")
                        {
                            myWebPart.GetType().GetProperty("Title").SetValue(myWebPart, "");
                            myWebPart.GetType().GetProperty("DisplayTemplate").SetValue(myWebPart, DisplayTemplate.PageTemplate);
                        }
                        else
                        {
                            myWebPart.GetType().GetProperty("Title").SetValue(myWebPart, web.Title + " News & Announcements");
                            myWebPart.GetType().GetProperty("ViewAllLink").SetValue(myWebPart, web.Url + "/" + propVal + "/NewsList.aspx");
                        }
                        mgr.SaveChanges((System.Web.UI.WebControls.WebParts.WebPart)myWebPart);
                        web.Update();

                    }
                    else if (((System.Web.UI.WebControls.WebParts.WebPart)myWebPart).Title == "Announcement Detail")
                    {
                        myWebPart.GetType().GetProperty("ListName").SetValue(myWebPart, "DeptNews_AK");
                        myWebPart.GetType().GetProperty("InstructionSet").SetValue(myWebPart, "DeptNewsIDS_AK.1");
                        myWebPart.GetType().GetProperty("Title").SetValue(myWebPart, web.Title + " News & Announcements");
                        mgr.SaveChanges((System.Web.UI.WebControls.WebParts.WebPart)myWebPart);
                        web.Update();
                    }
                    else if (((System.Web.UI.WebControls.WebParts.WebPart)myWebPart).Title == "QuickLinks" && isDeptNav)
                    {
                        myWebPart.GetType().GetProperty("Title").SetValue(myWebPart, web.Title);
                        myWebPart.GetType().GetProperty("QueryPart").SetValue(myWebPart, "DeptMenu_AK");
                        myWebPart.GetType().GetProperty("Directions").SetValue(myWebPart, Directions.TopBottom);
                        //System.Reflection.PropertyInfo querypart = myWebPart.GetType().GetProperty("Query Part");
                        isDeptNav = false;
                        mgr.SaveChanges((System.Web.UI.WebControls.WebParts.WebPart)myWebPart);
                        web.Update();

                    }
                    else if (((System.Web.UI.WebControls.WebParts.WebPart)myWebPart).Title == "Document Summary List")
                    {
                        myWebPart.GetType().GetProperty("CurrentSite").SetValue(myWebPart, true);
                        mgr.SaveChanges((System.Web.UI.WebControls.WebParts.WebPart)myWebPart);
                        web.Update();

                    }
                    else if (((System.Web.UI.WebControls.WebParts.WebPart)myWebPart).Title == "Discussion Board - Discussion Summary")
                    {
                        myWebPart.GetType().GetProperty("Title").SetValue(myWebPart, "Discussion Board");
                        mgr.SaveChanges((System.Web.UI.WebControls.WebParts.WebPart)myWebPart);
                        web.Update();
                    }
                    else if (((System.Web.UI.WebControls.WebParts.WebPart)myWebPart).Title == "Discussion Board - CreateNewDiscussion"
                        || ((System.Web.UI.WebControls.WebParts.WebPart)myWebPart).Title == "Discussion Board - Discussion Listing"
                        || ((System.Web.UI.WebControls.WebParts.WebPart)myWebPart).Title == "Discussion Board - Discussion Thread")
                    {

                        myWebPart.GetType().GetProperty("DiscussionTitle").SetValue(myWebPart, web.Title + " Discussions");
                        mgr.SaveChanges((System.Web.UI.WebControls.WebParts.WebPart)myWebPart);
                        web.Update();

                    }
                    else if (((System.Web.UI.WebControls.WebParts.WebPart)myWebPart).Title == "Search Results")
                    {
                        //myWebPart.GetType().GetProperty("Query Template").SetValue(myWebPart, "{searchTerms}");

                        //create search results webpart
                        var resultsWebPart = myWebPart as ResultScriptWebPart;
                        resultsWebPart.Title = "Team Search Results";
                        var querySettings = new DataProviderScriptWebPart
                        {
                            PropertiesJson = resultsWebPart.DataProviderJSON
                        };

                        //Get the search service application proxy
                        var settingsProxy = SPFarm.Local.ServiceProxies.GetValue
                                <SearchQueryAndSiteSettingsServiceProxy>();
                        var searchProxy = settingsProxy.ApplicationProxies.GetValue
                                <SearchServiceApplicationProxy>("Search Service Application");
                        var siteOwner = new SearchObjectOwner(SearchObjectLevel.SPSite, web);

                        //Set the result source.
                        var siteResultSource = searchProxy.GetResultSourceByName("Search Results", siteOwner);

                        querySettings.Properties["SourceName"] = siteResultSource.Name;
                        querySettings.Properties["SourceID"] = siteResultSource.Id;
                        querySettings.Properties["SourceLevel"] = siteOwner.Level;

                        //setting the search query text
                        querySettings.Properties["QueryTemplate"] = "{searchTerms}";
                        resultsWebPart.DataProviderJSON = querySettings.PropertiesJson;


                        mgr.SaveChanges(resultsWebPart);


                        //mgr.SaveChanges((System.Web.UI.WebControls.WebParts.WebPart)myWebPart);
                        web.Update();
                    }

                }

                page.CheckIn("");
                page.Publish("");
            });
        }

        
        private void AddSiteToTopNav(SPWeb web)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                //Create list Item
                SPList quicklinkList = web.ParentWeb.Lists["MainMenu_AK"];
                SPListItem newItem = quicklinkList.AddItem();
                SPFieldUrlValue field = new SPFieldUrlValue();
                field.Description = web.Description;
                field.Url = web.Url;

                newItem["Title"] = web.Title;
                newItem["NodeType"] = "Item";
                newItem["Link"] = field;
                newItem["DisplayOrder"] = quicklinkList.Items.Count;
                newItem["ParentItem"] = "3;#Department";
                newItem["Active"] = "true";
                newItem["Open_x0020_With"] = "Same Window";
                newItem.Update();
            });

        }

        private void AddWebPartToPage(SPWeb web, string pageUrl, string[] webPartNames, string[] zoneIDs, string pageType)
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
                                if (pageType == "home")
                                {
                                    using (WebPart webPart = CreateWebPart(web, webPartNames[i], webPartManager))
                                    {

                                        //We are putting multiple web parts in some web part zones
                                        //Modify this code to change how many webparts go in each zone
                                        //Or if the webPartNames[] param changes
                                        //This is currently specific to the Home.aspx                            
                                        if (i < 1)
                                        {
                                            webPartManager.AddWebPart(webPart, zoneIDs[0], 0);
                                        }
                                        else if (i < 5)
                                        {
                                            webPartManager.AddWebPart(webPart, zoneIDs[1], 0);
                                        }
                                        else
                                        {
                                            webPartManager.AddWebPart(webPart, zoneIDs[2], 0);
                                        }

                                    }
                                }
                                else if (pageType == "dms")
                                {
                                    using (WebPart webPart = CreateWebPart(web, webPartNames[i], webPartManager))
                                    {

                                        //We are putting multiple web parts in some web part zones
                                        //Modify this code to change how many webparts go in each zone
                                        //Or if the webPartNames[] param changes
                                        //This is currently specific to the Documents.aspx                           
                                        if (i < 1)
                                        {
                                            webPartManager.AddWebPart(webPart, zoneIDs[0], 0);
                                        }
                                        else if (i < 2)
                                        {
                                            webPartManager.AddWebPart(webPart, zoneIDs[1], 0);
                                        }
                                        else if (i < 3)
                                        {
                                            webPartManager.AddWebPart(webPart, zoneIDs[2], 0);
                                        }
                                        else
                                        {
                                            webPartManager.AddWebPart(webPart, zoneIDs[3], 0);
                                        }
                                    }
                                }
                                else
                                {
                                    using (WebPart webPart = CreateWebPart(web, webPartNames[i], webPartManager))
                                    {
                                        webPartManager.AddWebPart(webPart, zoneIDs[i], 0);
                                    }
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
                string pageUrl = propVal + "/Documents.aspx";
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
    }
}
