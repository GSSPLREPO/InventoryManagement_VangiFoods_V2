//=======The below function is for fetching the files value in the textbox of file type============//
$(document).ready(function () {
    var fileName = $('#tempSign').val();

    //Get a reference to our file input
    const fileInput = document.querySelector('input[type="file"]');
    var filePath = '/Signatures/' + fileName;

    // Create a new File object
    const myFile = new File([filePath], fileName, {
        type: 'image/jpeg/jpg/png'
    });

    // Now let's create a DataTransfer to get a FileList
    const dataTransfer = new DataTransfer();
    dataTransfer.items.add(myFile);
    fileInput.files = dataTransfer.files;
});
//===================end=======================//

//==================Set value in txtItemDetails onCick of Save/Update button======--------
function SaveBtnClick() {
    var VerifiedName = $("#VerifiedBy option:selected").text();
    $("#VerifiedByName").val(VerifiedName);

    var LocaitonName = $("#LocationID option:selected").text();
    $("#LocationName").val(LocaitonName);

    var flag = 0;
    PhyQty = document.getElementById("shippedQuantity_1").value;
    if (PhyQty == '')
        flag = 1;
    else
        PhyQty = parseFloat(PhyQty);

    if (PhyQty == 0) {
        flag = 1;
    }

    if (flag == 1) {
        alert("No item shipped, Cannot create outward note!");
        $('#btnSave').prop('disabled', true);
        return;
    }
    else
        $('#btnSave').prop('disabled', false);

    createJson();
};
//==========end===============

function onShippedQtyChange() {
    $('#btnSave').prop('disabled', false);
}

//==========================On location change==================================
function SelectedIndexChangedLocation(id) {
    $('#btnSave').prop('disabled', false);

    var selectedOptionLocationName, locationName;
    selectedOptionLocationName = document.getElementById("LocationID").selectedIndex;
    locationName = document.getElementById("LocationID").options[selectedOptionLocationName].innerText;
    document.getElementById("LocationName").value = locationName;

    $.ajax({
        type: "POST",
        url: `/PurchaseOrder/BindLocationMaster?id=` + id,
        data: "{}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            ////debugger;
            //console.log(result);
            var deliveryAdd = result[0].DeliveryAddress;
            $("#LocationAddress").val(deliveryAdd);
        }
    });
}
//===========================end=================================

function getitemDetailsJSTbl(count) {
    $('#btnSave').prop('disabled', false);
    var itemId = "ItemID_" + count;
    var selectedItemId = $("#" + itemId).val();
    $.ajax({
        type: 'POST',
        url: '@Url.Action("GetitemDetails","PurchaseOrder")', // we are calling json method

        dataType: 'json',
        data: { id: selectedItemId, currencyId: '0' },

        success: function (itemDescription) {
            var description = JSON.stringify(itemDescription);
            $("#itemCode_" + count).val(itemDescription.Item_Code);
            $("#itemDescription_" + count).val(itemDescription.Item_Name);
            $("#itemUnit_" + count).val(itemDescription.UnitCode);
            $("#shippedQuantity_" + count).val('0');
            $("#comments_" + count).val('');
        },
        failure: function () {
            alert('Failed to retrieved item detals!');
        },
        error: function (ex) {
            alert('Failed to retrieve Item description.' + ex);
        }
    });
}

///Finding the length of existing table
var table = document.getElementById('submissionTable');
var counter = table.rows.length;

