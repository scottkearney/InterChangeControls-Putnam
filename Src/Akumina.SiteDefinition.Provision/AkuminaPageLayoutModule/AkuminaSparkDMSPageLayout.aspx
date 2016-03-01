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

 <asp:Content ContentPlaceholderID="PlaceHolderPageTitle" runat="server">
 <SharePointWebControls:FieldValue id="FieldValue1" FieldName="Title" runat="server"/>
 </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">
    <meta name="GENERATOR" content="Microsoft SharePoint" />
    <meta name="ProgId" content="SharePoint.WebPartPage.Document" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="CollaborationServer" content="SharePoint Team Web Site" />
    <SharePointWebControls:ScriptBlock ID="Scriptblock1" runat="server">
        var navBarHelpOverrideKey ="WSSEndUser";
    </SharePointWebControls:ScriptBlock>
    <SharePointWebControls:StyleBlock ID="Styleblock1" runat="server">
body #s4-leftpanel {display:none; }
.s4-ca {margin-left:0px; }
    </SharePointWebControls:StyleBlock>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PlaceHolderSearchArea" runat="server">
    <SharePointWebControls:DelegateControl ID="Delegatecontrol1" runat="server"
        ControlId="SmallSearchInputBox" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="PlaceHolderPageDescription" runat="server">
    <SharePointWebControls:ProjectProperty ID="Projectproperty1" Property="Description" runat="server" />
</asp:Content>
 <asp:Content ContentPlaceholderID="PlaceHolderMain" runat="server">
         <div class="ms-hide">
        <WebPartPages:SPProxyWebPartManager runat="server" ID="__ProxyWebPartManagerForConnections__">
            <SPWebPartConnections>
            </SPWebPartConnections>
        </WebPartPages:SPProxyWebPartManager>
        <WebPartPages:WebPartZone runat="server" Title="loc:TitleBar" ID="TitleBar" AllowLayoutChange="false" AllowPersonalization="false" Style="display: none;">
            <ZoneTemplate></ZoneTemplate>
        </WebPartPages:WebPartZone>
    </div>
    <div class="site-alert-wrapper">
        <div class="row">
            <div class="large-12 columns">
                <WebPartPages:WebPartZone runat="server" Title="loc:ContentAlert" ID="ContentAlert" FrameType="TitleBarOnly">
                    <ZoneTemplate></ZoneTemplate>
                </WebPartPages:WebPartZone>
            </div>
        </div>

    </div>

    <div class="contentBody">

        <div class="row">
            <div class="large-3 medium-3 columns">
                <WebPartPages:WebPartZone runat="server" Title="loc:TreeContent" ID="TreeContent" FrameType="TitleBarOnly">
                    <ZoneTemplate>
                    </ZoneTemplate>
                </WebPartPages:WebPartZone>

            </div>
           
            <div class="large-9 medium-9 columns">
                <div class="row">

                    <div class="large-3 medium-12 large-push-9 columns">
                        <div class="filtersBlock">
                            <div class="filterTrigger">
                                <span class="filterTriggerTxt">Filters</span>
                            </div>
                            <div class="filtersWrapper">
                                <WebPartPages:WebPartZone runat="server" Title="loc:FilterContent" ID="FilterContent" FrameType="TitleBarOnly">
                                    <ZoneTemplate>
                                    </ZoneTemplate>
                                </WebPartPages:WebPartZone>

                            </div>
                        </div>
                    </div>
                    <div class="large-9 medium-12 large-pull-3 columns">
                        <WebPartPages:WebPartZone runat="server" Title="loc:TabContent" ID="TabContent" FrameType="TitleBarOnly">
                            <ZoneTemplate>
                            </ZoneTemplate>
                        </WebPartPages:WebPartZone>
                        <WebPartPages:WebPartZone runat="server" Title="loc:GridContent" ID="GridContent" FrameType="TitleBarOnly">
                            <ZoneTemplate>
                            </ZoneTemplate>
                        </WebPartPages:WebPartZone>

                    </div>
                </div>

            </div>
        </div>

    </div>

    <SharePointWebControls:ScriptBlock ID="Scriptblock2" runat="server">
        if(typeof(MSOLayout_MakeInvisibleIfEmpty) == &quot;function&quot;) 
	
	
	
	
	{MSOLayout_MakeInvisibleIfEmpty();}
    </SharePointWebControls:ScriptBlock>
    <script src="/_layouts/15/Akumina.WebParts/js/spark/document-filters.js">//<![CDATA[
         //]]></script>

     <SharePointWebControls:ScriptLink ID="ScriptLink1" Name="sp.debug.js" LoadAfterUI="true" Localizable="false" runat="server" />

 </asp:Content>