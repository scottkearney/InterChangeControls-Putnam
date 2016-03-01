<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ContentBlock.ascx.cs" Inherits="Akumina.WebParts.ContentBlock.ContentBlock.ContentBlock" %>
<asp:Literal ID="__searchCrawlFeed" runat="server"></asp:Literal>
<div id="<asp:Literal ID="ContentListItemId" runat="server"/>"></div>
<script type="text/javascript">
    var ContentBlock = function () {
        this.template =<asp:Literal ID="ContentListItemTemplate" runat="server" Text="{}"/>
        this.item =<asp:Literal ID="ContentListItem" runat="server" Text="{}"/>
    }
    ContentBlock.prototype.loadItem = function () {
        if (typeof Mustache != 'undefined'){
            var v = Mustache.to_html(this.template[0], this.item);        
            var elementTarget = document.getElementById(this.item.UniqueId);
            if (elementTarget != null) {
                elementTarget.insertAdjacentHTML('afterbegin', v);
            }
        }
    }
    
    var contentBlock = new ContentBlock();
    contentBlock.loadItem();

    function adjustHeight(){   
        if(typeof($) != "undefined"){
            if( $('.ia-banner-slides') != null && $('.welcome-message .ia-content-block') != null ){
                $('.welcome-message .ia-content-block').css('height', $('.ia-banner-slides').height() + 'px');
            }
        }
    }
    if (_spBodyOnLoadFunctionNames) {
        _spBodyOnLoadFunctionNames.push('adjustHeight');
    }
</script>