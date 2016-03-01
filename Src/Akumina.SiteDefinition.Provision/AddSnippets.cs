using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI.WebControls;

namespace Akumina.SiteDefinition.Provision
{
    public class AddSnippets : WebControl
    {
        private const string ListName = "CustomSnippet";

        public override void RenderControl(System.Web.UI.HtmlTextWriter writer)
        {
            var stringBuilder = new StringBuilder();
            try
            {
                var currentWeb = SPContext.Current.Web;
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    BuildCSSLink(currentWeb, ListName, stringBuilder);
                });

                writer.Write(stringBuilder.ToString());
            }
            catch (Exception ex)
            {

            }

            base.RenderControl(writer);
        }

        private void BuildCSSLink(SPWeb web, string listUrl, StringBuilder stringBuilder)
        {
            string[] cssLinks, jsLinks;
            var cssFiles = new List<string>();
            var jsFiles = new List<string>();

            var cWeb = SPContext.Current.Web;
            SPList oList = null;
            oList = cWeb.Lists.TryGetList(listUrl);
            if (oList != null && oList.BaseType == SPBaseType.DocumentLibrary)
            {
                var docLib = (SPDocumentLibrary)oList;
                var oQuery = new SPQuery();
                oQuery.Query = "<OrderBy><FieldRef Name='Order0' Ascending='True'  /></OrderBy>";
                oQuery.ViewAttributes = "Scope=\"Recursive\"";
                var collListItemsAvailable =
                    docLib.GetItems(oQuery);

                foreach (SPListItem oListItemAvailable in collListItemsAvailable)
                {
                    if (oListItemAvailable["File_x0020_Type"].ToString() == "css")
                        cssFiles.Add(oListItemAvailable.File.ServerRelativeUrl);

                    if (oListItemAvailable["File_x0020_Type"].ToString() == "js")
                        jsFiles.Add(oListItemAvailable.File.ServerRelativeUrl);
                }
            }

            for (int i = 0; i < jsFiles.Count; i++)
            {
                stringBuilder.AppendFormat(BindScript(jsFiles[i].ToLower(), true));
            }

            //bind style operation
            foreach (var cssfile in cssFiles)
                stringBuilder.AppendFormat(BindStyle(cssfile.ToLower(), true));

        }

        private string BindScript(string scriptUrl, bool pickFromSiteCollection)
        {
            scriptUrl = SPUrlUtility.CombineUrl(SPContext.Current.Web.Url, scriptUrl);

            return string.Format(@"<script type=""text/javascript"" src=""{0}""></script>", scriptUrl);
        }

        private string BindStyle(string styleUrl, bool pickFromSiteCollection)
        {
            styleUrl = SPUrlUtility.CombineUrl(SPContext.Current.Web.Url, styleUrl);

            return string.Format(@"<link rel=""stylesheet"" href=""{0}"" type=""text/css"" />", styleUrl);
        }

    }
}
