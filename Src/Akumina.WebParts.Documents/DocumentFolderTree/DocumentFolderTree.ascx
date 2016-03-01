<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DocumentFolderTree.ascx.cs" Inherits="Akumina.WebParts.Documents.DocumentFolderTree.DocumentFolderTree" %>
<asp:HiddenField ID="refreshIdleFT" ClientIDMode="Static" runat="server" />
<asp:UpdatePanel ID="upFolderTree" runat="server" UpdateMode="Conditional">

    <ContentTemplate>

        <div id="folderzone" style="display: none">
            <asp:TextBox ID="folderBox" runat="server" ClientIDMode="Static"></asp:TextBox><br />

        </div>

        <div class="interAction">
            <div class="ia-folder-tree" style="display: none">
                <asp:Literal ID="ltlFolderInfo" runat="server"></asp:Literal>
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
<div id="folder" style="visibility:hidden">
    <asp:Button ID="buttonchange" CssClass="folderchange" runat="server" OnClick="button_Click" Text="Change Folder" />
</div>    </ContentTemplate>
</asp:UpdatePanel>
<script type="text/javascript">
    var folderTreePrm = Sys.WebForms.PageRequestManager.getInstance();
    folderTreePrm.add_endRequest(EndRequest);

    function EndRequest(sender, args) {

        if ($("#refreshIdleFT").length > 0 && $("#refreshIdleFT").val() != "true") {
            EndRequestFolderTree();
        }
       
    }
</script>
