using System;
using System.Drawing;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebPartPages;

namespace Akumina.WebParts.DiscussionBoard.DiscussionBoardListing
{
    class DiscussionBoard_WPEditor : EditorPart
    {
        private DropDownList ddlList;
        private RadioButtonList displayAvatarPicture, listingPostType;
        private TextBox listName=null,pageTitle=null, numberOfPosts=null;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            ddlList = new DropDownList();
            SPWeb oWeb = SPContext.Current.Web;
            foreach (SPList oLists in oWeb.Lists)
            {
                //ddlList.Items.Add(oLists.Title);
            }

            listName = new TextBox();
            pageTitle = new TextBox();
            numberOfPosts = new TextBox();
            pageTitle.Text = "Please Enter The Page Title";
            numberOfPosts.Text = "5";

            displayAvatarPicture = new RadioButtonList();
            listingPostType = new RadioButtonList();

            displayAvatarPicture.ID = "RadioBtnDisplayAvatarPicture";
            displayAvatarPicture.RepeatDirection = RepeatDirection.Vertical;
            displayAvatarPicture.RepeatLayout = RepeatLayout.Table;
            displayAvatarPicture.Items.Add(new ListItem("Yes", "Yes"));
            displayAvatarPicture.Items.Add(new ListItem("No", "No"));
            displayAvatarPicture.SelectedIndex = 0;          

            listingPostType.ID = "RadioBtnListingPostType";
            listingPostType.RepeatDirection = RepeatDirection.Vertical;
            listingPostType.Items.Add(new ListItem("Original Post", "Original Post"));
            listingPostType.Items.Add(new ListItem("Last Post"));
            listingPostType.SelectedIndex = 0;
        }

        protected override void CreateChildControls()
        {
            base.CreateChildControls();

            this.Controls.Add(new LiteralControl("<br/><b>List Name</b><br/>"));
            this.Controls.Add(listName);
            this.Controls.Add(new LiteralControl("<br/><br/><b>Display Avatar Picture</b><br/>"));
            this.Controls.Add(displayAvatarPicture);
            this.Controls.Add(new LiteralControl("<b><br/>Listing Post Type</b><br/>"));
            this.Controls.Add(listingPostType);
            this.Controls.Add(new LiteralControl("<br/><br/><b>Page Title</b><br/>"));
            this.Controls.Add(pageTitle);
            this.Controls.Add(new LiteralControl("<br/><br/><b>Number Of Posts</b><br/>"));
            this.Controls.Add(numberOfPosts);
            this.Controls.Add(new LiteralControl("<br/>"));
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
        }

        public override bool ApplyChanges()  //ApplyChanges is used to save any changes to the currently-edited Web Part.
        {
            DiscussionBoardListing webPart = this.WebPartToEdit as DiscussionBoardListing;
            if (webPart != null)
            {
                webPart._listName = listName.Text.Trim();//.SelectedValue;
                //webPart._DisplayAvatarPicture = displayAvatarPicture.SelectedItem.Text;
                //webPart._ListingPostType = listingPostType.SelectedItem.Text;
                webPart._pageTitle = pageTitle.Text;
                int numberOfPostsResult;

                if (Int32.TryParse(numberOfPosts.Text, out numberOfPostsResult) == false || Int32.Parse(numberOfPosts.Text) <= 0)
                {
                    numberOfPosts.BorderColor = ColorTranslator.FromHtml("#ff0000");
                    throw new WebPartPageUserException("Enter Valid Number of Posts.");
                    
                }
                webPart._NumberOfPosts = numberOfPosts.Text;
            }
            return true;
        }

        public override void SyncChanges()  // SyncChanges is used to load the current configuration from Web Part Properties.
        {
            DiscussionBoardListing webPart = this.WebPartToEdit as DiscussionBoardListing;
            if (webPart != null)
            {
                //ddlList.SelectedValue = webPart._listName;
            }
            listName.Text = webPart._listName;
            //displayAvatarPicture.SelectedItem.Value = webPart._DisplayAvatarPicture;
            //listingPostType.SelectedItem.Value = webPart._ListingPostType;
            pageTitle.Text = webPart._pageTitle;
            numberOfPosts.Text = webPart._NumberOfPosts;
        }

    }
}
