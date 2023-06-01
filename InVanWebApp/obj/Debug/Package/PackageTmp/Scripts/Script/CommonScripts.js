/*---------------------This function is for exporting the page into pdf format using Canvas.----------------*/



function ExportPdfFunction(divId, pdfName, height=580, width=480) {
    var printD = document.getElementById(divId);
    $("#btnExport").hide();
    $("#btnBack").hide();

    html2canvas(printD).then(function (canvas) {

        var img = canvas.toDataURL("image/png", 1.0);

        var doc = new jsPDF('p', 'pt', 'a4');

        var specialElementHandlers = {
            '#editor': function (element, renderer) {
                return true;
            }
        };
        
        doc.addImage(img, 'JPEG', 5, 5, height, width);
        doc.fromHTML(printD, 15, 15, {
            'width': 200,
            'elementHandlers': specialElementHandlers
        });
        var currentDate = new Date();
        var today = new Date();
        var time = today.getHours() + ":" + today.getMinutes() + ":" + today.getSeconds();
        var today = formatDate(currentDate);
        doc.save(pdfName + '_' + today + '_' + time + '.pdf');
        $("#btnExport").show();
        $("#btnBack").show();
    });

}

/*----This is function is used in the above export to pdf function for inserting current date and time 
 * in the pdf file name-------------------------------------------------------------------------------*/

function formatDate(date) {
    var d = new Date(date),
        month = '' + (d.getMonth() + 1),
        day = '' + d.getDate(),
        year = d.getFullYear();

    if (month.length < 2)
        month = '0' + month;
    if (day.length < 2)
        day = '0' + day;
    return [day, month, year].join('-');
}

/*-----------------------------------End of the exporting function---------------------------*/

//Function for validating alpha numeric text with the mentioned special characters.
function isAlphaNumeric(evt,spanId) {
    var keycode = (evt.which) ? evt.which : evt.keyCode;
    if (!((keycode > 46 && keycode < 58) || (keycode > 64 && keycode < 91) || (keycode > 96 && keycode < 123) || (keycode == 45) || (keycode == 95) || (keycode == 32))) {
        var valMsg = 'Only \"/, -,_\, space" are allowed!';

        $('#' + spanId).text(valMsg);
        $('#' + spanId).css('display', 'contents');
        $('#'+spanId).css('color', 'red');
        return false;
    }
    else {
        $('#' + spanId).css('display', 'none');
        return true;
    }
}