<%@ Page language="C#" Inherits="Microsoft.SharePoint.Publishing.PublishingLayoutPage,
Microsoft.SharePoint.Publishing,Version=15.0.0.0,Culture=neutral,PublicKeyToken=71e9bce111e9429c" %>
 <%@ Register Tagprefix="SharePointWebControls" Namespace="Microsoft.SharePoint.WebControls"
 Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
 <%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages"
 Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
 <%@ Register Tagprefix="PublishingWebControls" Namespace="Microsoft.SharePoint.Publishing.WebControls"
 Assembly="Microsoft.SharePoint.Publishing, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
 <%@ Register Tagprefix="PublishingNavigation" Namespace="Microsoft.SharePoint.Publishing.Navigation"
 Assembly="Microsoft.SharePoint.Publishing, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

 <asp:Content ID="Content1" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
     <SharePointWebControls:FieldValue id="FieldValue1" FieldName="Title" runat="server"/>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">
    <meta name="GENERATOR" content="Microsoft SharePoint" />
    <meta name="ProgId" content="SharePoint.WebPartPage.Document" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="CollaborationServer" content="SharePoint Team Web Site" />
    <SharePointWebControls:ScriptBlock ID="Scriptblock1" runat="server">
        var navBarHelpOverrideKey = "WSSEndUser";
    </SharePointWebControls:ScriptBlock>
    <SharePointWebControls:StyleBlock ID="Styleblock1" runat="server">
        body #s4-leftpanel {
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; display:none;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; }
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; .s4-ca {
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; margin-left:0px;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; }
    </SharePointWebControls:StyleBlock>
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
                                   
                                </ZoneTemplate>
                            </WebPartPages:WebPartZone>
                        </div>
                        <div class="large-4 medium-12 small-12 columns">
                            <div class="welcome-message">
                                <div class="welcome-content">
                                    <WebPartPages:WebPartZone runat="server" Title="loc:WelcomeContent" ID="WelcomeContent" FrameType="TitleBarOnly">
                                        <ZoneTemplate>
                                           
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
                                       
                                    </ZoneTemplate>
                                </WebPartPages:WebPartZone>
                            </div>
                            
                        </div>
                        <div class="large-6 medium-12 small-12 columns">
                            <div class="strong-heading-block">
                                <h2 class="strong-heading">Documents</h2>
                                <WebPartPages:WebPartZone runat="server" Title="loc:DocumentsSummmary" ID="DocumentsSummmary" FrameType="TitleBarOnly">
                                    <ZoneTemplate>
                                        
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
                                        
                                    </ZoneTemplate>
                                </WebPartPages:WebPartZone>
                            </div>
                            <div class="large-12 medium-7 small-12 columns">
                                <WebPartPages:WebPartZone runat="server" Title="loc:DynamicControls" ID="DynamicControls" FrameType="TitleBarOnly">
                                    <ZoneTemplate>
                            
                                    </ZoneTemplate>
                                </WebPartPages:WebPartZone>
                            </div>
                        </div>
                       
                    </div>
                    
                </div>

            </div>
            <!--row-->
        </div>

    </div>


    <SharePointWebControls:ScriptBlock ID="Scriptblock2" runat="server">
        if(typeof(MSOLayout_MakeInvisibleIfEmpty) =="function")
{MSOLayout_MakeInvisibleIfEmpty();}
    </SharePointWebControls:ScriptBlock>

</asp:Content>
