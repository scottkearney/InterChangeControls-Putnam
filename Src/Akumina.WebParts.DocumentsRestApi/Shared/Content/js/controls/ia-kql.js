/* File Name: ia-KQL.js It */


//Common name with Response key by ':' seperator. Ex: Response key for file type "File_x0020_Type". Our key is "FileType". Its formatted as "FileType:File_x0020_Type"
//All key values are defined with comma seperator
//var Grid_Fields = ("Id:Id,FileCheckOut:CheckoutUserId,FileType:File_x0020_Type,ItemFullName:FileLeafRef,ProgID:Folder/ProgID,Modified:Modified,EditorName:FieldValuesAsText/Editor,EditorId:EditorId,Url:EncodedAbsUrl").split(',');
var Grid_Fields = ("Id:ListItemId,FileCheckOut:CheckoutUserId,FileType:SecondaryFileExtension,ItemFullName:Title,ProgID:Folder/ProgID,Modified:LastModifiedTime,EditorName:Author,EditorId:EditorId,Url:DefaultEncodingURL").split(',');
var Grid_Field_Mapping = {};
var RefinerFields = ("EditorName,FileType").split(',');//Category:File/ListItemAllFields/Category
var Refiner_Field_Mapping = {};
var RefinerToGrid = {};

if (Akumina_Interaction_Search === undefined) {
    var Akumina_Interaction_Search = function (listname, folderPath, searchText, currentTab, rowLimit, sortField, sortOrder, webdocLibrary, selecteddocLibrary, fileTypes, categories, users) {
        this.listTitle = listname;
        this.folderPath = folderPath;
        this.searchText = searchText;
        this.currentTab = currentTab;
        this.rowLimit = rowLimit;
        this.sortField = sortField;
        this.sortOrder = sortOrder;
        this.webdocLibrary = webdocLibrary;
        this.selecteddocLibrary = selecteddocLibrary;
        this.fileTypes = fileTypes;
        this.categories = categories;
        this.users = users;
        this.GetFieldMapping("refiner");
        this.GetFieldMapping("grid");
        /*
        listTitle = listname;
        folderPath = folderPath;
        searchText = searchText;
        currentTab = currentTab;
        rowLimit = rowLimit;
        sortField = sortField;
        sortOrder = sortOrder;
        webdocLibrary = webdocLibrary;
        selecteddocLibrary = selecteddocLibrary;*/
    }
}

Array.prototype.contains = function (v) {
    for (var i = 0; i < this.length; i++) {
        if (this[i] === v) return true;
    }
    return false;
};

Array.prototype.unique = function () {
    var arr = [];
    for (var i = 0; i < this.length; i++) {
        if (!arr.contains(this[i])) {
            arr.push(this[i]);
        }
    }
    return arr;
}

//Form the dictionary(Map common from the search key )
Akumina_Interaction_Search.prototype.GetFieldMapping = function (type) {

    for (var i = 0; i < Grid_Fields.length; i++) {
        var fieldKeys = Grid_Fields[i].split(':');
        var commonKey = fieldKeys[0];
        var searchKey = fieldKeys[1].split('/');
        if (type.toLowerCase() == "refiner")
            Refiner_Field_Mapping = insertIntoDic(Refiner_Field_Mapping, commonKey, searchKey);
        else
            Grid_Field_Mapping = insertIntoDic(Grid_Field_Mapping, commonKey, searchKey);
    }
}

Refiner = function (ListOfItems) {
    if (ListOfItems.length > 0) {
        for (var i = 0; i < ListOfItems.length; i++) {
            for (var j = 0; j < RefinerFields.length; j++) {
                var commonKey = RefinerFields[j];

                var result_value = ListOfItems[i][commonKey];
                if (result_value != null) {

                    RefinerToGrid = insertIntoDic(RefinerToGrid, commonKey, result_value);
                }
            }
        }
    }

    for (var i in RefinerToGrid) {

        var result_value = RefinerToGrid[i];
        var uniques = result_value.unique();
        console.log(RefinerToGrid[i]);
        RefinerToGrid[i] = uniques;
    }

}



