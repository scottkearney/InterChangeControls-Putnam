$(document).ready(function() {
	

	$(function() {
 
	    var resizeEnd;
	 
	    $(window).on('resize', function() {
	 
	        clearTimeout(resizeEnd);
	 
	        resizeEnd = setTimeout(function() {
	            $(window).trigger('resizeEnd');
	        }, 500);
	 
	    });

	    $(window).on('resizeEnd', function() {
		    if ($(".siteNavBlock").css("display") === "none" ){
			}
			if ($(".siteNavBlock").css("display") === "block" ){
		    	$('.header .siteNavBlock').removeClass('mobileNav');
		    	$('.header .menuIconWrap').removeClass('menuOpen');
			}
		});
 
	});

	$('.header .menuIconWrap').click(function() {
		$(this).toggleClass('menuOpen');
		$('.header .siteNavBlock').toggleClass('mobileNav');
	});

	/* Drop down functionality */
	//If a top level li has a child UL, assign a class
	$('.mainNav > ul > li:has(ul)').addClass('hasSubmenu');
	// On click, show child UL menu
	$('.mainNav > ul > li.hasSubmenu > a').click(function(e) {
		$(this).siblings('ul').toggle();
		$(this).parent('li').toggleClass('menuActive');
		e.preventDefault();
	});
	

	//Close submenu if clicking outside of submenu area.
	/*$(document).mouseup(function (e) {
	    var container = $('.mainNav > ul > li.hasSubmenu ul');

	    if (!container.is(e.target) && container.has(e.target).length === 0) // ... nor a descendant of the container
	    {
	        container.hide();
	        $('.mainNav > ul > li:has(ul)').removeClass('menuActive');
	    }
	});*/


});

