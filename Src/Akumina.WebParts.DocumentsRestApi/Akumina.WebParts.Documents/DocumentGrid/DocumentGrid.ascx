<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DocumentGrid.ascx.cs" Inherits="Akumina.WebParts.Documents.DocumentGrid.DocumentGrid" %>
<%@ Import Namespace="Akumina.WebParts.Documents.Properties" %>

<div class="interAction">


    <div class="ia-loading-panel ia-hide">
        <div class="ia-loader"></div>
    </div>
    <div class="ia-search-documentList" style="overflow: visible">

        <p class="hintText" runat="server" id="singleDoumentBreadCrumb">
            <%= Resources.breadCrumpText %> "<label class="lblBreadCrum" id="lblBreadCrum" runat="server" />"
        </p>



        <div class="ia-button-row uploadCreateButtons" id="uploadCreateFolder" runat="server">
            <a class="ia-button ia-button-dropdown" href="#" id="btnCreate" data-dropdown="#dropdown-1"><%= Resources.btnCreate %></a>
            <div id="dropdown-1" class="dropdown dropdown-tip dropdown-relative">

                <ul class="dropdown-menu">
                    <li>
                        <a href="#" id="btnDoc" onclick=" fileCreateFolder(); "><span class="ia-doclib-file-icon fa fa-folder fac"></span><%= Resources.btnDoc %></a>
                    </li>
                    <li>
                        <a href="#" id="btnWordDoc" onclick=" fileCreateDocument(1); " type="button">
                            <img src="../_catalogs/_layouts/15/Akumina.WebParts.Documents/images/icons/icdocx.png" />
                            <%= Resources.btnWordDoc %>
                        </a>
                    </li>
                    <li>
                        <a href="#" id="btnExcelDoc" onclick=" fileCreateDocument(2); " type="button">
                            <img src="../_layouts/15/Akumina.WebParts.Documents/images/icons/icxlsx.png" />
                            <%= Resources.btnExcelDoc %>
                        </a>
                    </li>
                    <li>
                        <a href="#" id="btnPPTDoc" onclick=" fileCreateDocument(3); " type="button">
                            <img src="../_layouts/15/Akumina.WebParts.Documents/images/icons/icpptx.png" />
                            <%= Resources.btnPPTDoc %>
                        </a>
                    </li>
                    <li>
                        <a href="#" id="btnONoteDoc" onclick=" fileCreateDocument(4); " type="button">
                            <img src="../_layouts/15/Akumina.WebParts.Documents/images/icons/icnotebk.png" />
                            <%= Resources.btnONoteDoc %>
                        </a>
                    </li>

                </ul>
            </div>
            <a class="ia-button" href="#" id="btnUpload" onclick=" fileUpload(); "><%= Resources.btnUpload %></a>
            <a class="ia-button uploadButton" href="#" id="ia-library-upload" style="visibility: hidden">Upload</a>
        </div>

        <div id="searchZone" class="ia-search-library ia-search-library-full-width">
            <asp:UpdatePanel ID="searchPanel" UpdateMode="Always" runat="server">
                <ContentTemplate>

                    <div class="subsite-header" style="display: none;">
                        <div class="subsite-page">

                            <asp:Literal ID="ltlbreadcrumb" runat="server"></asp:Literal>
                        </div>
                    </div>
                    <span class="ia-searchInput fa fa-search"></span>
                    <asp:TextBox ID="txtSearch" ClientIDMode="Static" CssClass="ia-searchBox" onkeypress="detect_enter(event);" runat="server"></asp:TextBox>


                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <asp:HiddenField ID="hdnContextMenuOptions" runat="server" />
        <asp:HiddenField ID="hdnContextMenuMoreOptions" runat="server" />
        <asp:HiddenField ID="hdnRowlimit" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="webdocLibrary" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="selecteddocLibrary" runat="server" ClientIDMode="Static" />
        <div id="contextMenu" runat="server">
            <ul id="ulContextMenu" class="ia-button-group">
                <% if (hdnContextMenuOptions.Value.Split(';').Contains(Resources.btnCheckIn))
                   { %>
                <li title='<%=  Resources.btnCheckIn %>'>
                    <a id="btnCheckIn" onclick=" fileCheckIn(); " class="ia-button secondary" href="#"><%=  Resources.btnCheckIn %></a>
                </li>
                <%} %>
                <% if (hdnContextMenuOptions.Value.Split(';').Contains(Resources.btnOpen))
                   { %>
                <li>
                    <a id="btnOpen" onclick=" fileOpen(event); " class="ia-button secondary" href="#"><%= Resources.btnOpen %></a>
                </li>
                <%} %>
                <% if (hdnContextMenuOptions.Value.Split(';').Contains(Resources.btnCheckOutAll))
                   { %>
                <li>
                    <a id="btnCheckOutAll" onclick=" fileCheckOut(); " class="ia-button secondary" href="#"><%= Resources.btnCheckOutAll %></a>
                </li>
                <%} %>
                <% if (hdnContextMenuOptions.Value.Split(';').Contains(Resources.btnDiscardChkOutAll))
                   { %>
                <li>
                    <a id="btnDiscardChkOutAll" onclick="fileDisCheckOut(); " class="ia-button secondary" href="#"><%= Resources.btnDiscardChkOutAll %></a>
                </li>
                <%} %>
                <% if (hdnContextMenuOptions.Value.Split(';').Contains(Resources.btnDownload))
                   { %>
                <li>
                    <a id="btnDownload" onclick=" fileDownload(); " class="ia-button secondary" href="#"><%= Resources.btnDownload %></a>
                </li>
                <%} %>
                <% if (hdnContextMenuOptions.Value.Split(';').Contains(Resources.btnDelete))
                   { %>
                <li>

                    <a id="btnDelete" onclick=" fileDelete(); " class="ia-button secondary" href="#"><%= Resources.btnDelete %></a>
                </li>
                <%} %>
                <% if (hdnContextMenuOptions.Value.Split(';').Contains(Resources.btnMore))
                   { %>
                <li>
                    <a id="btnMore" class="ia-button secondary ia-button-dropdown" data-dropdown="#dropdown-2" href="#"><%= Resources.btnMore %></a>
                    <div class="dropdown dropdown-tip dropdown-relative" id="dropdown-2">
                        <ul class="dropdown-menu">

                            <% if (hdnContextMenuMoreOptions.Value.Split(';').Contains(Resources.btnCheckIn))
                               { %>
                            <li>
                                <a href="#" id="btnCheckInMore" onclick=" fileCheckIn(); "><%= Resources.btnCheckIn %></a>
                            </li>
                            <%} %>
                            <% if (hdnContextMenuMoreOptions.Value.Split(';').Contains(Resources.btnDiscardCheckOut))
                               { %>
                            <li>
                                <a href="#" id="btnDiscardCheckOut" onclick=" fileDisCheckOut(); "><%= Resources.btnDiscardCheckOut %></a>
                            </li>
                            <%} %>
                            <% if (hdnContextMenuMoreOptions.Value.Split(';').Contains(Resources.btnCheckOut))
                               { %>
                            <li>
                                <a href="#" id="btnCheckOut" onclick=" fileCheckOut(); "><%= Resources.btnCheckOut %></a>
                            </li>
                            <%} %>

                            <li class="dropdown-divider"></li>
                            <% if (hdnContextMenuMoreOptions.Value.Split(';').Contains(Resources.btnViewProperties))
                               { %>
                            <li>
                                <a href="#" id="btnViewProperties" onclick=" fileViewProperties(); "><%= Resources.btnViewProperties %></a>
                            </li>
                            <%} %>
                            <% if (hdnContextMenuMoreOptions.Value.Split(';').Contains(Resources.btnEditProperties))
                               { %>
                            <li>
                                <a href="#" id="btnEditProperties" onclick=" fileEditProperties(); "><%= Resources.btnEditProperties %></a>
                            </li>
                            <%} %>
                            <li class="dropdown-divider"></li>
                            <% if (hdnContextMenuMoreOptions.Value.Split(';').Contains(Resources.btnShare))
                               { %>
                            <li>
                                <a href="#" id="btnShare" onclick=" fileShare(); "><%= Resources.btnShare %></a>
                            </li>
                            <%} %>
                            <li class="dropdown-divider"></li>
                            <% if (hdnContextMenuMoreOptions.Value.Split(';').Contains(Resources.btnShareWith))
                               { %>
                            <li>
                                <a href="#" id="btnShareWith" onclick=" fileShareWith(); "><%= Resources.btnShareWith %></a>
                            </li>
                            <%} %>
                            <li class="dropdown-divider"></li>
                            <% if (hdnContextMenuMoreOptions.Value.Split(';').Contains(Resources.btnFollow))
                               { %>
                            <li>
                                <a href="#" id="btnFollow" onclick=" fileFollow(); "><%= Resources.btnFollow %></a>
                            </li>
                            <%} %>
                            <li class="dropdown-divider"></li>
                            <% if (hdnContextMenuMoreOptions.Value.Split(';').Contains(Resources.btnCompliance))
                               { %>
                            <li>
                                <a href="#" id="btnCompliance" onclick=" fileComplianceDetails(); "><%= Resources.btnCompliance %></a>
                            </li>
                            <%} %>
                            <% if (hdnContextMenuMoreOptions.Value.Split(';').Contains(Resources.btnWorkflow))
                               { %>
                            <li>
                                <a href="#" id="btnWorkflow" onclick=" fileWorkflow(); "><%= Resources.btnWorkflow %></a>
                            </li>
                            <%} %>
                        </ul>
                    </div>
                </li>
                <%} %>
            </ul>
        </div>

        <asp:HiddenField ID="displayUploadSuccess" ClientIDMode="Static" runat="server" />
        <asp:HiddenField ID="listDisplayName" ClientIDMode="Static" runat="server" />
        <asp:HiddenField ID="listName" ClientIDMode="Static" runat="server" />
        <asp:HiddenField ID="listGuidId" ClientIDMode="Static" runat="server" />
        <asp:HiddenField ID="currentFolderPath" ClientIDMode="Static" runat="server" />
        <asp:HiddenField ID="webUrl" ClientIDMode="Static" runat="server" />

        <asp:HiddenField ID="webServerUrl" ClientIDMode="Static" runat="server" />
        <asp:HiddenField ID="tabValue" ClientIDMode="Static" runat="server" />
        <asp:HiddenField ID="autoCurrentFolder" ClientIDMode="Static" runat="server" />
        <asp:HiddenField ID="createpermission" ClientIDMode="Static" runat="server" />
        <asp:HiddenField ID="akuminaCookieName" ClientIDMode="Static" runat="server" />
        <asp:HiddenField ID="refinerReset" ClientIDMode="Static" runat="server" />
        <asp:HiddenField ID="refreshIdleGrid" ClientIDMode="Static" runat="server" />
        <asp:HiddenField ID="lstForceCheckout" ClientIDMode="Static" runat="server" />
        <asp:HiddenField ID="hdnUploadedFiles" ClientIDMode="Static" runat="server" />
        <asp:HiddenField ID="columnNames" ClientIDMode="Static" runat="server" />
        <asp:HiddenField ID="styleUrl" ClientIDMode="Static" runat="server" />
        <asp:HiddenField ID="GridJsMode" ClientIDMode="Static" runat="server" />

        <div id="uploadMessage" style="display: none">
            <div class="ia-upload-success" id="uploadSucess">
                <p class="uploaded_Success" id="uploadStatus" runat="server"></p>
            </div>
        </div>
        <asp:UpdatePanel ID="updatepanel1" runat="server">
            <ContentTemplate>
                <asp:Button ID="viewSateReset" runat="server" CssClass="hdnbutton" ClientIDMode="Static" OnClick="viewSateReset_Click" />
                <div class="serverSideMessage" style="display: none">
                    <asp:Literal ID="ltlUploadSuccess" runat="server"></asp:Literal>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>


        <asp:HiddenField ID="hdnColumnsToHide" runat="server" ClientIDMode="Static" />
        <div id="drop_zone" class="ia-documentList">
            <div class="ia-document-library" id="mainGrid">
            </div>

            <div id="noresults" style="display: none"><%= Resources.Gird_EmptyDataText %></div>
        </div>
        <asp:Literal ID="ltlDms_Template" runat="server"></asp:Literal>

        <div id="document-preview-1" class="mfp-hide interAction ia-modal-preview">
            <div class="currentListId" style="display: none"></div>
            <div class="currentListItemId" style="display: none"></div>
            <div class="ia-preview-document">
            </div>
            <div class="ia-preview-details">
                <h1 class="ia-doc-title"></h1>
                <p class="ia-doc-modified"></p>
                <p class="ia-doc-shared">Shared with <a href="#">lots of people</a></p>
                <div class="ia-doc-link">
                    <button class="ia-button secondary" onclick="CopyToClipboard('ia-Doc-PrevLink');">Copy Link</button>
                    <input id="ia-Doc-PrevLink" type="text" />
                </div>
                <ul class="ia-button-group1">
                    <li style="display: inline-block; list-style: outside none none;"><a href="#" class="ia-button secondary" onclick="fileOpenPreview();"><%= Resources.btnOpen %></a></li>
                    <li style="display: inline-block; list-style: outside none none;"><a href="#" class="ia-button secondary" onclick="fileSharePreview();"><%= Resources.preview_Share %></a></li>
                    <li style="display: inline-block; list-style: outside none none;"><a href="#" class="ia-button secondary" onclick="fileFollowPreview();"><%= Resources.preview_Follow %></a></li>
                </ul>
            </div>
        </div>

        <script type="text/javascript">
            var prm = Sys.WebForms.PageRequestManager.getInstance();
            prm.add_endRequest(EndRequest);

            function EndRequest(sender, args) {
                if ($("#refreshIdleGrid").val() != "true")
                    EndRequestGrid();

                $('.ia-sort-asc').click(function () {
                    showloader();
                });
            }
        </script>
        <asp:HiddenField ID="hdnDefaultTaxonomyField" ClientIDMode="Static" runat="server" />
        <div class="progress"></div>

        <div class="interAction mfp-hide ia-modal ia-library-upload ia-step1" id="requiredFields" runat="server" visible="false">
            <div class="ia-library-upload-reqs">
                <h2 class="ia-modal-heading">Upload Files</h2>
                <p>Please complete the following required fields.  All files being added will be tagged with this information. </p>
                <p>If you want to tag files with unique information, click “Cancel” and add the files individually</p>
                <div class="ia-form-error fieldValidate-error">
                    <p>Please correct the following errors and try again.</p>
                </div>
                <div class="ia-form">
                    <!-- NOTE: only add the class 'ia-field-error' class when a field is invalid -->
                    <asp:UpdatePanel ID="updateRequireFields" runat="server" UpdateMode="Always">
                        <ContentTemplate>
                            <asp:Panel ID="pnlRequiredFields" CssClass="pnlRequiredFields" runat="server"></asp:Panel>

                        </ContentTemplate>
                    </asp:UpdatePanel>

                    <div class="ia-form-row startDate-error" id="setExpireStartDate" runat="server" visible="false">
                        <label class="ia-form-label">* Start Date:</label>
                        <input type="text" runat="server" id="setFileExpireStart" class="ia-datepicker setStartDate" />
                    </div>

                    <div class="ia-form-row expire-error" id="setExpireDate" runat="server" visible="false">
                        <label class="ia-form-label">* Expiration Date:</label>
                        <input type="text" runat="server" id="setFileExpire" class="ia-datepicker setExpireDate" />
                    </div>
                    <div class="ia-form-row category-error" id="setCategory" runat="server" visible="false">

                        <label class="ia-form-label">* Categories:</label>
                        <input type="text" class="ia-upload-category-input" readonly />
                        <a class="ia-button ia-modal-inline-trigger" href="#upload-category-popup">Browse</a>


                    </div>

                </div>
                <%--Category Taxonomy--%>
                <div id="upload-category-popup" class="mfp-hide interAction ia-modal">
                    <div class="ia-modal-upload">
                        <h2 class="ia-modal-heading">Browse Tags</h2>

                        <div class="interAction gridCategory">
                            <div class="ax-treeview"></div>

                            <!-- Treeview -->
                            <div class="ia-current-selected">
                                Currently Selected:<span class="ia-current-selected-list"></span>
                                <input type="text" id="hdnAssigenedTax" runat="server" class="selectedTaxonomy" style="visibility: hidden" />


                            </div>

                            <div class="ia-button-row">
                                <a class="ia-button ia-upload-category-confirm">OK</a>
                                <a class="ia-button secondary ia-upload-category-cancel" href="#">Cancel</a>
                            </div>

                        </div>
                        <!-- InterAction-->
                    </div>
                    <!-- ia-modal-upload-->

                </div>
                <!-- ia-modal -->

                <!-- ia-form-->
                <div class="ia-button-row">
                    <a class="ia-button ia-upload-confirm">Next</a>
                    <a class="ia-button secondary ia-upload-cancel" href="#">Cancel</a>
                </div>
            </div>
            <!-- ia-library-upload-reqs -->

        </div>
        <div class="ax-category-tree" style="display: none;">
            <asp:Literal ID="ltlTaxonomyView" runat="server" Text=""></asp:Literal>
        </div>
        <!-- interAction ia-step1 -->
        <asp:HiddenField ID="hdnMinorCheckInEnable" ClientIDMode="Static" runat="server" />
        <div class="interAction mfp-hide ia-modal ia-fileCheckInOption">
            <div class="ia-library-upload-checkin">
                <h2 class="ia-modal-heading">Checkin</h2>
                <p>Do you want to check-in these files? Your changes will not be visible to others until they are checked in.</p>
                <%--     <div class="ia-form-error">
                        <p>Please select a checkin option and try again.</p>
                    </div>--%>
                <div class="ia-form">
                    <div class="ia-form-row">
                        <label class="ia-form-label">
                            <input type="radio" name="checkInFileOption" checked="" runat="server" id="checkInFileOption" class="ia-checkin checkInFileOption" />
                            Check-in the files</label>
                    </div>
                    <div class="ia-checkin-all">
                        <div class="ia-form-row">
                            <label class="ia-form-label">
                                <input type="radio" runat="server" checked="" id="checkInDraftOption" name="version-checkinOption" class="checkInDraftOption" />
                                Minor version (draft)</label>
                        </div>
                        <div class="ia-form-row">
                            <label class="ia-form-label">
                                <input type="radio" runat="server" id="checkInPublishOption" name="version-checkinOption" class="checkInPublishOption" />
                                Major version (publish)</label>
                        </div>
                        <div class="ia-form-row">
                            <label class="ia-form-label">Comment for check-in: (optional)</label>
                            <textarea id="checkinCommentOption" class="checkinCommentOption ax-checkincomment" runat="server"></textarea>
                        </div>
                    </div>
                    <div class="ia-form-row">
                        <label class="ia-form-label">
                            <input type="radio" name="checkInCheckOutFileOption" runat="server" id="checkInCheckOutFileOption" class="ia-checkin-checkout checkInCheckOutFileOption" />
                            Check-in the changes but leave the files checked out to me</label>
                    </div>
                    <div class="ia-checkin-changes">
                        <div class="ia-form-row">
                            <label class="ia-form-label">
                                <input type="radio" runat="server" checked="" id="checkInCheckoutDraftOption" name="version-checkchangeoption" class="checkInCheckoutDraftOption" />
                                Minor version (draft)</label>
                        </div>
                        <div class="ia-form-row">
                            <label class="ia-form-label">
                                <input type="radio" runat="server" id="checkInCheckOutPublishOption" name="version-checkchangeoption" class="checkInCheckOutPublishOption" />
                                Major version (publish)</label>
                        </div>
                        <div class="ia-form-row">
                            <label class="ia-form-label">Comment for check-in: (optional)</label>
                            <textarea id="checkInCheckOutCommentOption" class="checkInCheckOutCommentOption ax-checkincomment" runat="server"></textarea>
                        </div>
                    </div>

                </div>
                <div class="ia-button-row">
                    <a class="ia-button ia-checkin-confirmOption" href="#" onclick="fileCheckinWithOptions();">Done</a>
                    <a class="ia-button secondary ia-checkin-cancel" href="#" onclick="clearModal();">Cancel</a>
                </div>

            </div>
        </div>
        <div class="interAction mfp-hide ia-modal ia-library-upload ia-step2">
            <div class="ia-library-upload-checkin">
                <h2 class="ia-modal-heading">Checkin</h2>
                <p>Do you want to check-in these files? Your changes will not be visible to others until they are checked in.</p>

                <div class="ia-form">
                    <div class="ia-form-row">
                        <label class="ia-form-label">
                            <input type="radio" name="checkin" runat="server" id="checkInFile" class="ia-checkin" />
                            Check-in the files</label>
                    </div>
                    <div class="ia-checkin-all">
                        <div class="ia-form-row">
                            <label class="ia-form-label">
                                <input type="radio" runat="server" checked="" id="checkInDraft" name="version-checkin" class="checkInDraft" />
                                Minor version (draft)</label>
                        </div>
                        <div class="ia-form-row">
                            <label class="ia-form-label">
                                <input type="radio" runat="server" id="checkInPublish" name="version-checkin" />
                                Major version (publish)</label>
                        </div>
                        <div class="ia-form-row">
                            <label class="ia-form-label">Comment for check-in: (optional)</label>
                            <textarea id="checkinComment" class="checkinComment ax-checkincomment" runat="server"></textarea>
                        </div>
                    </div>
                    <div class="ia-form-row">
                        <label class="ia-form-label">
                            <input type="radio" name="checkin" runat="server" id="checkInCheckOutFile" class="ia-checkin-checkout" />
                            Check-in the changes but leave the files checked out to me</label>
                    </div>
                    <div class="ia-checkin-changes">
                        <div class="ia-form-row">
                            <label class="ia-form-label">
                                <input type="radio" runat="server" checked="" id="checkInCheckoutDraft" name="version-checkchange" class="checkInCheckoutDraft" />
                                Minor version (draft)</label>
                        </div>
                        <div class="ia-form-row">
                            <label class="ia-form-label">
                                <input type="radio" runat="server" id="checkInCheckOutPublish" name="version-checkchange" />
                                Major version (publish)</label>
                        </div>
                        <div class="ia-form-row">
                            <label class="ia-form-label">Comment for check-in: (optional)</label>
                            <textarea id="checkInCheckOutComment" class="checkInCheckOutComment ax-checkincomment" runat="server"></textarea>
                        </div>
                    </div>
                    <div class="ia-form-row">
                        <label class="ia-form-label">
                            <input type="radio" runat="server" checked="" name="checkin" id="dontCheckIn" class="ia-no-checkin" />
                            Do not check-in the files</label>
                    </div>
                </div>
                <div class="ia-button-row">
                    <a class="ia-button ia-checkin-confirm">Next</a>
                    <a class="ia-button secondary ia-checkin-cancel" href="#">Cancel</a>
                </div>

            </div>
        </div>

        <div class="interAction mfp-hide ia-modal ia-library-upload ia-step3">
            <div class="ia-library-upload-process">
                <h2 class="ia-modal-heading">Uploading...</h2>
                <div class="ia-progress-meter">
                    <div class="ia-progress">
                        <!-- this inline style width percentage should be updated as the progress moves along -->
                        <div class="ia-progress-percent" style="width: 25%;"></div>
                    </div>
                    <!-- this value should be updated as the progress moves along -->
                    <div class="ia-progress-text">25%</div>
                </div>
                <div class='progressWrapper' style='float: left; width: 100%'>
                    <div class='progress' style='float: left; width: 0%; color: red'></div>
                    <div class='progressText' style='float: left;'></div>
                </div>
            </div>
        </div>
        <div class="interAction mfp-hide ia-modal ia-fileCheckInMajorOption">
            <div class="ia-library-upload-checkin">
                <h2 class="ia-modal-heading">Check in</h2>
                <!-- <p>Check in</p> -->
                <div class="ia-form">
                    <div class="ia-form-row">
                        <label class="ia-form-label">
                            <input type="radio" name="checkInMajorFileOption" checked="" runat="server" id="checkInMajorFileOption" class="checkInMajorFileOption" />
                            Retain your check out after checking in?</label>
                    </div>

                    <div style="padding-left: 2%">
                        <div class="ia-form-row">
                            <label class="ia-form-label">
                                <input type="radio" runat="server" id="RadioMajor1" class="fileCheckinYes" name="version-RadioMajorOption" />
                                Yes</label>
                        </div>
                        <div class="ia-form-row">
                            <label class="ia-form-label">
                                <input type="radio" runat="server" checked="" id="RadioMajor2" class="fileCheckinNo" name="version-RadioMajorOption" />
                                No</label>
                        </div>
                    </div>

                    <div class="ia-form-row">
                        <label class="ia-form-label">Comments:</label>
                        <textarea id="checkInMajorCommentOption" class="checkInMajorCommentOption ax-checkincomment" runat="server"></textarea>
                    </div>
                </div>

            </div>
            <div class="ia-button-row">
                <a class="ia-button ia-checkin-confirmOption" href="#" onclick="fileCheckinMajorOption();">Done</a>
                <a class="ia-button secondary ia-checkin-cancel" href="#">Cancel</a>
            </div>

        </div>
        <div class="interAction mfp-hide ia-modal dd_fileCheckInMajorOption" id="dd_fileCheckInMajorOption" runat="server" visible="false">
            <div class="ia-library-upload-checkin">
                <h2 class="ia-modal-heading">Check in</h2>
                <!-- <p>Check in</p> -->
                <div class="ia-form">
                    <div class="ia-form-row">
                        <label class="ia-form-label">
                            <input type="radio" name="dd_checkInMajorFileOption" checked="" runat="server" id="Radio1" class="ia-checkin dd_checkInMajorFileOption" />
                            Retain your check out after checking in?</label>
                    </div>

                    <div style="padding-left: 2%">
                        <div class="ia-form-row">
                            <label class="ia-form-label">
                                <input type="radio" runat="server" id="dd_fileCheckinYes" class="dd_fileCheckinYes" name="version-RadioMajor" />
                                Yes</label>
                        </div>
                        <div class="ia-form-row">
                            <label class="ia-form-label">
                                <input type="radio" runat="server" checked="" id="dd_fileCheckinNo" class="dd_fileCheckinNo" name="version-RadioMajor" />
                                No</label>
                        </div>
                    </div>

                    <div class="ia-form-row">
                        <label class="ia-form-label">Comments:</label>
                        <textarea id="dd_checkincomment" class="dd_checkincomment ax-checkincomment" runat="server"></textarea>
                    </div>
                </div>

            </div>
            <div class="ia-button-row">
                <a class="ia-button ia-checkin-confirmOption" href="#" onclick="MajorCheckinWindow(event);">Done</a>
                <a class="ia-button secondary ia-checkin-cancel" href="#">Cancel</a>
            </div>

        </div>

        <div class="interAction mfp-hide ia-modal ia-library-multipleupload">
            <div class="ia-library-upload-process">
                <h2 class="ia-modal-heading">A file with the same name already exists</h2>
                <div>
                    <p>
                        A file named '<span class="currentUploadFileName"></span>' already exists in this library. What would you like to do?
                    </p>
                    <div>


                        <div class="divConflictCheck">
                            <input type="checkbox" class="conflictDlg"><label for="conflictDlg">Do this for the next '<span class="remainingFiles"></span>' conflicts</label>
                        </div>
                        <div class="ms-core-form-bottomButtonBox">
                            <button id="ms-conflictDlgReplaceBtn" onclick="CallReplaceIt(event)" href="#">Replace It</button>
                            <button id="ms-conflictDlgNoUploadBtn" onclick="RejectReplaceIt(event)" href="#">Don't Upload</button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="interAction mfp-hide ia-modal ia-library-fileDelete" style="min-height: 10%;">
            <div class="ia-library-upload-process">
                <h2 class="ia-modal-heading">File Delete</h2>
                <div>
                    <p>
                        Are you sure to want to Delete?
                    </p>
                    <div>


                        <div class="ms-core-form-bottomButtonBox">
                            <button id="confirmDelete" onclick="ConfirmDelete(event)" href="#">Ok</button>
                            <button id="cancelDelete" onclick="CancelDelete(event)" href="#">Cancel</button>
                        </div>
                    </div>
                </div>
            </div>
        </div>

    </div>
