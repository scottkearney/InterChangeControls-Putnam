using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Dynamic;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Web.UI;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;

namespace Akumina.InterAction
{
    /// <summary>
    ///     Base web part to include default properties
    /// </summary>
    public class AkuminaBaseWebPart : WebPart
    {
        /// <summary>
        ///     String denoting custom list and item title of instruction set. ({listname}.{item title} without curly braces)
        /// </summary>
        [Category("Akumina InterAction"), WebDisplayName("Instruction Set"), WebBrowsable(true),
         Personalizable(PersonalizationScope.Shared)]
        public string InstructionSet { get; set; }

        /// <summary>
        ///     String (starting with slash) leading to the directory where css and js files are loaded from.
        /// </summary>
        [Category("Akumina InterAction"), WebDisplayName("Scripts Path"), WebBrowsable(true),
         Personalizable(PersonalizationScope.Shared)]
        public string RootResourcePath { get; set; }

        public static T ParseEnum<T>(string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }
        protected void WriteJs(HtmlTextWriter writer, string file)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Type, @"text/javascript");
            writer.AddAttribute(HtmlTextWriterAttribute.Src, string.Format("{0}/{1}", RootResourcePath, file));
            writer.RenderBeginTag(HtmlTextWriterTag.Script);
            writer.RenderEndTag(); // script
        }

        protected void WriteCss(HtmlTextWriter writer, string file)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Rel, "stylesheet");
            writer.AddAttribute(HtmlTextWriterAttribute.Type, "text/style");
            writer.AddAttribute(HtmlTextWriterAttribute.Href, string.Format("{0}/{1}", RootResourcePath, file));
            writer.RenderBeginTag(HtmlTextWriterTag.Link);
            writer.RenderEndTag(); // script
        }

        protected InstructionResponse GetInstructionSet(string instructionSetName)
        {
            if (string.IsNullOrWhiteSpace(instructionSetName))
                return new InstructionResponse { Dictionary = new Dictionary<string, Dictionary<string, object>>() };
            IInstructionRepository repository = new InstructionRepository();
            var response = repository.Execute(instructionSetName);
            return response ??
                   new InstructionResponse { Dictionary = new Dictionary<string, Dictionary<string, object>>() };
        }

        public static string Serialize<T>(T t) where T : new()
        {
            var stream = new MemoryStream();
            var ser = new DataContractJsonSerializer(typeof(T));
            ser.WriteObject(stream, t);
            stream.Position = 0;
            var sr = new StreamReader(stream);
            return sr.ReadToEnd();
        }

        public static string GetUniqueId()
        {
            return Guid.NewGuid().ToString().Replace("-", "").ToUpper();
        }

        public static Dictionary<string, object> GetItem(string listName, string itemTitle, bool isPreview = false, bool useInternalName = false)
        {
            return GetItem(false, listName, itemTitle, isPreview, useInternalName);
        }


        public static Dictionary<string, object> GetPreview(bool isPreview, SPListItem items)
        {
            var response = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
            SPUser user = SPContext.Current.Web.CurrentUser;
            foreach (SPListItemVersion versionItem in items.Versions)
            {
                if ((!Convert.ToBoolean(versionItem.Level.ToString() == "Draft") && versionItem.IsCurrentVersion) || (isPreview ? Convert.ToBoolean(versionItem.Level.ToString() == "Draft" && user.LoginName == versionItem.CreatedBy.LoginName) : !Convert.ToBoolean(versionItem.Level.ToString() == "Draft")))
                {
                    try
                    {
                        foreach (SPField field in versionItem.Fields)
                        {
                            if (!response.ContainsKey(field.InternalName))
                            {
                                response.Add(field.InternalName, versionItem[field.InternalName] != null ? versionItem[field.InternalName].ToString() : "");
                            }
                        }
                    }
                    catch (Exception)
                    {
                        // ignored
                    }
                }
            }
            return response;
        }

        public static Dictionary<string, object> GetItem(bool rootWeb, string listName, string itemTitle, bool isPreview = false, bool useInternalName = false)
        {
            var fields = new Dictionary<string, string>();
            try
            {
                if (string.IsNullOrEmpty(listName) || string.IsNullOrWhiteSpace(itemTitle))
                {
                    return null;
                }
                Guid guid;
                SPList list;
                if (Guid.TryParse(listName, out guid))
                {
                    try
                    {
                        list = rootWeb ? SPContext.Current.Site.RootWeb.Lists.GetList(guid, false) : SPContext.Current.Web.Lists.GetList(guid, false);
                    }
                    catch
                    {
                        list = null;
                    }
                }
                else
                {
                    list = rootWeb ? SPContext.Current.Site.RootWeb.Lists.TryGetList(listName) : SPContext.Current.Web.Lists.TryGetList(listName);
                }

                if (list != null)
                {
                    string queryWhere;
                    int itemId;
                    if (int.TryParse(itemTitle, out itemId))
                    {
                        queryWhere =
                            string.Format(
                                @"<Where><Eq><FieldRef Name='ID' /><Value Type='Text'>{0}</Value></Eq></Where>", itemId);
                    }
                    else
                    {
                        queryWhere =
                            string.Format(
                                @"<Where><Eq><FieldRef Name='Title' /><Value Type='Text'>{0}</Value></Eq></Where>",
                                itemTitle);
                    }
                    var query = new SPQuery { Query = queryWhere, RowLimit = 1 };

                    var items = list.GetItems(query);

                    var response = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
                    if (items.Count > 0 && items.List.EnableVersioning)
                    {
                        return GetPreview(isPreview, items[0]);
                    }
                    var dt = items.GetDataTable();
                    if (dt == null) return null;
                    foreach (SPField field in items.Fields)
                    {
                        fields.Add(field.InternalName, useInternalName ? field.InternalName : field.Title);
                    }
                    foreach (DataRow row in dt.Rows)
                    {
                        foreach (DataColumn col in dt.Columns)
                        {
                            if (!response.ContainsKey(col.ColumnName))
                            {
                                try
                                {
                                    response.Add(fields[col.ColumnName], row[col.ColumnName]);
                                }
                                catch (Exception)
                                {
                                    // ignored
                                }
                            }
                            //HttpContext.Current.Response.Write(string.Format("Column Name: {0} \t Value:{1}", col.ColumnName, row[col.ColumnName]));
                        }
                        break; //Process only one row
                    }
                    return response;
                }
            }
            catch (Exception ex)
            {
                Logging.LogError(ex);
            }
            return null;
        }

        protected void SetTitle(WebPart webPart, string title)
        {
            if (webPart != null)
                webPart.Title = title;
        }
    }


    /// <summary>
    ///     Instruction Response
    /// </summary>
    public class InstructionResponse
    {
        //public dynamic Dynamic { get; set; }
        public Dictionary<string, Dictionary<string, object>> Dictionary { get; set; }

        public T GetValue<T>(string columnName,
            T defaultValue = default(T))
        {
            try
            {
                var arr = columnName.Split(new[] { '.' }, 2);
                var firstKey = (arr.Length == 1) ? InstructionConstants.DefaultCategory : arr[0];
                var secondKey = (arr.Length == 1) ? arr[0] : arr[1];
                if (!Dictionary.ContainsKey(firstKey) ||
                    !Dictionary[firstKey].ContainsKey(secondKey)) return defaultValue;
                var columnValue = ((IDictionary<string, object>)Dictionary[firstKey])[secondKey];
                if (defaultValue.GetType().ToString() == "System.Boolean")
                {
                    var ret = columnValue != null && !string.IsNullOrWhiteSpace((string)columnValue) &&
                              columnValue.ToString().ToLower() == "1";
                    columnValue = ret;
                }
                return (columnValue != null) ? (T)columnValue : defaultValue;
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }
    }

    /// <summary>
    ///     InstructionConstants
    /// </summary>
    public class InstructionConstants
    {
        /// <summary>
        ///     DefaultCategory hardcoded key
        /// </summary>
        public const string DefaultCategory = "AkuminaInterActionDefault";
    }

    /// <summary>
    ///     Instruction Repository interface
    /// </summary>
    public interface IInstructionRepository
    {
        InstructionResponse Execute(string instructionsetName);

        T GetValue<T>(Dictionary<string, Dictionary<string, object>> instructionDictionary, string columnName,
            T defaultValue = default(T));
    }

    /// <summary>
    ///     InstructionRepository
    /// </summary>
    public class InstructionRepository : IInstructionRepository
    {
        /// <summary>
        ///     Returns the expando, dictionary or datatable of given list.title
        /// </summary>
        /// <param name="instructionsetName">Instruction set name should follow LISTNAME.TITLE</param>
        /// <returns>Returns the expando, dictionary or datatable</returns>
        public InstructionResponse Execute(string instructionsetName)
        {
            var fields = new Dictionary<string, string>();
            try
            {
                if (string.IsNullOrEmpty(instructionsetName))
                {
                    return ProcessListData(null, fields);
                }
                if (!instructionsetName.Contains("."))
                {
                    return ProcessListData(null, fields);
                }
                var source = instructionsetName.Split(new[] { '.' }, 3);
                var listName = source[0];
                var itemTitle = source[1];
                bool rootWeb = false;
                if (source.Length == 3 && source[0].Equals("~"))
                {
                    rootWeb = true;
                    listName = source[1];
                    itemTitle = source[2];
                }
                if (string.IsNullOrEmpty(listName) || string.IsNullOrEmpty(itemTitle))
                {
                    return ProcessListData(null, fields);
                    ;
                }
                Guid guid;
                SPList list;
                if (Guid.TryParse(listName, out guid))
                {
                    try
                    {
                        list = rootWeb ? SPContext.Current.Site.RootWeb.Lists.GetList(guid, false) : SPContext.Current.Web.Lists.GetList(guid, false);
                    }
                    catch
                    {
                        list = null;
                    }
                }
                else
                {
                    list = rootWeb ? SPContext.Current.Site.RootWeb.Lists.TryGetList(listName) : SPContext.Current.Web.Lists.TryGetList(listName);
                }

                if (list != null)
                {
                    int itemId;
                    var queryWhere = "";
                    if (int.TryParse(itemTitle, out itemId))
                    {
                        queryWhere =
                            string.Format(
                                @"<Where><Eq><FieldRef Name='ID' /><Value Type='Text'>{0}</Value></Eq></Where>", itemId);
                    }
                    else
                    {
                        queryWhere =
                            string.Format(
                                @"<Where><Eq><FieldRef Name='Title' /><Value Type='Text'>{0}</Value></Eq></Where>",
                                itemTitle);
                    }

                    var query = new SPQuery { Query = queryWhere, RowLimit = 1 };

                    var items = list.GetItems(query);

                    var dt = items.GetDataTable();
                    if (dt == null) return null;
                    foreach (SPField field in items.Fields)
                    {
                        fields.Add(field.InternalName, field.Title);
                    }
                    return ProcessListData(dt, fields);
                }
            }
            catch (Exception ex)
            {
                Logging.LogError(ex);
            }
            return ProcessListData(null, fields);
        }

        /// <summary>
        ///     return the value of given column
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instructionDictionary"></param>
        /// <param name="columnName"></param>
        /// <param name="defaultValue"></param>
        /// <returns>T</returns>
        public T GetValue<T>(Dictionary<string, Dictionary<string, object>> instructionDictionary, string columnName,
            T defaultValue = default(T))
        {
            try
            {
                var arr = columnName.Split(new[] { '.' }, 2);
                var firstKey = (arr.Length == 1) ? InstructionConstants.DefaultCategory : arr[0];
                var secondKey = (arr.Length == 1) ? arr[0] : arr[1];
                if (!instructionDictionary.ContainsKey(firstKey) ||
                    !instructionDictionary[firstKey].ContainsKey(secondKey)) return defaultValue;
                var columnValue = ((IDictionary<string, object>)instructionDictionary[firstKey])[secondKey];
                return (columnValue != null) ? (T)columnValue : defaultValue;
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        /// <summary>
        ///     process the list row to instruction response
        /// </summary>
        /// <param name="dt">DataTable mapping of original row</param>
        /// <param name="fields">List of internalname-->displayname</param>
        /// <returns>instruction response</returns>
        private static InstructionResponse ProcessListData(DataTable dt, Dictionary<string, string> fields)
        {
            var categories = new Dictionary<string, Dictionary<string, object>>(StringComparer.OrdinalIgnoreCase);
            if (dt != null)
            {
                foreach (DataRow row in dt.Rows)
                {
                    foreach (DataColumn col in dt.Columns)
                    {
                        CreateDictionary(categories, fields[col.ColumnName], row[col.ColumnName]);
                        //HttpContext.Current.Response.Write(string.Format("Column Name: {0} \t Value:{1}", col.ColumnName, row[col.ColumnName]));
                    }
                    break; //Process only one row
                }
            }
            //if (AppDomain.CurrentDomain.IsHomogenous)
            //{
            //    dynamic d = CreateExpando(categories[DefaultCategory]);
            //    IDictionary<string, object> map = d;
            //    foreach (var category in categories.Where(category => category.Key != DefaultCategory))
            //    {
            //        map.Add(category.Key, CreateExpando(category.Value));
            //    }
            //    return new InstructionClientResponse {Dynamic = d, Dictionary = categories};
            //}
            return new InstructionResponse { Dictionary = categories };
        }

        /// <summary>
        ///     Creates the Expando object for given dictionary
        /// </summary>
        /// <param name="items">Dictionary of string,object</param>
        /// <returns>dynamic object</returns>
        private static dynamic CreateExpando(Dictionary<string, object> items)
        {
            dynamic d = new ExpandoObject();
            IDictionary<string, object> map = d;
            foreach (var item in items)
            {
                map.Add(item.Key, item.Value);
            }
            return d;
        }

        /// <summary>
        ///     converts the list field to dictionary object
        /// </summary>
        /// <param name="map">dictionary reference parameter</param>
        /// <param name="columnName">sharepoint column name</param>
        /// <param name="columnValue">sharepoint column value</param>
        private static void CreateDictionary(Dictionary<string, Dictionary<string, object>> map, string columnName,
            object columnValue)
        {
            var arr = columnName.Split(new[] { '.' }, 2);
            var cat = (arr.Length == 1) ? InstructionConstants.DefaultCategory : arr[0];
            var field = (arr.Length == 1) ? arr[0] : arr[1];
            if (map.ContainsKey(cat))
            {
                if (!map[cat].ContainsKey(field))
                    map[cat].Add(field, columnValue);
            }
            else
            {
                map.Add(cat, new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase) { { field, columnValue } });
            }
        }
    }

    /// <summary>
    ///     Logging
    /// </summary>
    public class Logging
    {
        private const string LoggingSource = "AkuminaEventLogs";

        public static void LogError(string message)
        {
            /*SPSecurity.RunWithElevatedPrivileges(delegate()
            {*/
            using (SPSite site = new SPSite(SPContext.Current.Web.Site.RootWeb.Url))
            {
                using (SPWeb currentWeb = site.OpenWeb())
                {
                    var loggingList = currentWeb.Lists.TryGetList(LoggingSource);
                    if (loggingList != null)
                    {
                        currentWeb.AllowUnsafeUpdates = true;
                        var item = loggingList.Items.Add();
                        item["Title"] = "Log Written Date:" + " " + DateTime.Now.ToString("d");
                        item["Description"] = SPContext.Current.Web.Url + " - " + message;
                        item.Update();
                        currentWeb.AllowUnsafeUpdates = false;
                    }
                }
            }
            //});
        }

        public static void LogError(Exception exception)
        {
            //var st = new StackTrace();
            //var sf = st.GetFrames();
            //var method = "";
            //if (sf != null)
            //{
            //    method = sf[1].GetMethod().Name;
            //}
            //LogError(string.Format("Error in Method:{0}\nStackTrace:{1}\nError:{2}", method, exception.StackTrace, exception.ToString()));
            LogError(exception.ToString());
        }

        public static void LogError(string message, Exception exception)
        {
            var st = new StackTrace();
            var sf = st.GetFrames();
            var method = "";
            if (sf != null)
            {
                method = sf[1].GetMethod().Name;
            }
            LogError(string.Format("Error in Method:{0}\nInner Exception:{1}\nStackTrace:{2}", method, message, exception.StackTrace));
        }
    }
}