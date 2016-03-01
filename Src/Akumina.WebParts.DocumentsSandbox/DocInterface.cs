using System;
using System.Collections.Generic;
using System.Linq;

namespace Akumina.WebParts.DocumentsSandbox
{
    [Serializable]
    public class Filter
    {
        public string Key { get; set; }
        public List<string> Options { get; set; }
    }

    public interface IGetCurrentPath
    {
        string CurrentPath { get; set; }
    }

    public interface IGetQuery
    {
        List<Filter> Query { get; set; }
    }

    public interface IGetCurrentTab
    {
        string CurrentTab { get; set; }
    }

    public class GetFileType
    {
        private static Dictionary<string, string> GetAllFileTypes()
        {
            string wordFiles = "docx','doc','docm','dot','nws','dotx",
                pdf = "pdf",
                powerPoint = "odp','ppt','pptm','pptx','potm','ppam','ppsm','ppsx",
                excel = "odc','xls','xlsb','xlsm','xlsx','xltm','xltx','xlam";
                //images = "gif','tiff','jpg','jpeg','bmp','png','rif";
            var wordDictionary = new Dictionary<string, string>();
            wordDictionary.Add("Word", wordFiles);
            wordDictionary.Add("PDF", pdf);
            wordDictionary.Add("PPT", powerPoint);
            wordDictionary.Add("Excel", excel);
            //wordDictionary.Add("Images", images);
            return wordDictionary;
        }

        public static string GetKey(string fileValue)
        {
            var fileKey = string.Empty;
            var matches = Files.Where(pair => pair.Value.Contains(fileValue))
                .Select(pair => pair.Key);
            if (matches.Count() > 0)
            {
                fileKey = matches.FirstOrDefault();
            }
            ;
            return fileKey;
        }

        public static string GetValue(string filekey)
        {
            var value = string.Empty;
            if (Files.TryGetValue(filekey, out value))
                return value;
            return value;
        }

        public static Dictionary<string, string> Files = GetAllFileTypes();
    }
}