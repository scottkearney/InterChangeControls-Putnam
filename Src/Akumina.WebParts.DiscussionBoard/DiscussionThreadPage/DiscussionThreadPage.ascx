<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DiscussionThreadPage.ascx.cs" Inherits="Akumina.WebParts.DiscussionBoard.DiscussionThreadPage.DiscussionThreadPage" %>
<%@ Register Assembly="CKEditor.NET, Version=3.6.6.2, Culture=neutral, PublicKeyToken=e379cdf2f8354999" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>

<%--
<!-- CSS -->
<link rel="stylesheet" href="../_layouts/15/Akumina.WebParts.DiscussionBoard/css/ia-interaction-controls.css" />

<!-- JS -->
<script src="../_layouts/15/Akumina.WebParts.DiscussionBoard/js/vendor/jquery-2.1.3.min.js"></script>
<script src="../_layouts/15/Akumina.WebParts.DiscussionBoard/js/vendor/modernizr.js"></script>

<!-- Discussion Thread JS -->
<script src="../_layouts/15/Akumina.WebParts.DiscussionBoard/js/controls/ia-discussion-thread.js"></script>

<!-- Modal JS -->
<script src="../_layouts/15/Akumina.WebParts.DiscussionBoard/js/vendor/jquery.magnific-popup.js"></script>
<script src="../_layouts/15/Akumina.WebParts.DiscussionBoard/js/components/ia-modal.js"></script>

<!-- Folder Tree JS -->
<script src="../_layouts/15/Akumina.WebParts.DiscussionBoard/js/controls/ia-jstree-discussion.js"></script>
<script src="../_layouts/15/Akumina.WebParts.DiscussionBoard/js/controls/ia-document-folder-tree.js"></script>

<!-- Search Picker JS -->
<script src="../_layouts/15/Akumina.WebParts.DiscussionBoard/js/vendor/chosen.jquery.min.js"></script>
<script src="../_layouts/15/Akumina.WebParts.DiscussionBoard/js/components/ia-search-picker.js"></script>
<script lang="javascript" type="text/javascript">
    //Ensures placeholder fits in input box for Edit Permissions
    $(document).ready(function () {

        $('.default').css('width', '250px');
    });
</script>

<!-- Sticky Action Menu JS -->
<script src="../_layouts/15/Akumina.WebParts.DiscussionBoard/js/vendor/jquery.sticky.js"></script>
<script src="../_layouts/15/Akumina.WebParts.DiscussionBoard/js/vendor/stickyscrollfix.js"></script>
<script src="../_layouts/15/Akumina.WebParts.DiscussionBoard/js/vendor/mustache.min.js"></script>
<script src="../_layouts/15/Akumina.WebParts.DiscussionBoard/js/controls/ia-common-discussion.js"></script>--%>

