/* File Name: ia-caml-Query.js
It */


//Common name with Response key by ':' seperator. Ex: Response key for file type "File_x0020_Type". Our key is "FileType". Its formatted as "FileType:File_x0020_Type"
//All key values are defined with comma seperator


var Grid_Fields = ("Id:Id,FileCheckOut:CheckoutUserId,FileType:File_x0020_Type,ItemFullName:FileLeafRef,ProgID:Folder/ProgID,Modified:Modified,EditorName:FieldValuesAsText/Editor,EditorId:EditorId,Url:EncodedAbsUrl").split(',');
var Grid_Field_Mapping = {};
var RefinerFields = ("EditorName,FileType").split(',');//Category:File/ListItemAllFields/Category
var Refiner_Field_Mapping = {};
var RefinerToGrid = {};

/*Caml Query Operators*/

//Contains
//BeginsWith
//Eq ‡Equal
//Neq ‡ Not equal
//Gt ‡ Greater than
//Lt ‡ Less than
//Geq ‡ Greater than or equal to
//Leq ‡ Less than or equal to
//Neq ‡ Not equal
//DateRangesOverlap ‡ Compare dates in recurring event to a specified value
//IsNotNull
//IsNull

//It is possible to use AND and OR to define complex comparisons.
var RefinerCamlMapping = ("startdate:AND/Geq/Modified/Datetime,enddate:AND/Leq/Modified/Datetime,category:AND/Leq/Category/TaxonomyFieldType,filepath:AND/Eq/FileDirRef/Text,filetype:AND/Eq/File_x0020_Type/Text,searchtext:AND/Contains/FileLeafRef/Text").split(',');
var Refiner_Filter_Mapping = {};

var filter = "";
if (Akumina_Interaction_Search === undefined) {
    var Akumina_Interaction_Search = function (listname, folderPath, searchText, currentTab, rowLimit, sortField, sortOrder, filters) {
        this.listTitle = listname;
        this.folderPath = folderPath;
        this.searchText = searchText;
        this.currentTab = currentTab;
        this.rowLimit = rowLimit;
        this.sortField = sortField;
        this.sortOrder = sortOrder;
        this.Filters = filters;
        this.GetFieldMapping("refiner");
        this.GetFieldMapping("grid");
        this.GetFilterFieldMapping("filter");
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
        else if (type.toLowerCase() == "grid")
            Grid_Field_Mapping = insertIntoDic(Grid_Field_Mapping, commonKey, searchKey);


    }
}

//Form the dictionary(Map common from the search key )
Akumina_Interaction_Search.prototype.GetFilterFieldMapping = function (type) {

    for (var i = 0; i < RefinerCamlMapping.length; i++) {

        var fieldKeys = RefinerCamlMapping[i].split(':');
        var commonKey = fieldKeys[0];
        var searchKey = fieldKeys[1].split('/');


        Refiner_Filter_Mapping = insertIntoDic(Refiner_Filter_Mapping, commonKey, searchKey);

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

        RefinerToGrid[i] = uniques;
    }

}

// Getting list items based on ODATA Query

Akumina_Interaction_Search.prototype.Transform = function () {

    var query = this.FormQuery(this.searchText, this.folderPath, this.currentTab, this.sortField, this.sortOrder, this.Filters);

    //search the items by rest api
    this.Search(this.listTitle, query, this.rowLimit)
    .done(function (data) {
        var listItems = data.d.results;
        var Items_GridBindData = [];
        for (var i = 0; i < listItems.length; i++) {

            var listItem = GetListItemGridData(listItems[i]);
            Items_GridBindData.push(listItem);
        }
        Refiner(Items_GridBindData);
        BindRefinerHtml(RefinerToGrid);

        ProcessQueryDataToGrid(Items_GridBindData);
    })
    .fail(function (error) {
        var JsonResults = {};
        ConsoleLog(JSON.stringify(error));
        ProcessQueryDataToGrid(JsonResults);
    });
}

Akumina_Interaction_Search.prototype.Search = function (listTitle, queryViewXml, rowLimit) {
    var queryPayload = {
        'query': {
            '__metadata': { 'type': 'SP.CamlQuery' },
            'ViewXml': queryViewXml
        }
    };

    var endpointUrl = _spPageContextInfo.webAbsoluteUrl + "/_api/web/lists/getbytitle('" + listTitle + "')/getitems?$select=FieldValuesAsText,Folder,File,EncodedAbsUrl,EditorId,FileLeafRef,id,Modified,EffectiveBasePermissions,File_x0020_Type,CheckoutUserId,File/ListItemAllFields&$expand=Folder,File,FieldValuesAsText,File/ListItemAllFields&$top=" + rowLimit;//&$top=5&$skip=2"
    return $.ajax({
        type: "POST",
        headers: {
            "accept": "application/json;odata=verbose",
            "content-type": "application/json;odata=verbose",
            "X-RequestDigest": $("#__REQUESTDIGEST").val()
        },
        data: JSON.stringify(queryPayload),
        url: endpointUrl
    });
}

function ConsoleLog(error) {
    console.log(error);
}

