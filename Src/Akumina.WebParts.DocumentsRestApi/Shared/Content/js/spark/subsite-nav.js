$(document).ready(function() {
	//If a top level li has a child UL, assign a class
	$('.subsite-menu ul > li:has(ul)').addClass('hasSubmenu');
	$('<span class="fa fa-caret-down submenu-trigger"></span>').insertBefore('.subsite-menu ul > li:has(ul) > a');
	
	$('.subsite-name').click(function(){
		$('.subsite-menu-wrapper').toggleClass('show-menu');
		$('.subsite-menu-wrapper').fadeToggle();
	});

	$('.subsite-menu-close').click(function(){
		$('.subsite-menu-wrapper').removeClass('show-menu');
		$('.subsite-menu-wrapper').fadeOut();
	});

	$('.submenu-trigger').click(function(){
		$(this).siblings('ul').slideToggle();
	});

	
});