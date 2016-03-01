using System.ComponentModel;
using System.Web.UI.WebControls.WebParts;

namespace Akumina.WebParts.SiteSummaryList
{
    public class AkuminaBaseWebPart : WebPart
    {
        public string _infoTextNewestTab, _infoTextRecentTab, _infoTextPopularTab, _infoTextRecommendedTab;
        //public int _tabList, _infoTextNewestTab, _infoTextRecentTab, _infoTextPopularTab, _infoTextRecommendedTab;

        private const int const_NumberOfPopularDays = 30;
        private int _numberOfPopularDays = const_NumberOfPopularDays;

        private const string const_tabList = "Newest,My Recent,Popular,Recommended";
        private string _tabList = const_tabList;

        

        /// <summary>        
        /// This parameter determines the resouce path for js and css files.
        /// </summary>
        [Category("Akumina - Tabs"), WebBrowsable(true), WebDisplayName("Enter The Resource Path "), Personalizable(PersonalizationScope.Shared)]
        public string RootResourcePath { get; set; }

        /// <summary>        
        /// This parameter determines the tabs to display and the order in which they appear.  At least one tab must be selected.
        /// </summary>
        [Category("Akumina - Tabs"), WebBrowsable(true), WebDisplayName("Enter Tab List"), DefaultValue(const_tabList), WebDescription("Ex:Newest,My Recent,Popular,Recommended"), Personalizable(PersonalizationScope.Shared)]
        public string TabList
        {
            get { return _tabList; }
            set
            {

                if (!string.IsNullOrEmpty(value))
                    _tabList = value;
                else
                    throw new Microsoft.SharePoint.WebPartPages.
                        WebPartPageUserException(
                        "Please enter at least one value");

            }
        }


        /// <summary>        
        /// This parameter determines the maximum number of Sites to display in the Newest list.
        /// </summary>
        [Category("Akumina - Newest Tab"), WebBrowsable(true),  WebDisplayName("Number Of Sites"), Personalizable(PersonalizationScope.Shared)]
        public int NumberOfSitesNewest { get; set; }

        /// <summary>        
        /// This parameter determines the maximum number of Sites to display in the My Recent list.
        /// </summary>
        [Category("Akumina - My Recent"), WebBrowsable(true), WebDisplayName("Number Of Sites"),Personalizable(PersonalizationScope.Shared)]
        public int NumberOfSitesMyRecent { get; set; }


        /// <summary>        
        /// This parameter determines the maximum number of Sites to display in the Popular list.
        /// </summary>
        [Category("Akumina - Popular Tab"), WebBrowsable(true),WebDisplayName("Number Of Sites"), Personalizable(PersonalizationScope.Shared)]
        public int NumberOfSitesPopular { get; set; }



        /// <summary>        
        /// This parameter determines the maximum number of Sites to display in the Recommended list.
        /// </summary>
        [Category("Akumina - Recommended Tab"), WebBrowsable(true), WebDisplayName("Number Of Sites"),Personalizable(PersonalizationScope.Shared)]
        public int NumberOfSitesRecommended { get; set; }

        /// <summary>        
        /// This parameter determines the Recommended List Name.
        /// </summary>
        [Category("Akumina - Recommended Tab"), WebBrowsable(true), WebDisplayName("Enter The Recommended List Name "), Personalizable(PersonalizationScope.Shared)]
        public string TabRecommendedListName { get; set; }
        [Category("Akumina - Recommended Tab"), WebBrowsable(true), WebDisplayName("Enter The Recommended List  Url field Name "), Personalizable(PersonalizationScope.Shared)]
        public string TabRecommendedListUrlFieldName { get; set; }

        /// <summary>        
        /// This parameter determines the maximum number of Sites to display in the Newest list.
        /// </summary>
        [Category("Akumina - Newest Tab"), WebBrowsable(true), WebDisplayName("Information Text"),Personalizable(PersonalizationScope.Shared)]
        public string InfoTextNewestTab { get; set; }

        /// <summary>        
        /// This parameter determines the maximum number of Sites to display in the My Recent list.
        /// </summary>
        [Category("Akumina - My Recent"), WebBrowsable(true), WebDisplayName("Information Text"), Personalizable(PersonalizationScope.Shared)]
        public string InfoTextRecentTab { get; set; }


        /// <summary>        
        /// This parameter determines the maximum number of Sites to display in the Popular list.
        /// </summary>
        [Category("Akumina - Popular Tab"), WebBrowsable(true), WebDisplayName("Information Text"), Personalizable(PersonalizationScope.Shared)]
        public string InfoTextPopularTab { get; set; }


        /// <summary>        
        /// This parameter determines the maximum number of Sites to display in the Recommended list.
        /// </summary>
        [Category("Akumina - Recommended Tab"), WebBrowsable(true), WebDisplayName("Information Text"), Personalizable(PersonalizationScope.Shared)]
        public string InfoTextRecommendedTab { get; set; }
        /// <summary>        
        /// This parameter determines the maximum number of Sites to display in the Popular list.
        /// </summary>
        [Category("Akumina - Popular Tab"), WebBrowsable(true), WebDisplayName("Number Of Days"), DefaultValue(const_NumberOfPopularDays), Personalizable(PersonalizationScope.Shared)]
        public int NumberOfDaysPopular { get { return _numberOfPopularDays; } set { _numberOfPopularDays = value; } }



    }
}
