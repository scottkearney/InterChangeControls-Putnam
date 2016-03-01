using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace Akumina.WebParts.DocumentsSandbox.DocumentFolderTree
{
    internal class WpEditor : EditorPart
    {
        private TextBox _txtLibraryName;
        private TextBox _txtInstruction;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            _txtLibraryName = new TextBox();
            _txtLibraryName.Text = "Akumina";
            _txtInstruction = new TextBox { Text = "" };
        }

        protected override void CreateChildControls()
        {
            base.CreateChildControls();
            Controls.Add(new LiteralControl("Enter the Instructionset<br/>"));
            Controls.Add(_txtInstruction);
            Controls.Add(new LiteralControl("<br/>"));

            Controls.Add(new LiteralControl("Enter the List Name<br/>"));
            Controls.Add(_txtLibraryName);
            Controls.Add(new LiteralControl("<br/>"));
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
        }

        public override bool ApplyChanges()
        {
            var webPart = WebPartToEdit as DocumentFolderTree;
            if (webPart != null)
            {
                webPart.InstructionSet = _txtInstruction.Text;
                webPart.ListName = _txtLibraryName.Text;
            }
            return true;
        }

        public override void SyncChanges()
        {
            var webPart = WebPartToEdit as DocumentFolderTree;
            if (webPart != null)
            {
                _txtInstruction.Text = webPart.InstructionSet;
                _txtLibraryName.Text = webPart.ListName;
            }
        }
    }
}