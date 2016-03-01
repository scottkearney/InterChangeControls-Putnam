$(document).ready(function(){

	$('.ak-doc-filters-mobile').click(function(){
		$(this).toggleClass('ak-filter-active');
		$(this).next().find('.interAction').toggleClass('ak-filter-show');
	});

});