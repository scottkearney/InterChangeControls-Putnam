using System;
using System.ComponentModel;
using Microsoft.SharePoint;

namespace Akumina.WebParts.SiteLinks.SiteLinks
{
    [ToolboxItem(false)]
    public partial class SiteLinks : SiteLinksBaseWebPart
    {
        private string _uniqueId = "";

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(RootResourcePath))
            {
                RootResourcePath = SPContext.Current.Site.Url + "/Akumina.WebParts.SiteLinks";
            }
            if (!Page.IsPostBack)
            {
                if (!SPContext.Current.IsDesignTime && !string.IsNullOrWhiteSpace(InstructionSet))
                {
                    try
                    {
                        
                        MapInstructionSetToProperties(GetInstructionSet(InstructionSet), this);
                        if (string.IsNullOrEmpty(RootResourcePath))
                        {
                            RootResourcePath = SPContext.Current.Site.Url + "/Akumina.WebParts.SiteLinks";
                        }
                    }
                    catch (Exception)
                    {
                        // ignored
                    }
                }

                // Initial script
                litTop.Text = WriteInitialScript();

                _uniqueId = Guid.NewGuid().ToString().Replace("-", "").ToUpper();
                litDivID.Text = _uniqueId;
                litUniqueId.Text = _uniqueId;
                uxUniqueId1.Text = _uniqueId;
                uxUniqueId2.Text = _uniqueId;
                uxUniqueId3.Text = _uniqueId;
                uxUniqueId4.Text = _uniqueId;
                uxUniqueId5.Text = _uniqueId;
                uxUniqueId6.Text = _uniqueId;
                uxUniqueId7.Text = _uniqueId;
                litTemplates.Text = WriteTemplate();

                litProperties.Text = WriteWebPartProperties(this, _uniqueId);
            }
        }
    }
}