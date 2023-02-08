//==================Set value in txtItemDetails onCick of Save/Update button======--------
function SaveBtnClick() {
    var VerifiedName = $("#VerifiedBy option:selected").text();
    $("#VerifiedByName").val(VerifiedName);

    var LocaitonName = $("#LocationID option:selected").text();
    $("#LocationName").val(LocaitonName);

    var tableLength = document.getElementById('submissionTable').rows.length;
    var flag = 0, i = 1, balQtyFlag = 0;
    if (tableLength > 1) {
        var PhyQty = document.getElementById("ShippedQuantity").value;
        if (PhyQty == '')
            flag = 1;
        else
            PhyQty = parseFloat(PhyQty);

        if (PhyQty == 0) {
            flag = 1;
        }

        while (i <= tableLength - 2) {
            PhyQty = document.getElementById("shippedQuantity_" + i).value;
            if (PhyQty == '')
                flag = 1;
            else
                PhyQty = parseFloat(PhyQty);

            if (PhyQty == 0)
                flag = 1;

            i++;
        }

        if (flag == 1) {
            alert("No item shipped, Cannot create outward note!");
            $('#btnSave').prop('disabled', true);
            return;
        }
        else
            $('#btnSave').prop('disabled', false);
    }

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

$("#Item_ID").change(function () {
    $('#btnSave').prop('disabled', false);
    getitemDetails();
});

function getitemDetails() {
    $("#f").empty();
    $.ajax({
        type: 'POST',
        url: '@Url.Action("GetitemDetails","PurchaseOrder")', // we are calling json method

        dataType: 'json',

        data: { id: $("#Item_ID").val(), currencyId: '0' },
        // here we are get value of selected item and passing same value
        //as inputto json method GetitemDetails.

        success: function (itemDescription) {

            var description = JSON.stringify(itemDescription);
            $("#ItemName").val(itemDescription.Item_Name);
            $("#ItemCode").val(itemDescription.Item_Code);
            $("#ItemUnit").val(itemDescription.UnitCode);
            $("#ShippedQuantity").val('0');
        },
        failure: function () {
            alert('Failed to retrieved item detals!');
        },
        error: function (ex) {
            alert('Failed to retrieve Item description.' + ex);
        }
    });


}

function getitemDetailsJSTbl(count) {
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
        },
        failure: function () {
            alert('Failed to retrieved item detals!');
        },
        error: function (ex) {
            alert('Failed to retrieve Item description.' + ex);
        }
    });
}

var counter = 1;
$("#AddItem").click(function () {
    var itemId = "ItemID_" + counter;
    var itemCode = "itemCode_" + counter;
    var itemDescription = "itemDescription_" + counter;
    var itemQuantity = "shippedQuantity_" + counter;
    var itemUnit = "itemUnit_" + counter;
    var comment = "comments_" + counter;
    $('<tr id="tablerow' + counter + '">' + '<td>' +
        '<select id="' + itemId + '" onchange="getitemDetailsJSTbl(' + counter + ')"; class="form-control form-control-sm" data-val="true" data-val-number="The field Item_ID must be a number." name="' + itemId + ' " style="height:30px;width:auto;" aria-describedby="Item_ID-error" aria-invalid="false">' + $('#Item_ID').html() + '</select>' +
        '</td>' +
        '<td class="d-none">' +
        '<input type="text" id="' + itemCode + '" class="form-control form-control-sm" style = "height:30px;width:200px;" name="' + itemCode + '" value="" required="required" />' +
        '</td>' +
        '<td>' +
        '<input type="text" id="' + itemDescription + '" class="form-control form-control-sm" style = "height:30px;width:auto;" name="' + itemDescription + '"readonly = "readonly" value="" required="required" />' +
        '</td>' +
        '<td>' +
        '<input type="text" id="' + itemQuantity + '" class="form-control form-control-sm text-right" style = "height:30px;" name="' + itemQuantity + '" value="0" onkeypress = "return isNumberKey(event)" required="required" maxlength="8", onchange="onShippedQtyChange()" />' +
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

    counter++;
    return false;
});

function removeTr(index) {
    var length = document.getElementById("submissionTable").rows.length;
    length = parseFloat(length) - 1;
    if (counter > 1 & length == index) {
        $('#tablerow' + index).remove();
        counter--;
    }
    else {
        $('#tablerow' + index).remove();
    }
    return false;
}

var TxtItemDetails = "";

function createJson() {
    var table = document.getElementById('submissionTable');
    var rowCount = table.rows.length;
    var i = 1;
    TxtItemDetails = "[{";
    var ItemCode = (document.getElementById("ItemCode")).value;
    var ItemID = (document.getElementById("Item_ID")).value;
    var ItemName = (document.getElementById("ItemName")).value;
    var ShippedQty = (document.getElementById("ShippedQuantity")).value;
    var Unit = (document.getElementById("ItemUnit")).value;
    var Comments = document.getElementById("Comments").value;

    TxtItemDetails = TxtItemDetails + "\"Item_Code\":\"" + ItemCode + "\", \"ItemId\":" + ItemID +
        ", \"ItemName\": \"" + ItemName + "\", \"ShippedQty\": " + ShippedQty +
        ", \"ItemUnit\": \"" + Unit + "\", \"Comments\": \""
        + Comments + "\"},";

    for (i = 1; i <= rowCount - 2; i++) {
        ItemCode = (document.getElementById("itemCode_" + i)).value;
        ItemID = (document.getElementById("ItemID_" + i)).value;
        ItemName = (document.getElementById("itemDescription_" + i)).value;
        ShippedQty = (document.getElementById("shippedQuantity_" + i)).value;
        Unit = (document.getElementById("itemUnit_" + i)).value;
        Comments = document.getElementById("comments_" + i).value;

        TxtItemDetails = TxtItemDetails + "{\"Item_Code\":\"" + ItemCode + "\", \"ItemId\":" + ItemID +
            ", \"ItemName\": \"" + ItemName + "\", \"ShippedQty\": " + ShippedQty +
            ", \"ItemUnit\": \"" + Unit + "\", \"Comments\": \""
            + Comments + "\"";

        if (i == (rowCount - 2))
            TxtItemDetails = TxtItemDetails + "}";
        else
            TxtItemDetails = TxtItemDetails + "},";
    }
    TxtItemDetails = TxtItemDetails + "]"
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