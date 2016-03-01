using System;
using System.Collections.Generic;
using System.Linq;

namespace Akumina.WebParts.Documents
{
    [Serializable]
    public class Filter
    {
        public string ClassName { get; set; }
        public string Key { get; set; }
        public List<string> Options { get; set; }
    }

    public interface ICurrentPath
    {
        string CurrentPath { get; set; }
    }

    public interface ICurrentTab
    {
        string CurrentTab { get; set; }
    }

    public class GetFileType
    {
        public static Dictionary<string, string> Files = GetAllFileTypes();

        private static Dictionary<string, string> GetAllFileTypes()
        {
            string wordFiles = "docx','doc','docm','dot','nws','dotx",
                pdf = "pdf",
                powerPoint = "odp','ppt','pptm','pptx','potm','ppam','ppsm','ppsx",
                excel = "odc','xls','xlsb','xlsm','xlsx','xltm','xltx','xlam";
            var wordDictionary = new Dictionary<string, string>
            {
                {"Word", wordFiles},
                {"PDF", pdf},
                {"PPT", powerPoint},
                {"Excel", excel}
            };
            return wordDictionary;
        }

        public static string GetKey(string fileValue)
        {
            var fileKey = string.Empty;
            var matches = Files.Where(pair => pair.Value.Contains(fileValue))
                 .Select(pair => pair.Key);
            if (matches.Any())
            {
                fileKey = matches.FirstOrDefault();
            }
            ;
            return fileKey;
        }

        public static string GetValue(string filekey)
        {
            string value;
            return Files.TryGetValue(filekey, out value) ? value : string.Empty;
        }
    }
}