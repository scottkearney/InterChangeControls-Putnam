function EndRequestGrid() {
    hideAllButton();

    //$('.ia-searchBox').each(function (i) {
    //    $(this).quicksearch('table#grid tbody tr', {
    //        noResults: '#noresults',
    //        'testQuery': function (query, txt, row) {
    //            if ($(row).children(":nth-child(2)").find('.disp-File-Name').length > 0)
    //                return $(row).children(":nth-child(2)").find('.disp-File-Name').text().toLowerCase().indexOf(query[0].toLowerCase()) >= 0;
    //            else
    //                return $(row).children(":nth-child(2)").find('.filename').text().toLowerCase().indexOf(query[0].toLowerCase()) >= 0;
    //            //return $(row).children(":nth-child(2)").find('.disp-File-Name').text().toLowerCase().indexOf(query[0].toLowerCase()) >= 0;
    //        }
    //    });
    //});
    if ($("table#grid tbody").find('tr:visible').length == 0) {
        $('#noresults').show();
    }
    $(".interAction .ia-document-library-list").each(function () {

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
        $('.interAction .ia-modal-inline-trigger').magnificPopup({
            type: 'inline',
            preloader: false,
            closeBtnInside: true,
            showCloseBtn: true,
            callbacks: {
                close: function () {
                    unCheckSelections();
                    UndoSelection();
                }
                // e.t.c.
            }
        });

        //On click of the "select all" checkbox
        $(".ia-doclist-selectAll").click(function () {
            // if this is already checked
            if (!$(this).is(":checked")) {
                $(".ia-document-library-list tbody tr").removeClass("ia-doclist-row-selected");
                $(".ia-document-library-list tbody td input[type=checkbox]").prop("checked", false);
            }
            if ($(this).is(":checked")) {
                $(".ia-document-library-list tbody tr").addClass("ia-doclist-row-selected");
                $(".ia-document-library-list tbody td input[type=checkbox]").prop("checked", true);
            }

        });

    });
    $('.ia-sort-asc').click(function () {
        showloader();
    });
}

$(function () {

    //$('.ia-searchBox').each(function (i) {
    //    $(this).quicksearch('table#grid tbody tr', {
    //        noResults: '#noresults',
    //        bind: 'keyup search',
    //        'testQuery': function (query, txt, row) {
    //            if ($(row).children(":nth-child(2)").find('.disp-File-Name').length > 0)
    //                return $(row).children(":nth-child(2)").find('.disp-File-Name').text().toLowerCase().indexOf(query[0].toLowerCase()) >= 0;
    //            else
    //                return $(row).children(":nth-child(2)").find('.filename').text().toLowerCase().indexOf(query[0].toLowerCase()) >= 0;
    //            //return $(row).children(":nth-child(2)").find('.disp-File-Name').text().toLowerCase().indexOf(query[0].toLowerCase()) >= 0;

    //        }
    //    });
    //});
    if ($("table#grid tbody").find('tr:visible').length == 0) {
        $('#noresults').show();
    }
})


function detect_enter(event) {
    if (event.keyCode == 13) {
        event.preventDefault();
    }
}

function myFunction(id, value) {
    if (document.getElementById(id) != null) {
        if ($("#hdnQueryQF"))
            $("#hdnQueryQF").attr("value", value);
    }
}

function myFunctionAuto(id, value) {
    if (document.getElementById(id) != null) {
        if ($("#autoCurrentFolder"))
            $("#autoCurrentFolder").attr("value", value);
    }
}
/*Grid Page End*/

