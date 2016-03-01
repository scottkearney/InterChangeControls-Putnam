$(document).ready(function() {

	$('.interAction .ia-modal-inline-trigger').magnificPopup({
		type: 'inline',
		preloader: false,
		closeBtnInside:true,
		showCloseBtn:true,
		fixedBgPos:true
	});

	$('.interAction .ia-modal-inline-trigger-preview').magnificPopup({
		type: 'inline',
		preloader: false,
		closeBtnInside:false,
		showCloseBtn:true,
		fixedBgPos:true
	});

	$(document).on('click', '.interAction .ia-modal-dismiss', function (e) {
		e.preventDefault();
		$.magnificPopup.close();
	});

	

});