using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;
using Akumina.WebParts.DiscussionBoard.CreateNewDiscussion;
using Microsoft.Office.Server.UserProfiles;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.WebControls;
using Akumina.InterAction;
using System.Text;
using Akumina.WebParts.DiscussionBoard.Properties;

namespace Akumina.WebParts.DiscussionBoard.DiscussionThreadPage
{
    [ToolboxItem(false)]
    public partial class DiscussionThreadPage : ThreadDiscussionBaseWebPart
    {
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]

        private string _discussionListName = string.Empty, _DocumentslistName = string.Empty;
        private readonly string _attachementLinkField = "AttachmentLinks";
        private bool _hasAdminPermission = false;
        //private ClientPeoplePicker pplpickerPermission = new ClientPeoplePicker();

        public DiscussionThreadPage()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            string propVal = string.Empty;
            if (SPContext.Current.Web.Properties.ContainsKey("AkuminaPageSetting"))
                propVal = SPContext.Current.Web.Properties["AkuminaPageSetting"];

            if (string.IsNullOrEmpty(propVal))
                propVal = "Pages";

            setInstructionSet();

            StringBuilder sb = new StringBuilder();
            sb.AppendLine(WriteInitialScript());
            litTop.Text = sb.ToString();

            if (!string.IsNullOrEmpty(DiscussionListName))
            {
                _discussionListName = DiscussionListName;
                _DocumentslistName = DocumentsListName;
                litrlTitle.Text = !string.IsNullOrEmpty(DiscussionTitle) ? DiscussionTitle : string.Empty;
                if (!Page.IsPostBack)
                {
                    GetFolderTree();
                    getCurrentUserPic();
                    //AddPeoplePicker();
                    LoadThreadDetails(Convert.ToInt16(HttpContext.Current.Request.QueryString["DiscussionID"]));
                }
                if (!string.IsNullOrEmpty(DiscussionListPageUrl) && !DiscussionListPageUrl.ToLower().Contains("http"))
                    discussionListPage.Attributes["href"] = SPContext.Current.Web.Url.TrimEnd('/') + "/" + propVal + "/" + DiscussionListPageUrl;
                else if (DiscussionListPageUrl.ToLower().Contains("http"))
                    discussionListPage.Attributes["href"] = DiscussionListPageUrl;
                else
                    discussionListPage.Attributes["href"] = "#";
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
                    DisplayAvatarPicture = clientServices.GetValue(response.Dictionary, "DiscussionThread.DisplayAvatarPicture", DisplayAvatarPicture == true ? "1" : "0") != "0" ? true : false;
                }
            }
        }
        private void GetUsers(List<string> users)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPSite oSite = new SPSite(SPContext.Current.Site.ID))
                {
                    using (SPWeb oWeb = oSite.OpenWeb(SPContext.Current.Web.ID))
                    {
                        pplpickerPermission.Items.Clear();
                        //User Groups start with symbol "^";                       
                        //oWeb.Users.Cast<SPUser>().ToList().Where(x => (x.Groups.Count != 0 || x.IsSiteAdmin)).ToList().ForEach(x => pplpickerPermission.Items.Add(new ListItem() { Text = x.Name, Value = x.LoginName, Selected = users.Contains(x.LoginName.ToLower().Trim()) }));
                        oWeb.Groups.Cast<SPGroup>().ToList().ForEach(x => pplpickerPermission.Items.Add(new ListItem() { Text = x.Name, Value = ("^" + x.LoginName), Selected = users.Contains(x.LoginName.ToLower()) }));



                    }
                }
            });


        }
        private string StripHTML(string input)
        {
            return Regex.Replace(input, "<.*?>", String.Empty);
        }

        private void LoadThreadDetails(int ID)
        {
            SPUser oCurrentUser = SPContext.Current.Web.CurrentUser;
            string[] oLinksArr;

            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPSite oSite = new SPSite(SPContext.Current.Site.ID))
                {
                    using (SPWeb oWeb = oSite.OpenWeb(SPContext.Current.Web.ID))
                    {
                        SPList oList = null;
                        oList = oWeb.Lists.TryGetList(_discussionListName);

                        if (oList != null)
                        {
                            if (oList.DoesUserHavePermissions(oCurrentUser, SPBasePermissions.AddListItems))
                            {
                                replypanel.Visible = true;
                                addReplyToPost.Visible = true;
                            }
                            string oTitle = string.Empty, oBody = string.Empty, oLinks = string.Empty, oCreatedDate = string.Empty, oCreatedBy = string.Empty;
                            SPFieldUserValue userValue = null;

                            SPQuery oQuery = new SPQuery();
                            oQuery.Query = "<Where><Eq><FieldRef Name=\"ID\"></FieldRef><Value Type=\"Integer\">" + ID.ToString(CultureInfo.InvariantCulture) + "</Value></Eq></Where>";
                            SPListItemCollection collListItems = oList.GetItems(oQuery);
                            if (collListItems.Count > 0)
                            {

                                SPListItem getTopic = collListItems[0];

                                if (getTopic.DoesUserHavePermissions(oCurrentUser, SPBasePermissions.Open))
                                {
                                    oTitle = getTopic["Title"] != null ? getTopic["Title"].ToString() : "";
                                    oBody = getTopic["Body"] != null ? getTopic["Body"].ToString() : "";
                                    oCreatedDate = getTopic["Created"].ToString();
                                    oCreatedBy = getTopic["Author"].ToString();

                                    lblSubject.Text = oTitle;
                                    lblDescription.Text = StripHTML(oBody).Trim() != string.Empty ? oBody : string.Empty;
                                    lblThreadDate.Text = DateTime.Parse(oCreatedDate).ToString("MMM. d, yyyy");
                                    lblAuthorName.Text = oCreatedBy.Split('#')[1];


                                    userValue = new SPFieldUserValue(oWeb, getTopic["Created By"].ToString());
                                    if (HasAdminPermission(oCurrentUser, userValue.LookupValue, oWeb.Site.SystemAccount.ID, oList))
                                    {
                                        _hasAdminPermission = true;
                                        divDeleteThread.Visible = true;
                                        divEditPermission.Visible = true;
                                    }
                                    string profilePicture = (new UserProfileInfo().getUserPic(userValue));
                                    if (profilePicture == string.Empty)
                                        imgAuthorProfile.ImageUrl = "/_layouts/15/Akumina.WebParts.DiscussionBoard/images/anonymous-user.png";
                                    else if (DisplayAvatarPicture)
                                        imgAuthorProfile.ImageUrl = profilePicture;
                                    else
                                        imgAuthorProfile.ImageUrl = "/_layouts/15/Akumina.WebParts.DiscussionBoard/images/anonymous-user.png";


                                    if (getTopic.Fields.ContainsField(_attachementLinkField))
                                        oLinks = getTopic[_attachementLinkField] != null ? getTopic[_attachementLinkField].ToString() : "";
                                    int i = 0;
                                    if (!string.IsNullOrEmpty(oLinks))
                                    {
                                        string listOFFilesHtml = string.Empty, listOfFilesHnVal = string.Empty;
                                        oLinksArr = oLinks.Split('|');

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
                                                HtmlGenericControl li;
                                                HtmlAnchor ancho;
                                                foreach (var id in list.IDs)
                                                {
                                                    if (id == "0")
                                                    {
                                                        i = i + 1;
                                                        //ltrlAttachement.Text += item.File != null ? item.File.ServerRelativeUrl : item.Folder.ServerRelativeUrl + "<br/>";
                                                        li = new HtmlGenericControl("li");
                                                        ancho = new HtmlAnchor();
                                                        ancho.HRef = oListDoc.RootFolder.ServerRelativeUrl; //oWeb.Url + item.File.ServerRelativeUrl;
                                                        ancho.InnerHtml = oListDoc.Title;
                                                        //li.InnerText = item.File != null ? item.File.ServerRelativeUrl : item.Folder.ServerRelativeUrl;
                                                        li.Controls.Add(ancho);
                                                        ulAttachment.Controls.Add(li);
                                                    }
                                                    else
                                                    {
                                                        SPListItem item = oListDoc.GetItemById(int.Parse(id));
                                                        if (item != null)
                                                        {
                                                            i = i + 1;
                                                            //ltrlAttachement.Text += item.File != null ? item.File.ServerRelativeUrl : item.Folder.ServerRelativeUrl + "<br/>";
                                                            li = new HtmlGenericControl("li");
                                                            ancho = new HtmlAnchor();
                                                            ancho.HRef = item.File != null ? item.File.ServerRelativeUrl : item.Folder.ServerRelativeUrl;//oWeb.Url + item.File.ServerRelativeUrl;
                                                            ancho.InnerHtml = item.File.Name;
                                                            //li.InnerText = item.File != null ? item.File.ServerRelativeUrl : item.Folder.ServerRelativeUrl;
                                                            li.Controls.Add(ancho);
                                                            ulAttachment.Controls.Add(li);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    if (i == 0)
                                        threadAttachments.Visible = false;
                                    List<string> users = new List<string>();
                                    if (divEditPermission.Visible && getTopic.HasUniqueRoleAssignments && getTopic.RoleAssignments.Count > 0)
                                    {
                                        //List<PickerEntity> entityArrayList = new List<PickerEntity>();

                                        foreach (SPRoleAssignment role in getTopic.RoleAssignments)
                                        {
                                            if (userValue.User.LoginName != role.Member.LoginName)
                                            {
                                                users.Add(role.Member.LoginName.ToLower().Trim());
                                                //PickerEntity entity = new PickerEntity();
                                                //entity.Key = role.Member.LoginName;
                                                //entityArrayList.Add(entity);
                                            }
                                        }
                                        GetUsers(users);
                                        //pplpickerPermission.AddEntities(entityArrayList.ToArray().ToList());
                                    }
                                    else
                                        GetUsers(users);


                                    GetReplies(oList, getTopic["ID"].ToString(), oWeb);

                                    var alerted = oCurrentUser.Alerts.Cast<SPAlert>().ToList().Where(x => (x.Title.Equals(oList.Title + ": " + getTopic.Title))).ToList();
                                    if (alerted.Count > 0)
                                    {
                                        liFollowingThread.Visible = true;
                                        liFollowThread.Visible = false;
                                    }
                                    else
                                    {
                                        liFollowingThread.Visible = false;
                                        liFollowThread.Visible = true;
                                    }


                                }
                            }
                        }
                    }
                }
            });
        }

        private void GetReplies(SPList ListReplies, string IDReply, SPWeb webReply)
        {
            SPFieldUserValue userValue = null;
            string strProfile = string.Empty;
            string strAttachment = string.Empty;
            string strBody = string.Empty;
            if (ListReplies != null)
            {
                SPFolder t = ListReplies.GetItemById(Convert.ToInt32(IDReply)).Folder;
                SPQuery q = new SPQuery();
                q.Folder = t;
                SPListItemCollection res = ListReplies.GetItems(q);
                DataTable dt = createDataTable();
                //dt = res.GetDataTable();

                foreach (SPListItem listItem in res)
                {
                    userValue = new SPFieldUserValue(webReply, listItem["Created By"].ToString());
                    string profilePicture = (new UserProfileInfo().getUserPic(userValue));
                    if (profilePicture == string.Empty)
                        strProfile = "/_layouts/15/Akumina.WebParts.DiscussionBoard/images/anonymous-user.png";
                    else if (DisplayAvatarPicture)
                        strProfile = profilePicture;
                    else
                        strProfile = "/_layouts/15/Akumina.WebParts.DiscussionBoard/images/anonymous-user.png";


                    if (listItem.Fields.ContainsField(_attachementLinkField))
                        strAttachment = listItem[_attachementLinkField] != null ? listItem[_attachementLinkField].ToString() : "";

                    strBody = (listItem["Body"] != null && StripHTML(listItem["Body"].ToString()).Trim() != string.Empty) ? listItem["Body"].ToString() : string.Empty;
                    dt.Rows.Add(listItem["ID"], strProfile, listItem["Author"].ToString().Split('#')[1], listItem["Title"], strBody, strAttachment, listItem["Created"], userValue.LookupValue);
                }
                if (dt.Rows.Count > 0)
                {
                    rptrReplies.DataSource = dt;
                    rptrReplies.DataBind();
                }
            }
        }

        public DataTable createDataTable()
        {
            DataTable table = new DataTable();

            table.Columns.Add("ID", typeof(string));
            table.Columns.Add("AuthorPicture", typeof(string));
            table.Columns.Add("Author", typeof(string));
            table.Columns.Add("Title", typeof(string));
            table.Columns.Add("Body", typeof(string));
            table.Columns.Add("Attachment", typeof(string));
            table.Columns.Add("Created", typeof(string));
            table.Columns.Add("ProfileAuthor", typeof(string));
            return table;
        }

        public void getCurrentUserPic()
        {
            string profilePictureOutput = string.Empty;
            if (!SPContext.Current.Web.CurrentUser.Name.ToString().Contains("System Account"))
            {
                SPServiceContext ctx = SPServiceContext.Current;     //ServerContext.GetContext("SharedServices1");
                UserProfileManager profileManager = new UserProfileManager(ctx);
                UserProfile profile = profileManager.GetUserProfile(@"" + SPContext.Current.Web.CurrentUser.LoginName);
                profilePictureOutput = profile[PropertyConstants.PictureUrl].Value != null ? profile[PropertyConstants.PictureUrl].Value.ToString() : string.Empty;
            }
            //(new UserProfileInfo().getUserProfileInfo(SPContext.Current.Web.CurrentUser.LoginName.Replace("/", "//")));            
            //UserProfileManager profileManager = new UserProfileManager();
            //UserProfile profile = profileManager.GetUserProfile(SPContext.Current.Web.CurrentUser.RawSid);
            //if (profile != null)
            //{
            //    //Set the Profile
            //    string account_name = profile.AccountName;
            //    profilePictureOutput = profile[PropertyConstants.PictureUrl].Value != null ? profile[PropertyConstants.PictureUrl].Value.ToString() : string.Empty;
            //}

            if (profilePictureOutput == string.Empty)
                imgCurrentUser.ImageUrl = "/_layouts/15/Akumina.WebParts.DiscussionBoard/images/anonymous-user.png";
            else if (DisplayAvatarPicture)
                imgCurrentUser.ImageUrl = profilePictureOutput;
            else
                imgCurrentUser.ImageUrl = "/_layouts/15/Akumina.WebParts.DiscussionBoard/images/anonymous-user.png";
            //SPSite siteColl = SPContext.Current.Site;
            //SPServiceContext serviceContext = SPServiceContext.GetContext(siteColl);
            //UserProfileManager userProfileManager = new UserProfileManager();
            //UserProfile profile = null;

            //bool existingUser = userProfileManager.UserExists(SPContext.Current.Web.CurrentUser.LoginName);
            //if (existingUser)
            //{
            //    profile = userProfileManager.GetUserProfile(SPContext.Current.Web.CurrentUser.LoginName);
            //    if (profile != null)
            //    {
            //        if (profile[PropertyConstants.PictureUrl].Value != null && string.IsNullOrEmpty(profile[PropertyConstants.PictureUrl].Value.ToString()))
            //            imgCurrentUser.ImageUrl = "/_layouts/15/Akumina.WebParts.DiscussionBoard/images/anonymous-user.png";
            //        else if (DisplayAvatarPicture)
            //            imgCurrentUser.ImageUrl = profile[PropertyConstants.PictureUrl].Value.ToString();
            //        else
            //            imgCurrentUser.ImageUrl = "/_layouts/15/Akumina.WebParts.DiscussionBoard/images/anonymous-user.png";
            //    }

            //}
        }

        protected void rptrReplies_ItemCommand(object source, RepeaterCommandEventArgs e)
        {

        }
        private void FollowDiscussion()
        {
            SPUser oCurrentUser = SPContext.Current.Web.CurrentUser;
            using (SPSite site = new SPSite(SPContext.Current.Site.ID))
            {
                using (SPWeb web = site.OpenWeb(SPContext.Current.Web.ID))
                {
                    SPList list = web.Lists.TryGetList(_discussionListName);
                    if (list != null)
                    {
                        XmlDocument xmlDoc = new XmlDocument();
                        // The path to your alert templates
                        //xmlDoc.Load(@"C:\Program Files\Common Files\Microsoft Shared\Web Server Extensions\15\TEMPLATE\XML\Akuminaalerttemplates.xml");

                        // The list you want to have custom alert template 
                        //SPAlertTemplate template = new SPAlertTemplate();
                        //template.Xml = xmlDoc.InnerXml;
                        //template.Name = "AkuminaAlertTemplate";

                        SPListItem topic = list.GetItemById(Convert.ToInt16(HttpContext.Current.Request.QueryString["DiscussionID"]));
                        SPAlert newAlert = oCurrentUser.Alerts.Add();
                        newAlert.Title = list.Title + ": " + topic.Title;
                        newAlert.AlertType = SPAlertType.List;
                        newAlert.List = list;
                        //newAlert.Item = topic;
                        newAlert.DeliveryChannels = SPAlertDeliveryChannels.Email;
                        newAlert.AlertTemplate = list.AlertTemplate;//template;
                        newAlert.EventType = SPEventType.Add;
                        newAlert.Filter = string.Format("<Query><Or><Eq><FieldRef Name=\"ItemFullUrl\"/><Value type=\"string\">{0}</Value></Eq><BeginsWith><FieldRef Name=\"ItemFullUrl\"/><Value type=\"string\">{0}</Value></BeginsWith></Or></Query>", topic[SPBuiltInFieldId.ServerUrl].ToString().TrimStart('/'));
                        newAlert.AlertFrequency = SPAlertFrequency.Immediate;
                        newAlert.Update();
                        liFollowingThread.Visible = true;
                        liFollowThread.Visible = false;
                    }
                }
            }



        }

        private void UnFollowDiscussion()
        {
            SPUser oCurrentUser = SPContext.Current.Web.CurrentUser;
            using (SPSite site = new SPSite(SPContext.Current.Site.ID))
            {
                using (SPWeb web = site.OpenWeb(SPContext.Current.Web.ID))
                {
                    SPList list = web.Lists.TryGetList(_discussionListName);
                    if (list != null)
                    {
                        SPListItem topic = list.GetItemById(Convert.ToInt16(HttpContext.Current.Request.QueryString["DiscussionID"]));
                        List<SPAlert> alerted = oCurrentUser.Alerts.Cast<SPAlert>().ToList().Where(x => (x.Title.Equals(list.Title + ": " + topic.Title))).ToList();
                        if (alerted.Count > 0)
                        {
                            foreach (SPAlert alert in alerted)
                                oCurrentUser.Alerts.Delete(alert.ID);

                            liFollowingThread.Visible = false;
                            liFollowThread.Visible = true;

                        }

                    }
                }
            }
        }

        protected void rptrReplies_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            string[] oLinksArr;
            SPWeb oWeb = SPContext.Current.Web;
            SPUser oCurrentUser = SPContext.Current.Web.CurrentUser;
            RepeaterItem item = e.Item;
            if (item.ItemType == ListItemType.AlternatingItem || item.ItemType == ListItemType.Item)
            {
                HiddenField lit_Attachment = (HiddenField)item.FindControl("hdnAttachment");
                HtmlGenericControl htmlgen_Ul = (HtmlGenericControl)item.FindControl("ulAttachmentReplies");
                HtmlGenericControl htmlgen_Attachment = (HtmlGenericControl)item.FindControl("divAttachmentReply");
                HtmlGenericControl htmlanc_DeleteReply = (HtmlGenericControl)item.FindControl("divDeleteReply");
                HiddenField hdn_ProfileAuthor = (HiddenField)item.FindControl("hdnProfileAuthor");

                if (_hasAdminPermission || HasAdminPermission(oCurrentUser, hdn_ProfileAuthor.Value, oWeb.Site.SystemAccount.ID, null))
                {
                    htmlanc_DeleteReply.Visible = true;
                }


                if (!string.IsNullOrEmpty(lit_Attachment.Value))
                {
                    oLinksArr = lit_Attachment.Value.Split('|');

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
                            HtmlGenericControl liReply;
                            HtmlAnchor anchorReply;
                            foreach (var id in list.IDs)
                            {
                                if (id == "0")
                                {
                                    //ltrlAttachement.Text += item.File != null ? item.File.ServerRelativeUrl : item.Folder.ServerRelativeUrl + "<br/>";
                                    liReply = new HtmlGenericControl("li");
                                    anchorReply = new HtmlAnchor();
                                    anchorReply.HRef = oListDoc.RootFolder.ServerRelativeUrl; //oWeb.Url + item.File.ServerRelativeUrl;
                                    anchorReply.InnerHtml = oListDoc.Title;
                                    //li.InnerText = item.File != null ? item.File.ServerRelativeUrl : item.Folder.ServerRelativeUrl;
                                    liReply.Controls.Add(anchorReply);
                                    htmlgen_Ul.Controls.Add(liReply);
                                }
                                else
                                {
                                    SPListItem spitem = oListDoc.GetItemById(int.Parse(id));
                                    if (spitem != null)
                                    {
                                        liReply = new HtmlGenericControl("li");
                                        anchorReply = new HtmlAnchor();
                                        anchorReply.HRef = (spitem.Folder != null ? spitem.Folder.ServerRelativeUrl : spitem.File.ServerRelativeUrl); //oWeb.Url + (spitem.Folder != null ? spitem.Folder.ServerRelativeUrl : spitem.File.ServerRelativeUrl);
                                        anchorReply.InnerHtml = (spitem.Folder != null ? spitem.Folder.Name : Path.GetFileNameWithoutExtension(spitem.File.Name));
                                        liReply.Controls.Add(anchorReply);
                                        htmlgen_Ul.Controls.Add(liReply);
                                    }
                                }
                            }
                        }
                    }
                }
                else
                    htmlgen_Attachment.Visible = false;
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

        protected void btnCancelReply_Click(object sender, EventArgs e)
        {
            ClearReplyFields();
            LoadThreadDetails(Convert.ToInt16(HttpContext.Current.Request.QueryString["DiscussionID"]));
        }
        //private void AddPeoplePicker()
        //{
        //    divPicker.Controls.Clear();
        //    divPicker.DataBind();
        //    pplpickerPermission = new ClientPeoplePicker();
        //    pplpickerPermission.EnableViewState = true;

        //    pplpickerPermission.ID = "pplpickerPermission";
        //    pplpickerPermission.AllowMultipleEntities = true;
        //    pplpickerPermission.PrincipalAccountType = "User,SPGroup";

        //    divPicker.Controls.Add(pplpickerPermission);
        //}
        protected void btnPermissions_ServerClick(object sender, EventArgs e)
        {
            SPUser oCurrentUser = SPContext.Current.Web.CurrentUser;
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPSite oSite = new SPSite(SPContext.Current.Site.ID))
                {
                    using (SPWeb oWeb = oSite.OpenWeb(SPContext.Current.Web.ID))
                    {
                        SPList oList = null;
                        oList = oWeb.Lists.TryGetList(_discussionListName);
                        if (oList != null)
                        {
                            oWeb.AllowUnsafeUpdates = true;
                            int Postid = 0;
                            SPListItem newTopic = null; bool oldPost = false;
                            if (HttpContext.Current.Request.QueryString["DiscussionID"] != null && int.TryParse(HttpContext.Current.Request.QueryString["DiscussionID"].ToString(), out Postid))
                            {
                                newTopic = oList.GetItemById(Postid);
                                oldPost = true;

                                SPFieldUserValue userValue = new SPFieldUserValue(oWeb, newTopic["Created By"].ToString());
                                if (HasAdminPermission(oCurrentUser, userValue.LookupId, oWeb.Site.SystemAccount.ID, oList))
                                {
                                    List<ListItem> selectedUsers = pplpickerPermission.Items.Cast<ListItem>().Where(x => x.Selected).ToList();
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
                                }
                                oWeb.AllowUnsafeUpdates = false;
                            }
                        }
                    }
                }
            });
            LoadThreadDetails(Convert.ToInt16(HttpContext.Current.Request.QueryString["DiscussionID"]));
        }

        protected void anhrDeleteReply_ServerClick(object sender, EventArgs e)
        {
            string strReplyID = hdnDeleteReplyId.Value;
            SPUser oCurrentUser = SPContext.Current.Web.CurrentUser;
            SPSecurity.RunWithElevatedPrivileges(delegate()
          {

              using (SPSite oSite = new SPSite(SPContext.Current.Site.ID))
              {
                  using (SPWeb oWeb = oSite.OpenWeb(SPContext.Current.Web.ID))
                  {

                      oWeb.AllowUnsafeUpdates = true;
                      SPList oList = null;
                      oList = oWeb.Lists.TryGetList(_discussionListName);
                      if (oList != null)
                      {
                          SPListItem item = oList.GetItemById(Convert.ToInt16(strReplyID));
                          SPFieldUserValue userValue = new SPFieldUserValue(oWeb, item["Created By"].ToString());
                          if (HasAdminPermission(oCurrentUser, userValue.LookupId, oWeb.Site.SystemAccount.ID, oList))
                          {
                              item.Delete();
                              oList.Update();
                          }
                      }
                      oWeb.AllowUnsafeUpdates = false;
                  }
              }
          });
            hdnDeleteReplyId.Value = null;
            rptrReplies.DataSource = null;
            rptrReplies.DataBind();

            LoadThreadDetails(Convert.ToInt16(HttpContext.Current.Request.QueryString["DiscussionID"]));
        }
        protected void anchrFollowThread_ServerClick(object sender, EventArgs e)
        {
            FollowDiscussion();
        }
        protected void anchrUnFollowThread_ServerClick(object sender, EventArgs e)
        {
            UnFollowDiscussion();
        }

        protected void anchrDeleteThread_ServerClick(object sender, EventArgs e)
        {
            SPUser oCurrentUser = SPContext.Current.Web.CurrentUser;
            SPSecurity.RunWithElevatedPrivileges(delegate()
           {
               using (SPSite oSite = new SPSite(SPContext.Current.Site.ID))
               {
                   using (SPWeb oWeb = oSite.OpenWeb(SPContext.Current.Web.ID))
                   {
                       oWeb.AllowUnsafeUpdates = true;
                       SPList oList = null;
                       oList = oWeb.Lists.TryGetList(_discussionListName);
                       if (oList != null)
                       {
                           SPListItem item = oList.GetItemById(Convert.ToInt16(HttpContext.Current.Request.QueryString["DiscussionID"]));
                           SPFieldUserValue userValue = new SPFieldUserValue(oWeb, item["Created By"].ToString());
                           if (HasAdminPermission(oCurrentUser, userValue.LookupId, oWeb.Site.SystemAccount.ID, oList))
                           {
                               item.Delete();
                               oList.Update();

                           }
                       }

                       oWeb.AllowUnsafeUpdates = false;
                       if (!string.IsNullOrEmpty(DiscussionListPageUrl))
                           HttpContext.Current.Response.Redirect(DiscussionListPageUrl);
                   }
               }
           });

        }

        protected void anchrPostreply_ServerClick(object sender, EventArgs e)
        {
            SPUser oCurrentUser = SPContext.Current.Web.CurrentUser;

            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                {
                    using (SPWeb web = site.OpenWeb(SPContext.Current.Web.ID))
                    {
                        SPList list = web.Lists.TryGetList(_discussionListName);
                        if (list != null)
                        {
                            web.AllowUnsafeUpdates = true;
                            SPListItem topic = list.GetItemById(Convert.ToInt16(HttpContext.Current.Request.QueryString["DiscussionID"]));
                            SPListItem reply = SPUtility.CreateNewDiscussionReply(topic);
                            reply["Body"] = CKEditorReply.Text;
                            if (reply.Fields.ContainsField(_attachementLinkField))
                                reply[_attachementLinkField] = listOfFilesHn.Value.TrimEnd('|');
                            reply.Update();
                            SPFieldUserValue oUser = new SPFieldUserValue(web, oCurrentUser.ID, oCurrentUser.LoginName);
                            reply["Author"] = oUser; // Created By
                            reply["Editor"] = oUser; // Modified By
                            reply.Update();
                            web.AllowUnsafeUpdates = true;

                            ClearReplyFields();
                            LoadThreadDetails(Convert.ToInt16(HttpContext.Current.Request.QueryString["DiscussionID"]));
                        }
                    }
                }
            });
        }
        private void ClearReplyFields()
        {

            CKEditorReply.Text = string.Empty;
            listOfFiles.InnerHtml = string.Empty;
            listOfFilesHn.Value = string.Empty;

        }

        private bool HasAdminPermission(SPUser user, int itemUserId, int systemAccountId, SPList list)
        {
            return (user.ID == systemAccountId || user.ID == itemUserId || user.IsSiteAdmin ||
                (list != null && list.DoesUserHavePermissions(user, SPBasePermissions.ManagePermissions))
                );
        }

        private bool HasAdminPermission(SPUser user, string itemUserName, int systemAccountId, SPList list)
        {
            return (user.ID == systemAccountId || user.Name == itemUserName || user.IsSiteAdmin ||
                (list != null && list.DoesUserHavePermissions(user, SPBasePermissions.ManagePermissions))
                );
        }
    }




}
