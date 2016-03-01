using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web.UI.WebControls.WebParts;
using Akumina.InterAction;
using Akumina.WebParts.SiteLinks.Properties;
using Akumina.WebParts.SiteLinks.SiteLinks;

namespace Akumina.WebParts.SiteLinks
{
    public class SiteLinksBaseWebPart : AkuminaBaseWebPart
    {

        private Icon _Icon = Icon.Linklist;
        private MoreWindow _MoreWindow = MoreWindow.Same;
        private Color _Color = Color.Blue;
        private string _QueryPart = "AkuminaSiteLinks.Quick Links";
        #region Properties

        /// <summary>
        ///     The color theme of the pod.
        /// </summary>
        [WebBrowsable(true),
        DisplayName("Query part"),
         Category("Akumina InterAction"),
         Personalizable(PersonalizationScope.Shared)]
        public string QueryPart
        {
          get
          {
            return _QueryPart;
          }
          set
          {
            _QueryPart = value;
          }
        }

        /// <summary>
        ///     The color theme of the pod.
        /// </summary>
        [WebBrowsable(true),
         DefaultValue(Color.Blue),
         Category("Akumina InterAction"),
         Personalizable(PersonalizationScope.Shared)]
        public Color Color { 
            get
            {
                return _Color;
            }
                
            set
            {
                _Color = value;
            }
        }

        /// <summary>
        ///     The icon of the pod.
        /// </summary>
        [WebBrowsable(true),
         DefaultValue(Icon.Linklist),
         Category("Akumina InterAction"),
         Personalizable(PersonalizationScope.Shared)]
        public Icon Icon
        {
            get
            {
                return _Icon;
            }
            set
            {
                _Icon = value;
            }
        }

        /// <summary>
        ///     The view more link.
        /// </summary>
        [WebBrowsable(true),
         DefaultValue(""),
         Category("Akumina InterAction"),
         Personalizable(PersonalizationScope.Shared)]
        public string MoreLink { get; set; }

        /// <summary>
        ///     The text for the view more link.
        /// </summary>
        [WebBrowsable(true),
         DefaultValue("More"),
         Category("Akumina InterAction"),
         Personalizable(PersonalizationScope.Shared)]
        public string MoreText { get; set; }

        /// <summary>
        ///     Whether the window opens in the same or a new browser.
        /// </summary>
        [WebBrowsable(true),
         DefaultValue(MoreWindow.Same),
         Category("Akumina InterAction"),
         Personalizable(PersonalizationScope.Shared)]
        public MoreWindow MoreWindow { 
            get {
                return _MoreWindow;
            }
            set {
                _MoreWindow = value;
            }
        }

        protected const string LinksFieldDefault = "Links";

        /// <summary>
        ///     The name of the field that the links are stored in.
        /// </summary>
        [WebBrowsable(true),
         DefaultValue(LinksFieldDefault),
         Category("Akumina InterAction"),
         Personalizable(PersonalizationScope.Shared)]
        public string LinksField { get; set; }

        #endregion

        #region IDS

        /// <summary>
        ///     Map instruction set fields to web part properties. If instruction set field is null, or is of the wrong type, the
        ///     web part property is left unchanged.
        /// </summary>
        /// <param name="response">InstructionClientResponse containing instruction set.</param>
        /// <param name="webPart">SiteLinks web part</param>
        protected void MapInstructionSetToProperties(InstructionResponse response, SiteLinks.SiteLinks webPart)
        {
            webPart.Title = response.GetValue("Title", webPart.Title);
            webPart.MoreLink = response.GetValue("MoreLink", webPart.MoreLink);
            webPart.MoreText = response.GetValue("MoreText", webPart.MoreText);
            webPart.Color = response.GetValue("Color", webPart.Color);
            webPart.Icon = response.GetValue("Icon", webPart.Icon);
            webPart.MoreWindow = response.GetValue("MoreWindow", webPart.MoreWindow);
            webPart.QueryPart = response.GetValue("QueryPart", webPart.QueryPart);
        }

