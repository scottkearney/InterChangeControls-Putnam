$(document).ready(function () {
    // this is a simple accordion that allows multiple accordion divs to be opened.

    //hide all accordion bodies
    //$(".ia-accordion .ia-accordion-body").hide();

    //show the accordion bocy on click of the heading
    $(".ia-accordion").on("click", ".ia-accordion-header", function () {

        if ($(this).next('.ia-accordion-body').is(":visible")) {
            $(this).children('.ia-accordion-icon').removeClass('ia-open');
            $(this).next('.ia-accordion-body').slideUp();
        } else {
            $(this).children('.ia-accordion-icon').addClass('ia-open');
            $(".accordion .accord-content").slideUp();
            $(this).next('.ia-accordion-body').slideToggle();
        }
    });
    //$(".ia-accordion").on("change", "input[type='text']", function () {
       
    //    showloader();
    //});
    //$(".ia-accordion").on("change", "input[type='checkbox']", function () {
      
    //    showloader();
    //});

});