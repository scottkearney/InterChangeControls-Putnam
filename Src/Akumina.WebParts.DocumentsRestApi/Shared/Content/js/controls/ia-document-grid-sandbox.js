var wordMapyKey = "Word", powerPointMapKey = "PowerPoint", excelMapKey = "Excel";
var mapArray = [wordMapyKey, powerPointMapKey, excelMapKey];
var word = ['docx', 'doc', 'docm', 'dot', 'nws', 'dotx'],
         powerPoint = ['odp', 'ppt', 'pptm', 'pptx', 'potm', 'ppam', 'ppsm', 'ppsx'],
         excel = ['odc', 'xls', 'xlsb', 'xlsm', 'xlsx', 'xltm', 'xltx', 'xlam'];
var queryFields = "?$select=Folder/ProgID,EncodedAbsUrl,EditorId,FileLeafRef,id,Category,Modified,Editor/Title,EffectiveBasePermissions,File_x0020_Type,CheckoutUserId&$expand=Editor,Folder&$top=100000000";

var styleUrl = "";
var str = "";
var mapIconUrls;
var listItemsJson = {};
var listname = "";// $('#listDisplayName').val();

var baseUrl = ""// $('#webUrl').val();

$(window).load(function () {
    var cookieName = $('#akuminaCookieName').val();

    var cookieValue = getCookie(cookieName);
    if (cookieValue != "") {
        showloader();
        setValuesToFields(cookieName, cookieValue);
        deleteCookie(cookieName);
    }
});


jQuery.fn.justtext = function () {

    return $(this).clone()
            .children()
            .remove()
            .end()
            .text();

};

function BindColumnNames() {
    var columnNames = $("#columnNames").val();
    if ($("table#ogrid thead tr").length > 0) {
        var columns = columnNames.split(',');
        if (columns.length > 0) {
            $("th.ia-doclib-header-name span").html(columns[0]);
            $("th.ia-doclib-header-modified span").html(columns[1]);
            $("th.ia-doclib-header-modifiedBy span").html(columns[2]);
        }
    }
}
$(document).ready(function () {
    showloader();
    styleUrl = $("#styleUrl").html();
    str = $("#docIconUrlMap").html().replace(/\s+/g, '');
    mapIconUrls = JSON.parse(str);
    listname = $('#listDisplayName').val();
    baseUrl = $('#webUrl').val();
    getListItems(baseUrl, listname, queryFields);
    $('.ia-searchBox').on("keyup", function (event) {

        var val = $(this).val();
        if (val.length < 3)
            val = "";

        if (val != "" || event.keyCode == 8 || event.keyCode == 46 || (typeof (event.keyCode) == "undefined")) {
            showloader();
            displayitemswithkey(val);
            setTimeout(function () { completedsearch(); }, 100);
            setTimeout(function () { hideloader(); }, 100);
        }


    });


});

function displayitemswithkey(val) {
    var documentclassName = "ia-doclist-name";

    if (val == "")
        $("table#ogrid tbody tr").show();
    else {
        if ($("table#ogrid tbody").find('tr').length > 0) {
            $("table#ogrid tbody").find('tr').each(function () {
                var key = $(this).find("." + documentclassName + " a").length > 0 ? $(this).find("." + documentclassName + " a").html() : $(this).find("." + documentclassName).justtext();

                if (key.toLowerCase().indexOf(val.toLowerCase()) >= 0) {
                    $(this).show();
                }
                else {
                    $(this).hide();

                }


            });
        }
    }


}




function setValuesToFields(cookieName, cookieValue) {
    var arr = cookieValue.split('!');
    var searchtext = "";//arr[0];
    var recursive = arr[1];
    var fd = arr[2];
    var qy = arr[3];
    var tab = arr[4];
    var tabID = arr[5];
    var fdreset = true, tabreset = true, qyreset = true;
    var urlQuery = "?fd=" + fd + "&recursive=" + recursive;//+ "&qy=" + qy + "&tab=" + tab + "&recursive=" + recursive + "&fdreset=" + fdreset + "&tabreset=" + tabreset + "&qyreset=" + qyreset;
    urlQuery = urlQuery.replace(/#/g, "$");
    var form = document.getElementById('aspnetForm');
    var action = form.getAttribute("action") + urlQuery;
    $.ajax({
        type: "GET",
        url: action,
        success: function (data) {
            BindHtml(data, fdreset, tabreset, qyreset, searchtext, recursive, cookieName, fd + ";" + tabID);
        },
        error: function () {
            showloader();
            console.log("Data didn't get sent!!");
        }
    })
}

function setCookie() {
    var cvalue = GetUrl();
    var cookieName = $('#akuminaCookieName').val();
    var d = new Date();
    d.setTime(d.getTime() + (1 * 24 * 60 * 60 * 1000));
    var expires = "expires=" + d.toGMTString();
    document.cookie = cookieName + "=" + cvalue + "; " + expires;
}

function getCookie(cname) {

    var name = cname + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') c = c.substring(1);
        if (c.indexOf(name) == 0) {
            return c.substring(name.length, c.length);
        }
    }
    return "";
}

function deleteCookie(c_name) {
    document.cookie = encodeURIComponent(c_name) + "=deleted; expires=" + new Date(0).toUTCString();
}
function pageloadfn() {

    var cookieName = $('#akuminaCookieName').val();
    var cookieValue = getCookie(cookieName);
    if (cookieValue != "") {
        //showloader();
        //setValuesToFields(cookieName,cookieValue);
        //deleteCookie(cookieName);
        //location.reload();
        //expandTreeView(); 
    }
    $('.ia-datepicker').pickadate({
        selectYears: true,
        selectMonths: true,
        format: 'mmm dd, yyyy'
    });
    if ($("#createpermission").val() != "yes")
        $(".ia-button-row").hide();
    else
        $(".ia-button-row").show();

}

