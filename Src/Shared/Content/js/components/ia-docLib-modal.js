//CN= ClassName of the field

var CN_errorField = "ia-field-error"; var CN_startDate = ".setStartDate"; var CN_endDate = ".setExpireDate";
var CN_Category_input = ".ia-upload-category-input";
var CN_endDate = ".setExpireDate";
var selectedMetadataElement;
function FieldValidation(e) {
    var i = 0;

    $('.ia-library-upload-reqs .ia-form').find("input[type=text]:visible").each(function () {

        if ($(this).val() == "") {
            i = i + 1;

            if (!$(this).parents('.ia-form-row').hasClass(CN_errorField))
                $(this).parents('.ia-form-row').addClass(CN_errorField);

        }
        else {
            $(this).parents('.ia-form-row').removeClass(CN_errorField);
        }


    });
    $('.ia-library-upload-reqs .ia-form').find("textarea").each(function () {

        if ($(this).val() == "") {
            i = i + 1;

            if (!$(this).parents('.ia-form-row').hasClass(CN_errorField))
                $(this).parents('.ia-form-row').addClass(CN_errorField);

        }
        else {
            $(this).parents('.ia-form-row').removeClass(CN_errorField);
        }


    });

    //if (i > 0)
    //    $(".ia-form-error").show();
    //else
    //    $(".ia-form-error").hide();
    if (i > 0 && e != null)
        return false;
    else
        return true;
}




function FileCheckinStep(e) {
    //var validCheck = true; var categoryCheck = true;
    //if ($(".setExpireDate").length > 0 && $(".setExpireDate").val() == "") {
    //    validCheck = false;
    //}

    //if ($(".setStartDate").length > 0 && $(".setStartDate").val() == "") {
    //    validCheck = false;
    //}
    //if ($(".ia-upload-category-input").length > 0 && $(".ia-upload-category-input").val() == "") {
    //    validCheck = false;
    //}
    //if (validCheck == true) {
    var validation = FieldValidation(e);
    if (validation) {
        $.magnificPopup.close(); // Close the Step 1 browse popup
        if ($('#hdnMinorCheckInEnable').length > 0 && $('#hdnMinorCheckInEnable').val() == "minor") {
            if ($("#lstForceCheckout").length > 0 && $('#lstForceCheckout').val() == "false") {
                if ($(".ia-step2 input[value='dontCheckIn']").length > 0) {
                    $(".ia-step2 input[value='dontCheckIn']").parent().parent().hide();
                }
                //if ($(".ia-step2 input[value='checkInFile']").length > 0) {
                //    $(".ia-step2 input[value='checkInFile']").prop("checked", true).trigger("change");
                //}
            }
            $.magnificPopup.open({  // Open step 2
                items: {
                    src: '.ia-step2',
                    type: 'inline'
                },
                closeOnBgClick: false,
                closeBtnInside: true
            });

        }
        else if ($('#hdnMinorCheckInEnable').length > 0 && $('#hdnMinorCheckInEnable').val() == "major") {

            if ($('.dd_fileCheckinNo').length > 0)
                $('.dd_fileCheckinNo').click();
            $.magnificPopup.open({
                items: {
                    src: '.dd_fileCheckInMajorOption',

                    type: 'inline',

                },
                closeOnBgClick: false,
                closeBtnInside: true
            });
        }

        else {

            $.magnificPopup.open({  // Open step 3
                items: {
                    src: '.ia-step3',
                    type: 'inline'
                },
                closeOnBgClick: false,
                showCloseBtn: false
            });
            FilesUploadToLibrary(e);
        }
    }
    else {
        $(".ia-form-error").show();
    }
    //}
    //else {
    //    $(".ia-form-error").show();
    //    if ($(".setExpireDate").length > 0 && $(".setExpireDate").val() == "") {
    //        $(".expire-error").addClass(CN_errorField);

    //    }
    //    if ($(".setStartDate").length > 0 && $(".setStartDate").val() == "") {
    //        $(".startDate-error").addClass(CN_errorField);
    //    }
    //    if ($(".ia-upload-category-input").length > 0 && $(".ia-upload-category-input").val() == "") {
    //        $(".category-error").addClass(CN_errorField);
    //    }
    //}
}
function MajorCheckinWindow(e) {
    $.magnificPopup.close();
    $.magnificPopup.open({  // Open step 3
        items: {
            src: '.ia-step3',
            type: 'inline'
        },
        closeOnBgClick: false,
        showCloseBtn: false
    });
    FilesUploadToLibrary(e);
}

