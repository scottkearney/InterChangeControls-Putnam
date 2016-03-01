$(document).ready(function() {
	// this is a simple Toggle Button with a dropdown

	$('.interAction .ia-toggle-box').click(function(e) {
		e.preventDefault();
		$(this).children('.ia-toggle-dropdown').slideToggle();
		$(this).children('.ia-toggle-name').toggleClass('ia-open');
		//$(this).children('.ia-toggle-dropdown').toggleClass('ia-hide').toggleClass('ia-show');
	});

	$('.interAction .ia-toggle-dropdown a').click(function() {
		var title = $(this).html();
		$('.ia-toggle-name').html(title);
	});

});