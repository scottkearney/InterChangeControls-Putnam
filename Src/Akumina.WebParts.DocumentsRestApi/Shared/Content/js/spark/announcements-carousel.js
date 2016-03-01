//InterAction Announcments Carousel
$(document).ready(function() {

  var owl = $('.annoucements-carousel .ia-announcements ul');
  $(owl).owlCarousel({

  	autoplayHoverPause: false, // pause on mouse hover
  	smartSpeed: 500, // slide transition speed  	
  	dotsEach: true, // show a dot for each slide item
  	navContainer: '.ia-carousel-nav',
  	dotsContainer: '.ia-carousel-dots',
    navText: ['<span class="ia-carousel-prev fa fa-chevron-left"></span>','<span class="ia-carousel-next fa fa-chevron-right"></span>'],
    items: 1, // Number of items to show on screen
    autoplaySpeed: 500, // slide transition speed when autoplay = true

  	//Values to be set by InterAction Banner Instruction Set:
  	loop: true, // Inifnity loop. Duplicate last and first items to get loop illusion
  	//autoplay:true, // start slider rotation on page load
  	autoplayTimeout: 2000, // slide duration when autoplay = true
  	nav: true, // show prev and next links
  	dots: true, // show dot navigation
  	//For Fade transition, the following two must be set. For slide, neither should be set
  	animateOut: 'fadeOut',
    animateIn: 'fadeIn'
  	
  });


  
  

});