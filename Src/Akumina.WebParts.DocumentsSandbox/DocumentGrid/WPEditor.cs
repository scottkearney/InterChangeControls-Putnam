using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace Akumina.WebParts.DocumentsSandbox.DocumentGrid
{
    public class WpEditor : EditorPart
    {
        private TextBox _txtHeader;
        private TextBox _txtLibraryName;
        private TextBox _txtInstruction;
        private TextBox _txtStylelibrary;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            _txtHeader = new TextBox { Text = "Title,Modified,Modified By" };
            _txtLibraryName = new TextBox { Text = "Akumina" };
            _txtInstruction = new TextBox { Text = "" };
            _txtStylelibrary = new TextBox { Text = "" };
        }

        protected override void CreateChildControls()
        {
            base.CreateChildControls();
            Controls.Add(new LiteralControl("Enter the Instructionset<br/>"));
            Controls.Add(_txtInstruction);
            Controls.Add(new LiteralControl("<br/>"));

            Controls.Add(new LiteralControl("Enter the Header Text<br/>"));
            Controls.Add(_txtHeader);
            Controls.Add(new LiteralControl("<br/>"));

            Controls.Add(new LiteralControl("Enter the List Name<br/>"));
            Controls.Add(_txtLibraryName);
            Controls.Add(new LiteralControl("<br/>"));

            Controls.Add(new LiteralControl("Enter the Script Path <br/>"));
            Controls.Add(_txtStylelibrary);
            Controls.Add(new LiteralControl("<br/>"));
        }

        public override bool ApplyChanges()
        {


            var webPart = WebPartToEdit as DocumentGrid;
            if (webPart != null)
            {
                webPart.InstructionSet = _txtInstruction.Text;
                webPart.ListName = _txtLibraryName.Text;
                //webPart.HeaderName = _txtHeader.Text;
                webPart.RootResourcePath = _txtStylelibrary.Text;
            }
            return true;
        }

        public override void SyncChanges()
        {
            var webPart = WebPartToEdit as DocumentGrid;


            if (webPart != null)
            {
                _txtInstruction.Text = webPart.InstructionSet;
                _txtLibraryName.Text = webPart.ListName;
               // _txtHeader.Text = webPart.HeaderName;
                _txtStylelibrary.Text = webPart.RootResourcePath;
            }




        }
    }
}