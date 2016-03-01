using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Akumina.InterAction;

namespace Akumina.WebParts.DocumentsSandbox.DocumentRefiner
{
    internal class RepeaterViewTemplate : ITemplate
    {
        public RepeaterViewTemplate(ListItemType type, string colname)
        {
            //Stores the template type.
            TemplateType = type;

            //Stores the column name.
            ColumnName = colname;
        }

        //A variable to hold the type of ListItemType.

        //ListItemType _liType;
        //string _columnname;
        public ListItemType TemplateType { get; set; }
        //A variable to hold the column name.
        public string ColumnName { get; set; }

        void ITemplate.InstantiateIn(Control container)
        {
            try
            {
                switch (TemplateType)
                {
                    case ListItemType.Item:
                        var filterDiv = new HtmlGenericControl("div");
                        filterDiv.Attributes["class"] = "ia-filter ia-filter-modified-by ia-accordion-item otherfield";

                        var filterHeading = new HtmlGenericControl("h3");
                        filterHeading.Attributes["class"] = "ia-filter-header ia-accordion-header";
                        filterHeading.DataBinding += filterHeading_DataBinding;

                        var filterDivBody = new HtmlGenericControl("div");
                        filterDivBody.Attributes["class"] = "ia-filter-body ia-accordion-body";

                        var hiddenKeyValue = new HiddenField();
                        hiddenKeyValue.ID = "hiddenKeyValue";
                        hiddenKeyValue.DataBinding += hiddenKeyValue_DataBinding;

                        filterDivBody.Controls.Add(hiddenKeyValue);

                        var repOptions = new Repeater();
                        repOptions.ID = "repOptions";

                        filterDivBody.Controls.Add(repOptions);
                        filterDiv.Controls.Add(filterHeading);
                        filterDiv.Controls.Add(filterDivBody);

                        container.Controls.Add(filterDiv);
                        break;
                }
            }
            catch (Exception ex)
            {
                Logging.LogError(ex);
            }
        }

        private void repOptions_DataBinding(object sender, EventArgs e)
        {
            try
            {
                var repOptions = (Repeater)sender;
                var container = (RepeaterItem)repOptions.NamingContainer;
                var dataValue = DataBinder.Eval(container.DataItem, "options");
                if (dataValue != null)
                {
                    repOptions.DataSource = dataValue as List<string>;
                    repOptions.DataBind();
                }
            }
            catch (Exception ex)
            {
                Logging.LogError(ex);
            }
        }

        private void hiddenKeyValue_DataBinding(object sender, EventArgs e)
        {
            try
            {
                var hiddenKeyValue = (HiddenField)sender;
                var container = (RepeaterItem)hiddenKeyValue.NamingContainer;
                var dataValue = DataBinder.Eval(container.DataItem, "key");
                if (dataValue != null)
                {
                    hiddenKeyValue.Value = dataValue.ToString();
                }
            }
            catch (Exception ex)
            {
                Logging.LogError(ex);
            }
        }

        private void filterHeading_DataBinding(object sender, EventArgs e)
        {
            try
            {

                var filterHeading = (HtmlGenericControl)sender;
                var container = (RepeaterItem)filterHeading.NamingContainer;
                var dataValue = DataBinder.Eval(container.DataItem, "key");
                if (dataValue != null)
                {
                    filterHeading.Attributes["title"] = dataValue.ToString();
                    var titleStr = dataValue.ToString() == "Editor"
                        ? "Modified By"
                        : dataValue.ToString().Replace("_x0020_", " ");
                    filterHeading.InnerText = titleStr;
                }
                else
                {
                    filterHeading.Attributes["title"] = string.Empty;
                    filterHeading.InnerText = "Default Heading";
                }
                var accordianIcon = new HtmlGenericControl();
                accordianIcon.Attributes["class"] = "ia-accordion-icon";
                filterHeading.Controls.Add(accordianIcon);
            }
            catch (Exception ex)
            {
                Logging.LogError(ex);
            }
        }
    }

    internal class NestedRepeaterTemplate : DocumentRefiner, ITemplate
    {
        public NestedRepeaterTemplate(ListItemType type, string colname)
        {
            //Stores the template type.
            TemplateType = type;

            //Stores the column name.
            ColumnName = colname;
        }

        //A variable to hold the type of ListItemType.

        //ListItemType _liType;
        //string _columnname;
        public ListItemType TemplateType { get; set; }
        //A variable to hold the column name.

        public string ColumnName { get; set; }

        void ITemplate.InstantiateIn(Control container)
        {
            try
            {
                switch (TemplateType)
                {
                    case ListItemType.Item:
                        var filterRowDiv = new HtmlGenericControl("div");
                        filterRowDiv.Attributes["class"] = "ia-filter-row";

                        var checkBox1 = new CheckBox();
                        checkBox1.ID = "CheckBox1";
                        checkBox1.CssClass = "chkSelect";

                        checkBox1.DataBinding += CheckBox1_DataBinding;
                        //checkBox1.CheckedChanged += CheckBox1_CheckedChanged;

                        var label1 = new Label();
                        label1.ID = "Label1";
                        label1.CssClass = "ia-filter-label-checkbox";
                        label1.DataBinding += Label1_DataBinding;

                        filterRowDiv.Controls.Add(checkBox1);
                        filterRowDiv.Controls.Add(label1);

                        container.Controls.Add(filterRowDiv);
                        break;
                }
            }
            catch (Exception ex)
            {
                Logging.LogError(ex);
            }
        }

        private void Label1_DataBinding(object sender, EventArgs e)
        {
            var label1 = (Label)sender;
            var container = (RepeaterItem)label1.NamingContainer;
            var dataValue = container.DataItem;
            if (dataValue != null)
            {
                label1.Text = dataValue.ToString();
            }
        }

        private void CheckBox1_DataBinding(object sender, EventArgs e)
        {
            var checkBox1 = (CheckBox)sender;
            var container = (RepeaterItem)checkBox1.NamingContainer;
            var dataValue = container.DataItem;
            if (dataValue != null)
            {
                checkBox1.Attributes["onchange"] = "SetRefineQuery();";
                checkBox1.ToolTip = dataValue.ToString();

                //checkBox1.AutoPostBack = true;
                checkBox1.EnableViewState = true;
            }
        }
    }
}