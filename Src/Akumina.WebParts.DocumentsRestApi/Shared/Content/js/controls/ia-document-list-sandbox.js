function rowhighlight()
{
$("#ogrid").on("click", "tbody tr td", function () {	
	if ( !$(this).hasClass('ia-doclib-checkbox') )
			{
				//uncheck all other checkboxes AND remove the highlight class
				$('.ia-doclist-selectAll').prop('checked', false);
				$(this).parent().siblings('tr').removeClass('ia-doclist-row-selected');
				$(this).parent().siblings('tr').children('.ia-doclib-checkbox').children('input[type=checkbox]').prop('checked', false);
				
				//add the highlight class to the curren row AND check the row's checkbox
				$(this).parent().addClass('ia-doclist-row-selected');
				$(this).siblings('.ia-doclib-checkbox').children('input[type=checkbox]').prop('checked', true);

			}
			
			if ( $(this).hasClass('ia-doclib-checkbox') ) {

				//if the checkbox is not already checked
				if($(this).children('input[type=checkbox]').is(':checked')) {
					$(this).parent().addClass('ia-doclist-row-selected');
				}

				//if the checkbox was checked
				if(!$(this).children('input[type=checkbox]').is(':checked')) {
					$('.ia-doclist-selectAll').prop('checked', false);
					$(this).parent().removeClass('ia-doclist-row-selected');
				}
				
			}
	});
}