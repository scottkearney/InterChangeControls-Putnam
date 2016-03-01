using System;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using Microsoft.SharePoint;


namespace Akumina.SiteDefinition.Provision.MasterPageModule
{
    public class AkuminaSparkMasterCB : MasterPage
    {
        public System.Web.UI.HtmlControls.HtmlAnchor homeLink;
        public System.Web.UI.HtmlControls.HtmlAnchor documentLink;
        public System.Web.UI.HtmlControls.HtmlAnchor discussionLink;
        public void Page_Load(object sender, EventArgs e)
        {
            string propVal = string.Empty;
            if (SPContext.Current.Web.Properties.ContainsKey("AkuminaPageSetting"))
                propVal = SPContext.Current.Web.Properties["AkuminaPageSetting"];

            if (string.IsNullOrEmpty(propVal))
                propVal = "Pages";

            homeLink.HRef = SPContext.Current.Web.Url + "/" + propVal + "/SparkHome.aspx";
            documentLink.HRef = SPContext.Current.Web.Url + "/" + propVal + "/SparkLibraryListing.aspx";
            discussionLink.HRef = SPContext.Current.Web.Url + "/" + propVal + "/SparkDiscussions.aspx";


        }
    }
}
