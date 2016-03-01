var wordMapyKey = "Word", powerPointMapKey = "PowerPoint", excelMapKey = "Excel";
var mapArray = [wordMapyKey, powerPointMapKey, excelMapKey];
var word = ['docx', 'doc', 'docm', 'dot', 'nws', 'dotx'],
         powerPoint = ['odp', 'ppt', 'pptm', 'pptx', 'potm', 'ppam', 'ppsm', 'ppsx'],
         excel = ['odc', 'xls', 'xlsb', 'xlsm', 'xlsx', 'xltm', 'xltx', 'xlam'];


var styleUrl = "";
var str = "";
var mapIconUrls;
var listItemsJson = {};
var listname = "";// $('#listDisplayName').val();

var baseUrl = ""// $('#webUrl').val();
var currentCount; var TotalCount; var OverAllFailureMessage;


var times = 40;

$(window).load(function () {
    var i = 0;

    loadDragfn();
    setIdleTimeout(300000); // 300 seconds
    document.onIdle = function () { i = i + 1; refreshIdle(i); }


});

$(document).ready(function () {
    showloader();
    styleUrl = $("#styleUrl").html();

    listname = $('#listDisplayName').val();
    baseUrl = $('#webUrl').val();
    var url = window.location.href;
    if (url.toLowerCase().indexOf('searchtext') > -1) {
        var searchText = getParameterByName('searchText');
        if ($("#txtSearch").length > 0 && searchText != "") {
            $("#txtSearch").val(searchText);
        }
    }
    //if ($(".hintText").length > 0) {
    //    $(".hintText").hide();
    //}
    //if (url.toLowerCase().indexOf('doclibname') > -1) {
    //    var documentLibraries = getParameterByName('docLibName');
    //    if (documentLibraries != "" && documentLibraries.toLowerCase() != "all") {
    //        var listofLibraries = documentLibraries.split(',');
    //        if (listofLibraries.length != 1) {
    //            if ($(".uploadCreateButtons").length > 0) {
    //                $(".uploadCreateButtons").hide();

    //            }
    //        }
    //        else {
    //            if ($(".hintText").length > 0) {
    //                $(".hintText").show();
    //            }
    //        }
    //    }
    //    else {
    //        if ($(".uploadCreateButtons").length > 0) {

    //            $(".uploadCreateButtons").hide();
    //        }
    //    }
    //}

});

function getParameterByName(name) {
    name = name.replace(/[\[]/, "\\[").replace(/[\]]/, "\\]");
    var regex = new RegExp("[\\?&]" + name + "=([^&#]*)"),
        results = regex.exec(location.search);
    return results === null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
}

function searchLibrary(event) {
    var libraries = [];

    var libQuery = "";
    event.preventDefault();
    if ($(".ia-search-library-site-list-dropdown ul li.ia-search-all input[type=checkbox]:checked").length > 0) {
        libQuery = "docLibName=all";
    }
    else {
        if ($(".ia-search-library-site-list-dropdown ul li.ia-search-single-library input[type=checkbox]:checked").length > 0) {

            $(".ia-search-library-site-list-dropdown ul li.ia-search-single-library input[type=checkbox]:checked").each(function () {
                libraries.push($(this).next().justtext());
            });
            libQuery = "docLibName=" + libraries.join(',');
        }
    }
    if ($("#txtSearch").length > 0 && $("#txtSearch").val() != "") {
        if (libQuery != "") {
            libQuery += "&searchText=" + $("#txtSearch").val();
        }
        else {
            libQuery += "searchText=" + $("#txtSearch").val();

        }

    }
    if (libQuery != "") {
        window.location.href = window.location.href.replace("#", "").split('?')[0] + "?" + libQuery;
    }
    //alert(libQuery);
}

function tabmenuhighlight() {
    $(".ia-tabs-nav li").each(function () {
        $(this).removeClass("ia-tab-active");
    });
    var current = $('#lblID').attr('value') != null ? parseInt($('#lblID').attr('value')) : 0;
    $(".ia-tabs-nav").find("li").eq(current).addClass("ia-tab-active");
    //if (current == 0) {
    //    if ($('#txtSearch'))
    //        $('#txtSearch').val('');
    //    ResetRefiner();
    //}
}

//Set categories based on grid. 
function setRefinerCategories() {
    var categoriesPath = [];
    var categoryNames = [];
    var categorydiv = $('.refinerCategory');

    $("table#grid tbody").find('tr:visible').each(function () {
        var category = $(this).find('.itemCategory').justtext();

        if ($.trim(category) != "") {
            var itemCategories = category.split(",");
            for (var i = 0; i < itemCategories.length; i++) {
                if ($.trim(itemCategories[i]) != "" && $.inArray(itemCategories[i], categoriesPath) === -1) {
                    if ($(categorydiv).length > 0 && $(categorydiv).find("input[value='" + itemCategories[i].toLowerCase().trim() + "']").length > 0) {
                        var pathArr = itemCategories[i].split('>');
                        categoryNames.push(pathArr[pathArr.length - 1]);
                        categoriesPath.push(itemCategories[i]);
                    }
                }
            }
        }
    });
    $('.ia-category-input').val(categoryNames.join(','));
    $('.selectedTaxonomyConfrirm').val(categoriesPath.join(','));

}


function getitemsbyCategory(categorySelected, val) {
    var documentclassName = "ia-doclist-name";
    var categoryMeta = [];

    if (categorySelected != "")
        categoryMeta = categorySelected.replace(/[^A-Z0-9,>]/ig, "_").split(',');


    if ($("table#grid tbody").find('tr').find("td").length > 1) {
        if ($("table#grid tbody").find('tr').length > 0) {
            categoryitemsResult = true;
            $("table#grid tbody").find('tr').each(function () {
                var IsFile = $(this).find("." + documentclassName + " a.disp-File-Name").length > 0 ? true : false;

                var categoryitem = $(this).find('.itemCategory').justtext().trim().replace(/[^A-Z0-9,>]/ig, "_");

                var key = IsFile ? $(this).find("." + documentclassName + " a.disp-File-Name").html() : $(this).find("." + documentclassName + " a.filename").html();

                var categoryIsthere = getCategoryIsthere(categoryitem, categoryMeta) || categoryMeta.length == 0;

                var keyIsthere = key != null && (typeof key != 'undefined') && key.toLowerCase().indexOf(val.toLowerCase()) >= 0;
                if (keyIsthere && categoryIsthere)
                    $(this).show();
                else
                    $(this).hide();



            });
        }

    }

}



function displayitemswithkey(val) {

    $("table#grid tbody tr").show();

    var modifiedrefinerresult = false;
    var categorySelected = "";

    if ($(".selectedTaxonomyConfrirm").length > 0 && $(".selectedTaxonomyConfrirm").val() != "") {
        categorySelected = $(".selectedTaxonomyConfrirm").val().trim();

    }
    if (val != "" || categorySelected != "")
        getitemsbyCategory(categorySelected, val);
    //BindRefine_Users();
}



function completedsearch(modfiedUsers) {

    updateTabCount(modfiedUsers);
}


