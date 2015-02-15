$(document).ready(function ()
{
    $('#btnGo').click(test);
});

function test() {

    var fileInput = $('input#fileUpload');

    var filelink = fileInput.prop('files')[0];

    var form = new FormData();
    form.append("file", filelink);

    var xhr = new XMLHttpRequest();
    xhr.onload = function () {
        console.log("Upload complete " + filelink.name);
        //TODO do something with output of API call
    };
    xhr.open("post", "/api/postfile", true);
    xhr.send(form);
}