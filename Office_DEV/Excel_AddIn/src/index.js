'use strict';

// Tutorial 1: (using Base64)
// https://docs.microsoft.com/en-us/office/dev/add-ins/word/get-the-whole-document-from-an-add-in-for-word

(function () {
    Office.initialize = function (reason) {
        $(document).ready(function () {
            $('#set-color').click(setColor);
            // $('#submit').click(sendFile);
            // debugger;
            var stat = document.getElementById("status");
            stat.innerHTML = ":";
            // var fileNameField = document.getElementById("office_file");
            // fileNameField.value = Office.context.document.url;
            updateStatus("REady to send tHa FilE  - 120");
        });
    };

    function setColor() {
        Excel.run(function (context) {
            var range = context.workbook.getSelectedRange();
            range.format.fill.color = 'green';

            return context.sync();
        }).catch(function (error) {
            console.log("Error: " + error);
            if (error instanceof OfficeExtension.Error) {
                console.log("Debug info: " + JSON.stringify(error.debugInfo));
            }
        });
    }

    function updateStatus(message) {
        // var statusInfo = $('#status');
        var statusInfo = document.getElementById("status");
        statusInfo.innerHTML += message + "<br/>";
    }

    // Get all of the content from a PowerPoint or Word document in 200-KB chunks of text.
    function sendFile() {
        var testDoc = Office.context.document;
        debugger;
        Office.context.document.getFileAsync(Office.FileType.Compressed,
           { sliceSize: 4194304 },
            function (result) {
                debugger;
                if (result.status == Office.AsyncResultStatus.Succeeded) {

                    // Get the File object from the result.
                    var myFile = result.value;
                    var state = {
                        file: myFile,
                        counter: 0,
                        sliceCount: myFile.sliceCount
                    };

                    updateStatus("Getting file of " + myFile.size + " bytes");
                    getSlice(state);
                }
                else {
                    updateStatus(result.status);
                }
            });
    }

    // Get a slice from the file and then call sendSlice.
    function getSlice(state) {
        state.file.getSliceAsync(state.counter, function (result) {
            debugger;
            if (result.status == Office.AsyncResultStatus.Succeeded) {
                updateStatus("Sending piece " + (state.counter + 1) + " of " + state.sliceCount);
                sendSlice(result.value, state);
            }
            else {
                updateStatus(result.status);
            }
        });
    }

    function sendSlice(slice, state) {
        var data = slice.data;
    
        // If the slice contains data, create an HTTP request.
        if (data) {
    
            // Encode the slice data, a byte array, as a Base64 string.
            // NOTE: The implementation of myEncodeBase64(input) function isn't 
            // included with this example. For information about Base64 encoding with
            // JavaScript, see https://developer.mozilla.org/en-US/docs/Web/JavaScript/Base64_encoding_and_decoding.
            //TODO: Get Rid of Base64 and send it BINARY.
            // var fileData = new Blob(data); // myEncodeBase64(data);
            var buff = new ArrayBuffer(data.length);
            debugger;
            var fileData = new Uint8Array(buff);
            for (var ix = 0; ix < fileData.length; ix++) {
                fileData[ix] = data[ix];
            }
    
            // Create a new HTTP request. You need to send the request 
            // to a webpage that can receive a post.
            var request = new XMLHttpRequest();
            debugger;
            // Create a handler function to update the status 
            // when the request has been sent.
            request.onreadystatechange = function () {
                debugger;
                if (request.readyState == 4) {
                    
                    updateStatus("Sent " + slice.size + " bytes.");
                    state.counter++;
    
                    if (state.counter < state.sliceCount) {
                        getSlice(state);
                    }
                    else {
                        closeFile(state);
                    }
                }
            }

            request.onload = function (oEvent) {
                updateStatus("Uploaded");
                debugger;
            }
            
            // request.setRequestHeader("Slice-Number", slice.index);
            // request.setRequestHeader('filename',"el_nombre.xlsx");
            // request.setRequestHeader('name', 'file');
            // request.setRequestHeader('Content-Type', 'multipart/form-data');
            // request.setRequestHeader('Accept-Encoding', 'gzip, deflate');
            // request.setRequestHeader('Content-Length', slice.size);
            
            request.open("POST", "https://localhost:44300/api/values/5", false);//false->sync
    
            // Send the file as the body of an HTTP POST 
            // request to the web server.
            request.send(fileData);
        }
    }
})();