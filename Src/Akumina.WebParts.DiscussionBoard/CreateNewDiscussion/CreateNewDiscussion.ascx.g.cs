﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34209
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Akumina.WebParts.DiscussionBoard.CreateNewDiscussion {
    using System.Linq;
    using System.Web.Security;
    using System.ComponentModel.DataAnnotations;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;
    using System.Web.UI.WebControls;
    using System.Xml.Linq;
    using CKEditor.NET;
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
    
    
    public partial class CreateNewDiscussion {
        
        [GeneratedCodeAttribute("Microsoft.VisualStudio.SharePoint.ProjectExtensions.CodeGenerators.SharePointWebP" +
            "artCodeGenerator", "12.0.0.0")]
        protected global::System.Web.UI.WebControls.Literal litTop;
        
        [GeneratedCodeAttribute("Microsoft.VisualStudio.SharePoint.ProjectExtensions.CodeGenerators.SharePointWebP" +
            "artCodeGenerator", "12.0.0.0")]
        protected global::System.Web.UI.WebControls.Literal litrlTitle;
        
        [GeneratedCodeAttribute("Microsoft.VisualStudio.SharePoint.ProjectExtensions.CodeGenerators.SharePointWebP" +
            "artCodeGenerator", "12.0.0.0")]
        protected global::System.Web.UI.HtmlControls.HtmlInputText titleDiscussion;
        
        [GeneratedCodeAttribute("Microsoft.VisualStudio.SharePoint.ProjectExtensions.CodeGenerators.SharePointWebP" +
            "artCodeGenerator", "12.0.0.0")]
        protected global::CKEditor.NET.CKEditorControl bodyDiscussion;
        
        [GeneratedCodeAttribute("Microsoft.VisualStudio.SharePoint.ProjectExtensions.CodeGenerators.SharePointWebP" +
            "artCodeGenerator", "12.0.0.0")]
        protected global::System.Web.UI.HtmlControls.HtmlGenericControl listOfFiles;
        
        [GeneratedCodeAttribute("Microsoft.VisualStudio.SharePoint.ProjectExtensions.CodeGenerators.SharePointWebP" +
            "artCodeGenerator", "12.0.0.0")]
        protected global::System.Web.UI.HtmlControls.HtmlInputText listOfFilesHn;
        
        [GeneratedCodeAttribute("Microsoft.VisualStudio.SharePoint.ProjectExtensions.CodeGenerators.SharePointWebP" +
            "artCodeGenerator", "12.0.0.0")]
        protected global::System.Web.UI.HtmlControls.HtmlGenericControl listOfFilesHnSec;
        
        [GeneratedCodeAttribute("Microsoft.VisualStudio.SharePoint.ProjectExtensions.CodeGenerators.SharePointWebP" +
            "artCodeGenerator", "12.0.0.0")]
        protected global::System.Web.UI.WebControls.Literal ltlFolderInfo;
        
        [GeneratedCodeAttribute("Microsoft.VisualStudio.SharePoint.ProjectExtensions.CodeGenerators.SharePointWebP" +
            "artCodeGenerator", "12.0.0.0")]
        protected global::System.Web.UI.WebControls.ListBox userAndUserGroups;
        
        [GeneratedCodeAttribute("Microsoft.VisualStudio.SharePoint.ProjectExtensions.CodeGenerators.SharePointWebP" +
            "artCodeGenerator", "12.0.0.0")]
        protected global::System.Web.UI.WebControls.Button postDisucssion;
        
        [GeneratedCodeAttribute("Microsoft.VisualStudio.SharePoint.ProjectExtensions.CodeGenerators.SharePointWebP" +
            "artCodeGenerator", "12.0.0.0")]
        protected global::System.Web.UI.HtmlControls.HtmlAnchor btncanConfirm;
        
        [GeneratedCodeAttribute("Microsoft.VisualStudio.SharePoint.ProjectExtensions.CodeGenerators.SharePointWebPartCodeGenerator", "12.0.0.0")]
        public static implicit operator global::System.Web.UI.TemplateControl(CreateNewDiscussion target) 
        {
            return target == null ? null : target.TemplateControl;
        }
        
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        [GeneratedCodeAttribute("Microsoft.VisualStudio.SharePoint.ProjectExtensions.CodeGenerators.SharePointWebP" +
            "artCodeGenerator", "12.0.0.0")]
        private global::System.Web.UI.WebControls.Literal @__BuildControllitTop() {
            global::System.Web.UI.WebControls.Literal @__ctrl;
            @__ctrl = new global::System.Web.UI.WebControls.Literal();
            this.litTop = @__ctrl;
            @__ctrl.ID = "litTop";
            return @__ctrl;
        }
        
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        [GeneratedCodeAttribute("Microsoft.VisualStudio.SharePoint.ProjectExtensions.CodeGenerators.SharePointWebP" +
            "artCodeGenerator", "12.0.0.0")]
        private global::System.Web.UI.WebControls.Literal @__BuildControllitrlTitle() {
            global::System.Web.UI.WebControls.Literal @__ctrl;
            @__ctrl = new global::System.Web.UI.WebControls.Literal();
            this.litrlTitle = @__ctrl;
            @__ctrl.ID = "litrlTitle";
            return @__ctrl;
        }
        
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        [GeneratedCodeAttribute("Microsoft.VisualStudio.SharePoint.ProjectExtensions.CodeGenerators.SharePointWebP" +
            "artCodeGenerator", "12.0.0.0")]
        private global::System.Web.UI.HtmlControls.HtmlInputText @__BuildControltitleDiscussion() {
            global::System.Web.UI.HtmlControls.HtmlInputText @__ctrl;
            @__ctrl = new global::System.Web.UI.HtmlControls.HtmlInputText();
            this.titleDiscussion = @__ctrl;
            @__ctrl.Name = "ia-discussion-create-title";
            ((System.Web.UI.IAttributeAccessor)(@__ctrl)).SetAttribute("class", "ia-discussion-create-title");
            @__ctrl.ID = "titleDiscussion";
            ((System.Web.UI.IAttributeAccessor)(@__ctrl)).SetAttribute("placeholder", "Enter a title for the discussion");
            ((System.Web.UI.IAttributeAccessor)(@__ctrl)).SetAttribute("type", "text");
            return @__ctrl;
        }
        
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        [GeneratedCodeAttribute("Microsoft.VisualStudio.SharePoint.ProjectExtensions.CodeGenerators.SharePointWebP" +
            "artCodeGenerator", "12.0.0.0")]
        private global::CKEditor.NET.CKEditorControl @__BuildControlbodyDiscussion() {
            global::CKEditor.NET.CKEditorControl @__ctrl;
            @__ctrl = new global::CKEditor.NET.CKEditorControl();
            this.bodyDiscussion = @__ctrl;
            @__ctrl.ApplyStyleSheetSkin(this.Page);
            @__ctrl.ID = "bodyDiscussion";
            @__ctrl.BasePath = "../_layouts/15/Akumina.WebParts.DiscussionBoard/ckeditor/";
            return @__ctrl;
        }
        
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        [GeneratedCodeAttribute("Microsoft.VisualStudio.SharePoint.ProjectExtensions.CodeGenerators.SharePointWebP" +
            "artCodeGenerator", "12.0.0.0")]
        private global::System.Web.UI.HtmlControls.HtmlGenericControl @__BuildControllistOfFiles() {
            global::System.Web.UI.HtmlControls.HtmlGenericControl @__ctrl;
            @__ctrl = new global::System.Web.UI.HtmlControls.HtmlGenericControl("div");
            this.listOfFiles = @__ctrl;
            @__ctrl.ID = "listOfFiles";
            ((System.Web.UI.IAttributeAccessor)(@__ctrl)).SetAttribute("class", "listOfFiles");
            return @__ctrl;
        }
        
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        [GeneratedCodeAttribute("Microsoft.VisualStudio.SharePoint.ProjectExtensions.CodeGenerators.SharePointWebP" +
            "artCodeGenerator", "12.0.0.0")]
        private global::System.Web.UI.HtmlControls.HtmlInputText @__BuildControllistOfFilesHn() {
            global::System.Web.UI.HtmlControls.HtmlInputText @__ctrl;
            @__ctrl = new global::System.Web.UI.HtmlControls.HtmlInputText();
            this.listOfFilesHn = @__ctrl;
            ((System.Web.UI.IAttributeAccessor)(@__ctrl)).SetAttribute("type", "text");
            ((System.Web.UI.IAttributeAccessor)(@__ctrl)).SetAttribute("class", "listOfFilesHidden");
            @__ctrl.ID = "listOfFilesHn";
            ((System.Web.UI.IAttributeAccessor)(@__ctrl)).SetAttribute("style", "display: none");
            return @__ctrl;
        }
        
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        [GeneratedCodeAttribute("Microsoft.VisualStudio.SharePoint.ProjectExtensions.CodeGenerators.SharePointWebP" +
            "artCodeGenerator", "12.0.0.0")]
        private global::System.Web.UI.HtmlControls.HtmlGenericControl @__BuildControllistOfFilesHnSec() {
            global::System.Web.UI.HtmlControls.HtmlGenericControl @__ctrl;
            @__ctrl = new global::System.Web.UI.HtmlControls.HtmlGenericControl("div");
            this.listOfFilesHnSec = @__ctrl;
            ((System.Web.UI.IAttributeAccessor)(@__ctrl)).SetAttribute("class", "listOfFilesHiddenSec");
            @__ctrl.ID = "listOfFilesHnSec";
            ((System.Web.UI.IAttributeAccessor)(@__ctrl)).SetAttribute("style", "display: none");
            return @__ctrl;
        }
        
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        [GeneratedCodeAttribute("Microsoft.VisualStudio.SharePoint.ProjectExtensions.CodeGenerators.SharePointWebP" +
            "artCodeGenerator", "12.0.0.0")]
        private global::System.Web.UI.WebControls.Literal @__BuildControlltlFolderInfo() {
            global::System.Web.UI.WebControls.Literal @__ctrl;
            @__ctrl = new global::System.Web.UI.WebControls.Literal();
            this.ltlFolderInfo = @__ctrl;
            @__ctrl.ID = "ltlFolderInfo";
            @__ctrl.EnableViewState = true;
            @__ctrl.Text = "";
            return @__ctrl;
        }
        
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        [GeneratedCodeAttribute("Microsoft.VisualStudio.SharePoint.ProjectExtensions.CodeGenerators.SharePointWebP" +
            "artCodeGenerator", "12.0.0.0")]
        private global::System.Web.UI.WebControls.ListBox @__BuildControluserAndUserGroups() {
            global::System.Web.UI.WebControls.ListBox @__ctrl;
            @__ctrl = new global::System.Web.UI.WebControls.ListBox();
            this.userAndUserGroups = @__ctrl;
            @__ctrl.ApplyStyleSheetSkin(this.Page);
            @__ctrl.ID = "userAndUserGroups";
            @__ctrl.SelectionMode = global::System.Web.UI.WebControls.ListSelectionMode.Multiple;
            ((System.Web.UI.IAttributeAccessor)(@__ctrl)).SetAttribute("data-placeholder", "Select users who can view this post...");
            ((System.Web.UI.IAttributeAccessor)(@__ctrl)).SetAttribute("class", "ia-search-picker");
            return @__ctrl;
        }
        
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        [GeneratedCodeAttribute("Microsoft.VisualStudio.SharePoint.ProjectExtensions.CodeGenerators.SharePointWebP" +
            "artCodeGenerator", "12.0.0.0")]
        private global::System.Web.UI.WebControls.Button @__BuildControlpostDisucssion() {
            global::System.Web.UI.WebControls.Button @__ctrl;
            @__ctrl = new global::System.Web.UI.WebControls.Button();
            this.postDisucssion = @__ctrl;
            @__ctrl.ApplyStyleSheetSkin(this.Page);
            @__ctrl.ID = "postDisucssion";
            @__ctrl.CssClass = "ia-button";
            @__ctrl.Text = "Post discussion";
            @__ctrl.OnClientClick = "postValidation(event);";
            @__ctrl.Click -= new System.EventHandler(this.postDisucssion_Click);
            @__ctrl.Click += new System.EventHandler(this.postDisucssion_Click);
            return @__ctrl;
        }
        
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        [GeneratedCodeAttribute("Microsoft.VisualStudio.SharePoint.ProjectExtensions.CodeGenerators.SharePointWebP" +
            "artCodeGenerator", "12.0.0.0")]
        private global::System.Web.UI.HtmlControls.HtmlAnchor @__BuildControlbtncanConfirm() {
            global::System.Web.UI.HtmlControls.HtmlAnchor @__ctrl;
            @__ctrl = new global::System.Web.UI.HtmlControls.HtmlAnchor();
            this.btncanConfirm = @__ctrl;
            ((System.Web.UI.IAttributeAccessor)(@__ctrl)).SetAttribute("class", "ia-button");
            @__ctrl.ID = "btncanConfirm";
            System.Web.UI.IParserAccessor @__parser = ((System.Web.UI.IParserAccessor)(@__ctrl));
            @__parser.AddParsedSubObject(new System.Web.UI.LiteralControl("Yes"));
            @__ctrl.ServerClick -= new System.EventHandler(this.cancelPostDiscussion_Click);
            @__ctrl.ServerClick += new System.EventHandler(this.cancelPostDiscussion_Click);
            return @__ctrl;
        }
        
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        [GeneratedCodeAttribute("Microsoft.VisualStudio.SharePoint.ProjectExtensions.CodeGenerators.SharePointWebP" +
            "artCodeGenerator", "12.0.0.0")]
        private void @__BuildControlTree(global::Akumina.WebParts.DiscussionBoard.CreateNewDiscussion.CreateNewDiscussion @__ctrl) {
            System.Web.UI.IParserAccessor @__parser = ((System.Web.UI.IParserAccessor)(@__ctrl));
            @__parser.AddParsedSubObject(new System.Web.UI.LiteralControl("\r\n\r\n\r\n\r\n<!-- JS for Search Picker -->\r\n\r\n\r\n"));
            global::System.Web.UI.WebControls.Literal @__ctrl1;
            @__ctrl1 = this.@__BuildControllitTop();
            @__parser.AddParsedSubObject(@__ctrl1);
            @__parser.AddParsedSubObject(new System.Web.UI.LiteralControl("\r\n\r\n<div class=\"interAction\">\r\n\r\n    <section class=\"ia-discussion-create-new ia-" +
                        "discussion-thread\">\r\n\r\n        <h1>\r\n            "));
            global::System.Web.UI.WebControls.Literal @__ctrl2;
            @__ctrl2 = this.@__BuildControllitrlTitle();
            @__parser.AddParsedSubObject(@__ctrl2);
            @__parser.AddParsedSubObject(new System.Web.UI.LiteralControl("\r\n        </h1>\r\n        <h2 class=\"ia-discussion-create-headline\">Start a new di" +
                        "scussion</h2>\r\n\r\n\r\n        <div class=\"ia-discussion-create-title-box\">\r\n       " +
                        "     "));
            global::System.Web.UI.HtmlControls.HtmlInputText @__ctrl3;
            @__ctrl3 = this.@__BuildControltitleDiscussion();
            @__parser.AddParsedSubObject(@__ctrl3);
            @__parser.AddParsedSubObject(new System.Web.UI.LiteralControl("\r\n            <div style=\"color: red; display: none\" id=\"titleMandatory\">*Title i" +
                        "s required</div>\r\n        </div>\r\n\r\n\r\n        <!-- Placeholder for Rich Text Edi" +
                        "tor -->\r\n        <div class=\"ia-discussion-richtext-editor\">\r\n            "));
            global::CKEditor.NET.CKEditorControl @__ctrl4;
            @__ctrl4 = this.@__BuildControlbodyDiscussion();
            @__parser.AddParsedSubObject(@__ctrl4);
            @__parser.AddParsedSubObject(new System.Web.UI.LiteralControl(@"

            
        </div>

        <!-- Opens Modal with Document Library -->
        <p class=""ia-discussion-library-selector"">
            <span class=""fa fa-paperclip""></span>To add a link to a file or folder, 
			
            <a href=""#ia-discussion-reply-folder-tree-modal"" class=""ia-modal-inline-trigger"">select from the document library.</a>
        </p>

        <!-- Action Items under text editor -->


        <script id=""files_Addtemplate"" type=""text/html"">
            <span class=""ia-discussion-attachments-action-btn"" item-id=""{{ItemId}}"" list-name=""{{ListName}}"" item-url=""{{Url}}""><a href=""#"" onclick=""fileRemove(this);""><span class=""fa fa-times-circle""></span></a>{{Name}}
            </span>
        </script>
        <div class=""ia-discussion-thread-attachments-edit"">
            "));
            global::System.Web.UI.HtmlControls.HtmlGenericControl @__ctrl5;
            @__ctrl5 = this.@__BuildControllistOfFiles();
            @__parser.AddParsedSubObject(@__ctrl5);
            @__parser.AddParsedSubObject(new System.Web.UI.LiteralControl("\r\n\r\n            "));
            global::System.Web.UI.HtmlControls.HtmlInputText @__ctrl6;
            @__ctrl6 = this.@__BuildControllistOfFilesHn();
            @__parser.AddParsedSubObject(@__ctrl6);
            @__parser.AddParsedSubObject(new System.Web.UI.LiteralControl("\r\n            "));
            global::System.Web.UI.HtmlControls.HtmlGenericControl @__ctrl7;
            @__ctrl7 = this.@__BuildControllistOfFilesHnSec();
            @__parser.AddParsedSubObject(@__ctrl7);
            @__parser.AddParsedSubObject(new System.Web.UI.LiteralControl(@"
        </div>
        <!-- Document Library Modal -->
        <div id=""ia-discussion-reply-folder-tree-modal"" class=""mfp-hide interAction ia-modal"">
            <div class=""ia-modal-upload"">
                <h2 class=""ia-modal-heading"">Choose a file to link</h2>

                <!-- Document Folder Tree -->
                <div class=""ia-folder-tree"">
                    "));
            global::System.Web.UI.WebControls.Literal @__ctrl8;
            @__ctrl8 = this.@__BuildControlltlFolderInfo();
            @__parser.AddParsedSubObject(@__ctrl8);
            @__parser.AddParsedSubObject(new System.Web.UI.LiteralControl(@"
                </div>

                <div class=""ia-button-row"">
                    <a class=""ia-button"" id=""addFileRef"" onclick=""addSelectedFile(event);"">OK</a>

                    <a class=""ia-button secondary ia-modal-dismiss"" href=""#"">Cancel</a>
                </div>

            </div>
        </div>

        <hr>
        <p>
            By default, this discussion will be visible by all authorized users of this site. If you want to control who can view this discussion thread, please select the users or groups in the box below.
        </p>


        <!-- Search Picker -->
        
        <div class=""ia-search-picker-wrapper"">
            "));
            global::System.Web.UI.WebControls.ListBox @__ctrl9;
            @__ctrl9 = this.@__BuildControluserAndUserGroups();
            @__parser.AddParsedSubObject(@__ctrl9);
            @__parser.AddParsedSubObject(new System.Web.UI.LiteralControl("\r\n            \r\n        </div>\r\n        <!-- end .ia-search-picker-wrapper -->\r\n\r" +
                        "\n\r\n        <div class=\"ia-discussion-create-cta\">\r\n            "));
            global::System.Web.UI.WebControls.Button @__ctrl10;
            @__ctrl10 = this.@__BuildControlpostDisucssion();
            @__parser.AddParsedSubObject(@__ctrl10);
            @__parser.AddParsedSubObject(new System.Web.UI.LiteralControl(@"
            
            
            <a href=""#ia-discussion-cancel-modal"" class=""ia-modal-inline-trigger ia-discussion-cancel ia-button secondary ia-discussion-cancel-btn"">Cancel</a>
            
        </div>
        <!-- Delete Modal -->
        <div id=""ia-discussion-cancel-modal"" class=""mfp-hide interAction ia-modal"">
            <div class=""ia-modal-upload"">
                <h2 class=""ia-modal-heading"">Confirm Cancel</h2>
                <p>Are you sure you want to cancel? </p>

                <div class=""ia-button-row"">
                    
                    "));
            global::System.Web.UI.HtmlControls.HtmlAnchor @__ctrl11;
            @__ctrl11 = this.@__BuildControlbtncanConfirm();
            @__parser.AddParsedSubObject(@__ctrl11);
            @__parser.AddParsedSubObject(new System.Web.UI.LiteralControl(@"
                    <a class=""ia-button secondary ia-modal-dismiss"" href=""#"">No</a>
                </div>

            </div>
        </div>

    </section>
    <!-- end .ia-discussion-create-new -->
    <div class=""ia-loading-panel ia-hide"">

        <div class=""ia-loader""></div>

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
