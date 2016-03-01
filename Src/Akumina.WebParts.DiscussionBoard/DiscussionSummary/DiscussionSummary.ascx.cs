using System;
using System.ComponentModel;
using System.Data;
using System.Text.RegularExpressions;
using System.Web;
using Microsoft.SharePoint;
using Akumina.InterAction;
using System.Text;
using Akumina.WebParts.DiscussionBoard.Properties;

namespace Akumina.WebParts.DiscussionBoard.DiscussionSummary
{
    [ToolboxItem(false)]
    public partial class DiscussionSummary :SummaryDiscussionBaseWebPart
    {
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
       
        public DiscussionSummary()
        {
        }

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

            if (!Page.IsPostBack)
            {                
                ViewState["Column"] = "Modified";
                ViewState["Sortorder"] = "DESC";
                
                getData();
               
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
                    DiscussionListName = clientServices.GetValue(response.Dictionary, "DiscussionListName", DiscussionListName);
                    DiscussionTitle = clientServices.GetValue(response.Dictionary, "DiscussionTitle", DiscussionTitle);
                    DocumentsListName = clientServices.GetValue(response.Dictionary, "DocumentLibraryName", DocumentsListName);
                    DiscussionListPageUrl = clientServices.GetValue(response.Dictionary, "DiscussionListPageUrl", DiscussionListPageUrl);
                    DiscussionCreatePageUrl = clientServices.GetValue(response.Dictionary, "DiscussionCreatePageUrl", DiscussionCreatePageUrl);
                    DiscussionThreadPageUrl = clientServices.GetValue(response.Dictionary, "DiscussionThreadPageUrl", DiscussionThreadPageUrl);
                    DisplayAvatarPicture = clientServices.GetValue(response.Dictionary, "Summary.DisplayAvatarPicture", DisplayAvatarPicture == true ? "1" : "0") != "0" ? true : false;
                    NumberOfPosts = Int32.Parse(clientServices.GetValue(response.Dictionary, "Summary.NumberOfPosts", NumberOfPosts.ToString()));
                    ListingPostType = (EnumPostType)Enum.Parse(typeof(EnumPostType), clientServices.GetValue(response.Dictionary, "Summary.ListingPostType", ListingPostType.ToString()));
                    //DisplayAvatarPicture = clientServices.GetValue(response.Dictionary, "Summary.DisplayAvatarPicture", DisplayAvatarPicture);
                    //NumberOfPosts = clientServices.GetValue(response.Dictionary, "Summary.NumberOfPosts", NumberOfPosts);
                    //ListingPostType = clientServices.GetValue(response.Dictionary, "Summary.ListingPostType", ListingPostType);
                }
            }
        }


        private SPListItemCollection GetListItems(int rowlimit, int pageNo, SPList list)
        {
            SPList oList = list;
            SPQuery query = new SPQuery();
            query.RowLimit = (uint)rowlimit;
            query.Query = @"<Where><Eq><FieldRef Name='ConfirmArchive' /><Value Type='Boolean'>False</Value></Eq></Where><OrderBy><FieldRef Name='ID' Ascending='False' /></OrderBy>";

            int index = 1;

            SPListItemCollection items;
            do
            {
                items = oList.GetItems(query);
                if (index == pageNo)
                    break;
                query.ListItemCollectionPosition = items.ListItemCollectionPosition;
                index++;
            }
            while (query.ListItemCollectionPosition != null);
            return items;
        }


        public void getData()
        {

            SPWeb web = SPContext.Current.Web;
            SPList list = web.Lists.TryGetList(DiscussionListName);
            if (list != null)
            {               
                DataTable dataTable = null;

                string modifyPermission = string.Empty;
                string folderid = string.Empty;
                string userProfileImage = string.Empty;

                DataTable table = createDataTable();

                SPQuery query = new SPQuery();
                string picURL = string.Empty;
                SPFieldUserValue userValue = null;
                string pattern = "<.*?>";
                string postID = string.Empty;
                string postTitle = string.Empty;
                string postBody = string.Empty;
                string postAuthor = string.Empty;
                string confirmArchive = string.Empty;
                string isModifyable = string.Empty;
                string currentUrl = HttpContext.Current.Request.Url.ToString();
                string redirectUrl = currentUrl.Substring(0, (currentUrl.LastIndexOf("/")));
                DiscussionThreadPageUrl = redirectUrl + "/" + DiscussionThreadPageUrl;
                SPListItemCollection listitemColl = GetListItems(NumberOfPosts,1, list);
                if (listitemColl.Count > 0)
                {
                    WebPartTitle.Text = Title;
                    WebPartIcon.Text = GetIcon(Icon);
                    discussionSummaryHeader.Visible = Title != "" || GetIcon(Icon) != "none";
                    foreach (SPListItem folder in listitemColl)
                    {
                        if (ListingPostType == EnumPostType.LastPost)
                        {
                            userValue = new SPFieldUserValue(web, folder["Created By"].ToString());
                            isModifyable = checkPermissions(web, modifyPermission, userValue);

                            if (DisplayAvatarPicture == true)
                                userProfileImage = (new UserProfileInfo().getUserPic(userValue));

                            query.RowLimit = 1;
                            query.Folder = folder.Folder;
                            query.Query = "<OrderBy><FieldRef Name='ID' Ascending='FALSE' /></OrderBy>";
                            SPListItemCollection collection = list.GetItems(query);
                            if (collection.Count > 0)
                            {
                                SPListItem replyItem = collection[0];
                                postID = replyItem["ID"].ToString();
                                postTitle = folder["Title"].ToString();

                                postTitle = postTitle.Length > DiscussionTitleTextCount ? postTitle.Substring(0, DiscussionTitleTextCount) + "..." : postTitle;
                                postBody = StripHTML(replyItem["Body"] != null ? replyItem["Body"].ToString() : String.Empty, pattern);
                                postBody = postBody.Length > DiscussionPreviewTextCount ? postBody.Substring(0, DiscussionPreviewTextCount) + "..." : postBody;

                                postAuthor = replyItem["Author"] != null ? replyItem["Author"].ToString() : string.Empty;
                                confirmArchive = replyItem["ConfirmArchive"] != null ? replyItem["ConfirmArchive"].ToString() : string.Empty;

                                //if (confirmArchive.Equals("False"))
                                    dataTable = GetTable(table, postID, userProfileImage != "" ? userProfileImage : "../_layouts/15/Akumina.WebParts.DiscussionBoard/images/anonymous-user.png", DiscussionThreadPageUrl + "?DiscussionID=" + folder["ID"].ToString(), postAuthor.ToString().Split('#')[1], postTitle, postBody, Convert.ToDateTime(folder["Created"].ToString()), Convert.ToDateTime(folder["Modified"].ToString()), Convert.ToDateTime(folder["DiscussionLastUpdated"].ToString()), folder["ItemChildCount"].ToString(), confirmArchive, isModifyable);
                            }
                            else
                            {
                                confirmArchive = folder["ConfirmArchive"] != null ? folder["ConfirmArchive"].ToString() : string.Empty;

                                postTitle = folder["Title"].ToString();
                                postTitle = postTitle.Length > DiscussionTitleTextCount ? postTitle.Substring(0, DiscussionTitleTextCount)+"..." : postTitle;

                                postBody = StripHTML(folder["Body"] != null ? folder["Body"].ToString() : String.Empty, pattern);
                                postBody = postBody.Length > DiscussionPreviewTextCount ? postBody.Substring(0, DiscussionPreviewTextCount) + "..." : postBody;
                                //if (confirmArchive.Equals("False"))
                                    dataTable = GetTable(table, folder["ID"].ToString(), userProfileImage != "" ? userProfileImage : "../_layouts/15/Akumina.WebParts.DiscussionBoard/images/anonymous-user.png", DiscussionThreadPageUrl + "?DiscussionID=" + folder["ID"], folder["Author"].ToString().Split('#')[1], postTitle, postBody, Convert.ToDateTime(folder["Created"].ToString()), Convert.ToDateTime(folder["Modified"].ToString()), Convert.ToDateTime(folder["DiscussionLastUpdated"].ToString()), folder["ItemChildCount"].ToString(), folder["ConfirmArchive"] != null ? folder["ConfirmArchive"].ToString() : "", isModifyable);
                            }
                        }
                        else
                        {
                            userValue = new SPFieldUserValue(web, folder["Created By"].ToString());
                            isModifyable = checkPermissions(web, modifyPermission, userValue);
                            if (DisplayAvatarPicture == true)
                                userProfileImage = (new UserProfileInfo().getUserPic(userValue));

                            postTitle = folder["Title"].ToString();
                            postTitle = postTitle.Length > DiscussionTitleTextCount ? postTitle.Substring(0, DiscussionTitleTextCount) + "..." : postTitle;

                            postBody = StripHTML(folder["Body"] != null ? folder["Body"].ToString() : String.Empty, pattern);
                            postBody = postBody.Length > DiscussionPreviewTextCount ? postBody.Substring(0, DiscussionPreviewTextCount) + "..." : postBody;

                            confirmArchive = folder["ConfirmArchive"] != null ? folder["ConfirmArchive"].ToString() : string.Empty;
                            //if (confirmArchive.Equals("False"))
                            dataTable = GetTable(table, folder["ID"].ToString(), userProfileImage != "" ? userProfileImage : "../_layouts/15/Akumina.WebParts.DiscussionBoard/images/anonymous-user.png", DiscussionThreadPageUrl + "?DiscussionID=" + folder["ID"], folder["Author"].ToString().Split('#')[1], postTitle, postBody, Convert.ToDateTime(folder["Created"].ToString()), Convert.ToDateTime(folder["Modified"].ToString()), Convert.ToDateTime(folder["DiscussionLastUpdated"].ToString()), folder["ItemChildCount"].ToString(), folder["ConfirmArchive"] != null ? folder["ConfirmArchive"].ToString() : "", isModifyable);

                        }
                    }

                    if (dataTable != null)
                    {                        
                        repeaterDBSummary.DataSource = dataTable;
                        repeaterDBSummary.DataBind();
                    }
                }
            }
            else
            {
                errorLiteralDBSummary.Text = "Please Enter the List Name in the Webpart Configuration";
            }
        }

        private string StripHTML(string input, string pattern)
        {
            return Regex.Replace(input, pattern, String.Empty);
        }

        public DataTable createDataTable()
        {
            DataTable table = new DataTable();

            table.Columns.Add("ID", typeof(string));
            table.Columns.Add("AuthorPicture", typeof(string));
            table.Columns.Add("EditDiscussionPost", typeof(string));
            table.Columns.Add("Author", typeof(string));
            table.Columns.Add("Title", typeof(string));
            table.Columns.Add("Body", typeof(string));
            table.Columns.Add("Created", typeof(DateTime));
            table.Columns.Add("Modified", typeof(DateTime));
            table.Columns.Add("DiscussionLastUpdated", typeof(DateTime));
            table.Columns.Add("ItemChildCount", typeof(string));
            table.Columns.Add("ConfirmArchive", typeof(string));
            table.Columns.Add("modifyPermission", typeof(string));

            return table;
        }

        public string checkPermissions(SPWeb web, string modifyPermission, SPFieldUserValue userValue)
        {
            if (web.CurrentUser.IsSiteAdmin)
            {
                modifyPermission = "true";
            }
            else if (userValue.User.Name != "System Account")
            {
                if (userValue.User.ToString().Split('|')[1].Equals(web.CurrentUser.ToString().Split('|')[1]))
                {
                    modifyPermission = "true";
                }
                else
                {
                    modifyPermission = "false";
                }
            }
            else
            {
                modifyPermission = "false";
            }
            return modifyPermission;
        }

        public DataTable GetTable(DataTable table, string ID, string authorpic, string EditDiscussionPost, string author, string discussionTitle, string discussionSummary, DateTime createdDate,DateTime Modified, DateTime lastPostDate, string replies, string ConfirmArchive, string modifyPermission)
        {
            table.Rows.Add(ID, authorpic,EditDiscussionPost, author, discussionTitle, discussionSummary, createdDate,Modified, lastPostDate, replies, ConfirmArchive, modifyPermission);
            return table;
        }

        protected void SeeallDiscussion_ServerClick(object sender, EventArgs e)
        {
            string currentUrl = HttpContext.Current.Request.Url.ToString();
            string redirectUrl=currentUrl.Substring(0, (currentUrl.LastIndexOf("/")));
            HttpContext.Current.Response.Redirect(redirectUrl+"/"+DiscussionListPageUrl);
        }
    }
}
