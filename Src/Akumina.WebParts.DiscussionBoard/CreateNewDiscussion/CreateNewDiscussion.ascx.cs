using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Web;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.WebControls;
using System.Web.UI.WebControls;
using Akumina.InterAction;
using System.Text;
using System.Text.RegularExpressions;
using Akumina.WebParts.DiscussionBoard.Properties;

namespace Akumina.WebParts.DiscussionBoard.CreateNewDiscussion
{
    [ToolboxItem(false)]
    public partial class CreateNewDiscussion : CreateNewDiscussionBaseWebPart
    {
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]

        public CreateNewDiscussion()
        {


        }

        #region priavteVariables


        private string _discussionListName = string.Empty, _DocumentslistName = string.Empty;
        private ClientPeoplePicker _people = new ClientPeoplePicker();
        private readonly string _attachementLinkField = "AttachmentLinks";

        #endregion

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            setInstructionSet();

            StringBuilder sb = new StringBuilder();
            sb.AppendLine(WriteInitialScript());
            litTop.Text = sb.ToString();


            if (!string.IsNullOrEmpty(DiscussionListName))
            {
                _discussionListName = DiscussionListName;
                litrlTitle.Text = DiscussionTitle;

                if (!string.IsNullOrEmpty(DocumentsListName))
                {
                    _DocumentslistName = DocumentsListName;
                    GetFolderTree();
                }
                AddPeoplePicker(); int Postid;
                if (!Page.IsPostBack && HttpContext.Current.Request.QueryString["DiscussionID"] != null && int.TryParse(HttpContext.Current.Request.QueryString["DiscussionID"].ToString(), out Postid))
                {
                    LoadPostDisucussion(Postid);
                }
            }

        }

        private string WriteInitialScript()
        {
            var sb = new StringBuilder();
            if (!string.IsNullOrEmpty(InstructionSet))
            {
                if (!string.IsNullOrEmpty(RootResourcePath) && !RootResourcePath.ToLower().Contains("http:"))
                {
                    string resourcePathValue = SPContext.Current.Web.Url.TrimEnd('/') + RootResourcePath;

                    var styleSheets = Resources.CSSLinks.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var sheet in styleSheets)
                    {
                        sb.AppendLine("<link rel=\"stylesheet\" href=\"" + resourcePathValue + sheet + "\" />");
                    }

                    var jsFiles = Resources.JSLinks.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var jsFile in jsFiles)
                    {
                        if (jsFile.ToLower().Contains("jquery"))
                    {
                        sb.AppendLine("<script type=\"text/javascript\">");
                        sb.AppendLine("if (typeof jQuery == 'undefined') { document.write(unescape(\"%3Cscript src='" + resourcePathValue + jsFile + "' type='text/javascript'%3E%3C/script%3E\")); }");
                        sb.AppendLine("</script>");
                    }

                    sb.AppendLine("<script type=\"text/javascript\" src=\"" + resourcePathValue + jsFile + "\"></script>");


                    }
                }

                else if (!string.IsNullOrEmpty(RootResourcePath) && RootResourcePath.ToLower().Contains("http:"))
                {
                    var styleSheets = Resources.CSSLinks.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var sheet in styleSheets)
                    {
                        sb.AppendLine("<link rel=\"stylesheet\" href=\"" + RootResourcePath + sheet + "\" />");
                    }

                    var jsFiles = Resources.JSLinks.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var jsFile in jsFiles)
                    {
                        if (jsFile.ToLower().Contains("jquery"))
                        {
                            sb.AppendLine("<script type=\"text/javascript\">");
                            sb.AppendLine("if (typeof jQuery == 'undefined') { document.write(unescape(\"%3Cscript src='" + RootResourcePath + jsFile + "' type='text/javascript'%3E%3C/script%3E\")); }");
                            sb.AppendLine("</script>");
                        }

                        sb.AppendLine("<script type=\"text/javascript\" src=\"" + RootResourcePath + jsFile + "\"></script>");

                    }
                }
            }
            else
            {
                string resourcePathValue = SPContext.Current.Web.Url.TrimEnd('/') + "/_layouts/15/Akumina.WebParts.DiscussionBoard";

                var styleSheets = Resources.CSSLinks.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var sheet in styleSheets)
                {
                    sb.AppendLine("<link rel=\"stylesheet\" href=\"" + resourcePathValue + sheet + "\" />");
                }

                var jsFiles = Resources.JSLinks.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var jsFile in jsFiles)
                {
                    if (jsFile.ToLower().Contains("jquery"))
                    {
                        sb.AppendLine("<script type=\"text/javascript\">");
                        sb.AppendLine("if (typeof jQuery == 'undefined') { document.write(unescape(\"%3Cscript src='" + resourcePathValue + jsFile + "' type='text/javascript'%3E%3C/script%3E\")); }");
                        sb.AppendLine("</script>");
                    }

                    sb.AppendLine("<script type=\"text/javascript\" src=\"" + resourcePathValue + jsFile + "\"></script>");


                }
            }

            return sb.ToString();
        }
        void setInstructionSet()
        {
            if (!string.IsNullOrEmpty(InstructionSet))
            {
                InstructionRepository clientServices = new InstructionRepository();
                var response = clientServices.Execute(InstructionSet);
                if (response != null && response.Dictionary != null)
                {
                    RootResourcePath = clientServices.GetValue(response.Dictionary, "RootResourcePath", RootResourcePath);
                    DiscussionListName = clientServices.GetValue(response.Dictionary, "DiscussionListName", DiscussionListName);//response.Dynamic.ListName);
                    //HeaderName = clientServices.GetValue(response.Dictionary, Resources.colName_Grid_Header, HeaderName);//response.Dynamic.HeaderName;
                    DiscussionTitle = clientServices.GetValue(response.Dictionary, "DiscussionTitle", DiscussionTitle);
                    DocumentsListName = clientServices.GetValue(response.Dictionary, "DocumentLibraryName", DocumentsListName);
                    DiscussionListPageUrl = clientServices.GetValue(response.Dictionary, "DiscussionListPageUrl", DiscussionListPageUrl);
                    DiscussionCreatePageUrl = clientServices.GetValue(response.Dictionary, "DiscussionCreatePageUrl", DiscussionCreatePageUrl);
                    DiscussionThreadPageUrl = clientServices.GetValue(response.Dictionary, "DiscussionThreadPageUrl", DiscussionThreadPageUrl);
                }
            }
        }
        private void LoadPostDisucussion(int ID)
        {
            if (!string.IsNullOrEmpty(_discussionListName))
            {
                SPUser oCurrentUser = SPContext.Current.Web.CurrentUser;

                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    using (SPSite oSite = new SPSite(SPContext.Current.Site.ID))
                    {
                        using (SPWeb oWeb = oSite.OpenWeb(SPContext.Current.Web.ID))
                        {
                            //SPSite oSite = new SPSite(SPContext.Current.Site.ID);
                            //SPWeb oWeb = oSite.OpenWeb(SPContext.Current.Web.ID);
                            SPList oList = null;
                            oList = oWeb.Lists.TryGetList(_discussionListName);
                            if (oList != null)
                            {
                                string oTitle = string.Empty, oBody = string.Empty, oLinks = string.Empty;

                                SPQuery oQuery = new SPQuery();
                                oQuery.Query = "<Where><Eq><FieldRef Name=\"ID\"></FieldRef><Value Type=\"Integer\">" + ID.ToString(CultureInfo.InvariantCulture) + "</Value></Eq></Where>";
                                SPListItemCollection collListItems = oList.GetItems(oQuery);
                                if (collListItems.Count > 0)
                                {
                                    SPListItem getTopic = collListItems[0];
                                    oTitle = getTopic["Title"] != null ? getTopic["Title"].ToString() : "";
                                    oBody = getTopic["Body"] != null ? getTopic["Body"].ToString() : "";
                                    titleDiscussion.Value = oTitle;//Assign to title Field
                                    bodyDiscussion.Text = oBody;//Assign body text to html editor
                                    if (getTopic.Fields.ContainsField(_attachementLinkField))
                                        oLinks = getTopic[_attachementLinkField] != null ? getTopic[_attachementLinkField].ToString() : "";
                                    if (!string.IsNullOrEmpty(oLinks))
                                    {
                                        string listOFFilesHtml = string.Empty, listOfFilesHnVal = string.Empty;
                                        string[] oLinksArr = oLinks.Split('|');

                                        var query = from p in oLinksArr
                                                    group p.Split(':')[1] by p.Split(':')[0] into g
                                                    select new
                                                    {
                                                        ListName = g.Key,
                                                        IDs = g.ToList()
                                                    };
                                        foreach (var list in query)
                                        {

                                            SPList oListDoc = null;
                                            oListDoc = oWeb.Lists.TryGetList(list.ListName);
                                            if (oListDoc != null)
                                            {
                                                foreach (var id in list.IDs)
                                                {
                                                    if (id == "0")
                                                    {
                                                        listOFFilesHtml += String.Format("<span class=\"ia-discussion-add-reply-action-btn\" item-id=\"{0}\" list-name=\"{1}\"><span class=\"fa fa-times-circle\" onclick=\"fileRemove(this);\"></span><a href=\"{2}\" class=\"ia-conference-invite\">{3}</a></span>", id, list.ListName, oListDoc.RootFolder.ServerRelativeUrl, list.ListName).ToString();
                                                        listOfFilesHnVal += list.ListName.ToString() + ":" + "0" + "|";
                                                    }

                                                    else
                                                    {
                                                        SPListItem item = oListDoc.GetItemById(int.Parse(id));
                                                        if (item != null)
                                                        {
                                                            listOFFilesHtml += String.Format("<span class=\"ia-discussion-add-reply-action-btn\" item-id=\"{0}\" list-name=\"{1}\"><span class=\"fa fa-times-circle\" onclick=\"fileRemove(this);\"></span><a href=\"{2}\" class=\"ia-conference-invite\">{3}</a></span>", id, list.ListName, item.File != null ? item.File.ServerRelativeUrl : item.Folder.ServerRelativeUrl, item.Name).ToString();
                                                            listOfFilesHnVal += list.ListName.ToString() + ":" + id + "|";
                                                        }
                                                    }
                                                }

                                            }
                                        }
                                        listOfFiles.InnerHtml = listOFFilesHtml;
                                        listOfFilesHnSec.InnerHtml = listOfFilesHnVal;
                                    }

                                    if (getTopic.HasUniqueRoleAssignments && getTopic.RoleAssignments.Count > 0)
                                    {
                                        List<PickerEntity> entityArrayList = new List<PickerEntity>();
                                        foreach (SPRoleAssignment role in getTopic.RoleAssignments)
                                        {
                                            PickerEntity entity = new PickerEntity();
                                            entity.Key = role.Member.LoginName;
                                            entityArrayList.Add(entity);
                                        }
                                        _people.AddEntities(entityArrayList.ToArray().ToList());

                                    }
                                }

                            }
                        }
                    }
                });
            }

        }

        private void AddPeoplePicker()
        {
            //peoplePicker.Controls.Clear();
            //peoplePicker.DataBind();
            //_people = new ClientPeoplePicker();
            //_people.ID = "people";
            //_people.AllowMultipleEntities = true;

            //_people.PrincipalAccountType = "User,SPGroup";
            ////_people.PrincipalAccountType = "SecGroup";



            //peoplePicker.Controls.Add(_people);
            GetUsers();
        }

        private void CreateNewDiscussionBoard(string oTitle, string oBody)
        {
            if (!string.IsNullOrEmpty(_discussionListName))
            {
                SPUser oCurrentUser = SPContext.Current.Web.CurrentUser;

                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    using (SPSite oSite = new SPSite(SPContext.Current.Site.ID))
                    {
                        using (SPWeb oWeb = oSite.OpenWeb(SPContext.Current.Web.ID))
                        {
                            //SPSite oSite = new SPSite(SPContext.Current.Site.ID);
                            //SPWeb oWeb = oSite.OpenWeb(SPContext.Current.Web.ID);

                            SPList oList = null;
                            oList = oWeb.Lists.TryGetList(_discussionListName);
                            if (oList != null)
                            {
                                bool userPermission = oList.DoesUserHavePermissions(oCurrentUser, SPBasePermissions.AddListItems);
                                if (userPermission)
                                {
                                    oWeb.AllowUnsafeUpdates = true;
                                    int Postid = 0;
                                    SPListItem newTopic = null; bool oldPost = false;
                                    if (HttpContext.Current.Request.QueryString["DiscussionID"] != null && int.TryParse(HttpContext.Current.Request.QueryString["DiscussionID"].ToString(), out Postid))
                                    {
                                        newTopic = oList.GetItemById(Postid);
                                        oldPost = true;
                                    }
                                    else
                                    {
                                        newTopic = SPUtilityExt.CreateNewDiscussion(oList, oTitle);
                                        //newTopic = SPUtility.CreateNewDiscussion(oList, oTitle);
                                    }
                                    newTopic["Body"] = oBody;
                                    if (newTopic.Fields.ContainsField("ConfirmArchive"))
                                        newTopic["ConfirmArchive"] = "No";
                                    if (newTopic.Fields.ContainsField(_attachementLinkField))
                                        newTopic[_attachementLinkField] = listOfFilesHn.Value.TrimEnd('|');

                                    newTopic.Update();

                                    List<ListItem> selectedUsers = userAndUserGroups.Items.Cast<ListItem>().Where(x => x.Selected).ToList();
                                    if (selectedUsers.Count > 0)
                                    {
                                        if (!newTopic.HasUniqueRoleAssignments)
                                        {
                                            newTopic.BreakRoleInheritance(false); // Ensure we don't inherit permissions from parent
                                        }

                                        SPRoleAssignmentCollection raCollection = newTopic.RoleAssignments;
                                        //remove exisiting permissions one by one
                                        if (raCollection.Count > 0)
                                            for (int a = raCollection.Count - 1; a >= 0; a--)
                                            {
                                                raCollection.Remove(a);
                                            }

                                        //grant permissions for specific list item

                                        SPRoleDefinition roleDefintion = oWeb.RoleDefinitions.GetByType(SPRoleType.Reader);
                                        if (oCurrentUser.ID != oWeb.Site.SystemAccount.ID && !oCurrentUser.IsSiteAdmin)
                                        {
                                            SPRoleDefinition roleDefintionOwner = oWeb.RoleDefinitions.GetByType(SPRoleType.Administrator);
                                            SPRoleAssignment roleAssignment = new SPRoleAssignment((SPPrincipal)oCurrentUser);
                                            roleAssignment.RoleDefinitionBindings.Add(roleDefintionOwner);
                                            newTopic.RoleAssignments.Add(roleAssignment);
                                        }
                                        foreach (ListItem entity in selectedUsers)
                                        {
                                            try
                                            {
                                                if (!entity.Value.StartsWith("^"))
                                                {
                                                    SPUser user = oWeb.EnsureUser(entity.Value);
                                                    if (user != null)
                                                    {
                                                        SPRoleAssignment roleAssignmentUser = new SPRoleAssignment((SPPrincipal)user);
                                                        roleAssignmentUser.RoleDefinitionBindings.Add(roleDefintion);
                                                        newTopic.RoleAssignments.Add(roleAssignmentUser);
                                                    }
                                                }
                                                else
                                                {
                                                    var groupName = entity.Value.TrimStart('^');
                                                    SPGroup group = oWeb.SiteGroups[groupName];
                                                    if (group != null)
                                                    {
                                                        SPRoleAssignment roleAssignmentGroup = new SPRoleAssignment((SPPrincipal)group);
                                                        roleAssignmentGroup.RoleDefinitionBindings.Add(roleDefintion);
                                                        newTopic.RoleAssignments.Add(roleAssignmentGroup);
                                                    }
                                                }


                                            }
                                            catch (Exception)
                                            {
                                                // ignored
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (newTopic.HasUniqueRoleAssignments)
                                            newTopic.ResetRoleInheritance();
                                    }

                                    SPFieldUserValue oUser = new SPFieldUserValue(oWeb, oCurrentUser.ID, oCurrentUser.LoginName);
                                    if (!oldPost)
                                        newTopic["Author"] = oUser; // Created By                                
                                    newTopic["Editor"] = oUser; // Modified By
                                    newTopic.Update();
                                    oWeb.AllowUnsafeUpdates = false;


                                }
                            }
                        }
                    }
                });
            }
            if (!string.IsNullOrEmpty(DiscussionListPageUrl))
                HttpContext.Current.Response.Redirect(DiscussionListPageUrl);
        }

        protected void postDisucssion_Click(object sender, EventArgs e)
        {
            string oTitle = string.Empty, oBody = string.Empty;
            oTitle = titleDiscussion.Value.Trim();

            Regex rgx = new Regex(@"[~`!@#$%^&*()+=|\\{}':;.,<>/?[\]""_-]");
            bool blnContainsSpecialCharacters = rgx.IsMatch(oTitle.ToString());
            if (!blnContainsSpecialCharacters)
            {
                oBody = bodyDiscussion.Text;
                if (!string.IsNullOrEmpty(oTitle))
                    CreateNewDiscussionBoard(oTitle, oBody);
                else
                {
                    titleDiscussion.Style.Add("border-color", "#FF0000");
                    Label DiscussionTitlelbl = new Label();
                    DiscussionTitlelbl.Text = "<script language='javascript'>" + Environment.NewLine + "window.alert('Discussion Title should not be Empty')</script>";
                    Page.Controls.Add(DiscussionTitlelbl);
                }
            }
            else
            {
                titleDiscussion.Style.Add("border-color", "#FF0000");
                Label DiscussionTitlelbl = new Label();
                DiscussionTitlelbl.Text = "<script language='javascript'>" + Environment.NewLine + "window.alert('Special Characters are not allowed in Discussion Title')</script>";
                Page.Controls.Add(DiscussionTitlelbl);
            }
        }

        protected void cancelPostDiscussion_Click(object sender, EventArgs e)
        {
            //ClearFields();
            if (!string.IsNullOrEmpty(DiscussionListPageUrl))
                HttpContext.Current.Response.Redirect(DiscussionListPageUrl);
        }

        private void ClearFields()
        {
            titleDiscussion.Value = string.Empty;
            bodyDiscussion.Text = string.Empty;
            listOfFiles.InnerHtml = string.Empty;
            listOfFilesHn.Value = string.Empty;
            AddPeoplePicker();

        }

        private void GetUsers()
        {
            if (!Page.IsPostBack)
            {
                SPSecurity.RunWithElevatedPrivileges(delegate()
                 {
                     using (SPSite oSite = new SPSite(SPContext.Current.Site.ID))
                     {
                         using (SPWeb oWeb = oSite.OpenWeb(SPContext.Current.Web.ID))
                         {
                             userAndUserGroups.Items.Clear();
                             //User Groups start with symbol "^";
                             List<ListItem> items = new List<ListItem>();
                             ///oWeb.Users.Cast<SPUser>().ToList().Where(x => (oWeb.DoesUserHavePermissions(x.LoginName, SPBasePermissions.ViewPages) || x.IsSiteAdmin)).ToList().ForEach(x => userAndUserGroups.Items.Add(new ListItem() { Text = x.Name, Value = x.LoginName }));
                             oWeb.Groups.Cast<SPGroup>().ToList().ForEach(x => userAndUserGroups.Items.Add(new ListItem() { Text = x.Name, Value = ("^" + x.LoginName) }));
                         }
                     }
                 });
            }

        }
        private void GetFolderTree()
        {
            if (_DocumentslistName != null)
            {
                var finalRes = Utility.GetFullTreeView(_DocumentslistName);
                ltlFolderInfo.Text = finalRes.ToString();
            }
        }
    }


    public static class SPUtilityExt
    {

        internal static void PrepareNewDiscussionItem(SPListItem item, string title)
        {

            Random RandomClass = new Random();

            SPList parentList = item.ParentList;

            item["Title"] = title;

            item["ContentType"] = parentList.ContentTypes["AkuminaDiscussionBoardContentType"].Name; // set custom Discussion content type

            item["ThreadIndex"] = SPUtility.CreateThreadIndex(null, DateTime.UtcNow);

            item["MessageId"] = SPUtilityExt.GenerateMessageId();

            item["Body"] = "<p>" + title + "</p>";

            //item["BaseName"] = fileAndFolderName + " " + RandomClass.Next(9999);

        }

        public static SPListItem CreateNewDiscussion(SPList list, string title)
        {

            if (list == null)
            {

                throw new ArgumentNullException("list");

            }

            SPListItem item = list.AddItem(list.RootFolder.ServerRelativeUrl, SPFileSystemObjectType.Folder);

            PrepareNewDiscussionItem(item, title);

            return item;

        }

        internal static string GenerateMessageId()
        {

            string str = "SharePoint";

            int capacity = 0x23 + str.Length;

            StringBuilder builder = new StringBuilder(capacity);

            builder.Append('<');

            builder.Append(Guid.NewGuid().ToString("N"));

            builder.Append('@');

            builder.Append(str);

            builder.Append('>');

            return builder.ToString();

        }

        public static SPListItem CreateNewDiscussionReply(SPListItem parent, string replyMessage)
        {

            SPListItem item;

            if ((parent == null) || (parent.ListItems == null))
            {

                throw new ArgumentNullException();

            }

            string str = (string)parent["ThreadIndex"];

            if (string.IsNullOrEmpty(str))
            {

                throw new ArgumentException();

            }

            SPList parentList = parent.ParentList;

            bool flag = parent.FileSystemObjectType == SPFileSystemObjectType.Folder;

            string folderUrl = parent["FileRef"].ToString();

            if (flag)
            {

                item = parent.ListItems.Add(folderUrl, SPFileSystemObjectType.File);

                item["ParentFolderId"] = parent.ID;

            }

            else
            {

                item = parent.ListItems.Add("/" + SPUtility.GetUrlDirectory(folderUrl), SPFileSystemObjectType.File);

                item["ParentFolderId"] = parent["ParentFolderId"];

            }

            SPContentType type = parentList.ContentTypes["CustomMessage"];

            item["ContentType"] = type.Name;

            item["MessageId"] = GenerateMessageId();

            item["ThreadIndex"] = SPUtility.CreateThreadIndex(str, DateTime.UtcNow);

            item["Body"] = "<div>"

            + replyMessage

            + "<br /> <br />"

            + "<hr class=\"ms-disc-quotedtext\" />"

            + "<b>From: </b>" + parent["Author"].ToString().Split('#')[1] + "<br />"

            + "<b>Posted: </b>" + parent["Created"] + "<br />"

            + "<b>Subject: </b>" + parent["Subject"] + "<br />"

            + "<br />"

            + parent["Body"] + "</div>";

            return item;

        }

    }

}
