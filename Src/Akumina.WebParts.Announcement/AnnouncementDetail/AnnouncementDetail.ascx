<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AnnouncementDetail.ascx.cs" Inherits="Akumina.WebParts.Announcement.AnnouncementDetail.AnnouncementDetail" %>
<asp:Literal ID="__searchCrawlFeed" runat="server"></asp:Literal>
<script type="text/javascript">
    if (Akumina === undefined) {
        var Akumina = {};
    }
    if (Akumina.InterAction === undefined) {
        Akumina.InterAction = {};
    }
</script>

<div id="targetAnnouncement"></div>

<script type="text/javascript">
    if (Akumina.InterAction.AnnouncementDetail === undefined) {

        Akumina.InterAction.AnnouncementDetail = function () {
            var me = this;
            this.targetDiv = "#targetAnnouncement";
            this.resultTemplateJson = <asp:Literal ID="ContentListItem" runat="server"/>;
            this.templateJson = <asp:Literal ID="ItemDetailTemplate" runat="server"/>;

            var adTemplate = Mustache.to_html(this.templateJson[0], this.resultTemplateJson);

            $(this.targetDiv).html(adTemplate);
        };
    }

    var announcementDetail =  new Akumina.InterAction.AnnouncementDetail();  

    $(document).ready(function () {
     
        var seoTitle = '';        
        if (announcementDetail && announcementDetail.resultTemplateJson && announcementDetail.resultTemplateJson.SEOTitle )
        {
            seoTitle = announcementDetail.resultTemplateJson.SEOTitle;
        }        
        if(seoTitle) {
            document.title = seoTitle;
        }
        else {
            document.title = $("#targetAnnouncement h1.h1Title").html();
        }        
    });
</script>