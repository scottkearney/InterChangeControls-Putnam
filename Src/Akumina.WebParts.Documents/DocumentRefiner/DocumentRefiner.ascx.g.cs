﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34209
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Akumina.WebParts.Documents.DocumentRefiner {
    using System.Linq;
    using System.Web.Security;
    using System.ComponentModel.DataAnnotations;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;
    using System.Web.UI.WebControls;
    using System.Xml.Linq;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web;
    using System.Configuration;
    using System;
    using System.Text;
    using System.Web.Profile;
    using System.Web.Caching;
    using System.Collections;
    using System.Web.UI.WebControls.WebParts;
    using System.Web.UI.WebControls.Expressions;
    using System.Collections.Specialized;
    using System.Web.SessionState;
    using System.Web.DynamicData;
    using System.CodeDom.Compiler;
    
    
    public partial class DocumentRefiner {
        
        [GeneratedCodeAttribute("Microsoft.VisualStudio.SharePoint.ProjectExtensions.CodeGenerators.SharePointWebP" +
            "artCodeGenerator", "12.0.0.0")]
        protected global::System.Web.UI.WebControls.HiddenField hdnFilterToHide;
        
        [GeneratedCodeAttribute("Microsoft.VisualStudio.SharePoint.ProjectExtensions.CodeGenerators.SharePointWebP" +
            "artCodeGenerator", "12.0.0.0")]
        protected global::System.Web.UI.WebControls.Literal ltlTaxonomyView;
        
        [GeneratedCodeAttribute("Microsoft.VisualStudio.SharePoint.ProjectExtensions.CodeGenerators.SharePointWebP" +
            "artCodeGenerator", "12.0.0.0")]
        protected global::System.Web.UI.HtmlControls.HtmlGenericControl category;
        
        [GeneratedCodeAttribute("Microsoft.VisualStudio.SharePoint.ProjectExtensions.CodeGenerators.SharePointWebP" +
            "artCodeGenerator", "12.0.0.0")]
        protected global::System.Web.UI.HtmlControls.HtmlAnchor today;
        
        [GeneratedCodeAttribute("Microsoft.VisualStudio.SharePoint.ProjectExtensions.CodeGenerators.SharePointWebP" +
            "artCodeGenerator", "12.0.0.0")]
        protected global::System.Web.UI.HtmlControls.HtmlAnchor lstweek;
        
        [GeneratedCodeAttribute("Microsoft.VisualStudio.SharePoint.ProjectExtensions.CodeGenerators.SharePointWebP" +
            "artCodeGenerator", "12.0.0.0")]
        protected global::System.Web.UI.HtmlControls.HtmlAnchor month;
        
        [GeneratedCodeAttribute("Microsoft.VisualStudio.SharePoint.ProjectExtensions.CodeGenerators.SharePointWebP" +
            "artCodeGenerator", "12.0.0.0")]
        protected global::System.Web.UI.HtmlControls.HtmlAnchor year;
        
        [GeneratedCodeAttribute("Microsoft.VisualStudio.SharePoint.ProjectExtensions.CodeGenerators.SharePointWebP" +
            "artCodeGenerator", "12.0.0.0")]
        protected global::System.Web.UI.HtmlControls.HtmlGenericControl dateField;
        
        [GeneratedCodeAttribute("Microsoft.VisualStudio.SharePoint.ProjectExtensions.CodeGenerators.SharePointWebP" +
            "artCodeGenerator", "12.0.0.0")]
        protected global::System.Web.UI.WebControls.Repeater rptCategory;
        
        [GeneratedCodeAttribute("Microsoft.VisualStudio.SharePoint.ProjectExtensions.CodeGenerators.SharePointWebP" +
            "artCodeGenerator", "12.0.0.0")]
        protected global::System.Web.UI.UpdatePanel updateQueryPanel;
        
        [GeneratedCodeAttribute("Microsoft.VisualStudio.SharePoint.ProjectExtensions.CodeGenerators.SharePointWebPartCodeGenerator", "12.0.0.0")]
        public static implicit operator global::System.Web.UI.TemplateControl(DocumentRefiner target) 
        {
            return target == null ? null : target.TemplateControl;
        }
        
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        [GeneratedCodeAttribute("Microsoft.VisualStudio.SharePoint.ProjectExtensions.CodeGenerators.SharePointWebP" +
            "artCodeGenerator", "12.0.0.0")]
        private global::System.Web.UI.WebControls.HiddenField @__BuildControlhdnFilterToHide() {
            global::System.Web.UI.WebControls.HiddenField @__ctrl;
            @__ctrl = new global::System.Web.UI.WebControls.HiddenField();
            this.hdnFilterToHide = @__ctrl;
            @__ctrl.ID = "hdnFilterToHide";
            @__ctrl.ClientIDMode = global::System.Web.UI.ClientIDMode.Static;
            return @__ctrl;
        }
        
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        [GeneratedCodeAttribute("Microsoft.VisualStudio.SharePoint.ProjectExtensions.CodeGenerators.SharePointWebP" +
            "artCodeGenerator", "12.0.0.0")]
        private global::System.Web.UI.WebControls.Literal @__BuildControlltlTaxonomyView() {
            global::System.Web.UI.WebControls.Literal @__ctrl;
            @__ctrl = new global::System.Web.UI.WebControls.Literal();
            this.ltlTaxonomyView = @__ctrl;
            @__ctrl.ID = "ltlTaxonomyView";
            @__ctrl.EnableViewState = true;
            @__ctrl.Text = "";
            return @__ctrl;
        }
        
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        [GeneratedCodeAttribute("Microsoft.VisualStudio.SharePoint.ProjectExtensions.CodeGenerators.SharePointWebP" +
            "artCodeGenerator", "12.0.0.0")]
        private global::System.Web.UI.HtmlControls.HtmlGenericControl @__BuildControlcategory() {
            global::System.Web.UI.HtmlControls.HtmlGenericControl @__ctrl;
            @__ctrl = new global::System.Web.UI.HtmlControls.HtmlGenericControl("div");
            this.category = @__ctrl;
            @__ctrl.TemplateControl = this;
            ((System.Web.UI.IAttributeAccessor)(@__ctrl)).SetAttribute("class", "ia-filter ia-filter-category-browse ia-accordion-item");
            @__ctrl.ID = "category";
            System.Web.UI.IParserAccessor @__parser = ((System.Web.UI.IParserAccessor)(@__ctrl));
            @__parser.AddParsedSubObject(new System.Web.UI.LiteralControl(@"
                        <h3 class=""ia-filter-header ia-accordion-header"">Categories <span class=""ia-accordion-icon ia-open""></span></h3>
                        <div class=""ia-filter-body ia-accordion-body"">
                            <div class=""ia-filter-row"">
                                <input type=""text"" class=""ia-filter-input-small ia-category-input"" readonly />
                                 <input type=""text"" id=""selectedTaxonomyConfrirm"" class=""selectedTaxonomyConfrirm"" style=""display:none;"" />
                                <a class=""ia-button ia-modal-inline-trigger category_browse"" href=""#category-popup"">Browse</a>
                             
                            </div>

                            <div id=""category-popup"" class=""mfp-hide interAction ia-modal"">
                                <div class=""ia-modal-upload"">
                                    <h2 class=""ia-modal-heading"">Browse Tags</h2>

                                    <div class=""interAction refinerCategory"">

                                        <div class=""ia-treeview"">
                                            "));
            global::System.Web.UI.WebControls.Literal @__ctrl1;
            @__ctrl1 = this.@__BuildControlltlTaxonomyView();
            @__parser.AddParsedSubObject(@__ctrl1);
            @__parser.AddParsedSubObject(new System.Web.UI.LiteralControl(@"
                                         
                                            
                                        </div>
                                        <!-- Treeview -->
                                        <div class=""ia-current-selected"">Currently Selected:<span class=""ia-current-selected-list""></span>
                                            <input type=""text"" id=""selectedTaxonomy"" class=""selectedTaxonomy"" style=""visibility:hidden"" />
                                        </div>

                                           

                                        <div class=""ia-button-rows"">
                                            <a class=""ia-button ia-modal-dismiss ia-category-confirm"">OK</a>
                                            <a class=""ia-button secondary ia-modal-dismiss"" href=""#"">Cancel</a>
                                        </div>
                                    </div>

                                </div>
                                <!-- ia-modal -->

                            </div>

                        </div>
                    "));
            return @__ctrl;
        }
        
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        [GeneratedCodeAttribute("Microsoft.VisualStudio.SharePoint.ProjectExtensions.CodeGenerators.SharePointWebP" +
            "artCodeGenerator", "12.0.0.0")]
        private global::System.Web.UI.HtmlControls.HtmlAnchor @__BuildControltoday() {
            global::System.Web.UI.HtmlControls.HtmlAnchor @__ctrl;
            @__ctrl = new global::System.Web.UI.HtmlControls.HtmlAnchor();
            this.today = @__ctrl;
            @__ctrl.ID = "today";
            ((System.Web.UI.IAttributeAccessor)(@__ctrl)).SetAttribute("onclick", "datechange(this);");
            System.Web.UI.IParserAccessor @__parser = ((System.Web.UI.IParserAccessor)(@__ctrl));
            @__parser.AddParsedSubObject(new System.Web.UI.LiteralControl("Today"));
            return @__ctrl;
        }
        
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        [GeneratedCodeAttribute("Microsoft.VisualStudio.SharePoint.ProjectExtensions.CodeGenerators.SharePointWebP" +
            "artCodeGenerator", "12.0.0.0")]
        private global::System.Web.UI.HtmlControls.HtmlAnchor @__BuildControllstweek() {
            global::System.Web.UI.HtmlControls.HtmlAnchor @__ctrl;
            @__ctrl = new global::System.Web.UI.HtmlControls.HtmlAnchor();
            this.lstweek = @__ctrl;
            @__ctrl.ID = "lstweek";
            ((System.Web.UI.IAttributeAccessor)(@__ctrl)).SetAttribute("onclick", "datechange(this);");
            System.Web.UI.IParserAccessor @__parser = ((System.Web.UI.IParserAccessor)(@__ctrl));
            @__parser.AddParsedSubObject(new System.Web.UI.LiteralControl("Last 7 days"));
            return @__ctrl;
        }
        
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        [GeneratedCodeAttribute("Microsoft.VisualStudio.SharePoint.ProjectExtensions.CodeGenerators.SharePointWebP" +
            "artCodeGenerator", "12.0.0.0")]
        private global::System.Web.UI.HtmlControls.HtmlAnchor @__BuildControlmonth() {
            global::System.Web.UI.HtmlControls.HtmlAnchor @__ctrl;
            @__ctrl = new global::System.Web.UI.HtmlControls.HtmlAnchor();
            this.month = @__ctrl;
            @__ctrl.ID = "month";
            ((System.Web.UI.IAttributeAccessor)(@__ctrl)).SetAttribute("onclick", "datechange(this);");
            System.Web.UI.IParserAccessor @__parser = ((System.Web.UI.IParserAccessor)(@__ctrl));
            @__parser.AddParsedSubObject(new System.Web.UI.LiteralControl("Last 30 days"));
            return @__ctrl;
        }
        
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        [GeneratedCodeAttribute("Microsoft.VisualStudio.SharePoint.ProjectExtensions.CodeGenerators.SharePointWebP" +
            "artCodeGenerator", "12.0.0.0")]
        private global::System.Web.UI.HtmlControls.HtmlAnchor @__BuildControlyear() {
            global::System.Web.UI.HtmlControls.HtmlAnchor @__ctrl;
            @__ctrl = new global::System.Web.UI.HtmlControls.HtmlAnchor();
            this.year = @__ctrl;
            @__ctrl.ID = "year";
            ((System.Web.UI.IAttributeAccessor)(@__ctrl)).SetAttribute("onclick", "datechange(this);");
            System.Web.UI.IParserAccessor @__parser = ((System.Web.UI.IParserAccessor)(@__ctrl));
            @__parser.AddParsedSubObject(new System.Web.UI.LiteralControl("This Year"));
            return @__ctrl;
        }
        
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        [GeneratedCodeAttribute("Microsoft.VisualStudio.SharePoint.ProjectExtensions.CodeGenerators.SharePointWebP" +
            "artCodeGenerator", "12.0.0.0")]
        private global::System.Web.UI.HtmlControls.HtmlGenericControl @__BuildControldateField() {
            global::System.Web.UI.HtmlControls.HtmlGenericControl @__ctrl;
            @__ctrl = new global::System.Web.UI.HtmlControls.HtmlGenericControl("div");
            this.dateField = @__ctrl;
            @__ctrl.TemplateControl = this;
            ((System.Web.UI.IAttributeAccessor)(@__ctrl)).SetAttribute("class", "ia-filter ia-filter-date ia-accordion-item datefield");
            @__ctrl.ID = "dateField";
            System.Web.UI.IParserAccessor @__parser = ((System.Web.UI.IParserAccessor)(@__ctrl));
            @__parser.AddParsedSubObject(new System.Web.UI.LiteralControl(@"
                        <h3 class=""ia-filter-header ia-accordion-header"">Date
                            <span class=""ia-accordion-icon""></span>
                        </h3>
                        <div class=""ia-filter-body ia-accordion-body"">
                            <div class=""ia-filter-row"">
                                <label class=""ia-filter-label ia-filter-label-small"">Start:</label>

                                <input type=""text"" id=""txtStrDate"" onchange=""SetRefineQuery();"" category=""date"" class=""ia-filter-input-small ia-datepicker picker__input"" />
                            </div>
                            <div class=""ia-filter-row"">
                                <label class=""ia-filter-label ia-filter-label-small"">End:</label>
                                <input type=""text"" id=""txtEndDate"" onchange=""SetRefineQuery();"" category=""date"" class=""ia-filter-input-small ia-datepicker picker__input"" />


                            </div>
                            <hr>
                            <ul>
                                <li>
                                    "));
            global::System.Web.UI.HtmlControls.HtmlAnchor @__ctrl1;
            @__ctrl1 = this.@__BuildControltoday();
            @__parser.AddParsedSubObject(@__ctrl1);
            @__parser.AddParsedSubObject(new System.Web.UI.LiteralControl("\r\n\r\n\r\n\r\n                                </li>\r\n                                <l" +
                        "i>\r\n                                    "));
            global::System.Web.UI.HtmlControls.HtmlAnchor @__ctrl2;
            @__ctrl2 = this.@__BuildControllstweek();
            @__parser.AddParsedSubObject(@__ctrl2);
            @__parser.AddParsedSubObject(new System.Web.UI.LiteralControl("\r\n\r\n                                </li>\r\n\r\n                                <li>" +
                        "\r\n                                    "));
            global::System.Web.UI.HtmlControls.HtmlAnchor @__ctrl3;
            @__ctrl3 = this.@__BuildControlmonth();
            @__parser.AddParsedSubObject(@__ctrl3);
            @__parser.AddParsedSubObject(new System.Web.UI.LiteralControl("\r\n\r\n                                </li>\r\n                                <li>\r\n" +
                        "                                    "));
            global::System.Web.UI.HtmlControls.HtmlAnchor @__ctrl4;
            @__ctrl4 = this.@__BuildControlyear();
            @__parser.AddParsedSubObject(@__ctrl4);
            @__parser.AddParsedSubObject(new System.Web.UI.LiteralControl("\r\n\r\n                                </li>\r\n\r\n                            </ul>\r\n " +
                        "                       </div>\r\n                    "));
            return @__ctrl;
        }
        
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        [GeneratedCodeAttribute("Microsoft.VisualStudio.SharePoint.ProjectExtensions.CodeGenerators.SharePointWebP" +
            "artCodeGenerator", "12.0.0.0")]
        private global::System.Web.UI.DataBoundLiteralControl @__BuildControl__control4() {
            global::System.Web.UI.DataBoundLiteralControl @__ctrl;
            @__ctrl = new global::System.Web.UI.DataBoundLiteralControl(4, 3);
            @__ctrl.TemplateControl = this;
            @__ctrl.SetStaticString(0, "\r\n                            <div class=\'ia-filter ia-accordion-item otherfield " +
                    "");
            @__ctrl.SetStaticString(1, "\'>\r\n                                <h3 class=\"ia-filter-header ia-accordion-head" +
                    "er\" title=\'");
            @__ctrl.SetStaticString(2, "\'>\r\n                                    ");
            @__ctrl.SetStaticString(3, @"
                                    <span class=""ia-accordion-icon""></span></h3>
                                <div class=""ia-filter-body ia-accordion-body"">
                                </div>
                            </div>
                        ");
            @__ctrl.DataBinding += new System.EventHandler(this.@__DataBind__control4);
            return @__ctrl;
        }
        
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        [GeneratedCodeAttribute("Microsoft.VisualStudio.SharePoint.ProjectExtensions.CodeGenerators.SharePointWebP" +
            "artCodeGenerator", "12.0.0.0")]
        public void @__DataBind__control4(object sender, System.EventArgs e) {
            System.Web.UI.WebControls.RepeaterItem Container;
            System.Web.UI.DataBoundLiteralControl target;
            target = ((System.Web.UI.DataBoundLiteralControl)(sender));
            Container = ((System.Web.UI.WebControls.RepeaterItem)(target.BindingContainer));
            target.SetDataBoundString(0, global::System.Convert.ToString(Eval("ClassName"), global::System.Globalization.CultureInfo.CurrentCulture));
            target.SetDataBoundString(1, global::System.Convert.ToString(Eval("key"), global::System.Globalization.CultureInfo.CurrentCulture));
            target.SetDataBoundString(2, global::System.Convert.ToString(Eval("key").ToString() == "Editor" ? "Modified By" : Eval("key").ToString().Replace("_x0020_", " "), global::System.Globalization.CultureInfo.CurrentCulture));
        }
        
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        [GeneratedCodeAttribute("Microsoft.VisualStudio.SharePoint.ProjectExtensions.CodeGenerators.SharePointWebP" +
            "artCodeGenerator", "12.0.0.0")]
        private void @__BuildControl__control3(System.Web.UI.Control @__ctrl) {
            global::System.Web.UI.DataBoundLiteralControl @__ctrl1;
            @__ctrl1 = this.@__BuildControl__control4();
            System.Web.UI.IParserAccessor @__parser = ((System.Web.UI.IParserAccessor)(@__ctrl));
            @__parser.AddParsedSubObject(@__ctrl1);
        }
        
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        [GeneratedCodeAttribute("Microsoft.VisualStudio.SharePoint.ProjectExtensions.CodeGenerators.SharePointWebP" +
            "artCodeGenerator", "12.0.0.0")]
        private global::System.Web.UI.WebControls.Repeater @__BuildControlrptCategory() {
            global::System.Web.UI.WebControls.Repeater @__ctrl;
            @__ctrl = new global::System.Web.UI.WebControls.Repeater();
            this.rptCategory = @__ctrl;
            @__ctrl.TemplateControl = this;
            @__ctrl.ItemTemplate = new System.Web.UI.CompiledTemplateBuilder(new System.Web.UI.BuildTemplateMethod(this.@__BuildControl__control3));
            @__ctrl.ID = "rptCategory";
            return @__ctrl;
        }
        
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        [GeneratedCodeAttribute("Microsoft.VisualStudio.SharePoint.ProjectExtensions.CodeGenerators.SharePointWebP" +
            "artCodeGenerator", "12.0.0.0")]
        private void @__BuildControl__control2(System.Web.UI.Control @__ctrl) {
            System.Web.UI.IParserAccessor @__parser = ((System.Web.UI.IParserAccessor)(@__ctrl));
            @__parser.AddParsedSubObject(new System.Web.UI.LiteralControl("\r\n                <div class=\"queryZone\">\r\n                    "));
            global::System.Web.UI.HtmlControls.HtmlGenericControl @__ctrl1;
            @__ctrl1 = this.@__BuildControlcategory();
            @__parser.AddParsedSubObject(@__ctrl1);
            @__parser.AddParsedSubObject(new System.Web.UI.LiteralControl("\r\n\r\n           \r\n                    "));
            global::System.Web.UI.HtmlControls.HtmlGenericControl @__ctrl2;
            @__ctrl2 = this.@__BuildControldateField();
            @__parser.AddParsedSubObject(@__ctrl2);
            @__parser.AddParsedSubObject(new System.Web.UI.LiteralControl("\r\n\r\n                    "));
            global::System.Web.UI.WebControls.Repeater @__ctrl3;
            @__ctrl3 = this.@__BuildControlrptCategory();
            @__parser.AddParsedSubObject(@__ctrl3);
            @__parser.AddParsedSubObject(new System.Web.UI.LiteralControl("\r\n                </div>\r\n            "));
        }
        
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        [GeneratedCodeAttribute("Microsoft.VisualStudio.SharePoint.ProjectExtensions.CodeGenerators.SharePointWebP" +
            "artCodeGenerator", "12.0.0.0")]
        private global::System.Web.UI.UpdatePanel @__BuildControlupdateQueryPanel() {
            global::System.Web.UI.UpdatePanel @__ctrl;
            @__ctrl = new global::System.Web.UI.UpdatePanel();
            this.updateQueryPanel = @__ctrl;
            @__ctrl.ContentTemplate = new System.Web.UI.CompiledTemplateBuilder(new System.Web.UI.BuildTemplateMethod(this.@__BuildControl__control2));
            @__ctrl.ID = "updateQueryPanel";
            @__ctrl.UpdateMode = global::System.Web.UI.UpdatePanelUpdateMode.Conditional;
            return @__ctrl;
        }
        
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        [GeneratedCodeAttribute("Microsoft.VisualStudio.SharePoint.ProjectExtensions.CodeGenerators.SharePointWebP" +
            "artCodeGenerator", "12.0.0.0")]
        private void @__BuildControlTree(global::Akumina.WebParts.Documents.DocumentRefiner.DocumentRefiner @__ctrl) {
            global::System.Web.UI.WebControls.HiddenField @__ctrl1;
            @__ctrl1 = this.@__BuildControlhdnFilterToHide();
            System.Web.UI.IParserAccessor @__parser = ((System.Web.UI.IParserAccessor)(@__ctrl));
            @__parser.AddParsedSubObject(@__ctrl1);
            @__parser.AddParsedSubObject(new System.Web.UI.LiteralControl("\r\n<div class=\"interAction\">\r\n    <div class=\"ia-document-filters\">\r\n        "));
            global::System.Web.UI.UpdatePanel @__ctrl2;
            @__ctrl2 = this.@__BuildControlupdateQueryPanel();
            @__parser.AddParsedSubObject(@__ctrl2);
            @__parser.AddParsedSubObject(new System.Web.UI.LiteralControl(@"
      
      
        <script type=""text/javascript"">
            var RefinerPrm = Sys.WebForms.PageRequestManager.getInstance();
            RefinerPrm.add_endRequest(EndRequest);

            function EndRequest(sender, args) {
                EndRequestRefiner();
            }
        </script>
    </div>
</div>


"));
        }
        
        [GeneratedCodeAttribute("Microsoft.VisualStudio.SharePoint.ProjectExtensions.CodeGenerators.SharePointWebP" +
            "artCodeGenerator", "12.0.0.0")]
        private void InitializeControl() {
            this.@__BuildControlTree(this);
            this.Load += new global::System.EventHandler(this.Page_Load);
        }
        
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        [GeneratedCodeAttribute("Microsoft.VisualStudio.SharePoint.ProjectExtensions.CodeGenerators.SharePointWebP" +
            "artCodeGenerator", "12.0.0.0")]
        protected virtual object Eval(string expression) {
            return global::System.Web.UI.DataBinder.Eval(this.Page.GetDataItem(), expression);
        }
        
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        [GeneratedCodeAttribute("Microsoft.VisualStudio.SharePoint.ProjectExtensions.CodeGenerators.SharePointWebP" +
            "artCodeGenerator", "12.0.0.0")]
        protected virtual string Eval(string expression, string format) {
            return global::System.Web.UI.DataBinder.Eval(this.Page.GetDataItem(), expression, format);
        }
    }
}