<asp:Literal runat="server" ID="litTop" >
</asp:Literal>
<div class="interAction">

    <div class="ia-discussion-board-page-title">
        <h1>
            <asp:Literal ID="litrlTitle" runat="server"></asp:Literal></h1>
    </div>

    <!-- Discussion Board Thread -->
    <div class="ia-discussion-thread clearfix">
        <asp:UpdatePanel ID="UpdatePanelThread" runat="server" UpdateMode="Conditional">
            <ContentTemplate>

                <section class="ia-discussion-thread-main">
                    <div class="ia-discussion-thread-subject ia-left">
                        <h2>
                            <asp:Literal ID="lblSubject" runat="server"></asp:Literal>
                        </h2>
                    </div>
                    <div id="divEditPermission" runat="server" visible="false">
                        <a href="#ia-discussion-edit-permissions-modal" class="ia-modal-inline-trigger ia-button ia-discussion-thread-permissions-btn ia-right">Edit Permissions</a>

                    </div>
                    <hr class="ia-show-on-small-only">
                    <!-- Thread Topic -->
                    <article class="ia-discussion-thread-topic clearfix">

                        <div class="ia-discussion-author-icon">
                            <%--<img id="imgAuthor" runat="server" />--%>
                            <asp:Image ID="imgAuthorProfile" runat="server" />
                        </div>

                        <div id="divDeleteThread" runat="server" visible="false">
                            <a href="#ia-discussion-delete-thread-modal" class="ia-modal-inline-trigger ia-button secondary ia-discussion-delete-btn ia-right" title="Delete Post">
                                <span class="fa fa-times ia-show-on-small-only"></span><span class="ia-hide-on-small-only">Delete Thread
                        <%--<asp:Button ID="lnkbtnDeleteDiscussion" OnClientClick='javascript:return confirm("Are you sure you want to delete?")' runat="server" Text="Delete Thread"></asp:Button>--%>
                                </span>
                            </a>
                        </div>

                        <div class="ia-discussion-thread-info">
                            <div class="ia-discussion-author-name ia-left">
                                <asp:Label ID="lblAuthorName" runat="server"></asp:Label>
                            </div>

                            <div class="ia-discussion-thread-content">
                                <asp:Label ID="lblDescription" runat="server"></asp:Label>
                            </div>

                            <div class="ia-discussion-thread-attachments" id="threadAttachments" runat="server">
                                <span class="fa fa-paperclip"></span>
                                <div class="ia-discussion-thread-attachments-title">Attachments:</div>
                                <ul id="ulAttachment" runat="server">
                                </ul>
                                <%--<asp:Literal ID="ltrlAttachement" runat="server" EnableViewState="true" Text=""></asp:Literal>--%>
                            </div>

                            <div class="ia-discussion-thread-date">
                                POSTED ON 
                        <asp:Label ID="lblThreadDate" runat="server"></asp:Label>
                            </div>
                        </div>
                    </article>

                    <!-- Thread Replies -->

                    <section class="ia-discussion-thread-replies clearfix">



                        <asp:Repeater ID="rptrReplies" runat="server" OnItemCommand="rptrReplies_ItemCommand" OnItemDataBound="rptrReplies_ItemDataBound">
                            <HeaderTemplate>
                                <div class="ia-discussion-thread-replies-title">Replies</div>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <article class="ia-discussion-reply-item clearfix">
                                    <div class="ia-discussion-author-icon">
                                        <asp:Image ID="imgReply" runat="server" src='<%# Eval("AuthorPicture") %>' />
                                    </div>

                                    <div id="divDeleteReply" runat="server" visible="false">
                                        <a href="#ia-discussion-delete-post-modal" onclick="DeleteReplyFunction('<%#Eval("ID") %>')" class="ia-modal-inline-trigger ia-button secondary ia-discussion-delete-btn ia-right">
                                            <span class="fa fa-times ia-show-on-small-only"></span><span class="ia-hide-on-small-only">Delete Post</span>
                                        </a>
                                    </div>

                                    <asp:HiddenField ID="hdnAttachment" Value='<%# Eval("Attachment") %>' runat="server" />
                                    <asp:HiddenField ID="hdnID" Value='<%# Eval("ID") %>' runat="server" />
                                    <asp:HiddenField ID="hdnProfileAuthor" Value='<%# Eval("ProfileAuthor") %>' runat="server" />
                                    <div class="ia-discussion-reply-info">
                                        <div class="ia-discussion-author-name ia-left"><%# Eval("Author") %></div>
                                        <div class="ia-discussion-reply-content">
                                            <%# Eval("Body") %>
                                            <div class="ia-discussion-thread-attachments" id="divAttachmentReply" runat="server">
                                                <span class="fa fa-paperclip"></span>
                                                <div class="ia-discussion-thread-attachments-title">Attachments:</div>

                                                <ul id="ulAttachmentReplies" runat="server">
                                                </ul>
                                            </div>
                                        </div>
                                        <div class="ia-discussion-post-time">POSTED ON <%#  DateTime.Parse(Eval("Created").ToString()).ToString("MMM. d, yyyy") %></div>
                                    </div>
                                </article>


                            </ItemTemplate>
                        </asp:Repeater>
                        <asp:HiddenField ID="hdnDeleteReplyId" ClientIDMode="Static" runat="server" />

                        <!-- Add Thread Reply -->
                        <asp:Panel ID="replypanel" runat="server" Visible="false">
                            <article class="ia-discussion-reply-item ia-discussion-add-reply clearfix">
                                <div class="ia-discussion-author-icon">
                                    <asp:Image ID="imgCurrentUser" runat="server" />
                                </div>

                                <div class="ia-discussion-reply-info">

                                    <div class="ia-discussion-reply-content">
                                        <!-- show input field until user clicks it -->
                                        <input type="text" name="ia-discussion-add-reply-box" id="ia-discussion-add-reply-box" readonly="readonly" placeholder="Add a reply or attach a file..." />

                                        <!-- This will all be hidden until Focus is placed on the <input> above, where it switches to an editor instead of a textbox -->
                                        <div class="ia-discussion-reply-editor">

                                            <div class="ia-discussion-reply-richtext-editor">
                                                <!-- Put Rich Text Editor here -->
                                                <CKEditor:CKEditorControl ID="CKEditorReply" BasePath="../_layouts/15/Akumina.WebParts.DiscussionBoard/ckeditor/" runat="server"></CKEditor:CKEditorControl>
                                            </div>

                                            <p class="ia-discussion-library-selector">
                                                <span class="fa fa-paperclip"></span>To add a link to a file or folder, 
										<!-- Opens Modal with Document Library -->
                                                <a href="#ia-discussion-reply-folder-tree-modal" class="ia-modal-inline-trigger">select from the document library.</a>
                                            </p>

                                            <script id="files_Addtemplate" type="text/html">

                                                <span class="ia-discussion-attachments-action-btn" item-id="{{ItemId}}" list-name="{{ListName}}" item-url="{{Url}}"><a href="#" onclick="fileRemove(this);"><span class="fa fa-times-circle"></span></a>{{Name}}
                                                </span>
                                            </script>
                                           
                                            <div class="ia-discussion-thread-attachments-edit">
                                                <div id="listOfFiles" runat="server" class="listOfFiles"></div>

                                                <input type="text" class="listOfFilesHidden" id="listOfFilesHn" runat="server" style="display: none" />
                                                <div class="listOfFilesHiddenSec" id="listOfFilesHnSec" runat="server" style="display: none"></div>
                                            </div>

                                            <div class="ia-button-row">
                                                <a id="anchrPostreply" runat="server" onserverclick="anchrPostreply_ServerClick" onclick="showloader();" data-dropdown="#dropdown-1" class="ia-button anchrPostreply">Post Reply</a>

                                                <a href="#ia-discussion-cancel-modal" class="ia-modal-inline-trigger ia-discussion-cancel ia-button secondary ia-discussion-cancel-btn">Cancel</a>
                                            </div>

                                            <!-- Document Library Modal -->
                                            <div id="ia-discussion-reply-folder-tree-modal" class="mfp-hide interAction ia-modal">
                                                <div class="ia-modal-upload">
                                                    <h2 class="ia-modal-heading">Choose a file to link</h2>

                                                    <!-- Document Folder Tree -->
                                                    <div class="ia-folder-tree">
                                                        <asp:Literal ID="ltlFolderInfo" runat="server" EnableViewState="true" Text=""></asp:Literal>
                                                    </div>


                                                    <div class="ia-button-row">
                                                        <a class="ia-button" id="addFileRef" onclick="addSelectedFile(event);">OK</a>

                                                        <a class="ia-button secondary ia-modal-dismiss" href="#">Cancel</a>
                                                    </div>

                                                </div>
                                            </div>

                                        </div>
                                    </div>
                                </div>


                            </article>
                        </asp:Panel>
                    </section>


                </section>
                <!-- end .ia-discussion-thread-left-col -->

                <!-- Sidebar/Action Menu -->
                <aside class="ia-discussion-thread-action-menu">
                    <span class="ia-action-menu-toggle ia-show-on-small-only"><span class="ia-action-menu-arrow fa fa-chevron-up"></span></span>

                    <div class="ia-action-menu-content">
                        <ul>
                            <li id="addReplyToPost" class="addReplyToPost" runat="server" visible="false"><a href=""><span class="fa fa-reply"></span>Add Reply </a></li>
                            <li runat="server" id="liFollowThread"><a id="anchrFollow" runat="server" onserverclick="anchrFollowThread_ServerClick"><span class="fa fa-star"></span>Follow </a></li>
                            <li runat="server" id="liFollowingThread" style="background-color: #2d84bb;"><a id="anchrFollowing" runat="server" onserverclick="anchrUnFollowThread_ServerClick"><span class="fa fa-star"></span>Following </a></li>
                            <li><a href=""><span class="fa fa-arrow-circle-up"></span>Back to Top </a></li>
                            <li><a id="discussionListPage" runat="server"><span class="fa fa-list"></span>Back to Discussion List </a></li>
                        </ul>
                    </div>

                </aside>

                <!-- Archive Modal -->
                <div id="ia-discussion-delete-thread-modal" class="mfp-hide interAction ia-modal">
                    <div class="ia-modal-upload">
                        <h2 class="ia-modal-heading">Confirm Delete Thread</h2>
                        <p>This action will remove the discussion from the list and archive its contents.  Do you want to continue </p>

                        <div class="ia-button-row">
                            <a id="anchrDeleteThread" runat="server" onserverclick="anchrDeleteThread_ServerClick" onclick="showloader();" class="ia-button">OK</a>
                            <a class="ia-button secondary ia-modal-dismiss" href="#">Cancel</a>
                        </div>

                    </div>
                </div>

                <!-- Delete Modal -->
                <div id="ia-discussion-delete-post-modal" class="mfp-hide interAction ia-modal">
                    <div class="ia-modal-upload">
                        <h2 class="ia-modal-heading">Confirm Delete Post</h2>
                        <p>This action will permanently delete the discussion. This action is not recoverable. Do you want to continue? </p>

                        <div class="ia-button-row">
                            <a class="ia-button" href="#" id="anhrDeleteReply" runat="server" onclick="showloader();" onserverclick="anhrDeleteReply_ServerClick">OK</a>
                            <a class="ia-button secondary ia-modal-dismiss" href="#">Cancel</a>
                        </div>

                    </div>
                </div>

                <!-- Edit Permissions Modal -->

                <div id="ia-discussion-edit-permissions-modal" class="mfp-hide interAction ia-modal">
                    <div class="ia-modal-upload">
                        <h2 class="ia-modal-heading">Edit Permissions</h2>
                        <p>By default, this discussion will be visible by all authorized users of this site. If you want to control who can view this discussion thread, please select the users or groups <%--by typing --%>in the box below.</p>
                        <!-- Search Picker -->
                        <div class="ia-search-picker-wrapper" id="divPicker" runat="server">
                            <asp:ListBox ID="pplpickerPermission" runat="server" SelectionMode="Multiple" data-placeholder="Select users who can view this post..." class="ia-search-picker"></asp:ListBox>
                            <%-- <SharePoint:ClientPeoplePicker ID="pplpickerPermission" runat="server" EnableViewState="true" AllowMultipleEntities="true" PrincipalAccountType="User,SPGroup" />--%>
                        </div>
                        <!-- end .ia-search-picker-wrapper -->

                        <p>&nbsp;</p>

                        <div class="ia-button-row">
                            <a id="btnPermissions" runat="server" onserverclick="btnPermissions_ServerClick" class="ia-button" onclick="showloader();">OK</a>

                            <a class="ia-button secondary ia-modal-dismiss" href="#">Cancel</a>

                        </div>

                        <!-- Delete Modal -->
                        <div id="ia-discussion-cancel-modal" class="mfp-hide interAction ia-modal">
                            <div class="ia-modal-upload">
                                <h2 class="ia-modal-heading">Confirm Cancel</h2>
                                <p>Are you sure you want to cancel? </p>

                                <div class="ia-button-row">
                                    <%--<a class="ia-button">OK</a>--%>
                                    <a class="ia-button" id="btncanConfirm" onserverclick="btnCancelReply_Click" runat="server">Yes</a>
                                    <a class="ia-button secondary ia-modal-dismiss" href="#">No</a>
                                </div>

                            </div>
                        </div>
                    </div>
                </div>


            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <!-- end .ia-discussion-thread -->


    <div class="ia-loading-panel ia-hide">

        <div class="ia-loader"></div>

    </div>
</div>
<!-- end .interAction -->




<script lang="javascript" type="text/javascript">



    var prm = Sys.WebForms.PageRequestManager.getInstance();
    prm.add_endRequest(EndRequest);

    function EndRequest(sender, args) {
        EndRequestDiscussionThread();
    }
    

</script>

