using System.ComponentModel;
using System.Web.UI.WebControls.WebParts;
using Akumina.InterAction;
using Microsoft.SharePoint;
using System;

namespace Akumina.WebParts.ContentBlock
{
    public class ContentBlockBaseWebPart : AkuminaBaseWebPart
    {
        private Themes _colorTheme = Themes.Blue;
        //private string _queryPart = "SiteAlertIDS_AK.1";
        private string _queryPart = "Welcome_AK.1"; //Set to default Content Block List on Department Site
        private bool _dynamicPreview = true;
        private string _siteWideAlertMessage = "";
        public DateTime Expires { get; set; }
        [Category("Akumina InterAction"), WebDisplayName("Query Part"), WebBrowsable(true),
         Personalizable(PersonalizationScope.Shared)]
        public string QueryPart
        {
            get
            {
                return _queryPart;
            }
            set
            {
                _queryPart = value;
            }
        }
        [Category("Akumina InterAction"), WebDisplayName("Color Theme"), WebBrowsable(true),
    Personalizable(PersonalizationScope.Shared)]
        public Themes ColorTheme
        {
            get
            {
                return _colorTheme;
            }
            set
            {
                _colorTheme = value;
            }
        }

        [Category("Akumina InterAction"), WebDisplayName("Site wide Alert Message"), WebBrowsable(true),
         Personalizable(PersonalizationScope.Shared)]
        public string SiteWideAlertMessage
        {
            get
            {
                return _siteWideAlertMessage;
            }
            set
            {
                _siteWideAlertMessage = value;
            }
        }

        [Category("Akumina InterAction"), WebDisplayName("Hide Content Block"), WebBrowsable(true),
         Personalizable(PersonalizationScope.Shared)]
        public Boolean DisableAlert { get; set; }
        
        [Category("Akumina InterAction"), WebDisplayName("Dynamic Preview"), WebBrowsable(true),
         Personalizable(PersonalizationScope.Shared)]
        public Boolean DynamicPreview
        {
            get
            {
                return _dynamicPreview; ;
            }
            set
            {
                _dynamicPreview = value;
            }
        }

        private string _titleIconImageUrl = "";
        public override string TitleIconImageUrl
        {
            get
            {
                try
                {
                    if (string.IsNullOrEmpty(_titleIconImageUrl) || (_titleIconImageUrl != null && _titleIconImageUrl.TrimEnd('/').Equals("Akumina.WebParts.ContentBlock/icon/ia-content-block.png")))
                        _titleIconImageUrl = SPContext.Current.Site.Url.TrimEnd('/') + "/" + _titleIconImageUrl;
                }
                catch (Exception)
                {
                    // ignored
                }

                return _titleIconImageUrl;
            }
            set { _titleIconImageUrl = value; }
        }

        private string _catalogIconImageUrl = "";
        public override string CatalogIconImageUrl
        {
            get
            {                
                return _catalogIconImageUrl;
            }
            set { _catalogIconImageUrl = value; }
        }

        protected void MapInstructionSetToProperties(InstructionResponse response, ContentBlock.ContentBlock webPart)
        {
            webPart.Expires  = response.GetValue("Expires", webPart.Expires);
            webPart.QueryPart = response.GetValue("QueryPart", webPart.QueryPart);
            webPart.RootResourcePath = response.GetValue("RootResourcePath", webPart.RootResourcePath);
            webPart.DisableAlert = !response.GetValue("EnableAlert", webPart.DisableAlert);
            webPart.SiteWideAlertMessage = response.GetValue("SiteWideAlertMessage", webPart.SiteWideAlertMessage);
            webPart.ColorTheme = ParseEnum<Themes>(response.GetValue("ColorTheme", webPart.ColorTheme.ToString()));
        }
        /// <summary>
        ///    
        /// </summary>
        [Category("Akumina InterAction"), WebDisplayName("Icon"), WebBrowsable(true), Personalizable(PersonalizationScope.Shared)]
        public Icons Icon { get; set; }
    }
}