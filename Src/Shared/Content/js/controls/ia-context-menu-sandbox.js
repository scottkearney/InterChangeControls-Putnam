//var allCheckbox = '.ia-doclist-selectAll input[type=checkbox]', selectionbox = '.selection input[type=checkbox]', checkoutImg = '.checkedout', hiddenFileype = '.itemhiddenFileType', emptyelement = '&nbsp;', openFilebn = '#btnOpen', delFilebn = '#btnDelete', checkOutFilebn = '#btnCheckOut', checkOutFilebnAll = '#btnCheckOutAll', checkInbnMore = '#btnCheckInMore', checkInFilebn = '#btnCheckIn', disCheckFilebn = '#btnDiscardCheckOut', morebn = '#btnMore', downloadbn = '#btnDownload';

var allCheckbox = '.ia-doclist-selectAll',
    selectionbox = '.selection',
    checkoutImg = '.ia-doc-checkedOut',
    hiddenFileype = '.itemhiddenFileType',
    emptyelement = '&nbsp;',
    openFilebn = '#btnOpen',
    delFilebn = '#btnDelete',
    checkOutFilebn = '#btnCheckOut',
    checkOutFilebnAll = '#btnCheckOutAll',
    checkInbnMore = '#btnCheckInMore',
    checkInFilebn = '#btnCheckIn',
    disCheckFilebn = '#btnDiscardCheckOut',
   disCheckFilebnAll = '#btnDiscardChkOutAll',
    morebn = '#btnMore',
