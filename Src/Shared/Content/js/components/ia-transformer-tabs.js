$(document).ready(function() {

  var Tabs = {

  init: function() {
    this.numberTabs();
    this.numberTabContent();
    this.bindUIfunctions();
    this.activeTab();
    //this.pageLoadCorrectTab();
  },

  //Assign an incrementing href to each tab to match the ID of each tab's content
  numberTabs: function() {
    $('.ia-transformer-tab-nav li a').each(function(i) {
      i = i+1;
      $(this).attr('href', '#tab-' + i);
    });
  },

  //Assign an incrementing ID to each tab. This is used to connect the tab content to the tab above.
  numberTabContent: function() {
    $('.ia-transformer-tabs .ia-single-tab').each(function(i) {
      i = i+1;
      $(this).attr('id', 'tab-' + i);
    });
  },

  bindUIfunctions: function() {

    // Delegation
    $(document)
      .on("click", ".ia-transformer-tab-nav a[href^='#']:not('.ia-tab-active-link')", function(event) {
        Tabs.changeTab(this.hash);
        event.preventDefault();
      })
      .on("click", ".ia-transformer-tab-nav a.ia-tab-active-link", function(event) {
        Tabs.toggleMobileMenu(event, this);
        event.preventDefault();
      });

  },

  changeTab: function(hash) {

    var anchor = $('[href=' + hash + ']');
    var div = $(hash);

    // activate correct anchor (visually)
    anchor.addClass("ia-tab-active-link").parent().siblings().find("a").removeClass("ia-tab-active-link");

    // activate correct div (visually)
    div.addClass("ia-tab-active-link").siblings().removeClass("ia-tab-active-link");

    // update URL, no history addition
    // You'd have this active in a real situation, but it causes issues in an <iframe> (like here on CodePen) in Firefox. So commenting out.
    //window.history.replaceState("", "", hash);

    // Close menu, in case mobile
    anchor.closest("ul").removeClass("open");

  },

  //Add an active class to the active tab LI
  activeTab: function() {
    $('.ia-transformer-tab-nav ul').each(function(){

        $(this).find('li').click(function() {

          if ( !$(this).hasClass('ia-tab-active') ) {
            //Remove the active class from all other tabs
            $(this).siblings().removeClass('ia-tab-active');
            //Add the active class to this tab
            $(this).addClass('ia-tab-active');
          }

        });

    });
  },

  // If the page has a hash on load, go to that tab
  /*pageLoadCorrectTab: function() {
    this.changeTab(document.location.hash);
  },*/

  toggleMobileMenu: function(event, el) {
    $(el).closest("ul").toggleClass("open");
  }

};

Tabs.init();
  

});