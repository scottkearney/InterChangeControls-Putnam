using System;
using System.Runtime.InteropServices;
using Microsoft.SharePoint;
using System.Web.UI.WebControls.WebParts;
using System.Xml;
using System.Globalization;
using Microsoft.Office.Server.Search.WebControls;
using Microsoft.Office.Server.Search.Administration;
using Microsoft.SharePoint.Administration;
using Akumina.InterAction;

namespace Akumina.Provisioning.IgniteDepartment.Features.IgniteDepartmentSite
{
    /// <summary>
    /// This class handles events raised during feature activation, deactivation, installation, uninstallation, and upgrade.
    /// </summary>
    /// <remarks>
    /// The GUID attached to this class may be used during packaging and should not be modified.
    /// </remarks>

    [Guid("c3cfb210-7823-4f87-9255-22b84322441c")]
    public class IgniteDepartmentSiteEventReceiver : SPFeatureReceiver
    {
        private string _propVal = "Pages";

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
                        catch (Exception)
                        {
                            // ignored
                        }

                        try
                        {
                            ProvisionDepartment(web);
                        }
                        catch (Exception)
                        {
                            // ignored
                        }
                    });
            }
            catch (Exception)
            {
                // ignored
            }
        }

        private static void AddModuleFilesToLibrary(SPWeb web, string xmlfilePath, string moveToUrl, string extensions)
        {
            string[] files = System.IO.Directory.GetFiles(xmlfilePath, extensions, System.IO.SearchOption.AllDirectories);
            var listtempId = web.Lists.Add("IgnitePageLayoutModule_Temp", "IgnitePageLayoutModule_Temp", SPListTemplateType.DocumentLibrary);
            foreach (string file in files)
            {
                SPFolder myLibrary = web.Folders["IgnitePageLayoutModule_Temp"];

                // Prepare to upload
                bool replaceExistingFiles = true;

                if (!System.IO.File.Exists(file))
                    throw new System.IO.FileNotFoundException("File not found.", file);
                string fileName = System.IO.Path.GetFileName(file);
                System.IO.FileStream fileStream = System.IO.File.OpenRead(file);

                // Upload document
                SPFile spfile = myLibrary.Files.Add(fileName, fileStream, replaceExistingFiles);

                // Commit 
                myLibrary.Update();
                spfile.MoveTo(moveToUrl + spfile.Name, true);
            }
            if (listtempId != null)
                web.Lists.Delete(listtempId);
        }

        private void ProvisionDepartment(SPWeb web)
        {

            // Define web parts and web part zones so the web parts can populate the appropriate sections on pages
            //Uncomment webparts as appropriate pages are created

            string[] dmsWebparts = { "Akumina.WebParts.QuickLinks_ListItem.webpart", "AkuminaInterActionDocumentFolderTree.webpart", "AkuminaInterActionDocumentRefiner.webpart", "AkuminaInterActionDocumentGrid.webpart", "AkuminaInterActionDocumentTab.webpart" };
            string[] dmszones = { "DeptNav", "WebPartZone1", "WebPartZone2", "WebPartZone3" };


            string[] homeZones = { "DeptNav", "WebPartZone1", "WebPartZone2" };
            string[] homeWebparts = { "Akumina.WebParts.QuickLinks_ListItem.webpart", "Akumina.WebParts.DocumentSummaryList_DocumentSummaryList.webpart", "Akumina.WebParts.DiscussionBoard_DiscussionSummary.webpart", "Akumina.WebParts.Announcement_AnnouncementItems.webpart", "Akumina.WebParts.ContentBlock_ListItem.webpart", "Akumina.WebParts.ImportantDates_ImportantDates.webpart", "Akumina.WebParts.QuickLinks_ListItem.webpart" };
            string[] discussionZones = { "DeptNav", "FullPage" };
            string[] newDisWebparts = { "Akumina.WebParts.QuickLinks_ListItem.webpart", "Akumina.WebParts.DiscussionBoard_CreateNewDiscussion.webpart" };
            string[] threadWebParts = { "Akumina.WebParts.QuickLinks_ListItem.webpart", "Akumina.WebParts.DiscussionBoard_DiscussionThreadPage.webpart" };
            string[] listingdisWebparts = { "Akumina.WebParts.QuickLinks_ListItem.webpart", "Akumina.WebParts.DiscussionBoard_DiscussionBoardListing.webpart" };

            string[] announcementDetailsZones = { "DeptNav", "FullAnnouncementDetail", "FullAnnouncements" };
            string[] announcementDetailsWebpart = { "Akumina.WebParts.QuickLinks_ListItem.webpart", "Akumina.WebParts.Announcement_AnnouncementDetail.webpart", "Akumina.WebParts.Announcement_AnnouncementItems.webpart" };

            string[] announcementListingZones = { "DeptNav", "Announcements" };
            string[] announcementListingWebpart = { "Akumina.WebParts.QuickLinks_ListItem.webpart", "Akumina.WebParts.Announcement_AnnouncementItems.webpart" };
            //string[] LibraryListingZones = { "LibraryListing" };
            //string[] LibraryListingWebpart = { "Akumina.WebParts.LibraryListing_LibraryListing.webpart" };
            string[] searchZones = { "DeptNav", "Results", "Refiner" };
            string[] searchWebParts = { "Akumina.WebParts.QuickLinks_ListItem.webpart", "ResultScript.webpart", "RefinementScript.webpart" };
            //Add Web Parts to Web Part Zones
            try
            {
                AddWebPartToPage(web, _propVal + "/Home.aspx", homeWebparts, homeZones, "home");
                AddWebPartToPage(web, _propVal + "/Documents.aspx", dmsWebparts, dmszones, "dms");
                AddWebPartToPage(web, _propVal + "/DiscussThread.aspx", threadWebParts, discussionZones, "discuss");
                AddWebPartToPage(web, _propVal + "/DiscussList.aspx", listingdisWebparts, discussionZones, "discuss");

                AddWebPartToPage(web, _propVal + "/DiscussNew.aspx", newDisWebparts, discussionZones, "discuss");

                AddWebPartToPage(web, _propVal + "/NewsDetail.aspx", announcementDetailsWebpart, announcementDetailsZones, "newsDetail");
                AddWebPartToPage(web, _propVal + "/NewsList.aspx", announcementListingWebpart, announcementListingZones, "newsList");
                AddWebPartToPage(web, _propVal + "/Search.aspx", searchWebParts, searchZones, "search");
                //AddWebPartToPage(web, propVal + "/SparkLibraryListing.aspx", LibraryListingWebpart, LibraryListingZones);

            }
            catch (Exception)
            {
                // ignored
            }
            //Set Web Part Lists to Department Lists
            try
            {
                ConfigureWebPartLists(web, _propVal + "/Home.aspx");
                ConfigureWebPartLists(web, _propVal + "/Documents.aspx");
                ConfigureWebPartLists(web, _propVal + "/DiscussThread.aspx");
                ConfigureWebPartLists(web, _propVal + "/DiscussList.aspx");

                ConfigureWebPartLists(web, _propVal + "/DiscussNew.aspx");

                ConfigureWebPartLists(web, _propVal + "/NewsDetail.aspx");
                ConfigureWebPartLists(web, _propVal + "/NewsList.aspx");
                ConfigureWebPartLists(web, _propVal + "/Search.aspx");

            }
            catch (Exception)
            {
                // ignored
            }

            //Set Custom Landing Page for Subsite
            try
            {
                if (web.GetFile(_propVal + "/Home.aspx") != null)
                {
                    //Get Root of subsite
                    SPFolder rootFolder = web.RootFolder;
                    rootFolder.WelcomePage = _propVal + "/Home.aspx";
                    rootFolder.Update();
                }
            }
            catch (Exception)
            {
                // ignored
            }
            //Connect DMS Web Parts
            try
            {
                ConnectWebParts(web);
            }
            catch (Exception)
            {
                // ignored
            }
           
        }


        private void ConfigureWebPartLists(SPWeb web, string url)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                SPFile page = web.GetFile(url);
                bool isDeptNav = true;
                page.CheckOut();
                Microsoft.SharePoint.WebPartPages.SPLimitedWebPartManager mgr = web.GetLimitedWebPartManager(url, PersonalizationScope.Shared);
                web.AllowUnsafeUpdates = true;

                foreach (var myWebPart in mgr.WebParts)
                {

                    if (((WebPart)myWebPart).Title == "Announcement Items")
                    {
                        myWebPart.GetType().GetProperty("ListName").SetValue(myWebPart, "DeptNews_AK");
                        myWebPart.GetType().GetProperty("Icon").SetValue(myWebPart, Icons.Newspaper);


                        //Make changes specific to NewsList.aspx
                        if (page.Name == "NewsList.aspx")
                        {
                            myWebPart.GetType().GetProperty("Title").SetValue(myWebPart, "Department News");
                            myWebPart.GetType().GetProperty("DisplayTemplate").SetValue(myWebPart, DisplayTemplate.PageTemplate);
                            myWebPart.GetType().GetProperty("InstructionSet").SetValue(myWebPart, "DeptNewsIDS_AK.2");
                        }
                        else
                        {
                            myWebPart.GetType().GetProperty("Title").SetValue(myWebPart, web.Title + " News & Announcements");
                            myWebPart.GetType().GetProperty("ItemsToDisplay").SetValue(myWebPart, 5);
                            myWebPart.GetType().GetProperty("InstructionSet").SetValue(myWebPart, "DeptNewsIDS_AK.1");
                        }
                        mgr.SaveChanges((WebPart)myWebPart);
                        web.Update();

                    }
                    else if (((WebPart)myWebPart).Title == "Announcement Detail")
                    {
                        myWebPart.GetType().GetProperty("ListName").SetValue(myWebPart, "DeptNews_AK");
                        myWebPart.GetType().GetProperty("InstructionSet").SetValue(myWebPart, "DeptNewsIDS_AK.1");
                        myWebPart.GetType().GetProperty("Title").SetValue(myWebPart, web.Title + " News & Announcements");
                        mgr.SaveChanges((WebPart)myWebPart);
                        web.Update();
                    }
                    else if (((WebPart)myWebPart).Title == "QuickLinks" && isDeptNav)
                    {
                        myWebPart.GetType().GetProperty("Title").SetValue(myWebPart, web.Title);
                        myWebPart.GetType().GetProperty("QueryPart").SetValue(myWebPart, "DeptMenu_AK");
                        myWebPart.GetType().GetProperty("Directions").SetValue(myWebPart, Directions.TopBottom);
                        //System.Reflection.PropertyInfo querypart = myWebPart.GetType().GetProperty("Query Part");
                        isDeptNav = false;
                        mgr.SaveChanges((WebPart)myWebPart);
                        web.Update();

                    }
                    else if (((WebPart)myWebPart).Title == "QuickLinks" && !isDeptNav)
                    {
                        myWebPart.GetType().GetProperty("Icon").SetValue(myWebPart, Icons.Link);
                        mgr.SaveChanges((WebPart)myWebPart);
                        web.Update();

                    }
                    else if (((WebPart)myWebPart).Title == "Document Summary List")
                    {
                        myWebPart.GetType().GetProperty("Icon").SetValue(myWebPart, Icons.Files);
                        myWebPart.GetType().GetProperty("Title").SetValue(myWebPart, "Documents");
                        myWebPart.GetType().GetProperty("CurrentSite").SetValue(myWebPart, true);
                        mgr.SaveChanges((WebPart)myWebPart);
                        web.Update();

                    }
                    else if (((WebPart)myWebPart).Title == "Content Block")
                    {
                        myWebPart.GetType().GetProperty("Title").SetValue(myWebPart, "");
                        mgr.SaveChanges((WebPart)myWebPart);
                        web.Update();

                    }
                    else if (((WebPart)myWebPart).Title == "Discussion Board - Discussion Summary")
                    {
                        myWebPart.GetType().GetProperty("Title").SetValue(myWebPart, "Discussion Board");
                        myWebPart.GetType().GetProperty("Icon").SetValue(myWebPart, Icons.Comments);
                        myWebPart.GetType().GetProperty("DiscussionTitleTextCount").SetValue(myWebPart, 40);
                        mgr.SaveChanges((WebPart)myWebPart);
                        web.Update();
                    }
                    else if (((WebPart)myWebPart).Title == "Discussion Board - Discussion Listing")
                    {
                        myWebPart.GetType().GetProperty("DiscussionTitle").SetValue(myWebPart, web.Title + " Discussions");
                        myWebPart.GetType().GetProperty("NumberOfPosts").SetValue(myWebPart, 100);
                        myWebPart.GetType().GetProperty("DiscussionTitleTextCount").SetValue(myWebPart, 40);
                        myWebPart.GetType().GetProperty("InstructionSet").SetValue(myWebPart, "");
                        mgr.SaveChanges((WebPart)myWebPart);
                        web.Update();
                    }
                    else if (((WebPart)myWebPart).Title == "Discussion Board - CreateNewDiscussion"
                        || ((WebPart)myWebPart).Title == "Discussion Board - Discussion Thread")
                    {

                        myWebPart.GetType().GetProperty("DiscussionTitle").SetValue(myWebPart, web.Title + " Discussions");
                        mgr.SaveChanges((WebPart)myWebPart);
                        web.Update();

                    }
                    else if (((WebPart)myWebPart).Title == "Important Dates")
                    {

                        myWebPart.GetType().GetProperty("Icon").SetValue(myWebPart, Icons.Calendar);
                        mgr.SaveChanges((WebPart)myWebPart);
                        web.Update();

                    }
                    else if (((WebPart)myWebPart).Title == "Search Results")
                    {
                        //myWebPart.GetType().GetProperty("Query Template").SetValue(myWebPart, "{searchTerms}");

                        //create search results webpart
                        var resultsWebPart = myWebPart as ResultScriptWebPart;
                        resultsWebPart.Title = "Team Search Results";
                        resultsWebPart.ShowPreferencesLink = false;
                        resultsWebPart.ShowAlertMe = false;
                        resultsWebPart.ShowAdvancedLink = false;
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
                            catch (Exception)
                            {
                                // ignored
                            }
                        }
                    }
                    page.CheckIn(string.Empty);
                    page.Publish(string.Empty);

                });

            }
            catch (Exception)
            {
                // ignored
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
                    qry.Query = string.Format(CultureInfo.CurrentCulture, "<Where><Eq><FieldRef Name='FileLeafRef'></FieldRef><Value Type='File'>{0}</Value></Eq></Where>", webPartName);

                    SPList webPartGallery = null;

                    webPartGallery = null == web.ParentWeb ? web.GetCatalog(SPListTemplateType.WebPartCatalog) : web.Site.RootWeb.GetCatalog(SPListTemplateType.WebPartCatalog);

                    SPListItemCollection webParts = webPartGallery.GetItems(qry);

                    XmlReader xmlReader = new XmlTextReader(webParts[0].File.OpenBinaryStream());
                    string errorMsg;
                    webPart = webPartManager.ImportWebPart(xmlReader, out errorMsg);
                });
            }
            catch (Exception)
            {
                // ignored
            }
            return webPart;
        }

        private void ConnectWebParts(SPWeb web)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                string pageUrl = _propVal + "/Documents.aspx";
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
                    page.CheckIn(string.Empty);
                    page.Publish(string.Empty);
                }

                catch (Exception)
                {
                    web.AllowUnsafeUpdates = false;
                }
            });
        }
    }
}
