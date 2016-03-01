using System;
using System.ComponentModel;
using System.Data;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.SharePoint;
using Akumina.InterAction;
using System.Text;
using Akumina.WebParts.DiscussionBoard.Properties;

namespace Akumina.WebParts.DiscussionBoard.DiscussionBoardListing
{
  [ToolboxItem(false)]
  public partial class DiscussionBoardListing : ListingDiscussionBaseWebPart
  {
    // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
    // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
    // for production. Because the SecurityPermission attribute bypasses the security check for callers of
    // your constructor, it's not recommended for production purposes.
    // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]

    public string TimeAgo(DateTime dt)
    {
      TimeSpan span = DateTime.Now - dt;
      //if (span.Days > 365)
      //{
      //    int years = (span.Days / 365);
      //    if (span.Days % 365 != 0)
      //        years += 1;
      //    return String.Format("{0} {1} ago",
      //    years, years == 1 ? "year" : "years");
      //}
      //if (span.Days > 30)
      //{
      //    int months = (span.Days / 30);
      //    if (span.Days % 31 != 0)
      //        months += 1;
      //    return String.Format("{0} {1} ago",
      //    months, months == 1 ? "month" : "months");
      //}
      if (span.Days > 0)
        return dt.ToString("MMM. d, yyyy");
      //return String.Format("{0} {1} ago",
      //span.Days, span.Days == 1 ? "day" : "days");
      if (span.Hours > 0)
        return String.Format("{0} {1} ago",
        span.Hours, span.Hours == 1 ? "hour" : "hours");
      if (span.Minutes > 0)
        return String.Format("{0} {1} ago",
        span.Minutes, span.Minutes == 1 ? "minute" : "minutes");
      if (span.Seconds > 5)
        return String.Format("{0} seconds ago", span.Seconds);
      if (span.Seconds <= 5)
        return "just now";
      return string.Empty;
    }
    private int CurrentPage
    {
      get
      {   //Check view state is null if null then return current index value as "0" else return specific page viewstate value
        if (ViewState["CurrentPage"] == null)
        {
          return 0;
        }
        else
        {
          return ((int)ViewState["CurrentPage"]);
        }
      }
      set
      {
        //Set View statevalue when page is changed through Paging "RepeaterPaging" DataList
        ViewState["CurrentPage"] = value;
      }
    }


    private string _addNewDiscussionURL;
    public string addNewDiscussionURL
    {
      get
      {
        return _addNewDiscussionURL;
      }
      set
      {
        _addNewDiscussionURL = value;
      }
    }

    public int PageCount
    {
      get
      {
        if (ViewState["PageCount"] != null)
          return Convert.ToInt32(ViewState["PageCount"]);
        else
          return 0;
      }
      set
      {
        ViewState["PageCount"] = value;
      }
    }
    public int TotalListItemsCount
    {
      get
      {
        if (ViewState["TotalListItemsCount"] != null)
          return Convert.ToInt32(ViewState["TotalListItemsCount"]);
        else
          return 0;
      }
      set
      {
        ViewState["TotalListItemsCount"] = value;
      }

    }

    public int DiscussionListItemsCount
    {
      get
      {
        if (ViewState["DiscussionListItemsCount"] != null)
          return Convert.ToInt32(ViewState["DiscussionListItemsCount"]);
        else
          return 0;
      }
      set
      {
        ViewState["DiscussionListItemsCount"] = value;
      }
    }

