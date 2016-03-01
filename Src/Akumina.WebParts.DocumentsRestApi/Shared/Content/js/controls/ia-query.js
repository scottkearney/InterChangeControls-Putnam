//Declare and initiate static variables
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
var listname = "";
var baseUrl = "";
var JsMode = "";
var sortAssignedColumn = ""; var sortTypeClassName = "";
var tabreset = true;

//Get and set the browser back button URL from cache
$(window).load(function () {
    var cookieName = $('#akuminaCookieName').val();

    var cookieValue = getCookie(cookieName);
    if (cookieValue != "") {
        showloader();
        setValuesToFields(cookieName, cookieValue);
        deleteCookie(cookieName);
    }

});



//Get only the text value in an element
jQuery.fn.justtext = function () {

    return $(this).clone()
            .children()
            .remove()
            .end()
            .text();

};

//Bind header column names from the resource file
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

//Call a function from "ia-common.js"
function BindGridItems() {
    showloader();
    styleUrl = $("#styleUrl").val();
    str = $("#docIconUrlMap").html().replace(/\s+/g, '');
    mapIconUrls = JSON.parse(str);
    listname = $('#listDisplayName').val();
    baseUrl = $('#webUrl').val();
    JsMode = $('#GridJsMode').val();
    tabreset = true;
    getListItems();

}

function uploadFilesHighlight() {
    var filesuploaded = [];
    var fupload = $('#hdnUploadedFiles').val().trim().toLowerCase();
    if (fupload != "" && fupload.length > 0) {
        filesuploaded = fupload.split(',');

        if ($("table#ogrid tbody").find('tr').length > 0) {
            $("table#ogrid tbody").find('tr').each(function (index) {
                var fileType = $.trim($(this).find('.itemhiddenFileType').justtext());



                if (fupload != "") {
                    if (fileType != "") {
                        var filecheck = $(this).find('.ia-doclist-name').find('.disp-File-Name').justtext() + "." + fileType;

                        if (filesuploaded.indexOf(filecheck.toLowerCase()) != -1) {

                            $(this).attr('class', 'ia-doclist-row-successful');
                            var that = $(this);
                            setTimeout(function () {
                                that.attr('class', '');
                                if ($('#hdnUploadedFiles').val() != "")
                                    $('#hdnUploadedFiles').val("");
                            }, 10000);
                        }
                    }
                }




            });
        }
    }
}

//Get the list of items for the cliked folder

function getcurrentfolder(element) {
    window.location.href = "#";
    //ResetRefiner();
    var folder = $(element).attr('url-data');

    if ($('#folderBox'))
        $('#folderBox').attr('value', folder);
    if ($('#txtSearch'))
        $('#txtSearch').val('');
    if ($('#lblID'))
        $('#lblID').attr('value', '0');
    if ($('#currentFolderPath'))
        $('#currentFolderPath').attr('value', folder);
    getListItems();
    tabreset = true;
}

//Getting list items based on ODATA Query
function getListItems(url, listname, query, sortField, sortOrder) {

    showloader();
    listname = $('#listDisplayName').val();
    var webdocLibrary = $("#webdocLibrary").val().split(',');
    var selecteddocLibrary = $("#selecteddocLibrary").val().split(',');
    //FILTERS
    var fileTypes = "";//"pdf".split(',');//[''];
    var categories = "";//"Category1,Category2".split(',');
    var users = "";// "testuser2,testuser1,irshad,System Account".split(','); //['testuser2', 'testuser1', 'irshad', 'System Account'];

    var currentTab = $("#lblID") != null ? $("#lblID").val() : "0";
    currentTab = currentTab != "" ? currentTab : "0";

    var folderPath = ''; folderPath = $("#webServerUrl").val() + "/" + $("#currentFolderPath").val();

    var searchText = ''; searchText = ($("#txtSearch").length > 0 && $("#txtSearch").val() != "" && $("#txtSearch").val().trim().length > 2) ? $("#txtSearch").val() : "";

    if (currentTab == "3")
        rowLimit = parseInt($('.recentFiles').attr("file-count"));
    else
        rowLimit = ($("#hdnRowlimit").length > 0 && $("#hdnRowlimit").val() != "") ? parseInt($("#hdnRowlimit").val()) : 100;

    //SortField is null, default sort is FileName
    if (sortField == null) {
        sortField = "FileLeafRef"; sortOrder = "True";
        sortAssignedColumn = "FileLeafRef"; sortTypeClassName = "headerSortDown";
    }
    var filters = GetFilters();
    //Execute caml query or else execute search query
    if (JsMode == 'caml') {
        //Get list of items by caml Query execution(Rest Api). The function is defined in the ia-caml-query.js. SortOrder Default Ascending. For Decending, set Ascending = "False"
        var url = styleUrl + "js/controls/ia-caml-query.js";
        $.getScript(url, function () {


            var search = new Akumina_Interaction_Search(listname, folderPath, searchText, currentTab, rowLimit, sortField, sortOrder, filters);

            search.Transform();




        });
    }
    else {
        var url = styleUrl + "js/controls/ia-kql.js";
        $.getScript(url, function () {
            var search = new Akumina_Interaction_Search(listname, folderPath, searchText, currentTab, rowLimit, sortField, sortOrder, webdocLibrary, selecteddocLibrary, fileTypes, categories, users);
            search.Transform();

        });
    }
}


