using Akumina.InterAction;
using System.ComponentModel;
using System.Web.UI.WebControls.WebParts;

namespace Akumina.WebParts.DiscussionBoard
{
    public class DiscussionBaseWebPart : AkuminaBaseWebPart
    {
        //[Category("Akumina Properties"), WebDisplayName("Enter the Instruction Set"), WebBrowsable(true), Personalizable(PersonalizationScope.Shared)]
        //public string InstructionSet { get; set; }
        public string _pageTitle;
        [Category("Akumina InterAction"), WebDisplayName("Enter the Discussion Board Title"), WebBrowsable(true), Personalizable(PersonalizationScope.Shared)]
        public string DiscussionTitle
        {
            get
            {
                if (string.IsNullOrEmpty(_pageTitle))
                    _pageTitle = "Ignite Discussions";
                return _pageTitle;
            }
            set
            {
                _pageTitle = value;
            }
        }
        public string _listName;
        [Category("Akumina InterAction"), WebDisplayName("Enter the Discussion Board List Name"), WebBrowsable(true), Personalizable(PersonalizationScope.Shared)]
        public string DiscussionListName
        {
            get
            {
                if (string.IsNullOrEmpty(_listName))
                    _listName = "Discussions_AK";
                return _listName;
            }
            set
            {
                _listName = value;
            }
        }
        public string _docListName;
        [Category("Akumina InterAction"), WebDisplayName("Enter the Document Library Name"), WebBrowsable(true), Personalizable(PersonalizationScope.Shared)]
        public string DocumentsListName
        {
            get
            {
                if (string.IsNullOrEmpty(_docListName))
                    _docListName = "Documents";
                return _docListName;
            }
            set
            {
                _docListName = value;
            }
        }
        public string _discussionListPageurl;
        [Category("Akumina InterAction"), WebDisplayName("Enter the Discussion Listing Page Url"), WebBrowsable(true), Personalizable(PersonalizationScope.Shared)]
        public string DiscussionListPageUrl
        {
            get
            {
                if (string.IsNullOrEmpty(_discussionListPageurl))
                    _discussionListPageurl = "DiscussList.aspx";
                return _discussionListPageurl;
            }
            set
            {
                _discussionListPageurl = value;
            }
        }
        public string _discussionCreatePageurl;
        [Category("Akumina InterAction"), WebDisplayName("Enter the Discussion Create Page Url"), WebBrowsable(true), Personalizable(PersonalizationScope.Shared)]
        public string DiscussionCreatePageUrl
        {
            get
            {
                if (string.IsNullOrEmpty(_discussionCreatePageurl))
                    _discussionCreatePageurl = "DiscussNew.aspx";
                return _discussionCreatePageurl;
            }
            set
            {
                _discussionCreatePageurl = value;
            }
        }
        public string _discussionThreadPageurl;
        [Category("Akumina InterAction"), WebDisplayName("Enter the Discussion Thread Page Url"), WebBrowsable(true), Personalizable(PersonalizationScope.Shared)]
        public string DiscussionThreadPageUrl
        {
            get
            {
                if (string.IsNullOrEmpty(_discussionThreadPageurl))
                    _discussionThreadPageurl = "DiscussThread.aspx";
                return _discussionThreadPageurl;
            }
            set
            {
                _discussionThreadPageurl = value;
            }
        }
    }
}