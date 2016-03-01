using Akumina.WebParts.LibraryListing.Properties;
using Microsoft.SharePoint;
using System;
using System.ComponentModel;
using System.Text;
using System.Web.UI.WebControls.WebParts;

namespace Akumina.WebParts.LibraryListing.LibraryListing
{
    [ToolboxItemAttribute(false)]
    public partial class LibraryListing : LibraryListingBaseWebPart
    {
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public LibraryListing()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
        }

        private string WriteInitialScript()
        {
            var sb = new StringBuilder();
            string resourcePathValue = RootResourcePath;

            var styleSheets = Resources.StyleSheetsPath.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var sheet in styleSheets)
            {
                sb.AppendLine("<link rel=\"stylesheet\" href=\"" + resourcePathValue + sheet + "\" />");
            }

            var jsFiles = Resources.javascriptPath.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var jsFile in jsFiles)
            {
                if (jsFile.ToLower().Contains("jquery"))
                {
                    sb.AppendLine("<script type=\"text/javascript\">");
                    sb.AppendLine("if (typeof jQuery == 'undefined') { document.write(unescape(\"%3Cscript src='" + resourcePathValue + jsFile + "' type='text/javascript'%3E%3C/script%3E\")); }");
                    sb.AppendLine("</script>");
                }
                sb.AppendLine("<script type=\"text/javascript\" src=\"" + resourcePathValue + jsFile + "\"></script>");
            }

            return sb.ToString();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (string.IsNullOrEmpty(RootResourcePath))
                {
                    RootResourcePath = SPContext.Current.Web.Site.RootWeb.Url + "/Akumina.WebParts.LibraryListing";
                }
            }
            StringBuilder sb = new StringBuilder();
            webTitleValue.Value = SPContext.Current.Web.Title;
            sb.AppendLine(WriteInitialScript());
            litTop.Text = sb.ToString();
            ImagesLibraryValue.Value =SPContext.Current.Web.Url+"/"+_ImageLibrary;
            DMSLandingPageValue.Value = _documentSummarypage;
            SearchRedirectURLValue.Value = _SearchRedirectURL;
            RedirectionOptionValue.Value = _RedirectionOption.ToString();
            listingsValue.Value = _Excludelist;
        }
    }
}
