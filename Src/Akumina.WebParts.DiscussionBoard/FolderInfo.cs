using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace Akumina.WebParts.DiscussionBoard
{
    public class FolderInfo
    {
        public string Name { get; set; }
        public long Size { get; set; }
        public string Url { get; set; }
        public long FilesNumber { get; set; }
        public string Id { get; set; }
    }

    public class FolderInfoComparer : IComparer<FolderInfo>
    {
        private readonly SortDirection _mDirection = SortDirection.Ascending;

        public FolderInfoComparer()
        {
        }

        public FolderInfoComparer(SortDirection direction)
        {
            _mDirection = direction;
        }

        int IComparer<FolderInfo>.Compare(FolderInfo x, FolderInfo y)
        {
            if (x == null && y == null)
            {
                return 0;
            }
            if (x == null && y != null)
            {
                return (_mDirection == SortDirection.Ascending) ? -1 : 1;
            }
            if (x != null && y == null)
            {
                return (_mDirection == SortDirection.Ascending) ? 1 : -1;
            }
            return
                (_mDirection == SortDirection.Ascending)
                    ? x.Name.CompareTo(y.Name)
                    : y.Name.CompareTo(x.Name);
        }
    }

    public class FolderDetail
    {
        private string _folderName = string.Empty;
        private SubFolderDetail _subfolders = new SubFolderDetail();

        public FolderDetail()
        {
        }

        public FolderDetail(string text)
        {
            _folderName = text;
        }

        public string FolderName { get; set; }

        public SubFolderDetail SubFolders
        {
            get { return _subfolders; }
            set { _subfolders = value; }
        }
    }

    public class SubFolderDetail
    {
        private readonly FolderDetail _parentFolder;
        private List<FolderDetail> _subfolderName = new List<FolderDetail>();

        public SubFolderDetail()
        {
        }

        public SubFolderDetail(FolderDetail owner)
        {
            _parentFolder = owner;
        }

        public List<FolderDetail> GetSubFolders
        {
            get { return _subfolderName; }
            set { _subfolderName = value; }
        }

        public void Add(FolderDetail child)
        {
            if (child.SubFolders != null)
                _subfolderName.Add(child);
            else
                _subfolderName.Add(new FolderDetail(_parentFolder.FolderName));
        }
    }

}