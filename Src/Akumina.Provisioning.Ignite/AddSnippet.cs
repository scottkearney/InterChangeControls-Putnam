using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using System.Web.UI.WebControls;

namespace Akumina.Provisioning.Ignite
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
                    BuildCssLink(currentWeb, ListName, stringBuilder);
                });

                writer.Write(stringBuilder.ToString());
            }
            catch (Exception ex)
            {
                // ignored
            }

            base.RenderControl(writer);
        }

        private void BuildCssLink(SPWeb web, string listUrl, StringBuilder stringBuilder)
        {
            var cssFiles = new List<string>();
            var jsFiles = new List<string>();

            var cWeb = SPContext.Current.Web;
            SPList oList = null;
            oList = cWeb.Lists.TryGetList(listUrl);
            if (oList != null && oList.BaseType == SPBaseType.DocumentLibrary)
            {
                var docLib = (SPDocumentLibrary)oList;
                var oQuery = new SPQuery
                {
                    Query = "<OrderBy><FieldRef Name='Order0' Ascending='True'  /></OrderBy>",
                    ViewAttributes = "Scope=\"Recursive\""
                };
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