/*Refiner starts*/
function pageLoad() {

    var cookieName = $('#akuminaCookieName').val();
    var cookieValue = getCookie(cookieName);
    if (cookieValue != "") {
        if (cookieValue.length > 4 && cookieValue.substr(-4) == "true") {

            deleteCookie(cookieName);
            commonload();
        }
        else {
            $('#DeltaPlaceHolderMain').hide();
            //$(".loader").hide();
            setCookieWithReload(cookieValue);

        }
    }
    else
        commonload();

}
function commonload() {
    if ($("#refreshIdleGrid").val() != "true") {
        if ($("#createpermission").val() != "yes")
            $(".ia-button-row").hide();
        else
            $(".ia-button-row").show();
        //var documentLibraries = getParameterByName('docLibName');
        //if (documentLibraries != "" && documentLibraries.toLowerCase() != "all") {
        //    var listofLibraries = documentLibraries.split(',');
        //    if (listofLibraries.length != 1) {
        //        if ($(".uploadCreateButtons").length > 0) {
        //            $(".uploadCreateButtons").hide();

        //        }
        //    }
        //}
        //else {
        //    if ($(".uploadCreateButtons").length > 0) {

        //        $(".uploadCreateButtons").hide();
        //    }
        //}
        $('#grid').append("<colgroup><col class='ia-col-width-5'><col style='ia-col-width-50'><col style='ia-col-width-20'><col style='ia-col-width-25'></colgroup>");
        if (document.getElementById("hdnQuerystatus") != null)
            document.getElementById("hdnQuerystatus").value = "";
        if ($('#refinerReset'))
            $('#refinerReset').val("");
        if ($('#refreshIdleTab'))
            $('#refreshIdleTab').val("");
        if ($('#refreshIdleRF'))
            $('#refreshIdleRF').val("");
        if ($('#refreshIdleGrid'))
            $('#refreshIdleGrid').val("");
        if ($('#refreshIdleFT'))
            $('#refreshIdleFT').val("");
        $(".ia-doclist-name").width(Math.round($(".ia-documentList").width() * (0.4)));

    }
    else {
        showloader();
        if ($('#refreshIdleTab'))
            $('#refreshIdleTab').val("");
        if ($('#refreshIdleRF'))
            $('#refreshIdleRF').val("");
        if ($('#refreshIdleGrid'))
            $('#refreshIdleGrid').val("");
        if ($('#refreshIdleFT'))
            $('#refreshIdleFT').val("");
    }
    if ($("#lblID"))
        $("#lblID").attr("value");
    if ($(".subsite-header").length > 0) {
        $(".contentBody").prev(".subsite-header").each(function () {
            $(this).remove();
        });
        $(".contentBody").before($(".subsite-header"));
        $(".subsite-header").attr("style", "display:block");
    }

    tabmenuhighlight();
    removeBreadCrumb();
    librarySearch();

    var currentfolder = "";
    if (document.getElementById("folderBox") != null) currentfolder = $("#folderBox").attr("value");
    else if ($("#currentFolderPath").prop("value")) currentfolder = $("#currentFolderPath").attr("value");
    else
        currentfolder = $("#listName").attr("value");
    if ($("#hdnPathQF"))
        $("#hdnPathQF").attr("value", currentfolder);
    if ($("#currentFolderPath"))
        $("#currentFolderPath").attr("value", currentfolder);

    var folderName = currentfolder.split("/");
    if ($(".lblBreadCrum")) {
        if (folderName.length > 1) { $(".lblBreadCrum").html(folderName[folderName.length - 1]); }
        else
            $(".lblBreadCrum").html($("#listDisplayName").attr("value"));
    }
    if ($('.interAction .ia-search-library-site-name').length > 0) {
        $('.interAction .ia-search-library-site-name').click();
        $('.interAction .ia-search-library-site-name').click();
    }
    //$('.ia-searchBox').on("keyup", function (event) {

    //    var val = $(this).val();
    //    if (val.length < 3)
    //        val = "";

    //    if (val != "" || event.keyCode == 8 || event.keyCode == 46 || (typeof (event.keyCode) == "undefined")) {
    //        showloader();
    //        displayitemswithkey(val);
    //        setTimeout(function () { completedsearch(); }, 100);
    //        setTimeout(function () { hideloader(); }, 100);
    //    }


    //});

    $('.ia-searchBox').on("keyup", function (event) {

        var val = $(this).val();
        //if (val != "") {
        if (val.length < 3)
            val = "";

    

        if (val != "" || event.keyCode == 8 || event.keyCode == 46 || (typeof (event.keyCode) == "undefined")) {
            showloader();
            displayitemswithkey(val);
         
            setTimeout(function () { completedsearch(); }, 100);
            setTimeout(function () { hideloader(); }, 100);
        }
        //}
        //else {
        //    if ($("table#grid tbody").find('tr:visible').length == 0) {
        //        $('#noresults').show();
        //    }
        //    else {
        //        $('#noresults').hide();
        //    }
        //}
    });

    GetMetaDataTreeView();
    hideDocumentListColumns();

    searchkeyup();
    fnSorting();
    hideloader();
    fadeOutUploadSuccess();
    //if ($("#hdnUploadedFiles"))
    //    $("#hdnUploadedFiles").attr("value", "");
    $('#grid').table().data("table");
}

function removeBreadCrumb() {
    //var arr = [];
    //var param_folderTree = Sys.WebForms.PageRequestManager.getInstance();
    //param_folderTree.add_endRequest(EndRequest);
    //arr = param_folderTree._updatePanelIDs;
    //var folderTreeSet = false;
    //$.each(arr, function (key, value) {
    //    if (value.indexOf("upFolderTree") > -1) {
    //       folderTreeSet = true;
    //   }
    //});
    if ($('#folderzone').length > 0) {
        $(".subsite-header").hide();
    }
}

