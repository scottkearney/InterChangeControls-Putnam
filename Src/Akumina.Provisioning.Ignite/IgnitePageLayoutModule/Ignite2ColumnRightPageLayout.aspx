<%@ Page Language="C#" Inherits="Microsoft.SharePoint.Publishing.PublishingLayoutPage,
Microsoft.SharePoint.Publishing,Version=15.0.0.0,Culture=neutral,PublicKeyToken=71e9bce111e9429c" %>

<%@ Register TagPrefix="SharePointWebControls" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages"
    Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="PublishingWebControls" Namespace="Microsoft.SharePoint.Publishing.WebControls"
    Assembly="Microsoft.SharePoint.Publishing, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="PublishingNavigation" Namespace="Microsoft.SharePoint.Publishing.Navigation"
    Assembly="Microsoft.SharePoint.Publishing, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<asp:Content contentplaceholderid="PlaceHolderPageTitle" runat="server">
 <SharePointWebControls:FieldValue id="FieldValue1" FieldName="Title" runat="server"/>
 </asp:Content>
<asp:Content id="Content2" contentplaceholderid="PlaceHolderAdditionalPageHead" runat="server">
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
<asp:Content id="Content3" contentplaceholderid="PlaceHolderSearchArea" runat="server">
    <SharePointWebControls:DelegateControl ID="Delegatecontrol1" runat="server"
        ControlId="SmallSearchInputBox" />
</asp:Content>
<asp:Content id="Content4" contentplaceholderid="PlaceHolderPageDescription" runat="server">
    <SharePointWebControls:ProjectProperty ID="Projectproperty1" Property="Description" runat="server" />
</asp:Content>
<asp:Content contentplaceholderid="PlaceHolderMain" runat="server">
         <div class="ms-hide">
        <WebPartPages:SPProxyWebPartManager runat="server" ID="__ProxyWebPartManagerForConnections__">
            <SPWebPartConnections>
            </SPWebPartConnections>
        </WebPartPages:SPProxyWebPartManager>
        <WebPartPages:WebPartZone runat="server" Title="loc:TitleBar" ID="TitleBar" AllowLayoutChange="false" AllowPersonalization="false" Style="display: none;">
            <ZoneTemplate></ZoneTemplate>
        </WebPartPages:WebPartZone>
    </div>
        
    <div class="ak-col-template ak-2-col-right-focus-template">
        <div class="ak-body-content">
            <div class="row">
				<div class="large-3 medium-3 small-12 columns ak-side-col">		
					<WebPartPages:WebPartZone runat="server" Title="loc:Refiner" ID="Refiner" FrameType="TitleBarOnly">
                     <ZoneTemplate>
                     </ZoneTemplate>
                </WebPartPages:WebPartZone>
				</div>
				<div class="large-9 medium-9 small-12 columns columns-no-padding ak-main-col">	
					<WebPartPages:WebPartZone runat="server" Title="loc:Results" ID="Results" FrameType="TitleBarOnly">
                     <ZoneTemplate>
                     </ZoneTemplate>
                </WebPartPages:WebPartZone>
				</div>
			</div>
        </div>
    </div>

    <SharePointWebControls:ScriptBlock ID="Scriptblock2" runat="server">
        if(typeof(MSOLayout_MakeInvisibleIfEmpty) == "function") 	
	        {MSOLayout_MakeInvisibleIfEmpty();}
    </SharePointWebControls:ScriptBlock>


 </asp:Content>