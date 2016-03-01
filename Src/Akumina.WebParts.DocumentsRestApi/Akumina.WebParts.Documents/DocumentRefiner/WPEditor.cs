using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace Akumina.InterAction.DocumentWebPart.DocumentRefiner
{
    internal class WpEditor : EditorPart
    {
        private TextBox _txtInstruction;
        private TextBox _txtLibraryName;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            _txtLibraryName = new TextBox {Text = "Akumina"};
            _txtInstruction = new TextBox {Text = ""};
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

        public override bool ApplyChanges()
        {
            var webPart = WebPartToEdit as DocumentRefiner;
            if (webPart != null)
            {
                webPart.ListName = _txtLibraryName.Text;
            }
            return true;
        }

        public override void SyncChanges()
        {
            var webPart = WebPartToEdit as DocumentRefiner;
            if (_txtInstruction.Text != "")
            {
                webPart.InstructionSet = _txtInstruction.Text;
            }
            else
            {
                if (webPart != null)
                {
                    _txtLibraryName.Text = webPart.ListName;
                    _txtInstruction.Text = webPart.InstructionSet;
                }
            }
        }
    }
}