function AfterGrid() {

    var node; headerhtml = "";
    if ($("table#ogrid").find('tr').length > 0) {
        $('#noresults').hide();
        // node=$("table#ogrid tbody tr:first");
        // headerhtml=$(node).html();
        // $("table#ogrid").prepend("<thead><tr>"+headerhtml+"</tr></thead>");
        //$(node).remove();
        // if ($.browser.msie) {

        $("table#ogrid thead tr th").each(function () {

            if (!$(this).hasClass("ia-doclib-header-checkbox")) {
                $(this).addClass("header");
            }
        });
        $("table#ogrid thead tr th").click(function () {

            if (!$(this).hasClass("ia-doclib-header-checkbox")) {

                var sortorder = $(this).hasClass("headerSortUp") ? true : false; var sortcolumn = ""; var type = "string";
                $("table#ogrid thead tr th.headerSortUp").each(function () {
                    $(this).removeClass("headerSortUp");

                });
                $("table#ogrid thead tr th.headerSortDown").each(function () {
                    $(this).removeClass("headerSortDown");

                });
                if (sortorder)
                    $(this).addClass("headerSortDown");
                else
                    $(this).addClass("headerSortUp");
                if ($(this).hasClass("ia-doclib-header-name")) {
                    sortcolumn = "ia-doclist-name";
                }
                else if ($(this).hasClass("ia-doclib-header-modified")) {
                    type = "date";
                    sortcolumn = "ia-doc-list-modified";
                }
                else if ($(this).hasClass("ia-doclib-header-modifiedBy")) {

                    sortcolumn = "ia-doc-list-modifiedBy";

                }

                getsortedcolumns(sortcolumn, sortorder, type);
                //getPostBackSort(false, false, false, sortcolumn, sortorder);
            }
        });
        // }
        // else{
        // var $table= $("table#ogrid").tablesorter({  dateFormat : "us",
        // headers: {
        // 0: { sorter: false }
        // }
        // });

        // //Default sorting
        // $("th.ia-doclib-header-name").addClass("temp");

        // $table.bind('sortStart',function(){

        // showloader();
        // });

        // // bind an event to the sortEnd event of tablesorter
        // $table.bind('sortEnd',function(){
        // if(!$("th.ia-doclib-header-name").hasClass("temp")){
        // var i=$("th.headerSortDown").length;
        // var j=0;
        // $("a.filename").each(function() {
        // dummyrow=$(this).parent().parent().parent();
        // if(i==0)
        // $('tbody', $table).append(dummyrow);
        // else{
        // $('tbody tr', $table).eq(j).before(dummyrow);
        // j=j+1;
        // }
        // });
        // }
        // else
        // {
        // $("th.ia-doclib-header-name").removeClass("temp");
        // $("th.ia-doclib-header-name").trigger("click");
        // }				 
        // hideloader();

        // });
        // }




        searchkeyup();
        if (($("table#ogrid tbody").find('tr').length > 0 && $("table#ogrid tbody").find('tr:visible').length == 0) || $("table#ogrid tbody").find('tr').length == 0) {
            $('#noresults').show();
        }
        var hidecolumnlen = parseInt($("#columnToHidden").val());
        for (i = 0; i < hidecolumnlen; i++) {

            if ($("table#ogrid thead tr th").length >= (3 - i)) {

                $("table#ogrid thead tr").find("th").eq(3 - i).hide();
            }
            if ($("table#ogrid tbody tr td").length >= (3 - i)) {
                var classname = $("table#ogrid tbody tr").find("td").eq(3 - i).attr("class");
                $("." + classname).hide();
            }


        }

    }
    else
        $('#noresults').show();


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
    //$(".ia-doclist-name").width(Math.round($(".ia-documentList").width() *(0.4)));
    rowhighlight();
    loadDragfn();
    setTimeout(function () { hideloader(); }, 100);
}


function getsortedcolumns(sortcolumn, sortorder, type) {



    if ($("table#ogrid tbody").find('tr').length > 0 && $("table#ogrid tbody").find('tr:visible').length > 0) {
        showloader(); var arrSort = []; var foldersort = [];
        $("table#ogrid tbody").find('tr:visible').each(function () {
            var key = $(this).find("." + sortcolumn + " a").length > 0 ? $(this).find("." + sortcolumn + " a").html() : $(this).find("." + sortcolumn).justtext();
            var folder = ($(this).find("a.filename").length > 0) || $(this).find(".itemhiddenFileType").justtext() == "one";
            var obj = {};
            obj.html = $(this).html();
            obj.key = key.toLowerCase();
            if (folder)
                foldersort.push(obj);
            else
                arrSort.push(obj);

        });
        if (type == "string") {
            if (arrSort.length > 0)
                arrSort = stringSorting(arrSort, sortorder);
            if (foldersort.length > 0)
                foldersort = stringSorting(foldersort, sortorder);
        }

        else if (type == "date") {
            if (arrSort.length > 0)
                arrSort = dateSorting(arrSort, sortorder);
            if (foldersort.length > 0)
                foldersort = stringSorting(foldersort, sortorder);
        }
        var folderlength = foldersort.length; var filelength = arrSort.length;
        var init = 0;
        if (sortorder && folderlength > 0) {
            for (var i = init; i < folderlength; i++) {
                //$(sorted[i].html).prependTo("table#ogrid tbody");
                $("table#ogrid tbody tr:visible").eq(i).html(foldersort[i].html);
            }
            init = folderlength;
        }
        if (filelength > 0) {
            for (var i = init; i < (init + filelength) ; i++) {
                //$(sorted[i].html).prependTo("table#ogrid tbody");
                $("table#ogrid tbody tr:visible").eq(i).html(arrSort[i - init].html);
            }
            init = filelength;
        }


        if (!sortorder && folderlength > 0) {
            for (var i = init; i < (folderlength + init) ; i++) {
                //$(sorted[i].html).prependTo("table#ogrid tbody");
                $("table#ogrid tbody tr:visible").eq(i).html(foldersort[i - init].html);
            }

        }

        $("#ulContextMenu").show();
        hideloader();

    }
}
function stringSorting(arr, descorder) {
    var sorted = arr.sort(function (a, b) {
        if (a.key > b.key) return -1;
        if (a.key < b.key) return 1;
        return 0;
    });
    if (descorder)
        sorted = sorted.reverse();
    return sorted;
}
function dateSorting(arr, descorder) {
    var sorted = arr.sort(function (a, b) {

        //if (order == "ASC") {
        //    return a.time > b.time;
        //} else {

        //return b.time > a.time;
        //}

        var c = new Date(a.key);
        var d = new Date(b.key);
        return d - c;

    });
    if (descorder)
        sorted = sorted.reverse();
    return sorted;
}
function completedsearch() {
    updateTabCount();
}
function updateRefine() {
    var fileTypes = [];
    var users = [];
    var mappingFileTypes = [];
    var categories = [];
    if ($(".queryZone").length > 0) {

        $("table#ogrid tbody").find('tr:visible').each(function () {
            var fileType = $(this).find('.itemhiddenFileType').justtext();
            var user = $(this).find('.userlink').length > 0 ? $(this).find('.userlink').html() : "";
            var category = $(this).find('.itemCategory').justtext();

            if ($.trim(fileType) != "") {
                if ($.inArray(fileType, word) != -1)
                    fileType = wordMapyKey;
                else if ($.inArray(fileType, powerPoint) != -1)
                    fileType = powerPointMapKey;
                else if ($.inArray(fileType, excel) != -1)
                    fileType = excelMapKey;
                else
                    fileType = $.trim(fileType.toUpperCase());
                if ($.inArray(fileType, fileTypes) === -1)
                    fileTypes.push(fileType);

            }
            if ($.trim(user) != "" && $.inArray(user, users) === -1) {
                users.push(user);
            }
            if ($.trim(category) != "") {
                var itemCategories = category.split(",");

                for (var i = 0; i < itemCategories.length; i++) {
                    if ($.trim(itemCategories[i]) != "" && $.inArray(itemCategories[i], categories) === -1) {
                        categories.push(itemCategories[i]);
                    }
                }
            }
        });

        var fileTypeHtml = buildRefiners(fileTypes);
        $("div.otherfield").eq(1).find(".ia-filter-body").html(fileTypeHtml);
        if (fileTypes.length == 0) {
            $("div.otherfield").eq(1).hide();
        }
        else
            $("div.otherfield").eq(1).show();
        var modifiedByHtml = buildRefiners(users);
        $("div.otherfield").eq(0).find(".ia-filter-body").html(modifiedByHtml);
        if (users.length == 0) {
            $("div.otherfield").eq(0).hide();
        }
        else
            $("div.otherfield").eq(0).show();

        var categoryHtml = buildRefiners(categories);
        $("div.otherfield").eq(2).find(".ia-filter-body").html(categoryHtml);
        if (categories.length == 0) {
            $("div.otherfield").eq(2).hide();
        }
        else
            $("div.otherfield").eq(2).show();

        filterQuery();

    }
    unCheckSelections();

}
function buildRefiners(types) {
    var html = "";
    if (types.length > 0) {
        for (var i = 0; i < types.length; i++) {
            html += buildRefine(types[i]);
        }
    }
    return html;
}
function buildRefine(key) {
    var str = key.replace(/\s+/g, '');
    if ($("#" + str + ":checked").length > 0)
        return "<div class='ia-filter-row'><input type='checkbox' id='" + str + "' checked='checked' class='ia-filter-input-checkbox' onchange='updateGrid()'><label for='" + key + "' class='ia-filter-label-checkbox'>" + key + "</label></div>";
    else
        return "<div class='ia-filter-row'><input type='checkbox' id='" + str + "' class='ia-filter-input-checkbox' onchange='updateGrid()'><label for='" + key + "' class='ia-filter-label-checkbox'>" + key + "</label></div>";


}
function searchkeyup() {

    $('.ia-searchBox').trigger("keyup");
}
function updateGrid()
{ searchkeyup(); }
function filterQuery() {
    var date = []; var modified = []; var filetype = []; var categoryMeta = []; var startdate = ""; var enddate = "";
    var qy = formrefinerQuery();
    if (qy != "") {
        var querys = qy.split('$');
        for (var i = 0; i < querys.length; i++) {
            var category = querys[i].split('@');
            if (category[0] == "Date") {
                date = category[1].split('|');
                startdate = date[0];
                enddate = date[1];
            }

            else if (category[0] == "Modified By")
                modified = category[1].split('|');
            else if (category[0] == "File Type") {
                var tempfiletype = category[1].split('|');
                jQuery.each(tempfiletype, function (i, val) {
                    if ($.inArray(val, mapArray) == -1)
                        filetype.push(val.toLowerCase());
                    else {
                        if (val == wordMapyKey) { filetype = $.merge($.merge([], word), filetype); }
                        else if (val == powerPointMapKey) { filetype = $.merge($.merge([], powerPoint), filetype); }
                        else if (val == excelMapKey) { filetype = $.merge($.merge([], excel), filetype); }
                    }
                });
            }
            else if (category[0] == "Category")
                categoryMeta = category[1].split('|');

        }

        if ($("table#ogrid tbody").find('tr').length > 0 && $("table#ogrid tbody").find('tr:visible').length > 0) {
            $("table#ogrid tbody").find('tr:visible').each(function () {
                var fType = $(this).find('.itemhiddenFileType').justtext();
                var user = $(this).find('.userlink').html().replace(/\s+/g, '');
                var modifieddate = $(this).find('.ia-doc-list-modified').justtext();
                var categoryitem = $(this).find('.itemCategory').justtext().replace(/\s+/g, '');
                //var str = $(this).attr("id").replace(/\s+/g, '');
                var fileIsthere = ($.inArray(fType, filetype) > -1) || filetype.length == 0;
                var userIsthere = ($.inArray(user, modified) > -1) || modified.length == 0;
                var datestart = (startdate != "" && (new Date(modifieddate) >= new Date(startdate))) || startdate == "";
                var dateend = (enddate != "" && (new Date(modifieddate) <= new Date(enddate))) || enddate == "";
                var categoryIsthere = getCategoryIsthere(categoryitem, categoryMeta) || categoryMeta.length == 0;


                if (datestart == true && dateend == true && fileIsthere == true && userIsthere == true && categoryIsthere == true) {

                }
                else {
                    $(this).hide();

                }


            });
        }
    }
    if ($("table#ogrid tbody").find('tr:visible').length == 0)
        $("#noresults").show();
    else
        $("#noresults").hide();


}
function getCategoryIsthere(value, categoryMeta) {
    if ($.trim(value) != "") {
        var itemCategories = value.split(",");

        for (var i = 0; i < itemCategories.length; i++) {
            if ($.inArray(itemCategories[i], categoryMeta) > -1) {
                return true;
            }
        }
    }
    return false;
}



