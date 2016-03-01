<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AnnouncementItems.ascx.cs" Inherits="Akumina.WebParts.Announcement.AnnouncementItems.AnnouncementItems" %>
<asp:Literal ID="__searchCrawlFeed" runat="server"></asp:Literal>
<script type="text/javascript">
    if (Akumina === undefined) {
        var Akumina = {};
    }
    if (Akumina.InterAction === undefined) {
        Akumina.InterAction = {};
    }
</script>

<div id="targetList" runat="server"></div>

<script type="text/javascript">
    Akumina.InterAction.AnnouncementItems = function () {
        var me = this;
        this.targetDiv = ".<%= this.UniqueId %>";
        this.resultTemplateJson = <asp:Literal ID="ContentListItem" runat="server"/>;
        this.templateJson = <asp:Literal ID="ItemListTemplate" runat="server"/>;

        var adTemplate = Mustache.to_html(this.templateJson[0], this.resultTemplateJson);

        $(this.targetDiv).html(adTemplate);
    };

    var announcementItems = Akumina.InterAction.AnnouncementItems();  
</script>
