using System;
using Microsoft.SharePoint;
using System.IO;
using System.Collections.Generic;
using System.Xml;
using System.Linq;
using Microsoft.Office.Server.Search.Administration;
using Microsoft.Office.Server.Search.Administration.Query;
using System.Collections;
namespace Akumina.Provisioning.IgniteLists
{
    class Provision
    {
        public bool IsIgnite { get; set; }

        /// <summary>
        /// Content Type and Site Column Creation
        /// </summary>
        /// <param name="oweb"></param>
        /// <param name="properties"></param>
        public void CreateContentTypes(SPWeb oweb, SPFeatureReceiverProperties properties)
        {

            var xmlfilePath = properties.Definition.RootDirectory + @"\Schemas\ContentTypes.xml";


            var xdoc = new XmlDocument();

            using (var xmlConfigFile = new FileStream(xmlfilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                xdoc.Load(xmlConfigFile);
            }

            var xnodes = xdoc.SelectNodes("/lists/ContentType");

            if (xnodes != null)
            {
                foreach (var node in xnodes.Cast<XmlNode>())
                {
                    string id = "";
                    string name = "";
                    if (node.Attributes["ID"] != null)
                    {
                        id = node.Attributes["ID"].Value;
                    }
                    if (node.Attributes["Name"] != null)
                    {
                        name = node.Attributes["Name"].Value;
                    }

                    SPContentTypeId myContentTypeId = new SPContentTypeId(id);

                    SPContentType myContentType = oweb.Site.RootWeb.ContentTypes[myContentTypeId];

                    if (myContentType == null)
                    {
                        myContentType = new SPContentType(myContentTypeId, oweb.ContentTypes, name);

                        oweb.ContentTypes.Add(myContentType);


                        var datNode = node.SelectNodes("FieldRefs");
                        foreach (var row in datNode.Cast<XmlNode>())
                        {
                            foreach (var subNode in row.Cast<XmlNode>().Where(subNode => subNode.Attributes != null))
                            {
                                string xml = subNode.OuterXml;
                                string fieldName = "";
                                string idField = "";
                                if (subNode.Attributes["Name"] != null)
                                {
                                    fieldName = subNode.Attributes["Name"].Value;
                                }

                                if (!oweb.Fields.ContainsField(fieldName))
                                {
                                    oweb.Fields.AddFieldAsXml(xml, true, SPAddFieldOptions.Default);
                                    if (!myContentType.Fields.ContainsField(fieldName))
                                    {
                                        SPField field = oweb.Fields[fieldName];
                                        SPFieldLink fieldLink = new SPFieldLink(field);
                                        myContentType.FieldLinks.Add(fieldLink);
                                        myContentType.Update(true);
                                    }
                                }
                                else
                                {
                                    if (!myContentType.Fields.ContainsField(fieldName))
                                    {
                                        SPField field = oweb.Fields[fieldName];
                                        SPFieldLink fieldLink = new SPFieldLink(field);
                                        myContentType.FieldLinks.Add(fieldLink);
                                        myContentType.Update(true);
                                    }
                                }
                            }
                        }
                    }
                    oweb.AllowUnsafeUpdates = true;
                }
            }
        }

        /// <summary>
        /// List Creation
        /// </summary>
        /// <param name="oweb"></param>        
        public void CreateLists(SPWeb oweb, string xmlfilePath, bool isRootSite = true)
        {
            string xmlfilePathRoot = xmlfilePath;
            xmlfilePath += isRootSite ? @"\Schemas\Lists.xml" : @"\Schemas\Department.xml";
            var xdoc = new XmlDocument();

            using (var xmlConfigFile = new FileStream(xmlfilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                xdoc.Load(xmlConfigFile);
            }

            var xnodes = xdoc.SelectNodes("/lists/list");

            if (xnodes != null)
            {
                foreach (var node in xnodes.Cast<XmlNode>())
                {
                    var templateId = 100;
                    string contentTypeName = "";
                    bool noCrawl = node.Attributes["noCrawl"] != null;
                    bool titleRequired = node.Attributes["Required"] != null;
                    bool titleEnforce = node.Attributes["Enforce"] != null;
                    
                    string uploadFiles = "";
                    uploadFiles = node.Attributes["filesUpload"] != null ? node.Attributes["filesUpload"].Value : "";

                    contentTypeName = node.Attributes["contentType"] != null ? node.Attributes["contentType"].Value : "";
                    var listName = IsIgnite && node.Attributes["ignite"] != null ? node.Attributes["ignite"].Value : node.Attributes["name"].Value;
                    if (node.Attributes["templateID"] != null)
                    {
                        templateId = Convert.ToInt32(node.Attributes["templateID"].Value);
                    }

                    SPContentTypeId contentTypeId = SPContentTypeId.Empty;
                    SPListTemplateType listTemplate = (SPListTemplateType)templateId;

                    UpdateDocumentsList(oweb, "Documents", uploadFiles, xmlfilePathRoot);

                    if (oweb.Lists.TryGetList(listName) == null)
                    {

                        oweb.AllowUnsafeUpdates = true;
                        oweb.Lists.Add(listName, listName, listTemplate);

                        SPList newList = oweb.Lists[listName];

                        if (titleRequired || titleEnforce)
                        {
                            SPField titleField = newList.Fields.GetField("Title");
                            titleField.Required = titleRequired;
                            titleField.Indexed = titleEnforce;
                            titleField.EnforceUniqueValues = titleEnforce;
                            titleField.Update();
                            newList.Update();
                            oweb.Update();
                        }

                        if (noCrawl)
                        {
                            newList.NoCrawl = noCrawl;
                            newList.Update();
                        }

                        //Remove atachments to all lists.
                        newList.EnableAttachments = false;

                        //Delete the Item content type to when user clicks in add new item uses the new content type;
                        if (!string.IsNullOrEmpty(contentTypeName))
                        {
                            newList.ContentTypes[0].Delete();
                        }
                        newList.Update();


                        //Add Content Type To List instead create columns using XML
                        if (!string.IsNullOrEmpty(contentTypeName))
                        {
                            //Add ContentType to List
                            contentTypeId = AddContentTypeToList(oweb, contentTypeName, contentTypeId, newList);
                        }
                        else // Create Columns using XML
                        {
                            foreach (var subNode in node.Cast<XmlNode>().Where(subNode => subNode.Attributes != null))
                            {
                                if (subNode.Name != "Data")
                                {
                                    string name = subNode.Attributes["Name"] != null ? subNode.Attributes["Name"].Value : subNode.Attributes["DisplayName"].Value;
                                    if (!newList.Fields.ContainsField(name))
                                    {
                                        newList.Fields.AddFieldAsXml(subNode.OuterXml, true, SPAddFieldOptions.Default);
                                    }
                                }
                            }
                        }
                        //ADD Data To List
                        contentTypeId = AddDataToList(node, contentTypeName, contentTypeId, newList, oweb);



                        if (!string.IsNullOrEmpty(uploadFiles))
                        {
                            foreach (string file in uploadFiles.Split(';'))
                            {
                                string fileToUpload = xmlfilePathRoot + @"\ModuleFiles\" + file;
                                if (!File.Exists(fileToUpload))
                                    throw new FileNotFoundException("File not found.", fileToUpload);

                                SPFolder myLibrary = newList.RootFolder;

                                // Prepare to upload
                                Boolean replaceExistingFiles = true;
                                String fileName = Path.GetFileName(fileToUpload);
                                FileStream fileStream = File.OpenRead(fileToUpload);

                                // Upload document
                                SPFile spfile = myLibrary.Files.Add(fileName, fileStream, replaceExistingFiles);

                                // Commit 
                                myLibrary.Update();
                            }
                        }
                        ForceExpiresBeDateTime(newList);
                        oweb.AllowUnsafeUpdates = true;
                    }
                }
            }
            if (isRootSite)
                CreateResultSource(oweb);

        }
        private void CreateResultSource(SPWeb web)
        {

            SPSite publishingSite = new SPSite(web.Site.ID);

            SPServiceContext context = SPServiceContext.GetContext(publishingSite);

            SearchServiceApplicationProxy searchProxy = context.GetDefaultProxy(typeof(SearchServiceApplicationProxy)) as SearchServiceApplicationProxy;

            FederationManager fedManager = new FederationManager(searchProxy);

            SearchObjectOwner owner = new SearchObjectOwner(SearchObjectLevel.SPSite, publishingSite.RootWeb);
            string searchName = "Search Results";

            Source currentResultSource = fedManager.CreateSource(owner);

            currentResultSource.Name = searchName;

            currentResultSource.Description = "Ignite Search";

            currentResultSource.ProviderId = fedManager.ListProviders()["Local SharePoint Provider"].Id;

            Microsoft.Office.Server.Search.Query.Rules.QueryTransformProperties QueryProperties = new Microsoft.Office.Server.Search.Query.Rules.QueryTransformProperties();

            //String resultSourceQuery = "{searchTerms} -Path:bdc3:// IsContainer<>true -Filename:Thumbnails  -Filename:AllItems  -contentclass:STS_ListItem_links -contentclass:STS_List_links -contentclass:STS_List_850";
            String resultSourceQuery = "{searchTerms} -ContentClass=urn:content-class:SPSPeople -filename:AllItems AND ( (IsDocument:True AND -FileExtension:aspx) OR (-IsDocument:True AND FileExtension:aspx AND -Filename:DispForm.aspx))";

            try
            {
                currentResultSource.CreateQueryTransform(QueryProperties, resultSourceQuery);

                currentResultSource.Commit();
            }
            catch (Exception e)
            {
                // ignored
            }
        }



        /// <summary>
        /// Enforce Expires field be Date and Time        
        /// </summary>
        /// <param name="newList"></param>
        private static void ForceExpiresBeDateTime(SPList newList)
        {
            if (newList.Fields.ContainsField("Expires"))
            {
                newList.Fields["Expires"].SchemaXml = "<Field Name=\"Expires\" Format=\"DateTime\"  DisplayName=\"Expires\" Type=\"DateTime\"/>";
                newList.Update();
            }
        }

        /// <summary>
        /// Update Documents list with minor and major versions
        /// </summary>
        /// <param name="oweb"></param>
        private void UpdateDocumentsList(SPWeb oweb, string listName, string uploadFiles, string xmlfilePathRoot)
        {
            SPList list = oweb.Lists.TryGetList(listName);
            if (list != null)
            {
                list.EnableMinorVersions = true;

                //set maximum limits for major/minor versions
                //list.NoCrawl = true;
                list.MajorVersionLimit = 5;
                list.MajorWithMinorVersionsLimit = 10;
                list.Update();

                if (!string.IsNullOrEmpty(uploadFiles))
                {
                    foreach (string file in uploadFiles.Split(';'))
                    {
                        string fileToUpload = xmlfilePathRoot + @"\ModuleFiles\" + file;
                        if (!File.Exists(fileToUpload))
                            throw new FileNotFoundException("File not found.", fileToUpload);

                        SPFolder myLibrary = list.RootFolder;

                        // Prepare to upload
                        Boolean replaceExistingFiles = true;
                        String fileName = Path.GetFileName(fileToUpload);
                        FileStream fileStream = File.OpenRead(fileToUpload);

                        // Upload document
                        SPFile spfile = myLibrary.Files.Add(fileName, fileStream, replaceExistingFiles);

                        // Commit 
                        myLibrary.Update();
                    }
                }
            }

        }
        /// <summary>
        /// Add Content Type to list instead use xml and set it as default 
        /// </summary>
        /// <param name="oweb"></param>
        /// <param name="contentTypeName"></param>
        /// <param name="contentTypeId"></param>
        /// <param name="newList"></param>
        /// <returns></returns>
        private static SPContentTypeId AddContentTypeToList(SPWeb oweb, string contentTypeName, SPContentTypeId contentTypeId, SPList newList)
        {
            newList.ContentTypesEnabled = true;
            newList.Update();
            SPContentType contentType = oweb.Site.RootWeb.ContentTypes[contentTypeName];
            //Check if the list already contains the contenttype
            if (newList.ContentTypes.Cast<SPContentType>().Where(i => i.Name == contentTypeName).Count() < 1)
            {

                if (newList.EntityTypeName == "Discussions_x005f_AKList")
                {
                    
                    newList.ContentTypes.Add(contentType);
                    newList.ContentTypes[0].Delete();
                    newList.Update();
                    SPContentType messageContentType = oweb.Site.RootWeb.ContentTypes["Message"];                    
                    newList.ContentTypes.Add(messageContentType);
                    newList.WriteSecurity = 1;
                    newList.Update();                    
                    
                }
                else {
                    newList.ContentTypes.Add(contentType);
                    newList.Update();
                }
                
            }
            
            contentTypeId = contentType.Id;
            return contentTypeId;
        }

        /// <summary>
        /// Add Data to List
        /// </summary>
        /// <param name="node"></param>
        /// <param name="contentTypeName"></param>
        /// <param name="contentTypeId"></param>
        /// <param name="newList"></param>
        /// <returns></returns>
        private static SPContentTypeId AddDataToList(XmlNode node, string contentTypeName, SPContentTypeId contentTypeId, SPList newListRef, SPWeb oweb)
        {
            SPList newList = oweb.Lists[newListRef.ID];
            var datNode = node.SelectNodes("Data/Rows/Row");

            foreach (var row in datNode.Cast<XmlNode>())
            {
                SPListItem oSpListItem = newList.Items.Add();

                foreach (var subNode in row.Cast<XmlNode>().Where(subNode => subNode.Attributes != null))
                {
                    string title = "";
                    if (subNode.Attributes["Title"] != null) {
                        title = subNode.Attributes["Title"].Value;
                    }
                    if (oSpListItem.Fields.ContainsField(subNode.Attributes["Name"].Value))
                    {
                        string data = subNode.InnerText;
                        if (data.Contains("Token_"))
                        {
                            if (data.Contains("{Token_SiteUrl}"))
                            {
                                data = data.Replace("{Token_SiteUrl}", oweb.Url + ", " + oweb.Site.RootWeb.Title);
                            }                            
                        }
                        if (data.Contains("{0}"))
                        {
                            data = string.Format(data, oweb.Url) + ", " + title;
                        }
                        oSpListItem[subNode.Attributes["Name"].Value] = data;
                    }

                }
                if (!string.IsNullOrEmpty(contentTypeName) && contentTypeId != SPContentTypeId.Empty)
                {
                    oSpListItem["ContentTypeId"] = contentTypeId;
                }
                oSpListItem.Update();
                newList.Update();

            }
            return contentTypeId;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="web"></param>
        public void AllowEveryOnePermission(SPWeb web)
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
                // ignored
            }
        }

        public void ProvisionBannerImage(SPWeb web)
        {
            try
            {
                var list = web.Lists.TryGetList("Banner_AK");
                if (list != null)
                {
                    web.AllowUnsafeUpdates = true;
                    SPListItemCollection bannerListitem = list.Items;
                    string imgUrl = string.Empty;
                    foreach (SPListItem listitem in bannerListitem)
                    {
                        if (listitem != null)
                        {
                            imgUrl = web.Url + "/Images_AK/Sparkbanner-" + listitem["BannerItemOrder"] + ".jpg";
                            listitem["BannerImage"] = imgUrl;
                            listitem.Update();
                        }
                    }
                    web.AllowUnsafeUpdates = false;

                }
            }
            catch
            {
                // ignored
            }
        }

        public void ProvisionIdsList(SPWeb web, bool isRoot = true)
        {
            string siteId = web.ID.ToString();
            var idsList = web.Site.RootWeb.Lists.TryGetList("IDS_AK");
            if (idsList != null)
            {
                web.AllowUnsafeUpdates = true;
                List<string> idslist = new List<string>();
                if (isRoot)
                {
                    idslist.Add("Traffic_WeatherIDS_AK");
                    idslist.Add("BannerIDS_AK");
                    idslist.Add("SiteAlertIDS_AK");
                    //idslist.Add("DocumentsIDS_AK");
                    idslist.Add("CompanyNewsIDS_AK");
                }
                else
                {
                    //idslist.Add("DocumentsIDS_AK");
                    idslist.Add("DeptNewsIDS_AK");
                    //idslist.Add("DiscussionIDS_AK");
                }
                foreach (var idslistitem in idslist)
                {
                    var fieldExist = idsList.Items.Cast<SPListItem>().Where(i => i.Title.Equals(idslistitem) && i["SiteId"].Equals(siteId));
                    if (fieldExist.Count() < 1)
                    {
                        SPListItem itemToAdd = idsList.Items.Add();

                        itemToAdd["Title"] = idslistitem;
                        itemToAdd["ReferenceList"] = idslistitem;
                        itemToAdd["SiteId"] = siteId;
                        switch (idslistitem)
                        {
                            case "BannerIDS_AK":
                                itemToAdd["ColumnOrder"] = "SlideNavigation,MaximumSlideCount,Autoplay,Animation.category-item,Animation.SlideTransition,Animation.SlideDuration,TextSettings.category-item,TextSettings.ColorTheme,TextSettings.Alignment,TextSettings.SubtitleMaxCharacters,TextSettings.ButtonMaxCharacters,TextSettings.MaxCharacters,TextSettings.Location,ResultSourceId,RootResourcePath,InfiniteLoop,QueryType";
                                break;
                            case "Traffic_WeatherIDS_AK":
                                itemToAdd["ColumnOrder"] = "WebPartId,RootResoucePath";
                                break;
                            case "SiteAlertIDS_AK":
                                itemToAdd["ColumnOrder"] = "SiteWideAlertMessage,EnableAlert,RootResourcePath";
                                break;
                            case "DocumentsIDS_AK":
                                itemToAdd["ColumnOrder"] = "ListName,RootResourcePath";
                                break;
                            case "CompanyNewsIDS_AK":
                                itemToAdd["ColumnOrder"] = "Title,ListName,RootResourcePath,ItemsToDisplay";
                                break;
                            case "DiscussionIDS_AK":
                                itemToAdd["ColumnOrder"] = "DiscussionListName,DocumentLibraryName,Listing.category-item,Listing.DisplayAvatarPicture,Listing.NumberOfPosts,Listing.ListingPostType,Summary.category-item,Summary.DisplayAvatarPicture,Summary.NumberOfPosts,Summary.ListingPostType,DiscussionThread.category-item,DiscussionThread.DisplayAvatarPicture,DiscussionTitle,ScriptPath";
                                break;
                            case "DeptNewsIDS_AK":
                                itemToAdd["ColumnOrder"] = "Title,ListName,RootResourcePath,ItemsToDisplay";
                                break;

                        }

                        itemToAdd.Update();
                    }

                }
                web.AllowUnsafeUpdates = false;
            }
        }

        public void ProvisionQuickLinks(SPWeb web)
        {
            try
            {
                var quickLinksList = web.Lists.TryGetList("Quicklinks_AK");
                if (quickLinksList != null)
                {
                    web.AllowUnsafeUpdates = true;
                    SPFieldLookup primaryColumn = (SPFieldLookup)quickLinksList.Fields.GetFieldByInternalName("ParentItem");
                    string strProjectedCol = quickLinksList.Fields.AddDependentLookup("TempCol", primaryColumn.Id);
                    SPFieldLookup projectedCol = (SPFieldLookup)quickLinksList.Fields.GetFieldByInternalName(strProjectedCol);
                    projectedCol.LookupField = quickLinksList.Fields["Title"].InternalName;
                    projectedCol.Hidden = true;
                    projectedCol.Update();

                    SPView view = quickLinksList.DefaultView;
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
                // ignored
            }
        }

        public void ProvisionEnforcingUniqueValues(SPWeb web)
        {
            try
            {
                web.AllowUnsafeUpdates = true;
                List<string> idslist = new List<string>();
                idslist.Add("AkAnnouncementsIDS");
                idslist.Add("AkBannerIDS");
                idslist.Add("AkContentBlockIDS");
                idslist.Add("AkDiscussionsIDS");
                idslist.Add("AkDocumentsIDS");
                idslist.Add("AkMiscIDS");
                idslist.Add("AkSiteLinksIDS");

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
                // ignored
            }
        }
    }
}
