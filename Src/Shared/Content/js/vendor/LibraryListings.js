$(document).ready(function(){
	SP.SOD.executeFunc('sp.js', 'SP.ClientContext', 
	 function(){
		 Initialize();
	 }
	);
});


function Initialize(){
	Initialize_GetLibraries().then(
		function(lists){
			//alert("working Initalize");
			
			var imgDocPath="/_layouts/15/images/itdl.png?rev=23";
			var imgImagepath="/_layouts/15/images/ital.png?rev=23";
			var imgSitePage="/_layouts/15/images/itwp.png?rev=23";
			var imageURL='';
			var libTitleColl = '';
			var count = lists.get_count();
			var excludelists=document.getElementById("listingsValue").value;
			//var redirectOption=document.getElementById("RedirectionOptionValue").value;
			var documentSummarypage=window.location.pathname.substring(0,window.location.pathname.lastIndexOf("/"))+"/"+document.getElementById("DMSLandingPageValue").value;   
		   
			$(".ia-search-library-site-list-dropdown > ul").append("<li class=\"ia-search-all\"><input class=\"ia-search-checkbox\" type=\"checkbox\" checked=\"checked\" name=\"ListingName\" id=\"all-libraries\"></input><label class=\"ia-search-label\" for=\"all-libraries\">All Libraries</label></li>");

			for (x = 0; x < count; x++) {
			  if (lists.itemAt(x).get_baseType() == SP.BaseType.documentLibrary) {
				var libTitle = lists.itemAt(x).get_title();
				//console.log(libTitle + "  IsCatalog: " + lists.itemAt(x).get_isCatalog()+" Hidden:"+lists.itemAt(x).get_hidden()+" Allow Editing: "+lists.itemAt(x).get_imageUrl());
				
				if(excludelists.split(',').indexOf(libTitle) == -1)
				{
					libTitleColl += libTitle + ",";        
					imageURL=lists.itemAt(x).get_imageUrl();
					if(imageURL==imgDocPath)
					{						
						imgValue="<img class=\"defaultimagestyle\" style=\" background-Color:#0072c6\" src=\"/_layouts/15/images/ltdl.png?rev=23\">";						
					}
					else if(imageURL==imgImagepath)
					{						
						imgValue="<img class=\"defaultimagestyle\" style=\" background-Color:#0072c6\" src=\"/_layouts/15/images/ltal.png?rev=23\">";						
					}
					else if(imageURL==imgSitePage)
					{
                        imgValue="<img class=\"defaultimagestyle\" style=\" background-Color:#0072c6\" src=\"/_layouts/15/images/ltwp.png?rev=23\">";
					}
					else
					{
						imgValue="<img class=\"defaultimagestyle\" src="+lists.itemAt(x).get_imageUrl()+">";
					}
					$(".ia-library-list-grid").append("<li><a href='"+documentSummarypage+"?docLibName="+libTitle+"'"+"><div class=\"ia-library-icon\">"+imgValue+"</div><div class=\"ia-library-info\"><p class=\"ia-library-title\">"+libTitle+"</p><p class=\"ia-library-site\">Site: "+$("#webTitleValue").val()+"</p></div></a></li>");
					$(".ia-search-library-site-list-dropdown > ul").append("<li class=\"ia-search-single-library\"><input class=\"ia-search-checkbox\" type=\"checkbox\" checked=\"checked\" name=\"ListingName\" id=\"" + libTitle + "-libraries\"></input><label class=\"ia-search-label\" for=\"" + libTitle + "-libraries\">" + libTitle + "</label></li>");
				
				}			   
			  }
			}
		},
		function (sender, args) {
                console.log('An error occured while retrieving libraries:' + args.get_message());
        }
	).then(	
		function(){
			getLibraryImages();
		}
	).then(
		function(){
			librarySearch();
		}
	);
}

function Initialize_GetLibraries(){
	var deferred= $.Deferred();
	var clientContext = new SP.ClientContext.get_current();
	 //get the current website
    var web = clientContext.get_web();
	//get all list from current web
    this.lists = web.get_lists();
	//load the context
    clientContext.load(lists);
	//execute the query in async mode
    clientContext.executeQueryAsync(Function.createDelegate(this, function(){deferred.resolve(lists);}), Function.createDelegate(this,function (sender, args) { deferred.reject(sender, args); }));	
	
	return deferred.promise();
}


function getLibraryImages(){
	Initialize_GetLibraryImages().then(
		function(pictures,FileLeafRef){			
			pictureArray = new Array();
			var pictureCount = 0;
			var enumerator = pictures.getEnumerator();
			while(enumerator.moveNext()) {
			var currentItem = enumerator.get_current();
			var filename = currentItem.get_item('FileLeafRef');
			pictureArray[pictureCount++] = filename.split('.')[0].toLowerCase();			
		   }
		   var ImagesLibrary=document.getElementById("ImagesLibraryValue").value; 
		   
			$(".ia-library-title").each(function(){				
				if(jQuery.inArray( $(this).text().toLowerCase(), pictureArray )>-1)
				{	
					$(this).parent().siblings('.ia-library-icon').find('.defaultimagestyle').remove();
					$(this).parent().siblings('.ia-library-icon').append('<img src=\"'+ImagesLibrary+'/'+$(this).text()+'.png">');
				}
			});
		},
		function (sender, args) {
                console.log('An error occured while retrieving:' + args.get_message());
        }
	);	
}

function Initialize_GetLibraryImages()
{
	var deferred=$.Deferred();
	
	var context = new SP.ClientContext.get_current();
    //get the current website
    var web = context.get_web();
    //get the pictures list
    var list = web.get_lists().getByTitle('AkuminaImages');
    //create the query to get all items
    var query = SP.CamlQuery.createAllItemsQuery();
    //get all items from the query
    pictures = list.getItems(query);
    //load the context
    context.load(pictures, 'Include(FileLeafRef,FileDirRef)');
    //execute the query in async mode
    context.executeQueryAsync(Function.createDelegate(this, function () { deferred.resolve(this.pictures); }),Function.createDelegate(this, function (sender, args) { deferred.reject(sender, args); }));
	
	return deferred.promise();
	
}


function SearchLib(){
	
	 var textSearch = $("#txtDocSearch").val();
	 if (textSearch!= '')
       {  
          var libValues='';
            $.each($("input[name='ListingName']:checked"), function(){				
               if($(this).siblings().text()!='All Libraries')
                libValues += $(this).siblings().text() + ",";
            });

           
            var loc=window.location.href;
            var redirectURL=loc.substring(0,loc.lastIndexOf("/"))+"/"+document.getElementById("SearchRedirectURLValue").value+"?searchText="+textSearch+"&docLibName="+libValues;
            var redirectOption=document.getElementById("RedirectionOptionValue").value;			
			
                if (redirectOption == "SameWindow") {
                    var link = document.createElement('a');
                    link.href = redirectURL;
                  
                    document.body.appendChild(link);
                    link.click();
                }
                else {
                   
                    window.open(redirectURL);
                }  
       }
       else {

           alert("Enter the search text");
       }
	
}

$(document).keypress(function (e) {
  if (e.which == 13) {
    SearchLib();
    return false;
  }
});