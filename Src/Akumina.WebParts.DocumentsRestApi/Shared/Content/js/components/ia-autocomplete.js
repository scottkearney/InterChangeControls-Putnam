$(document).ready(function() {

  //This is data for demonstration only, the data source should be integrated by the dev team
  $(function() {
    var data = [
      { label: "anders", category: "" },
      { label: "andreas", category: "" },
      { label: "antal", category: "" },
      { label: "annhhx10", category: "Products" },
      { label: "annk K12", category: "Products" },
      { label: "annttop C13", category: "Products" },
      { label: "anders andersson", category: "People" },
      { label: "andreas andersson", category: "People" },
      { label: "andreas johnson", category: "People" }
    ];

    // initialize the autocomplete plugin
    $( ".interAction .ia-autocomplete-box" ).autocomplete({
      // If using remote datasource, the delay may need to be increased
        delay: 500,
        // Set minLength to 3 as default, allow to be overridden in InterAction control
        minLength: 3,
        source: data
    }).autocomplete( "widget" ).addClass( "ia-autocomplete" );

  });

});


