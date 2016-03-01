<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Banner.ascx.cs" Inherits="Akumina.WebParts.Banner.Banner.Banner" %>
<asp:Literal ID="__searchCrawlFeed" runat="server"></asp:Literal>
<script type="text/javascript">
    if (Akumina === undefined) {
        var Akumina = {};
    }
    if (Akumina.InterAction === undefined) {
        Akumina.InterAction = {};
    }
</script>

<asp:Literal ID="litTop" runat="server"></asp:Literal>

<div id="div<asp:Literal ID="litDivID" runat="server">
    </asp:Literal>" class="interAction">
    
</div>
<input type="hidden" id="_ispostback" value="<%=Page.IsPostBack.ToString()%>" />
<script type="text/javascript">
    
    if (Akumina.InterAction.Banner === undefined) {
      
        Akumina.InterAction.Banner = function() {
            this.targetDiv = "";
            this.uniqueId = "";
            this.searchResults = {};
            this.itemsResult  = {};
            this.templateJson = {};
            this.webPartTheme = "";
            this.bannerResultSource = "";
            this.linkButtonTheme = "";
            this.linkButtonTextTheme = "";
            this.slideDuration = 3000;
            this.transitionEffect = "fade";
            this.showNavigator = true;
            this.autoPlay = true;
            this.infiniteLoop = true;
            this.MaxSlideCount = -1;

            this.getSearchResultsUsingREST = function(me) {
                var results = me.itemsResult;

                if (results.length == 0) {
                    $(me.targetDiv).html("");
                } else {
                    var heroTemplate = me.templateJson.ControlTemplate;
                    var liTemplate = me.templateJson.ItemTemplate;
                    var heroContainer = $(heroTemplate);
                    $(me.targetDiv).append(heroContainer);
                    var tileItems = [];
                    results = results.sort(function(obj1, obj2) {                       
                        return obj1.BannerItemOrderOWSNMBR - obj2.BannerItemOrderOWSNMBR;
                    });

                    $.each(results, function(index, result) {
                        var resultJson = {};
                        resultJson.BannerHeadingOWSTEXT = result.BannerHeadingOWSTEXT;
                        if(typeof me.H1MaxCharacters != 'undefined' && me.H1MaxCharacters > 0) {
                            if(resultJson.BannerHeadingOWSTEXT !=null)
                                resultJson.BannerHeadingOWSTEXT = resultJson.BannerHeadingOWSTEXT.substring(0, me.H1MaxCharacters);
                        }
                        resultJson.BannerImageAltTextOWSTEXT = result.BannerImageAltTextOWSTEXT;
                        resultJson.BannerImageOWSURLH = result.BannerImageOWSURLH;
                        var imgUrl = new String(resultJson.BannerImageOWSURLH);
                        if (imgUrl.indexOf(",") > -1) {
                            imgUrl = imgUrl.substring(0, imgUrl.indexOf(","));
                        }
                        resultJson.BannerImageOWSURLH = imgUrl;

                        resultJson.BannerItemOrderOWSNMBR = result.BannerItemOrderOWSNMBR;
                        resultJson.BannerLinkHoverTextOWSTEXT = result.BannerLinkHoverTextOWSTEXT;
                        resultJson.BannerLinkTargetOWSCHCS = result.BannerLinkTargetOWSCHCS;
                        if (resultJson.BannerLinkTargetOWSCHCS == "New Window") {
                            resultJson.BannerLinkTargetOWSCHCS = "_blank";
                        } else {
                            resultJson.BannerLinkTargetOWSCHCS = "_self";
                        }
                        resultJson.BannerLinkTextOWSTEXT = result.BannerLinkTextOWSTEXT;
                        if(typeof me.ButtonMaxCharacters != 'undefined' && me.ButtonMaxCharacters > 0) {
                            if(resultJson.BannerLinkTextOWSTEXT !=null)
                                resultJson.BannerLinkTextOWSTEXT = resultJson.BannerLinkTextOWSTEXT.substring(0, me.ButtonMaxCharacters);
                        }
                        resultJson.BannerSubHeadingOWSTEXT = result.BannerSubHeadingOWSTEXT;
                        if(typeof me.H2MaxCharacters != 'undefined' && me.H2MaxCharacters > 0) {
                            if(resultJson.BannerSubHeadingOWSTEXT !=null)
                                resultJson.BannerSubHeadingOWSTEXT = resultJson.BannerSubHeadingOWSTEXT.substring(0, me.H2MaxCharacters);
                        }
                        resultJson.BannerLinkUrlOWSURLH = result.BannerLinkUrlOWSURLH;
                        var href = new String(resultJson.BannerLinkUrlOWSURLH);
                        if (href.indexOf(",") > -1) {
                            href = href.substring(0, href.indexOf(","));
                        }
                        resultJson.BannerLinkUrlOWSURLH = href;

                        if (typeof me.linkButtonTheme != "undefined") resultJson.LinkButtonTheme = me.linkButtonTheme;
                        else resultJson.LinkButtonTheme = " ";
                        if (typeof me.linkButtonTextTheme != "undefined") resultJson.LinkButtonTextTheme = me.linkButtonTextTheme;
                        else resultJson.LinkButtonTextTheme = " ";
                        var li = liTemplate;
                        li = Mustache.to_html(liTemplate, resultJson);
                        $(".ia-banner-slides", me.targetDiv).append(li);
                            if(me.DisplayTiles != true) {
                            if(typeof me.TextLocation != 'undefined') {
                                $(".ia-banner-content", me.targetDiv).addClass(me.TextLocation);
                            }
                            if(typeof me.TextColorTheme != 'undefined') {
                                $(".ia-banner-content", me.targetDiv).addClass(me.TextColorTheme);
                            }
                            if(typeof me.TextAlignment != 'undefined') {
                                $(".ia-banner-content", me.targetDiv).addClass(me.TextAlignment);
                            }
                        }
                        if (index == 0)
                        {
                            resultJson.tileClass = 'tile-main';
                        }
                        else 
                        {
                            resultJson.tileClass = 'tile-sub' + index.toString();
                        }
                        tileItems.push(resultJson);
                    });
                    if($('.ia-banner-item').length <2)
                    {
                        me.autoPlay = false;
                        me.showNavigator= false;                        
                    }
                    if(me.DisplayTiles == true) {
                        var tileTemplate =  me.templateJson.TileItemTemplate;
                        var tileJSON = {
                            Items: tileItems
                        }
                        var tileHTML = Mustache.to_html(tileTemplate, tileJSON);

                        $(me.targetDiv).html(tileHTML);

                        //$('.ia-banner-nav').remove();
                        //$('.ia-banner-dots').remove();
                        //$('.ia-banner-pause').remove();

                        //$('.ia-banner-item').wrapAll( "<div/>");
                        //$('.ia-banner-item').wrapAll( "<div/>");

                        //var firstItem = $('.ia-banner-item').first();
                        //$(firstItem).wrap( "<div></div>" );
                        //var firstItemDiv = $(firstItem).parent();
                        //var secondItem = $('.ia-banner-item')[1];
                        //$(secondItem).wrap( "<div></div>" );
                        //var secondItemmDiv = $(secondItem).parent();
                        //var thirdItem = $('.ia-banner-item')[2];
                        //$(thirdItem).wrap( "<div></div>" );
                        //var thirdItemDiv = $(thirdItem).parent();
                        //var thirdWidth = (parseInt($('.ia-banner').width() / 3)) - 1;

                        //$(firstItemDiv).css( "width", (thirdWidth * 2).toString() + "px" );
                        ////$(firstItemDiv).css( "height", "457px" );
                        //$(firstItemDiv).css( "float", "left" );
                        //var firstItemContent = $(firstItemDiv).find('.ia-banner-content');
                        //$(firstItemContent).removeClass('ia-bottom-right');
                        //$(firstItemContent).removeClass('ia-text-right');
                        //$(firstItemContent).addClass('ia-bottom-left');
                        //$(firstItemContent).addClass('ia-text-left');
                        //$(firstItemContent).attr( "style" , "width:" + (thirdWidth * 2).toString() + "px !important;");
                        //$(firstItemContent).find('a').remove();

                        //$(secondItemmDiv).css( "width", (thirdWidth).toString() + "px" );
                        ////$(secondItemmDiv).css( "height", "228px" );
                        //$(secondItemmDiv).css( "float", "right" );
                        //var secondItemContent = $(secondItemmDiv).find('.ia-banner-content');
                        //$(secondItemContent).removeClass('ia-bottom-right');
                        //$(secondItemContent).removeClass('ia-text-right');
                        //$(secondItemContent).addClass('ia-top-right');
                        //$(secondItemContent).addClass('ia-text-right');
                        //$(secondItemContent).attr( "style" , "width:" + (thirdWidth).toString() + "px !important;");
                        //$(secondItemContent).find('a').remove();

                        //$(thirdItemDiv).css( "width", (thirdWidth).toString() + "px" );
                        ////$(thirdItemDiv).css( "height", "228px" );
                        //$(thirdItemDiv).css( "float", "right" );
                        //var thirdItemContent = $(thirdItemDiv).find('.ia-banner-content');
                        //$(thirdItemContent).removeClass('ia-bottom-right');
                        //$(thirdItemContent).removeClass('ia-text-right');
                        //$(thirdItemContent).addClass('ia-bottom-right');
                        //$(thirdItemContent).addClass('ia-text-right');
                        //$(thirdItemContent).attr( "style" , "width:" + (thirdWidth).toString() + "px !important;");
                        //$(thirdItemContent).find('a').remove();

                        //$('.ia-banner-slides').removeClass('owl-carousel');
                        
                        //$('.ia-banner-content h1:empty').hide();
                        //$('.ia-banner-content h2:empty').hide();
                    } else {
                        $(".ia-banner-slides", me.targetDiv).owlCarousel({
                            autoplayHoverPause: false, // pause on mouse hover
                            smartSpeed: 500, // slide transition speed  	
                            dotsEach: true, // show a dot for each slide item
                            navContainer: ".ia-banner-nav",
                            dotsContainer: ".ia-banner-dots",
                            navText: ["<span class=\"ia-banner-prev fa fa-chevron-left\"></span>", "<span class=\"ia-banner-next fa fa-chevron-right\"></span>"],
                            items: 1, // Number of items to show on screen
                            autoplaySpeed: 500, // slide transition speed when autoplay = true
                            mouseDrag: ($('.ia-banner-item').length <2 ? false : true),
                            singleItem: ($('.ia-banner-item').length <2 ? true : false),
                            //Values to be set by InterAction Banner Instruction Set:
                            loop: (typeof me.infiniteLoop != "undefined" ? me.infiniteLoop : false), // Inifnity loop. Duplicate last and first items to get loop illusion
                            autoplay: (typeof me.autoPlay != "undefined" ? me.autoPlay : false), // start slider rotation on page load
                            autoplayTimeout: (typeof me.slideDuration != "undefined" ? me.slideDuration : 5000), // slide duration when autoplay = true
                            nav: me.showNavigator, // show prev and next links
                            dots: me.showNavigator, // show dot navigation
                            //For Fade transition, the following two must be set. For slide, neither should be set                        
                            animateOut: (typeof me.transitionEffect != "undefined" ? (me.transitionEffect.toLowerCase() == "fade" ? "fadeOut" : me.transitionEffect.toLowerCase() =="vertical" ? "owl-goDown-out" : false) : false),
                            animateIn: (typeof me.transitionEffect != "undefined" ? (me.transitionEffect.toLowerCase() == "fade" ? "fadeIn" :  me.transitionEffect.toLowerCase() =="vertical" ? "owl-goDown-in" : false) : false),
  	
                        });
                        if(me.autoPlay != false){
                            var owl = $('.ia-banner-slides');
                            $('.ia-banner-play-control').click(function(){
                                owl.trigger('play.owl.autoplay',[1000]);
                            });
                            $('.ia-banner-pause-control').click(function(){
                                owl.trigger('stop.owl.autoplay');
                            });
                        }
                        else
                        {
                            $('.ia-banner-pause').hide();
                        }

                        $('.ia-banner-content span:empty').parent().hide();
                    }
                }
            };
          
        };
    }

    
            

    
        $.ajaxSetup({
            cache: true
        });

        var thisBanner = new Akumina.InterAction.Banner();
        thisBanner.uniqueId = '<asp:Literal ID="litUniqueId" runat="server"></asp:Literal>';
        thisBanner.targetDiv = "#div" + thisBanner.uniqueId;
        thisBanner.itemsResult = <asp:Literal ID="litItemsResult" runat="server"></asp:Literal>;
        thisBanner.templateJson = <asp:Literal ID="litTemplates" runat="server"></asp:Literal>;
        <asp:Literal ID="litProperties" runat="server"></asp:Literal>
        $(thisBanner.targetDiv).addClass(thisBanner.webPartTheme);                                                    
        $.getScript(bannerResourcePathValue + "/js/vendor/owl.carousel.min.js", function() { 
            thisBanner.getSearchResultsUsingREST(thisBanner);
        });               
    
</script>

<asp:Literal ID="litDebug" runat="server"></asp:Literal>