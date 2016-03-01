if (Akumina === undefined) {
    var Akumina = {};
}

if (Akumina.InterAction === undefined) {
    Akumina.InterAction = {};
}

if (Akumina.InterAction.SiteLinks === undefined) {

    Akumina.InterAction.SiteLinks = function () {
        this.targetDiv = "";
        this.uniqueId = "";
        this.searchResults = {};
        this.templateJson = {};
        this.Color = "";
        this.Icon = "";
        this.MoreLink = "";
        this.MoreText = "";
        this.MoreWindow = "";
        this.QueryPart = "";
        this.LinksField = "";

        this.getDataUsingREST = function (me) {

            var useListName = true;
            var listName = "";
            var useItemTitle = true;
            var itemTitle = "";
            if (this.InstructionSetId != "") {
                var instruction = this.InstructionSetId.split(".");
                listName = instruction[0];
                if (this.isGuid(listName)) {
                    useListName = false;
                }
                if (instruction.length > 0) {
                    itemTitle = instruction[1];
                    if (Math.floor(itemTitle) == itemTitle && $.isNumeric(itemTitle)) {
                        useItemTitle = false;
                    }
                }
                
                var searchUrl = this.getSearchUrl(listName, useListName, itemTitle, useItemTitle);

                $.ajax({
                    url: searchUrl,
                    headers: { "Accept": "application/json; odata=verbose" }
                }).done(function (data) {
                    me.ongetPropertySuccess(data);
                }).fail(function (data, errorCode, errorMessage) {
                    me.ongetDataFail(data, errorCode, errorMessage);
                });
            } else if (this.QueryPart != "") {
                var queryPart = this.QueryPart.split(".");
                listName = queryPart[0];
                if (this.isGuid(listName)) {
                    useListName = false;
                }
                if (queryPart.length > 0) {
                    itemTitle = queryPart[1];
                    if (Math.floor(itemTitle) == itemTitle && $.isNumeric(itemTitle)) {
                        useItemTitle = false;
                    }
                }

                var searchUrl = this.getSearchUrl(listName, useListName, itemTitle, useItemTitle);

                $.ajax({
                    url: searchUrl,
                    headers: { "Accept": "application/json; odata=verbose" }
                }).done(function (data) {
                    me.ongetDataSuccess(data);
                }).fail(function (data, errorCode, errorMessage) {
                    me.ongetDataFail(data, errorCode, errorMessage);
                });
            }
        };

        this.ongetPropertySuccess = function (data) {
            var useListName = true;
            var listName = "";
            var useItemTitle = true;
            var itemTitle = "";
            var me = this;
            var jsonObject = data;
            var results = jsonObject.d.results;
            if (results.length > 0) {
                var result = results[0];
                this.ItemDisplay = result.ItemDisplay;

                this.Title = result.Title;
                this.Color = result.Color.toLowerCase();
                this.Icon = me.getIcon(result.Icon.toLowerCase());

                this.MoreText = (result.MoreText != null ? result.MoreText : "");
                this.MoreLink = (result.MoreLink != null ? result.MoreLink : "");
                var moreTarget = (result.MoreWindow != null ? result.MoreWindow : "").toLowerCase();
                if (moreTarget.indexOf("new") > -1) {
                    resultJson.MoreTarget = '_blank';
                } else {
                    resultJson.MoreTarget = '_self';
                }

                this.QueryPart = result.QueryPart;
                var queryPart = this.QueryPart.split(".");
                listName = queryPart[0];
                if (this.isGuid(listName)) {
                    useListName = false;
                }
                if (queryPart.length > 0) {
                    itemTitle = queryPart[1];
                    if (Math.floor(itemTitle) == itemTitle && $.isNumeric(itemTitle)) {
                        useItemTitle = false;
                    }
                }
            }

            var searchUrl = this.getSearchUrl(listName, useListName, itemTitle, useItemTitle);

            $.ajax({
                url: searchUrl,
                headers: { "Accept": "application/json; odata=verbose" }
            }).done(function (data) {
                me.ongetDataSuccess(data);
            }).fail(function (data, errorCode, errorMessage) {
                me.ongetDataFail(data, errorCode, errorMessage);
            });
        };

        this.ongetDataSuccess = function (data) {
            var me = this;
            var jsonObject = data;
            var results = jsonObject.d.results;
            if (results.length == 0) {
                $(me.targetDiv).html("");
            } else {
                var searchResultsHtml = "";
                var controlTemplate = me.templateJson.ControlTemplate;
                var itemTemplate = me.templateJson.ItemTemplate;
                if (results.length > 0) {
                    var result = results[0];
                    var resultJson = {};
                    if (this.Title != null && this.Title != '') {
                        resultJson.Title = this.Title;
                    } else {
                        resultJson.Title = result.Title;
                    }
                    resultJson.Color = this.Color.toLowerCase();
                    resultJson.Icon = me.getIcon(this.Icon.toLowerCase());
                    
                    resultJson.MoreText = this.MoreText;
                    resultJson.MoreLink = this.MoreLink;
                    var moreTarget = (this.MoreWindow != null ? this.MoreWindow : "").toLowerCase();
                    if (moreTarget.indexOf("new") > -1) {
                        resultJson.MoreTarget = '_blank';
                    } else {
                        resultJson.MoreTarget = '_self';
                    }
                    // control
                    var heroContainer = Mustache.to_html(controlTemplate, resultJson);
                    $(me.targetDiv).append(heroContainer);

                    var linkField = result[this.LinksField];
                    // items
                    if (linkField != null && linkField != '') {
                        $.each($(linkField).find('a'), function (index) {
                            var anchorJson = {};
                            anchorJson.Title = $(this).text();
                            anchorJson.URL = $(this).attr('href');                            
                            var target = $(this).attr('target');
                            if (target == null || target == '') {
                                target = '_self';
                            }
                            anchorJson.Target = target;
                            anchorJson.Color = resultJson.Color;
                            if (anchorJson.Title != '') {
                                var li = Mustache.to_html(itemTemplate, anchorJson);
                                $(".ia-link-list-items", me.targetDiv).append(li);
                            }
                        });
                    }
                }

            }
        };

        this.ongetDataFail = function (data, errorCode, errorMessage) {
            var me = this;
            $(me.targetDiv).text("An error occurred - " + errorCode + " - " + errorMessage);
        };

        this.getIcon = function (iconName) {
            var iconValue = 'none';
            switch (iconName.toLowerCase()) {
                case 'linklist':
                    iconValue = 'link';
                    break;
                case 'forms':
                    iconValue = 'newspaper-o';
                    break;
                case 'emergency':
                    iconValue = 'exclamation-triangle';
                    break;
                case 'surveys':
                    iconValue = 'list-ul';
                    break;
                case 'policies':
                    iconValue = 'file-text-o';
                    break;
                case 'chat':
                    iconValue = 'weixin';
                    break;
                default:
                    iconValue = iconName.toLowerCase();
                    break;
            }
            return 'fa-' + iconValue;
        };

        this.getSearchUrl = function (listName, useListName, itemTitle, useItemTitle) {

            var searchUrl = _spPageContextInfo.webAbsoluteUrl + "/_api/lists/";
            if (useListName) {
                searchUrl += "getbytitle('" + listName + "')/";
            } else {
                searchUrl += "getbyid('" + listName + "')/";

            }

            if (useItemTitle) {
                searchUrl += "items?$filter=(Title eq '" + itemTitle + "')";
            } else {
                searchUrl += "items(" + itemTitle + ")";
            }
            return searchUrl;
        };

        this.isGuid = function (val) {
            var re = /\[([a-f0-9]{8}(?:-[a-f0-9]{4}){3}-[a-f0-9]{12})\]/i;

            var match = re.exec(val);

            return match ? match[1] : null;
        };
    };
}