downloadbn = '#btnDownload', editPermission = ".itemEditPermission", editProperties = "#btnEditProperties", workflow = "#btnWorkflow";
function selectall(checkoption)
{ 
if (!checkoption ) {
				$('.ia-document-library-list tbody tr').removeClass('ia-doclist-row-selected');
				$('.ia-document-library-list tbody td input[type=checkbox]').prop('checked', false);
			}
			else{
				$('.ia-document-library-list tbody tr:visible').addClass('ia-doclist-row-selected');
				$('.ia-document-library-list tbody tr:visible td input[type=checkbox]').prop('checked', true);
			}
//selection(selectionbox, checkoption); 
displayOptions(); }
function onselection()
{
    if ($(selectionbox + ':checked').length == $(selectionbox).length)

        selection(allCheckbox, true);
    else
        selection(allCheckbox, false);
    displayOptions();
}
function selection(element, option) {
    $(element).each(function () {
        this.checked = option;
		
    });
}
function hideAllButton() {
       hideElements([checkInFilebn, checkOutFilebnAll, disCheckFilebnAll, openFilebn, downloadbn, delFilebn, morebn]);
}
function displayOptions() {
    var i = 0; var editperVal = "", cancheckoutVal = "";
    $(selectionbox).each(function () {
        if (this.checked) {
            var text = $(this).parent().parent().find(hiddenFileype).justtext();
            var edit = $("#createpermission").val();//$(this).parent().parent().find(editPermission).justtext().split(":");

            if ((text == emptyelement || text == "") && edit[0] != "yes") {
                i = 0;
                return false;
            }
            else if (text == "one" && edit[0] != "yes") {
                i = -5;
                return false;
            }




            else if (text == emptyelement || text == "") {

                i = -2;
                return false;
            }
            else if (text == "one") {



                i = -3;
                return false;
            }
            else if (edit != "yes") {
                i = -4;
                return false;
            }
            //else if (edit[0] != "yes") {
            //    i = -4;
            //    return false;
            //}
            //else if (edit[1] != "yes") {
            //    i = -6;
            //    return false;
            //}
            ++i;
        }
    });

    if (i == -3 && $(selectionbox + ":checked").length > 1)
        i = -2;

    if (i == 0)
        hideElements([checkInFilebn, checkOutFilebnAll, disCheckFilebnAll, openFilebn, downloadbn, delFilebn, morebn]);

    else if (i == -2) {
        hideElements([checkInFilebn, checkOutFilebnAll, openFilebn, downloadbn, morebn, disCheckFilebnAll]);
        showElements([delFilebn]);
        AddBorderRight([delFilebn]); AddRightBorderRadius([delFilebn]); AddLeftBorderRadius([delFilebn]);
    }
    else if (i == -3) {

        showElements([openFilebn, delFilebn, morebn]);
        hideElements([downloadbn, checkOutFilebn, checkInFilebn, disCheckFilebn,checkInbnMore,disCheckFilebnAll]);
        AddLeftBorderRadius([openFilebn]);
        RemoveLeftBorderRadius([delFilebn]); RemoveRightBorderRadius([delFilebn]);
    }
    else if (i == -5) {
        showElements([openFilebn, morebn]);
        hideElements([downloadbn, checkOutFilebn, delFilebn, checkInFilebn, disCheckFilebn,checkInbnMore,disCheckFilebnAll]);
        AddLeftBorderRadius([openFilebn]); AddRightBorderRadius([openFilebn]);

    }
    else if (i == -4) {

        hideElements([disCheckFilebn, checkInFilebn, downloadbn, checkInbnMore, workflow, editProperties, delFilebn]);
        if ($(selectionbox + ':checked').length < 2) {
            showElements([openFilebn, morebn]);
            AddLeftBorderRadius([openFilebn]);
        }
        else { hideElements([checkInFilebn, checkOutFilebnAll, disCheckFilebnAll, openFilebn, downloadbn, delFilebn, morebn]); }
    }
    else if (i == -6) {
        if ($(selectionbox + ':checked').length < 2) {
			if ($(selectionbox + ':checked').parent().parent().find(checkoutImg).length == 1)
			{
				hideElements([disCheckFilebn, disCheckFilebnAll,checkInFilebn, checkInbnMore, checkOutFilebnAll,delFilebn,checkOutFilebn,editProperties]);
				showElements([openFilebn, downloadbn, morebn,workflow])
				AddLeftBorderRadius([openFilebn]);
			}
			else
			{
				hideElements([disCheckFilebn, checkInFilebn, checkInbnMore, checkOutFilebnAll]);
				showElements([openFilebn, downloadbn, morebn,workflow,editProperties,checkOutFilebn,delFilebn])
				AddLeftBorderRadius([openFilebn]);
			}
        }
        else {
            if ($(selectionbox + ':checked').parent().parent().find(checkoutImg).length == 0) {
                showElements([checkOutFilebnAll, delFilebn]); AddLeftBorderRadius([checkOutFilebnAll]);
                RemoveLeftBorderRadius([delFilebn]);
                AddRightBorderRadius([delFilebn]); hideElements([openFilebn, downloadbn, morebn]);
                AddBorderRight([delFilebn]);
            }
            else {

                hideElements([checkInFilebn, checkOutFilebnAll, disCheckFilebnAll, openFilebn, downloadbn, delFilebn, morebn]);
            }
        }

    }
    else if (i < 2) {

        if ($(selectionbox + ':checked').parent().parent().find(checkoutImg).length > 0) {
            $(delFilebn).show(); $(checkOutFilebn).hide(); $(checkInFilebn).hide(); $(checkOutFilebnAll).hide(); $(disCheckFilebnAll).hide(); 
			showElements([disCheckFilebn, checkInbnMore]);
        }
        else {
            $(delFilebn).show(); $(checkOutFilebn).show(); $(checkOutFilebnAll).hide();
            hideElements([disCheckFilebn, checkInFilebn, checkInbnMore,checkOutFilebnAll]);
        }
        showElements([openFilebn, downloadbn, morebn,workflow,editProperties]);
        AddLeftBorderRadius([openFilebn]);
        RemoveLeftBorderRadius([delFilebn]); RemoveRightBorderRadius([delFilebn]);
    }
    else {

        if ($(selectionbox + ':checked').parent().parent().find(checkoutImg).length == 0) { showElements([checkOutFilebnAll, delFilebn]); AddLeftBorderRadius([checkOutFilebnAll]); RemoveLeftBorderRadius([delFilebn]); AddRightBorderRadius([delFilebn]); }
        else if ($(selectionbox + ':checked').parent().parent().find(checkoutImg).length == i) {
            showElements([checkInFilebn, disCheckFilebnAll, delFilebn]); AddRightBorderRadius([delFilebn]); RemoveLeftBorderRadius([delFilebn]);
        }
        else { showElements([delFilebn]); $(disCheckFilebnAll).hide(); $(checkOutFilebnAll).hide(); $(checkInFilebn).hide(); AddRightBorderRadius([delFilebn]); AddLeftBorderRadius([delFilebn]); }
        hideElements([openFilebn, downloadbn, morebn]);
        AddBorderRight([delFilebn]);

    }



}


function hideElements(ids) {
    for (var i = 0; i < ids.length; i++) {
        $(ids[i]).hide();
    }
}
function showElements(ids) {


    for (var i = 0; i < ids.length; i++) {
        $(ids[i]).show();
		
    }
}

function AddBorderRight(ids) {
    for (var i = 0; i < ids.length; i++) {
        $(ids[i]).addClass('display-border-right');
    }
}
function AddLeftBorderRadius(ids) {
    for (var i = 0; i < ids.length; i++) {
        $(ids[i]).addClass('display-border-left-radius');
    }
}
function RemoveLeftBorderRadius(ids) {
    for (var i = 0; i < ids.length; i++) {
        $(ids[i]).removeClass('display-border-left-radius');
    }
}
function AddRightBorderRadius(ids) {
    for (var i = 0; i < ids.length; i++) {
        $(ids[i]).addClass('display-border-right-radius');
    }
}
function RemoveRightBorderRadius(ids) {
    for (var i = 0; i < ids.length; i++) {
        $(ids[i]).removeClass('display-border-right-radius');
    }
}
function unCheckSelections() {

				$('.ia-document-library-list tbody tr').removeClass('ia-doclist-row-selected');
				$('.ia-document-library-list tbody td input[type=checkbox]').prop('checked', false);
			
    selection(selectionbox, false); selection(allCheckbox, false); displayOptions();
}