function BindRefinerHtml(RefinerToGrid) {
    var modfiedUsers = [];
    for (var i in RefinerToGrid) {
        var result_value = RefinerToGrid[i];
        if (i == "EditorName") {


            if ($("div.otherfield").eq(0).find("option:selected:selected").length > 0) {
                modfiedUsers = $('#fileModifiedUser').chosen().val();
            }

            var modifiedByHtml = buildRefinersModifiedBy(result_value, modfiedUsers);
            $("div.otherfield").eq(0).find(".ia-filter-body").html(modifiedByHtml);

            $('#fileModifiedUser').chosen({ width: '100%' }).change(function (e, params) {
                updateGridData();
            });

        }
        else if (i == "FileType") {
            var fileTypes = [];
            for (var i = 0; i < result_value.length; i++) {
                var fileType = result_value[i];
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
            }

            var fileTypeHtml = buildRefiners(fileTypes);
            $("div.otherfield").eq(1).find(".ia-filter-body").html(fileTypeHtml);
            if (result_value.length == 0) {
                $("div.otherfield").eq(1).hide();
            }
            else
                $("div.otherfield").eq(1).show();
        }


    }
}

function updateGridData() {

    getListItems();


}

function GetFilters() {
    var date = []; var modified = []; var filetype = []; var categoryMeta = []; var startdate = ""; var enddate = "";
    var qy = formrefinerQuery();
    var filters = {};
    if (qy != "") {

        var querys = qy.split('$');
        for (var i = 0; i < querys.length; i++) {
            var category = querys[i].split('@');
            if (category[0] == "Date") {
                date = category[1].split('|');
                startdate = date[0];
                enddate = date[1];
                if (startdate != "") {
                    var date = new Date(startdate);
                    startdate=(date.getMonth() + 1) + '/' + date.getDate() + '/' + date.getFullYear();
                    filters = insertIntoDic(filters, "startdate", startdate);
                }
                if (enddate != "") {
                    var date = new Date(enddate);
                    enddate = (date.getMonth() + 1) + '/' + date.getDate() + '/' + date.getFullYear()
                    filters = insertIntoDic(filters, "enddate", enddate);
                }
            }

            else if (category[0] == "Modified By") {
                modified = category[1].split('|');
                filters = insertIntoDic(filters, "modifiedby", modified);


            }
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
                filters = insertIntoDic(filters, "filetype", filetype);
            }
            else if (category[0] == "Category") {


                categoryMeta = category[1].trim().replace(/[^A-Z0-9,>]/ig, "_").split(',');
                filters = insertIntoDic(filters, "category", categoryMeta);

            }

        }
    }
    return filters;
}

function FormKQL(ListId, RowLimit) {
    var currentFolderPath = $("#currentFolderPath").val();

    var currentURL = _spPageContextInfo.webAbsoluteUrl;
    var parentLink = '';
    var s = true;
    if (currentFolderPath.indexOf("/") === -1)
        parentLink = "'ParentLink:\"" + currentURL + "/" + currentFolderPath + "/Forms/AllItems.aspx\"'";
    else
        //parentLink = '"ParentLink:ends-with(\"/" + currentFolderPath + "\")"';
        parentLink = "'ParentLink:ends-with(\"/" + currentFolderPath + "\")'";

    var searchUrl = currentURL + "/_api/search/query?querytext='ListId:" + ListId + "'&RowLimit=" + RowLimit + "&selectproperties='Id,Title,Author,LastModifiedTime,SecondaryFileExtension,IsContainer,owstaxIdCategory,AttachmentURI,deeplinks,DefaultEncodingURL,ExternalMediaURL,HierarchyUrl,OrgParentUrls,OrgUrls,OriginalPath,ParentLink,Path,PictureThumbnailURL,PictureURL,PublishingImage,recommendedfor,ServerRedirectedEmbedURL,ServerRedirectedPreviewURL,ServerRedirectedURL,SiteLogo,SitePath,SPSiteURL,UserEncodingURL'&refinementfilters=" + parentLink + "";

    return searchUrl;
}
/*tab starts*/
function tabHighlight(tabid, element) {


    var tabquery = $(element).attr("tab_val");
    window.location.href = "#";
    showloader();
    $("#lblID").attr("value", tabid);
    if (tabid == "0") {

        getListItems();
    }
    else if (tabid == "1") {

        getListItems();
    }
    else if (tabid == "3") {
        sortTypeClassName = "headerSortUp";
        sortAssignedColumn = "Modified";

        getListItems("", "", "", sortAssignedColumn, "False");
    }
    else {
        getListItems();
    }

    tabmenuhighlight();


}



