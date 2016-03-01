/* Elements with Class Names Definition*/
var selectedNode = 'a.jstree-clicked', closeIcon = '.mfp-close', addFileDivId = 'listOfFiles', filesAdd_MustempId = 'files_Addtemplate', filesAdd_Hidden = "listOfFilesHidden",addReplyPostId= "li.addReplyToPost";
$(document).ready(function() {DisplayEditor();
});
function addSelectedFile(event) {
event.preventDefault();
    if ($(selectedNode).length > 0) {
        var url = $(selectedNode).parent('li').attr('url-data');
        var title = $(selectedNode).parent('li').attr('title');
        var listname = $(selectedNode).parent('li').attr('list-name');
        var itemid = $(selectedNode).parent('li').attr('item-id');
        var template = $('#' + filesAdd_MustempId).html();
        var frameJSON = '{ "Name":"' + title + '" , "Url":"' + url + '" , "ListName":"' + listname + '" , "ItemId":"' + itemid + '" }';
        var tempJson = JSON.parse(frameJSON);
        var output = Mustache.render(template, tempJson);
        var Names = listname + ":" + itemid;
        if ($('.' + addFileDivId).find('a[href="' + url + '"]').length == 0) {
            document.getElementsByClassName(addFileDivId)[0].innerHTML += output;
            $('.' + filesAdd_Hidden).val($('.' + filesAdd_Hidden).val() + Names + "|");
        }
          $.magnificPopup.close();
	if($("input[name='ia-discussion-add-reply-box']").length>0)
	{
	    $("input[name='ia-discussion-add-reply-box']").trigger("focus");
		
		}
		if(!$(".ia-folder-tree").hasClass("jstree"))
		{
		$('.ia-folder-tree').jstree(); 
		}


    }
    else {
        alert("Please select the file");
    }
}
function btnPermissions_CallServerClick(event)
{
event.preventDefault();
 $.magnificPopup.close();
 $(".hdnbtnPermissions").trigger("click");
}
function fileRemove(element) {

    $(element).parent().remove();
    var listname = $(selectedNode).parent('li').attr('list-name');
    var itemid = $(selectedNode).parent().attr('item-id');
    var listname = $(selectedNode).parent('li').attr('list-name');
    var Names = listname + ":" + itemid + "|";
    var content = $('.' + filesAdd_Hidden).val();
    var replacedContent = content.replace(Names, "");
    $('.' + filesAdd_Hidden).val(replacedContent);


}

function postValidation(event)
{
if($(".ia-discussion-create-title").val()==""){
event.preventDefault();
$("#titleMandatory").show();

}
else
{
showloader();
}

}
function EndRequestDiscussionThread()
{
hideloader();
$('.ia-folder-tree').jstree(); 
 // Toggle Editor when adding a Thread Reply
        $(".ia-discussion-reply-content").on("focus", "input[name='ia-discussion-add-reply-box']", function () {
            $(this).fadeOut("fast", function () {
                // Animation complete.
                $('.ia-discussion-reply-editor').fadeIn("fast");
            });
        });
  DisplayEditor();

  $(".ia-search-picker").chosen({
      width: '100%',
      no_results_text: "No results match"
  });
        $('.interAction .ia-modal-inline-trigger').magnificPopup({
            type: 'inline',
            preloader: false,
            closeBtnInside: true,
            showCloseBtn: true

        });

        $('.interAction .ia-modal-inline-trigger-preview').magnificPopup({
            type: 'inline',
            preloader: false,
            closeBtnInside: false,
            showCloseBtn: true

        });

        $(document).on('click', '.interAction .ia-modal-dismiss', function (e) {
            e.preventDefault();
            $.magnificPopup.close();
        });
      }
         function DisplayEditor()
    {
		$(".ia-discussion-reply-editor .ia-button-row").on("click","a.anchrPostreply",function() {
 $('.ia-discussion-reply-editor').hide();
		 // animation complete.
 $(".ia-discussion-reply-content input[name='ia-discussion-add-reply-box']").show();			

});
        $(".ia-action-menu-content").on("click", addReplyPostId , function (e) {
            e.preventDefault();
            $("input[name='ia-discussion-add-reply-box']").trigger("focus");
        });

       
    }
    function DeleteReplyFunction(id) {
        //var thisid = $(this);
        $('#hdnDeleteReplyId').val(id);
        // alert(this.attr('data-val'));

    };


function hideloader() {
    if ($('.ia-loading-panel')) $('.ia-loading-panel').addClass("ia-hide");
    $('.ia-loading-panel').removeClass("ia-show");
}

function showloader() {
$.magnificPopup.close();
  if ($('.ia-loading-panel')) $('.ia-loading-panel').addClass("ia-show");
    $('.ia-loading-panel').removeClass("ia-hide");
}

