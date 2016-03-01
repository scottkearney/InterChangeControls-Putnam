<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DiscussionSummary.ascx.cs" Inherits="Akumina.WebParts.DiscussionBoard.DiscussionSummary.DiscussionSummary" %>

<!-- CSS -->
<%--<link href="../_layouts/15/Akumina.WebParts.DiscussionBoard/css/ia-interaction-controls.css" rel="stylesheet" />
<!-- JS -->
<script src="../_layouts/15/Akumina.WebParts.DiscussionBoard/js/vendor/jquery-2.1.1.min.js"></script>
<script src="../_layouts/15/Akumina.WebParts.DiscussionBoard/js/vendor/modernizr.js"></script>--%>
<asp:Literal runat="server" ID="litTop" >
</asp:Literal>
<div class="interAction">
            <asp:Literal ID="errorLiteralDBSummary" runat="server"></asp:Literal>
            <div class="ia-control-header" id="discussionSummaryHeader" runat="server">
            <span class="ia-control-header-icon fa fa-<asp:Literal ID="WebPartIcon" runat="server"/>"></span>
            <h3 class="ia-control-header-heading"><asp:Literal ID="WebPartTitle" runat="server"/></h3>
            </div>
        <div class="ia-discussion-summary-wrapper">
            <asp:Repeater ID="repeaterDBSummary" runat="server">
                <ItemTemplate>                    
                    <!-- Discussion Summary Post -->
                    <div class="ia-discussion-summary ia-discussion-board">
                        <div class="ia-discussion-summary-wrapper">
                        <div class="ia-discussion-author-icon">
                            <%--<img src="content-images/discussion1.png" />--%>
                            <img src="<%# Eval("AuthorPicture") %>" />
                        </div>

                        <div class="ia-discussion-post-info">
                            <span class="ia-discussion-author-name"><%# Eval("Author") %></span>
                            <span class="ia-discussion-postname"><a href="<%#Eval("EditDiscussionPost") %>"><%# Eval("Title") %></a></span> 
                            <span class="ia-discussion-post-summary">&nbsp;&nbsp;<%# Eval("Body") %>
                            </span>
                            <div class="ia-discussion-post-time"><%# (int)(DateTime.Now - Convert.ToDateTime(Eval("Created"))).TotalDays==0?(int)(DateTime.Now - Convert.ToDateTime(Eval("Created"))).TotalHours>0?(int)(DateTime.Now - Convert.ToDateTime(Eval("Created"))).TotalHours==1?(int)(DateTime.Now - Convert.ToDateTime(Eval("Created"))).TotalHours +" Hour ago":(int)(DateTime.Now - Convert.ToDateTime(Eval("Created"))).TotalHours+" hours ago":(int)(DateTime.Now - Convert.ToDateTime(Eval("Created"))).TotalMinutes>0?(int)(DateTime.Now - Convert.ToDateTime(Eval("Created"))).TotalMinutes==1?(int)(DateTime.Now - Convert.ToDateTime(Eval("Created"))).TotalMinutes+" Minute ago":(int)(DateTime.Now - Convert.ToDateTime(Eval("Created"))).TotalMinutes+" Minutes ago":(int)(DateTime.Now - Convert.ToDateTime(Eval("Created"))).TotalSeconds+" Seconds ago":DataBinder.Eval(Container.DataItem, "Created", "{0:MMM. dd,yyyy}")%></div>

                        </div>
                    </div>
                    </div>

                </ItemTemplate>
            </asp:Repeater>

            <div class="ia-discussion-summary-see-all"><a href="" id="SeeallDiscussion" onserverclick="SeeallDiscussion_ServerClick" runat="server">See all Discussions <span class="fa fa-angle-right"></span></a></div>

        </div>
        
    <%--<div class="ia-discussion-summary ia-discussion-board">
        <div class="ia-discussion-author-icon">
            <img src="content-images/discussion1.png" />
        </div>

        <div class="ia-discussion-post-info">
            <span class="ia-discussion-author-name">David Smith</span>
            <span class="ia-discussion-postname"><a href="">Marketing Plan Updated</a></span>
            <span class="ia-discussion-post-summary">Lorem ipsum dolor sit amet, consectetur adipisicing elit. Consequatur reiciendis nam perferendis esse veritatis suscipit at, quidem nisi dignissimos! Consequuntur inventore, culpa fugit deleniti sunt perspiciatis rem atque eos laboriosam.
            </span>
            <div class="ia-discussion-post-time">2 hours ago</div>

        </div>
    </div>--%>

    
</div>
<!-- end .interAction -->
