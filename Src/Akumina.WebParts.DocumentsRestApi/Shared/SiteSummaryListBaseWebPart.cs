using System.ComponentModel;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint.WebPartPages;
using WebPart = System.Web.UI.WebControls.WebParts.WebPart;
using Akumina.InterAction;


namespace Akumina.WebParts.SiteSummaryList
{
    public class SiteSummaryListBaseWebPart : AkuminaBaseWebPart
    {
        public string _infoTextNewestTab, _infoTextRecentTab, _infoTextPopularTab, _infoTextRecommendedTab;
        //public int _tabList, _infoTextNewestTab, _infoTextRecentTab, _infoTextPopularTab, _infoTextRecommendedTab;

        private const int const_NumberOfPopularDays = 30;
        private int _numberOfPopularDays = const_NumberOfPopularDays;

        private const string const_tabList = "Newest,My Recent,Popular,Recommended";
        private string _tabList = const_tabList;
        private int _NumberOfSitesNewest = 20;
        private int _NumberOfSitesMyRecent = 20;
        private int _NumberOfSitesPopular = 20;
        private int _NumberOfSitesRecommended = 20;
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
        /// This parameter determines the maximum number of Sites to display in the Newest list.
        /// </summary>
        [Category("Akumina InterAction"), WebBrowsable(true), WebDisplayName("Newest Number Of Sites"), Personalizable(PersonalizationScope.Shared)]
        public int NumberOfSitesNewest
        {
            get
            {
                return _NumberOfSitesNewest;
            }
            set
            {
                _NumberOfSitesNewest = value;
            }
        }

        /// <summary>        
        /// This parameter determines the maximum number of Sites to display in the My Recent list.
        /// </summary>
        [Category("Akumina InterAction"), WebBrowsable(true), WebDisplayName("Recent Number Of Sites"), Personalizable(PersonalizationScope.Shared)]
        public int NumberOfSitesMyRecent
        {
            get
            {
                return _NumberOfSitesMyRecent;
            }
            set
            {
                _NumberOfSitesMyRecent = value;
            }
        }


        /// <summary>        
        /// This parameter determines the maximum number of Sites to display in the Popular list.
        /// </summary>
        [Category("Akumina InterAction"), WebBrowsable(true), WebDisplayName("Popular Number Of Sites"), Personalizable(PersonalizationScope.Shared)]
        public int NumberOfSitesPopular
        {
            get
            {
                return _NumberOfSitesPopular;
            }
            set
            {
                _NumberOfSitesPopular = value;
            }
        }



        /// <summary>        
        /// This parameter determines the maximum number of Sites to display in the Recommended list.
        /// </summary>
        [Category("Akumina InterAction"), WebBrowsable(true), WebDisplayName("Recommended Number Of Sites"), Personalizable(PersonalizationScope.Shared)]
        public int NumberOfSitesRecommended { get {
            return _NumberOfSitesRecommended;
        }
            set {
                _NumberOfSitesRecommended = value;
            }
        }

        
        /// <summary>        
        /// This parameter determines the maximum number of Sites to display in the Newest list.
        /// </summary>
        [Category("Akumina InterAction"), WebBrowsable(true), WebDisplayName("Newest Information Text"),Personalizable(PersonalizationScope.Shared)]
        public string InfoTextNewestTab { get; set; }

        /// <summary>        
        /// This parameter determines the maximum number of Sites to display in the My Recent list.
        /// </summary>
        [Category("Akumina InterAction"), WebBrowsable(true), WebDisplayName("Recent Information Text"), Personalizable(PersonalizationScope.Shared)]
        public string InfoTextRecentTab { get; set; }


        /// <summary>        
        /// This parameter determines the maximum number of Sites to display in the Popular list.
        /// </summary>
        [Category("Akumina InterAction"), WebBrowsable(true), WebDisplayName("Popular Information Text"), Personalizable(PersonalizationScope.Shared)]
        public string InfoTextPopularTab { get; set; }


        /// <summary>        
        /// This parameter determines the maximum number of Sites to display in the Recommended list.
        /// </summary>
        [Category("Akumina InterAction"), WebBrowsable(true), WebDisplayName("Recommended Information Text"), Personalizable(PersonalizationScope.Shared)]
        public string InfoTextRecommendedTab { get; set; }
        /// <summary>        
        /// This parameter determines the maximum number of Sites to display in the Popular list.
        /// </summary>
        [Category("Akumina InterAction"), WebBrowsable(true), WebDisplayName("Popular Number Of Days"), DefaultValue(const_NumberOfPopularDays), Personalizable(PersonalizationScope.Shared)]
        public int NumberOfDaysPopular { get { return _numberOfPopularDays; } set { _numberOfPopularDays = value; } }



    }
}
