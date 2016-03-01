$(document).ready(function() {

	$('.interAction .ia-tabs').each(function(){

		$(this).find('li').click(function() {

			if ( !$(this).hasClass('ia-tab-active') ) {
				//Remove the active class from all other tabs
				$(this).siblings().removeClass('ia-tab-active');
				//Add the active class to this tab
				$(this).addClass('ia-tab-active');
			}
		});
	});

});