//Getting the result for search or page load
function ProcessQueryDataToGrid(listItems) {
    listItemsJson = listItems;
    BindMustacheHtml(listItemsJson);
}



//Getting the result for the current folder selection
function getCurrentSubFolder(element) {
    showloader();
    var currentfolderUrl = $.trim(decodeURIComponent($(element).attr('url-data')));
    if ($('#txtSearch'))
        $('#txtSearch').val('');
    if ($('#lblID'))
        $('#lblID').attr('value', '0');
    if ($('#currentFolderPath'))
        $('#currentFolderPath').attr('value', currentfolderUrl.replace(baseUrl + "/", ""));

    if ($('#folderBox'))
        $('#folderBox').attr('value', currentfolderUrl.replace(baseUrl + "/", ""));
    tabreset = true;
    getListItems();
}

//Updating the bread crumb based on the current folder\libraries selection
function updateBreadCrumb() {
    var html = '';
    if ($("#currentFolderPath").prop("value")) {
        html = GetBreadcrumb($("#currentFolderPath"));
    } else
        html = GetBreadcrumb($("#listDisplayName"));
    $('.subsite-header > .subsite-page').html(html);
}

//Setting the bread crumb values based on the selection
function GetBreadcrumb(currentFolderPathElement) {
    var folderPath = []; var previousFolderName = ""; var currentFolderName = "", previousFolderPath = ""; var breadCrumbHtml = "";
    if ($(currentFolderPathElement) != null && $(currentFolderPathElement).val().trim() != "") {
        folderPath = $(currentFolderPathElement).val().split('/');
    }
    if (folderPath.length == 1) {
        currentFolderName = folderPath[0];
        breadCrumbHtml = "<a title='Document Libraries' href='#'>Document Libraries</a> > " + folderPath[0];
    }
    else {

        previousFolderName = folderPath[folderPath.length - 2];
        currentFolderName = folderPath[folderPath.length - 1];
        if (folderPath.length == 2)
            previousFolderName = folderPath[0];
        var getFolderPath = folderPath.slice(0, folderPath.length - 1);
        previousFolderPath = getFolderPath.join('/');
        breadCrumbHtml = "<a title=\"" + previousFolderName + "\" href=\"#\" url-data=\"" + previousFolderPath + "\" onclick=\"getCurrentSubFolder(this);\">" + previousFolderName + "</a> > " + currentFolderName;
    }
    if ($('.lblBreadCrum')) {
        $('.lblBreadCrum').html(currentFolderName);
    }
    return breadCrumbHtml;
}

//Binding the values to Grid using Mustache template "DMSTemplate.html"
function BindMustacheHtml(docItems) {

    var template = document.getElementById('dms_template').innerHTML;
    var output = Mustache.render(template, docItems);
    $("#mainGrid").html(output);
    //BindColumnNames();
    AfterGrid();
    BindSortingOrder();
    updateBreadCrumb();
    uploadFilesHighlight();
    UpdateTabCount(docItems);
}



//Update the item count in the tab
function UpdateTabCount(docItems) {
    //Check the tab exists in the page
    if ($('#lblID')) {
        var tab = $('#lblID').length > 0 && $('#lblID').val() != "" ? $('#lblID').val() : "0";
        if (tabreset) {
            var totalcount = 0; var myfileCount = 0; var recentcount = 0; var popularcount = 0; var userName = "";
            userName = $('.myFiles').length > 0 ? $('.myFiles').attr("user") : "";

            var onlyFiles = docItems.filter(function (el) {
                return el.FileType != null;
            });

            totalcount = onlyFiles.length;
            popularcount = (totalcount >= parseInt($(".popularFiles").attr("file-count")) ? parseInt($(".popularFiles").attr("file-count")) : totalcount);
            recentcount = (totalcount >= parseInt($(".recentFiles").attr("file-count")) ? parseInt($(".recentFiles").attr("file-count")) : totalcount);

            var mydcouments = onlyFiles.filter(function (el) {
                return el.EditorName.toLowerCase() == userName.toLowerCase();
            });

            myfileCount = mydcouments.length;

            if ($('.allfilecount'))
                $('.allfilecount').html(totalcount);
            if ($('.myfilecount'))
                $('.myfilecount').html(myfileCount);

            if ($('.recentfilecount'))
                $('.recentfilecount').html(recentcount);
            if ($('.popularfilecount'))
                $('.popularfilecount').html(popularcount);

            tabmenuhighlight();
        }
        tabreset = false;
    }
}