// Getting list items based on ODATA Query

Akumina_Interaction_Search.prototype.Transform = function () {

    //var query = FormKQL(documenLibId, RowLimit);
    //this.getListItems_KQL(query, 'GET');

    var query = this.FormQuery(this.searchText, this.folderPath, this.currentTab, this.sortField, this.sortOrder, this.webdocLibrary, this.selecteddocLibrary, this.rowLimit, this.fileTypes, this.categories, this.users);

    //search the items by rest api
    this.Search(this.listTitle, query, this.rowLimit)
    .done(function (data) {
        var listItems = convertRowsToObjects(data.d.query.PrimaryQueryResult.RelevantResults.Table.Rows.results);
        var Items_GridBindData = [];
        var users = [];
        var fileTypes = [];
        //console.log(listItems[0]);
        for (var i = 0; i < listItems.length; i++) {

            var listItem = GetListItemGridData(listItems[i]);
            Items_GridBindData.push(listItem);
        }
        Refiner(Items_GridBindData);
        BindRefinerHtml(RefinerToGrid);
        console.log(RefinerToGrid);
        ProcessQueryDataToGrid(Items_GridBindData);
    })
    .fail(function (error) {
        var JsonResults = {};
        ConsoleLog(JSON.stringify(error));
        ProcessQueryDataToGrid(JsonResults);
    });
}


Akumina_Interaction_Search.prototype.Search = function (listTitle, searchUrl, rowLimit) {
    var res = $.ajax(
    {
        type: "GET",
        headers: {
            "accept": "application/json;odata=verbose",
            "content-type": "application/json;odata=verbose",
            "X-RequestDigest": $("#__REQUESTDIGEST").val()
        },
        // data: JSON.stringify(data.d.query.PrimaryQueryResult.RelevantResults.Table.Rows.results),
        url: searchUrl
    });
    return res;
}

function ConsoleLog(error) {
    console.log(error);
}


//Get the Value for the key from Json result
function GetFieldValueForSearchKey(searchKey, listItem) {
    var currentValue = null;
    currentValue = listItem[searchKey[0]];
    if (currentValue != null && searchKey.length > 1) {
        for (var i = 1; i < searchKey.length; i++) {
            if (currentValue[searchKey[i]] != null) {
                currentValue = currentValue[searchKey[i]];
            }

        }
    }
    return currentValue;

}

//Getting the list data for defined fields
function GetListItemGridData(listItem) {
    var obj = {};
    for (var key in Grid_Field_Mapping) {
        var fieldName = key;
        var searchKey = Grid_Field_Mapping[fieldName];

        var result_value = GetFieldValueForSearchKey(searchKey, listItem);

        if (result_value != null) {
            obj[fieldName] = result_value;
            if (fieldName == "ItemFullName")//Item name of the file or folder.
            {
                obj["NoExtensionName"] = result_value.split(".")[0];// File name without file type.
            }

            else if (fieldName == "Modified") {
                var modifieddateAndDate = result_value.split("T");
                var modifieddate = modifieddateAndDate[0].split("-");
                var modifiedTime = modifieddateAndDate[1].slice(0, -4);
                var modifiedDateFormat = modifieddate[1] + "/" + modifieddate[2] + "/" + modifieddate[0]
                obj["ModifiedDate"] = modifiedDateFormat;// Modified date without time.
                obj["ModifiedTime"] = modifiedTime;// Modified time without date.
            }
            else if (fieldName == "FileType")
                obj["DocIconUrl"] = result_value != null ? GetFileIconUrl(result_value) : "";//Get the document icon url for the file type.
            else if (fieldName == "ProgID") {
                if (result_value != null && result_value == "OneNote.Notebook")
                    obj["FileType"] = "one";
            }
        }
    }
    return obj;

}

