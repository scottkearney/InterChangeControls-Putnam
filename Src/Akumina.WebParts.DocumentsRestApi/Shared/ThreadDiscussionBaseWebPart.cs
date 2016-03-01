using System.ComponentModel;
using System.Web.UI.WebControls.WebParts;

namespace Akumina.WebParts.DiscussionBoard.DiscussionThreadPage
{
    public class ThreadDiscussionBaseWebPart : DiscussionBaseWebPart
    {
        public bool display_AvatarPicture = true;
        [Category("Akumina Properties"), WebDisplayName("Display Avatar Picture"), WebBrowsable(true), DefaultValue(false), Personalizable(PersonalizationScope.Shared)]
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

    }
}