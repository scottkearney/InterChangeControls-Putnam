using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Akumina.InterAction;
using Akumina.WebParts.Banner.Properties;

namespace Akumina.WebParts.Banner
{
    public sealed class WpEditor : EditorPart
    {
        //public WpEditor1(string id, string title, string groupingText)
        //{
        //    ID = id;
        //    Title = title;
        //    GroupingText = groupingText;
        //}

        #region private methods

        /// <summary>
        ///     Adds a given control to the page with a label. Optionally adds a <br /> afterwards.
        /// </summary>
        /// <param name="label">String for the label to the left of the control</param>
        /// <param name="control">Control to add to the page.</param>
        /// <param name="breakAfter">Add a <br /> at the end. Default = true.</param>
        private void AddChildControl(string label, Control control, bool breakAfter = true)
        {
            Controls.Add(new LiteralControl(label));
            Controls.Add(control);
            if (breakAfter) Controls.Add(new LiteralControl("<br />"));
        }

        #endregion

        #region Controls

        private TextBox _txtInstructionSet;
        private TextBox _txtWebPartTheme;
        private TextBox _txtResultSourceId;
        private TextBox _txtLinkButtonTheme;
        private TextBox _txtLinkButtonTextTheme;
        private TextBox _txtRootResourcePath;
        private CheckBox _chkInfiniteLoop;
        private CheckBox _chkAutoPlay;
        private CheckBox _chkShowNavigator;
        private DropDownList _drpTransition;
        private TextBox _txtDuration;

        #endregion

        #region Event Handlers

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            _txtInstructionSet = new TextBox();
            _txtWebPartTheme = new TextBox();
            _txtResultSourceId = new TextBox();
            _txtLinkButtonTextTheme = new TextBox();
            _txtLinkButtonTheme = new TextBox();
            _txtRootResourcePath = new TextBox();
            _chkInfiniteLoop = new CheckBox();
            _chkAutoPlay = new CheckBox();
            _chkShowNavigator = new CheckBox();
            _drpTransition = new DropDownList();
            _txtDuration = new TextBox();
        }

        protected override void CreateChildControls()
        {
            base.CreateChildControls();

            //TODO: Is there a way to iterate thru a list of "instructions"?
            AddChildControl(Resources.lbl_InstructionSet, _txtInstructionSet);
            AddChildControl(Resources.lbl_ResultSource, _txtResultSourceId);
            AddChildControl(Resources.lbl_BannerTheme, _txtWebPartTheme);

            Controls.Add(new LiteralControl(Resources.lbl_InfinitLoop));
            Controls.Add(_chkInfiniteLoop);
            Controls.Add(new LiteralControl("<br />"));
            Controls.Add(new LiteralControl(Resources.lbl_AutoPlay));
            Controls.Add(new LiteralControl("<br />"));
            Controls.Add(_chkAutoPlay);
            Controls.Add(new LiteralControl(Resources.lbl_ShowNavigator));
            Controls.Add(_chkShowNavigator);
            Controls.Add(new LiteralControl("<br />"));

            var transitions = Resources.pdl_Transistions.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var transition in transitions)
            {
                var nvp = transition.Split('~');
                _drpTransition.Items.Add(new ListItem { Text = nvp[0], Value = nvp[1] });
            }
            Controls.Add(new LiteralControl(Resources.lbl_Transition));
            Controls.Add(_drpTransition);
            Controls.Add(new LiteralControl("<br />"));

            AddChildControl(Resources.lbl_SlideDuration, _txtDuration);

            AddChildControl(Resources.lbl_LinkButtonTheme, _txtLinkButtonTextTheme);
            AddChildControl(Resources.lbl_LinkTextTheme, _txtLinkButtonTheme);
            AddChildControl(Resources.lbl_ResourcePath, _txtRootResourcePath);
        }

        public override bool ApplyChanges()
        {
            var webPart = WebPartToEdit as Banner.Banner;
            if (webPart != null)
            {
                webPart.InstructionSet = _txtInstructionSet.Text;
                webPart.WebPartTheme = _txtWebPartTheme.Text;
                webPart.ResultSourceId = _txtResultSourceId.Text;
                //webPart.ResultSourceScope = _drpResultSourceLevel.SelectedItem.Value;
                webPart.LinkButtonTextTheme = _txtLinkButtonTextTheme.Text;
                webPart.LinkButtonTheme = _txtLinkButtonTheme.Text;
                webPart.RootResourcePath = _txtRootResourcePath.Text;
                webPart.InfiniteLoop = _chkInfiniteLoop.Checked;
                webPart.AutoPlay = _chkAutoPlay.Checked;
                webPart.ShowNavigator = _chkShowNavigator.Checked;
                webPart.TransitionEffect = _drpTransition.SelectedItem.Value;
                var tmpInt = 3000;
                Int32.TryParse(_txtDuration.Text, out tmpInt);
                webPart.SlideDuration = tmpInt;
            }
            return true;
        }

        public override void SyncChanges()
        {
            var webPart = WebPartToEdit as Banner.Banner;
            if (webPart != null)
            {
                _txtWebPartTheme.Text = webPart.WebPartTheme;
                _txtResultSourceId.Text = webPart.ResultSourceId;
                _txtLinkButtonTextTheme.Text = webPart.LinkButtonTextTheme;
                _txtLinkButtonTheme.Text = webPart.LinkButtonTheme;
                _txtRootResourcePath.Text = webPart.RootResourcePath;
                _chkInfiniteLoop.Checked = webPart.InfiniteLoop;
                _chkAutoPlay.Checked = webPart.AutoPlay;
                _chkShowNavigator.Checked = webPart.ShowNavigator;
                foreach (ListItem item in _drpTransition.Items)
                {
                    if (item.Value == webPart.TransitionEffect)
                    {
                        item.Selected = true;
                        break;
                    }
                }
                _txtDuration.Text = webPart.SlideDuration.ToString();

                _txtInstructionSet.Text = webPart.InstructionSet;

                if (!string.IsNullOrWhiteSpace(webPart.InstructionSet))
                {
                    IInstructionRepository clientServices = new InstructionRepository();
                    var response = clientServices.Execute(webPart.InstructionSet);

                    if (response != null)
                    {
                        webPart.MapInstructionSetToProperties(response, webPart, clientServices);
                    }
                }
            }
        }

        #endregion
    }
}