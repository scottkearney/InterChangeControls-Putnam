using System.ComponentModel;
using System.Web.UI.WebControls.WebParts;

namespace Akumina.WebParts.DiscussionBoard.DiscussionSummary
{
    public class SummaryDiscussionBaseWebPart : DiscussionBaseWebPart
    {
        public bool display_AvatarPicture=true;
        [Category("Akumina Properties"),
        Personalizable(PersonalizationScope.Shared),
        WebBrowsable(true),
        WebDisplayName("Summary Display Avatar Picture"),
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

        public int NumberOfPosts;
        [Category("Akumina Properties"),
        Personalizable(PersonalizationScope.Shared),
        WebBrowsable(true),
        WebDisplayName("Number Of Summary Posts"),
        WebDescription("Number Of Summary Posts")]
        public int _NumberOfPosts
        {
            get
            {
                if (NumberOfPosts==0)
                    NumberOfPosts = 5;
                return NumberOfPosts;
            }
            set
            {
                NumberOfPosts = value;
            }
        }

        public enum EnumPostType
        {
            OriginalPost,
            LastPost
        }
        public EnumPostType _ListingPostType = EnumPostType.OriginalPost;
        [Category("Akumina Properties"),
        Personalizable(PersonalizationScope.Shared),
        WebBrowsable(true),
        WebDisplayName("Summary Listing Post Type"),
        WebDescription("Summary Listing Post Type(Recent Discussion Post or Origirnal Discussion Post)")]


        public EnumPostType ListingPostType
        {
            get
            {
                return _ListingPostType;
            }
            set
            {
                _ListingPostType = value;
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