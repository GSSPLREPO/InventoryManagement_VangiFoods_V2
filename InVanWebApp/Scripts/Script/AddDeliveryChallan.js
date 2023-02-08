﻿//==================Set value in txtItemDetails onCick of Save/Update button======--------
function SaveBtnClick() {
    var SONumber = $("#SO_Id option:selected").text();
    $("#SONumber").val(SONumber);

    var tableLength = document.getElementById('submissionTable').rows.length;
    var flag = 0, i = 1, balQtyFlag = 0;
    if (tableLength > 1) {
        while (i <= tableLength - 1) {
            var PhyQty = document.getElementById("txtShippingQty_" + i).value;
            PhyQty = parseFloat(PhyQty);

            var BalQty = document.getElementById("txtBalQty_" + i).value;
            BalQty = parseFloat(BalQty);

            if (BalQty == 0)
                balQtyFlag = 1;

            if (PhyQty != 0) {
                flag = 1;
            }

            i++;
        }

        if (balQtyFlag == 1) {
            alert("All items shipped, Cannot create its outward note!");
            $('#btnSave').prop('disabled', true);
            return;
        }
        else
            $('#btnSave').prop('disabled', false);

        if (flag != 1) {
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

//=====================Onchange of SO===========================
function SelectedIndexChangedSO(id) {

    $('#btnSave').prop('disabled', false);

    //For deleting the rows of Item table if exist.

    var table = document.getElementById('submissionTable');
    var rowCount = table.rows.length;
    while (rowCount != '1') {
        var row = table.deleteRow(rowCount - 1);
        rowCount--;
    }

    var SO_Id = $("#SO_Id").val();

    $.ajax({
        url: '/DeliveryChallan/GetSODetails',
        type: "POST",
        data: { id: SO_Id },
        success: function (result) {
            $("#SONumber").val(result[0].SONo);
            $("#CurrencyID").val(result[0].CurrencyID);
            $("#CurrencyName").val(result[0].CurrencyName);
            $("#CurrencyPrice").val(result[0].CurrencyPrice);
            $("#LocationId").val(result[0].LocationId);
            $("#LocationName").val(result[0].LocationName);
            $("#VendorsID").val(result[0].ClientID);
            $("#CompanyName").val(result[0].CompanyName);
            $('#ShippingAddress').val(result[0].DeliveryAddress);
            $('#SupplierAddress').val(result[0].SupplierAddress);
            $('#TermsAndCondition_ID').val(result[0].TermsAndConditionID);
            $('#Terms').val(result[0].Terms);

            var table = document.getElementById('submissionTable');
            for (var j = 1; j < result.length; j++) {
                var rowCount = table.rows.length;
                var cellCount = table.rows[0].cells.length;
                var row = table.insertRow(rowCount);
                if (j % 2 == 0) {
                    row.setAttribute("style", "background-color:rgba(0, 0, 0, 0.05);");
                }

                for (var i = 0; i < cellCount; i++) {
                    var cell = 'cell' + i;
                    cell = row.insertCell(i);

                    if (i == 0) {
                        cell.innerHTML = result[j].Item_Code;
                        cell.setAttribute("id", "ItemCode_" + j);
                    }
                    else if (i == 1) {
                        cell.innerHTML = result[j].Item_ID;
                        cell.setAttribute("class", "d-none");
                        cell.setAttribute("id", "ItemID_" + j);
                    }
                    else if (i == 2) {
                        cell.innerHTML = result[j].ItemName;
                        cell.setAttribute("id", "ItemName_" + j);
                    }
                    else if (i == 3) {
                        cell.innerHTML = result[j].ItemQuantity;
                        cell.setAttribute("id", "SOQty_" + j);

                    }
                    else if (i == 4) {
                        cell.innerHTML = result[j].BalanceQuantity;
                        cell.setAttribute("id", "BalQty_" + j);
                    }
                    else if (i == 5) {
                        var t5 = document.createElement("input");
                        t5.id = "txtShippingQty_" + j;
                        t5.setAttribute("value", "0");
                        t5.setAttribute("type", "number");
                        t5.setAttribute("onkeypress", "return isNumberKey(event,id)");
                        t5.setAttribute("maxlength", "8");
                        t5.setAttribute("onchange", "OnChangeQty($(this).val(),id)");
                        t5.setAttribute("class", "form-control form-control-sm");
                        if (result[j].BalanceQuantity == 0)
                            t5.setAttribute("readonly", "readonly");

                        cell.appendChild(t5);
                        var t6 = document.createElement('span');
                        t6.id = "spanShippingQty_" + j;
                        t6.setAttribute("class", "text-wrap");
                        cell.appendChild(t6);

                    }
                    else if (i == 6) {
                        cell.innerHTML = result[j].ItemUnit;
                        cell.setAttribute("id", "ItemUnit_" + j);
                    }
                    else if (i == 7) {
                        cell.innerHTML = result[j].ItemUnitPrice;
                        cell.setAttribute("id", "ItemUnitPrice_" + j);
                    }
                    else if (i == 8) {
                        cell.innerHTML = result[j].CurrencyName
                        cell.setAttribute("id", "CurrencyName_" + j);
                    }
                    else if (i == 9) {
                        cell.innerHTML = result[j].ItemTaxValue + " %";
                        cell.setAttribute("id", "ItemTaxValue_" + j);
                    }
                    else if (i == 10) {
                        var t5 = document.createElement("input");
                        t5.id = "txtTotalItemCost_" + j;
                        t5.setAttribute("class", "form-control form-control-sm text-wrap");
                        t5.setAttribute("readonly", "readonly");
                        t5.setAttribute("value", "0");
                        cell.appendChild(t5);
                    }
                    else if (i == 11) {
                        var t5 = document.createElement("input");
                        t5.id = "txtBalQty_" + j;
                        t5.setAttribute("class", "d-none");
                        t5.setAttribute("value", result[j].BalanceQuantity);
                        cell.appendChild(t5);
                        cell.setAttribute("style", "display:none;");
                    }
                }

            }

        },
        error: function (err) {
            alert('Not able to fetch SO item details!');

        }

    });
}
//=============End==============

function OnChangeQty(value, id) {
    $('#btnSave').prop('disabled', false);
    $('#spanShippingQty_' + rowNo).text('');
    var rowNo = id.split('_')[1];
    var BalQty = document.getElementById("txtBalQty_" + rowNo).value;
    BalQty = parseFloat(BalQty);
    var unitPrice = document.getElementById("ItemUnitPrice_" + rowNo).innerHTML;
    unitPrice = parseFloat(unitPrice);

    var SOQty = document.getElementById("SOQty_" + rowNo).innerHTML;
    SOQty = parseFloat(SOQty);
    if (value == '')
        value = 0;

    var OutwardQty = parseFloat(value);

    var DiffQty = 0;

    if (OutwardQty > BalQty) {
        $('#spanShippingQty_' + rowNo).text('Shipped quantity cannot be greater than balance quantity!');
        document.getElementById('spanShippingQty_' + rowNo).setAttribute('style', 'color:red;');
        document.getElementById(id).focus();
        return;
    }
    else {
        $('#spanShippingQty_' + rowNo).text('');
        DiffQty = BalQty - OutwardQty;
        DiffQty = parseFloat(DiffQty);
        document.getElementById("BalQty_" + rowNo).innerHTML = DiffQty;

        document.getElementById("txtTotalItemCost_" + rowNo).value = Math.round(OutwardQty * unitPrice);

    }
    CalculateTotalBeforeTax();

}

function CalculateTotalBeforeTax() {
    $('#TotalBeforeTax').val('');
    $('#TotalTax').val('');
    var length = document.getElementById("submissionTable").rows.length;
    length = parseFloat(length) - 1;
    var total = 0;
    var totalTax = 0;
    var OtherTax = document.getElementById("OtherTax").value;

    if (OtherTax == '')
        OtherTax = 0;

    OtherTax = parseFloat(OtherTax);
    var i = 1;
    while (i <= length) {
        var temp = document.getElementById("txtTotalItemCost_" + i).value;
        var tempTotalTax = ((document.getElementById("ItemTaxValue_" + i)).innerHTML).split(" %")[0];
        total = parseFloat(temp) + total;

        tempTotalTax = (parseFloat(tempTotalTax) / 100) * parseFloat(temp);
        totalTax = parseFloat(tempTotalTax) + totalTax;

        i++;
    }

    $('#TotalBeforeTax').val(total.toFixed(2));
    $('#TotalTax').val(totalTax.toFixed(2));
    var tempGrandTotal = total + totalTax + OtherTax;
    tempGrandTotal = Math.round(tempGrandTotal);
    $('#TotalAfterTax').val(tempGrandTotal);
    $('#GrandTotal').val(tempGrandTotal);

}

var TxtItemDetails = "";

function createJson() {

    var table = document.getElementById('submissionTable');
    var rowCount = table.rows.length;
    var i = 1;
    TxtItemDetails = "[";
    for (i = 1; i <= rowCount - 1; i++) {
        var ItemCode = (document.getElementById("ItemCode_" + i)).innerHTML;
        var ItemID = (document.getElementById("ItemID_" + i)).innerHTML;
        var ItemName = (document.getElementById("ItemName_" + i)).innerHTML;
        var SOQty = (document.getElementById("SOQty_" + i)).innerHTML;
        var BalQty = (document.getElementById("BalQty_" + i)).innerHTML;
        var Unit = (document.getElementById("ItemUnit_" + i)).innerHTML;
        var OutwardQty = document.getElementById("txtShippingQty_" + i).value;

        var PricePerUnit = (document.getElementById("ItemUnitPrice_" + i)).innerHTML;
        PricePerUnit = (PricePerUnit == null || PricePerUnit == '') ? 0 : PricePerUnit;

        var CurrencyName = (document.getElementById("CurrencyName_" + i)).innerHTML;
        var Tax = (document.getElementById("ItemTaxValue_" + i)).innerHTML.split(" %")[0];

        var TotalItemCost = (document.getElementById("txtTotalItemCost_" + i)).value;
        TotalItemCost = (TotalItemCost == null || TotalItemCost == '') ? 0 : TotalItemCost;

        TxtItemDetails = TxtItemDetails + "{\"Item_Code\":\"" + ItemCode + "\", \"ItemId\":" + ItemID +
            ", \"ItemName\": \"" + ItemName + "\", \"SOQty\": " + SOQty + ", \"BalQty\": " + BalQty +
            ", \"OutwardQty\": " + OutwardQty +
            ", \"ItemUnit\": \"" + Unit + "\", \"ItemUnitPrice\": " + PricePerUnit + ", \"CurrencyName\": \""
            + CurrencyName + "\",\"ItemTaxValue\": " + Tax + ", \"TotalItemCost\": " + TotalItemCost;

        if (i == (rowCount - 1))
            TxtItemDetails = TxtItemDetails + "}";
        else
            TxtItemDetails = TxtItemDetails + "},";
    }
    TxtItemDetails = TxtItemDetails + "]"
    $('#txtItemDetails').val(TxtItemDetails);
}

function isNumberKey(evt, id) {
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