<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SiteLinks.ascx.cs" Inherits="Akumina.WebParts.SiteLinks.SiteLinks.SiteLinks" %>

<asp:Literal ID="litTop" runat="server"></asp:Literal>

<div id="div<asp:Literal ID="litDivID" runat="server"/>" class="interAction">

</div>

<script type="text/javascript">
    $(document).ready(function() {
        $.ajaxSetup({
            cache: true
        });

        var thisSiteLinks<asp:Literal ID="uxUniqueId1" runat="server"></asp:Literal> = new Akumina.InterAction.SiteLinks();
        thisSiteLinks<asp:Literal ID="uxUniqueId2" runat="server"></asp:Literal> .uniqueId = ';<asp:Literal ID="litUniqueId" runat="server"></asp:Literal>';;
        thisSiteLinks<asp:Literal ID="uxUniqueId3" runat="server"></asp:Literal> .targetDiv = "#div" + thisSiteLinks<asp:Literal ID="uxUniqueId6" runat="server"></asp:Literal> .uniqueId;
        thisSiteLinks<asp:Literal ID="uxUniqueId4" runat="server"></asp:Literal> .templateJson =  <asp:Literal ID="litTemplates" runat="server"></asp:Literal>;
        
        <asp:Literal ID="litProperties" runat="server"></asp:Literal>

        thisSiteLinks<asp:Literal ID="uxUniqueId5" runat="server"></asp:Literal> .getDataUsingREST(thisSiteLinks<asp:Literal ID="uxUniqueId7" runat="server"></asp:Literal> );
    });
</script>