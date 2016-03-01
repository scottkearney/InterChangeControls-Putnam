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
        var navBarHelpOverrideKey ="WSSEndUser";
    </SharePoint:ScriptBlock>
    <SharePoint:StyleBlock ID="Styleblock1" runat="server">
body #s4-leftpanel {display:none; }
.s4-ca {margin-left:0px; }
    </SharePoint:StyleBlock>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PlaceHolderSearchArea" runat="server">
    <SharePoint:DelegateControl ID="Delegatecontrol1" runat="server"
        ControlId="SmallSearchInputBox" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="PlaceHolderPageDescription" runat="server">
    <SharePoint:ProjectProperty ID="Projectproperty1" Property="Description" runat="server" />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="PlaceHolderMain" runat="server">
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

       <%-- <div class="row">
            <div class="large-3 medium-3 columns">
                <WebPartPages:WebPartZone runat="server" Title="loc:TreeContent" ID="TreeContent" FrameType="TitleBarOnly">
                    <ZoneTemplate>
                    </ZoneTemplate>
                </WebPartPages:WebPartZone>

            </div><div class="large-9 medium-9 columns">--%>
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

            <%--</div>
        </div>--%>

    </div>

    <SharePoint:ScriptBlock ID="Scriptblock2" runat="server">
        if(typeof(MSOLayout_MakeInvisibleIfEmpty) == &quot;function&quot;) 
	
	
	
	
	{MSOLayout_MakeInvisibleIfEmpty();}
    </SharePoint:ScriptBlock>
    <script src="/_layouts/15/Akumina.WebParts/js/spark/document-filters.js">//<![CDATA[
         //]]></script>
      <SharePoint:ScriptLink ID="ScriptLink1" Name="sp.debug.js" LoadAfterUI="true" Localizable="false" runat="server" />

</asp:Content>
