using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace Akumina.InterAction.SiteSummaryListWebPart
{
    class AdditionalPageHead : WebControl  
    {
        protected override void OnLoad(EventArgs e)
        {
            try
            {
                using (SPSite siteCollection = new SPSite(SPContext.Current.Web.Url))
                {
                    //Recent Log                    
                    SPWeb myWeb = siteCollection.OpenWeb();
                    SPUser currentUser = myWeb.CurrentUser;
                    var logXml = string.Format("<root><SiteName>{0}</SiteName><LastAccess>{1}</LastAccess></root>", myWeb.ID, System.DateTime.Now.ToString());
                    SPAudit audit = siteCollection.Audit;
                    audit.WriteAuditEvent(SPAuditEventType.Custom, "Aku_SSL_Log", logXml);

                }

            }
            catch
            {
            }
        }
    }
}
