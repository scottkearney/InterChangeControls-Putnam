using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace Akumina.WebParts.DocumentSummaryList
{
    public sealed class WpEditor : EditorPart
    {
        public WpEditor(string id, string title, string groupingText)
        {
            ID = id;
            Title = title;
            GroupingText = groupingText;

        }

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
        private TextBox _rootResourcePath;
        private TextBox _tabList;
        private TextBox _numberOfSitesNewest;
        private TextBox _numberOfSitesMyRecent;
        private TextBox _numberOfSitesPopular;
        private TextBox _numberOfSitesRecommended;
        private TextBox _tabRecommendedListName;
        private TextBox _targetDocumentLibrary;
        private TextBox _tabRecommendedListUrlFieldName;
        private TextBox _infoTextRecentTab;
        private TextBox _infoTextPopularTab;
        private TextBox _infoTextRecommendedTab;
        private TextBox _NumberOfDaysPopular;
        private TextBox _InfoTextNewestTab;
    
        private DropDownList _drpTransition;
        

        #endregion

        #region Event Handlers

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            
            _rootResourcePath= new TextBox();
            _tabList= new TextBox();
            _numberOfSitesNewest= new TextBox();
            _numberOfSitesMyRecent= new TextBox();
            _numberOfSitesPopular= new TextBox();
            _numberOfSitesRecommended= new TextBox();
            _tabRecommendedListName= new TextBox();
            _tabRecommendedListUrlFieldName= new TextBox();
            _infoTextRecentTab= new TextBox();
            _infoTextPopularTab= new TextBox();
            _infoTextRecommendedTab= new TextBox();
            _NumberOfDaysPopular= new TextBox();
            _InfoTextNewestTab = new TextBox();
            _targetDocumentLibrary = new TextBox();

            //_drpTransition = new DropDownList();
        
        }

        protected override void CreateChildControls()
        {
            base.CreateChildControls();

            AddChildControl("Enter The Resource Path ", _rootResourcePath);
            AddChildControl("Enter The Target Document Library", _targetDocumentLibrary);            
            AddChildControl("Enter Tab List ", _tabList);
            Controls.Add(new LiteralControl("<div style=\"width:100%\" class=\"UserDottedLine\"></div>"));
            AddChildControl("Newest Information Text ", _InfoTextNewestTab);
            AddChildControl("Newest Number of Files ", _numberOfSitesNewest);
            Controls.Add(new LiteralControl("<div style=\"width:100%\" class=\"UserDottedLine\"></div>"));
            AddChildControl("My Recent Information Text ", _infoTextRecentTab);
            AddChildControl("My Recent Number of Files ", _numberOfSitesMyRecent);            
            Controls.Add(new LiteralControl("<div style=\"width:100%\" class=\"UserDottedLine\"></div>"));
            AddChildControl("Popular Information Text ", _infoTextPopularTab);
            AddChildControl("Popular Number of Files ", _numberOfSitesPopular);
            AddChildControl("Popular Number of Days ", _NumberOfDaysPopular);
            Controls.Add(new LiteralControl("<div style=\"width:100%\" class=\"UserDottedLine\"></div>"));
            AddChildControl("Recommended List Name ", _tabRecommendedListName);
            AddChildControl("Recommended Information Text", _infoTextRecommendedTab);
            AddChildControl("Recommended Number of Files ", _numberOfSitesRecommended);                      
            Controls.Add(new LiteralControl("<div style=\"width:100%\" class=\"UserDottedLine\"></div>"));

            

            //TODO: Is there a way to iterate thru a list of "instructions"?                        
            ////var transitions = Resources.pdl_Transistions.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            ////foreach (var transition in transitions)
            ////{
            ////    var nvp = transition.Split('~');
            ////    _drpTransition.Items.Add(new ListItem { Text = nvp[0], Value = nvp[1] });
            //}
            //Controls.Add(new LiteralControl(Resources.lbl_Transition));
            //Controls.Add(_drpTransition);

            
            
            
            

            
        }
        private void AddBeginFieldSet(string legend) {
            Controls.Add(new LiteralControl(string.Format("<fieldset><legend>{0}</legend>",legend)));
        }
        //private void AddEndFieldSet()
        //{
        //    Controls.Add(new LiteralControl(string.Format("<fieldset><legend>{0}</legend>", legend)));
        //}

        public override bool ApplyChanges()
        {
            var webPart = WebPartToEdit as DocumentSummaryList.DocumentSummaryList;
            if (webPart != null)
            {
                webPart.RootResourcePath=_rootResourcePath.Text;
                webPart.TabList=_tabList.Text;
                webPart.NumberOfSitesNewest= !string.IsNullOrEmpty(_numberOfSitesNewest.Text) ? Convert.ToInt32(_numberOfSitesNewest.Text) : 0;
                webPart.NumberOfSitesMyRecent= ! string.IsNullOrEmpty(_numberOfSitesMyRecent.Text) ?  Convert.ToInt32(_numberOfSitesMyRecent.Text) : 0;
                webPart.NumberOfSitesPopular= ! string.IsNullOrEmpty(_numberOfSitesPopular.Text) ? Convert.ToInt32(_numberOfSitesPopular.Text) : 0 ;
                webPart.NumberOfSitesRecommended= ! string.IsNullOrEmpty(_numberOfSitesRecommended.Text) ?Convert.ToInt32(_numberOfSitesRecommended.Text) : 0;
                webPart.TabRecommendedListName=_tabRecommendedListName.Text;
                webPart.InfoTextRecentTab=_infoTextRecentTab.Text;
                webPart.InfoTextPopularTab=_infoTextPopularTab.Text;
                webPart.InfoTextRecommendedTab=_infoTextRecommendedTab.Text;
                webPart.NumberOfDaysPopular=!string.IsNullOrEmpty(_NumberOfDaysPopular.Text)? Convert.ToInt32(_NumberOfDaysPopular.Text) : 0;
                webPart.InfoTextNewestTab=_InfoTextNewestTab.Text;
                webPart.TargetDocumentLibrary = _targetDocumentLibrary.Text;
            }
            return true;
        }

        public override void SyncChanges()
        {
            var webPart = WebPartToEdit as DocumentSummaryList.DocumentSummaryList;
            if (webPart != null)
            {
                _rootResourcePath.Text = webPart.RootResourcePath;
                _tabList.Text = webPart.TabList;
                _numberOfSitesNewest.Text = webPart.NumberOfSitesNewest.ToString();
                _numberOfSitesMyRecent.Text = webPart.NumberOfSitesMyRecent.ToString();
                _numberOfSitesPopular.Text = webPart.NumberOfSitesPopular.ToString();
                _numberOfSitesRecommended.Text = webPart.NumberOfSitesRecommended.ToString();
                _tabRecommendedListName.Text = webPart.TabRecommendedListName;                
                _infoTextRecentTab.Text = webPart.InfoTextRecentTab;
                _infoTextPopularTab.Text = webPart.InfoTextPopularTab;
                _infoTextRecommendedTab.Text = webPart.InfoTextRecommendedTab;
                _NumberOfDaysPopular.Text = webPart.NumberOfDaysPopular.ToString();
                _InfoTextNewestTab.Text = webPart.InfoTextNewestTab;
                _targetDocumentLibrary.Text = webPart.TargetDocumentLibrary;
                //_infoTextNewestTab.Text = webPart._infoTextNewestTab;
              
            }
        }

        #endregion
    }
}