function pageRefresh() {
    showloader();
    tabreset = true;
    getListItems(baseUrl, listname, queryFields);
}
function pageRefreshCreate() {
    showloader();
    document.getElementById('viewSateReset').click();
}

//Bind the sort order the sorted column
function BindSortingOrder() {


    if ($("table#ogrid thead tr [columnName='" + sortAssignedColumn + "']").length > 0)
        $("table#ogrid thead tr [columnName='" + sortAssignedColumn + "']").addClass(sortTypeClassName);

    sortAssignedColumn = ""; sortTypeClassName = "";
}


//Get and set the file icon for all the grid items
function GetFileIconUrl(filetype) {
    var imagePath = styleUrl + "images/icons/";
    if (typeof mapIconUrls[filetype] == "undefined")
        return imagePath + mapIconUrls["default"];
    else
        return imagePath + mapIconUrls[filetype];

}

//Show\hide the grid search list items
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

//Expand\collapse the folder tree view
function foldershow(content, folderPath) {

    var folder = $(content).find('.ia-folder-tree').parent().html();
    $('.ia-folder-tree').parent().html(folder);
    if (folderPath != "") {
        $('#folderBox').attr("value", folderPath); expandTreeView();
    } else
        expandTreeView();
}

//Expand folder tree view and highlight the item
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

//Bind the grid from HTML response
function BindHtml(response, fdreset, tabreset, qyreset, searchtext, recursive, cookieName, redirect) {
    var start = response.indexOf("<body>");
    var end = response.indexOf("</body>");
    var content = response.substring(start - 1, end - 1);
    var folderRedirect = ["", ""];
    if (redirect != "")
        folderRedirect = redirect.split(";");

    if (fdreset) foldershow(content, folderRedirect[0]);
    getListItems(baseUrl, listname, queryFields);
}


//Set the values back to grid on back button click event
function setValuesToFields(cookieName, cookieValue) {
    var arr = cookieValue.split('!');
    var searchtext = "";
    var recursive = arr[1];
    var fd = arr[2];
    var qy = arr[3];
    var tab = arr[4];
    var tabID = arr[5];
    var fdreset = true, tabreset = true, qyreset = true;
    var urlQuery = "?fd=" + fd + "&recursive=" + recursive;
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


//Set the cookie values
function setCookie() {
    var cvalue = GetUrl();
    var cookieName = $('#akuminaCookieName').val();
    var d = new Date();
    d.setTime(d.getTime() + (1 * 24 * 60 * 60 * 1000));
    var expires = "expires=" + d.toGMTString();
    document.cookie = cookieName + "=" + cvalue + "; " + expires;
}

//Get the cookie values
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

//Delete the cookie values
function deleteCookie(c_name) {
    document.cookie = encodeURIComponent(c_name) + "=deleted; expires=" + new Date(0).toUTCString();
}

//Page load event for JS
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
}

//Sets the header column css and sort column order
function AfterGrid() {

    var node; headerhtml = "";
    if ($("table#ogrid > tbody").find('tr').length > 0) {
        $('#noresults').hide();

        $("table#ogrid thead tr th").each(function () {

            if (!$(this).hasClass("ia-doclib-header-checkbox")) {
                $(this).addClass("header");
            }
        });
        $("table#ogrid thead tr th").click(function () {

            if (!$(this).hasClass("ia-doclib-header-checkbox")) {

                var sortorder = ""; var sortcolumn = ""; var type = "string"; sortTypeClassName = "";

                if ($(this).hasClass("headerSortUp")) { sortorder = "True" }
                else if ($(this).hasClass("headerSortDown"))
                { sortorder = "False" }
                else
                { sortorder = "False" }
                if (sortorder == "True")
                    sortTypeClassName = "headerSortDown";
                else
                    sortTypeClassName = "headerSortUp";

                sortAssignedColumn = $(this).attr("columnName");

                getListItems("", "", "", sortAssignedColumn, sortorder)


            }
        });

    }
    else
        $('#noresults').show();

    setTimeout(function () { hideloader(); }, 100);
}




