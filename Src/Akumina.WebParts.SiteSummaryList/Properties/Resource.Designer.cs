﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18408
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace Akumina.WebParts.SiteSummaryList.Properties {
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [DebuggerNonUserCode()]
    [CompilerGenerated()]
    internal class Resource {
        
        private static ResourceManager resourceMan;
        
        private static CultureInfo resourceCulture;
        
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resource() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        internal static ResourceManager ResourceManager {
            get {
                if (ReferenceEquals(resourceMan, null)) {
                    ResourceManager temp = new ResourceManager("Akumina.WebParts.SiteSummaryList.Properties.Resource", typeof(Resource).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        internal static CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;div class=&quot;interAction&quot;&gt;
        ///		&lt;div class=&quot;ia-transformer-tabs&quot;&gt;
        ///			&lt;nav role=&apos;navigation&apos; class=&quot;ia-transformer-tab-nav&quot;&gt;
        ///				&lt;ul&gt;
        ///					&lt;li class=&quot;ia-tab-active&quot;&gt;&lt;a href=&quot;#&quot; class=&quot;ia-tab-active-link&quot;&gt;New&lt;/a&gt;&lt;/li&gt;
        ///					&lt;li&gt;&lt;a href=&quot;#&quot;&gt;My Recent&lt;/a&gt;&lt;/li&gt;
        ///					&lt;li&gt;&lt;a href=&quot;#&quot;&gt;Popular&lt;/a&gt;&lt;/li&gt;
        ///					&lt;li&gt;&lt;a href=&quot;#&quot;&gt;Recommended&lt;/a&gt;&lt;/li&gt;
        ///				&lt;/ul&gt;
        ///			&lt;/nav&gt;
        ///
        ///			&lt;div class=&quot;ia-single-tab ia-tab-active-link&quot;&gt;
        ///				&lt;!-- start site summary --&gt;
        ///				&lt;div class=&quot;ia-site-summary&quot;&gt;
        ///					&lt;table class=&quot;ia-site-summary-list tab [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string ControlTemplate {
            get {
                return ResourceManager.GetString("ControlTemplate", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;tr&gt;
        ///    &lt;td&gt;&lt;a href=&quot;{{SITEURLH}}&quot;&gt;{{SITENAME}}&lt;/a&gt;&lt;/td&gt;
        ///    &lt;td&gt;{{SITEDATE}}&lt;/td&gt;
        ///&lt;/tr&gt;
        ///&lt;!--&lt;div class=&quot;ia-banner-item&quot;&gt; 
        ///	&lt;img class=&quot;ia-banner-img&quot; src=&quot;{{BannerImageOWSURLH}}&quot; alt=&quot;{{BannerImageAltTextOWSTEXT}}&quot; /&gt; 
        ///	&lt;div class=&quot;ia-banner-content&quot;&gt;
        ///		&lt;h1 class=&quot;ia-banner-title&quot;&gt;{{BannerHeadingOWSTEXT}}&lt;/h1&gt;
        ///		&lt;h2 class=&quot;ia-banner-subtitle&quot;&gt;{{BannerSubHeadingOWSTEXT}}&lt;/h2&gt;
        ///		&lt;a href=&quot;{{BannerLinkUrlOWSURLH}}&quot; class=&quot;ia-button {{LinkButtonTheme}}&quot; title=&quot;{{BannerLinkHoverTextOWSTEXT}}&quot; target=&quot;{ [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string ItemTemplate {
            get {
                return ResourceManager.GetString("ItemTemplate", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to /scripts/vendor/jquery-2.1.3.min.js|/scripts/vendor/modernizr.js|/scripts/components/ia-transformer-tabs.js|/scripts/vendor/tablesaw.stackonly.js.
        /// </summary>
        internal static string pdl_JSFiles {
            get {
                return ResourceManager.GetString("pdl_JSFiles", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to /css/interaction-controls.css.
        /// </summary>
        internal static string pdl_StyleSheets {
            get {
                return ResourceManager.GetString("pdl_StyleSheets", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to test.
        /// </summary>
        internal static string String1 {
            get {
                return ResourceManager.GetString("String1", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to /_layouts/15/.
        /// </summary>
        internal static string val_ScriptBase {
            get {
                return ResourceManager.GetString("val_ScriptBase", resourceCulture);
            }
        }
    }
}