function ClearCheck() {

    if ($('.checkInCheckoutDraft').length > 0)
        $('.checkInCheckoutDraft').click();

    if ($('.checkInFileOption').length > 0)
        $('.checkInFileOption').click();

    if ($('.checkInDraftOption').length > 0)
        $('.checkInDraftOption').click();
    if ($('.ia-checkin').length > 0)
        $('.ia-checkin').click();
    if ($('.checkInDraft').length > 0)
        $('.checkInDraft').click();

    if ($('.conflictDlg').length > 0)
        $('.conflictDlg').prop("checked", false);

    $('.ia-checkin-all').show();
    $('.ia-checkin-changes').hide();
    if ($('.checkInCheckoutDraft').length > 0)
        $('.checkInCheckoutDraft').click();
    if ($('.checkInCheckoutDraftOption').length > 0)
        $('.checkInCheckoutDraftOption').click();




}

$(document).ready(function () {

    $(document).on('click', '#ia-library-upload', function (e) {

        dispUploadPopup(e);


    });

    $(document).on('click', '.category_browse', function (e) {

        var categorydiv = $('.refinerCategory');
        var selectedTaxonomy = $(this).prev();
        if (categorydiv.length > 0) {




            $(categorydiv).find("input[type='checkbox']:checked").each(function () {
                $(categorydiv).find("input[type='checkbox']:checked").prop('checked', false);
            });


            if (selectedTaxonomy.length > 0) {
                var selectedValue = $(selectedTaxonomy).val().trim();
                if (selectedValue != "") {
                    var selectedCategoryItems = selectedValue.split(',');
                    for (var i = 0; i < selectedCategoryItems.length; i++) {
                        if ($(categorydiv).find("input[value='" + selectedCategoryItems[i] + "']").length > 0) {
                            $(categorydiv).find("input[value='" + selectedCategoryItems[i] + "']").click();
                        }
                    }
                }
                else {
                    $(categorydiv).children('.ia-current-selected').children('.ia-current-selected-list').html("");
                    $(categorydiv).children('.ia-current-selected').children('.selectedTaxonomy').val("");

                }
            }

        }

    });

    function dispUploadPopup(e) {
        ClearCheck();
        if ($(".ia-step1").length > 0) {
            SetDefaultValueToRequiredFields();
            $.magnificPopup.open({
                items: {
                    src: '.ia-step1',

                    type: 'inline',

                },
                closeOnBgClick: false,
                closeBtnInside: true
            });
        }
        else {
            FileCheckinStep(e);
        }
    }
    GetPopupFunctions();


});

