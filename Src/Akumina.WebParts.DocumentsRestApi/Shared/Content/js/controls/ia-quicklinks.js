$(document).ready(function() {
	

	$('.ia-quicklinks-level-1 li .ia-quicklink-flyout-trigger').click(function(e) {
		
		//If the flyover is not visible, show it. Hide all open flyovers
		if ($(this).siblings('.ia-quicklink-cols').is(':not(:visible)')) {
			$('.ia-quicklink-cols').fadeOut();
			$('.ia-quicklinks-level-3').fadeOut();
			$('.ia-quicklinks-level-1 > li .ia-quicklink-flyout-trigger').removeClass('menu-active');
			$(this).siblings('.ia-quicklink-cols').fadeIn();
			$(this).addClass('menu-active');
		}
		//if the flyout is already visible, hide it
		else if ($(this).siblings('.ia-quicklink-cols').is(':visible')) {
			$(this).siblings('.ia-quicklink-cols').fadeOut();
			$('.ia-quicklinks-level-3').fadeOut();
			$(this).removeClass('menu-active');
		}
		e.preventDefault();
	});

	// Level 3 flyouts
	$('.ia-quicklinks-level-2 .ia-quicklink-sublevel').click(function(e) {
		//If the flyover is not visible, show it. Hide all open flyovers
		if ($(this).siblings('.ia-quicklinks-level-3').is(':not(:visible)')) {
			$('.ia-quicklinks-level-3').fadeOut();
			$('.ia-quicklinks-level-2 .ia-quicklink-sublevel').removeClass('menu-active');
			$(this).siblings('.ia-quicklinks-level-3').fadeIn();
			$(this).addClass('menu-active');
		}
		//if the flyout is already visible, hide it
		else if ($(this).siblings('.ia-quicklinks-level-3').is(':visible')) {
			$(this).siblings('.ia-quicklinks-level-3').fadeOut();
			$(this).removeClass('menu-active');
		}
		e.preventDefault();
	});

	// Close menus when clicking outside the menu area
	$(document).on('click', function(event) {
	  if (!$(event.target).closest('.ia-quicklinks, .ia-quicklink-cols').length) {
	    	$('.ia-quicklink-cols').fadeOut();
            $('.ia-quicklinks-level-1 > li .ia-quicklink-flyout-trigger').removeClass('menu-active');
	  }
	});

	//JS to add classes for styling flyout widths
	$('.ia-quicklinks-level-1 > li .ia-quicklink-cols.category-cols').each(function(){
		var count = $(this).children('.ia-quicklink-col').length;
		// add a class that matches the number of columns
		$(this).addClass('ia-cols-' + count);
	});

});