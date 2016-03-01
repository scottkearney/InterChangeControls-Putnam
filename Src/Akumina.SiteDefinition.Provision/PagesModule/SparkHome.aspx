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
    <div class="site-alert-wrapper">
        <div class="row">
            <div class="large-12 columns columns-no-padding">

                <WebPartPages:WebPartZone runat="server" Title="loc:ContentAlert" ID="ContentAlert" FrameType="TitleBarOnly">
                    <ZoneTemplate>
                        <%--<WpNs4:ContentBlock runat="server" CatalogIconImageUrl="../Akumina.WebParts.ContentBlock/icon/ia-content-block.png" ChromeType="None" Description="Content Block Web Part" Title="Content Block" TitleIconImageUrl="../Akumina.WebParts.ContentBlock/icon/ia-content-block.png" ID="g_5b223b57_9e7c_4700_ac1f_a3d22962dd10" __MarkupType="vsattributemarkup" __WebPartId="{5B223B57-9E7C-4700-AC1F-A3D22962DD10}" WebPart="true" __designer:IsClosed="false" partorder="2"></WpNs4:ContentBlock>--%>
                    </ZoneTemplate>
                </WebPartPages:WebPartZone>
            </div>
        </div>

    </div>
    <div class="home-layout">
        <div class="contentBody clearfix">
            <div class="row">
                <div class="large-10 medium-12 small-12 large-push-2 columns">
                    <div class="row">
                        <div class="large-8 medium-12 small-12 columns">
                            <WebPartPages:WebPartZone runat="server" Title="loc:BannerItem" ID="BannerItem" FrameType="TitleBarOnly">
                                <ZoneTemplate>
                                    <%--<WpNs0:Banner runat="server" ResultSourceId="" MaxSlideCount="4" InfiniteLoop="True" AutoPlay="True" ShowNavigator="True" TransitionEffect="Fade" SlideDuration="5000" TextColorTheme="" TextLocation="" TextAlignment="" H1MaxCharacters="0" H2MaxCharacters="0" ButtonMaxCharacters="0" WebPartTheme="" LinkButtonTheme="" LinkButtonTextTheme="" InstructionSet="AkuminaBannerIDS.HomePage" RootResourcePath="http://akuminademo.cloudapp.net/_layouts/15/Akumina.WebParts/" CatalogIconImageUrl="../Akumina.WebParts.Banner/icon/ia-banner.png" ChromeType="None" Description="Banner Web Part" Title="Banner" TitleIconImageUrl="../Akumina.WebParts.Banner/icon/ia-banner.png" ID="g_dc165e46_1d0f_43a5_bb94_2061d4c60074" __MarkupType="vsattributemarkup" __WebPartId="{DC165E46-1D0F-43A5-BB94-2061D4C60074}" WebPart="true" __designer:IsClosed="false" partorder="2"></WpNs0:Banner>--%>
                                </ZoneTemplate>
                            </WebPartPages:WebPartZone>
                        </div>
                        <div class="large-4 medium-12 small-12 columns">
                            <div class="welcome-message">
                                <div class="welcome-content">
                                    <WebPartPages:WebPartZone runat="server" Title="loc:WelcomeContent" ID="WelcomeContent" FrameType="TitleBarOnly">
                                        <ZoneTemplate>
                                            <%--<WpNs4:ContentBlock runat="server"  CatalogIconImageUrl="../Akumina.WebParts.ContentBlock/icon/ia-content-block.png" ChromeType="None" Description="Content Block Web Part" Title="Content Block" TitleIconImageUrl="../Akumina.WebParts.ContentBlock/icon/ia-content-block.png" ID="g_ee0b7c90_28e1_4e39_870d_97f6aa59f098" __MarkupType="vsattributemarkup" __WebPartId="{EE0B7C90-28E1-4E39-870D-97F6AA59F098}" WebPart="true" __designer:IsClosed="false" partorder="2"></WpNs4:ContentBlock>--%>
                                        </ZoneTemplate>
                                    </WebPartPages:WebPartZone>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="large-12 medium-12 small-12 columns">

                            <div class="strong-heading-block">

                                <h2 class="strong-heading">Announcements</h2>


                                <WebPartPages:WebPartZone runat="server" Title="loc:Announcements" ID="Announcements" FrameType="TitleBarOnly">
                                    <ZoneTemplate>
                                        <%--<WpNs5:AnnouncementItems runat="server" ChromeType="None" Description="Announcement Items Web Part" ID="g_d45a8b49_0b47_45b0_9e50_1f155ab15e48" __MarkupType="vsattributemarkup" __WebPartId="{D45A8B49-0B47-45B0-9E50-1F155AB15E48}" WebPart="true" __designer:IsClosed="false" partorder="2"></WpNs5:AnnouncementItems>--%>
                                    </ZoneTemplate>
                                </WebPartPages:WebPartZone>
                            </div>
                        </div>
                    </div>

                    <div class="row">
                        <div class="large-6 medium-12 small-12 columns">
                            <div class="strong-heading-block">
                                <h2 class="strong-heading">Sites</h2>
                                <WebPartPages:WebPartZone runat="server" Title="loc:Department" ID="Department" FrameType="TitleBarOnly">
                                    <ZoneTemplate>
                                        <%--<WpNs6:SiteSummaryList runat="server" ChromeType="None" Description="Site Summary List Web Part" ID="g_caffa36d_88c8_4ae0_9b4d_3eac2d71b8e4" __MarkupType="vsattributemarkup" __WebPartId="{CAFFA36D-88C8-4AE0-9B4D-3EAC2D71B8E4}" WebPart="true" __designer:IsClosed="false" partorder="2"></WpNs6:SiteSummaryList>--%>
                                    </ZoneTemplate>
                                </WebPartPages:WebPartZone>
                            </div>
                            <!-- end home-heading-block-->
                        </div>
                        <div class="large-6 medium-12 small-12 columns">
                            <div class="strong-heading-block">
                                <h2 class="strong-heading">Documents</h2>
                                <WebPartPages:WebPartZone runat="server" Title="loc:DocumentsSummmary" ID="DocumentsSummmary" FrameType="TitleBarOnly">
                                    <ZoneTemplate>
                                        <%--<WpNs3:DocumentSummaryList runat="server" TargetDocumentLibrary="" ID="g_dd29770d_fe80_43a7_a6dd_b9218f883277" __MarkupType="vsattributemarkup" __WebPartId="{DD29770D-FE80-43A7-A6DD-B9218F883277}" WebPart="true" __designer:IsClosed="false" partorder="2"></WpNs3:DocumentSummaryList>--%>
                                    </ZoneTemplate>
                                </WebPartPages:WebPartZone>
                            </div>
                        </div>

                    </div>
                </div>
                <div class="large-2 medium-12 small-12 large-pull-10 columns">
                    <div class="home-left-col">
                        <div class="row">
                            <div class="large-12 medium-5 small-12 columns">
                                <WebPartPages:WebPartZone runat="server" Title="loc:QuickLinks" ID="QuickLinks" FrameType="TitleBarOnly">
                                    <ZoneTemplate>
                                        <%--<WpNs7:SiteLinks runat="server" CatalogIconImageUrl="../Akumina.WebParts.SiteLinks/icon/ia-sitelinks.png" ChromeType="None" Description="Site Links Web Part" Title="Site Links" TitleIconImageUrl="../Akumina.WebParts.SiteLinks/icon/ia-sitelinks.png" ID="g_78ce03ee_83b0_4ae3_9c37_05b894eee11d" __MarkupType="vsattributemarkup" __WebPartId="{78CE03EE-83B0-4AE3-9C37-05B894EEE11D}" WebPart="true" __designer:IsClosed="false" partorder="2"></WpNs7:SiteLinks>--%>
                                    </ZoneTemplate>
                                </WebPartPages:WebPartZone>
                            </div>
                            <div class="large-12 medium-7 small-12 columns">
                                <WebPartPages:WebPartZone runat="server" Title="loc:DynamicControls" ID="DynamicControls" FrameType="TitleBarOnly">
                                    <ZoneTemplate>
                                        <%--<WpNs1:PlaceHolder runat="server"  ChromeType="None" Description="Miscellaneous - PlaceHolder WebPart" Title="Miscellaneous - PlaceHolder" ID="g_ec9b6bc5_ebe2_4b19_b74f_b34366b77090" __MarkupType="vsattributemarkup" __WebPartId="{EC9B6BC5-EBE2-4B19-B74F-B34366B77090}" WebPart="true" __designer:IsClosed="false" partorder="2"></WpNs1:PlaceHolder>--%>
                                    </ZoneTemplate>
                                </WebPartPages:WebPartZone>
                            </div>
                        </div>
                        <!--row-->
                    </div>
                    <!-- home-left-col -->
                </div>

            </div>
            <!--row-->
        </div>
        <!-- Content Body -->
    </div>
    <!-- Home Layout -->

    <%--<SharePoint:ScriptBlock ID="Scriptblock2" runat="server">
        if(typeof(MSOLayout_MakeInvisibleIfEmpty) == &quot;function&quot;)

&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; {MSOLayout_MakeInvisibleIfEmpty();}
    </SharePoint:ScriptBlock>--%>

    <SharePoint:ScriptBlock ID="Scriptblock2" runat="server">
        if(typeof(MSOLayout_MakeInvisibleIfEmpty) =="function")
{MSOLayout_MakeInvisibleIfEmpty();}
    </SharePoint:ScriptBlock>

</asp:Content>
