using System;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace Akumina.InterAction.SiteSummaryListWebPart.SiteSummaryList
{
    public partial class SiteSummaryListUserControl : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(WriteInitialScript());            

            litTop.Text = sb.ToString();
        }

        private string WriteInitialScript()
        {
            string RootResourcePath = "";
            string resourcePathValue = RootResourcePath;

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
                    sb.AppendLine("if (typeof jQuery == 'undefined') { document.write(unescape(\"%3Cscript src='" + resourcePathValue + jsFile + "' type='text/javascript'%3E%3C/script%3E\")); }");
                    sb.AppendLine("</script>");
                }
                sb.AppendLine("<script type=\"text/javascript\" src=\"" + resourcePathValue + jsFile + "\"></script>");
            }

            var d = new Templates { ControlTemplate = Resources.ControlTemplate, ItemTemplate = Resources.ItemTemplate };

            var memStream = new MemoryStream();
            var ser = new DataContractJsonSerializer(d.GetType());
            ser.WriteObject(memStream, d);
            memStream.Position = 0;
            var sr = new StreamReader(memStream);
            var templateJson = sr.ReadToEnd();

            sb.AppendLine("<script type=\"text/javascript\">");
            sb.AppendLine("var scriptbase = \"" + Resources.val_ScriptBase + "\";");
            sb.AppendLine("</script>");

            litTemplates.Text = templateJson;

            return sb.ToString();
        }
    }
}
