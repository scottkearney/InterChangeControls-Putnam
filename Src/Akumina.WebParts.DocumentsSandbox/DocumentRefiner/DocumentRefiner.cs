using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Akumina.InterAction;
using Akumina.WebParts.Documents.DocumentRefiner;
using Akumina.WebParts.DocumentsSandbox.Properties;
using Microsoft.SharePoint;

namespace Akumina.WebParts.DocumentsSandbox.DocumentRefiner
{
    [ToolboxItem(false)]
    public class DocumentRefiner : DocumentRefinerBaseWebPart
    {
        private readonly string[] _fieldArray = Resources.Refiner_Fields.Split('|');
        private readonly Repeater _rptCategory = new Repeater();
        private readonly TextBox _txtEndDate = new TextBox();
        private readonly TextBox _txtStrDate = new TextBox();
        public List<Filter> Query = new List<Filter>();

        protected override void OnInit(EventArgs e)
        {
            try
            {
                var reset = HttpContext.Current.Request.QueryString["qyreset"];
                if (string.IsNullOrEmpty(reset) || reset == "true")
                {
                    BindChildControls();
                    Page.Load += Page_Load;

                }
            }
            catch (Exception ex)
            {
                Logging.LogError(ex);
            }
        }

        private void Page_Load(object sender, EventArgs e)
        {
            try
            {
                SetRootResourcePathSandbox();
                if (!string.IsNullOrEmpty(InstructionSet))
                {
                    IInstructionRepository clientServices = new InstructionRepository();
                    var response = clientServices.Execute(InstructionSet);
                    if (response != null && response.Dictionary != null)
                    {
                        if (response.Dictionary.Count > 0)
                        {
                            object listName = string.Empty;
                            var values = response.Dictionary.FirstOrDefault().Value;
                            if (values.Count > 0)
                            {
                                values.TryGetValue("ListName", out listName);
                            }
                            if (listName != null && !string.IsNullOrEmpty(listName.ToString()))
                                ListName = listName.ToString();
                        }
                    }
                }
                if (ListName != null && !string.IsNullOrEmpty(ListName))
                    CalFunction();
            }
            catch (Exception ex)
            {
                Logging.LogError(ex);
            }

        }

        private void CalFunction()
        {
            try
            {

                string currFolderPath = HttpContext.Current.Request.QueryString["fd"],
                    currTab = HttpContext.Current.Request.QueryString["tab"],
                    searchterm = HttpContext.Current.Request.QueryString["searchterm"],
                     currQuery = HttpContext.Current.Request.QueryString["qy"],
                    recursive = HttpContext.Current.Request.QueryString["recursive"];
                currFolderPath = !string.IsNullOrEmpty(currFolderPath) ? currFolderPath.Trim() : string.Empty;
                currTab = !string.IsNullOrEmpty(currTab) ? currTab.Replace("$", "#") : string.Empty;
                searchterm = !string.IsNullOrEmpty(searchterm) ? searchterm.Trim() : string.Empty;
                recursive = !string.IsNullOrEmpty(recursive) ? recursive.Trim() : string.Empty;
                currQuery = !string.IsNullOrEmpty(currQuery) ? currQuery.Trim() : string.Empty;

                //var optionResults = BindFields(currFolderPath, currTab, searchterm, recursive);
                //AssignRepeaterTemplate();
                //_rptCategory.DataSource = optionResults;
                //_rptCategory.DataBind();
                //if (!string.IsNullOrEmpty(currQuery))
                //{
                //    setCheckedOptions(currQuery);
                //}

                List<Filter> optionResults = new List<Filter>();
                optionResults.Add(new Filter() { Key = "Modified By", Options = new List<string>() });
                optionResults.Add(new Filter() { Key = "File Type", Options = new List<string>() });
                optionResults.Add(new Filter() { Key = "Category", Options = new List<string>() });

                AssignRepeaterTemplate();
                _rptCategory.DataSource = optionResults;
                _rptCategory.DataBind();
            }
            catch (Exception ex)
            {
                Logging.LogError(ex);
            }
        }

