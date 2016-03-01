$(document).ready(function () {
    //Metadata Treeview
    $("div.ia-treeview").each(function () {
        //if ($(this).find("ul.ia-treeview").length == 0) {
        $(this).find("ul").treeview({
            persist: "location",
            collapsed: true
        });
        //}
    });

});

function GetMetaDataTreeView() {

    //Add a selected class to the selected li span item (for use with "Add Item")
    $(".ia-treeview ul li span").click(function (e) {
        $(".ia-treeview ul").find('span').removeClass('ia-selected');
        $(this).addClass('ia-selected');
        //add the selected label text to the Add Item popup
        var selectedLabel = $(this).text();
        $(".ia-selected-label").html(selectedLabel);
    });


    //Populate the currently selected block with the selected options.
    $('.ia-treeview').each(function () {
        var currentSelected = $(this).next('.ia-current-selected').children('.ia-current-selected-list');
        var hiddenSelected = $(this).next('.ia-current-selected').children('.selectedTaxonomy');
        var treeview = $(this);
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

    //SetDefaultCategoryValue();
}

