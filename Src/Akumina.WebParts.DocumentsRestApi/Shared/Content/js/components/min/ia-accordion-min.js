$(document).ready(function(){$(".ia-accordion .ia-accordion-body").hide(),$(".ia-accordion .ia-accordion-header").each(function(){$(this).click(function(){$(this).next(".ia-accordion-body").is(":visible")?($(this).children(".ia-accordion-icon").removeClass("ia-open"),$(this).next(".ia-accordion-body").slideUp()):($(this).children(".ia-accordion-icon").addClass("ia-open"),$(".accordion .accord-content").slideUp(),$(this).next(".ia-accordion-body").slideToggle())})})});