$("#AddItem").click(function () {
    var itemId = "ItemID_" + counter;
    var itemCode = "itemCode_" + counter;
    var itemDescription = "itemDescription_" + counter;
    var itemQuantity = "shippedQuantity_" + counter;
    var itemUnit = "itemUnit_" + counter;
    var comment = "comments_" + counter;
    $('<tr id="tablerow_' + counter + '">' + '<td>' +
        '<select id="' + itemId + '" onchange="getitemDetailsJSTbl(' + counter + ')"; class="form-control form-control-sm" data-val="true" data-val-number="The field Item_ID must be a number." name="' + itemId + ' " style="height:30px;width:auto;" aria-describedby="Item_ID-error" aria-invalid="false">' + $('#ItemId_1').html() + '</select>' +
        '</td>' +
        '<td class="d-none">' +
        '<input type="text" id="' + itemCode + '" class="form-control form-control-sm" style = "height:30px;width:200px;" name="' + itemCode + '" value="" required="required" />' +
        '</td>' +
        '<td>' +
        '<input type="text" id="' + itemDescription + '" class="form-control form-control-sm" style = "height:30px;width:auto;" name="' + itemDescription + '"readonly = "readonly" value="" required="required" />' +
        '</td>' +
        '<td>' +
        '<input type="text" id="' + itemQuantity + '" class="form-control form-control-sm" style = "height:30px;" name="' + itemQuantity + '" value="0" onkeypress = "return isNumberKey(event)" required="required" maxlength="8", onchange="onShippedQtyChange()" />' +
        '</td>' +
        '<td>' +
        '<input type="text" id="' + itemUnit + '" class="form-control form-control-sm" style = "height:30px;width:auto;" name="' + itemUnit + '"readonly = "readonly" value="" required="required" />' +
        '</td>' +
        '<td>' +
        '<input type="text" id="' + comment + '" class="form-control form-control-sm" style = "height:30px;width:auto;" name="' + comment + '"value="" maxLength = "95" />' +
        '</td>' +
        '<td>' +
        '<button type="button" class="btn btn-sm btn-primary" style="background: linear-gradient(85deg, #392c70, #6a005b);" onclick="removeTr(' + counter + ');">Delete</button>' +
        '</td>' +
        '</tr>').appendTo('#submissionTable');

    /////////////This is piece of code is for de-selecting the Item list in the dropdown of newly created/added row//////////////////////
    document.getElementById('ItemID_' + counter).selectedIndex = 0;
    /////////////////////end.../////////////////

    counter++;
    return false;
});

function removeTr(index) {
    /////Finding the length of existing table

    var length = document.getElementById("submissionTable").rows.length;
    length = parseFloat(length) - 1;
    //console.log("Length: " + length + counter);
    if (counter > 1 & length == index) {
        $('#tablerow_' + index).remove();
        counter--;
    }
    else {
        $('#tablerow_' + index).remove();
    }
    //CalculateTotalBeforeTax();
    return false;
}

var TxtItemDetails = "";

function createJson() {
    let res = [...document.getElementById("myTableBody").children].map(tr =>
        Object.fromEntries([...tr.querySelectorAll("input,select")].map(el =>
            [el.name, el.value])));
    console.log(res);
    var TxtItemDetails = JSON.stringify(res);
    console.log(TxtItemDetails);
    $('#txtItemDetails').val(TxtItemDetails);

}

function isNumberKey(evt) {
    var keycode = (evt.which) ? evt.which : evt.keyCode;
    if (!(keycode == 8 || keycode == 46) && (keycode < 48 || keycode > 57)) {
        return false;
    }
    else {
        var parts = evt.srcElement.value.split('.');
        if (parts.length > 1 && keycode == 46)
            return false;
        else
            return true;
    }
    return true;
}

function fileValidation() {
    var fileInput =
        document.getElementById('file');

    var filePath = fileInput.value;

    // Allowing file type
    var allowedExtensions =
        /(\.jpg|\.jpeg|\.png)$/i;

    if (!allowedExtensions.exec(filePath)) {
        alert('Invalid file type');
        fileInput.value = '';
        return false;
    }
    else {

        // Image preview
        if (fileInput.files && fileInput.files[0]) {
            var reader = new FileReader();
            reader.onload = function (e) {
                document.getElementById(
                    'imagePreview').innerHTML =
                    '<img src="' + e.target.result
                    + '" style="height:10%;width:20%" />';
            };

            reader.readAsDataURL(fileInput.files[0]);
        }
    }
}

function isAlphaNumeric(evt) {
    var keycode = (evt.which) ? evt.which : evt.keyCode;
    if (!((keycode > 46 && keycode < 58) || (keycode > 64 && keycode < 91) || (keycode > 96 && keycode < 123) || (keycode == 45) || (keycode == 95) || (keycode == 32))) {
        var valMsg = 'Only \"/, -,_\" are allowed!';

        $('#valDocketNumber').text(valMsg);
        $('#valDocketNumber').css('display', 'contents');
        $('#valDocketNumber').css('color', 'red');
        return false;
    }
    else {
        $('#valDocketNumber').css('display', 'none');
        return true;
    }
}