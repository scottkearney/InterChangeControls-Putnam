<%--<%@ Register TagPrefix="WpNs0" Namespace="Akumina.WebParts.DiscussionBoard.DiscussionBoardListing" Assembly="Akumina.WebParts.DiscussionBoard, Version=1.0.0.0, Culture=neutral, PublicKeyToken=fee3f2d978bf0486"%>--%>
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
	var navBarHelpOverrideKey = "WSSEndUser";
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
	<WebPartPages:TitleBarWebPart runat="server" HeaderTitle="SparkDiscussionListing" Title="Web Part Page Title Bar" FrameType="None" SuppressWebPartChrome="False" Description="" IsIncluded="True" ZoneID="TitleBar" PartOrder="2" FrameState="Normal" AllowRemove="False" AllowZoneChange="True" AllowMinimize="False" AllowConnect="True" AllowEdit="True" AllowHide="True" IsVisible="True" DetailLink="" HelpLink="" HelpMode="Modeless" Dir="Default" PartImageSmall="" MissingAssembly="Cannot import this Web Part." PartImageLarge="" IsIncludedFilter="" ExportControlledProperties="True" ConnectionID="00000000-0000-0000-0000-000000000000" ID="g_3f000ee4_edc8_4989_985b_f19dad7206bc" AllowClose="False" ChromeType="None" ExportMode="All" __MarkupType="vsattributemarkup" __WebPartId="{3F000EE4-EDC8-4989-985B-F19DAD7206BC}" WebPart="true" Height="" Width=""></WebPartPages:TitleBarWebPart>

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
					<%--<WpNs0:DiscussionBoardListing runat="server" CatalogIconImageUrl="_layouts/15/Images/Akumina.WebParts.DiscussionBoard/ia-discussion-listing.png" ChromeType="None" Description="Discussion Board - Discussion Listing Web Part" Title="Discussion Board - Discussion Listing" TitleIconImageUrl="_layouts/15/Images/Akumina.WebParts.DiscussionBoard/ia-discussion-listing.png" ID="g_17a92403_65aa_4ac2_bd96_f6c699b55591" __MarkupType="vsattributemarkup" __WebPartId="{17A92403-65AA-4AC2-BD96-F6C699B55591}" WebPart="true" __designer:IsClosed="false" partorder="2"></WpNs0:DiscussionBoardListing>--%>

					</ZoneTemplate></WebPartPages:WebPartZone>

                </div>
            </div><!--row-->

        </div>
				<SharePoint:ScriptBlock ID="ScriptBlock2" runat="server">
				if(typeof(MSOLayout_MakeInvisibleIfEmpty) == &quot;function&quot;) 
				
				
				{MSOLayout_MakeInvisibleIfEmpty();}</SharePoint:ScriptBlock>
</asp:Content>
