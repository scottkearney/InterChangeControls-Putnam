using System.ComponentModel;
using System.Web.UI.WebControls.WebParts;

namespace Akumina.WebParts.Documents.DocumentTab
{
    public class DocumentTabBaseWebPart : DocumentBaseWebPart
    {
        public string _NoOfRecentFiles;
        [Category("Akumina InterAction"), WebDisplayName("Tab.Number of Recent Files"), WebBrowsable(true), Personalizable(PersonalizationScope.Shared)]
        public string NoOfRecentFiles
        {
            get
            {
                if (string.IsNullOrEmpty(_NoOfRecentFiles))
                    _NoOfRecentFiles = "5";
                return _NoOfRecentFiles;
            }
            set
            {
                _NoOfRecentFiles = value;
            }
        }

        public string _NoOfPopularFiles;
        [Category("Akumina InterAction"), WebDisplayName("Tab.Number of Popular Files"), WebBrowsable(true), Personalizable(PersonalizationScope.Shared)]
        public string NoOfPopularFiles
        {
            get
            {
                if (string.IsNullOrEmpty(_NoOfPopularFiles))
                    _NoOfPopularFiles = "5";
                return _NoOfPopularFiles;
            }
            set
            {
                _NoOfPopularFiles = value;
            }
        }

        //public string TabNumOfRecentFiles { get; set; }

        //public string NumOfDays { get; set; }

        //public string NoOfRecentFiles { get; set; }

        //public string NumOfPopularFiles { get; set; }

        //public string NumOfFiles { get; set; }

    }
}