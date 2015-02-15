$(document).ready(function ()
{
    $('#btnGo').click(test);
});

function test() {

    var fileInput = $('input#fileUpload');

    var files = fileInput.prop('files');
    
    if (files.length == 0)
    {
        $('#output').text("No file selected");
        return;
    }

    var filelink = files[0];

    var form = new FormData();
    form.append("file", filelink);

    //var myData = $(form).serialize();
    //var formData = new FormData($('form')[0]);

    $.ajax({
        type: "POST",
        //contentType: attr("enctype", "multipart/form-data"),
        enctype: 'multipart/form-data',
        url: "/api/postfile",
        data: form,
        cache: false,
        contentType: false,
        processData: false,
        success: function(returnData)
        {
            console.log(returnData);
        }
    });

    //var form = new FormData();
    //form.append("file", filelink);

    //var xhr = new XMLHttpRequest();
    //xhr.onload = function () {
    //    console.log("Upload complete " + filelink.name);
    //    console.log()
    //    //TODO do something with output of API call
    //};
    //xhr.open("post", "/api/postfile", true);
    //xhr.send(form);
}