//SortOrder Default Ascending. For Decending, set Ascending = "False"
Akumina_Interaction_Search.prototype.FormQuery = function (searchText, folderPath, currentTab, sortField, sortOrder, filters) {


    var orderBy = sortField == null ? '<OrderBy><FieldRef Name="FileLeafRef" Ascending="True"/></OrderBy>' : ('<OrderBy><FieldRef Name="' + sortField + '" Ascending="' + sortOrder + '"/></OrderBy>');
    var query = '';

    if (currentTab == "1") {

        var userName = $('.myFiles').attr("user");
        if (searchText != "")
            query = '<View Scope="RecursiveAll"><Query><Where><And><And><And><Eq><FieldRef Name="FileDirRef" /><Value Type="Text">' + folderPath + '</Value></Eq><Contains><FieldRef Name="FileLeafRef" /><Value Type="Text">' + searchText + '</Value></Contains></And><IsNotNull><FieldRef Name="File_x0020_Type" /></IsNotNull></And><Eq><FieldRef Name="Editor" /><Value Type="User">' + '' + '  <UserID /></Value></Eq></And></Where>' + orderBy + '</Query></View>';
        else
            query = '<View Scope="RecursiveAll"><Query><Where><And><And><Eq><FieldRef Name="FileDirRef" /><Value Type="Text">' + folderPath + '</Value></Eq><IsNotNull><FieldRef Name="File_x0020_Type" /></IsNotNull></And><Eq><FieldRef Name="Editor" /><Value Type="User">' + '' + '<UserID /></Value></Eq></And></Where>' + orderBy + '</Query></View>';

    }
    else if (currentTab == "3") {

        if (searchText != "")
            query = '<View Scope="RecursiveAll"><Query><Where><And><And><Eq><FieldRef Name="FileDirRef" /><Value Type="Text">' + folderPath + '</Value></Eq><Contains><FieldRef Name="FileLeafRef" /><Value Type="Text">' + searchText + '</Value></Contains></And><IsNotNull><FieldRef Name="File_x0020_Type" /></IsNull></And></Where>' + orderBy + '</Query></View>';
        else
            query = '<View Scope="RecursiveAll"><Query><Where><And><Eq><FieldRef Name="FileDirRef" /><Value Type="Text">' + folderPath + '</Value></Eq><IsNotNull><FieldRef Name="File_x0020_Type" /></IsNotNull></And></Where>' + orderBy + '</Query></View>';


    }
    else if (currentTab == "0") {
        //var currentFilters = filters;
        //currentFilters = insertIntoDic(currentFilters, "filepath", folderPath);
        //if (searchText != "")
        //    currentFilters = insertIntoDic(currentFilters, "searchtext", searchText);
        //query = '<View Scope="RecursiveAll"><Query><Where>' + DynamicCamlQuery(currentFilters) + '</Where>' + orderBy + '</Query></View>';

        //console.log(query);
        //query = '<View Scope="RecursiveAll"><Query><Where><And><Eq><FieldRef Name="FileDirRef" /><Value Type="Text">' + folderPath + '</Value></Eq><Contains><FieldRef Name="FileLeafRef" /><Value Type="Text">' + searchText + '</Value></Contains></And></Where>' + orderBy + '</Query></View>';
        //if (filter != "" && searchText != "") {

        //}
        if (searchText != "")
            query = '<View Scope="RecursiveAll"><Query><Where><And><Eq><FieldRef Name="FileDirRef" /><Value Type="Text">' + folderPath + '</Value></Eq><Contains><FieldRef Name="FileLeafRef" /><Value Type="Text">' + searchText + '</Value></Contains></And></Where>' + orderBy + '</Query></View>';
        else
            query = '<View Scope="RecursiveAll"><Query><Where><Eq><FieldRef Name="FileDirRef" /><Value Type="Text">' + folderPath + '</Value></Eq></Where>' + orderBy + '</Query></View>';
        //if (searchText != "") {
        //    console.log('<View Scope="RecursiveAll"><Query><Where><And><Eq><FieldRef Name="FileDirRef" /><Value Type="Text">' + folderPath + '</Value></Eq><Contains><FieldRef Name="FileLeafRef" /><Value Type="Text">' + searchText + '</Value></Contains></And></Where>' + orderBy + '</Query></View>');
        //    query = '<View Scope="RecursiveAll"><Query><Where><And><Eq><FieldRef Name="FileDirRef" /><Value Type="Text">' + folderPath + '</Value></Eq><Contains><FieldRef Name="FileLeafRef" /><Value Type="Text">' + searchText + '</Value></Contains></And></Where>' + orderBy + '</Query></View>';
        //}
    }
    return query;

}

function GetRefinerResults() {
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

function DynamicCamlQuery(filters) {
    //AND / Eq / FileDirRef / Text
    var query = '', operator = "", comparator = "", columnName = "", columnType = "";
    var i = 0;
    for (var key in filters) {
        var value = filters[key];
        console.log(value);
        var filterKey = Refiner_Filter_Mapping[key];
        console.log(filterKey);
        if (filterKey != null && filterKey.length > 0) {
            operator = filterKey[0];
            comparator = filterKey[1];
            columnName = filterKey[2];
            columnType = filterKey[3];
            var valueFirst = value[0];
            i++;
            if (i > 1)
                query = "<" + operator + ">" + query + QueryBuild(comparator, columnName, columnType, valueFirst) + "</" + operator + ">";
            else
                query = QueryBuild(comparator, columnName, columnType, valueFirst);



        }
    }
    return query;

}

function QueryBuild(comparator, columnName, columnType, value) {
    var query = "";

    var strValue = value.toString();
    query = '<' + comparator + '><FieldRef Name="' + columnName + '" /><Value Type="' + columnType + '">' + strValue + '</Value></' + comparator + '>';

    return query;
}