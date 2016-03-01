
function loadDragfn() {	
    if (window.File && window.FileList && window.FileReader) {
        var dropZone = document.getElementById('drop_zone');
		  dropZone.addEventListener('dragleave', handleDragLeave, false);
        dropZone.addEventListener('dragover', handleDragOver, false);
        dropZone.addEventListener('drop', handleDnDFileSelect, false);
    }
    
		
}
function handleDragLeave(event) {
    $('#drop_zone').removeClass('ia-highlighted');
}
function handleDragOver(event) {if ($("#createpermission").val() == "yes") {
    $('#drop_zone').addClass('ia-highlighted');
	}
    event.stopPropagation();
    event.preventDefault();
	
	 
}

function addFileToFolder(arrayBuffer,serverUrl,serverRelativeUrlToFolder,fileName,i,len) {

     

        // Construct the endpoint.
     var fileCollectionEndpoint = String.format(
                "{0}/_api/web/getfolderbyserverrelativeurl('{1}')/files" +
                "/add(overwrite=true, url='{2}')",
                serverUrl, serverRelativeUrlToFolder, fileName);

        // Send the request and return the response.
        // This call returns the SharePoint file.
        jQuery.ajax({
            url: fileCollectionEndpoint,
            type: "POST",
            data: arrayBuffer,
            processData: false,
            headers: {
                "accept": "application/json;odata=verbose",
                "X-RequestDigest": jQuery("#__REQUESTDIGEST").val(),
                "content-length": arrayBuffer.byteLength
            },
			 success: function (data) {
           	successHandler(fileName,i,len);
        },
        error: function () {
           errorHandler(fileName,i,len)
        }
        });
    }
function createFile(i,resultpanel,name,len) {

 var serverRelativeUrlToFolder = $('#webServerUrl').val() + '/' + $('#currentFolderPath').val();
    var baseUrl = $('#webUrl').val();
    if(serverRelativeUrlToFolder=="/")serverRelativeUrlToFolder=serverRelativeUrlToFolder+$('#listName').val();
 
			addFileToFolder(resultpanel,baseUrl,serverRelativeUrlToFolder,name,i,len);
			

  
	
}
   function successHandler(fileName,i,len) {
   
			  if(i==len)
			  successload();
    }

    function errorHandler(name,i,len) {
		var result=i+". "+name +" -	Folders and unsupported file types can't be uploaded.";
		alert(result);
		if(len==1000)
		document.getElementById('overlay').style.display = "none";
		  if(i==len)
			  successload();
    }
function successload()
{

			document.getElementById('overlay').style.display = "none";
			//ResetRefiner();
    //getPostBack(false,false,false);
			pageRefresh();
}
function handleDnDFileSelect(event) { if ($("#createpermission").val() == "yes") {
$('#drop_zone').removeClass('ia-highlighted');
    event.stopPropagation();
    event.preventDefault();
    if (event.dataTransfer) {
	var result="";
        var files;
	
        document.getElementById('txtSearch').value = "";
		  
        document.getElementById('overlay').style.display = "block";
        files = event.dataTransfer.files;
        /* Consolidate the output element. */
        var form = document.getElementById('aspnetForm');
       	
		if(files.length>0){
        for (var i = 0; i < files.length; i++) {
		
            getBuffer((i+1),files[i],files.length);
			
			
			}
			}
			else
			{
			alert("Folders and unsupported file types can't be uploaded.");
			  document.getElementById('overlay').style.display = "none";
			}
        

	 
    }    else { alert("Browser doesn't support. Please use other browsers"); }
	 }
	   else {
        $('#drop_zone').removeClass('ia-highlighted');
        alert("Drag and Drop Upload \nThe documents cannot be uploaded because different permissions are needed. Request the necessary permissions.");

    }
   
}

function getBuffer(i,file,len) {

    var reader = new FileReader();
    reader.onload = function (e) {
	createFile(i,e.target.result,file.name,len);
	
    }
    reader.onerror = function (e) {
   errorHandler(file.name,i,1000)
	 
    }
    reader.readAsArrayBuffer(file);
	
};