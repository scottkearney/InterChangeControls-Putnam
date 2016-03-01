using System.ComponentModel;
using System.Web.UI.WebControls.WebParts;

namespace Akumina.WebParts.DiscussionBoard.DiscussionBoardListing
{
    public class ListingDiscussionBaseWebPart : DiscussionBaseWebPart
    {
        public bool display_AvatarPicture = true;
        [Category("Akumina Properties"),
        Personalizable(PersonalizationScope.Shared),
        WebBrowsable(true),
        WebDisplayName("Display Avatar Picture"),
        WebDescription("Display Avatar Picture")]
        public bool DisplayAvatarPicture
        {
            get
            {
                return display_AvatarPicture;
            }
            set
            {
                display_AvatarPicture = value;
            }
        }

        public int _NumberOfPosts;
        [Category("Akumina Properties"),
        Personalizable(PersonalizationScope.Shared),
        WebBrowsable(true),
        WebDisplayName("Number Of Posts"),
        WebDescription("Number of posts to be displayed per page")]
        public int NumberOfPosts
        {
            get
            {
                if (_NumberOfPosts == 0)
                    _NumberOfPosts = 5;
                return _NumberOfPosts;
            }
            set
            {
                _NumberOfPosts = value;
            }
        }

        public enum EnumPostType
        {
            OriginalPost,
            LastPost
        }
        public EnumPostType ListingPostType = EnumPostType.OriginalPost;
        [Category("Akumina Properties"),
        Personalizable(PersonalizationScope.Shared),
        WebBrowsable(true),
        WebDisplayName("Listing Post Type"),
        WebDescription("Listing Post Type(Recent Discussion Post or Original Discussion Post)")]


        public EnumPostType _ListingPostType
        {
            get
            {
                return ListingPostType;
            }
            set
            {
                ListingPostType = value;
            }
        }

        public int _DiscussionTitleTextCount;
        [Category("Akumina Properties"),
        Personalizable(PersonalizationScope.Shared),
        WebBrowsable(true),
        WebDisplayName("Discussion Title Text Count"),
        WebDescription("Discussion Title Text Count")]
        public int DiscussionTitleTextCount
        {
            get
            {
                if (_DiscussionTitleTextCount == 0)
                    _DiscussionTitleTextCount = 20;
                return _DiscussionTitleTextCount;
            }
            set
            {
                _DiscussionTitleTextCount = value;
            }
        }

        public int _DiscussionPreviewTextCount;
        [Category("Akumina Properties"),
        Personalizable(PersonalizationScope.Shared),
        WebBrowsable(true),
        WebDisplayName("Discussion Preview Text Count"),
        WebDescription("Discussion Preview Text Count")]
        public int DiscussionPreviewTextCount
        {
            get
            {
                if (_DiscussionPreviewTextCount == 0)
                    _DiscussionPreviewTextCount = 100;
                return _DiscussionPreviewTextCount;
            }
            set
            {
                _DiscussionPreviewTextCount = value;
            }
        }
    }
}