<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LibraryListing.ascx.cs" Inherits="Akumina.WebParts.LibraryListing.LibraryListing.LibraryListing" %>

<asp:Literal ID="litTop" runat="server"></asp:Literal>

<asp:HiddenField ID="ImagesLibraryValue" ClientIDMode="Static" runat="server"></asp:HiddenField>
<asp:HiddenField ID="webTitleValue" ClientIDMode="Static" runat="server"></asp:HiddenField>
<asp:HiddenField ID="listingsValue" ClientIDMode="Static" runat="server"></asp:HiddenField>
<asp:HiddenField ID="DMSLandingPageValue" ClientIDMode="Static" runat="server"></asp:HiddenField>
<asp:HiddenField ID="SearchRedirectURLValue" ClientIDMode="Static" runat="server" />
<asp:HiddenField ID="RedirectionOptionValue" ClientIDMode="Static" runat="server" />


<div class="interAction">

    <div class="ia-search-library">
        <span class="ia-search-icon fa fa-search"></span>
        <input id="txtDocSearch" class="ia-search-library-box" title="type &quot;a&quot;" placeholder="Search" type="text">


        <!-- Current Site Dropdown -->
        <div class="ia-search-library-site-list">
            <span class="ia-search-library-site-name">
                <span class="ia-search-library-site-name-field">All Libraries</span>
                <span class="ia-search-library-site-list-icon fa fa-caret-down"></span>
            </span>

            <div style="display: none;" class="ia-search-library-site-list-dropdown">
                <ul>
                </ul>
            </div>
        </div>
        <button id="ButtonSearchDoc" class="ia-search-library-btn" onclick="SearchLib(); return false;">Go</button>
    </div>

</div>
<!-- end .interAction -->


<div class="interAction">

    <section class="ia-library-list">
        <p>Click on an icon to access the selected library</p>

        <ul class="ia-library-list-grid">
        </ul>

    </section>

</div>
<!-- end .interAction -->
