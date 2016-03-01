<%--<%@ Register TagPrefix="WpNs1" Namespace="Akumina.WebParts.Announcement.AnnouncementItems" Assembly="Akumina.WebParts.Announcement, Version=1.0.0.0, Culture=neutral, PublicKeyToken=4450a92cb425f02a"%>
<%@ Register TagPrefix="WpNs0" Namespace="Akumina.WebParts.Announcement.AnnouncementDetail" Assembly="Akumina.WebParts.Announcement, Version=1.0.0.0, Culture=neutral, PublicKeyToken=4450a92cb425f02a"%>--%>
<%-- _lcid="1033" _version="15.0.4420" _dal="1" --%>
<%-- _LocalBinding --%>
<%@ Page language="C#" MasterPageFile="../_catalogs/masterpage/AkuminaSpark/akuminaspark.master"    Inherits="Microsoft.SharePoint.WebPartPages.WebPartPage,Microsoft.SharePoint,Version=15.0.0.0,Culture=neutral,PublicKeyToken=71e9bce111e9429c" meta:progid="SharePoint.WebPartPage.Document" meta:webpartpageexpansion="full"  %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> <%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> <%@ Import Namespace="Microsoft.SharePoint" %> <%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> <%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<asp:Content ContentPlaceHolderId="PlaceHolderPageTitle" runat="server">
	<SharePoint:ListItemProperty Property="BaseName" maxlength="40" runat="server"/>
</asp:Content>
<asp:Content ContentPlaceHolderId="PlaceHolderAdditionalPageHead" runat="server">
	<meta name="GENERATOR" content="Microsoft SharePoint" />
	<meta name="ProgId" content="SharePoint.WebPartPage.Document" />
	<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
	<meta name="CollaborationServer" content="SharePoint Team Web Site" />
	<SharePoint:ScriptBlock runat="server">
	var navBarHelpOverrideKey = "WSSEndUser";
	</SharePoint:ScriptBlock>
<SharePoint:StyleBlock runat="server">
body #s4-leftpanel {
	display:none;
}
.s4-ca {
	margin-left:0px;
}
</SharePoint:StyleBlock>
</asp:Content>
<asp:Content ContentPlaceHolderId="PlaceHolderSearchArea" runat="server">
	<SharePoint:DelegateControl runat="server"
		ControlId="SmallSearchInputBox"/>
</asp:Content>
<asp:Content ContentPlaceHolderId="PlaceHolderPageDescription" runat="server">
	<SharePoint:ProjectProperty Property="Description" runat="server"/>
</asp:Content>
<asp:Content ContentPlaceHolderId="PlaceHolderMain" runat="server">
	<div class="ms-hide">
	<WebPartPages:WebPartZone runat="server" title="loc:TitleBar" id="TitleBar" AllowLayoutChange="false" AllowPersonalization="false" Style="display:none;"><ZoneTemplate>
	<WebPartPages:TitleBarWebPart runat="server" HeaderTitle="Untitled_1" Title="Web Part Page Title Bar" FrameType="None" SuppressWebPartChrome="False" Description="" IsIncluded="True" ZoneID="TitleBar" PartOrder="2" FrameState="Normal" AllowRemove="False" AllowZoneChange="True" AllowMinimize="False" AllowConnect="True" AllowEdit="True" AllowHide="True" IsVisible="True" DetailLink="" HelpLink="" HelpMode="Modeless" Dir="Default" PartImageSmall="" MissingAssembly="Cannot import this Web Part." PartImageLarge="" IsIncludedFilter="" ExportControlledProperties="True" ConnectionID="00000000-0000-0000-0000-000000000000" ID="g_fa9d1d9a_2597_4e2e_9c95_fa09aadfe5f7" AllowClose="False" ChromeType="None" ExportMode="All" __MarkupType="vsattributemarkup" __WebPartId="{FA9D1D9A-2597-4E2E-9C95-FA09AADFE5F7}" WebPart="true" Height="" Width=""></WebPartPages:TitleBarWebPart>

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
						<li><a href="#">Marketing Events</a>
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

				<div class="large-9 medium-9 small-12 columns">
						<WebPartPages:WebPartZone runat="server" Title="loc:FullAnnouncementDetail" ID="FullAnnouncementDetail" FrameType="TitleBarOnly"><ZoneTemplate>
						<%--<WpNs0:AnnouncementDetail runat="server" ListName="AkuminaAnnouncements" RootResourcePath="http://akuminademo.cloudapp.net/_layouts/15/Akumina.WebParts.Announcement" ChromeType="None" Description="Announcement Detail Web Part" Title="Announcement Detail" ID="g_4204c25f_859f_4548_8060_c53fbe6f0189" __MarkupType="vsattributemarkup" __WebPartId="{4204C25F-859F-4548-8060-C53FBE6F0189}" WebPart="true" __designer:IsClosed="false" partorder="2"></WpNs0:AnnouncementDetail>--%>

						</ZoneTemplate></WebPartPages:WebPartZone>

				</div>

				<div class="large-3 medium-3 small-12 columns">
				
					<WebPartPages:WebPartZone runat="server" Title="loc:FullAnnouncements" ID="FullAnnouncements" FrameType="TitleBarOnly"><ZoneTemplate>
					<%--<WpNs1:AnnouncementItems runat="server" ListName="AkuminaAnnouncements" RootResourcePath="http://akuminademo.cloudapp.net/_layouts/15/Akumina.WebParts.Announcement" ChromeType="None" Description="Announcement Items Web Part" Title="Announcements" ID="g_10f46b6c_4221_4e27_a18f_f506dc9bf5f6" __MarkupType="vsattributemarkup" __WebPartId="{10F46B6C-4221-4E27-A18F-F506DC9BF5F6}" WebPart="true" __designer:IsClosed="false" partorder="2"></WpNs1:AnnouncementItems>--%>

					</ZoneTemplate></WebPartPages:WebPartZone>
				</div>

			</div><!--row-->


		</div>
					
				<SharePoint:ScriptBlock runat="server">
				if(typeof(MSOLayout_MakeInvisibleIfEmpty) == &quot;function&quot;) 
				
				{MSOLayout_MakeInvisibleIfEmpty();}</SharePoint:ScriptBlock>

</asp:Content>
