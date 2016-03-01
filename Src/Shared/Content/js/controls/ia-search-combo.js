$(document).ready(function() {

	$('.interAction .ia-search-combo-site-list').on("click",function(e) {
		e.preventDefault();
		$(this).children('.ia-search-combo-site-list-dropdown').slideToggle();
		$('.ia-search-combo-site-list-icon').toggleClass('fa-caret-down');
		$('.ia-search-combo-site-list-icon').toggleClass('fa-caret-up');
	});

	$('.interAction .ia-search-combo-site-list-dropdown a').click(function() {
		var title = $(this).html();
		$('.ia-search-combo-site-name').html(title);
	});

	$.widget( "custom.catcomplete", $.ui.autocomplete, {
    _create: function() {
      this._super();
      this.widget().menu( "option", "items", "> :not(.ui-autocomplete-category)" );
    },
    _renderMenu: function( ul, items ) {
      var that = this,
        currentCategory = "";
      $.each( items, function( index, item ) {
        var li;
        if ( item.category !== currentCategory ) {
          ul.append( "<li class='ui-autocomplete-category'>" + item.category + "</li>" );
          currentCategory = item.category;
        }
        li = that._renderItemData( ul, item );
        if ( item.category ) {
          li.attr( "aria-label", item.category + " : " + item.label );
        }
      });
    }
});

//This is data for demonstration only, the data source should be integrated by the dev team
$(function() {
  var data = [
    { label: "anders", category: "" },
    { label: "andreas", category: "" },
    { label: "antal", category: "" },
    { label: "annhhx10", category: "Products" },
    { label: "annk K12", category: "Products" },
    { label: "annttop C13", category: "Products" },
    { label: "anderson C13", category: "Products" },
    { label: "anderson C14", category: "Products" },
    { label: "anders andersson", category: "People" },
    { label: "andreas andersson", category: "People" },
    { label: "andreas johnson", category: "People" }
  ];

  // initialize the autocomplete plugin
  $( ".interAction .ia-search-combo-box" ).catcomplete({
    // If using remote datasource, the delay may need to be increased
      delay: 500,
      // Set minLength to 3 as default, allow to be overridden in InterAction control
      minLength: 3,
      source: data
  }).catcomplete( "widget" ).addClass( "ia-autocomplete" );
});

});


