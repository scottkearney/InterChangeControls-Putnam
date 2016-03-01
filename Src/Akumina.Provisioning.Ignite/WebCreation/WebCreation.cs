using System;
using System.Web.UI.WebControls.WebParts;
using System.Xml;
using System.Globalization;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using Microsoft.Office.Server.Search.WebControls;
using Microsoft.Office.Server.Search;
using Microsoft.Office.Server.Search.Administration;
using Microsoft.Office.Server.Search.Administration.Query;
using Microsoft.SharePoint.Administration;
using Akumina.InterAction;

namespace Akumina.Provisioning.Ignite.WebCreation
{
    /// <summary>
    /// Web Events
    /// </summary>
    public class WebCreation : SPWebEventReceiver
    {
        //Add to the Pages List
        private string propVal = "Pages";

        /// <summary>
        /// A site collection is being deleted.
        /// </summary>
        public override void SiteDeleting(SPWebEventProperties properties)
        {
            base.SiteDeleting(properties);
        }

        /// <summary>
        /// A site is being deleted.
        /// </summary>
        public override void WebDeleting(SPWebEventProperties properties)
        {
            base.WebDeleting(properties);
        }

        /// <summary>
        /// A site is being moved.
        /// </summary>
        public override void WebMoving(SPWebEventProperties properties)
        {
            base.WebMoving(properties);
        }

        /// <summary>
        /// A site is being provisioned.
        /// </summary>
        public override void WebAdding(SPWebEventProperties properties)
        {
            base.WebAdding(properties);
        }

        /// <summary>
        /// A site collection was deleted.
        /// </summary>
        public override void SiteDeleted(SPWebEventProperties properties)
        {
            base.SiteDeleted(properties);
        }

        /// <summary>
        /// A site was deleted.
        /// </summary>
        public override void WebDeleted(SPWebEventProperties properties)
        {
            base.WebDeleted(properties);
        }

        /// <summary>
        /// A site was moved.
        /// </summary>
        public override void WebMoved(SPWebEventProperties properties)
        {
            base.WebMoved(properties);
        }

        /// <summary>
        /// A site was provisioned.
        /// </summary>
        public override void WebProvisioned(SPWebEventProperties properties)
        {
            base.WebProvisioned(properties);
            if (properties.Web.WebTemplate == "IGNITESITEDEFINITION")
            {
                ProvisionDepartment(properties);
            }
            
        }

        private void ProvisionDepartment(SPWebEventProperties properties)
        {            
                     
            //Set Web Part Lists to Department Lists
            try
            {
                ConfigureWebPartLists(properties.Web, propVal + "/Home.aspx");
                ConfigureWebPartLists(properties.Web, propVal + "/Documents.aspx");
                ConfigureWebPartLists(properties.Web, propVal + "/DiscussThread.aspx");
                ConfigureWebPartLists(properties.Web, propVal + "/DiscussList.aspx");
                ConfigureWebPartLists(properties.Web, propVal + "/DiscussNew.aspx");
                ConfigureWebPartLists(properties.Web, propVal + "/NewsDetail.aspx");
                ConfigureWebPartLists(properties.Web, propVal + "/NewsList.aspx");
                ConfigureWebPartLists(properties.Web, propVal + "/Search.aspx");

            }
            catch (Exception)
            {
                // ignored
            }

            //Set Custom Landing Page for Subsite
            try
            {
                if (properties.Web.GetFile(propVal + "/Home.aspx") != null)
                {
                    //Get Root of subsite
                    SPFolder rootFolder = properties.Web.RootFolder;
                    rootFolder.WelcomePage = propVal + "/Home.aspx";
                    rootFolder.Update();
                }
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
                    page.CheckOut();
                    Microsoft.SharePoint.WebPartPages.SPLimitedWebPartManager mgr = web.GetLimitedWebPartManager(url, System.Web.UI.WebControls.WebParts.PersonalizationScope.Shared);
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

    }

}