function hideDocumentListColumns() {

    if ($("#hdnColumnsToHide").val() != "") {
        var columnsList = $("#hdnColumnsToHide").val().split(',');

        for (var i = 0; i < columnsList.length; i++) {
            $('.' + columnsList[i]).hide();
        }
    }


}
function fadeOutUploadSuccess(message) {
    if (message == null) {
        if ($("#displayUploadSuccess").length > 0 && $("#displayUploadSuccess").val() != "") {
            if ($('#uploadMessage').length > 0) {
                if (($('.uploaded_Success').length > 0 && $('.uploaded_Success').justtext().trim() != "") || ($('.serverSideMessage').length > 0 && $('.serverSideMessage').justtext().trim() != "")) {
                    $('.uploaded_Success').html($('.uploaded_Success').html() + $('.serverSideMessage').html());
                    if ($('#uploadSucess').length > 0)
                        $('#uploadSucess').attr("class", "ia-upload-error");
                }
                else {
                    if ($('.uploaded_Success').length > 0)
                        $('.uploaded_Success').html("Files Uploaded Successfully.");

                    if ($('#uploadSucess').length > 0)
                        $('#uploadSucess').attr("class", "ia-upload-success");
                }
                $('#uploadMessage').show();
                $('#uploadMessage').fadeOut(10000, function () {
                    if ($('.uploaded_Success').length > 0)
                        $('.uploaded_Success').html("");
                });

            }
            $("#displayUploadSuccess").val("");
        }
        else {
            if ($('.uploaded_Success').length > 0)
                $('.uploaded_Success').html("");

        }
    }
    else {
        if ($('.uploaded_Success').length > 0)
            $('.uploaded_Success').html(message);

        if ($('#uploadSucess').length > 0)
            $('#uploadSucess').attr("class", "ia-upload-error");
        $('#uploadMessage').show();
        $('#uploadMessage').fadeOut(10000, function () {
            if ($('.uploaded_Success').length > 0)
                $('.uploaded_Success').html("");
        });
    }
}
function copyOfsearchTrigger(modfiedUsers) {


    var val = $('.ia-searchBox').val();
    if (val.length < 3)
        val = "";

    showloader();
    displayitemswithkey(val);
  
    setTimeout(function () { completedsearch(modfiedUsers); }, 100);
    setTimeout(function () { hideloader(); }, 100);
}

function EndRequestRefiner() {
    $(".ia-datepicker").pickadate({
        selectYears: true,
        selectMonths: true,
        format: "mmm dd, yyyy"
    });

    $(".ia-filer-name-list a").click(function (e) {
        $(this).fadeOut(300, function () { $(this).remove(); });
        e.preventDefault();
    });
}


/*Refiner End*/
/*Folder Tree Starts*/

$(document).ready(function () {
    $(".ia-folder-tree").jstree();
    $(".ia-folder-tree").show();
    expandTreeView();

});

function EndRequestFolderTree() {
    $(".ia-folder-tree").jstree();
    $(".ia-folder-tree").show();
    expandTreeView();

}

function expandTreeView() {
    if (document.getElementById("folderBox") != null) {


        var folderurl = $("#folderBox").attr("value").split("/");
        var currentUrl = "", i, currentFolder;

        for (i = 0; i < folderurl.length; i++) {

            currentUrl += folderurl[i];
            currentFolder = $('a.jstree-anchor[url-data="' + currentUrl + '"]');
            if (currentUrl == $("#folderBox").attr("value"))
                currentFolder.addClass("jstree-clicked");
            else {

                if (currentFolder.parent().hasClass("jstree-closed")) { currentFolder.prev().click(); }

            }
            currentUrl += "/";
        }
        var currentFolder1 = $('a.jstree-anchor[url-data="' + folderurl[0] + '"]');
        if (!currentFolder1.hasClass("jstree-clicked") && currentFolder1.parent().find("ul").length == 0) {
            currentFolder1.prev().click();
        }



    }
    //autoCompleteSearch();

}

/*End*/


/*tab starts*/
function tabHighlight(tabid, element) {
    var tabquery = $(element).attr("tab_val");
    window.location.href = "#";
    showloader();


    //if (tabid == "0") {
    //if ($('#txtSearch'))
    //    $('#txtSearch').val('');
    //}
    $("#lblID").attr("value", tabid);
    //if ($("#txtSearch")) {
    //    $("#txtSearch").attr("title", $("#txtSearch").val());
    //    $("#txtSearch").val("");
    //}
    //if ($('#ddlSelect'))
    //    $('#ddlSelect').val("This Folder");

    //if ($("#hdnQueryQF"))
    //    $("#hdnQueryQF").attr("value", "");
    //if ($("#hdnQuerystatus"))
    //    $("#hdnQuerystatus").attr("value", "reset");
    //$("#tabQuery").val(tabquery);
    //$("#btnTabQuery").click();
    tabmenuhighlight();
    searchkeyup();
}

var windowElem = "#s4-workspace";
var elem = ".sticky-wrapper";
var ribbonElem = "#s4-ribbonrow";
function sticyfix() {

    if ($('.sticky-wrapper').length > 0)
        $('.sticky-wrapper').css("height", "");


    $(windowElem).scroll(function () {
        if ($('.sticky-wrapper').length > 0)
            $('.sticky-wrapper').css("height", "");


    });


}