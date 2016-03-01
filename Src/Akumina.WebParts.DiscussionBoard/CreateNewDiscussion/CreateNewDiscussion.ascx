<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CreateNewDiscussion.ascx.cs" Inherits="Akumina.WebParts.DiscussionBoard.CreateNewDiscussion.CreateNewDiscussion" %>
<%@ Register Assembly="CKEditor.NET, Version=3.6.6.2, Culture=neutral, PublicKeyToken=e379cdf2f8354999" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>

<%--<link href="../_layouts/15/Akumina.WebParts.DiscussionBoard/css/ia-interaction-controls.css" rel="stylesheet" />
<script src="../_layouts/15/Akumina.WebParts.DiscussionBoard/js/vendor/jquery-2.1.3.min.js"></script>
<script src="../_layouts/15/Akumina.WebParts.DiscussionBoard/js/vendor/modernizr.js"></script>
<script src="../_layouts/15/Akumina.WebParts.DiscussionBoard/js/vendor/jquery.magnific-popup.js"></script>
<script src="../_layouts/15/Akumina.WebParts.DiscussionBoard/js/components/ia-modal.js"></script>
<script src="../_layouts/15/Akumina.WebParts.DiscussionBoard/js/controls/ia-jstree-discussion.js"></script>
<script src="../_layouts/15/Akumina.WebParts.DiscussionBoard/js/controls/ia-document-folder-tree.js"></script>
<script src="../_layouts/15/Akumina.WebParts.DiscussionBoard/js/vendor/chosen.jquery.min.js"></script>
<script src="../_layouts/15/Akumina.WebParts.DiscussionBoard/js/components/ia-search-picker.js"></script>
<script src="../_layouts/15/Akumina.WebParts.DiscussionBoard/js/vendor/mustache.min.js"></script>
<script src="../_layouts/15/Akumina.WebParts.DiscussionBoard/js/controls/ia-common-discussion.js"></script>--%>

<!-- JS for Search Picker -->
<%--<script src="../_layouts/15/Akumina.WebParts.DiscussionBoard/js/components/ia-search-picker.js"></script>--%>

<asp:Literal runat="server" ID="litTop" >
</asp:Literal>

