
function loadDragfn() {
    if ($(".uploadCreateButtons").length > 0) {
        if (window.File && window.FileList && window.FileReader) {
            var dropZone = document.getElementById('drop_zone');
            dropZone.addEventListener('dragover', handleDragOver, false);
            dropZone.addEventListener('dragleave', handleDragLeave, false);
            dropZone.addEventListener('drop', handleDnDFileSelect, false);
        }

    }
}
function handleDragLeave(event) {
    $('#drop_zone').removeClass('ia-highlighted');
}
function handleDragOver(event) {
    if ($("#createpermission").val() == "yes") {
        $('#drop_zone').addClass('ia-highlighted');
    }
    event.stopPropagation();
    event.preventDefault();
}
var files; var rejectedOWFiles = []; var ConflictFiles = []; var fileStatus = ""; var folderPath = ""; var currentCount = 0; var uploadingFiles = [];
var percent = 10;
function handleDnDFileSelect(event) {
    fileStatus = ""; folderPath = "";
    if ($("#createpermission").val() == "yes") {
        $('#drop_zone').removeClass('ia-highlighted');
        event.stopPropagation();
        event.preventDefault();
        if (event.dataTransfer) {
            IntializeEmptyValues();
            rejectedOWFiles.length = 0;
            ConflictFiles.length = 0;
            files = event.dataTransfer.files;
            ExistingFileCheck();

        } else { alert("Browser doesn't support. Please use other browsers"); }
    }
    else {
        $('#drop_zone').removeClass('ia-highlighted');
        alert("Drag and Drop Upload \nThe documents cannot be uploaded because different permissions are needed. Request the necessary permissions.");

    }
}
function ExistingFileCheck() {
    folderPath = $('#webServerUrl').val() + '/' + $('#currentFolderPath').val();
    var listOfFiles = []; var exists = false; var upload_continue = false;
    var tempfiles = files; var exitsCount = []; var rejectedcount = 0; var consecutiveReject
    $("#grid .disp-File-Name").each(function () {

        var documentName = decodeURIComponent($(this).attr("href")).trim().split('?')[0];
        listOfFiles.push(documentName);
    });

    if (tempfiles.length > 0) {


        var emptyVal = "";
        for (var i = 0; i < tempfiles.length; i++) {
            var fileName = folderPath + "/" + tempfiles[i].name;
            if (tempfiles[i].type == "" && tempfiles[i].size == 0) {
                rejectedOWFiles.push(tempfiles[i].name);
                fileStatus += (tempfiles[i].name + " - Folders and unsupported file types can't be uploaded. <br/>");
            }
            else if (tempfiles[i].size == 0) {
                rejectedOWFiles.push(tempfiles[i].name);
                fileStatus += (tempfiles[i].name + " - This file is empty and needs content to be uploaded. <br/>");
            }
            else if (jQuery.inArray(fileName, listOfFiles) >= 0) {
                ConflictFiles.push(tempfiles[i].name);
            }
        }
        CallModalDialogExistingFile();

    }
    else
        fadeOutUploadSuccess("Folders and unsupported file types can't be uploaded.");


}



function CallModalDialogExistingFile() {
    if (ConflictFiles.length > 0) {
        //Ensure conflict checkbox is cleared
        $('.conflictDlg').prop("checked", false);
        //We need to show the div here since nothing will cause it to reappear once it's hidden
        $('.divConflictCheck').show();
        $('.currentUploadFileName').html(ConflictFiles[0]);
        ConflictFiles.splice(0, 1);
        if (ConflictFiles.length > 0) {
            $('.remainingFiles').html(ConflictFiles.length);
        }
        else {
            $('.divConflictCheck').hide();
        }

        $.magnificPopup.open({
            items: {
                src: '.ia-library-multipleupload',
                type: 'inline'
            },
            closeOnBgClick: false,
            showCloseBtn: true
        });
    }
    else {
        if (files.length != rejectedOWFiles.length) {
            if ($('.uploadButton').length > 0)
                $('.uploadButton').click();
        }
        else if (fileStatus != "") {
            fadeOutUploadSuccess(fileStatus);
        }
    }

}



function CallReplaceIt(event) {
    event.preventDefault();
    $.magnificPopup.close();

    if ($(".conflictDlg").prop('checked') == true)
        ConflictFiles.length = 0;

    CallModalDialogExistingFile();

}

