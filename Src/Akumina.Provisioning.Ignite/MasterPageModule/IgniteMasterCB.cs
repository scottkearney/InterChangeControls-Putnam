using System;
using System.Web.UI;
using Microsoft.SharePoint;

namespace Akumina.Provisioning.Ignite.MasterPageModule
{
    public class IgniteMasterCB : MasterPage
    {
        public System.Web.UI.HtmlControls.HtmlAnchor homeLink;

        public void Page_Load(object sender, EventArgs e)
        {
            string propVal = string.Empty;
            if (SPContext.Current.Web.Properties.ContainsKey("AkuminaPageSetting"))
            {
                propVal = SPContext.Current.Web.Properties["AkuminaPageSetting"];
            }

            if (string.IsNullOrEmpty(propVal))
            {
                propVal = "Pages";
            }

            homeLink.HRef = SPContext.Current.Web.Url + "/" + propVal + "/Home.aspx";
        }
    }
}
