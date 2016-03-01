<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DocumentRefiner.ascx.cs" Inherits="Akumina.WebParts.Documents.DocumentRefiner.DocumentRefiner" %>
<%--<asp:HiddenField ID="hdnPathQF" ClientIDMode="Static" runat="server" />
<asp:HiddenField ID="hdnQueryQF" ClientIDMode="Static" runat="server" />
<asp:HiddenField ID="hdnQuerystatus" ClientIDMode="Static" runat="server" />
<asp:HiddenField ID="searchTextQuery" ClientIDMode="Static" runat="server" />
<asp:HiddenField ID="refreshIdleRF" ClientIDMode="Static" runat="server" />--%>
<asp:HiddenField ID="hdnFilterToHide" runat="server" ClientIDMode="Static"/>
<div class="interAction">
    <div class="ia-accordion-filters">
        <asp:UpdatePanel ID="updateQueryPanel" UpdateMode="Conditional" runat="server">
            <ContentTemplate>
                <div class="queryZone">
                    <div class="ia-filter ia-filter-category-browse ia-accordion-item" id="category" runat="server">
                        <h3 class="ia-filter-header ia-accordion-header">Categories <span class="ia-accordion-icon ia-open"></span></h3>
                        <div class="ia-filter-body ia-accordion-body">
                            <div class="ia-filter-row">
                                <input type="text" class="ia-filter-input-small ia-category-input" readonly />
                                 <input type="text" id="selectedTaxonomyConfrirm" class="selectedTaxonomyConfrirm" style="display:none;" />
                                <a class="ia-button ia-modal-inline-trigger category_browse" href="#category-popup">Browse</a>
                             
                            </div>

                            <div id="category-popup" class="mfp-hide interAction ia-modal">
                                <div class="ia-modal-upload">
                                    <h2 class="ia-modal-heading">Browse Tags</h2>

                                    <div class="interAction refinerCategory">

                                        <div class="ia-treeview">
                                            <asp:Literal ID="ltlTaxonomyView" runat="server" EnableViewState="true" Text=""></asp:Literal>
                                         
                                            
                                        </div>
                                        <!-- Treeview -->
                                        <div class="ia-current-selected">Currently Selected:<span class="ia-current-selected-list"></span>
                                            <input type="text" id="selectedTaxonomy" class="selectedTaxonomy" style="visibility:hidden" />
                                        </div>

                                           

                                        <div class="ia-button-rows">
                                            <a class="ia-button ia-modal-dismiss ia-category-confirm">OK</a>
                                            <a class="ia-button secondary ia-modal-dismiss" href="#">Cancel</a>
                                        </div>
                                    </div>

                                </div>
                                <!-- ia-modal -->

                            </div>

                        </div>
                    </div>

           
                    <div class="ia-filter ia-filter-date ia-accordion-item datefield" id="dateField" runat="server">
                        <h3 class="ia-filter-header ia-accordion-header">Date
                            <span class="ia-accordion-icon"></span>
                        </h3>
                        <div class="ia-filter-body ia-accordion-body">
                            <div class="ia-filter-row">
                                <label class="ia-filter-label ia-filter-label-small">Start:</label>

                                <input type="text" id="txtStrDate" onchange="SetRefineQuery();" category="date" class="ia-filter-input-small ia-datepicker picker__input" />
                            </div>
                            <div class="ia-filter-row">
                                <label class="ia-filter-label ia-filter-label-small">End:</label>
                                <input type="text" id="txtEndDate" onchange="SetRefineQuery();" category="date" class="ia-filter-input-small ia-datepicker picker__input" />


                            </div>
                            <hr>
                            <ul>
                                <li>
                                    <a id="today" runat="server" onclick="datechange(this);">Today</a>



                                </li>
                                <li>
                                    <a id="lstweek" runat="server" onclick="datechange(this);">Last 7 days</a>

                                </li>

                                <li>
                                    <a id="month" runat="server" onclick="datechange(this);">Last 30 days</a>

                                </li>
                                <li>
                                    <a id="year" runat="server" onclick="datechange(this);">This Year</a>

                                </li>

                            </ul>
                        </div>
                    </div>

                    <asp:Repeater ID="rptCategory" runat="server">
                        <ItemTemplate>
                            <div class='ia-filter ia-accordion-item otherfield <%# Eval("ClassName") %>'>
                                <h3 class="ia-filter-header ia-accordion-header" title='<%# Eval("key") %>'>
                                    <%# Eval("key").ToString() == "Editor" ? "Modified By" : Eval("key").ToString().Replace("_x0020_", " ") %>
                                    <span class="ia-accordion-icon"></span></h3>
                                <div class="ia-filter-body ia-accordion-body">
                                </div>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
      
      
        <script type="text/javascript">
            var RefinerPrm = Sys.WebForms.PageRequestManager.getInstance();
            RefinerPrm.add_endRequest(EndRequest);

            function EndRequest(sender, args) {
                EndRequestRefiner();
            }
        </script>
    </div>
</div>


