using System.ComponentModel;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint.WebPartPages;
using Akumina.InterAction;
using Akumina.WebParts.DocumentSummaryList;

namespace Akumina.WebParts.DocumentSummaryList.DocumentSummaryList
{
    public class DocumentSummaryListBaseWebPart : AkuminaBaseWebPart
    {
        public string _infoTextNewestTab, _infoTextRecentTab, _infoTextPopularTab, _infoTextRecommendedTab = "";
        //public int _tabList, _infoTextNewestTab, _infoTextRecentTab, _infoTextPopularTab, _infoTextRecommendedTab;

        private const int const_NumberOfPopularDays = 30;
        
        private const int const_NumberOfDocsNewest = 20;
        private int _numberOfDocsNewest = 20;
        private const int const_NumberOfDocsMyRecent = 20;
        private int _numberOfDocsMyRecent = 20;
        private const int const_NumberOfDocsPopular = 20;
        private int _numberOfDocsPopular = 20;
        private const int const_NumberOfDocsRecommended = 20;
        private int _numberOfDocsRecommended = 20;

        private int _numberOfPopularDays = const_NumberOfPopularDays;
        private const string const_tabListTarget = "Documents";
        private string _tabListTarget = const_tabListTarget;
        private const string const_tabList = "Newest,My Recent,Popular,Recommended";
        private string _tabList = const_tabList;
        private const bool const_CurrentSite = false;
        private bool _currentSite = const_CurrentSite;


        [Category("Akumina InterAction"), WebBrowsable(true), DefaultValue(const_tabListTarget), WebDisplayName("Target Document Library "), Personalizable(PersonalizationScope.Shared)]
        public string TargetDocumentLibrary { get { return _tabListTarget; } set { _tabListTarget = value; } }
        /// <summary>
        ///    
        /// </summary>
        [Category("Akumina InterAction"), WebDisplayName("Icon"), WebBrowsable(true), Personalizable(PersonalizationScope.Shared)]
        public Icons Icon { get; set; }

        /// <summary>        
        /// This parameter determines the tabs to display and the order in which they appear.  At least one tab must be selected.
        /// </summary>
        [Category("Akumina InterAction"), WebBrowsable(true), WebDisplayName("Enter Tab List"), DefaultValue(const_tabList), WebDescription("Ex:Newest,My Recent,Popular,Recommended"), Personalizable(PersonalizationScope.Shared)]
        public string TabList
        {
            get { return _tabList; }
            set
            {

                if (!string.IsNullOrEmpty(value))
                    _tabList = value;
                else
                    throw new WebPartPageUserException(
                        "Please enter at least one value");

            }
        }


        /// <summary>        
        /// This parameter determines the maximum number Of Files to display in the Newest list.
        /// </summary>
        [Category("Akumina InterAction"), WebBrowsable(true), DefaultValue(const_NumberOfDocsNewest), WebDisplayName("Newest Number Of Files"), Personalizable(PersonalizationScope.Shared)]
        public int NumberOfSitesNewest { get { return _numberOfDocsNewest; } set { _numberOfDocsNewest = value; } }

        /// <summary>        
        /// This parameter determines the maximum number Of Files to display in the My Recent list.
        /// </summary>
        [Category("Akumina InterAction"), WebBrowsable(true),  DefaultValue(const_NumberOfDocsMyRecent), WebDisplayName("Recent Number Of Files"),Personalizable(PersonalizationScope.Shared)]
        public int NumberOfSitesMyRecent { get { return _numberOfDocsMyRecent; } set { _numberOfDocsMyRecent = value; } }


        /// <summary>        
        /// This parameter determines the maximum number Of Files to display in the Popular list.
        /// </summary>
        [Category("Akumina InterAction"), WebBrowsable(true), DefaultValue(const_NumberOfDocsPopular), WebDisplayName("Popular Number Of Files"), Personalizable(PersonalizationScope.Shared)]
        public int NumberOfSitesPopular { get { return _numberOfDocsPopular; } set { _numberOfDocsPopular = value; } }