function archiveFunction(id) {

    $('#hdnArchieveId').val(id);


}
function deleteFunction(id) {
    $('#hdnDeleteId').val(id);
}
function commonDiscussionThreadListing() {
    if (typeof (Sys.Browser.WebKit) == "undefined") {
        Sys.Browser.WebKit = {};
    }
    if (navigator.userAgent.indexOf("WebKit/") > -1) {
        Sys.Browser.agent = Sys.Browser.WebKit;
        Sys.Browser.version = parseFloat(navigator.userAgent.match(/WebKit\/(\d+(\.\d+)?)/)[1]);
        Sys.Browser.name = "WebKit";
    }

    if ($('#hdnCurrentPage').attr('value') == "first") {
        $("#iafirstpage").removeAttr("class").attr("class", "ia-paging-first ia-paging-disabled");
        $("#iapreviouspage").removeAttr("class").attr("class", "ia-paging-previous ia-paging-disabled");
    }
    else if ($('#hdnCurrentPage').attr('value') == "last") {
        $("#ianextpage").removeAttr("class").attr("class", "ia-paging-next ia-paging-disabled");
        $("#ialastpage").removeAttr("class").attr("class", "ia-paging-last ia-paging-disabled");
    }
    else if ($('#hdnCurrentPage').attr('value') == "inbetween") {
        $("#iafirstpage").removeAttr("class").attr("class", "ia-paging-first");
        $("#iapreviouspage").removeAttr("class").attr("class", "ia-paging-previous");
        $("#ianextpage").removeAttr("class").attr("class", "ia-paging-next");
        $("#ialastpage").removeAttr("class").attr("class", "ia-paging-last");
    }

    if ($('#hdnSortBy').attr('value') == "Created") {
        //if ($('[id$=linkSortCreated]').attr('class') == "ia-sort-icon fa fa-sort-asc ia-sort-asc") {
        if ($('#hdnSortDirection').attr('value') == "ASC") {
            //alert("if : " + $('[id$=linkSortPostTime]').attr('class'));
            $('[id$=linkSortCreated]').removeAttr('class').addClass("ia-sort-icon fa fa-sort-desc ia-sort-desc");
        }
        else {
            //alert("else : " + $('[id$=linkSortPostTime]').attr('class'));
            $('[id$=linkSortCreated]').removeAttr('class').addClass("ia-sort-icon fa fa-sort-asc ia-sort-asc");
        }
        $('[id$=linkSortPostTime]').removeAttr('class').attr("class", "ia-sort-icon fa fa-sort ia-sort-off");
        $('[id$=linkSortReplies]').removeAttr('class').attr("class", "ia-sort-icon fa fa-sort ia-sort-off");
    }
    else if ($('#hdnSortBy').attr('value') == "DiscussionLastUpdated") {

        //if ($('[id$=linkSortPostTime]').attr('class') == "ia-sort-icon fa fa-sort-asc ia-sort-asc") {
        if ($('#hdnSortDirection').attr('value') == "ASC") {
            //alert("if : "+$('[id$=linkSortPostTime]').attr('class'));
            $('[id$=linkSortPostTime]').removeAttr('class').addClass("ia-sort-icon fa fa-sort-desc ia-sort-desc");
        }
        else {
            //alert("else : "+$('[id$=linkSortPostTime]').attr('class'));
            $('[id$=linkSortPostTime]').removeAttr('class').addClass("ia-sort-icon fa fa-sort-asc ia-sort-asc");
        }
        $('[id$=linkSortCreated]').removeAttr('class').attr("class", "ia-sort-icon fa fa-sort ia-sort-off");
        $('[id$=linkSortReplies]').removeAttr('class').attr("class", "ia-sort-icon fa fa-sort ia-sort-off");
    }
    else if ($('#hdnSortBy').attr('value') == "ItemChildCount") {
        //if ($('[id$=linkSortReplies]').attr('class') == "ia-sort-icon fa fa-sort-asc ia-sort-asc") {
        if ($('#hdnSortDirection').attr('value') == "ASC") {
            //alert("if : " + $('[id$=linkSortPostTime]').attr('class'));
            $('[id$=linkSortReplies]').removeAttr('class').addClass("ia-sort-icon fa fa-sort-desc ia-sort-desc");
        }
        else {
            //alert("else : " + $('[id$=linkSortPostTime]').attr('class'));
            $('[id$=linkSortReplies]').removeAttr('class').addClass("ia-sort-icon fa fa-sort-asc ia-sort-asc");
        }
        $('[id$=linkSortCreated]').removeAttr('class').attr("class", "ia-sort-icon fa fa-sort ia-sort-off");
        $('[id$=linkSortPostTime]').removeAttr('class').attr("class", "ia-sort-icon fa fa-sort ia-sort-off");
    }
}
function EndRequestThreadListing() {
    hideloader();



    $('.interAction .ia-modal-inline-trigger').magnificPopup({
        type: 'inline',
        preloader: false,
        closeBtnInside: true,
        showCloseBtn: true

    });

    $('.interAction .ia-modal-inline-trigger-preview').magnificPopup({
        type: 'inline',
        preloader: false,
        closeBtnInside: false,
        showCloseBtn: true

    });

    $(document).on('click', '.interAction .ia-modal-dismiss', function (e) {
        e.preventDefault();
        $.magnificPopup.close();
    });

    commonDiscussionThreadListing();
}