using System;
using System.Runtime.InteropServices;
using Microsoft.SharePoint;
using System.Web.UI.WebControls.WebParts;
using System.Xml;
using System.Globalization;
using System.Diagnostics;
using Akumina.InterAction;
using Microsoft.Office.Server.Search.WebControls;
using Microsoft.Office.Server.Search.Administration;
using Microsoft.SharePoint.Administration;


namespace Akumina.Provisioning.Ignite.Features.IgniteStarterSite
{
    /// <summary>
    /// This class handles events raised during feature activation, deactivation, installation, uninstallation, and upgrade.
    /// </summary>
    /// <remarks>
    /// The GUID attached to this class may be used during packaging and should not be modified.
    /// </remarks>
    [Guid("28f827f5-d45e-48ef-b723-02f4ac52eea1")]
    public class IgniteStarterSiteEventReceiver : SPFeatureReceiver
    {
        private string _propVal = "Pages";

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

                    // Define web parts and web part zones so the web parts can populate the appropriate sections on pages
                    //Uncomment webparts as appropriate pages are created

                    string[] dmsWebparts = { "AkuminaInterActionDocumentFolderTree.webpart", "AkuminaInterActionDocumentRefiner.webpart", "AkuminaInterActionDocumentGrid.webpart", "AkuminaInterActionDocumentTab.webpart" };
                    string[] dmszones = { "WebPartZone1", "WebPartZone2", "WebPartZone3" };


                    string[] homeZones = { "WebPartZone1", "WebPartZone2" };
                    string[] homeWebparts = { "Akumina.WebParts.DocumentSummaryList_DocumentSummaryList.webpart", "Akumina.WebParts.Announcement_AnnouncementItems.webpart", "AkuminaInterActionBanner.webpart", "Akumina.WebParts.Miscellaneous_PlaceHolder.webpart", "Akumina.WebParts.ImportantDates_ImportantDates.webpart", "Akumina.WebParts.QuickLinks_ListItem.webpart" };

                    //string[] DiscussionZones = { "FullPage" };
                    //string[] newDisWebparts = { "Akumina.WebParts.DiscussionBoard_CreateNewDiscussion.webpart" };
                    //string[] threadWebParts = { "Akumina.WebParts.DiscussionBoard_DiscussionThreadPage.webpart" };
                    //string[] listingdisWebparts = { "Akumina.WebParts.DiscussionBoard_DiscussionBoardListing.webpart" };

                    string[] announcementDetailsZones = { "FullAnnouncementDetail", "FullAnnouncements" };
                    string[] announcementDetailsWebpart = { "Akumina.WebParts.Announcement_AnnouncementDetail.webpart", "Akumina.WebParts.Announcement_AnnouncementItems.webpart" };

                    string[] announcementListingZones = { "Announcements" };
                    string[] announcementListingWebpart = { "Akumina.WebParts.Announcement_AnnouncementItems.webpart" };
                    string[] searchZones = { "Results", "Refiner" };
                    string[] searchWebParts = { "ResultScript.webpart", "RefinementScript.webpart" };

                    try
                    {
                        AddWebPartToPage(web, _propVal + "/Search.aspx", searchWebParts, searchZones, "search");
                        AddWebPartToPage(web, _propVal + "/NewsList.aspx", announcementListingWebpart, announcementListingZones, "newsList");
                        AddWebPartToPage(web, _propVal + "/Home.aspx", homeWebparts, homeZones, "home");
                        AddWebPartToPage(web, _propVal + "/Documents.aspx", dmsWebparts, dmszones, "dms");
                        AddWebPartToPage(web, _propVal + "/NewsDetail.aspx", announcementDetailsWebpart, announcementDetailsZones, "newsDetail");
                    }
                    catch (Exception)
                    {
                        // ignored
                    }
                    //Configure the Search Page
                    try
                    {
                        ConfigureWebPartLists(web, _propVal + "/Search.aspx");
                        ConfigureWebPartLists(web, _propVal + "/Home.aspx");
                        ConfigureWebPartLists(web, _propVal + "/Documents.aspx");
                        ConfigureWebPartLists(web, _propVal + "/NewsDetail.aspx");
                        ConfigureWebPartLists(web, _propVal + "/NewsList.aspx");

                    }
                    catch (Exception)
                    {
                        // ignored
                    }