function GetPopupFunctions() {
    //OK Button on Category Browse popup
    $('.interAction .ia-upload-category-confirm').click(function (e) {
        e.preventDefault();
        $.magnificPopup.close(); // Close the category browse popup

        $.magnificPopup.open({  // Re-open step 1
            items: {
                src: '.ia-step1',
                type: 'inline'
            },
            closeOnBgClick: false,
            closeBtnInside: true
        });
        // Populate Categories input box with selections from popup
        var fieldname = $(".gridCategory .ax-treeview").attr("field-name");
        if ($("input[field-name='" + fieldname + "'].ia-upload-category-input").length > 0) {
            var ele = $("input[field-name='" + fieldname + "'].ia-upload-category-input");

            var selectedCats = $('#upload-category-popup .ia-current-selected-list').text();
            var selectedCatVals = $('#upload-category-popup .selectedTaxonomy').val();
            $(ele).val(selectedCats);
            $(ele).attr("selected-taxonomies", selectedCatVals);
            $("#" + fieldname + "_Hidden").val(selectedCatVals);
            if ($(ele).val() == "") {


                if (!$(ele).parents('.ia-form-row').hasClass(CN_errorField))
                    $(ele).parents('.ia-form-row').addClass(CN_errorField);


            }
            else {
                $(ele).parents('.ia-form-row').removeClass(CN_errorField);
                $(".ia-form-error").hide();
            }
        }


        //$('.ia-upload-category-input').val(selectedCats);
        //FieldValidation(e);
    });

    //Cancel Button on Category Browse popup
    $('.interAction .ia-upload-category-cancel').click(function (e) {
        e.preventDefault();
        $.magnificPopup.close(); // Close the category browse popup
        $.magnificPopup.open({  // Re-open step 1
            items: {
                src: '.ia-step1',
                type: 'inline'
            },
            closeOnBgClick: false,
            closeBtnInside: true
        });
    });

    //Next Button on Step 1 popup
    $('.interAction .ia-upload-confirm').click(function (e) {
        e.preventDefault();
        FileCheckinStep(e);
    });

    //Cancel Button on Step 1 popup
    $('.interAction .ia-upload-cancel').click(function (e) {
        e.preventDefault();
        $.magnificPopup.close(); // Close the Step 1 popup

    });

    //Next Button on Step 2 popup
    $('.interAction .ia-checkin-confirm').click(function (e) {
        e.preventDefault();
        $.magnificPopup.close(); // Close the Step 2 checkin popup

        $.magnificPopup.open({  // Open step 3
            items: {
                src: '.ia-step3',
                type: 'inline'
            },
            closeOnBgClick: false,
            showCloseBtn: false
        });
        FilesUploadToLibrary(e);
    });

    //Cancel Button on Step 2 popup
    $('.interAction .ia-checkin-cancel').click(function (e) {
        e.preventDefault();
        $.magnificPopup.close(); // Close the Step 2 popup

    });

    // Check-in radio button options
    $('input[type="radio"].ia-checkin').click(function () {
        if ($(this).is(':checked')) {
            $('.ia-checkin-all').show();
            $('.ia-checkin-changes').hide();
        }
        else {
            $('.ia-checkin-all').hide();
        }

    });
    $('input[type="radio"].ia-checkin-checkout').click(function () {
        if ($(this).is(':checked')) {
            $('.ia-checkin-changes').show();
            $('.ia-checkin-all').hide();
        }
        else {
            $('.ia-checkin-changes').hide();
        }

    });
    $('input[type="radio"].ia-no-checkin').click(function () {
        if ($(this).is(':checked')) {
            $('.ia-checkin-changes').hide();
            $('.ia-checkin-all').hide();

        }
    });
    $('.ia-form').find('.ia-datepicker').each(function () {
        $(this).change(function () {
            //FieldValidation();
            if ($(this).val() == "") {
                if (!$(this).parents('.ia-form-row').hasClass(CN_errorField))
                    $(this).parents('.ia-form-row').addClass(CN_errorField);

            }
            else {
                $(this).parents('.ia-form-row').removeClass(CN_errorField);
            }
        });
    });

}

function IntializeEmptyValues() {

    ReintializeCheckInComment();

    //$('.ia-form').each(function () {

    //    $(this).find("input[type='text']").each(function () {

    //        $(this).val("");

    //    });
    //});

    $('.ia-form').each(function () {
        $('.' + CN_errorField).each(function () {
            $(this).removeClass(CN_errorField);
        });
    });

    if ($(".gridCategory").find("input[type=checkbox]:checked").length > 0) {
        $(".gridCategory").find("input[type=checkbox]:checked").each(function (ind, val) {
            //arrTemp.push(" <span>" + $(val).next().html() + "</span>");
            //arrTempVal.push($(val).val());
            $(this).click();
        });
    }

    //SetDefaultCategoryValue();
    if ($(".ia-form-error"))
        $(".ia-form-error").hide();

    //if ($(".setExpireDate").length > 0) {
    //    $(".setExpireDate").val("");
    //}
    //if ($(".setStartDate").length > 0) {
    //    $(".setStartDate").val("");
    //}
    //if ($('.ia-upload-category-input').length > 0)
    //    $('.ia-upload-category-input').val("");
    //if ($(".expire-error").length > 0 && $(".expire-error").hasClass(CN_errorField)) {
    //    $(".expire-error").removeClass(CN_errorField);

    //}
    //if ($(".startDate-error").length > 0 && $(".startDate-error").hasClass(CN_errorField)) {
    //    $(".startDate-error").removeClass(CN_errorField);
    //}
    //if ($(".category-error").length > 0 && $(".category-error").hasClass(CN_errorField)) {
    //    $(".category-error").removeClass(CN_errorField);
    //}

}
function ReintializeCheckInComment() {
    //if ($(".checkinCommentOption").length > 0)
    //    $('.checkinCommentOption').val("");
    //if ($(".checkInCheckOutCommentOption").length > 0)
    //    $('.checkInCheckOutCommentOption').val("");
    //if ($(".checkinComment").length > 0)
    //    $('.checkinComment').val("");
    //if ($(".checkInCheckOutComment").length > 0)
    //    $('.checkInCheckOutComment').val("");
    $('.ia-form').each(function () {

        $(this).find("textarea.ax-checkincomment").each(function () {

            $(this).val("");

        });
    });




}

