using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using Microsoft.SharePoint;

namespace Akumina.WebParts.Documents.DocumentFolderTree
{
    internal class Utility
    {
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
                        //Size = GetFolderSize(subFolder)/1024,
                        Url = subFolder.Url
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
                var newFolder = new FolderDetail {FolderName = folders[j].Name};
                var subfolder = folder.SubFolders[folders[j].Url];
                newFolder.SubFolders.Add(GetFolders(newFolder, subfolder, baseUrl));
                fStrt.SubFolders.Add(newFolder);
            }

            return fStrt;
        }

        public static StringBuilder FrameFolderTree(StringBuilder xWriter, SPFolder folder, string baseUrl)
        {
            var folders = GetFoldersInFolder(folder);
            folders.Sort(new FolderInfoComparer(SortDirection.Ascending));

            for (var j = 0; j <= folders.Count - 1; j++)
            {
                var element = new StringBuilder();
                var newElement = new StringBuilder();
                element.AppendLine("<ul>");
                var subfolder = folder.SubFolders[folders[j].Url];
                if (GetFoldersInFolder(subfolder).Count > 0)
                    element.AppendLine(String.Format("<li title='{0}'>{0}|{1}{2}</li>", folders[j].Name, folders[j].Url,
                        FrameFolderTree(newElement, subfolder, baseUrl)));
                else
                    element.AppendLine(String.Format("<li title='{0}'>{0}|{1}</li>", folders[j].Name, folders[j].Url));
                element.AppendLine("</ul>");
                xWriter.AppendLine(String.Format("{0}", element));
            }
            return xWriter;
        }
    }
}