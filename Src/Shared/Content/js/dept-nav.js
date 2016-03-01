$(document).ready(function(){
	$('.ia-dept-nav-wrapper').css({'height':($(".ak-col-template").height()+'px')});
	$('.ia-dept-nav-wrapper').css({'width':($(".ak-col-template").width()+'px')});
	
});
var waitForFinalEvent = (function () {
	  var timers = {};
	  return function (callback, ms, uniqueId) {
	    if (!uniqueId) {
	      uniqueId = "Don't call this twice without a uniqueId";
	    }
	    if (timers[uniqueId]) {
	      clearTimeout (timers[uniqueId]);
	    }
	    timers[uniqueId] = setTimeout(callback, ms);
	  };
	})();
$(window).resize(function () {
    waitForFinalEvent(function(){
     	$('.ia-dept-nav-wrapper').css({'height':($(".ak-col-template").height()+'px')});
		$('.ia-dept-nav-wrapper').css({'width':($(".ak-col-template").width()+'px')});
    }, 250);
});