Akumina_Interaction_Search.prototype.FormQuery = function (searchText, folderPath, currentTab, sortField, sortOrder, webdocLibrary, selecteddocLibrary, rowLimit, fileTypes, categories, users) {

    var selectProperties = "&selectproperties=\'ListItemId,Title,Author,LastModifiedTime,SecondaryFileExtension,IsContainer,owstaxIdCategory,AttachmentURI,deeplinks,DefaultEncodingURL,ExternalMediaURL,HierarchyUrl,OrgParentUrls,OrgUrls,OriginalPath,ParentLink,Path,PictureThumbnailURL,PictureURL,PublishingImage,recommendedfor,ServerRedirectedEmbedURL,ServerRedirectedPreviewURL,ServerRedirectedURL,SiteLogo,SitePath,SPSiteURL,UserEncodingURL,fileLeafRef\'";
    //rowLimit = 500;

    var IsContainer = ",isContainer:true";

    //FILTERS
    // var fileTypes = "";//"pdf".split(',');//[''];
    // var categories_selected = "";//"Category1,Category2".split(',');
    // var users_Modified = "";// "testuser2,testuser1,irshad,System Account".split(','); //['testuser2', 'testuser1', 'irshad', 'System Account'];

    var fileTypes = fileTypes.split(',');//[''];
    var categories_selected = categories.split(',');
    var users_Modified = users.split(','); //['testuser2', 'testuser1', 'irshad', 'System Account'];


    //CATEGORY - FILTER
    var selectedCategories = 'owstaxIdCategory:*';
    if (categories_selected.length > 1) {
        var scategory = '';
        for (var i = 0; i < categories_selected.length; i++) {
            scategory = categories_selected.join(",");
        }
        selectedCategories = "and(owstaxIdCategory:" + scategory + ")";
    }
    else if (categories_selected.length === 1 && categories_selected[0] != "")
        selectedCategories = "owstaxIdCategory:" + categories_selected[0];

    //FILE TYPE - FILTER
    var selectedFileTypes = "SecondaryFileExtension:*";
    if (fileTypes.length > 1) {
        var ftype = '';
        for (var i = 0; i < fileTypes.length; i++) {
            ftype = fileTypes.join(",");
        }
        selectedFileTypes = "or(SecondaryFileExtension:" + ftype + ")";
    }
    else if (fileTypes.length === 1 && fileTypes[0] != "")
        selectedFileTypes = "SecondaryFileExtension:" + fileTypes[0];


    //AUTHORS - FILTER
    var selectedUsers = "author:*";
    if (users_Modified.length > 1) {
        var authors = '';
        for (var i = 0; i < users_Modified.length; i++) {
            authors = users_Modified.join("\",\"");
        }
        selectedUsers = "or(author:\"" + authors + "\")";
    }
    else if (users_Modified.length === 1 && users_Modified[0] != "")
        selectedUsers = "author:\"" + users_Modified[0] + "\"";


    var docids = [];
    for (var i = 0; i < webdocLibrary.length; i++) {
        var id = webdocLibrary[i].split(':')[1];
        var doc_Name = webdocLibrary[i].split(':')[0];
        //alert("doc: "+doc_Name+"selected: "+selecteddocLibrary[i] + "{avail}:" + $.inArray(doc_Name, selecteddocLibrary));
        if ($.inArray(doc_Name, selecteddocLibrary) > -1) {
            if (id != null && id != "") {
                docids.push(id);
            }
        }
    }


    var searchUrl = '';
    var searchKey = '';
    var searchUser = '';

    var ListId = docids.join("\", \"");
    var ListIdrefiners = "ListId:or(\"" + ListId + "\")";
    var currentFolderPath = $("#currentFolderPath").val();
    var currentURL = _spPageContextInfo.webAbsoluteUrl;
    var parentLink = 'ParentLink:*';

    /*var parentLink = '';

    if (currentFolderPath.indexOf("/") === -1)
        parentLink = "'ParentLink:\"" + currentURL + "/" + currentFolderPath + "/Forms/AllItems.aspx\"'";
    else        
        parentLink = "'ParentLink:ends-with(\"/" + currentFolderPath + "\")'";*/

    searchKey = "\'and(" + ListIdrefiners + "," + parentLink + "," + selectedFileTypes + "," + selectedUsers + "," + selectedCategories + ",ContentClass:STS_ListItem*)\'";

    if (currentTab === "1") {
        searchUser = $('.myFiles').attr("user");
        var txt_search = searchText != "" ? searchText : '*';
        txt_search += " XRANK(cb=1) iscontainer=true";
        searchKey = "\'and(" + ListIdrefiners + "," + parentLink + "," + selectedFileTypes + "," + selectedCategories + ",ContentClass:STS_ListItem*,author:\"" + searchUser + "\")\'";
        searchUrl = currentURL + "/_api/search/query?querytext='" + txt_search + "'&trimduplicates=false&RowLimit=" + rowLimit + selectProperties + "&refinementfilters=" + searchKey + "";
    }
    else if (currentTab === "2") {
        var popular = "&sortlist='Rank:descending'";
        rowLimit = 5;
        searchUser = '*';
        searchUrl = currentURL + "/_api/search/query?querytext='*'&trimduplicates=false&RowLimit=" + rowLimit + selectProperties + popular + "&refinementfilters=" + searchKey + "";
    }
    else if (currentTab === "3") {
        rowLimit = 5;
        searchUser = '*';
        var sortorder = "&sortlist='LastModifiedTime:descending'";
        searchUrl = currentURL + "/_api/search/query?querytext='*'&trimduplicates=false&RowLimit=" + rowLimit + selectProperties + sortorder + "&refinementfilters=" + searchKey + "";

    }
    else {
        var txt_search = searchText != "" ? searchText : '*';
        txt_search += " XRANK(cb=1) iscontainer=true";
        //var sortBy = "&sortlist='title:ascending'";
        searchUrl = currentURL + "/_api/search/query?querytext='" + txt_search + "'&trimduplicates=false&RowLimit=" + rowLimit + selectProperties + "&refinementfilters=" + searchKey + "";
    }

    //  var searchUrl = currentURL + "/_api/search/query?querytext='ListId:" + documenLibId + "'&RowLimit=" + RowLimit + "&selectproperties='Id,Title,Author,LastModifiedTime,SecondaryFileExtension,IsContainer,owstaxIdCategory,AttachmentURI,deeplinks,DefaultEncodingURL,ExternalMediaURL,HierarchyUrl,OrgParentUrls,OrgUrls,OriginalPath,ParentLink,Path,PictureThumbnailURL,PictureURL,PublishingImage,recommendedfor,ServerRedirectedEmbedURL,ServerRedirectedPreviewURL,ServerRedirectedURL,SiteLogo,SitePath,SPSiteURL,UserEncodingURL,fileLeafRef'&refinementfilters=" + parentLink + "";

    return searchUrl;
}

function convertRowsToObjects(itemRows) {
    var items = [];
    //foreach row in the result set 
    for (var i = 0; i < itemRows.length; i++) {
        var row = itemRows[i], item = {};
        //Each cell in the row is a key/value pair, save each one as an object property 
        for (var j = 0; j < row.Cells.results.length; j++) {
            if (row.Cells.results[j].Key === "Author") {
                var author = row.Cells.results[j].Value;
                var Editor = author.indexOf(";") > -1 ? author.substr((author.lastIndexOf(";") + 1), author.length) : author;
                item[row.Cells.results[j].Key] = Editor;
            }
            else
                item[row.Cells.results[j].Key] = row.Cells.results[j].Value;
        }
        items.push(item);
    }
    return items;
}
