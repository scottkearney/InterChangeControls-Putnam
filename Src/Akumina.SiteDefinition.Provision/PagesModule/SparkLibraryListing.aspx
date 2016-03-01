<%--<%@ Register TagPrefix="WpNs8" Namespace="Akumina.WebParts.Miscellaneous.Map" Assembly="Akumina.WebParts.Miscellaneous, Version=1.0.0.0, Culture=neutral, PublicKeyToken=e49efafec81135b4"%>
<%@ Register TagPrefix="WpNs7" Namespace="Akumina.WebParts.SiteLinks.SiteLinks" Assembly="Akumina.WebParts.SiteLinks, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f82d8f89e76ed671"%>
<%@ Register TagPrefix="WpNs6" Namespace="Akumina.WebParts.SiteSummaryList.SiteSummaryList" Assembly="Akumina.WebParts.SiteSummaryList, Version=1.0.0.0, Culture=neutral, PublicKeyToken=1c6494ded3d64ff2"%>
<%@ Register TagPrefix="WpNs5" Namespace="Akumina.WebParts.Announcement.AnnouncementItems" Assembly="Akumina.WebParts.Announcement, Version=1.0.0.0, Culture=neutral, PublicKeyToken=4450a92cb425f02a"%>
<%@ Register TagPrefix="WpNs4" Namespace="Akumina.WebParts.ContentBlock.ContentBlock" Assembly="Akumina.WebParts.ContentBlock, Version=1.0.0.0, Culture=neutral, PublicKeyToken=93d8bbc0806dc6c4"%>
<%@ Register TagPrefix="WpNs3" Namespace="Akumina.WebParts.DocumentSummaryList.DocumentSummaryList" Assembly="Akumina.WebParts.DocumentSummaryList, Version=1.0.0.0, Culture=neutral, PublicKeyToken=1302f10db64f177c"%>
<%@ Register TagPrefix="WpNs2" Namespace="Akumina.WebParts.ContentBlock.ContentBlock" Assembly="Akumina.WebParts.ContentBlock, Version=1.0.0.0, Culture=neutral, PublicKeyToken=93d8bbc0806dc6c4"%>
<%@ Register TagPrefix="WpNs1" Namespace="Akumina.WebParts.Miscellaneous.PlaceHolder" Assembly="Akumina.WebParts.Miscellaneous, Version=1.0.0.0, Culture=neutral, PublicKeyToken=e49efafec81135b4"%>
<%@ Register TagPrefix="WpNs0" Namespace="Akumina.WebParts.Banner.Banner" Assembly="Akumina.WebParts.Banner, Version=1.0.0.0, Culture=neutral, PublicKeyToken=26af758ad9c19cff"%>--%>
<%-- _lcid="1033" _version="15.0.4420" _dal="1" --%>
<%-- _LocalBinding --%>

<%@ Page Language="C#" MasterPageFile="../_catalogs/masterpage/AkuminaSpark/akuminaspark.master" Inherits="Microsoft.SharePoint.WebPartPages.WebPartPage,Microsoft.SharePoint,Version=15.0.0.0,Culture=neutral,PublicKeyToken=71e9bce111e9429c" meta:progid="SharePoint.WebPartPage.Document" meta:webpartpageexpansion="full" %>

<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
    <SharePoint:ListItemProperty ID="Listitemproperty1" Property="BaseName" MaxLength="40" runat="server" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">
    <meta name="GENERATOR" content="Microsoft SharePoint" />
    <meta name="ProgId" content="SharePoint.WebPartPage.Document" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="CollaborationServer" content="SharePoint Team Web Site" />
    <SharePoint:ScriptBlock ID="Scriptblock1" runat="server">
        var navBarHelpOverrideKey = "WSSEndUser";
    </SharePoint:ScriptBlock>
    <SharePoint:StyleBlock ID="Styleblock1" runat="server">
        body #s4-leftpanel {
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; display:none;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; }
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; .s4-ca {
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; margin-left:0px;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; }
    </SharePoint:StyleBlock>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PlaceHolderSearchArea" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="PlaceHolderPageDescription" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="PlaceHolderMain" runat="server">
    <div class="ms-hide">
        <WebPartPages:WebPartZone runat="server" Title="loc:TitleBar" ID="TitleBar" AllowLayoutChange="false" AllowPersonalization="false" Style="display: none;">
            <ZoneTemplate>
                <WebPartPages:TitleBarWebPart runat="server" HeaderTitle="Untitled_1" Title="Web Part Page Title Bar" FrameType="None" SuppressWebPartChrome="False" Description="" IsIncluded="True" ZoneID="TitleBar" PartOrder="2" FrameState="Normal" AllowRemove="False" AllowZoneChange="True" AllowMinimize="False" AllowConnect="True" AllowEdit="True" AllowHide="True" IsVisible="True" DetailLink="" HelpLink="" HelpMode="Modeless" Dir="Default" PartImageSmall="" MissingAssembly="Cannot import this Web Part." PartImageLarge="" IsIncludedFilter="" ExportControlledProperties="True" ConnectionID="00000000-0000-0000-0000-000000000000" ID="g_deb89284_bf92_42cd_933e_537095c9429a" AllowClose="False" ChromeType="None" ExportMode="All" __MarkupType="vsattributemarkup" __WebPartId="{DEB89284-BF92-42CD-933E-537095C9429A}" WebPart="true" Height="" Width=""></WebPartPages:TitleBarWebPart>

            </ZoneTemplate>
        </WebPartPages:WebPartZone>
    </div>
    <%--    <div class="site-alert-wrapper">
        <div class="row">
          
        </div>

    </div>--%>

        <div class="contentBody clearfix">
            <div class="row">

                <div class="large-12 medium-12 columns">

                    <WebPartPages:WebPartZone runat="server" Title="loc:LibraryListing" ID="LibraryListing" FrameType="TitleBarOnly">
                        <ZoneTemplate>
                            <%--<WpNs4:ContentBlock runat="server" CatalogIconImageUrl="../Akumina.WebParts.ContentBlock/icon/ia-content-block.png" ChromeType="None" Description="Content Block Web Part" Title="Content Block" TitleIconImageUrl="../Akumina.WebParts.ContentBlock/icon/ia-content-block.png" ID="g_5b223b57_9e7c_4700_ac1f_a3d22962dd10" __MarkupType="vsattributemarkup" __WebPartId="{5B223B57-9E7C-4700-AC1F-A3D22962DD10}" WebPart="true" __designer:IsClosed="false" partorder="2"></WpNs4:ContentBlock>--%>
                        </ZoneTemplate>
                    </WebPartPages:WebPartZone>
                </div>
            </div>
            <!--row-->
        </div>
        <!-- Content Body -->
  

    <%--<SharePoint:ScriptBlock ID="Scriptblock2" runat="server">
        if(typeof(MSOLayout_MakeInvisibleIfEmpty) == &quot;function&quot;)

&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; {MSOLayout_MakeInvisibleIfEmpty();}
    </SharePoint:ScriptBlock>--%>

    <SharePoint:ScriptBlock ID="Scriptblock2" runat="server">
        if(typeof(MSOLayout_MakeInvisibleIfEmpty) =="function")
{MSOLayout_MakeInvisibleIfEmpty();}
    </SharePoint:ScriptBlock>

</asp:Content>