function updateTabCount() {

    var tab = $('#lblID').val() != "" ? $('#lblID').val() : "0";
    var totalcount = 0; var myfileCount = 0;
    var userName = $('.myFiles').attr("user");
    var arrSort = [];
    var modifiedBy = [];
    var filetype = [];
    if ($("table#ogrid tbody").find('tr').length > 0 && $("table#ogrid tbody").find('tr:visible').length > 0) {
        $("table#ogrid tbody").find('tr:visible').each(function (index) {
            var fileType = $(this).find('.itemhiddenFileType').justtext();
            var user = $(this).find('.userlink').html();
            if (tab == "0") {
                if (fileType != "") {
                    totalcount = totalcount + 1;
                    if (user == userName)
                        myfileCount = myfileCount + 1;
                }
            }
            else if (tab == "1") {
                if (fileType != "") {
                    totalcount = totalcount + 1;
                    if (user == userName) {
                        myfileCount = myfileCount + 1;
                    }
                    else
                        $(this).hide();
                }
                else
                    $(this).hide();
            }
            else if (tab == "2") {
                if (fileType != "") {
                    totalcount = totalcount + 1;
                    if (user == userName) {
                        myfileCount = myfileCount + 1;
                    }
                    var obj = {};
                    obj.html = $(this).html();
                    obj.Id = $(this).find('.itemhidden').justtext();
                    obj.time = new Date($(this).find('.ia-doc-list-modified').justtext());
                    obj.index = $(this).index();
                    arrSort.push(obj);

                }
                else
                    $(this).hide();

            }

        });
        if (tab == "2") {
            var sortByid = arrSort.sort(function (a, b) {

                return a.Id - b.Id;

            });

            var sorted = sortByid.sort(function (a, b) {

                //if (order == "ASC") {
                //    return a.time > b.time;
                //} else {

                //return b.time > a.time;
                //}

                var c = new Date(a.time);
                var d = new Date(b.time);
                return d - c;

            });



            for (var i = 0; i < sorted.length; i++) {
                //$(sorted[i].html).prependTo("table#ogrid tbody");
                $("table#ogrid tbody tr:visible").eq(i).html(sorted[i].html);
            }
            if ($("#recentFiles").attr("file-count") != "0") {
                var recentcount = (sorted.length >= parseInt($("#recentFiles").attr("file-count")) ? parseInt($("#recentFiles").attr("file-count")) : sorted.length);
                recentcount = recentcount - 1;
                $('table#ogrid tbody tr:visible:gt(' + recentcount + ')').hide();
            }
        }
    }
    if ($('.allfilecount'))
        $('.allfilecount').html(totalcount);
    if ($('.myfilecount'))
        $('.myfilecount').html(myfileCount);
    $('#ulContextMenu').show();
    updateRefine();

}
function datechange(element) {
    $('#txtStrDate').val($(element).attr("date"));
    $('#txtEndDate').val("");
    //getPostBack(false, false, false);
    searchkeyup();
}
function formrefinerQuery() {
    var refiner = "";
    var categoryset = [], categoryoptions = [];
    if ($('#txtStrDate').val() != "" || $('#txtEndDate').val() != "")
        categoryset.push("Date" + "@" + $('#txtStrDate').val() + "|" + $('#txtEndDate').val());
    $(".ia-accordion .otherfield").each(function () {
        var category = $(this).find("h3").attr("title");
        categoryoptions = [];
        $(this).find("input:checkbox:checked").each(function () {
            categoryoptions.push($(this).attr("id"));

        });
        if (categoryoptions.length > 0)
            categoryset.push(category + "@" + categoryoptions.join("|"));
    });
    refiner = categoryset.join("$");
    return refiner;
}
function SetRefineQuery() {
    searchkeyup(); //getPostBack(false, false, false); 
}
function ResetRefiner() {
    $('.ia-filter-row input:checkbox:checked').each(function () {
        $(this).removeAttr('checked');
    });
    if ($('#txtStrDate'))
        $('#txtStrDate').val('');
    if ($('#txtEndDate'))
        $('#txtEndDate').val('');

}

