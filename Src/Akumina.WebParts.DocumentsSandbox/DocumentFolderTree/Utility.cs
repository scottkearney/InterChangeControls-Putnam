using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using Microsoft.SharePoint;

namespace Akumina.WebParts.DocumentsSandbox.DocumentFolderTree
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
            FolderInfo folderinfo;
            var subFolders = folder.SubFolders;
            foreach (SPFolder subFolder in subFolders)
            {
                if (subFolder.Name.ToLower() != "forms" && subFolder.ProgID == null)
                {
                    folderinfo = new FolderInfo();
                    folderinfo.Name = subFolder.Name;
                    folderinfo.Size = GetFolderSize(subFolder)/1024;
                    folderinfo.Url = subFolder.Url;
                    result.Add(folderinfo);
                }
            }
            return result;
        }

        public static TreeNode GetFolderNode(TreeNode node, SPFolder folder, string baseUrl)
        {
            var folders = GetFoldersInFolder(folder);
            folders.Sort(new FolderInfoComparer(SortDirection.Ascending));
            TreeNode folderNode;
            for (var j = 0; j <= folders.Count - 1; j++)
            {
                folderNode = new TreeNode();
                folderNode.NavigateUrl = baseUrl + "/" + folders[j].Url;
                folderNode.ImageUrl = baseUrl + "/_layouts/images/folder.gif";
                folderNode.Text = folders[j].Name;
                folderNode.ToolTip = "Size:" + folders[j].Size + " KBs " + " Files:" + folders[j].FilesNumber;
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
            FolderDetail newFolder;
            for (var j = 0; j <= folders.Count - 1; j++)
            {
                newFolder = new FolderDetail();
                newFolder.FolderName = folders[j].Name;
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
            StringBuilder element, newElement;

            for (var j = 0; j <= folders.Count - 1; j++)
            {
                element = new StringBuilder();
                newElement = new StringBuilder();
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