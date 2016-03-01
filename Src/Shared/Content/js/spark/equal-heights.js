/**
* jquery.matchHeight-min.js v0.5.2
* http://brm.io/jquery-match-height/
* License: MIT
*/
(function(c){var n=-1,f=-1,r=function(a){var b=null,d=[];c(a).each(function(){var a=c(this),k=a.offset().top-h(a.css("margin-top")),l=0<d.length?d[d.length-1]:null;null===l?d.push(a):1>=Math.floor(Math.abs(b-k))?d[d.length-1]=l.add(a):d.push(a);b=k});return d},h=function(a){return parseFloat(a)||0},p=function(a){var b={byRow:!0,remove:!1,property:"height"};if("object"===typeof a)return c.extend(b,a);"boolean"===typeof a?b.byRow=a:"remove"===a&&(b.remove=!0);return b},b=c.fn.matchHeight=function(a){a=
p(a);if(a.remove){var e=this;this.css(a.property,"");c.each(b._groups,function(a,b){b.elements=b.elements.not(e)});return this}if(1>=this.length)return this;b._groups.push({elements:this,options:a});b._apply(this,a);return this};b._groups=[];b._throttle=80;b._maintainScroll=!1;b._beforeUpdate=null;b._afterUpdate=null;b._apply=function(a,e){var d=p(e),g=c(a),k=[g],l=c(window).scrollTop(),f=c("html").outerHeight(!0),m=g.parents().filter(":hidden");m.each(function(){var a=c(this);a.data("style-cache",
a.attr("style"))});m.css("display","block");d.byRow&&(g.each(function(){var a=c(this),b="inline-block"===a.css("display")?"inline-block":"block";a.data("style-cache",a.attr("style"));a.css({display:b,"padding-top":"0","padding-bottom":"0","margin-top":"0","margin-bottom":"0","border-top-width":"0","border-bottom-width":"0",height:"100px"})}),k=r(g),g.each(function(){var a=c(this);a.attr("style",a.data("style-cache")||"")}));c.each(k,function(a,b){var e=c(b),f=0;d.byRow&&1>=e.length?e.css(d.property,
""):(e.each(function(){var a=c(this),b={display:"inline-block"===a.css("display")?"inline-block":"block"};b[d.property]="";a.css(b);a.outerHeight(!1)>f&&(f=a.outerHeight(!1));a.css("display","")}),e.each(function(){var a=c(this),b=0;"border-box"!==a.css("box-sizing")&&(b+=h(a.css("border-top-width"))+h(a.css("border-bottom-width")),b+=h(a.css("padding-top"))+h(a.css("padding-bottom")));a.css(d.property,f-b)}))});m.each(function(){var a=c(this);a.attr("style",a.data("style-cache")||null)});b._maintainScroll&&
c(window).scrollTop(l/f*c("html").outerHeight(!0));return this};b._applyDataApi=function(){var a={};c("[data-match-height], [data-mh]").each(function(){var b=c(this),d=b.attr("data-match-height")||b.attr("data-mh");a[d]=d in a?a[d].add(b):b});c.each(a,function(){this.matchHeight(!0)})};var q=function(a){b._beforeUpdate&&b._beforeUpdate(a,b._groups);c.each(b._groups,function(){b._apply(this.elements,this.options)});b._afterUpdate&&b._afterUpdate(a,b._groups)};b._update=function(a,e){if(e&&"resize"===
e.type){var d=c(window).width();if(d===n)return;n=d}a?-1===f&&(f=setTimeout(function(){q(e);f=-1},b._throttle)):q(e)};c(b._applyDataApi);c(window).bind("load",function(a){b._update(!1,a)});c(window).bind("resize orientationchange",function(a){b._update(!0,a)})})(jQuery);

$(window).load(function() {
  // Once all elements on the page have loaded (including the banenr), size the welcome box
  $('.welcome-message .ia-content-block').css('height', $('.ia-banner-slides').height() + 'px');
  
});

$(document).ready(function() {
	// Resize the welcome message area when the banner resizes */
  	$('.owl-carousel').on('initialize.owl.carousel initialized.owl.carousel' + 
  	'resize.owl.carousel resized.owl.carousel ' +
    'refresh.owl.carousel refreshed.owl.carousel ' +
    'update.owl.carousel updated.owl.carousel', function() {
    	$('.welcome-message .ia-content-block').css('height', $('.ia-banner-slides').height() + 'px');
	    //$('.ia-banner-slides, .welcome-message').matchHeight();
	});
});