        /// <summary>        
        /// This parameter determines the maximum number Of Files to display in the Recommended list.
        /// </summary>
        [Category("Akumina InterAction"), WebBrowsable(true), DefaultValue(const_NumberOfDocsRecommended), WebDisplayName("Recommended Number Of Files"),Personalizable(PersonalizationScope.Shared)]
        public int NumberOfSitesRecommended { get { return _numberOfDocsRecommended; } set { _numberOfDocsRecommended = value; } }
        

        /// <summary>        
        /// This parameter determines the maximum number Of Files to display in the Newest list.
        /// </summary>
        [Category("Akumina InterAction"), WebBrowsable(true), WebDisplayName("Newest Information Text"),Personalizable(PersonalizationScope.Shared)]
        public string InfoTextNewestTab { get { return _infoTextNewestTab; } set { _infoTextNewestTab = value; } }

        /// <summary>        
        /// This parameter determines the maximum number Of Files to display in the My Recent list.
        /// </summary>
        [Category("Akumina InterAction"), WebBrowsable(true), WebDisplayName("Recent Information Text"), Personalizable(PersonalizationScope.Shared)]
        public string InfoTextRecentTab { get { return _infoTextRecentTab; } set { _infoTextRecentTab = value; } }


        /// <summary>        
        /// This parameter determines the maximum number Of Files to display in the Popular list.
        /// </summary>
        [Category("Akumina InterAction"), WebBrowsable(true), WebDisplayName("Popular Information Text"), Personalizable(PersonalizationScope.Shared)]
        public string InfoTextPopularTab { get { return _infoTextPopularTab; } set { _infoTextPopularTab = value; } }


        /// <summary>        
        /// This parameter determines the maximum number Of Files to display in the Recommended list.
        /// </summary>
        [Category("Akumina InterAction"), WebBrowsable(true), WebDisplayName("Recommended Information Text"), Personalizable(PersonalizationScope.Shared)]
        public string InfoTextRecommendedTab { get { return _infoTextRecommendedTab; } set { _infoTextRecommendedTab = value; } }

        /// <summary>        
        /// This parameter determines the maximum number Of Files to display in the Popular list.
        /// </summary>
        [Category("Akumina InterAction"), WebBrowsable(true), WebDisplayName("Popular Number Of Days"), DefaultValue(const_NumberOfPopularDays), Personalizable(PersonalizationScope.Shared)]
        public int NumberOfDaysPopular { get { return _numberOfPopularDays; } set { _numberOfPopularDays = value; } }

        /// <summary>        
        /// This parameter filters to the curent site only, and will hide the site column.
        /// </summary>
        [Category("Akumina InterAction"), WebBrowsable(true), WebDisplayName("Current Site"), DefaultValue(const_CurrentSite), Personalizable(PersonalizationScope.Shared)]
        public bool CurrentSite { get { return _currentSite; } set { _currentSite = value; } }

        protected void MapInstructionSetToProperties(InstructionResponse response, DocumentSummaryList webPart)
        {
            webPart.TargetDocumentLibrary = response.GetValue("TargetDocumentLibrary", webPart.TargetDocumentLibrary);
            webPart.TabList = response.GetValue("TabList", webPart.TabList);
            webPart.NumberOfSitesNewest = response.GetValue("NumberOfDocsNewest", webPart.NumberOfSitesNewest);
            webPart.NumberOfSitesMyRecent = response.GetValue("NumberOfDocsMyRecent", webPart.NumberOfSitesMyRecent);
            webPart.NumberOfSitesPopular = response.GetValue("NumberOfDocsPopular", webPart.NumberOfSitesPopular);
            webPart.NumberOfSitesRecommended = response.GetValue("NumberOfDocsRecommended", webPart.NumberOfSitesRecommended);
            webPart.InfoTextNewestTab = response.GetValue("InfoTextNewestTab", webPart.InfoTextNewestTab);
            webPart.InfoTextRecentTab = response.GetValue("InfoTextRecentTab", webPart.InfoTextRecentTab);
            webPart.InfoTextPopularTab = response.GetValue("InfoTextPopularTab", webPart.InfoTextPopularTab);
            webPart.InfoTextRecommendedTab = response.GetValue("InfoTextRecommendedTab", webPart.InfoTextRecommendedTab);
            webPart.NumberOfDaysPopular = response.GetValue("NumberOfDaysPopular", webPart.NumberOfDaysPopular);
        }
    }
}
