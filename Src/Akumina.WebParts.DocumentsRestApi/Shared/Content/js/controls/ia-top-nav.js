$(document).ready(function() {

	/* Drop down functionality */
	//If a top level li has a child UL, assign a class
	$('.ia-top-nav > ul > li:has(ul)').addClass('hasSubmenu');

	// On click, show child UL menu
	$('.ia-top-nav > ul > li.hasSubmenu > .ia-dropdown-trigger').click(function(e) {

		if ($(this).parent('li').is(":not(.menuActive)")) {
			$('.ia-top-nav ul li.hasSubmenu ul').slideUp('fast');
        	$('.ia-top-nav ul li.hasSubmenu').removeClass('menuActive');
			$(this).siblings('ul').slideDown('fast');
			$(this).parent('li').addClass('menuActive');
		}
		//if this menu is already active, hide it
		else if ($(this).parent('li').hasClass('menuActive')) {
			$(this).siblings('ul').slideUp('fast');
			$(this).parent('li').removeClass('menuActive');
		}
		
		e.preventDefault();
	});
	
    // Close menus when clicking outside the menu area
	$(document).on('click', function(event) {
	  if (!$(event.target).closest('.ia-top-nav ul li.hasSubmenu').length) {
	    	$('.ia-top-nav ul li.hasSubmenu ul').slideUp('fast');
        	$('.ia-top-nav ul li.hasSubmenu').removeClass('menuActive');
	  }
	});
	

	/* Drop down functionality */
	//If a second level li has a child UL, assign a class
	$('.ia-top-nav > ul li ul > li:has(ul)').addClass('hasSubmenu');

	// On click, show child UL menu
	$('.ia-top-nav > ul li ul > li.hasSubmenu > .ia-dropdown-trigger').click(function(e) {
		if ($(this).parent('li').is(":not(.menuActive)")) {
			$('.ia-top-nav ul li ul li.hasSubmenu ul').slideUp('fast');
        	$('.ia-top-nav ul li ul li.hasSubmenu').removeClass('menuActive');
			$(this).siblings('ul').slideDown('fast');
			$(this).parent('li').addClass('menuActive');
		}
		//if this menu is already active, hide it
		else if ($(this).parent('li').hasClass('menuActive')) {
			$(this).siblings('ul').slideUp('fast');
			$(this).parent('li').removeClass('menuActive');
		}
		
		e.preventDefault();
	});

	//Remove active state from second level when a link is clicked.
	$('.ia-top-nav > ul > li.hasSubmenu > ul li a').click(function(){
		$(this).parent().siblings('li.hasSubmenu').removeClass('menuActive');

	});

    // Close menus when clicking outside the menu area
	$(document).on('click', function(event) {
	  if (!$(event.target).closest('.ia-top-nav ul li.hasSubmenu ul li.hasSubmenu').length) {
	    	$('.ia-top-nav ul li.hasSubmenu ul li.hasSubmenu ul').slideUp('fast');
        	$('.ia-top-nav ul li.hasSubmenu ul li.hasSubmenu ul').removeClass('menuActive');
	  }
	});




});