                    try
                    {

                        if (web.GetFile(_propVal + "/Home.aspx") != null)
                        {
                            SPFolder rootFolder = web.RootFolder;
                            rootFolder.WelcomePage = _propVal + "/Home.aspx"; //custom landing page                            
                            rootFolder.Update();
                        }
                    }
                    catch (Exception)
                    {
                        // ignored
                    }
                    //Connect DMS WebParts
                    try
                    {
                        ConnectWebParts(web);
                    }
                    catch (Exception)
                    {
                        // ignored
                    }

                    //Update the root site QuickLinks_AK and MainMenu_AK lists to reflect current context
                    UpdateQuickLinks(web);

                    //Set up Search and Offline Availability
                    try
                    {

                        web.AllowUnsafeUpdates = true;
                        web.AllowAutomaticASPXPageIndexing = true;
                        web.ASPXPageIndexMode = WebASPXPageIndexMode.Always;
                        web.NoCrawl = false;
                        web.Update();
                        web.AllowUnsafeUpdates = false;
                    }
                    catch (Exception)
                    {
                        // ignored
                    }
                });
            }
            catch (Exception ex)
            {
                //Record error in Application event log
                if (!EventLog.SourceExists("Akumina Ignite"))
                {
                    EventLog.CreateEventSource("Akumina Ignite", "Application");
                }
                EventLog.WriteEntry("Akumina Ignite", ex.Message + "Trace" + ex.StackTrace, EventLogEntryType.Error);
            }

        }

        private static void AddModuleFilesToLibrary(SPWeb web, string xmlfilePath, string moveToUrl, string extensions)
        {
            string[] files = System.IO.Directory.GetFiles(xmlfilePath, extensions, System.IO.SearchOption.AllDirectories);
            var listtempId = web.Lists.Add("IgnitePageLayoutModule_Temp", "IgnitePageLayoutModule_Temp", SPListTemplateType.DocumentLibrary);
            foreach (string file in files)
            {
                SPFolder myLibrary = web.Folders["IgnitePageLayoutModule_Temp"];

                if (!System.IO.File.Exists(file))
                    throw new System.IO.FileNotFoundException("File not found.", file);
                string fileName = System.IO.Path.GetFileName(file);
                System.IO.FileStream fileStream = System.IO.File.OpenRead(file);

                // Upload document
                SPFile spfile = myLibrary.Files.Add(fileName, fileStream, true);

                // Commit 
                myLibrary.Update();
                spfile.MoveTo(moveToUrl + spfile.Name, true);
            }
            if (listtempId != null)
                web.Lists.Delete(listtempId);
        }

        private static void AddWebPartToPage(SPWeb web, string pageUrl, string[] webPartNames, string[] zoneIDs, string pageType)
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
                                        if (i < 3)
                                        {
                                            webPartManager.AddWebPart(webPart, zoneIDs[0], 0);
                                        }
                                        else
                                        {
                                            webPartManager.AddWebPart(webPart, zoneIDs[1], 0);
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
                                        //This is currently specific to the Home.aspx                            
                                        if (i < 1)
                                        {
                                            webPartManager.AddWebPart(webPart, zoneIDs[0], 0);
                                        }
                                        else if (i < 2)
                                        {
                                            webPartManager.AddWebPart(webPart, zoneIDs[1], 0);
                                        }
                                        else
                                        {
                                            webPartManager.AddWebPart(webPart, zoneIDs[2], 0);
                                        }
                                    }
                                }
                                else
                                {
                                    using (WebPart webPart = CreateWebPart(web, webPartNames[i], webPartManager))
                                    {

                                        //We are putting multiple web parts in some web part zones
                                        //Modify this code to change how many webparts go in each zone
                                        //Or if the webPartNames[] param changes
                                        //This is currently specific to the Documents.aspx                                 
                                        //if (i < 1)
                                        //{
                                        //    webPartManager.AddWebPart(webPart, zoneIDs[0], 0);
                                        //}
                                        //else if (i >= 1 && i < 2)
                                        //{
                                        //    webPartManager.AddWebPart(webPart, zoneIDs[1], 0);
                                        //}
                                        //else
                                        //{
                                        //    webPartManager.AddWebPart(webPart, zoneIDs[2], 0);
                                        //}
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

        private static WebPart CreateWebPart(SPWeb web, string webPartName, Microsoft.SharePoint.WebPartPages.SPLimitedWebPartManager webPartManager)
        {
            WebPart webPart = null;
            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    SPQuery qry = new SPQuery
                    {
                        Query = string.Format(CultureInfo.CurrentCulture, "<Where><Eq><FieldRef Name='FileLeafRef'></FieldRef><Value Type='File'>{0}</Value></Eq></Where>", webPartName)
                    };

                    var webPartGallery = null == web.ParentWeb ? web.GetCatalog(SPListTemplateType.WebPartCatalog) : web.Site.RootWeb.GetCatalog(SPListTemplateType.WebPartCatalog);

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

        private void UpdateQuickLinks(SPWeb web)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                //Update Home Page
                SPList topnavList = web.Lists["MainMenu_AK"];
                SPListItem home = topnavList.Items.GetItemById(2);
                SPFieldUrlValue homeField = new SPFieldUrlValue
                {
                    Description = "Root Site Homepage",
                    Url = web.Url + "/Pages/Home.aspx"
                };
                home["Link"] = homeField;
                home.Update();

                ////Create list Item
                //SPList quicklinkList = web.Lists["QuickLinks_AK"];
                //SPListItem newItem = quicklinkList.AddItem();
                //SPFieldUrlValue field = new SPFieldUrlValue
                //{
                //    Description = "The Document Library",
                //    Url = web.Url + "/Pages/Documents.aspx"
                //};

                //newItem["Title"] = "Document Library";
                //newItem["NodeType"] = "Item";
                //newItem["Link"] = field;
                //newItem["DisplayOrder"] = quicklinkList.Items.Count;
                //newItem["ParentItem"] = "1;#IGNITE";
                //newItem["Active"] = "true";
                //newItem["Open_x0020_With"] = "Same Window";
                //newItem.Update();
            });

        }

        private static void ConfigureWebPartLists(SPWeb web, string url)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                SPFile page = web.GetFile(url);
                page.CheckOut();
                Microsoft.SharePoint.WebPartPages.SPLimitedWebPartManager mgr = web.GetLimitedWebPartManager(url, PersonalizationScope.Shared);
                web.AllowUnsafeUpdates = true;

                foreach (var myWebPart in mgr.WebParts)
                {

                    if (((WebPart)myWebPart).Title == "Announcement Items")
                    {

                        myWebPart.GetType().GetProperty("ListName").SetValue(myWebPart, "CompanyNews_AK");
                        myWebPart.GetType().GetProperty("Icon").SetValue(myWebPart, Icons.Newspaper);
                        myWebPart.GetType().GetProperty("Title").SetValue(myWebPart, "Company News");
                        

                        if (page.Name == "NewsList.aspx")
                        {
                            myWebPart.GetType().GetProperty("DisplayTemplate").SetValue(myWebPart, DisplayTemplate.PageTemplate);
                            myWebPart.GetType().GetProperty("InstructionSet").SetValue(myWebPart, "CompanyNewsIDS_AK.2");
                        }
                        else
                        {                            
                            myWebPart.GetType().GetProperty("ItemsToDisplay").SetValue(myWebPart, 5);
                            myWebPart.GetType().GetProperty("InstructionSet").SetValue(myWebPart, "CompanyNewsIDS_AK.1");
                        }
                        mgr.SaveChanges((WebPart)myWebPart);
                        web.Update();

                    }
                    else if (((WebPart)myWebPart).Title == "Announcement Detail")
                    {
                        myWebPart.GetType().GetProperty("ListName").SetValue(myWebPart, "CompanyNews_AK");
                        myWebPart.GetType().GetProperty("InstructionSet").SetValue(myWebPart, "CompanyNewsIDS_AK.1");
                        myWebPart.GetType().GetProperty("Title").SetValue(myWebPart, "Company News");
                        mgr.SaveChanges((WebPart)myWebPart);
                        web.Update();
                    }
                    else if (((WebPart)myWebPart).Title == "Important Dates")
                    {

                        myWebPart.GetType().GetProperty("Icon").SetValue(myWebPart, Icons.Calendar);
                        mgr.SaveChanges((WebPart)myWebPart);
                        web.Update();

                    }
                    else if (((WebPart)myWebPart).Title == "QuickLinks")
                    {
                        myWebPart.GetType().GetProperty("Icon").SetValue(myWebPart, Icons.Link);
                        mgr.SaveChanges((WebPart)myWebPart);
                        web.Update();

                    }
                    else if (((WebPart)myWebPart).Title == "Document Summary List")
                    {
                        myWebPart.GetType().GetProperty("Icon").SetValue(myWebPart, Icons.Files);
                        myWebPart.GetType().GetProperty("Title").SetValue(myWebPart, "Documents");
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
    }
}
