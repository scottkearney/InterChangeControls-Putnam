using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace Akumina.WebParts.DocumentsSandbox.DocumentTab
{
    internal class WpEditor : EditorPart
    {
        private TextBox _txtLibraryName;
        private TextBox _txtNumOfDays;
        private TextBox _txtPopularFiles;
        private TextBox _txtInstruction;
        private TextBox _txtNumOfFiles;
        private TextBox _txtNumOfRecentFiles;
        //private TextBox _txtNumOfPopularFiles;


        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            _txtNumOfDays = new TextBox { Text = "5" };
            _txtPopularFiles = new TextBox { Text = "30" };
            _txtLibraryName = new TextBox { Text = "Akumina" };
            _txtInstruction = new TextBox { Text = "" };
            _txtNumOfFiles = new TextBox { Text = "" };
            _txtNumOfRecentFiles = new TextBox { Text = "" };
            //_txtNumOfPopularFiles = new TextBox { Text = "" };
        }

        protected override void CreateChildControls()
        {
            base.CreateChildControls();
            Controls.Add(new LiteralControl("Enter the Instructionset<br/>"));
            Controls.Add(_txtInstruction);
            Controls.Add(new LiteralControl("<br/>"));

            Controls.Add(new LiteralControl("Tab.Number of Recent Files<br />"));
            Controls.Add(_txtNumOfRecentFiles);
            Controls.Add(new LiteralControl("<br/>"));

            //Controls.Add(new LiteralControl("Tab.Number of Popular Files<br/>"));
            //Controls.Add(_txtNumOfPopularFiles);
            //Controls.Add(new LiteralControl("<br/>"));

            //Controls.Add(new LiteralControl("Enter No of Days for Recent files<br/>"));
            //Controls.Add(_txtNumOfDays);
            //Controls.Add(new LiteralControl("<br/>"));

            //Controls.Add(new LiteralControl("Enter No of Days for Popular files<br/>"));
            //Controls.Add(_txtPopularFiles);
            //Controls.Add(new LiteralControl("<br/>"));

            Controls.Add(new LiteralControl("Enter the List Name<br/>"));
            Controls.Add(_txtLibraryName);
            Controls.Add(new LiteralControl("<br/>"));
        }

        public override bool ApplyChanges()
        {
            //var webPart = WebPartToEdit as DocumentTab;
            //if (webPart != null)
            //{
            //    webPart.InstructionSet = _txtInstruction.Text;
            //    webPart.ListName = _txtLibraryName.Text;
            //    webPart.NumOfDays = _txtNumOfDays.Text;
            //    //webPart.NumOfPopularFiles = _txtPopularFiles.Text;
            //    //webPart.NumOfFiles = _txtNumOfFiles.Text;
            //    //webPart.TabNumOfPopularFiles = _txtNumOfPopularFiles.Text;
            //    webPart.TabNumOfRecentFiles = _txtNumOfRecentFiles.Text;
            //}
            return true;
        }

        public override void SyncChanges()
        {
            //var webPart = WebPartToEdit as DocumentTab;
            ////if (_txtInstruction.Text != "")
            ////{
            ////    webPart.InstructionSet = _txtInstruction.Text;

            ////}
            ////else
            ////{

            //    if (webPart != null)
            //    {
            //        _txtInstruction.Text = webPart.InstructionSet;
            //        _txtLibraryName.Text = webPart.ListName;
            //        _txtNumOfDays.Text = webPart.NumOfDays;
            //        _txtPopularFiles.Text = webPart.NumOfPopularFiles;
            //        //_txtNumOfFiles.Text = webPart.NumOfFiles;
            //        //_txtNumOfPopularFiles.Text = webPart.TabNumOfPopularFiles;
            //        _txtNumOfRecentFiles.Text = webPart.TabNumOfRecentFiles;
            //    }
            ////}
        }
    }
}