function SortByCategory(a, b) {
    var aCat = a.category.toLowerCase();
    var bCat = b.category.toLowerCase();
    return ((aCat < bCat) ? -1 : ((aCat > bCat) ? 1 : 0));
}

var browserSearch = {
    isIe: function () {
        return navigator.appVersion.indexOf("MSIE") != -1;
    },
    navigator: navigator.appVersion,
    getVersion: function () {
        var version = 999; // we assume a sane browser
        if (navigator.appVersion.indexOf("MSIE") != -1)
            // bah, IE again, lets downgrade version number
            version = parseFloat(navigator.appVersion.split("MSIE")[1]);
        return version;
    }
}

function setupTypeahead() {

    $.widget("custom.catcomplete", $.ui.autocomplete, {
        _renderMenu: function (ul, items) {
            var that = this, currentCategory = "";
            var isCurrent = $('.ia-search-combo-site-name').text().indexOf('Current') > -1;

            if (isCurrent) {
                // get list names
                var request = new XMLHttpRequest();
                var currentUrl = window.location.href
                var currentUrlParts = currentUrl.split("/");
                var restUrl = String.format("{1}//{2}{0}/_api/Web/Lists?$select=Title,Id,ContentType'", _spPageContextInfo.webServerRelativeUrl, currentUrlParts[0], currentUrlParts[2]);
                request.open('GET', restUrl, true);
                request.setRequestHeader('Accept', 'application/json; odata=verbose');
                request.onload = function (e) {
                    if (request.readyState === 4) {
                        // Check if the get call was successful
                        if (request.status === 200) {
                            // List retrieve
                            if (browserSearch.isIe() && browserSearch.getVersion() <= 9) {
                                var data =  JSON.parse(request.responseText)
                            }
                            else {
                                var data = JSON.parse(request.response);
                            }


                            for (var i = 0; i < items.length; i++) {
                                var currentCategory = items[i].category;
                                var newCategory = currentCategory != null ? currentCategory.toLowerCase() : "";
                                for (var j = 0; j < data.d.results.length; j++) {
                                    if (currentCategory == '{' + data.d.results[j].Id.toLowerCase() + '}') {
                                        newCategory = data.d.results[j].Title;
                                        break;
                                    }
                                }
                                items[i].category = newCategory;
                            }
                            // sort
                            items.sort(SortByCategory);
                            // display
                            $.each(items, function (index, item) {
                                var li;
                                if (item.category !== currentCategory) {
                                    ul.append("<li class='ui-autocomplete-category'>" + item.category + "</li>");
                                    currentCategory = item.category;
                                }

                                var prevUrl = "";
                                if (item.category == "Documents") {
                                    if (item.value.indexOf('.doc') > -1 || item.value.indexOf('.docx') > -1) {
                                        prevUrl += _spPageContextInfo.webAbsoluteUrl + '/_layouts/15/WopiFrame.aspx?sourcedoc=';
                                    }
                                    else if (item.value.indexOf('.xls') > -1 || item.value.indexOf('.xlsx') > -1) {
                                        prevUrl += _spPageContextInfo.webAbsoluteUrl + '/_layouts/15/xlviewer.aspx?id=';
                                    }
                                }
                                var liValue = prevUrl + item.value;

                                $("<li class='ui-menu-item'>").data("item.autocomplete", item).append("<a href='" + liValue + "'>" + item.label + "</a>").appendTo(ul);
                                /* li = that._renderItemData( ul, item );
								if ( item.category ) {
								  
								  li.append("<a href='" + item.value + "'>" + item.label + "</a>").appendTo(ul);
								} */
                            });
                        } else {
                            // No list found
                        }
                    }
                };
                request.onerror = function (e) {
                    // Catching errors
                };
                request.send(null);

            }
            else {
                items.sort(SortByCategory);

                $.each(items, function (index, item) {
                    var li;
                    if (item.category !== currentCategory) {
                        ul.append("<li class='ui-autocomplete-category'>" + item.category + "</li>");
                        currentCategory = item.category;
                    }
                    $("<li>").data("item.autocomplete", item).append("<a href='" + item.value + "'>" + item.label + "</a>").appendTo(ul);
                    /* li = that._renderItemData( ul, item );
					if ( item.category ) {
					  li.append("<a href='" + item.value + "'>" + item.label + "</a>").appendTo(ul);
					} */
                });
            }
        }
    });
    //var searchResult = "%20-Path:bdc3:// IsContainer<>true -Filename:Thumbnails  -Filename:AllItems  -contentclass:STS_ListItem_links -contentclass:STS_List_links -contentclass:STS_List_850 AND ( (IsDocument:True AND -FileExtension:aspx) OR (-IsDocument:True AND FileExtension:aspx))";
    var searchResult = "%20-Filename:Thumbnails  -Filename:AllItems  -contentclass:STS_ListItem_links -contentclass:STS_List_links  AND ( (IsDocument:True AND -FileExtension:aspx) OR (-IsDocument:True AND FileExtension:aspx AND -Filename:DispForm.aspx))	";

    var qUrl = _spPageContextInfo.webAbsoluteUrl + "/_api/search/query?selectproperties=%27Title,Path,ContentType,SiteTitle,ListID%27&QueryTemplatePropertiesUrl=%27spfile://webroot/queryparametertemplate.xml%27&querytext=";


    $(".ia-search-combo-box").catcomplete({
        delay: 0,
        minLength: 3,
        select: function (event, ui) {
            location.href = ui.item.value;
        },
        source: function (request, response) {

            $.ajax({
                url: qUrl + "%27*" + $(".ia-search-combo-box").val() + "*%20 " + typeaheadCurrentSite() + searchResult + " %27" + '&RowLimit=12',
                dataType: 'json',
                headers: {
                    accept: "application/json;odata=verbose"
                },
                success: function (data) {
                    var isCurrent = $('.ia-search-combo-site-name').text().indexOf('Current') > -1;
                    response($.map(data.d.query.PrimaryQueryResult.RelevantResults.Table.Rows.results, function (item) {
                        var itemName = item.Cells.results[2]["Value"];
                        var itemCategory = item.Cells.results[6]["Value"];
                        if (!isCurrent) {
                            itemCategory = item.Cells.results[5]["Value"];
                            if (itemCategory == null ||
								itemCategory.toLowerCase() == "akumina site definition") {
                                itemCategory = "";
                            }
                        }
                        var itemValue = item.Cells.results[3]["Value"];
                        var itemDate = "";

                        return {
                            label: itemName,
                            value: itemValue,
                            category: itemCategory,
                            date: itemDate
                        }
                    }));
                },
                error: function () {
                    return {
                        label: null,
                        value: null,
                        category: null,
                        date: null
                    }
                }
            });
        }
    }).catcomplete("widget").addClass("ia-autocomplete");


}

function typeaheadCurrentSite() {
    var searchCurrentSubsite = '';
    if ($('.ia-search-combo-site-name').text().indexOf('Current') > -1) {
        searchCurrentSubsite = 'WebId:' + webID.get_id().toString();
    }
    return searchCurrentSubsite;
}

//Get site display name from "My Sites" DDL: 
function getSiteDisplayName(value) {
    var sitedisplayname = value;
    sitedisplayname = $('#mysites option[value="' + value + '"]').html();
    return sitedisplayname;
}