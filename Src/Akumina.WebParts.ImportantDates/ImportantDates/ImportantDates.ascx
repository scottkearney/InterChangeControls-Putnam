<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ImportantDates.ascx.cs" Inherits="Akumina.WebParts.ImportantDates.ImportantDates" %>
<asp:Literal ID="__searchCrawlFeed" runat="server"></asp:Literal>
<script type="text/javascript">
    if (Akumina === undefined) {
        var Akumina = {};
    }
    if (Akumina.InterAction === undefined) {
        Akumina.InterAction = {};
    }
</script>

<section class="rog-widget rog-important-dates"></section>

<script type="text/javascript">
    if (Akumina.InterAction.ImportantDates === undefined) {

        Akumina.InterAction.ImportantDates = function () {
            this.targetDiv = ".rog-important-dates";
            this.resultTemplateJson = <asp:Literal ID="ContentListItem" runat="server"/>;
            this.templateJson = <asp:Literal ID="ItemListTemplate" runat="server"/>;

            var adTemplate = Mustache.to_html(this.templateJson[0], this.resultTemplateJson);

            $(this.targetDiv).html(adTemplate);
        };
    }

    var announcementDetail = Akumina.InterAction.ImportantDates();  
</script>