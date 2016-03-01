using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Web.UI;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using System.ComponentModel;

namespace Akumina.InterAction
{
    /// <summary>
    /// Base web part to include default properties
    /// </summary>
    public class AkuminaBaseWebPart : WebPart
    {
        /// <summary>
        ///     String denoting custom list and item title of instruction set. ({listname}.{item title} without curly braces)
        /// </summary>
        [Category("Akumina Interaction"), WebDisplayName("Enter the Instructionset"), WebBrowsable(true), Personalizable(PersonalizationScope.Shared)]
        public string InstructionSet { get; set; }

        /// <summary>
        ///     String (starting with slash) leading to the directory where css and js files are loaded from.
        /// </summary>
        [Category("Akumina Interaction"), WebDisplayName("Enter the Script Path"), WebBrowsable(true), Personalizable(PersonalizationScope.Shared)]
        public string RootResourcePath { get; set; }

        protected void WriteJs(HtmlTextWriter writer, string file)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Type, @"text/javascript");
            writer.AddAttribute(HtmlTextWriterAttribute.Src, String.Format("{0}/{1}", RootResourcePath, file));
            writer.RenderBeginTag(HtmlTextWriterTag.Script);
            writer.RenderEndTag(); // script
        }
        protected void WriteCss(HtmlTextWriter writer, string file)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Rel, "text/style");
            writer.AddAttribute(HtmlTextWriterAttribute.Href, String.Format("{0}/{1}", RootResourcePath, file));
            writer.RenderBeginTag(HtmlTextWriterTag.Link);
            writer.RenderEndTag(); // script
        }

        protected InstructionResponse GetInstructionSet(string instructionSetName)
        {
            if (string.IsNullOrWhiteSpace(instructionSetName))
                return new InstructionResponse { Dictionary = new Dictionary<string, Dictionary<string, object>>() };
            IInstructionRepository service = new InstructionRepository();
            var response = service.Execute(instructionSetName);
            return response ?? new InstructionResponse { Dictionary = new Dictionary<string, Dictionary<string, object>>() };
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

        public static Dictionary<string, object> GetItem(string listName, int itemId)
        {
            var fields = new Dictionary<string, string>();
            try
            {
                if (string.IsNullOrEmpty(listName) || itemId <= 0)
                {
                    return null;
                }
                Guid guid;
                SPList list;
                if (Guid.TryParse(listName, out guid))
                {
                    try
                    {
                        list = SPContext.Current.Web.Lists.GetList(guid, false);
                    }
                    catch
                    {
                        list = null;
                    }
                }
                else
                {
                    list = SPContext.Current.Web.Lists.TryGetList(listName);
                }

                if (list != null)
                {
                    var queryWhere = string.Format(@"<Where><Eq><FieldRef Name='ID' /><Value Type='Text'>{0}</Value></Eq></Where>", itemId);


                    var query = new SPQuery { Query = queryWhere, RowLimit = 1 };

                    var items = list.GetItems(query);

                    var dt = items.GetDataTable();
                    if (dt == null) return null;
                    foreach (SPField field in items.Fields)
                    {
                        fields.Add(field.InternalName, field.Title);
                    }
                    var response = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

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
    }


    /// <summary>
    /// Instruction Response
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
                if (defaultValue.GetType().ToString()  == "System.Boolean" )
                {
                    var ret = columnValue != null && !string.IsNullOrWhiteSpace((string) columnValue) &&  columnValue.ToString().ToLower() == "1";
                    columnValue=ret;
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
    /// InstructionConstants
    /// </summary>
    public class InstructionConstants
    {
        /// <summary>
        ///     DefaultCategory hardcoded key
        /// </summary>
        public const string DefaultCategory = "AkuminaInterActionDefault";
    }

    /// <summary>
    /// Instruction Repository interface
    /// </summary>
    public interface IInstructionRepository
    {
        InstructionResponse Execute(string instructionsetName);

        T GetValue<T>(Dictionary<string, Dictionary<string, object>> instructionDictionary, string columnName,
            T defaultValue = default(T));
    }

    /// <summary>
    /// InstructionRepository
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
                    return ProcessListData(null, fields); ;
                }
                if (!instructionsetName.Contains("."))
                {
                    return ProcessListData(null, fields); ;
                }
                var source = instructionsetName.Split(new[] { '.' }, 2);
                var listName = source[0];
                var itemTitle = source[1];
                if (string.IsNullOrEmpty(listName) || string.IsNullOrEmpty(itemTitle))
                {
                    return ProcessListData(null, fields); ;
                }
                Guid guid;
                SPList list;
                if (Guid.TryParse(listName, out guid))
                {
                    try
                    {
                        list = SPContext.Current.Web.Lists.GetList(guid, false);
                    }
                    catch
                    {
                        list = null;
                    }
                }
                else
                {
                    list = SPContext.Current.Web.Lists.TryGetList(listName);
                }

                if (list != null)
                {
                    int itemId;
                    string queryWhere = "";
                    if (Int32.TryParse(itemTitle, out itemId))
                    {
                        queryWhere = string.Format(@"<Where><Eq><FieldRef Name='ID' /><Value Type='Text'>{0}</Value></Eq></Where>", itemId);
                    }
                    else
                    {
                        queryWhere = string.Format(@"<Where><Eq><FieldRef Name='Title' /><Value Type='Text'>{0}</Value></Eq></Where>", itemTitle);
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
            return ProcessListData(null, fields); ;
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
    /// Logging
    /// </summary>
    public class Logging
    {
        private static String Errormsg, Extype, ErrorLocation;
        public static void LogError(string message)
        {

        }
        public static void LogError(Exception exception)
        {
            using (SPWeb CurrentWeb = SPContext.Current.Web)
            {               
                SPList EventLogList = CurrentWeb.Lists.TryGetList("AkuminaEventLog");
                if (EventLogList != null)
                {
                    CurrentWeb.AllowUnsafeUpdates = true;
                    SPListItem item = EventLogList.Items.Add();

                    var line = Environment.NewLine + Environment.NewLine;
                    Errormsg = exception.GetType().Name.ToString();
                    Extype = exception.GetType().ToString();
                    ErrorLocation = exception.Message.ToString();

                    item["Title"] = "Log Written Date:" + " " + DateTime.Now.ToString("d");
                    item["Description"] = "Error Message: " + Errormsg + line + "Exception Type: " + Extype + line + "Error Location: " + ErrorLocation + line;
                    item.Update();
                    CurrentWeb.AllowUnsafeUpdates = false;
                }
            }
        }
    }

}