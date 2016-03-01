<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="QuickLinks.ascx.cs" Inherits="Akumina.WebParts.QuickLinks.QuickLinks.QuickLinks" %>

<asp:Literal ID="__searchCrawlFeed" runat="server"></asp:Literal>

<script type="text/javascript">

    var styleCSS = "<asp:Literal ID="styleCSS" runat="server"/>";
    
    if (Akumina === undefined) {
        var Akumina = {};
    }
    if (Akumina.InterAction === undefined) {
        Akumina.InterAction = {};
    }


    function checkStyleSheet(url){
        var found = false;
        for(var i = 0; i < document.styleSheets.length; i++){
            if(document.styleSheets[i].href==url){
                found=true;
                break;
            }
        }
        if(!found){
            $('head').append(
                $('<link rel="stylesheet" type="text/css" href="' + url + '" />')
            );
        }
    }
    
    

    $(document).ready( function() {
        if(styleCSS !=""){
            checkStyleSheet(styleCSS);
        }        
    });

</script>

<asp:Literal ID="litTop" runat="server"></asp:Literal>
<div id="quickLinksTarget" runat="server"></div>

<script type="text/javascript">
    if (Akumina.InterAction.QuickLinks === undefined) {

        Akumina.InterAction.QuickLinks = function (targetDiv, resultTemplateJson, templateJson) {
            var me = this;
            this.targetDiv = targetDiv;
            this.resultTemplateJson = resultTemplateJson;
            this.templateJson = templateJson;

            var adTemplate = Mustache.to_html(this.templateJson[0], this.resultTemplateJson);
            if($(adTemplate)[2] != undefined){
                $(document).ready( function() {
                    $('.ak-col-template .ak-dept-nav').length >0
                    $('.ak-col-template .ak-dept-nav').append($('.interAction.flyOut'));
                    //If a top level li has a child UL, assign a class
                    $('.ia-dept-nav-menu ul > li:has(ul)').addClass('ia-dept-nav-submenu');
                    //$('<span class="fa fa-caret-down ia-dept-sublevel-trigger"></span>').insertBefore('.ia-dept-nav-menu ul > li:has(ul) > span');
                    $('.ia-dept-nav-menu ul > li:has(ul) > span').addClass('ia-dept-sublevel-trigger')
	
                    $('.ia-dept-nav-trigger').click(function(){
                        $('.ia-dept-nav-wrapper').toggleClass('ia-show-dept-nav');
                        $('.ia-dept-nav-wrapper').fadeToggle();
                    });

                    $('.ia-dept-nav-close').click(function(){
                        $('.ia-dept-nav-wrapper').removeClass('ia-show-dept-nav');
                        $('.ia-dept-nav-wrapper').fadeOut();
                    });

                    $('.ia-dept-sublevel-trigger').click(function(){
                        $(this).siblings('ul').slideToggle();
                        $(this).toggleClass('ia-dept-sublevel-active');
                    });

                });
            }

            $(this.targetDiv).html(adTemplate);
        
        };
    }
    var targetDiv = '.<asp:Literal ID="UniqueIdItem" runat="server"/>';
    var resultTemplateJson = <asp:Literal ID="QuickLinksItem" runat="server"/>;
    var templateJson = <asp:Literal ID="ItemListTemplate" runat="server"/>;
    var quickLinks = Akumina.InterAction.QuickLinks(targetDiv, resultTemplateJson, templateJson);  
</script>