        private void setCheckedOptions(string currQuery)
        {
            try
            {
                List<Filter> itemsTocheck = GetRefineQuery(currQuery);

                List<string> otpions; Filter filterOption; string keyvalue;
                foreach (RepeaterItem aItem in _rptCategory.Items)
                {
                    otpions = new List<string>();
                    filterOption = new Filter();
                    keyvalue = ((HiddenField)aItem.FindControl("hiddenKeyValue")).Value;
                    filterOption = itemsTocheck.Find(x => (x.Key == keyvalue));
                    if (filterOption != null)
                    {
                        Repeater Repeater2 = (Repeater)aItem.FindControl("repOptions");
                        foreach (RepeaterItem withinrepeater in Repeater2.Items)
                        {
                            CheckBox optionCeckbox = (CheckBox)withinrepeater.FindControl("CheckBox1");
                            if (optionCeckbox != null && filterOption.Options.Contains((optionCeckbox.ToolTip)))
                                optionCeckbox.Checked = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logging.LogError(ex);
            }
        }

        protected void BindChildControls()
        {
            try
            {
                var controlDiv = new HtmlGenericControl("DIV");
                controlDiv.Attributes["class"] = "interAction";

                var divAccordion = new HtmlGenericControl("DIV");
                divAccordion.Attributes["class"] = "ia-accordion";

                var divqueryZone = new HtmlGenericControl("DIV");
                divqueryZone.Attributes["class"] = "queryZone";

                var divAccordionField = new HtmlGenericControl("DIV");
                divAccordionField.Attributes["class"] = "ia-filter ia-filter-date ia-accordion-item datefield";

                var h3Header = new HtmlGenericControl("h3");
                h3Header.Attributes["class"] = "ia-filter-header ia-accordion-header";
                h3Header.InnerHtml = "Date";

                var h3Span = new HtmlGenericControl();
                h3Span.Attributes["class"] = "ia-accordion-icon";
                h3Header.Controls.Add(h3Span);

                var divAccordionBody = new HtmlGenericControl("DIV");
                divAccordionBody.Attributes["class"] = "ia-filter-body ia-accordion-body";

                var divfilterrow = new HtmlGenericControl("DIV");
                divfilterrow.Attributes["class"] = "ia-filter-row";

                var labelsmall = new HtmlGenericControl("label");
                labelsmall.Attributes["class"] = "ia-filter-label ia-filter-label-small";
                labelsmall.InnerHtml = "Start:";

                _txtStrDate.ClientIDMode = ClientIDMode.Static;
                _txtStrDate.ID = "txtStrDate";
                _txtStrDate.EnableViewState = true;
                _txtStrDate.Attributes["category"] = "date";
                _txtStrDate.Attributes["onchange"] = "SetRefineQuery();";

                _txtStrDate.CssClass = "ia-filter-input-small ia-datepicker picker__input";

                divfilterrow.Controls.Add(labelsmall);
                divfilterrow.Controls.Add(_txtStrDate);
                divAccordionBody.Controls.Add(divfilterrow);

                var divfilterrowsecondary = new HtmlGenericControl("DIV");
                divfilterrowsecondary.Attributes["class"] = "ia-filter-row";

                var labelsmallsecondary = new HtmlGenericControl("label");
                labelsmallsecondary.Attributes["class"] = "ia-filter-label ia-filter-label-small";
                labelsmallsecondary.InnerHtml = "End:";


                _txtEndDate.ClientIDMode = ClientIDMode.Static;
                _txtEndDate.ID = "txtEndDate";
                _txtEndDate.EnableViewState = true;
                _txtEndDate.Attributes["category"] = "date";
                _txtEndDate.Attributes["onchange"] = "SetRefineQuery();";
                _txtEndDate.CssClass = "ia-filter-input-small ia-datepicker picker__input";

                divfilterrowsecondary.Controls.Add(labelsmallsecondary);
                divfilterrowsecondary.Controls.Add(_txtEndDate);
                divAccordionBody.Controls.Add(divfilterrowsecondary);


                var htmlHRtag = new HtmlGenericControl("hr");
                divAccordionBody.Controls.Add(htmlHRtag);

                var createUl = new HtmlGenericControl("ul");

                var todayli = new HtmlGenericControl("li");
                var lnkbtntoday = new HtmlGenericControl("a");
                lnkbtntoday.ID = "today";
                lnkbtntoday.Attributes["href"] = "#";
                lnkbtntoday.Attributes["onclick"] = "datechange(this);";
                lnkbtntoday.Attributes["date"] = String.Format("{0:MMM dd, yyyy}", DateTime.Now);
                ;
                lnkbtntoday.InnerText = "Today";
                todayli.Controls.Add(lnkbtntoday);

                var weekli = new HtmlGenericControl("li");
                var lnkbtnlstweek = new HtmlGenericControl("a");

                lnkbtnlstweek.ID = "lstweek";
                lnkbtnlstweek.Attributes["href"] = "#";
                lnkbtnlstweek.Attributes["onclick"] = "datechange(this);";
                lnkbtnlstweek.Attributes["date"] = String.Format("{0:MMM dd, yyyy}", DateTime.Now.AddDays(-7));
                lnkbtnlstweek.InnerText = "Last 7 days";
                weekli.Controls.Add(lnkbtnlstweek);

                var monthli = new HtmlGenericControl("li");
                var lnkbtnlstmonth = new HtmlGenericControl("a");

                lnkbtnlstmonth.ID = "month";
                lnkbtnlstmonth.Attributes["href"] = "#";
                lnkbtnlstmonth.Attributes["onclick"] = "datechange(this);";
                lnkbtnlstmonth.Attributes["date"] = String.Format("{0:MMM dd, yyyy}", DateTime.Now.AddDays(-30));
                lnkbtnlstmonth.InnerText = "Last 30 days";

                monthli.Controls.Add(lnkbtnlstmonth);

                var yearli = new HtmlGenericControl("li");
                var lnkbtnlstyear = new HtmlGenericControl("a");
                lnkbtnlstyear.ID = "year";
                lnkbtnlstyear.Attributes["href"] = "#";
                lnkbtnlstyear.Attributes["onclick"] = "datechange(this);";
                lnkbtnlstyear.Attributes["date"] = String.Format("{0:MMM dd, yyyy}", DateTime.Now.AddDays(-365));
                lnkbtnlstyear.InnerText = "This Year";

                yearli.Controls.Add(lnkbtnlstyear);

                createUl.Controls.Add(todayli);
                createUl.Controls.Add(weekli);
                createUl.Controls.Add(monthli);
                createUl.Controls.Add(yearli);
                divAccordionBody.Controls.Add(createUl);
                divAccordionField.Controls.Add(h3Header);
                divAccordionField.Controls.Add(divAccordionBody);


                _rptCategory.ID = "rptCategory";
                _rptCategory.ClientIDMode = ClientIDMode.Static;
                _rptCategory.ItemDataBound += rptCategory_ItemDataBound;


                divqueryZone.Controls.Add(divAccordionField);
                divqueryZone.Controls.Add(_rptCategory);

                divAccordion.Controls.Add(divqueryZone);


                controlDiv.Controls.Add(divAccordion);

                #region Hidden Fields

                #endregion

                Controls.Add(controlDiv);
            }
            catch (Exception ex)
            {
                Logging.LogError(ex);
            }
        }

        private void rptCategory_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            try
            {
                var data = DataBinder.Eval(e.Item.DataItem, "options") as List<string>;
                if (data != null)
                {
                    var repeater2 = (Repeater)e.Item.FindControl("repOptions");
                    repeater2.ItemTemplate = new NestedRepeaterTemplate(ListItemType.Item, "options");
                    repeater2.DataSource = data;
                    repeater2.DataBind();
                }
            }
            catch (Exception ex)
            {
                Logging.LogError(ex);
            }
        }

        private List<Filter> GetRefineQuery(string currQuery)
        {
            var querys = new List<Filter>();
            try
            {
                if (!string.IsNullOrEmpty(currQuery))
                {
                    var filters = currQuery.Split('$');
                    string[] filterVal;
                    foreach (var filter in filters)
                    {
                        filterVal = filter.Split('@');
                        querys.Add(new Filter { Key = filterVal[0], Options = filterVal[1].Split('|').ToList() });
                    }
                }
            }
            catch (Exception ex)
            {
                Logging.LogError(ex);
            }
            return querys;
        }
        private List<Filter> BindFields(string folderPath, string currentTab, string searchterm, string recursive)
        {
            var options = new List<Filter>();
            try
            {
                DataTable dt = null;
                DataView dataView = null;               
                var currentWeb = SPContext.Current.Web;
                var docLibrary = currentWeb.Lists[ListName];
                SPDocumentLibrary docLib = null;
                if (docLibrary != null && docLibrary.BaseType.ToString() == "DocumentLibrary")
                {
                    docLib = (SPDocumentLibrary)currentWeb.Lists[ListName];

                    var sQuery = new SPQuery();
                    sQuery.Query = "<OrderBy><FieldRef Name='Modified' Ascending='False'  /></OrderBy>";
                    sQuery.ViewFields =
                        "<FieldRef Name='ID' /><FieldRef Name='File_x0020_Type' /><FieldRef Name='LinkFilename' /><FieldRef Name='Modified' /><FieldRef Name='Editor' /><FieldRef Name='Category' />";
                    sQuery.ViewFieldsOnly = true;
                    if (!string.IsNullOrEmpty(recursive))
                        sQuery.ViewAttributes = "Scope='Recursive'";

                    if (!string.IsNullOrEmpty(folderPath))
                    {
                        var folder = currentWeb.GetFolder(folderPath);
                        if (folder != null)
                            sQuery.Folder = folder;
                    }
                    var myColl = docLib.GetItems(sQuery);
                    if (myColl != null)
                        dt = myColl.GetDataTable();

                    if (dt != null)
                    {
                        #region searchterm

                        if (!string.IsNullOrEmpty(searchterm))
                        {
                            dataView = dt.AsDataView();
                            dataView.RowFilter = "LinkFilename like '%" + searchterm + "%'";
                            dt = dataView.ToTable();
                        }

                        #endregion

                        EnumerableRowCollection<DataRow> query = from order in dt.AsEnumerable()
                                                                 where string.IsNullOrEmpty(order.Field<string>("File_x0020_Type"))
                                                                 select order;
                        var notebook = "OneNote.Notebook";
                        SPListItem item = null;
                        foreach (DataRow dr in query) // search whole table
                        {
                            DataRow drOne = dt.Select("Id=" + dr["Id"].ToString()).FirstOrDefault();
                            item = docLib.GetItemById(Convert.ToInt32(dr["Id"]));
                            if (item.Folder.ProgID != null && item.Folder.ProgID == notebook)
                            {
                                drOne["File_x0020_Type"] = "one"; //change the file type
                                break;
                            }
                        }

                        if (currentTab != "All Files")
                        {
                            if (currentTab.Contains("|"))
                            {
                                dataView = dt.AsDataView();
                                dataView.RowFilter = currentTab.Split('|')[0];
                                var tempdt = dataView.ToTable();
                                var indexAc = tempdt.Columns.Add("Order", typeof(Int32)).Ordinal;
                                var indexId = tempdt.Columns["ID"].Ordinal;
                                var ids = currentTab.Split('|')[1].Split(',');
                                for (var i = 0; i < tempdt.Rows.Count; i++)
                                    tempdt.Rows[i][indexAc] = Array.IndexOf(ids, tempdt.Rows[i][indexId]);
                                dataView = tempdt.AsDataView();
                                dataView.Sort = "Order ASC";
                                dt = dataView.ToTable();
                            }
                            else
                            {
                                dataView = dt.AsDataView();
                                dataView.RowFilter = currentTab;
                                dt = dataView.ToTable();
                            }
                        }

                        if (dt != null && dt.Rows != null && dt.Rows.Count > 0)
                        {
                            foreach (var field in _fieldArray)
                            {
                                if (dt.Columns.Contains(field))
                                {
                                    var result = (from table in dt.AsEnumerable()
                                                  where !string.IsNullOrEmpty(table.Field<string>(field))
                                                  select
                                                       (table.Field<string>(field).Contains('|')
                                                              ? table.Field<string>(field).Split('|')[0]
                                                              : table.Field<string>(field))).Distinct().ToList();
                                    if (result.Count > 0)
                                    {
                                        if (field == "File_x0020_Type")
                                        {
                                            var tempFile = string.Empty;
                                            var fileTypes = new List<string>();
                                            foreach (var file in result)
                                            {
                                                tempFile = GetFileType.GetKey(file);
                                                if (!string.IsNullOrEmpty(tempFile))
                                                    fileTypes.Add(tempFile);
                                                else
                                                    fileTypes.Add(file.ToUpper());
                                            }
                                            result = fileTypes.Distinct().ToList();
                                        }
                                        else if (field == "Category")
                                        {
                                            var fileTypes = new List<string>();
                                            foreach (var file in result)
                                            {
                                                List<string> list = file.Split('#').Where((value, index) => index % 2 == 1).Select(s => s.Trim(';')).ToList();
                                                fileTypes.AddRange(list);

                                            }
                                            result = fileTypes.Distinct().ToList();

                                        }
                                        var filter = new Filter();
                                        filter.Key = field;
                                        filter.Options = result;
                                        options.Add(filter);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logging.LogError(ex);
            }
            return options;
        }

        protected void AssignFilters()
        {
            EnsureChildControls();
        }

        private void AssignRepeaterTemplate()
        {
            _rptCategory.ItemTemplate = new RepeaterViewTemplate(ListItemType.Item, "key");
        }

        #region WebPart Properties

        //public override EditorPartCollection CreateEditorParts()
        //{
        //    var editorParts = new List<EditorPart>();
        //    var oWpEditor = new WpEditor();
        //    oWpEditor.ID = ID + "_akuminaEditorPart";
        //    oWpEditor.Title = "Akumina InterAction";
        //    editorParts.Add(oWpEditor);
        //    return new EditorPartCollection(base.CreateEditorParts(), editorParts);
        //}

        #endregion
    }
}