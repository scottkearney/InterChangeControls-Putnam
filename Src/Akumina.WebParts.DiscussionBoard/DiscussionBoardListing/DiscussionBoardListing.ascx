<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DiscussionBoardListing.ascx.cs" Inherits="Akumina.WebParts.DiscussionBoard.DiscussionBoardListing.DiscussionBoardListing" %>




<%--<link href="../_layouts/15/Akumina.WebParts.DiscussionBoard/css/ia-interaction-controls.css" rel="stylesheet" />
<script src="../_layouts/15/Akumina.WebParts.DiscussionBoard/js/vendor/jquery-2.1.1.min.js"></script>
<script src="../_layouts/15/Akumina.WebParts.DiscussionBoard/js/vendor/modernizr.js"></script>
<script src="../_layouts/15/Akumina.WebParts.DiscussionBoard/js/vendor/jquery.magnific-popup.js"></script>
<script src="../_layouts/15/Akumina.WebParts.DiscussionBoard/js/components/ia-modal.js"></script>
<script src="../_layouts/15/Akumina.WebParts.DiscussionBoard/js/controls/ia-common-discussion.js"></script>--%>
<asp:Literal runat="server" ID="litTop" >
</asp:Literal>
<div class="interAction">
    <div class="ia-discussion-board">
        <div class="ia-discussion-board-page-title ia-left">
            <h1>
                <asp:Literal ID="boardPageTitle" runat="server"></asp:Literal></h1>
        </div>

        <div class="ia-discussion-page-title clearfix">
            <button id="ListingAddNewDiscussion" visible="false" class="ia-button ia-discussion-board-page-title-button ia-left" onserverclick="ListingAddNewDiscussion_ServerClick" runat="server">Add New Discussion</button>
        </div>



        <asp:UpdatePanel ID="DiscussionBoardListingUpdatePanel" runat="server">
            <ContentTemplate>
                <!-- Discussion Board List -->
                <div class="ia-discussion-board-list clearfix">

                    <asp:Literal ID="errorLiteral" runat="server"></asp:Literal>

                    <asp:HiddenField ID="hdnCurrentPage" ClientIDMode="Static" runat="server" />
                    <asp:HiddenField ID="hdnArchieveId" ClientIDMode="Static" runat="server" />
                    <asp:HiddenField ID="hdnDeleteId" ClientIDMode="Static" runat="server" />
                    <asp:HiddenField ID="hdnSortBy" ClientIDMode="Static" runat="server" />
                    <asp:HiddenField ID="hdnSortDirection" ClientIDMode="Static" runat="server" />
                    <asp:HiddenField ID="hdnPageLoad" ClientIDMode="Static" runat="server" />

                    <asp:Repeater ID="repeaterDBList" runat="server" OnItemCommand="repeaterDBList_ItemCommand">
                        <HeaderTemplate>
                            <!-- Discussion Board Table Headline -->
                            <div class="ia-discussion-board-table-headers ia-discussion-board-item clearfix">

                                <div class="ia-discussion-author-icon">&nbsp;</div>
                                <span class="ia-discussion-author-name">&nbsp;</span>
                                <div class="ia-discussion-post-info">&nbsp;</div>
                                <span class="ia-discussion-created">CREATED
                            <span>
                                <asp:LinkButton ID="linkSortCreated" CssClass="" CommandName="Created" runat="server" Text=""></asp:LinkButton>
                            </span>
                                </span>

                                <span class="ia-discussion-post-time">LAST POST
                            <span>                               
                                <asp:LinkButton ID="linkSortPostTime" CssClass="" CommandName="DiscussionLastUpdated" runat="server" Text=""></asp:LinkButton></span>
                                </span>

                                <span class="ia-discussion-replies">REPLIES<span>                                    
                                    <asp:LinkButton ID="linkSortReplies" CssClass="" CommandName="ItemChildCount" runat="server" Text=""></asp:LinkButton></span>
                                </span>
                                <div class="ia-discussion-actions">&nbsp;</div>
                            </div>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <div class="ia-discussion-board-item clearfix">
                                <div class="ia-discussion-author-icon">
                                    <img src="<%# Eval("AuthorPicture") %>" />
                                </div>
                                <span class="ia-discussion-author-name"><%# Eval("Author") %></span>

                                <div class="ia-discussion-post-info">
                                    <span class="ia-discussion-post-subject"><a href="<%#  Eval("EditDiscussionPost") %>"><%# Eval("Title") %></a></span>
                                    <span class="ia-discussion-post-summary">&nbsp;&nbsp;<%# Eval("Body") %>
                                    </span>
                                </div>

                                <span class="ia-discussion-created">
                                        <%# TimeAgo(Convert.ToDateTime(Eval("Created"))) %>
                                </span>
                                <span class="ia-discussion-post-time">
                                    <%# TimeAgo(Convert.ToDateTime(Eval("DiscussionLastUpdated"))) %>                                    
                                </span>
                                <span class="ia-discussion-replies"><span class="ia-discussion-replies-amount"><%# Eval("ItemChildCount") %></span></span>

                                <div class="ia-discussion-actions" runat="server" visible='<%#Convert.ToBoolean(Eval("modifyPermission"))?true:false%>'>                                    
                                        <a href="#ia-discussion-archive-modal" onclick="archiveFunction('<%#Eval("ID") %>');" class="ia-modal-inline-trigger ia-discussion-archive ia-button secondary ia-discussion-archive-btn">Archive</a>
                                        <a href="#ia-discussion-delete-modal" onclick="deleteFunction('<%#Eval("ID") %>');" class="ia-modal-inline-trigger ia-discussion-delete ia-button secondary ia-discussion-delete-btn">Delete</a>                                
                                </div>
                            </div>
                        </ItemTemplate>
                        <FooterTemplate>
                        </FooterTemplate>
                    </asp:Repeater>
                    <!-- Post -->
                </div>
                <!-- end .ia-discussion-board-list -->

                <!-- Pagination -->
                <div class="ia-paging">
                    <ul class="ia-pagination">
                        <li class="ia-paging-status">
                            <asp:Label ID="lblpage" runat="server" />
                        </li>
                        <li id="iafirstpage" class="ia-paging-first">
                            
                            <asp:LinkButton Text="" CssClass="fa fa-angle-double-left" runat="server" OnClick="linkbtnFirst_Click"
                                ID="linkbtnFirst" />
                        </li>
                        <li id="iapreviouspage" class="ia-paging-previous">
                            
                            <asp:LinkButton Text="" CssClass="fa fa-angle-left" runat="server" OnClick="linkbtnPrevious_Click"
                                ID="linkbtnPrevious" />
                        </li>
                        <li id="ianextpage" class="ia-paging-next">
                            
                            <asp:LinkButton Text="" CssClass="fa fa-angle-right" runat="server" OnClick="linkbtnNext_Click"
                                ID="linkbtnNext" />
                        </li>
                        <li id="ialastpage" class="ia-paging-last">
                            
                            <asp:LinkButton Text="" CssClass="fa fa-angle-double-right" runat="server" OnClick="linkbtnLast_Click"
                                ID="linkbtnLast" />
                        </li>
                    </ul>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>

        <!-- Archive Modal -->
        <div id="ia-discussion-archive-modal" class="mfp-hide interAction ia-modal">
            <div class="ia-modal-upload">
                <h2 class="ia-modal-heading">Confirm Archive</h2>
                <p>This action will remove the discussion from the list and archive its contents.  Do you want to continue? </p>
                <div class="ia-button-row">
                    <a class="ia-button" id="btnarcConfirm" onserverclick="btnArchive_Click" onclick="showloader();" runat="server">OK</a>
                    <a class="ia-button secondary ia-modal-dismiss" href="#">Cancel</a>
                </div>

            </div>
        </div>

        <!-- Delete Modal -->
        <div id="ia-discussion-delete-modal" class="mfp-hide interAction ia-modal">
            <div class="ia-modal-upload">
                <h2 class="ia-modal-heading">Confirm Delete</h2>
                <p>This action will permanently delete the discussion. This action is not recoverable. Do you want to continue? </p>
                <div class="ia-button-row">
                    <a class="ia-button" id="btndelConfirm" onserverclick="btnDelete_Click" onclick="showloader();" runat="server">OK</a>
                    <a class="ia-button secondary ia-modal-dismiss" href="#">Cancel</a>
                </div>

            </div>
        </div>
    </div>
    <!-- end .ia-discussion-board -->
    <div class="ia-loading-panel ia-hide">

        <div class="ia-loader"></div>

    </div>
</div>
<!-- end .interAction -->





<script lang="javascript" type="text/javascript">

  
    $(document).ready(function () {

        commonDiscussionThreadListing();
    });


    var prm = Sys.WebForms.PageRequestManager.getInstance();
    prm.add_endRequest(EndRequest);

    function EndRequest(sender, args) {
      
        
        EndRequestThreadListing();

    }
   
    


</script>
