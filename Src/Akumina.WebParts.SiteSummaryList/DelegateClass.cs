using System;
using System.Web.UI.WebControls;
using Microsoft.SharePoint;

namespace Akumina.WebParts.SiteSummaryList.SiteSummaryList
{
    class DelegateClass : WebControl
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
                    var logXml = string.Format("<root><WebSiteID>{0}</WebSiteID><LastAccess>{1}</LastAccess></root>", myWeb.ID, DateTime.Now.ToString());
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
