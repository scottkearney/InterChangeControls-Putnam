using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace Akumina.WebParts.Announcement.AnnouncementDetail
{
    public class WpEditor : EditorPart
    {
        private TextBox _txtListName;
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            _txtListName = new TextBox { Text = "" };
        }

        protected override void CreateChildControls()
        {
            base.CreateChildControls();

            Controls.Add(new LiteralControl("Enter List Name<br/>"));
            Controls.Add(_txtListName);
            Controls.Add(new LiteralControl("<br/>"));
        }

        public override bool ApplyChanges()
        {
            var webPart = WebPartToEdit as AnnouncementDetail;
            if (webPart != null)
            {
                webPart.ListName = _txtListName.Text;
            }
            return true;
        }

        public override void SyncChanges()
        {
            var webPart = WebPartToEdit as AnnouncementDetail;
            if (webPart != null)
            {
                _txtListName.Text = webPart.ListName;
            }
        }
    }
}
