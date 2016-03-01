$(document).ready(function() {

	//remove name from list when clicked
	$('.ia-filer-name-list a').click( function(e) {
		$(this).fadeOut(300, function() { $(this).remove(); });
		e.preventDefault();
	});

	//show the accordion body on click of the heading
	$(".ia-accordion-header").each(function() {
		
		//if the filter should be expanded/open, it must have class .ia-open
		if ($(this).children('span.ia-accordion-icon').hasClass('ia-open')) {
			$(this).next('.ia-accordion-body').show();
		}
		//if the filter should be collapsed, it should not have .ia-open
		else if (!$(this).children('span.ia-accordion-icon').hasClass('ia-open')) {
			$(this).next('.ia-accordion-body').hide();
		}

		//on click of the header, toggle the accordion to expand or collapse
		$(this).click(function() {
			$(this).children('.ia-accordion-icon').toggleClass('ia-open');
			$(this).next('.ia-accordion-body').slideToggle();
			
		});
	});
    //When closing category popup, place selected categories in text input
	$('.refinerCategory .ia-button-rows .ia-category-confirm').click(function () {

	    var selectedCats = $('.refinerCategory .ia-current-selected-list').text();
	    var selectedCatsPath = $('.refinerCategory .selectedTaxonomy').val();
	    $('.ia-category-input').val(selectedCats);
	    $('.selectedTaxonomyConfrirm').val(selectedCatsPath);

	    updateGridData();

	});


});