function GetUrl() {
    var searchtext = $('#txtSearch').val();
    var recursive = $('#ddlSelect').find(":selected").text() != "This Folder" ? $('#ddlSelect').find(":selected").text() : "";
    var fd = $('#currentFolderPath').prop('value') != null ? $('#currentFolderPath').attr('value') : "";
    var qy = formrefinerQuery();
    var tab = $('#tabStr').prop('value') != null ? $('#tabStr').attr('value') : "";
    var tabID = $('#lblID').val();
    return searchtext + "!" + recursive + "!" + fd + "!" + qy + "!" + tab + "!" + tabID;
}
function getPostBackSort(fdreset, tabreset, qyreset, sortcolumn, sorttype) {
    showloader();
    var searchtext = $('#txtSearch').val();
    var recursive = $('#ddlSelect').find(":selected").text() != "This Folder" ? $('#ddlSelect').find(":selected").text() : "";
    var fd = $('#currentFolderPath').prop('value') != null ? $('#currentFolderPath').attr('value') : "";
    var qy = formrefinerQuery();
    var tab = $('#tabStr').prop('value') != null ? $('#tabStr').attr('value') : "";
    //var urlQuery="?fd="+fd+"&qy="+qy+"&tab="+tab+"&searchterm="+searchtext+"&recursive="+recursive+"&fdreset="+fdreset+"&tabreset="+tabreset+"&qyreset="+qyreset;
    var urlQuery = "?fd=" + fd + "&recursive=" + recursive; //+ "&qy=" + qy + "&tab=" + tab + "&recursive=" + recursive + "&fdreset=" + fdreset + "&tabreset=" + tabreset + "&qyreset=" + qyreset + "&sortcolumn=" + sortcolumn + "&sortorder=" + sorttype;
    urlQuery = urlQuery.replace(/#/g, "$");
    var form = document.getElementById('aspnetForm');

    var action = form.getAttribute("action") + urlQuery;
    var cookieName = $('#akuminaCookieName').val();
    $.ajax({
        type: "POST",
        url: action,
        success: function (data) {
            BindHtml(data, fdreset, tabreset, qyreset, searchtext, recursive, cookieName, "");
            setTimeout(function () { getSort(sortcolumn, sorttype); }, 100);
        },
        error: function () {
            showloader();
            console.log("Data didn't get sent!!");
        }
    })
}
function getSort(sortcolumn, sorttype) {
    if (sorttype == "True")
        $("table#ogrid thead tr th." + sortcolumn).addClass("headerSortDown");
    else
        $("table#ogrid thead tr th." + sortcolumn).addClass("headerSortUp");

}
function getPostBack(fdreset, tabreset, qyreset) {

    showloader();
    var searchtext = $('#txtSearch').val();
    var recursive = $('#ddlSelect').find(":selected").text() != "This Folder" ? $('#ddlSelect').find(":selected").text() : "";
    var fd = $('#currentFolderPath').prop('value') != null ? $('#currentFolderPath').attr('value') : "";
    var qy = formrefinerQuery();
    var tab = $('#tabStr').prop('value') != null ? $('#tabStr').attr('value') : "";
    //var urlQuery="?fd="+fd+"&qy="+qy+"&tab="+tab+"&searchterm="+searchtext+"&recursive="+recursive+"&fdreset="+fdreset+"&tabreset="+tabreset+"&qyreset="+qyreset;
    var urlQuery = "?fd=" + fd + "&recursive=" + recursive + "&fdreset=" + fdreset;//+ "&qy=" + qy + "&tab=" + tab + "&recursive=" + recursive + "&fdreset=" + fdreset + "&tabreset=" + tabreset + "&qyreset=" + qyreset;
    urlQuery = urlQuery.replace(/#/g, "$");
    var form = document.getElementById('aspnetForm');

    var action = form.getAttribute("action") + urlQuery;
    var cookieName = $('#akuminaCookieName').val();
    $.ajax({
        type: "POST",
        url: action,
        success: function (data) {
            BindHtml(data, fdreset, tabreset, qyreset, searchtext, recursive, cookieName, "")
        },
        error: function () {
            showloader();
            console.log("Data didn't get sent!!");
        }
    })
}

function pageRefresh() {
    showloader();
    //unCheckSelections();
    //$('#lblID').attr('value', '0');
    //if ($('#txtSearch'))
    //    $('#txtSearch').val('');
    //if ($('#hdnQuerystatus'))
    //    $('#hdnQuerystatus').attr('value', 'reset');
    //getPostBack(false, false, false);
    getListItems(baseUrl, listname, queryFields);


}
function pageRefreshCreate() {

    showloader();
    //$('#lblID').attr('value', '0');

    getPostBack(true, false, false);
}
function searchterm() {
    showloader();


    var recursive = $('#ddlSelect').find(":selected").text() != "This Folder" ? $('#ddlSelect').find(":selected").text() : "";
    var currentfolderUrl = "";
    if ($('#currentFolderPath'))
        currentfolderUrl = $('#currentFolderPath').attr('value');
    currentfolderUrl = $.trim(baseUrl + "/" + currentfolderUrl + "/").toLowerCase();
    var currentfolderlength = currentfolderUrl.split("/").length;
    if (recursive != "") {
        var returnedData = $.grep(listItemsJson, function (element, index) {
            var folderUrl = decodeURIComponent(element.EncodedAbsUrl).toLowerCase();
            var folderlength = folderUrl.split("/").length;
            if (currentfolderUrl != folderUrl && folderUrl.indexOf(currentfolderUrl) >= 0 && element.File_x0020_Type != null)
                return true;
            else
                return false;

        });
        BindMustacheHtml(returnedData);
    }
    else {
        var returnedData = $.grep(listItemsJson, function (element, index) {
            var folderUrl = decodeURIComponent(element.EncodedAbsUrl).toLowerCase();
            var folderlength = folderUrl.split("/").length;
            if (currentfolderUrl != folderUrl && folderUrl.indexOf(currentfolderUrl) >= 0 && folderlength == currentfolderlength)
                return true;
            else
                return false;

        });
        BindMustacheHtml(returnedData);
    }
    //getPostBack(false, false, false);
}
function BindHtml(response, fdreset, tabreset, qyreset, searchtext, recursive, cookieName, redirect) {
    var start = response.indexOf("<body>");
    var end = response.indexOf("</body>");
    var content = response.substring(start - 1, end - 1);
    var folderRedirect = ["", ""];
    if (redirect != "")
        folderRedirect = redirect.split(";");


    if (fdreset) foldershow(content, folderRedirect[0]);
    getListItems(baseUrl, listname, queryFields);
    //gridbind(content, folderRedirect[0]);
    //$('#akuminaCookieName').val(cookieName);
    //if (tabreset) settabmenu(content, folderRedirect[1]);
    //if (qyreset) setRefiner(content);
    //updateBreadCrumb();
    //if ($('#txtSearch'))
    //    $('#txtSearch').val(searchtext);
    //pageloadfn();
    //hideloader();

    //if (recursive != "")
    //    $('#ddlSelect option[value="' + recursive + '"]').prop('selected', true);

}


// Getting list items based on ODATA Query
function getListItems(url, listname, query) {

    // Executing our items via an ajax request
    $.ajax({
        url: url + "/_api/web/lists/getbytitle('" + listname + "')/items" + query,
        method: "GET",
        headers: { "Accept": "application/json; odata=verbose" },
        success: function (data) {

            var tempJson = JSON.parse(JSON.stringify(data.d.results));
            for (var i = 0; i < tempJson.length; i++) {
                var fileName = tempJson[i]["FileLeafRef"].split(".")[0];
                var modifieddateAndDate = tempJson[i]["Modified"].split("T");
                var modifieddate = modifieddateAndDate[0].split("-");
                var modifiedTime = modifieddateAndDate[1].slice(0, -4);
                var modifiedDateFormat = modifieddate[1] + "/" + modifieddate[2] + "/" + modifieddate[0]
                var filetype = tempJson[i]["File_x0020_Type"];
                if ((tempJson[i]["Folder"] != null && tempJson[i]["Folder"]["ProgID"] != null && tempJson[i]["Folder"]["ProgID"] == "OneNote.Notebook")) {
                    filetype = "one";
                    tempJson[i]["File_x0020_Type"] = "one";
                }
                tempJson[i]["NoExtensionName"] = fileName;
                tempJson[i]["ModifiedDate"] = modifiedDateFormat;
                tempJson[i]["ModifiedTime"] = modifiedTime;
                tempJson[i]["DocIconUrl"] = filetype != null ? GetFileIconUrl(filetype) : "";


            }
            listItemsJson = tempJson;

            if (listItemsJson.length > 0)
                searchterm();
            else
                AfterGrid();


        },
        error: function (data) {
            alert("Failure: " + JSON.stringify(data));
        }
    });

}
function displaynoresults() {


}


function BindMustacheHtml(docItems) {

    var template = document.getElementById('dms_template').innerHTML;
    var output = Mustache.render(template, docItems);
    $("#mainGrid").html(output);
    BindColumnNames();
    AfterGrid();
}

function GetFileIconUrl(filetype) {

    if (typeof mapIconUrls[filetype] == "undefined")
        return styleUrl + mapIconUrls["default"];
    else
        return styleUrl + mapIconUrls[filetype];

}
function settabmenu(content, tabID) {
    var tab = $(content).find('.ia-tabs').html();
    $('.ia-tabs').html(tab);
    if (tabID != "")
        $('#lblID').val(tabID);
    tabmenuhighlight();

}
function detectSearch_enter(event) {

    if (event.keyCode == 13) {
        event.preventDefault();
        getPostBack(false, true, true);
    }

}

function tabmenuhighlight() {
    $(".ia-tabs-nav li").each(function () {
        $(this).removeClass("ia-tab-active");
    });
    var current = $('#lblID').attr('value') != null ? parseInt($('#lblID').attr('value')) : 0;
    $(".ia-tabs-nav").find("li").eq(current).addClass("ia-tab-active");
}

function setRefiner(content) {
    var refiner = $(content).find('.queryZone').parent().html();
    $('.queryZone').parent().html(refiner);
}


function gridbind(content, folder) {
    var grid = $(content).find('#drop_zone').html();

    $('#drop_zone').html(grid);
    if (folder != "")
        $('#currentFolderPath').val(folder);
    $('.loader').hide();
}
function foldershow(content, folderPath) {

    var folder = $(content).find('.ia-folder-tree').parent().html();
    $('.ia-folder-tree').parent().html(folder);
    if (folderPath != "") {
        $('#folderBox').attr("value", folderPath); expandTreeView();
    } else
        expandTreeView();


}
function expandTreeView() {
    $('.ia-folder-tree').jstree();
    $('.ia-folder-tree').show();
    var folderurl = $("#folderBox").attr("value").split("/");

    var currentUrl = "", i, currentFolder;
    for (i = 0; i < folderurl.length; i++) {
        currentUrl += folderurl[i];

        currentFolder = $("a[url-data='" + currentUrl + "']");
        if (currentUrl == $("#folderBox").attr("value")) {

            currentFolder.addClass("jstree-clicked");

        }
        else {
            if (currentFolder.parent().find("ul").length < 1) {
                currentFolder.prev().click();
            }
        }
        currentUrl += "/";

    }

    if ($("a.jstree-clicked").length > 2) {

        $("a.jstree-clicked")[0].removeClass("jstree-clicked");
    }


}
function sortinghead(element) {
    $(element).next().find("input").click();
}
function updateBreadCrumb() {
    var folderName, currentfolder, value = '';
    if ($("#currentFolderPath").prop("value")) {
        currentfolder = $("#currentFolderPath").attr("value");
        folderName = currentfolder.split("/");
        if (folderName.length > 1)
            value = folderName[folderName.length - 1];
        else
            value = $("#listDisplayName").attr("value");

        if ($(".lblBreadCrump"))
            $(".lblBreadCrump").html('"' + value + '"');
    }
}
function tabHighlight(tabid, element) {
    $('#lblID').attr('value', tabid);

    if ($('#hdnQueryQF'))
        $('#hdnQueryQF').attr('value', '');
    $('#tabStr').attr('value', $(element).attr('tab-val'));
    tabmenuhighlight();
    searchkeyup();
    //ResetRefiner();
    //getPostBack(false, false, true);
}
function getcurrentfolder(element) {
    showloader();
    var currentfolderUrl = $.trim(decodeURIComponent($(element).attr('url-data')));

    if ($('#currentFolderPath'))
        $('#currentFolderPath').attr('value', currentfolderUrl.replace(baseUrl + "/", ""));

    if ($('#folderBox'))
        $('#folderBox').attr('value', currentfolderUrl.replace(baseUrl + "/", ""));
    if (currentfolderUrl.indexOf("http") == 0)
        currentfolderUrl = ((currentfolderUrl) + "/").toLowerCase();
    else
        currentfolderUrl = (baseUrl + "/" + currentfolderUrl + "/").toLowerCase();
    var currentfolderlength = currentfolderUrl.split("/").length;
    var returnedData = $.grep(listItemsJson, function (element, index) {
        var folderUrl = decodeURIComponent(element.EncodedAbsUrl).toLowerCase();
        var folderlength = folderUrl.split("/").length;

        if (currentfolderUrl != folderUrl && folderUrl.indexOf(currentfolderUrl) >= 0 && folderlength == currentfolderlength)

            return true;

        else
            return false;

    });
    ResetRefiner();
    if ($('#txtSearch'))
        $('#txtSearch').val('');
    BindMustacheHtml(returnedData);

    updateBreadCrumb();
    $('#ddlSelect option[value="' + "This Folder" + '"]').prop('selected', true);
    if ($('#lblID'))
        $('#lblID').attr('value', '0');
    tabmenuhighlight();
    expandTreeView();
    //hideAllButton();
    //showloader();
    //$('#ddlSelect option[value="' + "This Folder" + '"]').prop('selected', true);
    //if ($('#txtSearch'))
    //    $('#txtSearch').val('');
    //$('#currentFolderPath').attr('value', folder);
    //if ($('#folderBox')) {
    //    $('#folderBox').attr('value', folder);
    //    if ($(element).attr("class") == "filename")

    //        expandTreeView();
    //}

    //if ($('#hdnQueryQF'))
    //    $('#hdnQueryQF').attr('value', '');

    //if ($('#lblID'))
    //    $('#lblID').attr('value', '0');
    //$('#tabStr').attr('value', 'All Files');
    //ResetRefiner();
    //getPostBack(false, true, true);


}
function showloader() {
    if ($('#ulContextMenu')) $('#ulContextMenu').hide();
    hideAllButton(); if ($('.ia-loading-panel')) {
        $('.ia-loading-panel').addClass("ia-show");
        $('.ia-loading-panel').removeClass("ia-hide");
    }
    //if ($('.loader')) $('.loader').show(); 
}
function hideloader() { //if ($('.loader')) $('.loader').hide(); 
    if ($('.ia-loading-panel')) {
        $('.ia-loading-panel').addClass("ia-hide");
        $('.ia-loading-panel').removeClass("ia-show");
    }
}
function fileCreateFolder() {
    if ($("#dropdown-1"))
        $("#dropdown-1").hide();
    if ($('#txtSearch'))
        $('#txtSearch').val('');
    if ($('#lblID')) {
        $('#lblID').val("0");
        tabmenuhighlight();
    }
    ResetRefiner();
    var listname = $('#listName').val();
    var folderPath = $('#webServerUrl').val() + '/' + $('#currentFolderPath').val();
    var baseUrl = $('#webUrl').val();
    if (folderPath == "/") folderPath = folderPath + $('#listName').val();
    var options = {
        url: baseUrl + '/' + listname + '/Forms/Upload.aspx?RootFolder=' + folderPath + '&Type=1',
        tite: 'Create Folder',
        allowMaximize: false,
        width: 550,
        height: 200,
        showClose: true,
        dialogReturnValueCallback: onPopUpCloseCallBackWithData
    };
    SP.UI.ModalDialog.showModalDialog(options);
}
function onPopUpCloseCallBackWithData(result, returnValue) {
    if (result == SP.UI.DialogResult.OK) {
        pageRefreshCreate();
    }
}

function fileCreateDocument(template) {
    if ($("#dropdown-1"))
        $("#dropdown-1").hide();
    if ($('#txtSearch'))
        $('#txtSearch').val('');
    if ($('#lblID')) {
        $('#lblID').val("0");
        tabmenuhighlight();
    }
    ResetRefiner();
    var listname = $('#listName').val();
    var folderPath = $('#webServerUrl').val() + '/' + $('#currentFolderPath').val();
    var baseUrl = $('#webUrl').val();
    setCookie();
    //window.location.href = 'http://www.google.com';
    OpenPopUpPageWithTitle(baseUrl + "/_layouts/15/CreateNewDocument.aspx?" + "SaveLocation=" + folderPath + "&DefaultItemOpen=1&Source=" + listname + "&TemplateType=" + template, OnCloseDialogNavigate);

}

function fileUpload() {
    var baseUrl = $('#webUrl').val();
    var listId = $('#listGuidId').val();
    var folderPath = $('#webServerUrl').val() + '/' + $('#currentFolderPath').val();

    if (folderPath == "/") folderPath = folderPath + $('#listName').val();
    //if ($('#txtSearch'))
    //    $('#txtSearch').val('');
    //ResetRefiner();
    var options = {
        url: baseUrl + '/_layouts/15/Upload.aspx?List={' + listId + '}&RootFolder=' + folderPath,
        tite: 'Upload Files',
        allowMaximize: false,
        width: 800,
        height: 500,
        showClose: true,
        dialogReturnValueCallback: onPopUpCloseCallBackWithData
    };
    SP.UI.ModalDialog.showModalDialog(options);


}



function fileCheckOut() {

    var sList = '';

    $('.selection').each(function () {
        if (this.checked) {
            sList += $(this).parent().parent().find('.itemhidden').justtext() + ';';
        }
    });
    var listname = $('#listDisplayName').val();
    var clientContext = SP.ClientContext.get_current();
    if (clientContext != undefined && clientContext != null) {
        var webSite = clientContext.get_web();
        this.list = webSite.get_lists().getByTitle(listname);
        var lstID = '';
        var i;
        var myarray = sList.split(';');
        var item, file;

        for (var i = 0; i < myarray.length; i++) {
            lstID = myarray[i];
            item = this.list.getItemById(lstID);
            file = item.get_file();
            file.checkOut();
            clientContext.load(file)
        }
        clientContext.executeQueryAsync(Function.createDelegate(this, this.OnLoadSuccess), Function.createDelegate(this, this.OnLoadFailed));

    }
}

function OnLoadSuccess(sender, args) {
    alert(this.file.get_title() + ' checked out successfully');

    pageRefresh();



}

function OnLoadFailed(sender, args) {

    if (args.get_message() == 'Input string was not in a correct format.')
    { alert('File(s) Checked out successfully'); pageRefresh(); }
    else
    {
        alert('Request failed. ' + args.get_message());
        unCheckSelections();
    }


}


function fileEditCheckOut() {

    var sList = '';

    $('.selection').each(function () {
        if (this.checked) {
            sList += $(this).parent().parent().find('.itemhidden').justtext() + ';';
        }
    });
    var listname = $('#listDisplayName').val();
    var clientContext = SP.ClientContext.get_current();
    if (clientContext != undefined && clientContext != null) {
        var webSite = clientContext.get_web();
        this.list = webSite.get_lists().getByTitle(listname);
        var lstID = '';
        var i;
        var myarray = sList.split(';');
        var item, file;

        for (var i = 0; i < myarray.length; i++) {
            lstID = myarray[i];
            item = this.list.getItemById(lstID);
            file = item.get_file();
            file.checkOut();
            clientContext.load(file)
        }
        clientContext.executeQueryAsync(Function.createDelegate(this, this.OnLoadSuccessFileEditCheckout), Function.createDelegate(this, this.OnLoadFailedFileEditCheckout));

    }
}

function OnLoadSuccessFileEditCheckout(sender, args) {


    var sList = '';

    $('.selection').each(function () {
        if (this.checked) {
            sList += $(this).parent().parent().find('.itemhidden').justtext();
        }
    });

    var listname = $('#listName').val();
    var baseUrl = $('#webUrl').val();


    var options = {
        url: baseUrl + '/' + listname + '/Forms/EditForm.aspx?ID=' + sList,
        tite: 'Edit Properties',
        allowMaximize: false,
        showClose: false,
        width: 800,
        height: 600,
        showClose: true,
        dialogReturnValueCallback: onPopUpCloseCallBackWithData
    };
    SP.UI.ModalDialog.showModalDialog(options);


}

function OnLoadFailedFileEditCheckout(sender, args) {

    if (args.get_message() == 'Input string was not in a correct format.') {
        var sList = '';


        $('.selection').each(function () {
            if (this.checked) {
                sList += $(this).parent().parent().find('.itemhidden').justtext();
            }
        });

        var listname = $('#listName').val();
        var baseUrl = $('#webUrl').val();


        var options = {
            url: baseUrl + '/' + listname + '/Forms/EditForm.aspx?ID=' + sList,
            tite: 'Edit Properties',
            allowMaximize: false,
            showClose: false,
            width: 800,
            height: 600,
            showClose: true,
            dialogReturnValueCallback: onPopUpCloseCallBackWithData
        };
        SP.UI.ModalDialog.showModalDialog(options);
    }
    else {
        alert('Checkout failed. ' + args.get_message());
        unCheckSelections();
    }


}

function fileCheckIn() {

    var sList = '';

    $('.selection').each(function () {
        if (this.checked) {
            sList += $(this).parent().parent().find('.itemhidden').justtext() + ';';
        }
    });
    var listname = $('#listDisplayName').val();

    var clientContext = SP.ClientContext.get_current();
    if (clientContext != undefined && clientContext != null) {
        var webSite = clientContext.get_web();
        this.list = webSite.get_lists().getByTitle(listname);

        var lstID = '';
        var i;
        var myarray = sList.split(';');
        var item, file;

        for (var i = 0; i < myarray.length; i++) {
            lstID = myarray[i];
            item = this.list.getItemById(lstID);

            file = item.get_file();
            file.checkIn('Checked in using ECMA script', 1);
            clientContext.load(file)
        }
        clientContext.executeQueryAsync(Function.createDelegate(this, this.OnLoadCheckInSuccess), Function.createDelegate(this, this.OnLoadCheckInFailed));
    }
}

function OnLoadCheckInSuccess(sender, args) {
    alert(this.file.get_title() + ' checked in successfully');
    pageRefresh();


}

function OnLoadCheckInFailed(sender, args) {
    if (args.get_message() == 'Input string was not in a correct format.')
    { alert('File(s) checked in successfully'); pageRefresh(); }
    else
    {
        alert('Request failed. ' + args.get_message());
        unCheckSelections();
    }

}

function fileDisCheckOut() {
    var sList = '';

    $('.selection').each(function () {
        if (this.checked) {
            sList += $(this).parent().parent().find('.itemhidden').justtext() + ';';
        }
    });
    var listname = $('#listDisplayName').val();


    var clientContext = SP.ClientContext.get_current();
    if (clientContext != undefined && clientContext != null) {
        var webSite = clientContext.get_web();
        this.list = webSite.get_lists().getByTitle(listname);

        var lstID = '';
        var i;
        var myarray = sList.split(';');
        var item, file;

        for (var i = 0; i < myarray.length; i++) {
            lstID = myarray[i];
            item = this.list.getItemById(lstID);

            file = item.get_file();
            file.undoCheckOut();
            clientContext.load(file)
        }
        clientContext.executeQueryAsync(Function.createDelegate(this, this.OnLoadDiscCheckOutSuccess), Function.createDelegate(this, this.OnLoadDisCheckOutFailed));
    }
}

function OnLoadDiscCheckOutSuccess(sender, args) {
    alert(this.file.get_title() + ' Discarded successfully');
    pageRefresh();

}

function OnLoadDisCheckOutFailed(sender, args) {
    if (args.get_message() == 'Input string was not in a correct format.')
    { alert('File(s) discard checkout successfully'); pageRefresh(); }
    else
    {
        alert('Request failed.  You cannot discard check out because there is no checked in version of the document.');
        unCheckSelections();
    }


}




function fileDownload() {
    var sList = '';
    var sDocument = '';

    $('.selection').each(function () {
        if (this.checked) {
            sList += $(this).parent().parent().find('.itemhidden').justtext();
            sDocument += $('#webUrl').val() + '/_layouts/15/download.aspx?SourceUrl=' + $('#webUrl').val() + '/' + $(this).parent().parent().find('.itemhiddenPath').justtext();
        }
    });
    var listname = $('#listName').val();

    window.open(sDocument);
    unCheckSelections();
}

function fileOpen(event) {

    var thisitem;

    $('.selection').each(function () {
        if (this.checked) {

            thisitem = $(this).parent().parent().find('.docOpen');
        }
    });
    setCookie();
    unCheckSelections();
    window.location.href = thisitem.attr('href');

}

function fileDelete() {

    var sList = '';

    $('.selection').each(function () {
        if (this.checked) {
            sList += $(this).parent().parent().find('.itemhidden').justtext() + ';';
        }
    });
    var listname = $('#listDisplayName').val();


    if (confirm('Are you sure to want to Delete?') == true) {
        var clientContext = SP.ClientContext.get_current();
        var oList = clientContext.get_web().get_lists().getByTitle(listname);

        var lstID = '';
        var i, oListItem;
        var myarray = sList.split(';');

        for (var i = 0; i < myarray.length; i++) {
            lstID = myarray[i];
            oListItem = oList.getItemById(lstID);
            oListItem.deleteObject();
        }

        clientContext.executeQueryAsync(Function.createDelegate(this, this.onQuerySucceededDelete), Function.createDelegate(this, this.onQueryFailedDelete));
    }
    else { }
}

function onQuerySucceededDelete() {

    alert('Item deleted: ' + itemId);
    pageRefreshCreate();

}

function onQueryFailedDelete(sender, args) {
    if (args.get_message() == 'Input string was not in a correct format.')
    { pageRefreshCreate(); }
    else
    {
        alert('Request failed. ' + args.get_message());
        unCheckSelections();
    }

}

function fileViewProperties() {
    var sList = '';

    $('.selection').each(function () {
        if (this.checked) {
            sList += $(this).parent().parent().find('.itemhidden').justtext();
        }
    });
    var listname = $('#listName').val();
    var listId = $('#listGuidId').val();
    var baseUrl = $('#webUrl').val();

    var options = {
        url: baseUrl + '/' + listname + '/Forms/DispForm.aspx?ID=' + sList,
        tite: 'View Properties',
        allowMaximize: false,
        showClose: false,
        width: 800,
        height: 600,
        showClose: true,

    };
    SP.UI.ModalDialog.showModalDialog(options);
    unCheckSelections();
}

function fileEditProperties() {
    var sList = '';
    var checkoutImg = '.ia-doc-checkedOut';
    $('.selection').each(function () {
        if (this.checked) {
            sList += $(this).parent().parent().find('.itemhidden').justtext();
        }
    });
    var forceCheckout = $('#lstForceCheckout').val();
    var listname = $('#listName').val();
    var baseUrl = $('#webUrl').val();
    var listId = $('#listGuidId').val();

    if (forceCheckout == "true" && $('.selection:checked').parent().parent().find(checkoutImg).length == 0) {
        if (confirm("You must check out this item before making changes.  Do you want to check out this item now?")) {
            fileEditCheckOut();
        }
    }
    else {
        var options = {
            url: baseUrl + '/' + listname + '/Forms/EditForm.aspx?ID=' + sList,
            tite: 'Edit Properties',
            allowMaximize: false,
            showClose: false,
            width: 800,
            height: 600,
            showClose: true,
            dialogReturnValueCallback: onPopUpCloseCallBackWithData
        };
        SP.UI.ModalDialog.showModalDialog(options);
    }



}

function fileComplianceDetails() {
    var baseUrl = $('#webUrl').val();
    var sList = '';

    $('.selection').each(function () {
        if (this.checked) {
            sList += $(this).parent().parent().find('.itemhidden').justtext();
        }
    });
    var listname = $('#listName').val();
    var listId = $('#listGuidId').val();

    var options = {
        url: baseUrl + '/_layouts/15/itemexpiration.aspx?ID=' + sList + '&List={' + listId + '}',
        tite: 'Compliance Details',
        allowMaximize: false,
        showClose: false,
        width: 800,
        height: 600,
        showClose: true,
    };
    SP.UI.ModalDialog.showModalDialog(options);

}

function fileShare() {
    var baseUrl = $('#webUrl').val();
    var sList = '';

    $('.selection').each(function () {
        if (this.checked) {
            sList += $(this).parent().parent().find('.itemhidden').justtext();
        }
    });

    var listId = $('#listGuidId').val();

    var options = {
        url: baseUrl + '/_layouts/15/aclinv.aspx?forSharing=1&List=' + listId + '&obj={' + listId + '},' + sList + ',DOCUMENT&IsDlg=1',
        tite: 'Share File',
        allowMaximize: false,

        width: 800,
        height: 600,
        showClose: true,
    };
    SP.UI.ModalDialog.showModalDialog(options);
    unCheckSelections();

}

function fileShareWith() {
    var baseUrl = $('#webUrl').val();
    var sList = '';

    $('.selection').each(function () {
        if (this.checked) {
            sList += $(this).parent().parent().find('.itemhidden').justtext();
        }
    });
    var listname = $('#listName').val();
    var listId = $('#listGuidId').val();

    EnsureScriptFunc('sharing.js', 'DisplaySharedWithDialog', function () { DisplaySharedWithDialog(baseUrl, '{' + listId + '}', sList); })
    unCheckSelections();
}

function fileWorkflow() {
    var baseUrl = $('#webUrl').val();
    var sList = '';

    $('.selection').each(function () {
        if (this.checked) {
            sList += $(this).parent().parent().find('.itemhidden').justtext();
        }
    });
    var listId = $('#listGuidId').val();


    var options = {
        url: baseUrl + '/_layouts/15/Workflow.aspx?ID=' + sList + '&List={' + listId + '}',
        tite: 'Workflow',
        allowMaximize: false,
        showClose: false,
        width: 800,
        height: 600,
        showClose: true,
    };
    SP.UI.ModalDialog.showModalDialog(options);
    unCheckSelections();
}

var itemId1;
var docName1;
$('.preview').on("click", function () {

    itemId1 = $(this).parent().parent().find('.itemhidden').justtext();
    docName1 = $(this).parent().parent().find('.itemhiddenPath').justtext();

    alert(itemId1 + docName1);
});
function previewFile() {

}

function fileFollow() {

    var itemId = '';
    var docName = '';

    $('.selection').each(function () {
        if (this.checked) {
            itemId += $(this).parent().parent().find('.itemhidden').justtext();
            docName += $(this).parent().parent().find('.itemhiddenPath').justtext();
        }
    });

    var listId = $('#listGuidId').val();

    if (listId != null && itemId != null && docName != null) {
        SP.SOD.executeFunc('followingcommon.js', 'FollowDocumentFromEmail', function () {
            FollowDocumentFromEmail(itemId, listId, docName);
        });
    }
    unCheckSelections();
}


function getCallOutFilePreviewBodyContent(urlWOPIFrameSrc, pxWidth, pxHeight, Modifiedby, Modified, ShareTitle, docurl) {
    var callOutContenBodySection = "<div class='js-callout-bodySection'>";
    callOutContenBodySection += "<div class='js-filePreview-containingElement'>";
    callOutContenBodySection += "<div class='js-frame-wrapper' style='line-height: 0'>";
    callOutContenBodySection += '<iframe style="width: ' + pxWidth + 'px; height: ' + pxHeight + 'px;" src=' + urlWOPIFrameSrc + ' frameborder="0"></iframe>';
    callOutContenBodySection += '</div></div></div><br>';
    callOutContenBodySection += "<div class='js-callout-bodySection'>";
    callOutContenBodySection += "<span id='co2,2,0_calloutModified'>" + "Changed by";
    callOutContenBodySection += "<span class='ms-verticalAlignTop ms-noWrap ms-displayInlineBlock'>";
    callOutContenBodySection += "<span class='ms-noWrap ms-imnSpan'>";
    callOutContenBodySection += "<span class='ms-spimn-presenceLink'>" + "&nbsp;" + Modifiedby + "&nbsp;" + Modified;
    callOutContenBodySection += "</span></span></span></span></div><br>";
    callOutContenBodySection += "<div class='js-callout-sharedWithInfo' id='co1,6,0_calloutSharedWithInfo' style='display: block;'";
    callOutContenBodySection += "<div class='js-callout-bodySection'>";
    callOutContenBodySection += "<div class='js-callout-sharedWithList'>";
    callOutContenBodySection += "<span>" + "Shared with" + "&nbsp;" + "<a href='#' onclick='fileShareWith();' class='js-callout-sharedWithLink' Title=" + ShareTitle + ">" + "lots of People </a></span>"
    callOutContenBodySection += "</div></div></div><br>";
    callOutContenBodySection += "<div class='js-callout-bodySection'>";
    callOutContenBodySection += "<input class='js-callout-location' onclick='javascript: this.select();' onblur='javascript:this.value = this.defaultValue;' value='" + docurl + "' >";
    callOutContenBodySection += "</div>";
    return callOutContenBodySection;
}

function getCallOutFilePreviewBodyContent2(urlWOPIFrameSrc, pxWidth, pxHeight, Modifiedby, Modified, ShareTitle, docurl) {
    var callOutContenBodySection = '<iframe style="width: 100%; height: ' + pxHeight + 'px;" src=' + urlWOPIFrameSrc + ' frameborder="0"></iframe>';

    return callOutContenBodySection;
}


function CopyToCB() {
    if (!document.all) alert("supports only in ie");
    else {
        var copytext = $("#ia-doc-prevlink").val();
        window.clipboarddata.setdata("text", copytext);
    }


}


function OpenItemFilePreviewCallOut(sender) {
    var html = $(sender).next().justtext().split(','), fileurl = "";
    var Images = ("gif,tiff,jpg,jpeg,bmp,png,rif,txt").split(',');
    if (jQuery.inArray(html[4], Images) != -1)
        fileurl = "'" + html[1] + "'";
    else
        fileurl = '"' + $('#webServerUrl').val() + '/_layouts/15/WopiFrame.aspx?sourcedoc=' + html[1] + '&amp;action=interactivepreview&amp;wdSmallView=1"';
    var Modified = html[3];
    var Editor = html[2];
    var shareTileName = html[0];
    var FileType = ("one,onetoc2,mpp,vsdx").split(',');

    var baseUrl = $('#webUrl').val();
    var listname = $('#listName').val();

    var docLinkUrl = baseUrl + "/" + listname + "/" + html[0];

    var openNewWindow = true; //set this to false to open in current window
    if (jQuery.inArray(html[4], FileType) != 0 && jQuery.inArray(html[4], FileType) != 1 && jQuery.inArray(html[4], FileType) != 2 && jQuery.inArray(html[4], FileType) != 3) {

        var callOutContenBodySection = getCallOutFilePreviewBodyContent2(fileurl, 500, 600, Editor, Modified, shareTileName, docLinkUrl);
        $('#document-preview-1 div.ia-preview-document').html(callOutContenBodySection);

    }
    else
        $('#document-preview-1 div.ia-preview-document').html("");
    $('#document-preview-1 h1.ia-doc-title').text(html[0]);
    $('#document-preview-1 p.ia-doc-modified').html("Changed by " + Editor + " on " + Modified);
    $('#document-preview-1 p.ia-doc-shared').html("Shared with <a href='#' onclick='fileShareWith();' Title=" + shareTileName + ">" + "lots of people </a>");
    //$('#document-preview-1 input#ia-Doc-PrevLink').text(fileurl);
    $('#document-preview-1 h1.ia-doc-title').text(html[0]);
    $('#document-preview-1 input#ia-Doc-PrevLink').val(docLinkUrl);


}

function RemoveAllItemCallouts() {
    CalloutManager.forEach(function (callout) {
        // remove the current callout
        CalloutManager.remove(callout);
    });
}

function RemoveItemCallout(sender) {
    var callout = CalloutManager.getFromLaunchPointIfExists(sender);
    if (callout != null) {
        // remove
        CalloutManager.remove(callout);
    }
}


function CloseItemCallout(sender) {
    var callout = CalloutManager.getFromLaunchPointIfExists(sender);
    if (callout != null) {
        // close
        callout.close();
    }
}


function UndoSelection() {
    $('.interAction .ia-document-library-list').each(function () {

        //On click of individual <td> in a <tr>
        $(this).find('tbody tr td').each(function () {

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