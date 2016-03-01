
/* ****************************************
        Document.Ready()
***************************************** */
$(document).ready(function () {
    if (document.location.search.indexOf("&u=") > -1) { $('.ia-search-combo-site-name').html("Entire System"); }
    // remove the edit link
    /*Commented out to allow search refiners to show*/
    //$('.ms-displayInlineBlock').hide();

    // initialize the typeahead code
    if ($.ui == null) {
        $.getScript("../_layouts/15/Akumina.WebParts/js/vendor/jquery-ui.min.js", function () {
            setupTypeahead();
        });
    } else {
        setupTypeahead();
    }

    $('.ia-search-combo-site-list').click(function (e) {
        e.preventDefault();
        $(this).children('.ia-search-combo-site-list-dropdown').slideToggle();
        $('.ia-search-combo-site-list-icon').toggleClass('fa-caret-down');
        $('.ia-search-combo-site-list-icon').toggleClass('fa-caret-up');
    });

    $('.ia-search-combo-site-list-dropdown a').click(function () {
        var title = $(this).html();
        $('.ia-search-combo-site-name').html(title);
    });


    SP.SOD.executeFunc('sp.js', 'SP.ClientContext', function () {
        getWebID(_spPageContextInfo.webServerRelativeUrl);
    });

    // determine if in the home page
    if (_spPageContextInfo.siteServerRelativeUrl == _spPageContextInfo.webServerRelativeUrl) {
        $('.subsite-header').hide();
    }
    else {
        // set the site title 
        var siteURL = _spPageContextInfo.webServerRelativeUrl;

        setSiteTitle();
    }

    // set up the apps
    //retrieveMyApps();


}); // end .ready()


/* ****************************************
        Functions
***************************************** */
function retrieveMyApps() {
    var siteURL = _spPageContextInfo.siteAbsoluteUrl;
    retrieveListItems(siteURL);
}

function retrieveListItems(siteUrl) {
    var clientContext = new SP.ClientContext(siteUrl);
    var oList = clientContext.get_web().get_lists().getByTitle('MyApps');
    var camlQuery = new SP.CamlQuery();
    camlQuery.set_viewXml('<View><RowLimit>100</RowLimit></View>');
    collListItem = oList.getItems(camlQuery);

    clientContext.load(
        collListItem
    );
    clientContext.load(oList);
    clientContext.executeQueryAsync(
        Function.createDelegate(this, this.onQueryRetrieveSucceeded),
        Function.createDelegate(this, this.onQueryRetrieveFailed)
    );
}
function onQueryRetrieveSucceeded(sender, args) {
    if (collListItem.get_count() > 0) {
        var listItemInfo = '<ul class="myAppsMenu">';
        var listItemEnumerator = collListItem.getEnumerator();
        while (listItemEnumerator.moveNext()) {
            var oListItem = listItemEnumerator.get_current();
            listItemInfo += '<li>' +
			'<a href="' + oListItem.get_item('URL').get_url() + '">' +
			'<img src="' + oListItem.get_item('DisplayTemplateJSIconUrl').$3_1 + '" alt="app name" />' +
			'<span class="myAppName">' + oListItem.get_item('Title') + '</span>' +
				'</a>' +
				'</li>';
        }
        listItemInfo += '</ul>';

        var li = $('span').filter(function () { return $(this).text() == 'My Apps'; }).parents("li:first");

        li.append(listItemInfo);
        li.addClass('hasSubmenu');

    }
    /* alert(listItemInfo.toString()); */
}
function onQueryRetrieveFailed(sender, args) {
    alert('Request failed. ' + args.get_message() +
        '\n' + args.get_stackTrace());
}

// redirect to the search page, appending searchterm to url
function redirectSearch() {
    var siteURL = _spPageContextInfo.webAbsoluteUrl;
    var searchPage = siteURL + "/Pages/search.aspx";
    var textValue = $('.ia-search-combo-box').val();

    urlSearchComp = "";
    if (textValue != "" && textValue.trim() != "*") {
        urlSearchComp = "?k=*" + textValue + '*' + getCurrentSite();
    } else {
        urlSearchComp = "?k=*" + getCurrentSite();
    }

    window.location = searchPage + urlSearchComp;
}

function createFeedback() {
    var siteURL = _spPageContextInfo.siteAbsoluteUrl;

    var feedback = $('.feedbackMenuTextarea').val();
    if (feedback) {
        createListItem(siteURL, feedback);
    }
}

function feedbackThankyou() {

    $.magnificPopup.open({
        items: {
            src: '#ia-feedback',

            type: 'inline'
        },
        closeBtnInside: true
    });
}



function createListItem(siteUrl, feedback) {
    var clientContext = new SP.ClientContext(siteUrl);
    var oList = clientContext.get_web().get_lists().getByTitle('Feedback');
    var itemCreateInfo = new SP.ListItemCreationInformation();
    this.oListItem = oList.addItem(itemCreateInfo);
    oListItem.set_item('Title', 'Feedback');
    oListItem.set_item('Comments', feedback);
    oListItem.update();
    clientContext.load(oListItem);
    clientContext.executeQueryAsync(
        Function.createDelegate(this, this.onQuerySucceeded),
        Function.createDelegate(this, this.onQueryFailed)
    );
}
var webID = null;
function getWebID(siteUrl) {
    var clientContext = new SP.ClientContext(siteUrl);
    webID = clientContext.get_web();
    clientContext.load(webID);
    clientContext.executeQueryAsync(
        Function.createDelegate(this, this.onQueryWebSucceeded),
        Function.createDelegate(this, this.onQueryWebFailed)
    );
}
function onQueryWebSucceeded() {

}

function onQueryWebFailed() {
}

function onQuerySucceeded() {
    $('.feedbackMenuTextarea').val('');
    $('.feedbackMenu').hide();
    if ($.magnificPopup == null) {
        $.getScript("/_catalogs/masterpage/ROG/js/jquery.magnific-popup.js", function () {
            feedbackThankyou();
        });
    }
    else {
        feedbackThankyou();
    }

}
function onQueryFailed(sender, args) {
    alert('Request failed. ' + args.get_message() +
        '\n' + args.get_stackTrace());
}

function setSiteTitle() {
    $('.subsite-name').append(_spPageContextInfo.webTitle);
}

function getCurrentSite() {
    var searchCurrentSubsite = '';
    if ($('.ia-search-combo-site-name').text().indexOf('Current') > -1) {
        searchCurrentSubsite = ' webid:' + webID.get_id().toString();
    }
    else {
        searchCurrentSubsite = '&u=' + _spPageContextInfo.siteAbsoluteUrl.replace('http://', '').replace('https://', '');
    }
    return searchCurrentSubsite;
}