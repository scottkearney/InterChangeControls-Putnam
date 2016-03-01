<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DocumentTab.ascx.cs" Inherits="Akumina.WebParts.Documents.DocumentTab.DocumentTab" %>
<%--<asp:HiddenField ID="tabQuery" runat="server" ClientIDMode="Static" />
<asp:HiddenField ID="hdnsubstatus" ClientIDMode="Static" runat="server" />
<asp:HiddenField ID="searchTextTab" ClientIDMode="Static" runat="server" />
<asp:HiddenField ID="currentDFolderPath" ClientIDMode="Static" runat="server" />
<asp:HiddenField ID="refreshIdleTab" ClientIDMode="Static" runat="server" />
<asp:HiddenField ID="hdnQuerystatus" ClientIDMode="Static" runat="server" />--%>
<div class="interAction">
    <asp:UpdatePanel ID="updatePnlTab" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <asp:HiddenField ID="lblID" ClientIDMode="Static" runat="server" />
            <div class="ia-tabs">
                <ul class="ia-tabs-nav">
                    <li>
                        <a id="allFiles" class="allFiles" runat="server" title="All Files" onclick="tabHighlight('0',this) ">All Files
                           <span class="ia-tab-count allfilecount"></span>

                        </a>
                    </li>
                    <li>
                        <a id="myFiles" class="myFiles" runat="server" title="My Files" onclick=" tabHighlight('1',this)">My Files
                            <span class="ia-tab-count myfilecount"></span>
                        </a>
                    </li>
                    <li>
                        <a id="popularFiles" runat="server" title="Popular" class="popularFiles" onclick="tabHighlight('2',this)">Popular
                          <span class="ia-tab-count popularfilecount"></span>   
                        </a>
                    </li>
                    <li>
                        <a id="recentFiles" runat="server" title="Recent" class="recentFiles" onclick=" tabHighlight('3',this)">Recent
                            <span class="ia-tab-count recentfilecount"></span>
                        </a>
                    </li>
                </ul>
                <%--  <asp:Button ID="btnTabQuery" CssClass="tabQuerybtn" runat="server" ClientIDMode="Static" OnClick="btnTabQuery_Click" />--%>
                <%--  <ul class="ia-tabs-nav">
                    <li>
                        <asp:LinkButton ID="allFiles" runat="server" OnClick="tab_Click_allfiles" CommandName="All Files" OnClientClick=" tabHighlight('0') " CommandArgument="All Files">
                            <%= Resources.Tab_AllFiles_Text %> <span class="ia-tab-count">
                                <label id="lblAllFiles" runat="server"></label>
                            </span>
                            <asp:Literal ID="ltrlAllFiles" runat="server"></asp:Literal>
                        </asp:LinkButton>
                    </li>
                    <li>
                        <asp:LinkButton ID="myFiles" runat="server" OnClick="tab_Click_myfiles" Text="My Files" OnClientClick=" tabHighlight('1') " CommandName="My Files">
                        </asp:LinkButton>
                    </li>
                    <li>
                        <asp:LinkButton ID="popularFiles" runat="server" Text="Popular" OnClick="tab_Click_popular" OnClientClick=" tabHighlight('2') " CommandName="Popular">

                        </asp:LinkButton>
                    </li>
                    <li>
                        <asp:LinkButton ID="recentFiles" runat="server" OnClick="tab_Click_recent" Text="Recent" OnClientClick=" tabHighlight('3') " CommandName="Recent">

                        </asp:LinkButton>
                    </li>
                </ul>--%>
            </div>
        </ContentTemplate>

    </asp:UpdatePanel>
</div>
