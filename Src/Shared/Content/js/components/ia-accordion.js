$(document).ready(function() {
	// this is a simple accordion that allows multiple accordion divs to be opened.

	//hide all accordion bodies
	$(".ia-accordion .ia-accordion-body").hide();

	//show the accordion bocy on click of the heading
	$(".ia-accordion .ia-accordion-header").each(function() {
		$(this).click(function() {
			if($(this).next('.ia-accordion-body').is(":visible")){
				$(this).children('.ia-accordion-icon').removeClass('ia-open');
		        $(this).next('.ia-accordion-body').slideUp();
		      } else {
		      	$(this).children('.ia-accordion-icon').addClass('ia-open');
		        $(".accordion .accord-content").slideUp();
		        $(this).next('.ia-accordion-body').slideToggle();
		      }
		});
	});

});