function getCategoryModel(ele) {
    var categoryelement = $(ele).prev();
    var fieldName = categoryelement.attr("field-name");
    var associatedcategory = categoryelement.attr("associated-taxonomy");
    var selectedcategory = categoryelement.attr("selected-taxonomies");
    $(".gridCategory .ax-treeview").html("");
    $(".gridCategory .ax-treeview").attr("field-name", fieldName);

    var selectedtree = "";

    if ($(".ax-category-tree").find("input[value='" + associatedcategory + "']").length > 0) {
        if ($(".ax-category-tree").find("input[value='" + associatedcategory + "']").parent().find("ul").length > 0) {
            selectedtree = "<li>" + $(".ax-category-tree").find("input[value='" + associatedcategory + "']").parent().html() + "</li>";
        }


    }
    else if ($(".ax-category-tree").find("ul[category='" + associatedcategory + "']").length > 0) {
        selectedtree = $(".ax-category-tree").find("ul[category='" + associatedcategory + "']").html();
    }
    $(".gridCategory .ax-treeview").html("<ul>" + selectedtree + "</ul>");
    if ($(".gridCategory .ax-treeview").find("input[value='" + associatedcategory + "']").length > 0) {
        $(".gridCategory .ax-treeview").find("input[value='" + associatedcategory + "']").attr("disabled", "");
    }
    $(".gridCategory .ax-treeview ul").treeview({
        persist: "location",
        collapsed: true
    });
    $('.ax-treeview').each(function () {
        var currentSelected = $(this).next('.ia-current-selected').children('.ia-current-selected-list');
        var hiddenSelected = $(this).next('.ia-current-selected').children('.selectedTaxonomy');
        var treeview = $(this);
        $(currentSelected).html('');
        $(hiddenSelected).html('');
        //When checkboxes change, update the Currently selected area with current selections
        $(this).find("input[type=checkbox]").on("change", function () {

            $(currentSelected).html('');
            $(hiddenSelected).html('');
            var arrTemp = [];
            var arrTempVal = [];
            $(treeview).find("input[type=checkbox]:checked").each(function (ind, val) {
                arrTemp.push(" <span>" + $(val).next().html() + "</span>");
                arrTempVal.push($(val).val());
            });

            $(currentSelected).append(arrTemp.join());
            $(hiddenSelected).val(arrTempVal.join());

        });



    });
    if (selectedcategory != "") {

        SetValueToInput(selectedcategory);
        var TaxSelectedNames = $('.ax-treeview').next('.ia-current-selected').children('.ia-current-selected-list').text();
        var TaxSelectedPath = $('.ax-treeview').next('.ia-current-selected').children('.selectedTaxonomy').val();
        $(categoryelement).attr("selected-taxonomies", TaxSelectedPath);
        $(categoryelement).val(TaxSelectedNames);
    }
    else {
    }



}


function SetDefaultValueToRequiredFields() {

    if ($(".pnlRequiredFields").length > 0) {
        $(".pnlRequiredFields").find('.ia-upload-category-input').each(function () {
            var pathANdValue = $(this).attr("default-value");
            if (pathANdValue != "") {
                var pathANdVal = pathANdValue.split('|');
                $(this).attr("selected-taxonomies", pathANdVal[1]);
                $(this).val(pathANdVal[0]);

            }
            else {
                $(this).attr("selected-taxonomies", "");
                $(this).val("");
            }
        });
        $(".pnlRequiredFields").find('.ax-textbox').each(function () {
            var defaultValue = $(this).attr("default-value");
            if (defaultValue != "") {
                $(this).val(defaultValue);
            }
            else
                $(this).val("");

        });
        $(".pnlRequiredFields").find('textarea').each(function () {
            var defaultValue = $(this).attr("default-value");
            if (defaultValue != "") {
                $(this).val(defaultValue);
            }
            else
                $(this).val("");

        });

    }


}
function SetValueToInput(defaultVal) {
    var values = defaultVal.split(',');
    for (var i = 0; i < values.length; i++) {
        if ($(".gridCategory").find("input[value='" + values[i] + "']").length > 0) {
            $(".gridCategory").find("input[value='" + values[i] + "']").click();

        }
    }


}