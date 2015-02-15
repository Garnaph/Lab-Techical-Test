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

    $.ajax({
        type: "POST",
        enctype: 'multipart/form-data',
        url: "/api/postfile",
        data: form,
        cache: false,
        contentType: false,
        processData: false,
        success: function(returnData)
        {
            console.log(returnData);
            $('#output').text('Result : ' + returnData.Result);
            $('#message').text('Debug Message : ' + returnData.Message);
        }
    });
}