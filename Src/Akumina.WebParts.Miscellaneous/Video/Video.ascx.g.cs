﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18449
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Web.UI;

namespace Akumina.WebParts.Miscellaneous.Video {
    public partial class Video {
        
        [GeneratedCode("Microsoft.VisualStudio.SharePoint.ProjectExtensions.CodeGenerators.SharePointWebPartCodeGenerator", "12.0.0.0")]
        public static implicit operator TemplateControl(Video target) 
        {
            return target == null ? null : target.TemplateControl;
        }
        
        [EditorBrowsable(EditorBrowsableState.Never)]
        [GeneratedCode("Microsoft.VisualStudio.SharePoint.ProjectExtensions.CodeGenerators.SharePointWebP" +
            "artCodeGenerator", "12.0.0.0")]
        private void @__BuildControlTree(Video @__ctrl) {
            IParserAccessor @__parser = ((IParserAccessor)(@__ctrl));
            @__parser.AddParsedSubObject(new LiteralControl("\r\n\r\n<h3 class=\"homeHeader\">Video</h3>\r\n<div class=\"videoWidget\">\r\n    <iframe wid" +
                        "th=\"200\" height=\"150\" src=\"//www.youtube.com/embed/LdUPXiglaKE\" frameborder=\"0\" " +
                        "allowfullscreen></iframe>\r\n</div>"));
        }
        
        [GeneratedCode("Microsoft.VisualStudio.SharePoint.ProjectExtensions.CodeGenerators.SharePointWebP" +
            "artCodeGenerator", "12.0.0.0")]
        private void InitializeControl() {
            this.@__BuildControlTree(this);
            this.Load += new EventHandler(this.Page_Load);
        }
        
        [EditorBrowsable(EditorBrowsableState.Never)]
        [GeneratedCode("Microsoft.VisualStudio.SharePoint.ProjectExtensions.CodeGenerators.SharePointWebP" +
            "artCodeGenerator", "12.0.0.0")]
        protected virtual object Eval(string expression) {
            return DataBinder.Eval(this.Page.GetDataItem(), expression);
        }
        
        [EditorBrowsable(EditorBrowsableState.Never)]
        [GeneratedCode("Microsoft.VisualStudio.SharePoint.ProjectExtensions.CodeGenerators.SharePointWebP" +
            "artCodeGenerator", "12.0.0.0")]
        protected virtual string Eval(string expression, string format) {
            return DataBinder.Eval(this.Page.GetDataItem(), expression, format);
        }
    }
}
