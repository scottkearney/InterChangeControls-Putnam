using System.ComponentModel;
using System.Web.UI.WebControls.WebParts;
using Akumina.InterAction;
using Akumina.WebParts.Banner.Banner;
using Microsoft.SharePoint.WebPartPages;
using Microsoft.SharePoint;
using System;

namespace Akumina.WebParts.Banner
{
    public class BannerBaseWebPart : AkuminaBaseWebPart
    {
        #region Properties
        private const string ResultResouceConst = "Banner_AK";
        private string _resultSourceId = ResultResouceConst;
        private int _maxSlideCount = 4;
        private bool _infiniteLoop = true;
        private bool _autoPlay = true;
        private bool _showNavigator = true;
        private TransitionEffect _transitionEffect = TransitionEffect.Fade;
        private int _slideDuration = 5000;
        private int _h1MaxCharacters = 80;
        private int _h2MaxCharacters = 80;
        private int _buttonMaxCharacters = 60;

        /// <summary>
        ///     String containing the name of the result source to pull banner results from.
        /// </summary>
        [Category("Akumina InterAction"), WebDisplayName("Query Type"), WebBrowsable(true), Personalizable(PersonalizationScope.Shared)]
        public QueryType QueryType { get; set; }
        /// <summary>
        ///     String containing the name of the result source to pull banner results from.
        /// </summary>
        [Category("Akumina InterAction"), WebDisplayName("Result source Id"), DefaultValue(ResultResouceConst), WebBrowsable(true), Personalizable(PersonalizationScope.Shared)]
        public string ResultSourceId
        {
            get { return _resultSourceId; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    _resultSourceId = value;
                    if (QueryType == QueryType.SearchQuery && value.IndexOf("-", StringComparison.Ordinal) < 0)
                    {
                        _resultSourceId = "";
                        throw new WebPartPageUserException(
                        "Guid should contain 32 digits with 4 dashes (xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx)");
                    }
                }

                else
                {
                    _resultSourceId = "";
                }

            }
        }

        /// <summary>
        ///     Integer denoting maximum records to return from search. (Zero or negative means return all.)
        /// </summary>
        [Category("Akumina InterAction"), WebDisplayName("Max Slide Count"), WebBrowsable(true), Personalizable(PersonalizationScope.Shared)]
        public int MaxSlideCount
        {
            get
            {
                return _maxSlideCount;
            }
            set
            {
                _maxSlideCount = value;
            }
        }

        /// <summary>
        ///     Boolean indicating whether the banner should continuously rotate.
        /// </summary>
        [Category("Akumina InterAction"), WebDisplayName("Infinite Loop"), WebBrowsable(true), Personalizable(PersonalizationScope.Shared)]
        public bool InfiniteLoop
        {
            get
            {
                return _infiniteLoop;
            }
            set
            {
                _infiniteLoop = value;
            }
        }

        /// <summary>
        ///     Boolean indicating whether or not the banner should automatically start transitions.
        /// </summary>
        [Category("Akumina InterAction"), WebDisplayName("Auto Play"), WebBrowsable(true), Personalizable(PersonalizationScope.Shared)]
        public bool AutoPlay
        {
            get
            {
                return _autoPlay;
            }
            set
            {
                _autoPlay = value;
            }
        }

        /// <summary>
        ///     Boolean indicating if the slide navigator should show.
        /// </summary>
        [Category("Akumina InterAction"), WebDisplayName("Show Navigator"), WebBrowsable(true), Personalizable(PersonalizationScope.Shared)]
        public bool ShowNavigator
        {
            get
            {
                return _showNavigator;
            }
            set
            {
                _showNavigator = value;
            }
        }

        /// <summary>
        ///     String indicating transition effect to apply between slides. Valid values "Slide" and "Fade".
        /// </summary>
        [Category("Akumina InterAction"), WebDisplayName("Transition Effect"), WebBrowsable(true), Personalizable(PersonalizationScope.Shared)]
        public TransitionEffect TransitionEffect
        {
            get
            {
                return _transitionEffect;
            }
            set
            {
                _transitionEffect = value;
            }
        }

        /// <summary>
        ///     Integer denoting in milliseconds the duration each slide appears.
        /// </summary>
        [Category("Akumina InterAction"), WebDisplayName("Slide Duration"), WebBrowsable(true), Personalizable(PersonalizationScope.Shared)]
        public int SlideDuration
        {
            get
            {
                return _slideDuration;
            }
            set
            {
                _slideDuration = value;
            }
        }

