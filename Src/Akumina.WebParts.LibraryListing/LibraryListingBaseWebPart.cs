using System.ComponentModel;
using System.Web.UI.WebControls.WebParts;
using Akumina.InterAction;
using Akumina.WebParts.LibraryListing.LibraryListing;
using Microsoft.SharePoint.WebPartPages;
using Microsoft.SharePoint;
using System;

namespace Akumina.WebParts.LibraryListing
{
    public class LibraryListingBaseWebPart : AkuminaBaseWebPart
    {
        #region Properties
        public string Excludelist = "Converted Forms,Documents,Form Templates,Images,List Template Gallery,Master Page Gallery,Pages,Site Assets,Site Collection Documents,Site Collection Images,Site Pages,Solution Gallery,Style Library,Theme Gallery,Translation Packages,Web Part Gallery";
        [Category("Akumina InterAction"),
        Personalizable(PersonalizationScope.Shared),
        WebBrowsable(true),
        WebDisplayName("Exclude List"),
        WebDescription("Exclude List")]
        public string _Excludelist
        {
            get
            {
                return Excludelist;
            }
            set
            {
                Excludelist = value;
            }
        }

        public string documentSummarypage = "SparkDocuments.aspx";
        [Category("Akumina InterAction"),
        Personalizable(PersonalizationScope.Shared),
        WebBrowsable(true),
        WebDisplayName("Document page"),
        WebDescription("Document page")]
        public string _documentSummarypage
        {
            get
            {
                return documentSummarypage;
            }
            set
            {
                documentSummarypage = value;
            }
        }

        public enum EnumRedirection
        {
            SameWindow,
            DifferentWindow
        }
        public EnumRedirection RedirectionOption = EnumRedirection.SameWindow;
        [Category("Akumina InterAction"),
        Personalizable(PersonalizationScope.Shared),
        WebBrowsable(true),
        WebDisplayName("Redirection Options"),
        WebDescription("Redirection Option(Same Window or Different Window)")]

        public EnumRedirection _RedirectionOption
        {
            get
            {
                return RedirectionOption;
            }
            set
            {
                RedirectionOption = value;
            }
        }

        public string SearchRedirectURL = "SparkDocuments.aspx";
        [Category("Akumina InterAction"),
        Personalizable(PersonalizationScope.Shared),
        WebBrowsable(true),
        WebDisplayName("Search Redirect URL"),
        WebDescription("Search Redirect URL")]
        public string _SearchRedirectURL
        {
            get
            {
                return SearchRedirectURL;
            }
            set
            {
                SearchRedirectURL = value;
            }
        }
        
        public string ImageLibrary = "Images_AK";
        [Category("Akumina InterAction"),
        Personalizable(PersonalizationScope.Shared),
        WebBrowsable(true),
        WebDisplayName("Images Library"),
        WebDescription("Images Library")]
        public string _ImageLibrary
        {
            get
            {
                return ImageLibrary;
            }
            set
            {
                ImageLibrary = value;
            }
        }
        #endregion
    }
}