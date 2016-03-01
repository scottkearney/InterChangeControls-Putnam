$(document).ready(function() {

	// Open/Close the Action Menu on mobile
	$('.ia-action-menu-toggle').click(function(e) {
		e.preventDefault();

		$('.ia-action-menu-content').slideToggle("slow");
		$('.ia-action-menu-arrow').toggleClass('fa-chevron-up').toggleClass('fa-chevron-down');
	});

	

	// Toggle Editor when adding a Thread Reply
	$("input[name='ia-discussion-add-reply-box']").focus(function() {
		$(this).fadeOut( "fast", function() {
		    // Animation complete.
			$('.ia-discussion-reply-editor').fadeIn("fast");
		});
	});

	// Make the button group "sticky" to the page as the user scrolls
	// uses jquery.sticky.js plugin
	$(".interAction .ia-action-menu-content").sticky({topSpacing:10, responsiveWidth:true});

});



// Show/Hide the Action Menu when resizing between mobile to desktop
$( window ).resize(function() {
	var browserWidth = viewport().width;

	if (browserWidth > 640) {
		$('.ia-action-menu-content').show();
		
	}
	else {
		$('.ia-action-menu-content').hide();
	}
});



// Get proper browser width, without scrollbars
function viewport() {
    var e = window, a = 'inner';
    if (!('innerWidth' in window )) {
        a = 'client';
        e = document.documentElement || document.body;
    }
    return { width : e[ a+'Width' ] , height : e[ a+'Height' ] };
}

