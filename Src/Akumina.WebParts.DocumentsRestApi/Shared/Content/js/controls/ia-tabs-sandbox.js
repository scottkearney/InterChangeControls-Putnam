$(document).ready(function () {   
   
   $('#tabStr').attr('value', "All Files");
    var current = $('#lblID').attr('value') != null ? parseInt($('#lblID').attr('value')) : 0;
    $(".ia-tabs-nav").find("li").eq(0).addClass("ia-tab-active");
    $('.ia-folder-tree').jstree();
    $('.ia-folder-tree').show();  

if($("#currentFolderPath").val()=="")    
	$("#currentFolderPath").attr("value",$("#listName").attr("value"));

if($("#currentFolderPath").val()!=$("#listName").attr("value"))    
expandTreeView(); 
	
    updateBreadCrumb();
	pageloadfn();
	rowhighlight();
//$(".ia-documentList").width(Math.round($(".ia-documentList").width() *(0.95)));
//$(".ia-doclist-name").width(Math.round($(".ia-documentList").width() *(0.4)));



});


