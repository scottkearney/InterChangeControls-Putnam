$(document).ready(function() {
	
	$('.filterTrigger').click(function() {
		//$('.filtersWrapper').slideToggle('fast');
		$(this).toggleClass('filterOpen');
		$(this).next('.filtersWrapper').toggleClass('filterShow');
		
		//$('.filterOpen').next('.filtersWrapper').css('display', 'block');
	});
	//$('.filterOpen').next('.filtersWrapper').css('display', 'block');

});