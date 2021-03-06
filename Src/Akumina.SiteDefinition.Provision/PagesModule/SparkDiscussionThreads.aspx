﻿<%--<%@ Register TagPrefix="WpNs0" Namespace="Akumina.WebParts.DiscussionBoard.DiscussionThreadPage" Assembly="Akumina.WebParts.DiscussionBoard, Version=1.0.0.0, Culture=neutral, PublicKeyToken=fee3f2d978bf0486"%>--%>
<%-- _lcid="1033" _version="15.0.4420" _dal="1" --%>
<%-- _LocalBinding --%>
<%@ Page language="C#" MasterPageFile="../_catalogs/masterpage/AkuminaSpark/akuminaspark.master"    Inherits="Microsoft.SharePoint.WebPartPages.WebPartPage,Microsoft.SharePoint,Version=15.0.0.0,Culture=neutral,PublicKeyToken=71e9bce111e9429c" meta:progid="SharePoint.WebPartPage.Document" meta:webpartpageexpansion="full"  %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> <%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> <%@ Import Namespace="Microsoft.SharePoint" %> <%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> <%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<asp:Content ID="Content1" ContentPlaceHolderId="PlaceHolderPageTitle" runat="server">
	<SharePoint:ListItemProperty ID="ListItemProperty1" Property="BaseName" maxlength="40" runat="server"/>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderId="PlaceHolderAdditionalPageHead" runat="server">
	<meta name="GENERATOR" content="Microsoft SharePoint" />
	<meta name="ProgId" content="SharePoint.WebPartPage.Document" />
	<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
	<meta name="CollaborationServer" content="SharePoint Team Web Site" />
	<SharePoint:ScriptBlock ID="ScriptBlock1" runat="server">
	var navBarHelpOverrideKey ="WSSEndUser";
	</SharePoint:ScriptBlock>
<SharePoint:StyleBlock ID="StyleBlock1" runat="server">
body #s4-leftpanel {
	display:none;
}
.s4-ca {
	margin-left:0px;
}
</SharePoint:StyleBlock>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderId="PlaceHolderSearchArea" runat="server">
	<SharePoint:DelegateControl ID="DelegateControl1" runat="server"
		ControlId="SmallSearchInputBox"/>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderId="PlaceHolderPageDescription" runat="server">
	<SharePoint:ProjectProperty ID="ProjectProperty1" Property="Description" runat="server"/>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderId="PlaceHolderMain" runat="server">
	<div class="ms-hide">
	<WebPartPages:WebPartZone runat="server" title="loc:TitleBar" id="TitleBar" AllowLayoutChange="false" AllowPersonalization="false" Style="display:none;"><ZoneTemplate>
	<WebPartPages:TitleBarWebPart runat="server" HeaderTitle="SparkDiscussionThread" Title="Web Part Page Title Bar" FrameType="None" SuppressWebPartChrome="False" Description="" IsIncluded="True" ZoneID="TitleBar" PartOrder="2" FrameState="Normal" AllowRemove="False" AllowZoneChange="True" AllowMinimize="False" AllowConnect="True" AllowEdit="True" AllowHide="True" IsVisible="True" DetailLink="" HelpLink="" HelpMode="Modeless" Dir="Default" PartImageSmall="" MissingAssembly="Cannot import this Web Part." PartImageLarge="" IsIncludedFilter="" ExportControlledProperties="True" ConnectionID="00000000-0000-0000-0000-000000000000" ID="g_66dc9d8f_d42b_443b_9b21_020891585bdd" AllowClose="False" ChromeType="None" ExportMode="All" __MarkupType="vsattributemarkup" __WebPartId="{66DC9D8F-D42B-443B-9B21-020891585BDD}" WebPart="true" Height="" Width=""></WebPartPages:TitleBarWebPart>

	</ZoneTemplate></WebPartPages:WebPartZone>
  </div>
   <div class="contentBody">
            <div class="subsite-menu-wrapper">
                <div class="subsite-menu">
                    <span class="fa fa-times subsite-menu-close"></span>
                    <ul>
                        <li><a href="#">Home</a></li>
                        <li><a href="#">Documents</a></li>
                        <li><a href="#">Discussion Board</a></li>
                        <li><a href="#">Calendar</a></li>
                        <li>
                            <a href="#">Marketing Events</a>
                            <ul>
                                <li><a href="#">2015 Event Page</a></li>
                                <li><a href="#">2014 Event Page</a></li>
                            </ul>
                        </li>
                        <li><a href="#">Site Administration</a></li>
                    </ul>
                </div>
            </div>

            <div class="row">

                <div class="large-12 medium-12 columns">

                  <WebPartPages:WebPartZone runat="server" Title="loc:FullPage" ID="FullPage" FrameType="TitleBarOnly"><ZoneTemplate>
					<%--<WpNs0:DiscussionThreadPage runat="server" DisplayAvatarPicture="True" DiscussionListName="AkuminaDiscussions" DocumentsListName="AkuminaDocuments" DiscussionListPageUrl="http://akuminademo.cloudapp.net/SitePages/SparkDiscussions.aspx" DiscussionCreatePageUrl="http://akuminademo.cloudapp.net/SitePages/SparkNewDiscussion.aspx" DiscussionThreadPageUrl="http://akuminademo.cloudapp.net/SitePages/SparkDiscussionThreads.aspx" CatalogIconImageUrl="_layouts/15/Images/Akumina.WebParts.DiscussionBoard/ia-discussion-thread.png" ChromeType="None" Description="Discussion Board - Discussion Thread Web Part" Title="Discussion Board - Discussion Thread" TitleIconImageUrl="_layouts/15/Images/Akumina.WebParts.DiscussionBoard/ia-discussion-thread.png" ID="g_2123fb17_4445_47c6_92fc_c03ea7392cc5" __MarkupType="vsattributemarkup" __WebPartId="{2123FB17-4445-47C6-92FC-C03EA7392CC5}" WebPart="true" __designer:IsClosed="false" partorder="2"></WpNs0:DiscussionThreadPage>--%>

					</ZoneTemplate></WebPartPages:WebPartZone>
                </div>
            </div><!--row-->

        </div>

 
				<SharePoint:ScriptBlock ID="ScriptBlock2" runat="server">
				if(typeof(MSOLayout_MakeInvisibleIfEmpty) == &quot;function&quot;) 
				
				
				{MSOLayout_MakeInvisibleIfEmpty();}</SharePoint:ScriptBlock>

</asp:Content>