    public DiscussionBoardListing()
    {
    }
    protected override void OnInit(EventArgs e)
    {
      try
      {
        if (Page.Request.Browser.Type.ToUpper().Contains("SAFARI"))
        {
          ScriptManager sm = ScriptManager.GetCurrent(this.Page);
          if (sm != null)
          {
            sm.EnablePartialRendering = false;
          }
        }
      }
      catch
      {
      }
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
        boardPageTitle.Text = DiscussionTitle;
        DiscussionListItemsCount = 0;
        ViewState["Column"] = "Created";
        ViewState["Sortorder"] = "DESC";
        hdnPageLoad.Value = "true";

        getData();
        hdnSortBy.Value = ViewState["Column"].ToString();
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
                    else
                    {
                        sb.AppendLine("<script type=\"text/javascript\" src=\"" + resourcePathValue + jsFile + "\"></script>");
                    }

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
          DisplayAvatarPicture = clientServices.GetValue(response.Dictionary, "Listing.DisplayAvatarPicture", DisplayAvatarPicture == true ? "1" : "0") != "0" ? true : false;
          NumberOfPosts = Int32.Parse(clientServices.GetValue(response.Dictionary, "Listing.NumberOfPosts", NumberOfPosts.ToString()));
          ListingPostType = (EnumPostType)Enum.Parse(typeof(EnumPostType), clientServices.GetValue(response.Dictionary, "Listing.ListingPostType", ListingPostType.ToString()));
        }
      }
    }

    private SPListItemCollection GetListItems(int rowlimit, int pageNo, SPList list)
    {

      SPQuery query = new SPQuery();
      query.Query = @"<Where><Eq><FieldRef Name='ConfirmArchive' /><Value Type='Boolean'>False</Value></Eq></Where><OrderBy><FieldRef Name='ID' Ascending='False' /></OrderBy>";

      DiscussionListItemsCount = list.GetItems(query).Count;
      PageCount = (int)Math.Ceiling(DiscussionListItemsCount / (decimal)rowlimit);
      TotalListItemsCount = DiscussionListItemsCount;

      SPQuery query2 = new SPQuery();
      if (ViewState["Sortorder"].ToString() == "ASC")
        query2.Query = @"<Where><Eq><FieldRef Name='ConfirmArchive' /><Value Type='Boolean'>False</Value></Eq></Where><OrderBy><FieldRef Name='" + ViewState["Column"] + "' Ascending='True' /></OrderBy>";
      else
        query2.Query = @"<Where><Eq><FieldRef Name='ConfirmArchive' /><Value Type='Boolean'>False</Value></Eq></Where><OrderBy><FieldRef Name='" + ViewState["Column"] + "' Ascending='False' /></OrderBy>";
      query2.RowLimit = (uint)rowlimit;
      int index = 1;

      SPListItemCollection items;
      do
      {
        items = list.GetItems(query2);
        if (index == pageNo)
          break;
        query2.ListItemCollectionPosition = items.ListItemCollectionPosition;
        index++;
      }
      while (query2.ListItemCollectionPosition != null);
      return items;
    }

    public void getData()
    {

      SPWeb web = SPContext.Current.Web;
      SPList list = web.Lists.TryGetList(DiscussionListName);
      if (list != null)
      {
          bool hasAdminPermission = list.DoesUserHavePermissions(SPBasePermissions.ManagePermissions);

        if (list.DoesUserHavePermissions(SPBasePermissions.AddListItems))
          ListingAddNewDiscussion.Visible = true;

        DataTable dataTable = null;

        string modifyPermission = string.Empty;
        string folderid = string.Empty;
        string userProfileImage = string.Empty;

        DataTable table = createDataTable();

        SPQuery query = new SPQuery();
        string picURL = string.Empty;
        SPFieldUserValue userValue = null;

        string pattern = "<.*?>";
        SPListItemCollection listitemColl = GetListItems(NumberOfPosts, (CurrentPage + 1), list);
        if (listitemColl.Count > 0)
        {
          foreach (SPListItem folder in listitemColl)
          {
            if (ListingPostType == EnumPostType.LastPost)
            {
              userValue = new SPFieldUserValue(web, folder["Created By"].ToString());
              string isModifyable = checkPermissions(web, modifyPermission, userValue, hasAdminPermission);

              if (DisplayAvatarPicture == true)
                userProfileImage = (new UserProfileInfo().getUserPic(userValue));

              query.RowLimit = 1;
              query.Folder = folder.Folder;
              query.Query = "<OrderBy><FieldRef Name='ID' Ascending='FALSE' /></OrderBy>";
              SPListItemCollection collection = list.GetItems(query);
              if (collection.Count > 0)
              {
                SPListItem replyItem = collection[0];
                string postID = replyItem["ID"].ToString();
                string postTitle = folder["Title"].ToString();

                postTitle = postTitle.Length > DiscussionTitleTextCount ? postTitle.Substring(0, DiscussionTitleTextCount) + "..." : postTitle;
                string postBody = StripHTML(replyItem["Body"] != null ? replyItem["Body"].ToString() : String.Empty, pattern);
                postBody = postBody.Length > DiscussionPreviewTextCount ? postBody.Substring(0, DiscussionPreviewTextCount) + "..." : postBody;
                string postAuthor = replyItem["Author"] != null ? replyItem["Author"].ToString() : string.Empty;

                string confirmArchive = replyItem["ConfirmArchive"] != null ? replyItem["ConfirmArchive"].ToString() : string.Empty;

                //if (confirmArchive.Equals("False"))
                dataTable = GetTable(table, postID, userProfileImage != "" ? userProfileImage : "../_layouts/15/Akumina.WebParts.DiscussionBoard/images/anonymous-user.png", DiscussionThreadPageUrl + "?DiscussionID=" + folder["ID"].ToString(), postAuthor.ToString().Split('#')[1], postTitle, postBody, Convert.ToDateTime(folder["Created"].ToString()), Convert.ToDateTime(folder["DiscussionLastUpdated"].ToString()), folder["ItemChildCount"].ToString(), confirmArchive, isModifyable);
              }
              else
              {
                string confirmArchive = folder["ConfirmArchive"] != null ? folder["ConfirmArchive"].ToString() : string.Empty;

                string postTitle = folder["Title"].ToString();
                postTitle = postTitle.Length > DiscussionTitleTextCount ? postTitle.Substring(0, DiscussionTitleTextCount) + "..." : postTitle;

                string postBody = StripHTML(folder["Body"] != null ? folder["Body"].ToString() : String.Empty, pattern);
                postBody = postBody.Length > DiscussionPreviewTextCount ? postBody.Substring(0, DiscussionPreviewTextCount) + "..." : postBody;
                //if (confirmArchive.Equals("False"))
                dataTable = GetTable(table, folder["ID"].ToString(), userProfileImage != "" ? userProfileImage : "../_layouts/15/Akumina.WebParts.DiscussionBoard/images/anonymous-user.png", DiscussionThreadPageUrl + "?DiscussionID=" + folder["ID"], folder["Author"].ToString().Split('#')[1], postTitle, postBody, Convert.ToDateTime(folder["Created"].ToString()), Convert.ToDateTime(folder["DiscussionLastUpdated"].ToString()), folder["ItemChildCount"].ToString(), folder["ConfirmArchive"] != null ? folder["ConfirmArchive"].ToString() : "", isModifyable);
              }
            }
            else
            {

              userValue = new SPFieldUserValue(web, folder["Created By"].ToString());
              string isModifyable = checkPermissions(web, modifyPermission, userValue, hasAdminPermission);
              if (DisplayAvatarPicture == true)
                userProfileImage = (new UserProfileInfo().getUserPic(userValue));

              string postTitle = folder["Title"].ToString();
              postTitle = postTitle.Length > DiscussionTitleTextCount ? postTitle.Substring(0, DiscussionTitleTextCount) + "..." : postTitle;

              string postBody = StripHTML(folder["Body"] != null ? folder["Body"].ToString() : String.Empty, pattern);
              postBody = postBody.Length > DiscussionPreviewTextCount ? postBody.Substring(0, DiscussionPreviewTextCount) + "..." : postBody;


              string confirmArchive = folder["ConfirmArchive"] != null ? folder["ConfirmArchive"].ToString() : string.Empty;
              //if (confirmArchive.Equals("False"))
              dataTable = GetTable(table, folder["ID"].ToString(), userProfileImage != "" ? userProfileImage : "../_layouts/15/Akumina.WebParts.DiscussionBoard/images/anonymous-user.png", DiscussionThreadPageUrl + "?DiscussionID=" + folder["ID"], folder["Author"].ToString().Split('#')[1], postTitle, postBody, Convert.ToDateTime(folder["Created"].ToString()), Convert.ToDateTime(folder["DiscussionLastUpdated"].ToString()), folder["ItemChildCount"].ToString(), folder["ConfirmArchive"] != null ? folder["ConfirmArchive"].ToString() : "", isModifyable);

            }
          }

          if (dataTable != null)
          {
            PagedDataSource pgitems = new PagedDataSource();
            DataView dv = new DataView(dataTable);
            //dv.Sort = ViewState["Column"] + " " + ViewState["Sortorder"];
            pgitems.DataSource = dv;
            //pgitems.AllowPaging = true;
            //pgitems.PageSize = NumberOfPosts;
            int firstPageNo = 0, lastPageNo = 0;

            if (CurrentPage == 0)
            {
              firstPageNo = 1;
              lastPageNo = pgitems.Count;
            }
            /*else if (PageNumber.ToString() == ViewState["totpage"].ToString())
            {
                firstPageNo=(CurrentPage * Int32.Parse(_NumberOfPosts)) + 1;
                lastPageNo = firstPageNo + pgitems.Count - 1;
            }*/
            else
            {
              firstPageNo = (CurrentPage * NumberOfPosts) + 1;
              lastPageNo = firstPageNo + pgitems.Count - 1;
            }



            lblpage.Text = firstPageNo + "-" + lastPageNo + " of " + DiscussionListItemsCount;

            repeaterDBList.DataSource = pgitems;
            repeaterDBList.DataBind();

          }
        }
        else
        {
          repeaterDBList.DataSource = null;
          repeaterDBList.DataSourceID = null;
          repeaterDBList.DataBind();
        }
        setPagination();
      }
      else
      {
        errorLiteral.Text = "Please Enter the List Name in the Webpart Configuration";
      }
    }

    void setPagination()
    {
      if (PageCount <= 1)
      {
        lblpage.Text = "";
        linkbtnFirst.Visible = false;
        linkbtnLast.Visible = false;
        linkbtnNext.Visible = false;
        linkbtnPrevious.Visible = false;
      }
      //hdnCurrentPage.Value = "NoPaging";
      else if (CurrentPage == 0)
        hdnCurrentPage.Value = "first";
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
      table.Columns.Add("DiscussionLastUpdated", typeof(DateTime));
      table.Columns.Add("ItemChildCount", typeof(string));
      table.Columns.Add("ConfirmArchive", typeof(string));
      table.Columns.Add("modifyPermission", typeof(string));

      return table;
    }

    public string checkPermissions(SPWeb web, string modifyPermission, SPFieldUserValue userValue, bool hasAdminPermission)
    {
        if (web.CurrentUser.IsSiteAdmin || hasAdminPermission)
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

    public DataTable GetTable(DataTable table, string ID, string authorpic, string EditDiscussionPost, string author, string discussionTitle, string discussionSummary, DateTime createdDate, DateTime lastPostDate, string replies, string ConfirmArchive, string modifyPermission)
    {
      table.Rows.Add(ID, authorpic, EditDiscussionPost, author, discussionTitle, discussionSummary, createdDate, lastPostDate, replies, ConfirmArchive, modifyPermission);
      return table;
    }

    protected void repeaterDBList_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
      LinkButton linkBtn = null;
      if (e.CommandName == "archiveRow")
      {
        linkBtn = (LinkButton)e.CommandSource;
        string rowId = linkBtn.CommandArgument;
        ArchiveRow(rowId);
      }
      else if (e.CommandName == "deleteRow")
      {
        linkBtn = (LinkButton)e.CommandSource;
        string rowId = linkBtn.CommandArgument;
        DeleteRow(rowId);
      }
      else if (e.Item.ItemType == ListItemType.Header)
      {
        if (e.CommandName == "Created")
        {
          linkBtn = (LinkButton)e.Item.FindControl("linkSortCreated");
          hdnSortBy.Value = "Created";
        }
        if (e.CommandName == "DiscussionLastUpdated")
        {
          linkBtn = (LinkButton)e.Item.FindControl("linkSortPostTime");
          hdnSortBy.Value = "DiscussionLastUpdated";
        }

        if (e.CommandName == "ItemChildCount")
        {
          linkBtn = (LinkButton)e.Item.FindControl("linkSortReplies");
          hdnSortBy.Value = "ItemChildCount";
        }



        if (e.CommandName == ViewState["Column"].ToString())
        {
          if (ViewState["Sortorder"].ToString() == "ASC")
          {
            ViewState["Sortorder"] = "DESC";
            hdnSortDirection.Value = "DESC";
          }
          else
          {
            ViewState["Sortorder"] = "ASC";
            hdnSortDirection.Value = "ASC";
          }
        }
        else
        {
          ViewState["Column"] = e.CommandName;
          ViewState["Sortorder"] = "DESC";
          hdnSortDirection.Value = "DESC";
        }
      }
      getData();
    }


    protected void linkbtnPrevious_Click(object sender, EventArgs e)
    {
      if (CurrentPage > 0)
      {
        CurrentPage -= 1;
        hdnCurrentPage.Value = "";
        if (CurrentPage == 0)
          hdnCurrentPage.Value = "first";
        else
        {
          hdnCurrentPage.Value = "inbetween";

        }
        getData();
      }
    }

    protected void linkbtnNext_Click(object sender, EventArgs e)
    {
      if (CurrentPage < PageCount - 1)
      {
        CurrentPage += 1;
        hdnCurrentPage.Value = "";
        if (CurrentPage == PageCount - 1)
          hdnCurrentPage.Value = "last";
        else
        {
          hdnCurrentPage.Value = "inbetween";

        }
        getData();
      }
    }

    protected void linkbtnFirst_Click(object sender, EventArgs e)
    {
      if (CurrentPage > 0)
      {
        hdnCurrentPage.Value = "";
        hdnCurrentPage.Value = "first";
        CurrentPage = 0;
        getData();
      }
    }

    protected void linkbtnLast_Click(object sender, EventArgs e)
    {
      if (CurrentPage < PageCount - 1)
      {
        hdnCurrentPage.Value = "";
        hdnCurrentPage.Value = "last";
        CurrentPage = PageCount - 1;
        getData();
      }
    }

    public void DeleteRow(string ItemIDNew)
    {
      SPWeb currentWeb = SPContext.Current.Web;
      SPList lst = currentWeb.Lists[DiscussionListName];
      SPListItem item = null;
      item = lst.GetItemById(int.Parse(ItemIDNew));
      currentWeb.AllowUnsafeUpdates = true;
      item.Delete();
      currentWeb.AllowUnsafeUpdates = false;
    }

    public void ArchiveRow(string ItemIDNew)
    {
      SPWeb currentWeb = SPContext.Current.Web;
      SPList lst = currentWeb.Lists[DiscussionListName];
      SPListItem item = null;
      item = lst.GetItemById(int.Parse(ItemIDNew));
      currentWeb.AllowUnsafeUpdates = true;

      SPFieldBoolean boolField = item.Fields["ConfirmArchive"] as SPFieldBoolean;
      bool CheckBoxValue = (bool)boolField.GetFieldValue(item["ConfirmArchive"].ToString());
      item["ConfirmArchive"] = true;
      item.Update();
      currentWeb.AllowUnsafeUpdates = false;
    }

    protected void btnArchive_Click(object sender, EventArgs e)
    {
      string sIdClicked = hdnArchieveId.Value;
      ArchiveRow(sIdClicked);
      getData();
      hdnArchieveId.Value = string.Empty;
      if (!string.IsNullOrEmpty(DiscussionListPageUrl))
          HttpContext.Current.Response.Redirect(DiscussionListPageUrl);
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
      string sIdClicked = hdnDeleteId.Value;
      DeleteRow(sIdClicked);
      getData();
      hdnDeleteId.Value = string.Empty;
      if (!string.IsNullOrEmpty(DiscussionListPageUrl))
          HttpContext.Current.Response.Redirect(DiscussionListPageUrl);

    }

    protected void ListingAddNewDiscussion_ServerClick(object sender, EventArgs e)
    {
      HttpContext.Current.Response.Redirect(DiscussionCreatePageUrl);
    }
  }
}