function RejectReplaceIt(event) {
    event.preventDefault();

    $.magnificPopup.close();
    var currentFile = $('.currentUploadFileName').html();
    rejectedOWFiles.push(currentFile);
    if ($(".conflictDlg").prop('checked') == true) {
        for (var i = 0; i < ConflictFiles.length; i++) {
            rejectedOWFiles.push(ConflictFiles[0]);
        }
        ConflictFiles.length = 0;
    }




    CallModalDialogExistingFile();

}
function FilesUploadToLibrary(event) {
    event.preventDefault();
    /* Consolidate the output element. */
    var form = document.getElementById('aspnetForm');
    /* var data = new FormData(form);
    var action = form.getAttribute("action");*/
    var j = 0;
    var uploadedfiles = [];
    for (var i = 0; i < files.length; i++) {
        var filename = files[i].name;
        if (jQuery.inArray(filename, rejectedOWFiles) < 0) {

            ++j;

        }

    }
    uploadingFiles.length = 0;
    currentCount = 0;
    if (j > 0) {

        if ($('.ia-progress-percent'))
            $('.ia-progress-percent').attr("style", "width: " + "10" + "%;");
        if ($('.ia-progress-text'))
            $('.ia-progress-text').html("10%");
        var k = 0;
        for (var i = 0; i < files.length; i++) {
            var filename = files[i].name;
            if (jQuery.inArray(filename, rejectedOWFiles) < 0) {
                ++k;
                uploadedfiles.push(filename);
                uploadingFiles.push(i);
                //getBuffer(k, files[i], j);
            }
            $('#hdnUploadedFiles')
            $('#hdnUploadedFiles').val(uploadedfiles.join(','));
        }
        if (uploadingFiles.length > 0) {
            getBuffer(0, files[uploadingFiles[0]], j);
        }
    }

}

function callCodeBehindForUpload() {
    document.getElementById('txtSearch').value = "";

    if ($('#hdnQueryQF'))
        $('#hdnQueryQF').attr('value', '');
    if ($('#searchTextTab'))
        $('#searchTextTab').val('');
    if ($('#searchTextQuery'))
        $('#searchTextQuery').val('');

    $.magnificPopup.close();
    updaterefiner();
    showloader();
    if ($('#displayUploadSuccess'))
        $('#displayUploadSuccess').val('success');
    document.getElementById('viewSateReset').click();


}
function addFileToFolder(arrayBuffer, serverUrl, serverRelativeUrlToFolder, fileName, i, len) {



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
            "X-RequestDigest": jQuery("#__REQUESTDIGEST").val()
        },
        success: function (data) {
            successHandler(fileName, i, len);
        },
        error: function (xhr, textStatus, err) {

            try {
                var response = JSON.parse(xhr.responseText);
                var message = response ? response.error.message.value :  (" Unexpected response from server. The status code of response is " + textStatus.toString());
                errorHandler(fileName, i, len, message);
            }
            catch (err) {

                errorHandler(fileName, i, len, ("Unexpected response from server. The status code of response is " + xhr.status.toString()));
            }
        }
    });
}
function createFile(i, resultpanel, name, len) {

    var serverRelativeUrlToFolder = $('#webServerUrl').val() + '/' + $('#currentFolderPath').val();
    var baseUrl = $('#webUrl').val();
    if (serverRelativeUrlToFolder == "/") serverRelativeUrlToFolder = serverRelativeUrlToFolder + $('#listName').val();

    addFileToFolder(resultpanel, baseUrl, serverRelativeUrlToFolder, name, i, len);




}
function successHandler(fileName, i, len) {
    ++currentCount;

    var len = uploadingFiles.length;
    if (currentCount == len) {
        setTimeout(uploadProgress(Math.ceil(90 / len)), 500);
        percent = 10;
    }
    else
        uploadProgress(Math.ceil(90 / len));
    if (currentCount < len)
        getBuffer(currentCount, files[uploadingFiles[currentCount]], len);
}

function errorHandler(name, i, len, messsage) {

    ++currentCount;
    if (len == 1000)
        fileStatus += (name + " -	Folders and unsupported file types can't be uploaded." + "<br/>");
    else if (messsage != null && typeof messsage !== 'undefined') {
        var fileName = folderPath + "/";
        fileStatus += (name + " - " + messsage.replace(fileName, "") + "<br/>");
    }

    var len = uploadingFiles.length;
    if (currentCount == len) {
        setTimeout(uploadProgress(Math.ceil(90 / len)), 500);
        percent = 10;
    }
    else
        uploadProgress(Math.round(90 / len));
    if (currentCount < len)
        getBuffer(currentCount, files[uploadingFiles[currentCount]], len);
}

function uploadProgress(uploadPercent) {

    percent = percent + uploadPercent;
    if (percent > 100)
        percent = 100;
    //alert(percent);
    if ($('.ia-progress-percent'))
        $('.ia-progress-percent').attr("style", "width: " + percent + "%;");
    if ($('.ia-progress-text'))
        $('.ia-progress-text').html(percent + "%");
    if (percent == 100) {
        if ($('.uploaded_Success').length > 0)
            $('.uploaded_Success').html(fileStatus);
        dummyFunciton();
        setTimeout(function () { callCodeBehindForUpload(); }, 5000);
    }

}

function dummyFunciton() {
    var context = new SP.ClientContext.get_current();
    var web = context.get_web();
    context.load(web);
    context.executeQueryAsync(function () {
        console.log(web.get_title());
        console.log(web.get_url());
    },
      function (sender, args) {
          console.log(args.get_message());
      }
    );
}
function getBuffer(i, file, len) {
    if (file.size > 0) {

        var reader = new FileReader();
        reader.onload = function (e) {
            createFile(i, e.target.result, file.name, len);

        }
        reader.onerror = function (e) {
            errorHandler(file.name, i, 1000)

        }
        reader.readAsArrayBuffer(file);
    }
    else
        errorHandler(file.name, i, len);



};