function library_change(event) {
    event.preventDefault();

    window.location.href = window.location.href.split('?')[0] + "?library=" + escape($("#listOfLibraries").val());
}
function formrefinerQuery(modfiedUsers) {


    var refiner = "";
    var categoryset = [], categoryoptions = [];

    if ($(".selectedTaxonomyConfrirm").length > 0 && $(".selectedTaxonomyConfrirm").val() != "") {
        categoryset.push("Category" + "@" + $(".selectedTaxonomyConfrirm").val());
    }
    if ($('#txtStrDate').val() != "" || $('#txtEndDate').val() != "")
        categoryset.push("Date" + "@" + $('#txtStrDate').val() + "|" + $('#txtEndDate').val());
    $(".queryZone .otherfield").each(function () {

        var category = $(this).find("h3").attr("title");

        categoryoptions = [];
        $(this).find("input:checkbox:checked").each(function () {
            categoryoptions.push($(this).attr("id"));

        });
        if ($(this).find("option").length > 0) {
            if (typeof modfiedUsers != 'undefined') {
                categoryoptions = modfiedUsers;

            }
        }


        if (categoryoptions != null && categoryoptions.length > 0)
            categoryset.push(category + "@" + categoryoptions.join("|"));
    });
    refiner = categoryset.join("$");

    return refiner;
}
function updateFileType() {

    var categories = [];
    var fileTypes = [];
    var users = [];
    $("table#grid tbody").find('tr:visible').each(function () {
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
        //$("div.otherfield").eq(1).hide();
    }
    else
        $("div.otherfield").eq(1).show();
}
function updateRefine(modfiedUsers) {




    //if ($(".queryZone").length > 0) {
    //    updateFileType();
    //}


    filterQuery(modfiedUsers);
    unCheckSelections();
}
function BindRefine_Users() {
    var users = [];
    var modfiedUsers = [];


    $("div.otherfield").hover(function () {
        $(this).find('.chosen-drop').hide();
    });
    $("div.otherfield").find(".ia-filter-body").hover(function () {
        $(this).find('.chosen-drop').hide();
    });
    $("div.otherfield").find(".ia-filter-body").focus(function () {
        $(this).find('.chosen-drop').hide();
    });


    var input = $("div.otherfield").find(".ia-filter-body");
    var ukey = '';    

    input.keyup(function () {
        
        ukey = $(this).find('.active-result em').first().text();
        
        if ($("table#grid tbody").find('tr:visible').length > 0) {
            if (ukey.length > 2) {
                $(this).find('.chosen-drop').show();
            }
            else {
                $(this).find('.chosen-drop').hide();
            }
        }
    });


    if ($("div.otherfield").eq(0).find("option:selected:selected").length > 0) {
        modfiedUsers = $('#fileModifiedUser').chosen().val();
    }


    if ($(".queryZone").length > 0) {
        $("table#grid tbody").find('tr:visible').each(function () {
            var user = $(this).find('.userlink').length > 0 ? $(this).find('.userlink').html() : "";
            var user1 = user != "" ? user.replace(/[^A-Z0-9]/ig, "_") : ""; 
            var userIsthere = ($.inArray(user1, modfiedUsers) > -1) || modfiedUsers.length == 0;
            if (userIsthere)
                $(this).show();
            else
                $(this).hide();
            if ($.trim(user) != "" && $.inArray(user, users) === -1) {
                users.push(user);
            }
        });
    }

    var modifiedByHtml = buildRefinersModifiedBy(users, modfiedUsers);
    $("div.otherfield").eq(0).find(".ia-filter-body").html(modifiedByHtml);
    $('#fileModifiedUser').chosen({ width: '100%', no_results_text: 'No results match', search_contains: true }).change(function (e, params) {
        copyOfsearchTrigger();
    });

    if (users.length == 0) {
        //$("div.otherfield").eq(0).hide();
    }
    else
        $("div.otherfield").eq(0).show();
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
    var str = key.replace(/[^A-Z0-9]/ig, "_");
    //var str = key.replace(/\s+/g, '');
    if ($("#" + str + ":checked").length > 0)
        return "<div class='ia-filter-row'><input type='checkbox' id='" + str + "' checked='checked' class='ia-filter-input-checkbox' onchange='updateGrid()'><label for='" + key + "' class='ia-filter-label-checkbox'>" + key + "</label></div>";
    else
        return "<div class='ia-filter-row'><input type='checkbox' id='" + str + "' class='ia-filter-input-checkbox' onchange='updateGrid()'><label for='" + key + "' class='ia-filter-label-checkbox'>" + key + "</label></div>";


}
function buildRefinersModifiedBy(users, selectedUsers) {

    var html = "<select id='fileModifiedUser'  data-placeholder='Enter Name(s)...'  multiple='multiple'>";
    if (users.length > 0) {
        for (var i = 0; i < users.length; i++) {
            html += buildRefineModifiedBy(users[i], selectedUsers);
        }
    }
    html += "</select>";
    return html;
}
function buildRefineModifiedBy(key, selectedUsers) {
    var str = key.replace(/[^A-Z0-9]/ig, "_");
    //var str = key.replace(/\s+/g, '');	

    if (jQuery.inArray(str, selectedUsers) > -1)
        return "<option value='" + str + "' selected='selected'>" + key + "</option>";
    else
        return "<option value='" + str + "'>" + key + "</option>";


}
function searchkeyup() {

    $('.ia-searchBox').trigger("keyup");
}
function updateGrid()
{ searchkeyup(); }

function filterQuery(modfiedUsers) {

    var date = []; var modified = []; var filetype = []; var categoryMeta = []; var startdate = ""; var enddate = "";
    var qy = formrefinerQuery(modfiedUsers);

    var dateSelected = false;
    var fileTypeSelected = false;
    var filteredUsers = [];
    var filteredfileTypes = [];
    var modifiedUsers = [];
    var tempfileTypes = [];
    if (qy != "") {

        var querys = qy.split('$');
        for (var i = 0; i < querys.length; i++) {
            var category = querys[i].split('@');
            if (category[0] == "Date") {
                dateSelected = true;
                date = category[1].split('|');
                startdate = date[0];
                enddate = date[1];
            }

            else if (category[0] == "Modified By") {
                modified = category[1].split('|');

            }
            else if (category[0] == "File Type") {
                fileTypeSelected = true;
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
            else if (category[0] == "Category") {


                categoryMeta = category[1].trim().replace(/[^A-Z0-9,>]/ig, "_").split(',');

            }

        }

        if ($("table#grid tbody").find('tr').length > 0 && $("table#grid tbody").find('tr:visible').length > 0) {
            $("table#grid tbody").find('tr:visible').each(function () {
                var fType = $(this).find('.itemhiddenFileType').justtext();
                var user = $(this).find('.userlink').html().replace(/[^A-Z0-9]/ig, "_");
                var modifieddate = $(this).find('.ia-doc-list-modified').justtext();

                //var str = $(this).attr("id").replace(/\s+/g, '');
                var fileIsthere = ($.inArray(fType, filetype) > -1) || filetype.length == 0;
                //var userIsthere = ($.inArray(user, modified) > -1) || modified.length == 0;
                var datestart = (startdate != "" && (new Date(modifieddate) >= new Date(startdate))) || startdate == "";
                var dateend = (enddate != "" && (new Date(modifieddate) <= new Date(enddate))) || enddate == "";
                //var categoryitem = $(this).find('.itemCategory').justtext().trim().replace(/[^A-Z0-9,>]/ig, "_");
                //var categoryIsthere = getCategoryIsthere(categoryitem, categoryMeta) || categoryMeta.length == 0;
                if ($.inArray(user, modifiedUsers) === -1)
                    modifiedUsers.push(user);
                // && categoryIsthere == true && userIsthere == true
                if (datestart == true && dateend == true && fileIsthere == true) {
                    if (fileTypeSelected === true || dateSelected === true) {
                        if ($.inArray(fType, filteredfileTypes) === -1)
                            tempfileTypes.push(fType);

                        if ($.inArray(user, filteredUsers) === -1)
                            filteredUsers.push(user);
                    }
                }
                else {
                    $(this).hide();

                }


            });
        }
    }

    if ($("table#grid tbody").find('tr:visible').length == 0) {
        var emptymodifiedByHtml = "<select id='fileModifiedUser'  data-placeholder='Enter Name(s)...'  multiple='multiple'><option value=\"\"></option></select>";
        $("#noresults").show();
        $("div.otherfield").eq(1).find('.ia-filter-body').html('');
       // $("div.otherfield").eq(1).find('.ia-accordion-icon').click();
       // $("div.otherfield").eq(0).find('.ia-accordion-icon').click();
        $("div.otherfield").eq(0).find(".ia-filter-body").html(emptymodifiedByHtml);
        $('#fileModifiedUser').chosen({ width: '100%', no_results_text: 'No results match', search_contains: true });
    }
    else {
        $("#noresults").hide();

        if (fileTypeSelected === true || dateSelected === true) {
            /* var modifiedByHtml = buildRefinersModifiedBy(modifiedUsers, filteredUsers);
             $("div.otherfield").eq(0).find(".ia-filter-body").html(modifiedByHtml);
             $('#fileModifiedUser').chosen({ width: '100%', no_results_text: 'No results match', search_contains: true }).change(function (e, params) {
                 copyOfsearchTrigger();
             });*/

            if (!fileTypeSelected)
                updateFileType();
            var checkedfileTypes = $('.ia-filter-row input:checkbox:checked');
            if (checkedfileTypes.length > 0) {
                checkedfileTypes.each(function () {
                    var ftype = $(this).attr('id');
                    if ($.inArray(ftype, filteredfileTypes) === -1)
                        filteredfileTypes.push(ftype);
                });
            }
            else {
                $('.ia-filter-row input:checkbox').each(function () {
                    var ftype = $(this).attr('id');
                    if ($.inArray(ftype, filteredfileTypes) === -1)
                        filteredfileTypes.push(ftype);
                });
            }
            var filetype = [];
            var tempfiletype = [];
            jQuery.each(filteredfileTypes, function (i, val) {
                if ($.inArray(val, mapArray) == -1) {
                    filetype.push(val.toLowerCase());
                }
                else {
                    if (val == wordMapyKey) { filetype = $.merge($.merge([], word), filetype); }
                    else if (val == powerPointMapKey) { filetype = $.merge($.merge([], powerPoint), filetype); }
                    else if (val == excelMapKey) { filetype = $.merge($.merge([], excel), filetype); }
                }
            });
            if (tempfileTypes.length > 0) {
                jQuery.each(filetype, function (key, val) {
                    if ($.inArray(val, tempfileTypes) >= 0)
                        tempfiletype.push(val);
                });
            }


            //BindRefine_Users();

            filter_BindUsers(tempfiletype, fileTypeSelected, dateSelected);
            fileTypeSelected = false;
            dateSelected = false;
        }
        else {
            updateFileType();

        }

    }
}

function filter_BindUsers(filteredfileTypes, fileTypeSelected, dateSelected) {
    var updatedUsers = [];
    var selectedUsers = [];
    if (griditems.length > 0) {
        if (dateSelected && fileTypeSelected) {
            for (var i = 0; i < griditems.length; i++) {
                var start_date = $('#txtStrDate').val();
                var end_date = $('#txtEndDate').val();
                var modified_date = new Date(griditems[i].modified.trim());

                var datestart = (start_date != "" && (new Date(modified_date) >= new Date(start_date))) || start_date == "";
                var dateend = (end_date != "" && (new Date(modified_date) <= new Date(end_date))) || end_date == "";


                if (datestart == true && dateend == true) {
                    if ($.inArray(griditems[i].fileType.toLowerCase(), filteredfileTypes) != -1) {
                        if ($.inArray(griditems[i].user, updatedUsers) === -1)
                            updatedUsers.push(griditems[i].user);
                    }
                }

                /*if ($.inArray(griditems[i].fileType.toLowerCase(), filteredfileTypes) != -1) {
                    if ($.inArray(griditems[i].user, updatedUsers) === -1)
                        updatedUsers.push(griditems[i].user);
                }*/
            }

        }
        else if (dateSelected) {
            /*if ($("table#grid tbody").find('tr').length > 0 && $("table#grid tbody").find('tr:visible').length > 0) {
                $("table#grid tbody").find('tr:visible').each(function () {
                    var user = $(this).find('.userlink').html();
                    if ($.inArray(user, updatedUsers) === -1)
                        updatedUsers.push(user);

                });
            }*/

            for (var i = 0; i < griditems.length; i++) {
                var start_date = $('#txtStrDate').val();
                var end_date = $('#txtEndDate').val();
                var modified_date = new Date(griditems[i].modified.trim());

                var datestart = (start_date != "" && (new Date(modified_date) >= new Date(start_date))) || start_date == "";
                var dateend = (end_date != "" && (new Date(modified_date) <= new Date(end_date))) || end_date == "";

                if (datestart == true && dateend == true) {

                    if ($.inArray(griditems[i].user, updatedUsers) === -1)
                        updatedUsers.push(griditems[i].user);

                }
            }
        }

        else {

            for (var i = 0; i < griditems.length; i++) {
                if (griditems[i].fileType != "" || griditems[i].fileType != 'undefined') {
                    /*if (dateSelected) {
                        var start_date = $('#txtStrDate').val();
                        var end_date = $('#txtEndDate').val();
                        var modified_date = new Date(griditems[i].modified.trim());
    
                        var datestart = (start_date != "" && (new Date(modified_date) >= new Date(start_date))) || start_date == "";
                        var dateend = (end_date != "" && (new Date(modified_date) <= new Date(end_date))) || end_date == "";
    
    
                        if (datestart == true && dateend == true) {
                            if ($.inArray(griditems[i].user, updatedUsers) === -1)
                                updatedUsers.push(griditems[i].user);
                        }
                    }*/
                    if (fileTypeSelected) {
                        if ($.inArray(griditems[i].fileType.toLowerCase(), filteredfileTypes) != -1) {
                            if ($.inArray(griditems[i].user, updatedUsers) === -1)
                                updatedUsers.push(griditems[i].user);
                        }
                    }

                }
            }
        }
        if ($("div.otherfield").eq(0).find("option:selected:selected").length > 0) {
            selectedUsers = $('#fileModifiedUser').chosen().val();
        }
        var modifiedByHtml = updateFileHTML(updatedUsers, selectedUsers);
        $("div.otherfield").eq(0).find(".ia-filter-body").html(modifiedByHtml);
        $('#fileModifiedUser').chosen({ width: '100%', no_results_text: 'No results match', search_contains: true }).change(function (e, params) {
            copyOfsearchTrigger();
        });
        if (griditems.length == 0) {
            //$("div.otherfield").eq(0).hide();
        }
        else
            $("div.otherfield").eq(0).show();

    }
}


function updateFileHTML(users, selectedUsers) {
    var html = "<select id='fileModifiedUser'  data-placeholder='Enter Name(s)...'  multiple='multiple'>";
    if (users.length > 0) {
        for (var i = 0; i < users.length; i++) {
            html += updateRefineModifiedBy(users[i], selectedUsers);
        }
    }
    html += "</select>";
    return html;
}

function updateRefineModifiedBy(key, selectedUsers) {
    var str = key.replace(/[^A-Z0-9]/ig, "_");
    //var str = key.replace(/\s+/g, '');	

    if (jQuery.inArray(str, selectedUsers) > -1)
        return "<option value='" + str + "' selected='selected'>" + key + "</option>";
    else
        return "<option value='" + str + "'>" + key + "</option>";


}

function getCategoryIsthere(value, categoryMeta) {

    if ($.trim(value) != "") {
        var itemCategories = value.split(",");
        for (var i = 0; i < itemCategories.length; i++) {
            for (var j = 0; j < categoryMeta.length; j++) {

                if (itemCategories[i].toLowerCase() == categoryMeta[j].toLowerCase()) {
                    //if (itemCategories[i].indexOf(categoryMeta[j]) > -1) {
                    return true;
                }
            }
        }
    }
    return false;
}


function SetRefineQuery() {
    searchkeyup(); //getPostBack(false, false, false); 
}
function datechange(element) {
    window.location.href = "#";
    $('#txtStrDate').val($(element).attr("date"));
    $('#txtEndDate').val("");
    //getPostBack(false, false, false);
    searchkeyup();
}
var griditems = [];
function updateTabCount(modfiedUsers) {

    var tab = $('#lblID').length > 0 && $('#lblID').val() != "" ? $('#lblID').val() : "0";
    var totalcount = 0; var myfileCount = 0; var recentCount = 0; var popularcount = 0;
    var userName = $('.myFiles').length > 0 ? $('.myFiles').attr("user") : "";
    var arrSort = [];
    var modifiedBy = [];
    var filetype = [];

    var filesuploaded = [];
    var fupload = $('#hdnUploadedFiles').val();
    if (fupload != '')
        filesuploaded = fupload.split(',');

    if ($("table#grid tbody").find('tr').length > 0) {
        $("table#grid tbody").find('tr').each(function (index) {
            var fileType = $.trim($(this).find('.itemhiddenFileType').justtext());
            var user = $(this).find('.userlink').html();

            if (fileType != "") {
                totalcount = totalcount + 1;
                if (user == userName)
                    myfileCount = myfileCount + 1;
            }
            if (fupload != '') {
                if (fileType != '') {
                    var filecheck = $(this).find('.ia-doclist-name').find('#HyperLink1').justtext() + "." + fileType;

                    if (filesuploaded.indexOf(filecheck) != -1) {

                        $(this).attr('class', 'ia-doclist-row-successful');
                        var that = $(this);
                        setTimeout(function () {
                            that.attr('class', '');
                            if ($('#hdnUploadedFiles').val() != '')
                                $('#hdnUploadedFiles').val('');
                        }, 10000);
                    }
                }
            }




        });

        popularcount = totalcount;
        if ($(".popularFiles").attr("file-count") != "0") {
            recentCount = parseInt($(".popularFiles").attr("file-count"));
            if (totalcount < popularcount) {
                popularcount = totalcount;
            }
        }
        else
            popularcount = totalcount;

        recentCount = totalcount;
        if ($(".recentFiles").attr("file-count") != "0") {
            recentCount = parseInt($(".recentFiles").attr("file-count"));
            if (totalcount < recentCount) {
                recentCount = totalcount;
            }
        }
        else
            recentCount = totalcount;

        if ($('.allfilecount'))
            $('.allfilecount').html(totalcount);
        if ($('.myfilecount'))
            $('.myfilecount').html(myfileCount);

        if ($('.recentfilecount'))
            $('.recentfilecount').html(recentCount);
        if ($('.popularfilecount'))
            $('.popularfilecount').html(recentCount);
        getsortedcolumns("ia-doclist-name", true, "string");
        griditems = [];
    }

    if ($("table#grid tbody").find('tr').length > 0 && $("table#grid tbody").find('tr:visible').length > 0) {
        $("table#grid tbody").find('tr:visible').each(function (index) {
            var fileType = $.trim($(this).find('.itemhiddenFileType').justtext());
            var user = $(this).find('.userlink').html();



            if (tab == "0") {
                //griditems = [];
                var gobj = {};
                gobj.Id = $(this).find('.itemhidden').justtext();
                gobj.fileType = fileType;
                gobj.modified = $(this).find('.ia-doc-list-modified').justtext();
                gobj.user = user;
                griditems.push(gobj);

            }
            else if (tab == "1") {
                if (fileType != "") {

                    if (user == userName) {
                        var gobj = {};
                        gobj.Id = $(this).find('.itemhidden').justtext();
                        gobj.fileType = fileType;
                        gobj.modified = $(this).find('.ia-doc-list-modified').justtext();
                        gobj.user = user;
                        griditems.push(gobj);
                    }
                    else
                        $(this).hide();
                }
                else
                    $(this).hide();
            }
            else if (tab == "3") {
                if (fileType != "") {


                    var obj = {};
                    obj.Id = $(this).find('.itemhidden').justtext();
                    obj.html = $(this).html();
                    obj.time = new Date($(this).find('.itemhiddenDatenTime').justtext());
                    obj.index = $(this).index();
                    arrSort.push(obj);

                }
                else
                    $(this).hide();

            }
            else if (tab == "2") {
                if (fileType != "") {


                    var obj = {};
                    obj.Id = $(this).find('.itemhidden').justtext();
                    obj.html = $(this).html();
                    obj.count = parseInt($(this).find('.itemPopularCount').justtext());
                    obj.index = $(this).index();
                    arrSort.push(obj);

                }
                else
                    $(this).hide();

            }

        });
        if (tab == "3") {
            for (var i = 0; i < arrSort.length; i++) {
                for (var j = i + 1; j < arrSort.length; j++) {
                    if (arrSort[i].time < arrSort[j].time) {
                        var temp = arrSort[i];
                        arrSort[i] = arrSort[j];
                        arrSort[j] = temp;
                    }
                }
            }

            for (var i = 0; i < arrSort.length; i++) {
                //$(sorted[i].html).prependTo("table#grid tbody");
                $("table#grid tbody tr:visible").eq(i).html(arrSort[i].html);
            }
            if ($(".recentFiles").attr("file-count") != "0") {
                var recentcount = (arrSort.length >= parseInt($(".recentFiles").attr("file-count")) ? parseInt($(".recentFiles").attr("file-count")) : arrSort.length);
                recentcount = recentcount - 1;
                $('table#grid tbody tr:visible:gt(' + recentcount + ')').hide();
            }
        }
        if (tab == "2") {
            for (var i = 0; i < arrSort.length; i++) {
                for (var j = i + 1; j < arrSort.length; j++) {
                    if (arrSort[i].count < arrSort[j].count) {
                        var temp = arrSort[i];
                        arrSort[i] = arrSort[j];
                        arrSort[j] = temp;
                    }
                }
            }

            for (var i = 0; i < arrSort.length; i++) {
                //$(sorted[i].html).prependTo("table#grid tbody");
                $("table#grid tbody tr:visible").eq(i).html(arrSort[i].html);
            }
            if ($(".popularFiles").attr("file-count") != "0") {
                var recentcount = (arrSort.length >= parseInt($(".popularFiles").attr("file-count")) ? parseInt($(".popularFiles").attr("file-count")) : arrSort.length);
                recentcount = recentcount - 1;
                $('table#grid tbody tr:visible:gt(' + recentcount + ')').hide();
            }
        }
        if (tab == "2" || tab == "3") {
            if ($("table#grid tbody").find('tr').length > 0 && $("table#grid tbody").find('tr:visible').length > 0) {
                $("table#grid tbody").find('tr:visible').each(function (index) {
                    var fileType = $.trim($(this).find('.itemhiddenFileType').justtext());
                    var user = $(this).find('.userlink').html();

                    var gobj = {};
                    gobj.Id = $(this).find('.itemhidden').justtext();
                    gobj.fileType = fileType;
                    gobj.modified = $(this).find('.ia-doc-list-modified').justtext();
                    gobj.user = user;
                    griditems.push(gobj);
                });
            }
        }
    }

    $('#ulContextMenu').show();
    BindRefine_Users();
    updateRefine(modfiedUsers);
    previewSelect();
    callPreview();
}

jQuery.fn.justtext = function () {

    return $(this).clone()
            .children()
            .remove()
            .end()
            .text();

};
function refreshIdle(i) {
    if ($('#hdnUploadedFiles').length > 0 && $('#hdnUploadedFiles').val().trim() != "") {
    }
    else {
        if (i <= times) {
            // if ($('#refreshIdleTab'))
            // $('#refreshIdleTab').val("true");
            // if ($('#refreshIdleRF'))
            // $('#refreshIdleRF').val("true");
            if ($('#refreshIdleGrid'))
                $('#refreshIdleGrid').val("true");
            if ($('#refreshIdleFT'))
                $('#refreshIdleFT').val("true");

            $('#viewSateReset').click();
        }
    }

}
function clearpagecache() {
    $.ajax({
        url: "",
        context: document.body,
        success: function (s, x) {

            $('html[manifest=saveappoffline.appcache]').attr('content', '');
            $(this).html(s);

        }
    });

}

function setValuesToFields(cookieName, cookieValue) {

    var arr = cookieValue.split('!');
    var searchtext = arr[0];
    var recursive = arr[1];
    var fd = arr[2];
    var qy = arr[3];
    var tab = arr[4];
    var tabID = '0';//arr[5];
    //setValuesToFields(cookieName, cookieValue + "!true");
    //if ($('#currentFolderPath').attr('value') != fd) {
    showloader();
    if (recursive != "")
        $('#ddlSelect option[value="' + recursive + '"]').prop('selected', true);

    $('#folderBox').attr('value', fd);
    if ($('#hdnQueryQF'))
        $('#hdnQueryQF').attr('value', '');
    if ($('#txtSearch'))
        $('#txtSearch').val(searchtext);
    if ($('#lblID'))
        $('#lblID').attr('value', tabID);
    if ($('#currentFolderPath'))
        $('#currentFolderPath').attr('value', '');
    if ($('#hdnPathQF'))
        $('#hdnPathQF').attr('value', '');

    window.location.reload();
    // $('.folderchange').click();
    //}

}
function setCookieWithReload(cvalue) {


    var cookieName = $('#akuminaCookieName').val();
    var d = new Date();
    d.setTime(d.getTime() + (1 * 24 * 60 * 60 * 1000));
    var expires = "expires=" + d.toGMTString();
    document.cookie = cookieName + "=" + cvalue + "!true" + "; " + expires;
    window.location.reload();
}
function setCookie() {
    //clearpagecache();
    var cvalue = GetUrl();
    var cookieName = $('#akuminaCookieName').val();
    var d = new Date();
    d.setTime(d.getTime() + (1 * 24 * 60 * 60 * 1000));
    var expires = "expires=" + d.toGMTString();
    document.cookie = cookieName + "=" + cvalue + "; " + expires;
    document.cookie = "WOPISessionContext=" + window.location.href + ";path=/";
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
function GetUrl() {
    var searchtext = $('#txtSearch').val();
    var recursive = $('#ddlSelect').find(":selected").text() != "This Folder" ? $('#ddlSelect').find(":selected").text() : "";
    var fd = $('#currentFolderPath').prop('value') != null ? $('#currentFolderPath').attr('value') : "";
    var qy = "";// formrefinerQuery();
    var tab = "";//$('#tabStr').prop('value') != null ? $('#tabStr').attr('value') : "";
    var tabID = $('#lblID').val();
    return searchtext + "!" + recursive + "!" + fd + "!" + qy + "!" + tab + "!" + tabID;
}
function dropdown_change() {
    var recursive = $("#ddlSelect")[0].selectedIndex > 0 ? "recursive" : "this";
    if ($('#hdnQuerystatus'))
        $('#hdnQuerystatus').attr('value', recursive);
    if ($('#hdnsubstatus'))
        $('#hdnsubstatus').attr('value', recursive);
    if ($('#refinerReset'))
        $('#refinerReset').val("true");
    if ($('#refreshIdleFT'))
        $('#refreshIdleFT').val("true");
    showloader();
    document.getElementById('viewSateReset').click();

}


//$('#drop_zone').on("click", ".filename", function () {
function folderNavigation(element) {
    var file = $(element);
    var filetype = removehiddenvalue($(file).find('.itemhiddenFileType').html());
    var filepath = removehiddenvalue($(file).find('.itemhiddenPath').html());
    if (filetype != '&nbsp;') {
        var baseUrl = $('#webUrl').val();
        var docUrl = baseUrl + filepath;

        editDocumentWithProgID2(docUrl, '', 'SharePoint.OpenDocuments', '0', baseUrl, '0');
    }
    else {
        showloader();
        if ($('#lblID'))
            $('#lblID').attr('value', '0');
        if ($('#txtSearch'))
            $('#txtSearch').val('');
        if ($('#searchTextTab'))
            $('#searchTextTab').val('');
        if ($('#searchTextQuery'))
            $('#searchTextQuery').val('');
        if ($('#hdnQuerystatus'))
            $('#hdnQuerystatus').attr('value', 'reset');
        //if ($('.lblBreadCrum'))
        //    $('.lblBreadCrum').html($(file).find('.itemhiddenPath').html());

        if ($('#folderBox'))
            $('#folderBox').attr('value', filepath);
        if ($('#hdnQueryQF'))
            $('#hdnQueryQF').attr('value', '');
        if ($('#currentFolderPath'))
            $('#currentFolderPath').attr('value', filepath);
    }
}
//});
$(function () {
    $('#searchZone').on("click", "button", function () {
        showloader();
        var searchtext = $('#txtSearch').val();
        //if ($('#hdnQuerystatus'))
        //    $('#hdnQuerystatus').attr('value', 'true');
        if ($('#hdnQuerystatus'))
            $('#hdnQuerystatus').attr('value', 'reset');
        if ($('#searchTextTab'))
            $('#searchTextTab').attr('value', searchtext);
        if ($('#searchTextQuery'))
            $('#searchTextQuery').attr('value', searchtext);

    });
    //$(".ia-document-library").width(Math.round($("#searchZone").width() * (0.95)));
    hideloader();

    $('.ia-sort-asc').click(function () {
        showloader();
    });
})


function sortinghead(element) {
    $(element).next().find("input").click();
}
function getfolder(value) {
    var filepath = value;
    window.location.href = "#";
    ResetRefiner();
    showloader();
    if ($('#ddlSelect'))
        $('#ddlSelect').val("This Folder");


    if ($('#lblID'))
        $('#lblID').attr('value', '0');
    if ($('#txtSearch'))
        $('#txtSearch').val('');
    if ($('#searchTextTab'))
        $('#searchTextTab').val('');
    if ($('#searchTextQuery'))
        $('#searchTextQuery').val('');
    if ($('#hdnQuerystatus'))
        $('#hdnQuerystatus').attr('value', 'reset');

    if ($('#folderBox'))
        $('#folderBox').attr('value', filepath);
    if ($('#hdnQueryQF'))
        $('#hdnQueryQF').attr('value', '');
    if ($('#currentFolderPath'))
        $('#currentFolderPath').attr('value', filepath);
    document.getElementById('viewSateReset').click();



}
function getcurrentfolder(element) {
    window.location.href = "#";
    ResetRefiner();
    var folder = $(element).attr('url-data');
    if ($('#folderBox')) {
        showloader();
        $('#folderBox').attr('value', folder);

        if ($('#ddlSelect'))
            $('#ddlSelect').val("This Folder");
        if ($('#hdnQueryQF'))
            $('#hdnQueryQF').attr('value', '');
        if ($('#txtSearch'))
            $('#txtSearch').val('');
        if ($('#searchTextTab'))
            $('#searchTextTab').val('');
        if ($('#searchTextQuery'))
            $('#searchTextQuery').val('');

        // if ($('#refreshIdleTab'))
        // $('#refreshIdleTab').val("true");
        // if ($('#refreshIdleRF'))
        // $('#refreshIdleRF').val("true");  
        if ($('#refreshIdleFT'))
            $('#refreshIdleFT').val("true");

        if ($('#lblID'))
            $('#lblID').attr('value', '0');
        if ($('#currentFolderPath'))
            $('#currentFolderPath').attr('value', '');
        if ($('#hdnPathQF'))
            $('#hdnPathQF').attr('value', '');
        $('.folderchange').click();


    }
}

function hideloader() {
    if ($('.ia-loading-panel')) $('.ia-loading-panel').addClass("ia-hide");
    $('.ia-loading-panel').removeClass("ia-show");
}

function showloader() {
    hideAllButton(); if ($('.ia-loading-panel')) $('.ia-loading-panel').addClass("ia-show");
    $('.ia-loading-panel').removeClass("ia-hide");
}

function fileCreateFolder() {
    if ($('#txtSearch'))
        $('#txtSearch').val('');
    if ($('#searchTextTab'))
        $('#searchTextTab').val('');
    if ($('#searchTextQuery'))
        $('#searchTextQuery').val('');
    if ($('#hdnQuerystatus'))
        $('#hdnQuerystatus').attr('value', 'reset');


    if ($('#hdnQueryQF'))
        $('#hdnQueryQF').attr('value', '');

    var listname = $('#listName').val();
    var folderPath = $('#webServerUrl').val() + '/' + $('#currentFolderPath').val();
    var baseUrl = $('#webUrl').val();

    var options = {
        url: baseUrl + '/' + listname + '/Forms/Upload.aspx?RootFolder=' + folderPath + '&Type=1',
        tite: 'Create Folder',
        allowMaximize: false,
        width: 550,
        height: 200,
        showClose: true,
        dialogReturnValueCallback: onPopUpCloseCallBackWithData
    };
    SP.SOD.execute('sp.ui.dialog.js', 'SP.UI.ModalDialog.showModalDialog', options);
}
function onPopUpCloseCallBackWithData(result, returnValue) {
    if (result == SP.UI.DialogResult.OK) {
        pageRefreshCreate();
    }
}

function fileCreateDocument(template) {
    //dropdown-1
    if ($('#txtSearch'))
        $('#txtSearch').val('');
    if ($('#searchTextTab'))
        $('#searchTextTab').val('');
    if ($('#searchTextQuery'))
        $('#searchTextQuery').val('');
    if ($('#searchTextTab'))
        $('#searchTextTab').val('');

    if ($('#hdnQuerystatus'))
        $('#hdnQuerystatus').attr('value', 'reset');

    if ($('#hdnQueryQF'))
        $('#hdnQueryQF').attr('value', '');
    $('#dropdown-1').css('display', 'none');
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


    var options = {
        url: baseUrl + '/_layouts/15/Upload.aspx?List={' + listId + '}&RootFolder=' + folderPath,
        tite: 'Upload Files',
        allowMaximize: false,
        width: 800,
        height: 500,
        showClose: true,
        dialogReturnValueCallback: onPopUpCloseCallBackWithData
    };
    SP.SOD.execute('sp.ui.dialog.js', 'SP.UI.ModalDialog.showModalDialog', options);


}
function insertIntoDic(dictionary, key, value) {
    // If key is not initialized or some bad structure
    if (!dictionary[key] || !(dictionary[key] instanceof Array)) {
        dictionary[key] = [];
    }
    // All arguments, exept first push as valuses to the dictonary
    dictionary[key] = dictionary[key].concat(value);
    return dictionary;
}

function fileCheckOut() {
    OverAllFailureMessage = "";
    //var sList = '';
    var dictionary = {};

    $('.selection:visible').each(function () {
        if (this.checked) {
            var listName = $(this).parent().parent().find('.itemListDisplayName').justtext().split(';')[0];

            var itemId = $(this).parent().parent().find('.itemhidden').justtext();
            //sList += removehiddenvalue($(this).parent().parent().find('.itemhidden').html()) + ';';

            dictionary = insertIntoDic(dictionary, listName, itemId);

        }
    });

    TotalCount = Object.keys(dictionary).length;
    currentCount = 0;
    for (var key in dictionary) {

        if (dictionary.hasOwnProperty(key)) {

            var clientContext = SP.ClientContext.get_current();
            if (clientContext != undefined && clientContext != null) {
                var webSite = clientContext.get_web();
                this.list = webSite.get_lists().getByTitle(key);
                var lstID = '';
                var i;
                var myarray = dictionary[key];//sList.split(';');
                var item, file;

                for (var i = 0; i < myarray.length; i++) {
                    lstID = myarray[i];
                    item = this.list.getItemById(lstID);
                    file = item.get_file();
                    file.checkOut();
                    clientContext.load(file);
                }
                clientContext.executeQueryAsync(Function.createDelegate(this, this.OnLoadSuccess), Function.createDelegate(this, this.OnLoadFailed));

            }

        }
    }

}

function OnLoadSuccess(sender, args) {
    currentCount = currentCount + 1;

    if (currentCount == TotalCount) {
        OverallComplete('File(s) Checked out successfully');
    }
}

function OnLoadFailed(sender, args) {
    if (args.get_message() != 'Input string was not in a correct format.') {

        OverAllFailureMessage += 'Request failed. ' + args.get_message() + "\n";
    }
    currentCount = currentCount + 1;
    if (currentCount == TotalCount) {
        OverallComplete('File(s) Checked out successfully');
    }
}

function OverallComplete(message) {
    if (OverAllFailureMessage != "") {
        alert(OverAllFailureMessage);
        pageRefresh();
    }
    else {
        alert(message);
        pageRefresh();
    }

}


function updaterefiner() {
    //if ($('#hdnQuerystatus'))
    //    $('#hdnQuerystatus').attr('value', 'resetRefine');
    //if ($('#refinerReset'))
    //    $('#refinerReset').val("true");
}

function deletefileReftesh() {
    showloader();
    //$('#lblID').attr('value', '0');

    //if ($('#hdnQuerystatus'))
    //    $('#hdnQuerystatus').attr('value', 'resetRefine');
    //if ($('#refinerReset'))
    //    $('#refinerReset').val("deleteFile");
    //if ($('#deleteDataBind'))
    //    $('#deleteDataBind').click();
    //else
    document.getElementById('viewSateReset').click();
}
function pageRefresh() {
    showloader();
    //unCheckSelections();
    //$('#lblID').attr('value', '0');
    //if ($('#txtSearch'))
    //    $('#txtSearch').val('');
    //updaterefiner();
    document.getElementById('viewSateReset').click();
}
function pageRefreshCreate() {
    showloader();
    //$('#lblID').attr('value', '0');
    //if ($('#txtSearch'))
    //    $('#txtSearch').val('');
    //updaterefiner();

    document.getElementById('viewSateReset').click();
}

function ResetRefiner() {


    $('.ia-filter-row input:checkbox:checked').each(function () {
        $(this).removeAttr('checked');
    });
    $('.ia-filter-body input:text').each(function () {
        $(this).val('');
    });
    if ($('#fileModifiedUser'))
        $('#fileModifiedUser').val('').trigger('chosen:updated');
    //if ($('#txtStrDate'))
    //    $('#txtStrDate').val('');
    //if ($('#txtEndDate'))
    //    $('#txtEndDate').val('');



}
function fnSorting() {
    $("table#grid thead tr th").click(function () {

        if (!$(this).hasClass("ia-doclib-header-checkbox")) {

            var sortorder = $(this).hasClass("headerSortUp") ? true : false; var sortcolumn = ""; var type = "string";
            $("table#grid thead tr th.headerSortUp").each(function () {
                $(this).removeClass("headerSortUp");

            });
            $("table#grid thead tr th.headerSortDown").each(function () {
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
}

function getsortedcolumns(sortcolumn, sortorder, type) {



    if ($("table#grid tbody").find('tr').length > 0 && $("table#grid tbody").find('tr:visible').length > 0) {
        showloader(); var arrSort = []; var foldersort = [];
        $("table#grid tbody").find('tr:visible').each(function () {
            var key = $(this).find("." + sortcolumn + " a").length > 0 ? $(this).find("." + sortcolumn + " a").html() : $(this).find("." + sortcolumn).justtext();
            var folder = ($(this).find("a.disp-File-Name").length == 0) || $(this).find(".itemhiddenFileType").justtext() == "one";
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
                //$(sorted[i].html).prependTo("table#grid tbody");
                $("table#grid tbody tr:visible").eq(i).html(foldersort[i].html);
            }
            init = folderlength;
        }
        if (filelength > 0) {
            for (var i = init; i < (init + filelength) ; i++) {
                //$(sorted[i].html).prependTo("table#grid tbody");
                $("table#grid tbody tr:visible").eq(i).html(arrSort[i - init].html);
            }
            init = filelength;
        }


        if (!sortorder && folderlength > 0) {
            for (var i = init; i < (folderlength + init) ; i++) {
                //$(sorted[i].html).prependTo("table#grid tbody");
                $("table#grid tbody tr:visible").eq(i).html(foldersort[i - init].html);
            }

        }

        $("#ulContextMenu").show();
        hideloader();

    }
}
function stringSorting(arr, descorder) {
    /*var sorted = arr.sort(function (a, b) {
        if (a.key > b.key) return -1;
        if (a.key < b.key) return 1;
        return 0;
    });
    if (descorder)
        sorted = sorted.reverse();
    return sorted;*/
    for (var i = 0; i < arr.length; i++) {
        for (var j = i + 1; j < arr.length; j++) {
            if (arr[i].key < arr[j].key) {
                var temp = arr[i];
                arr[i] = arr[j];
                arr[j] = temp;
            }
        }
    }

    if (descorder)
        arr = arr.reverse();
    return arr;
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

function fileCheckIn() {
    //Clear checkboxes in the modal before checkint
    ClearCheck();

    ReintializeCheckInComment();
    if ($(".hintText").length > 0 && $(".hintText").is(":visible")) {
        if ($('#hdnMinorCheckInEnable').length > 0 && $('#hdnMinorCheckInEnable').val() == "minor") {
            $.magnificPopup.open({
                items: {
                    src: '.ia-fileCheckInOption',

                    type: 'inline',

                },
                closeOnBgClick: false,
                closeBtnInside: true
            });
        }
        else if ($('#hdnMinorCheckInEnable').length > 0 && $('#hdnMinorCheckInEnable').val() == "major") {
            if ($('.fileCheckinNo').length > 0)
                $('.fileCheckinNo').click();
            $.magnificPopup.open({
                items: {
                    src: '.ia-fileCheckInMajorOption',

                    type: 'inline',

                },
                closeOnBgClick: false,
                closeBtnInside: true
            });
        }
        else {
            fileCheckinWithOutOptions();
        }
    }
    else {
        fileCheckinWithOutOptions();
    }
}
function fileCheckinWithOutOptions() {
    OverAllFailureMessage = "";
    var dictionary = {};

    $('.selection:visible').each(function () {
        if (this.checked) {
            var listName = $(this).parent().parent().find('.itemListDisplayName').justtext().split(';')[0];
            var itemId = $(this).parent().parent().find('.itemhidden').justtext();
            dictionary = insertIntoDic(dictionary, listName, itemId);

        }
    });

    TotalCount = Object.keys(dictionary).length;
    currentCount = 0;
    for (var key in dictionary) {

        if (dictionary.hasOwnProperty(key)) {

            var clientContext = SP.ClientContext.get_current();
            if (clientContext != undefined && clientContext != null) {
                var webSite = clientContext.get_web();
                this.list = webSite.get_lists().getByTitle(key);

                var lstID = '';
                var i;
                var myarray = dictionary[key];
                var item, file;

                for (var i = 0; i < myarray.length; i++) {
                    lstID = myarray[i];
                    item = this.list.getItemById(lstID);

                    file = item.get_file();
                    file.checkIn('Checked in using ECMA script', 1);

                    clientContext.load(file);
                }
                clientContext.executeQueryAsync(Function.createDelegate(this, this.OnLoadCheckInSuccess), Function.createDelegate(this, this.OnLoadCheckInFailed));
            }

        }
    }
}
//Checks in or checks out based on radio button input
//Clears modal radio buttons and hides minor/major version divs
function fileCheckinWithOptions() {
    $.magnificPopup.close();
    var sList = '';

    $('.selection:visible').each(function () {
        if (this.checked) {
            sList += removehiddenvalue($(this).parent().parent().find('.itemhidden').html()) + ';';
        }
    });

    var listname = $('#listDisplayName').val();
    TotalCount = 1;
    currentCount = 0;
    OverAllFailureMessage = '';
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


            if ($(".checkInFileOption:checked").length > 0 || $(".checkInCheckOutFileOption:checked").length > 0) {
                {
                    var Comment = '';
                    if ($(".checkInFileOption:checked").length > 0)
                        Comment = $('.checkinCommentOption').val();
                    else
                        Comment = $('.checkInCheckOutCommentOption').val();
                    if ($(".checkInFileOption:checked").length > 0) {
                        if ($(".checkInDraftOption:checked").length > 0) {
                            file.checkIn(Comment, 0);
                            $(".checkInDraftOption").prop("checked", false);
                        }
                        else {
                            file.checkIn(Comment, 1);
                            $(".checkInPublishOption").prop("checked", false);
                        }
                        $(".checkInFileOption").prop("checked", false);
                    }
                    else {
                        if ($(".checkInCheckoutDraftOption:checked").length > 0) {
                            file.checkIn(Comment, 0);
                            $(".checkInCheckoutDraftOption").prop("checked", false);
                        }
                        else {
                            file.checkIn(Comment, 1);
                            $(".checkInCheckoutPublishOption").prop("checked", false);
                        }
                    }
                }
                if ($(".checkInCheckOutFileOption:checked").length > 0) {
                    file.checkOut();
                    $(".checkInCheckOutFileOption").prop("checked", false);
                }
                $('.ia-checkin-all').hide();
                $('.ia-checkin-changes').hide();
            }
            else {
                file.checkIn('Checked in using ECMA script', 1);
            }
            clientContext.load(file);
        }
        clientContext.executeQueryAsync(Function.createDelegate(this, this.OnLoadCheckInSuccess), Function.createDelegate(this, this.OnLoadCheckInFailed));
    }
}

//Clears modal radio buttons and and hides minor/major version divs
function clearModal() {
    if ($(".checkInFileOption:checked").length > 0 || $(".checkInCheckOutFileOption:checked").length > 0) {
        {
            var Comment = '';
            if ($(".checkInFileOption:checked").length > 0)
                Comment = $('.checkinCommentOption').val();
            else
                Comment = $('.checkInCheckOutCommentOption').val();
            if ($(".checkInFileOption:checked").length > 0) {
                if ($(".checkInDraftOption:checked").length > 0) {
                    $(".checkInDraftOption").prop("checked", false);
                }
                else {
                    $(".checkInPublishOption").prop("checked", false);
                }
                $(".checkInFileOption").prop("checked", false);
            }
            else {
                if ($(".checkInCheckoutDraftOption:checked").length > 0) {
                    $(".checkInCheckoutDraftOption").prop("checked", false);
                }
                else {
                    $(".checkInCheckoutPublishOption").prop("checked", false);
                }
            }
        }
        if ($(".checkInCheckOutFileOption:checked").length > 0) {
            $(".checkInCheckOutFileOption").prop("checked", false);
        }
        $('.ia-checkin-all').hide();
        $('.ia-checkin-changes').hide();
    }
}

function OnLoadCheckInSuccess(sender, args) {
    currentCount = currentCount + 1;
    if (currentCount == TotalCount) {
        OverallComplete('File(s) checked-in successfully.');
    }

}

function OnLoadCheckInFailed(sender, args) {
    if (args.get_message() != 'Input string was not in a correct format.') {

        OverAllFailureMessage += 'Request failed. ' + args.get_message() + "\n";
    }
    currentCount = currentCount + 1;
    if (currentCount == TotalCount) {
        OverallComplete('File(s) checked-in successfully.');
    }

}



function fileCheckinMajorOption() {
    $.magnificPopup.close();
    var sList = '';

    $('.selection:visible').each(function () {
        if (this.checked) {
            sList += removehiddenvalue($(this).parent().parent().find('.itemhidden').html()) + ';';
        }
    });
    var listname = $('#listDisplayName').val();
    TotalCount = 1;
    currentCount = 0;
    OverAllFailureMessage = '';
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


            if ($(".checkInMajorFileOption:checked").length > 0) {
                {
                    var Comment = '';
                    if ($(".checkInMajorFileOption:checked").length > 0)
                        Comment = $('.checkInMajorCommentOption').val();

                    if ($(".checkInMajorFileOption:checked").length > 0) {
                        file.checkIn(Comment, 1);
                        if ($(".fileCheckinYes:checked").length > 0)
                            file.checkOut();
                    }
                }
                //if ($(".checkInCheckOutFileOption:checked").length > 0)
                //    file.checkOut();
            }
            else {
                file.checkIn('Checked in using ECMA script', 1);
            }
            clientContext.load(file);
        }
        clientContext.executeQueryAsync(Function.createDelegate(this, this.OnLoadCheckInSuccess), Function.createDelegate(this, this.OnLoadCheckInFailed));
    }
}

function OnLoadCheckInSuccess(sender, args) {
    currentCount = currentCount + 1;
    if (currentCount == TotalCount) {
        OverallComplete('File(s) checked-in successfully.');
    }

}

function OnLoadCheckInFailed(sender, args) {
    if (args.get_message() != 'Input string was not in a correct format.') {

        OverAllFailureMessage += 'Request failed. ' + args.get_message() + "\n";
    }
    currentCount = currentCount + 1;
    if (currentCount == TotalCount) {
        OverallComplete('File(s) checked-in successfully.');
    }

}




function fileDisCheckOut() {
    OverAllFailureMessage = "";
    var dictionary = {};

    $('.selection:visible').each(function () {
        if (this.checked) {
            var listName = $(this).parent().parent().find('.itemListDisplayName').justtext().split(';')[0];
            var itemId = $(this).parent().parent().find('.itemhidden').justtext();
            dictionary = insertIntoDic(dictionary, listName, itemId);
        }
    });

    TotalCount = Object.keys(dictionary).length;
    currentCount = 0;
    for (var key in dictionary) {

        if (dictionary.hasOwnProperty(key)) {

            var clientContext = SP.ClientContext.get_current();
            if (clientContext != undefined && clientContext != null) {
                var webSite = clientContext.get_web();
                this.list = webSite.get_lists().getByTitle(key);
                var lstID = '';
                var i;
                var myarray = dictionary[key];
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
    }
}

function OnLoadDiscCheckOutSuccess(sender, args) {
    currentCount = currentCount + 1;
    if (currentCount == TotalCount) {
        OverallComplete('File(s) discard checkout successfully');
    }
}

function OnLoadDisCheckOutFailed(sender, args) {


    if (args.get_message() != 'Input string was not in a correct format.') {

        OverAllFailureMessage += 'Request failed. ' + args.get_message() + "\n";
    }
    currentCount = currentCount + 1;
    if (currentCount == TotalCount) {
        OverallComplete('File(s) discard checkout successfully');
    }


}


function fileDownload() {

    var sDocument = '';

    $('.selection:visible').each(function () {
        if (this.checked) {

            sDocument = $('#webUrl').val() + '/_layouts/15/download.aspx?SourceUrl=' + $('#webUrl').val() + '/' + removehiddenvalue($(this).parent().parent().find('.itemhiddenPath').html());
        }
    });

    window.open(sDocument, '_self');
    unCheckSelections();
    UndoSelection();
}

function fileOpen(event) {

    var thisitem;

    $('.selection:visible').each(function () {
        if (this.checked) {

            thisitem = $(this).parent().parent().find('.docOpen');
        }
    });
    setCookie();
    window.location.href = thisitem.attr('href');



}
function fileDelete() {
    $.magnificPopup.open({
        items: {
            src: '.ia-library-fileDelete',

            type: 'inline',

        },
        closeOnBgClick: false,
        closeBtnInside: true
    });

}

function CancelDelete(event) {
    event.preventDefault();
    $.magnificPopup.close();
}

function ConfirmDelete(event) {
    event.preventDefault();
    $.magnificPopup.close();
    OverAllFailureMessage = "";
    var dictionary = {};

    $('.selection:visible').each(function () {
        if (this.checked) {
            var listName = $(this).parent().parent().find('.itemListDisplayName').justtext().split(';')[0];
            var itemId = $(this).parent().parent().find('.itemhidden').justtext();
            dictionary = insertIntoDic(dictionary, listName, itemId);
        }
    });

    TotalCount = Object.keys(dictionary).length;
    currentCount = 0;
    for (var key in dictionary) {

        if (dictionary.hasOwnProperty(key)) {
            var clientContext = SP.ClientContext.get_current();
            var oList = clientContext.get_web().get_lists().getByTitle(key);

            var lstID = '';
            var i, oListItem;
            var myarray = dictionary[key];

            for (var i = 0; i < myarray.length; i++) {
                lstID = myarray[i];
                oListItem = oList.getItemById(lstID);
                oListItem.deleteObject();
            }

            clientContext.executeQueryAsync(Function.createDelegate(this, this.onQuerySucceededDelete), Function.createDelegate(this, this.onQueryFailedDelete));
        }
    }

}

function onQuerySucceededDelete() {

    currentCount = currentCount + 1;
    if (currentCount == TotalCount) {
        OverallComplete('Item(s) deleted successfully.');
    }

}

function onQueryFailedDelete(sender, args) {
    if (args.get_message() != 'Input string was not in a correct format.') {

        OverAllFailureMessage += 'Request failed. ' + args.get_message() + "\n";
    }
    currentCount = currentCount + 1;
    if (currentCount == TotalCount) {
        OverallComplete('Item(s) deleted successfully.');
    }

}

function fileViewProperties() {
    var sList = '';
    var listname = ''; //$('#listName').val();
    $('.selection:visible').each(function () {
        if (this.checked) {
            sList += $(this).parent().parent().find('.itemhidden').justtext();

            listname = $(this).parent().parent().find('.itemhiddenPath').justtext().split('/')[0];//$(this).parent().parent().find('.itemListDisplayName').justtext().split(';')[1];
        }
    });


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
    SP.SOD.execute('sp.ui.dialog.js', 'SP.UI.ModalDialog.showModalDialog', options);
    unCheckSelections();
}

function fileEditProperties() {

    var checkoutImg = '.ia-doc-checkedOut';

    var itemId = '';
    var listname = ''; //$('#listName').val();
    var forceCheckout = '';
    $('.selection:visible').each(function () {
        if (this.checked) {
            itemId = $(this).parent().parent().find('.itemhidden').justtext();
            listname = $(this).parent().parent().find('.itemhiddenPath').justtext().split('/')[0];//$(this).parent().parent().find('.itemListDisplayName').justtext().split(';')[1];
            forceCheckout = $(this).parent().parent().find('.itemForceCheckOut').justtext();
        }
    });


    var baseUrl = $('#webUrl').val();
    //var forceCheckout = $('#lstForceCheckout').val();



    if (forceCheckout == "true" && $('.selection:checked').parent().parent().find(checkoutImg).length == 0) {
        if (confirm("You must check out this item before making changes.  Do you want to check out this item now?")) {
            fileEditCheckOut();
        }
    }
    else {
        var options = {
            url: baseUrl + '/' + listname + '/Forms/EditForm.aspx?ID=' + itemId,
            tite: 'Edit Properties',
            allowMaximize: false,
            showClose: false,
            width: 800,
            height: 600,
            showClose: true,
            dialogReturnValueCallback: onPopUpCloseCallBackWithData
        };
        SP.SOD.execute('sp.ui.dialog.js', 'SP.UI.ModalDialog.showModalDialog', options);
    }
}

function fileEditCheckOut() {

    var itemId = '';
    var listname = ''; //$('#listName').val();

    $('.selection:visible').each(function () {
        if (this.checked) {
            itemId = $(this).parent().parent().find('.itemhidden').justtext();
            listname = $(this).parent().parent().find('.itemListDisplayName').justtext().split(';')[1];

        }
    });
    var clientContext = SP.ClientContext.get_current();
    if (clientContext != undefined && clientContext != null) {
        var webSite = clientContext.get_web();
        this.list = webSite.get_lists().getByTitle(listname);
        var lstID = '';
        var i;
        //var myarray = sList.split(';');
        var item, file;

        // for (var i = 0; i < myarray.length; i++) {
        //lstID = myarray[i];
        item = this.list.getItemById(itemId);
        file = item.get_file();
        file.checkOut();
        clientContext.load(file)
        //}
        clientContext.executeQueryAsync(Function.createDelegate(this, this.OnLoadSuccessFileEditCheckout), Function.createDelegate(this, this.OnLoadFailedFileEditCheckout));

    }
}

function OnLoadSuccessFileEditCheckout(sender, args) {
    var itemId = '';
    var listname = ''; //$('#listName').val();

    $('.selection:visible').each(function () {
        if (this.checked) {
            itemId = $(this).parent().parent().find('.itemhidden').justtext();
            listname = $(this).parent().parent().find('.itemhiddenPath').justtext().split('/')[0];//$(this).parent().parent().find('.itemListDisplayName').justtext().split(';')[1];

        }
    });
    var baseUrl = $('#webUrl').val();


    var options = {
        url: baseUrl + '/' + listname + '/Forms/EditForm.aspx?ID=' + itemId,
        tite: 'Edit Properties',
        allowMaximize: false,
        showClose: false,
        width: 800,
        height: 600,
        showClose: true,
        dialogReturnValueCallback: onPopUpCloseCallBackWithData
    };
    SP.SOD.execute('sp.ui.dialog.js', 'SP.UI.ModalDialog.showModalDialog', options);


}

function OnLoadFailedFileEditCheckout(sender, args) {

    if (args.get_message() == 'Input string was not in a correct format.') {
        var itemId = '';
        var listname = ''; //$('#listName').val();

        $('.selection:visible').each(function () {
            if (this.checked) {
                itemId = $(this).parent().parent().find('.itemhidden').justtext();
                listname = $(this).parent().parent().find('.itemhiddenPath').justtext().split('/')[0];//$(this).parent().parent().find('.itemListDisplayName').justtext().split(';')[1];

            }
        });
        var baseUrl = $('#webUrl').val();


        var options = {
            url: baseUrl + '/' + listname + '/Forms/EditForm.aspx?ID=' + itemId,
            tite: 'Edit Properties',
            allowMaximize: false,
            showClose: false,
            width: 800,
            height: 600,
            showClose: true,
            dialogReturnValueCallback: onPopUpCloseCallBackWithData
        };
        SP.SOD.execute('sp.ui.dialog.js', 'SP.UI.ModalDialog.showModalDialog', options);
    }
    else {
        alert('Checkout failed. ' + args.get_message());
        unCheckSelections();
    }


}

function fileComplianceDetails() {
    var baseUrl = $('#webUrl').val();
    var itemId = '';
    var listId = '';// $('#listGuidId').val();
    $('.selection:visible').each(function () {
        if (this.checked) {
            itemId += $(this).parent().parent().find('.itemhidden').justtext();
            listId = $(this).parent().parent().find('.itemListId').justtext();
        }
    });
    var options = {
        url: baseUrl + '/_layouts/15/itemexpiration.aspx?ID=' + itemId + '&List={' + listId + '}',
        tite: 'Compliance Details',
        allowMaximize: false,
        showClose: false,
        width: 800,
        height: 600,
        showClose: true,
    };
    SP.SOD.execute('sp.ui.dialog.js', 'SP.UI.ModalDialog.showModalDialog', options);

}
function removehiddenvalue(element) {
    var split = element.split("</b>");
    return split.length > 1 ? split[1] : split[0];
}

function fileShare() {

    var baseUrl = $('#webUrl').val();
    var itemId = '';
    var listId = '';// $('#listGuidId').val();
    $('.selection:visible').each(function () {
        if (this.checked) {
            itemId = $(this).parent().parent().find('.itemhidden').justtext();
            listId = $(this).parent().parent().find('.itemListId').justtext();
        }
    });



    var options = {
        url: baseUrl + '/_layouts/15/aclinv.aspx?forSharing=1&List=' + listId + '&obj={' + listId + '},' + itemId + ',DOCUMENT&IsDlg=1',
        tite: 'Share File',
        allowMaximize: false,

        width: 800,
        height: 600,
        showClose: true,
        dialogReturnValueCallback: unCheckSelections
    };
    SP.SOD.execute('sp.ui.dialog.js', 'SP.UI.ModalDialog.showModalDialog', options);
    $.magnificPopup.close();
    //CallShare();
    // unCheckSelections();
}

function fileSharePreview() {

    var baseUrl = $('#webUrl').val();
    var itemId = '';
    var listId = '';// $('#listGuidId').val();
    var selectedid;
    $('.selection:visible').each(function () {
        if (this.checked) {
            itemId = $(this).parent().parent().find('.itemhidden').justtext();
            listId = $(this).parent().parent().find('.itemListId').justtext();
            selectedid = $(this).attr('name');
        }
    });



    var options = {
        url: baseUrl + '/_layouts/15/aclinv.aspx?forSharing=1&List=' + listId + '&obj={' + listId + '},' + itemId + ',DOCUMENT&IsDlg=1',
        tite: 'Share File',
        allowMaximize: false,
        width: 800,
        height: 600,
        showClose: true,
        //args:{ arg1: selectedid },
        dialogReturnValueCallback:
			  function (res, retVal) {
			      var passedArgs = this.get_args();

			      if (res === SP.UI.DialogResult.OK) {
			          setTimeout(function () {
			              $("table#grid tbody").find('tr:visible').each(function () {
			                  if ($(this).find('#chkSelect').attr('name') === selectedid) {
			                      $(this).find('#previewIt').click();
			                  }
			              });
			          }, 1500);
			      }
			      else if (res === SP.UI.DialogResult.cancel) {
			          $("table#grid tbody").find('tr:visible').each(function () {
			              if ($(this).find('#chkSelect').attr('name') === selectedid) {
			                  $(this).find('#previewIt').click();
			              }
			          });
			      }
			  }

    };
    SP.SOD.execute('sp.ui.dialog.js', 'SP.UI.ModalDialog.showModalDialog', options);
    $.magnificPopup.close();
}

function fileShareWith() {
    var baseUrl = $('#webUrl').val();
    var itemId = '';
    var listId = '';// $('#listGuidId').val();
    $('.selection:visible').each(function () {
        if (this.checked) {
            itemId = $(this).parent().parent().find('.itemhidden').justtext();
            listId = $(this).parent().parent().find('.itemListId').justtext();
        }
    });

    EnsureScriptFunc('sharing.js', 'DisplaySharedWithDialog', function () { DisplaySharedWithDialog(baseUrl, '{' + listId + '}', itemId); });
    //unCheckSelections();
}

function fileWorkflow() {
    var baseUrl = $('#webUrl').val();
    var itemId = '';
    var listId = '';// $('#listGuidId').val();
    $('.selection:visible').each(function () {
        if (this.checked) {
            itemId = $(this).parent().parent().find('.itemhidden').justtext();
            listId = $(this).parent().parent().find('.itemListId').justtext();
        }
    });



    var options = {
        url: baseUrl + '/_layouts/15/Workflow.aspx?ID=' + itemId + '&List={' + listId + '}',
        tite: 'Workflow',
        allowMaximize: false,
        showClose: false,
        width: 800,
        height: 600,
        showClose: true,
    };
    SP.SOD.execute('sp.ui.dialog.js', 'SP.UI.ModalDialog.showModalDialog', options);
    unCheckSelections();
}



function fileFollow() {

    var itemId = '';
    var docName = '';
    var listId = '';// $('#listGuidId').val();
    $('.selection:visible').each(function () {
        if (this.checked) {
            itemId += $(this).parent().parent().find('.itemhidden').justtext();
            docName += $(this).parent().parent().find('.itemhiddenPath').justtext();
            listId = $(this).parent().parent().find('.itemListId').justtext();
        }
    });


    if (listId != null && itemId != null && docName != null) {
        SP.SOD.executeFunc('followingcommon.js', 'FollowDocumentFromEmail', function () {
            FollowDocumentFromEmail(itemId, listId, docName);
        });
    }
    //unCheckSelections();
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


function copyTextvalue() {
    return document.getElementById('ia-Doc-PrevLink').value;
}


function OpenItemFilePreviewCallOut(sender) {
    var html = $(sender).next().html().split(','), fileurl = "";
    var Images = ("gif,tiff,jpg,jpeg,bmp,png,rif,txt").split(',');
    if (jQuery.inArray(html[4], Images) != -1)
        fileurl = "'" + html[1] + "'";
    else
        fileurl = '"' + $('#webServerUrl').val() + '/_layouts/15/WopiFrame.aspx?sourcedoc=' + html[1] + '&amp;action=interactivepreview&amp;wdSmallView=1"';
    var Modified = html[3];
    var Editor = html[2];
    var FileType = ("one,onetoc2,mpp,vsdx,zip").split(',');
    var shareTileName = html[0];
    var baseUrl = $('#webUrl').val();
    var listname = $(sender).parent().parent().parent().find('.itemListDisplayName').justtext().split(';')[1];

    var docLinkUrl = baseUrl.replace($('#webServerUrl').val(), "") + html[1];

    //RemoveAllItemCallouts();

    var openNewWindow = true; //set this to false to open in current window
    if (jQuery.inArray(html[4], FileType) <= -1) {

        var callOutContenBodySection = getCallOutFilePreviewBodyContent2(fileurl, 500, 600, Editor, Modified, shareTileName, docLinkUrl);
        $('#document-preview-1 div.ia-preview-document').html(callOutContenBodySection);

    }
    else
        $('#document-preview-1 div.ia-preview-document').html("");

    $('#document-preview-1 h1.ia-doc-title').text(html[0]);
    $('#document-preview-1 p.ia-doc-modified').html("Changed by " + Editor + " on " + Modified);
    $('#document-preview-1 p.ia-doc-shared').html("Shared with <a href='#' onclick='fileShareWith();' Title=" + shareTileName + ">" + "lots of people </a>");
    $('#document-preview-1 input#ia-Doc-PrevLink').text(fileurl);
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
function CopyToClipboard() {
    if (!document.all) alert("Supports only in IE");
    else {
        var copyText = $("#ia-Doc-PrevLink").val();
        window.clipboardData.setData("Text", copyText);
    }
}