        /// <summary>
        ///     Writes out the initialization script which contains the necessary css, js, and "global" (banner-wide) settings.
        /// </summary>
        /// <returns>String containing script element that defines the necessary settings and includes the necessary files.</returns>
        protected string WriteInitialScript()
        {
            var resourcePathValue = RootResourcePath;

            var sb = new StringBuilder();

            var styleSheets = Resources.pdl_StyleSheets.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var sheet in styleSheets)
            {
                sb.AppendLine("<link rel=\"stylesheet\" href=\"" + resourcePathValue + sheet + "\" />");
            }

            var jsFiles = Resources.pdl_JSFiles.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var jsFile in jsFiles)
            {
                if (jsFile.ToLower().Contains("jquery"))
                {
                    sb.AppendLine("<script type=\"text/javascript\">");
                    sb.AppendLine("if (typeof jQuery == 'undefined') { document.write(unescape(\"%3Cscript src='" +
                                  resourcePathValue + jsFile + "' type='text/javascript'%3E%3C/script%3E\")); }");
                    sb.AppendLine("</script>");
                }
                sb.AppendLine("<script type=\"text/javascript\" src=\"" + resourcePathValue + jsFile + "\"></script>");
            }

            sb.AppendLine("<script type=\"text/javascript\">");
            sb.AppendLine("var scriptbase = \"" + Resources.val_ScriptBase + "\";");
            sb.AppendLine("</script>");

            return sb.ToString();
        }

        /// <summary>
        ///     Writes out the initialization script which contains the necessary css, js, and "global" (banner-wide) settings.
        /// </summary>
        /// <returns>String containing script element that defines the necessary settings and includes the necessary files.</returns>
        protected string WriteTemplate()
        {
            var d = new Templates { ControlTemplate = Resources.ControlTemplate, ItemTemplate = Resources.ItemTemplate };

            var memStream = new MemoryStream();
            var ser = new DataContractJsonSerializer(d.GetType());
            ser.WriteObject(memStream, d);
            memStream.Position = 0;
            var sr = new StreamReader(memStream);
            var templateJson = sr.ReadToEnd();

            return templateJson;
        }

        /// <summary>
        ///     Writes out the necessary property values from the web parts properties collection.
        /// </summary>
        /// <returns>String containing script element that defines the necessary values specific to this instance of the web part.</returns>
        protected string WriteWebPartProperties(SiteLinks.SiteLinks webPart, string uniqueId)
        {
            var sb = new StringBuilder();

            sb.AppendLine("thisSiteLinks" + uniqueId + ".InstructionSetId = \"" + InstructionSet + "\";");
            sb.AppendLine("thisSiteLinks" + uniqueId + ".Color = \"" + webPart.Color + "\";");
            sb.AppendLine("thisSiteLinks" + uniqueId + ".Icon = \"" + webPart.Icon + "\";");
            sb.AppendLine("thisSiteLinks" + uniqueId + ".MoreText = \"" + webPart.MoreText + "\";");
            sb.AppendLine("thisSiteLinks" + uniqueId + ".MoreLink = \"" + webPart.MoreLink + "\";");
            sb.AppendLine("thisSiteLinks" + uniqueId + ".MoreWindow = \"" + webPart.MoreWindow + "\";");
            sb.AppendLine("thisSiteLinks" + uniqueId + ".QueryPart = \"" + webPart.QueryPart + "\";");
            sb.AppendLine("thisSiteLinks" + uniqueId + ".LinksField = \"" +
                          (!string.IsNullOrEmpty(webPart.LinksField) ? webPart.LinksField : LinksFieldDefault) + "\";");

            return sb.ToString();
        }

        protected string WriteScriptTag(string script)
        {
            return string.Format("<script type=\"text/javascript\">{0}</script>", script);
        }

        #endregion
    }
}