using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using Microsoft.SharePoint;

namespace Akumina.WebParts.DiscussionBoard
{
    internal class Utility
    {
        public static string imgPath = "/_layouts/15/images/";

        public static string webUrl = string.Empty;
        public Utility()
        {
            var wb = SPContext.Current.Web;
            webUrl = wb.Url;
        }
        public static long GetFolderSize(SPFolder folder)
        {
            long folderSize = 0;
            foreach (SPFile file in folder.Files)
            {
                folderSize += file.Length;
            }
            foreach (SPFolder subfolder in folder.SubFolders)
            {
                folderSize += GetFolderSize(subfolder);
            }
            return folderSize;
        }

        public static List<FolderInfo> GetFoldersInFolder(SPFolder folder)
        {
            var result = new List<FolderInfo>();
            var subFolders = folder.SubFolders;
            foreach (SPFolder subFolder in subFolders)
            {

                if (subFolder.Name.ToLower() != "forms" && subFolder.ProgID == null)
                {
                    var folderinfo = new FolderInfo
                    {
                        Name = subFolder.Name,
                        Size = GetFolderSize(subFolder) / 1024,
                        Url = subFolder.ServerRelativeUrl,
                        Id = subFolder.Item.ID.ToString()
                    };
                    result.Add(folderinfo);
                }
            }
            return result;
        }

        public static TreeNode GetFolderNode(TreeNode node, SPFolder folder, string baseUrl)
        {
            var folders = GetFoldersInFolder(folder);
            folders.Sort(new FolderInfoComparer(SortDirection.Ascending));
            for (var j = 0; j <= folders.Count - 1; j++)
            {
                var folderNode = new TreeNode
                {
                    NavigateUrl = baseUrl + "/" + folders[j].Url,
                    ImageUrl = baseUrl + "/_layouts/images/folder.gif",
                    Text = folders[j].Name,
                    ToolTip = "Size:" + folders[j].Size + " KBs " + " Files:" + folders[j].FilesNumber
                };
                var subfolder = folder.SubFolders[folders[j].Url];
                folderNode.ChildNodes.Add(GetFolderNode(folderNode, subfolder, baseUrl));
                node.ChildNodes.Add(folderNode);
            }
            return node;
        }

        public static FolderDetail GetFolders(FolderDetail fStrt, SPFolder folder, string baseUrl)
        {
            var folders = GetFoldersInFolder(folder);
            folders.Sort(new FolderInfoComparer(SortDirection.Ascending));
            for (var j = 0; j <= folders.Count - 1; j++)
            {
                var newFolder = new FolderDetail { FolderName = folders[j].Name };
                var subfolder = folder.SubFolders[folders[j].Url];
                newFolder.SubFolders.Add(GetFolders(newFolder, subfolder, baseUrl));
                fStrt.SubFolders.Add(newFolder);
            }

            return fStrt;
        }

        public static StringBuilder FrameFolderTree(StringBuilder xWriter, SPFolder folder, string DocumentLibName)
        {

            string Files = string.Empty;
            var folders = GetFoldersInFolder(folder);
            folders.Sort(new FolderInfoComparer(SortDirection.Ascending));
            var element = new StringBuilder();
            element.AppendLine("<ul>");
            for (var j = 0; j <= folders.Count - 1; j++)
            {

                var newElement = new StringBuilder();

                var subfolder = folder.SubFolders[folders[j].Url];
                if (GetFoldersInFolder(subfolder).Count > 0)
                    element.AppendLine(String.Format("<li title='{0}' item-id='{3}' list-name='{4}' url-data='{1}'>{0}{2}</li>", folders[j].Name, folders[j].Url,
                        FrameFolderTree(newElement, subfolder, DocumentLibName), folders[j].Id, DocumentLibName));
                else
                {


                    if (subfolder.Files.Count > 0)
                    {
                        element.AppendLine(String.Format("<li title='{0}' item-id='{2}' list-name='{3}' url-data='{1}'>{0}", folders[j].Name, folders[j].Url, folders[j].Id, DocumentLibName));

                        element.Append("<ul>");
                        subfolder.Files.Cast<SPFile>().ToList().ForEach(x => element.Append("<li data-jstree='{\"icon\":\"ia-hide-folder-icon\"}'" + String.Format(" title=\"{0}\" item-id=\"{3}\" list-name=\"{4}\" url-data=\"{1}\"><img src=\"{2}\" class=\"ia-folder-tree-icon\"/> <span title=\"{0}\">{0}</span></li>", Path.GetFileNameWithoutExtension(x.Name), x.ServerRelativeUrl, webUrl + imgPath + x.IconUrl, x.Item.ID, DocumentLibName)));
                        element.Append("</li></ul>");

                    }
                    else
                    {
                        element.AppendLine(String.Format("<li title='{0}' item-id='{2}' list-name='{3}' url-data='{1}'>{0}</li>", folders[j].Name, folders[j].Url, folders[j].Id, DocumentLibName));


                    }

                }

            }
            if (folder.Files.Count > 0)
                folder.Files.Cast<SPFile>().ToList().ForEach(x => element.Append("<li data-jstree='{\"icon\":\"ia-hide-folder-icon\"}'" + String.Format(" title=\"{0}\" item-id=\"{3}\" list-name=\"{4}\" url-data=\"{1}\"><img src=\"{2}\" class=\"ia-folder-tree-icon\"/> <span title=\"{0}\">{0}</span></li>", Path.GetFileNameWithoutExtension(x.Name), x.ServerRelativeUrl, webUrl + imgPath + x.IconUrl, x.Item.ID, DocumentLibName)));

            element.AppendLine("</ul>");
            xWriter.AppendLine(String.Format("{0}", element));


            return xWriter;
        }

        public static string GetFullTreeView(string DocumentLibName)
        {
            var wb = SPContext.Current.Web;
            var folderTreeInfo = new StringBuilder();
            var doclib = (SPDocumentLibrary)wb.Lists[DocumentLibName];
            var root = doclib.RootFolder;
            var sb = new StringBuilder();
            var liSb = "";
            liSb = String.Format("<li title='{0}' url-data='{1}' item-id='{3}' list-name='{0}'>{0}{2}</li>", doclib.Title, root.ServerRelativeUrl,
                FrameFolderTree(sb, root, DocumentLibName),"0");
            //if (root.Files.Count > 0)
            //    root.Files.Cast<SPFile>().ToList().ForEach(x => liSb += ("<li data-jstree='{\"icon\":\"ia-hide-folder-icon\"}'" + String.Format(" title=\"{0}\" item-id=\"{3}\" list-name=\"{4}\" url-data=\"{1}\"><img src=\"{2}\" class=\"ia-folder-tree-icon\"/> <span title=\"{0}\">{0}</span></li>", Path.GetFileNameWithoutExtension(x.Name), x.ServerRelativeUrl, webUrl + imgPath + x.IconUrl, x.Item.ID, DocumentLibName)));
            folderTreeInfo.AppendLine(liSb);
            var finalRes = String.Format("<ul>{0}</ul>", folderTreeInfo);
            return finalRes;
        }
    }

}