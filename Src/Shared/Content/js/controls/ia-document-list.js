$(document).ready(function () {

    previewSelect();

});

function previewSelect()
{
    //Make the button group "sticky" to the page as the user scrolls
    //uses jquery.sticky.js plugin
    //$(".ia-documentList .ia-button-group").sticky({topSpacing:0, responsiveWidth:true});
    $('.interAction .ia-document-library-list').each(function () {

        //on load, reset all checkboxes to unchecked state.
        $('.ia-document-library-list tbody td input[type=checkbox]').prop('checked', false);
        $('.ia-doclist-selectAll').prop('checked', false);

        //On click of individual <td> in a <tr>
        $(this).find('tbody tr td').click(function () {

            // if the <td> is not .doclib-checkbox
            if (!$(this).hasClass('ia-doclib-checkbox')) {
                //uncheck all other checkboxes AND remove the highlight class
                $('.ia-doclist-selectAll').prop('checked', false);
                $(this).parent().siblings('tr').removeClass('ia-doclist-row-selected');
                $(this).parent().siblings('tr').children('.ia-doclib-checkbox').children('input[type=checkbox]').prop('checked', false);

                //add the highlight class to the curren row AND check the row's checkbox
                $(this).parent().addClass('ia-doclist-row-selected');
                $(this).siblings('.ia-doclib-checkbox').children('input[type=checkbox]').prop('checked', true);

            }

            // if the <td> is .doclib-checkbox
            if ($(this).hasClass('ia-doclib-checkbox')) {

                //if the checkbox is not already checked
                if ($(this).children('input[type=checkbox]').is(':checked')) {
                    $(this).parent().addClass('ia-doclist-row-selected');
                }

                //if the checkbox was checked
                if (!$(this).children('input[type=checkbox]').is(':checked')) {
                    $('.ia-doclist-selectAll').prop('checked', false);
                    $(this).parent().removeClass('ia-doclist-row-selected');
                }

            }
            displayOptions();
        });

        //On click of the "select all" checkbox
        $('.ia-doclist-selectAll').click(function () {
            // if this is already checked
            if (!$(this).is(':checked')) {
                $('.ia-document-library-list tbody tr').removeClass('ia-doclist-row-selected');
                $('.ia-document-library-list tbody td input[type=checkbox]').prop('checked', false);
            }
            if ($(this).is(':checked')) {
                $('.ia-document-library-list tbody tr').addClass('ia-doclist-row-selected');
                $('.ia-document-library-list tbody td input[type=checkbox]').prop('checked', true);
            }

        });

    });
}

function UndoSelection()
{
    $('.interAction .ia-document-library-list').each(function () {

        //On click of individual <td> in a <tr>
        //$(this).find('tbody tr td').each(function () {
        $(this).find('tbody .ia-doclib-checkbox').each(function () {
            // if the <td> is not .doclib-checkbox
            if (!$(this).hasClass('ia-doclib-checkbox')) {
                //uncheck all other checkboxes AND remove the highlight class
                $('.ia-doclist-selectAll').prop('checked', false);
                $(this).parent().siblings('tr').removeClass('ia-doclist-row-selected');
                $(this).parent().siblings('tr').children('.ia-doclib-checkbox').children('input[type=checkbox]').prop('checked', false);

                ////add the highlight class to the curren row AND check the row's checkbox
                //$(this).parent().addClass('ia-doclist-row-selected');
                //$(this).siblings('.ia-doclib-checkbox').children('input[type=checkbox]').prop('checked', true);

            }

           
        });
              

    });
}