<div class="interAction">

    <section class="ia-discussion-create-new ia-discussion-thread">

        <h1>
            <asp:Literal ID="litrlTitle" runat="server"></asp:Literal>
        </h1>
        <h2 class="ia-discussion-create-headline">Start a new discussion</h2>


        <div class="ia-discussion-create-title-box">
            <input name="ia-discussion-create-title" runat="server" class="ia-discussion-create-title" id="titleDiscussion" placeholder="Enter a title for the discussion" type="text">
            <div style="color: red; display: none" id="titleMandatory">*Title is required</div>
        </div>


        <!-- Placeholder for Rich Text Editor -->
        <div class="ia-discussion-richtext-editor">
            <CKEditor:CKEditorControl ID="bodyDiscussion" BasePath="../_layouts/15/Akumina.WebParts.DiscussionBoard/ckeditor/" runat="server"></CKEditor:CKEditorControl>

            <%--<SharePoint:InputFormTextBox ID="bodyDiscussion" EnableViewState="true"
                RichText="true"
                RichTextMode="FullHtml" runat="server"
                TextMode="MultiLine" Rows="5">
            </SharePoint:InputFormTextBox>--%>
        </div>

        <!-- Opens Modal with Document Library -->
        <p class="ia-discussion-library-selector">
            <span class="fa fa-paperclip"></span>To add a link to a file or folder, 
			
            <a href="#ia-discussion-reply-folder-tree-modal" class="ia-modal-inline-trigger">select from the document library.</a>
        </p>

        <!-- Action Items under text editor -->


        <script id="files_Addtemplate" type="text/html">
            <span class="ia-discussion-attachments-action-btn" item-id="{{ItemId}}" list-name="{{ListName}}" item-url="{{Url}}"><a href="#" onclick="fileRemove(this);"><span class="fa fa-times-circle"></span></a>{{Name}}
            </span>
        </script>
        <div class="ia-discussion-thread-attachments-edit">
            <div id="listOfFiles" runat="server" class="listOfFiles"></div>

            <input type="text" class="listOfFilesHidden" id="listOfFilesHn" runat="server" style="display: none" />
            <div class="listOfFilesHiddenSec" id="listOfFilesHnSec" runat="server" style="display: none"></div>
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

        <hr>
        <p>
            By default, this discussion will be visible by all authorized users of this site. If you want to control who can view this discussion thread, please select the users or groups in the box below.
        </p>


        <!-- Search Picker -->
        <%--<div class="ia-search-picker-wrapper">
            <asp:Panel ID="peoplePicker" runat="server">
            </asp:Panel>

        </div>--%>
        <div class="ia-search-picker-wrapper">
            <asp:ListBox ID="userAndUserGroups" runat="server" SelectionMode="Multiple" data-placeholder="Select users who can view this post..." class="ia-search-picker"></asp:ListBox>
            <%--<select id="userAndUserGroups" runat="server" data-placeholder="Select users who can view this post..." class="ia-search-picker" multiple="true" >
		            <option>Alison Haynes</option>
					<option>Amanda Dibble</option>
		            <option>Andy Frades</option>
		            <option>Cristine Pefine</option>
		            <option>Dan O'Neil</option>
		            <option>David Burkinshaw</option>
		            <option>Diego Rosa</option>
					<option>DJ Hughes</option>
		            <option>Dan Leahy</option>
		            <option>David Olson</option>
		            <option>David West</option>
		            <option>Ed Rogers</option>
		            <option>Heather Bowman</option>
		            <option>Heather Shuck</option>
		            <option>Jackie Doyle</option>
		            <option>Jason Vivier</option>
		            <option>Josh Apgar</option>
		            <option>Kathleen Muhonen</option>
		            <option>Lee Zheng</option>
		            <option>Michael Kowalewicz</option>
		            <option>Scott Hughes</option>
		            <option>Sharon Genest</option>
		            <option>Steve Hallmark</option>
		            <option>Steven Sherkanowski</option>
		            <option>Scott Kearney</option>
		            <option>Tom Smith</option>
				</select>--%>
        </div>
        <!-- end .ia-search-picker-wrapper -->


        <div class="ia-discussion-create-cta">
            <asp:Button ID="postDisucssion" runat="server" CssClass="ia-button" Text="Post discussion" OnClientClick="postValidation(event);" OnClick="postDisucssion_Click" />
            <%--<button class="ia-button">Post discussion</button>--%>
            <%--<asp:Button ID="cancelPostDiscussion" runat="server" CssClass="ia-button secondary" Text="Cancel" OnClick="cancelPostDiscussion_Click" />--%>
            <a href="#ia-discussion-cancel-modal" class="ia-modal-inline-trigger ia-discussion-cancel ia-button secondary ia-discussion-cancel-btn">Cancel</a>
            <%--<button class="ia-button secondary">Cancel</button>--%>
        </div>
        <!-- Delete Modal -->
        <div id="ia-discussion-cancel-modal" class="mfp-hide interAction ia-modal">
            <div class="ia-modal-upload">
                <h2 class="ia-modal-heading">Confirm Cancel</h2>
                <p>Are you sure you want to cancel? </p>

                <div class="ia-button-row">
                    <%--<a class="ia-button">OK</a>--%>
                    <a class="ia-button" id="btncanConfirm" onserverclick="cancelPostDiscussion_Click" runat="server">Yes</a>
                    <a class="ia-button secondary ia-modal-dismiss" href="#">No</a>
                </div>

            </div>
        </div>

    </section>
    <!-- end .ia-discussion-create-new -->
    <div class="ia-loading-panel ia-hide">

        <div class="ia-loader"></div>

    </div>

</div>


