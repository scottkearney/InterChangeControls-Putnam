/* Used in conjunction with pickadate js pluging */
/* Options: http://amsul.ca/pickadate.js/date.htm */

$(document).ready(function() {
	$('.ia-datepicker').pickadate({
    	selectYears: true,
    	selectMonths: true,
    	format: 'mmm dd, yyyy'
	});

});