        /// <summary>
        ///     String indicating the text color (White, Black or Grey) to apply to Heading and SubHeading elements.
        /// </summary>
        [Category("Akumina InterAction"), WebDisplayName("Text Color Theme"), WebBrowsable(true), Personalizable(PersonalizationScope.Shared)]
        public ColorTheme TextColorTheme { get; set; }

        /// <summary>
        ///     String indicating the location (e.g. Top-left, Middle-center) of the Heading, SubHeading and Link Button.
        /// </summary>
        [Category("Akumina InterAction"), WebDisplayName("Text Location"), WebBrowsable(true), Personalizable(PersonalizationScope.Shared)]
        public Location TextLocation { get; set; }

        /// <summary>
        ///     String indicating the text alignment (Left, Right or Center)
        /// </summary>
        [Category("Akumina InterAction"), WebDisplayName("Text Alignment"), WebBrowsable(true), Personalizable(PersonalizationScope.Shared)]
        public Alignment TextAlignment { get; set; }

        /// <summary>
        ///     Integer indicating the maximum number of characters in the Heading to display. (Zero or negative means no limit.)
        /// </summary>
        [Category("Akumina InterAction"), WebDisplayName("H1 Max Characters"), WebBrowsable(true), Personalizable(PersonalizationScope.Shared)]
        public int H1MaxCharacters
        {
            get
            {
                return _h1MaxCharacters;
            }
            set
            {
                _h1MaxCharacters = value;
            }
        }

        /// <summary>
        ///     Integer indicating the maximum number of characters in the SubHeading to display. (Zero or negative means no
        ///     limit.)
        /// </summary>
        [Category("Akumina InterAction"), WebDisplayName("H2 Max Characters"), WebBrowsable(true), Personalizable(PersonalizationScope.Shared)]
        public int H2MaxCharacters
        {
            get
            {
                return _h2MaxCharacters;
            }
            set
            {
                _h2MaxCharacters = value;
            }
        }

        /// <summary>
        ///     Integer indicating the maximum number of characters in the Button to display. (Zero or negative means no limit.)
        /// </summary>
        [Category("Akumina InterAction"), WebDisplayName("Button Max Characters"), WebBrowsable(true), Personalizable(PersonalizationScope.Shared)]
        public int ButtonMaxCharacters
        {
            get
            {
                return _buttonMaxCharacters;
            }
            set
            {
                _buttonMaxCharacters = value;
            }
        }

        /// <summary>
        ///     Css class to be applied to the outer most container.
        /// </summary>
        [Category("Akumina InterAction"), WebDisplayName("Web Part Theme"), WebBrowsable(true), Personalizable(PersonalizationScope.Shared)]
        public string WebPartTheme { get; set; }

        /// <summary>
        ///     Css class applied to the anchor representing the link button.
        /// </summary>
        [Category("Akumina InterAction"), WebDisplayName("Link Button Theme"), WebBrowsable(true), Personalizable(PersonalizationScope.Shared)]
        public string LinkButtonTheme { get; set; }

        /// <summary>
        ///     Css class applied to the text within the link button.
        /// </summary>
        [Category("Akumina InterAction"), WebDisplayName("Link Button Text Theme"), WebBrowsable(true), Personalizable(PersonalizationScope.Shared)]
        public string LinkButtonTextTheme { get; set; }


        private string _titleIconImageUrl = "";
        public override string TitleIconImageUrl
        {
            get
            {                
                return _titleIconImageUrl;
            }
            set { _titleIconImageUrl = value; }
        }

        private string _catalogIconImageUrl = "";
        public override string CatalogIconImageUrl
        {
            get
            {
                //try
                //{
                //    if (string.IsNullOrEmpty(_catalogIconImageUrl) || (_catalogIconImageUrl != null && _catalogIconImageUrl.Equals("Akumina.WebParts.Banner/icon/ia-banner.png")))
                //        _catalogIconImageUrl = SPContext.Current.Site.Url.TrimEnd('/') + "/" + _catalogIconImageUrl;
                //}
                //catch (Exception)
                //{
                //    // ignored
                //}
                return _catalogIconImageUrl;
            }
            set { _catalogIconImageUrl = value; }
        }


        [Category("Akumina InterAction"), WebDisplayName("Display Tiles"), WebBrowsable(true), Personalizable(PersonalizationScope.Shared)]
        public bool DisplayTiles { get; set; }

        #endregion
    }
}