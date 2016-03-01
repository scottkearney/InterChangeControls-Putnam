// $(document).ready(function() {

function librarySearch()
{
	
$('.interAction .ia-search-library-site-name').on('click',function(e) {
		e.preventDefault();

    //If the dropdown is already visible:
    if ($(this).siblings('.ia-search-library-site-list-dropdown').is(":visible")) {
      //Hide the dropdown
      $(this).siblings('.ia-search-library-site-list-dropdown').hide();
      //Change CSS classes for menu arrow to point down
      $('.ia-search-library-site-list-icon').addClass('fa-caret-down');
      $('.ia-search-library-site-list-icon').removeClass('fa-caret-up');

      //If all libraries checked:
      if ($('.ia-search-all .ia-search-checkbox').prop('checked')) {
        //change the dropdown text to the All Libraries label
        var allLibs = $('.ia-search-all .ia-search-label').html();
        $('.ia-search-library-site-name-field').html(allLibs);
      }

      //If one other library is checked: 
      else if ($('.ia-search-single-library .ia-search-checkbox:checked').length < 2){
        //change the dropdown text to the selected library's label name
        var singleLib = $('.ia-search-single-library .ia-search-checkbox:checked').next('.ia-search-label').html();
        $('.ia-search-library-site-name-field').html(singleLib);
      }

      //If multiple other libraries checked: 
      else if ($('.ia-search-single-library input:checked').length > 1){
        //change the dropdown text to indicate multiple libraries are selected
        $('.ia-search-library-site-name-field').html('Multiple Libraries');
      }


    }

    //If the dropdown is hidden:
    else if ($(this).siblings('.ia-search-library-site-list-dropdown').is(":hidden")) {
      //Show the dropdown
      $(this).siblings('.ia-search-library-site-list-dropdown').show();
      //Change CSS classes for menu arrow to point up
      $('.ia-search-library-site-list-icon').removeClass('fa-caret-down');
      $('.ia-search-library-site-list-icon').addClass('fa-caret-up');
    }

	});
  
  //If user checks off any library other than "All Libraries", uncheck "All Libraries"
  $('.ia-search-single-library input:checkbox').on('change',function() {

    if ($('.ia-search-all input').prop('checked')) {
        $('.ia-search-all input').prop('checked', false);
    }


  });
  
  //If user checks off "All Libraries", uncheck all others
  $('.ia-search-all input:checkbox').on('change',function() {

    if ($(this).prop('checked')) {
        $('.ia-search-single-library input').prop('checked', true);
    }
    else if ($(this).prop('checked', false)) {
        $('.ia-search-single-library input').prop('checked', false);
    }

  });

}


// });