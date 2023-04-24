//==================Set value in txtItemDetails onCick of Save/Update button======--------
function SaveBtnClick() {
    var PONumber = $("#PO_ID option:selected").text();
    $("#PO_Number").val(PONumber);

    createJson();
};
//==========end===============

//=====================Onchange of PO===========================
//function SelectedIndexChangedPO(id) {

//    $('#btnSave').prop('disabled', false);

//    //For deleting the rows of Item table if exist.

//    var table = document.getElementById('submissionTable');
//    var rowCount = table.rows.length;
//    while (rowCount != '1') {
//        var row = table.deleteRow(rowCount - 1);
//        rowCount--;
//    }

//    var PO_Id = $("#PO_ID").val();

//    $.ajax({
//        url: '/CreditNote/GetPODetails',
//        type: "POST",
//        data: { id: PO_Id },
//        success: function (result) {
//            $("#PO_Number").val(result[0].PONumber);
//            $("#CurrencyID").val(result[0].CurrencyID);
//            $("#CurrencyName").val(result[0].CurrencyName);
//            $("#CurrencyPrice").val(result[0].CurrencyPrice);
//            $("#LocationId").val(result[0].LocationId);
//            $("#LocationName").val(result[0].LocationName);
//            $("#VendorID").val(result[0].VendorsID);
//            $("#VendorName").val(result[0].CompanyName);
//            $('#DeliveryAddress').val(result[0].DeliveryAddress);
//            $('#VendorAddress').val(result[0].SupplierAddress);

//            $('#TermsAndConditionID').val(result[0].TermsAndConditionID);
//            $('#Terms').val(result[0].Terms);
//            $('#OtherTax').val((result[0].OtherTax));

//            var table = document.getElementById('submissionTable');
//            for (var j = 1; j < result.length; j++) {
//                var rowCount = table.rows.length;
//                var cellCount = table.rows[0].cells.length;
//                var row = table.insertRow(rowCount);

//                for (var i = 0; i < cellCount; i++) {
//                    var cell = 'cell' + i;
//                    cell = row.insertCell(i);

//                    if (i == 0) {
//                        cell.innerHTML = result[j].Item_Code;
//                        cell.setAttribute("id", "ItemCode_" + j);
//                    }
//                    else if (i == 1) {
//                        cell.innerHTML = result[j].Item_ID;
//                        cell.setAttribute("class", "d-none");
//                        cell.setAttribute("id", "ItemID_" + j);
//                    }
//                    else if (i == 2) {
//                        cell.innerHTML = result[j].ItemName;
//                        cell.setAttribute("id", "ItemName_" + j);

//                    }
//                    else if (i == 3) {
//                        cell.innerHTML = result[j].ItemQuantity + " " + result[j].ItemUnit;
//                        cell.setAttribute("id", "POQty_" + j);

//                    }
//                    else if (i == 4) {
//                        cell.innerHTML = result[j].RejectedQuantity;
//                        cell.setAttribute("id", "RejectedQuantity_" + j);
//                    }
//                    else if (i == 5) {
//                        cell.innerHTML = result[j].ItemUnit;
//                        cell.setAttribute("id", "ItemUnit_" + j);
//                    }
//                    else if (i == 6) {
//                        cell.innerHTML = result[j].ItemUnitPrice + " " + result[j].CurrencyName;
//                        cell.setAttribute("id", "ItemUnitPrice_" + j);
//                    }
//                    else if (i == 7) {
//                        cell.innerHTML = result[j].CurrencyName
//                        cell.setAttribute("id", "CurrencyName_" + j);
//                    }
//                    else if (i == 8) {
//                        cell.innerHTML = result[j].ItemTaxValue + " %";
//                        cell.setAttribute("id", "ItemTax_" + j);
//                    }
//                    else if (i == 9) {
//                        cell.innerHTML = result[j].TotalItemCost + " " + result[j].CurrencyName;
//                        cell.setAttribute("id", "TotalItemCost_" + j);
//                    }
//                    else if (i == 10) {
//                        cell.innerHTML = result[j].Remarks;
//                        cell.setAttribute("id", "Remarks_" + j);
//                    }
//                }

//            }

//            CalculateTotalBeforeTax();
//            var grandTotal = $('#GrandTotal').val()
//            grandTotal = parseFloat(grandTotal);
//            if (grandTotal == 0) {
//                alert('Wastage of all listed items are zero, Cannot create its credit note!');
//                $('#btnSave').prop('disabled', true);
//            }
//            else {
//                $('#btnSave').prop('disabled', false);
//            }
//        },
//        error: function (err) {
//            alert('Not able to fetch indent item details!');

//        }

//    });
//}
//=============End==============

//var TxtItemDetails = "";

//function CalculateTotalBeforeTax() {
//    $('#TotalBeforeTax').val('');
//    $('#TotalTax').val('');
//    var length = document.getElementById("submissionTable").rows.length;
//    length = parseFloat(length) - 1;
//    var total = 0;
//    var totalTax = 0;
//    var otherTax = ((document.getElementById("OtherTax").value).split(" %")[0]);
//    otherTax = parseFloat(otherTax);
//    totalTax = Math.round(totalTax);

//    var i = 1;
//    while (i <= length) {
//        var temp = ((document.getElementById("TotalItemCost_" + i)).innerHTML).split(" ")[0];
//        var tempTotalTax = ((document.getElementById("ItemTax_" + i)).innerHTML).split(" %")[0];
//        total = parseFloat(temp) + total;

//        tempTotalTax = (parseFloat(tempTotalTax) / 100) * parseFloat(temp);
//        totalTax = parseFloat(tempTotalTax) + totalTax;

//        i++;
//    }
//    otherTax = ((otherTax / 100) * total);
//    totalTax = totalTax + otherTax;

//    $('#TotalBeforeTax').val(total.toFixed(2));
//    $('#TotalTax').val(totalTax.toFixed(2));
//    var tempGrandTotal = total + totalTax;
//    $('#GrandTotal').val(tempGrandTotal.toFixed(2));

//    createJson();
//}

//function createJson() {

//    var table = document.getElementById('submissionTable');
//    var rowCount = table.rows.length;
//    var i = 1;
//    TxtItemDetails = "[";
//    for (i = 1; i <= rowCount - 1; i++) {
//        var ItemCode = (document.getElementById("ItemCode_" + i)).innerHTML;
//        var ItemID = (document.getElementById("ItemID_" + i)).innerHTML;
//        var ItemName = (document.getElementById("ItemName_" + i)).innerHTML;
//        var POQty = (document.getElementById("POQty_" + i)).innerHTML.split(" ")[0];
//        var RejectedQty = (document.getElementById("RejectedQuantity_" + i)).innerHTML;
//        var Unit = (document.getElementById("ItemUnit_" + i)).innerHTML;

//        var PricePerUnit = (document.getElementById("ItemUnitPrice_" + i)).innerHTML.split(" ")[0];
//        PricePerUnit = (PricePerUnit == null || PricePerUnit == '') ? 0 : PricePerUnit;

//        var CurrencyName = (document.getElementById("CurrencyName_" + i)).innerHTML;
//        var Tax = (document.getElementById("ItemTax_" + i)).innerHTML.split(" ")[0];
//        var TotalItemCost = (document.getElementById("TotalItemCost_" + i)).innerHTML.split(" ")[0];
//        TotalItemCost = (TotalItemCost == null || TotalItemCost == '') ? 0 : TotalItemCost;
//        var Remarks = (document.getElementById("Remarks_" + i)).innerHTML;

//        TxtItemDetails = TxtItemDetails + "{\"Item_Code\":\"" + ItemCode + "\", \"ItemId\":" + ItemID +
//            ", \"ItemName\": \"" + ItemName + "\", \"POQty\": " + POQty + ", \"RejectedQty\": " + RejectedQty +
//            ", \"ItemUnit\": \"" + Unit + "\", \"ItemUnitPrice\": " + PricePerUnit + ", \"CurrencyName\": \"" + CurrencyName + "\",\"ItemTaxValue\": " + Tax +
//            ", \"TotalItemCost\": " + TotalItemCost +",\"Remarks\": \""+Remarks+"\"";

//        if (i == (rowCount - 1))
//            TxtItemDetails = TxtItemDetails + "}";
//        else
//            TxtItemDetails = TxtItemDetails + "},";
//    }
//    TxtItemDetails = TxtItemDetails + "]"
//    $('#TxtItemDetails').val(TxtItemDetails);
//}

//=====================Onchange of SO===========================
function SelectedIndexChangedPO(id) {

    $('#btnSave').prop('disabled', false);

    //For deleting the rows of Item table if exist.

    var table = document.getElementById('submissionTable');
    var rowCount = table.rows.length;
    while (rowCount != '1') {
        var row = table.deleteRow(rowCount - 1);
        rowCount--;
    }

    var SO_Id = $("#PO_ID").val();
    /*Rename the GetPODetails to GetSODetails*/
    $.ajax({
        url: '/CreditNote/GetSODetails',
        type: "POST",
        data: { id: SO_Id },
        success: function (result) {
            $("#SO_Number").val(result[0].SONumber);
            $("#CurrencyID").val(result[0].CurrencyID);
            $("#CurrencyName").val(result[0].CurrencyName);
            $("#CurrencyPrice").val(result[0].CurrencyPrice);
            $("#LocationId").val(result[0].LocationId);
            $("#LocationName").val(result[0].LocationName);
            $("#VendorID").val(result[0].VendorsID);
            $("#VendorName").val(result[0].CompanyName);
            $('#DeliveryAddress').val(result[0].DeliveryAddress);
            $('#VendorAddress').val(result[0].SupplierAddress);

            $('#TermsAndConditionID').val(result[0].TermsAndConditionID);
            $('#Terms').val(result[0].Terms);
            $('#OtherTax').val((result[0].OtherTax));

            var table = document.getElementById('submissionTable');
            for (var j = 1; j < result.length; j++) {
                var rowCount = table.rows.length;
                var cellCount = table.rows[0].cells.length;
                var row = table.insertRow(rowCount);

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
                        cell.innerHTML = result[j].ItemQuantity; /*+ " " + result[j].ItemUnit*/
                        cell.setAttribute("id", "POQty_" + j);

                    }
                    else if (i == 4) {
                        //cell.text = result[j].RejectedQuantity;
                        //cell.setAttribute("id", "RejectedQuantity_" + j);
                        var t4 = document.createElement("input");
                        t4.id = "RejectedQuantity_" + j;
                        t4.setAttribute("class", "form-control form-control-sm");
                        //t4.setAttribute("type", "text");
                        t4.setAttribute("onchange", "OnChangeWasteQty($(this).val(),id)");
                        cell.appendChild(t4);
                        var t6 = document.createElement('span');
                        t6.id = "spanRemark_" + j;
                        t6.setAttribute("style", "white-space: break-space !important;");
                        cell.appendChild(t6);


                    }
                    else if (i == 5) {
                        cell.innerHTML = result[j].ItemUnit;
                        cell.setAttribute("id", "ItemUnit_" + j);
                    }
                    else if (i == 6) {
                        cell.innerHTML = result[j].ItemUnitPrice + " " + result[j].CurrencyName;
                        cell.setAttribute("id", "ItemUnitPrice_" + j);
                    }
                    else if (i == 7) {
                        cell.innerHTML = result[j].CurrencyName
                        cell.setAttribute("id", "CurrencyName_" + j);
                    }
                    else if (i == 8) {
                        cell.innerHTML = result[j].ItemTaxValue + " %";
                        cell.setAttribute("id", "ItemTax_" + j);
                    }
                    else if (i == 9) {
                        cell.innerHTML = result[j].TotalItemCost + " " + result[j].CurrencyName;
                        cell.setAttribute("id", "TotalItemCost_" + j);
                    }
                    //else if (i == 10) {
                    //    cell.innerHTML = result[j].Remarks;
                    //    cell.setAttribute("id", "Remarks_" + j);
                    //}
                }

            }

            CalculateTotalBeforeTax();
            var grandTotal = $('#GrandTotal').val()
            grandTotal = parseFloat(grandTotal);
            if (grandTotal == 0) {
                alert('Wastage of all listed items are zero, Cannot create its credit note!');
                $('#btnSave').prop('disabled', true);
            }
            else {
                $('#btnSave').prop('disabled', false);
            }
        },
        error: function (err) {
            alert('Not able to fetch indent item details!');

        }

    });
}
//=============End==============

var TxtItemDetails = "";

function CalculateTotalBeforeTax() {
    $('#TotalBeforeTax').val('');
    $('#TotalTax').val('');
    var length = document.getElementById("submissionTable").rows.length;
    length = parseFloat(length) - 1;
    var total = 0;
    var totalTax = 0;
    var otherTax = ((document.getElementById("OtherTax").value).split(" %")[0]);
    otherTax = parseFloat(otherTax);
    totalTax = Math.round(totalTax);

    var i = 1;
    while (i <= length) {
        var temp = ((document.getElementById("TotalItemCost_" + i)).innerHTML).split(" ")[0];
        var tempTotalTax = ((document.getElementById("ItemTax_" + i)).innerHTML).split(" %")[0];
        total = parseFloat(temp) + total;

        tempTotalTax = (parseFloat(tempTotalTax) / 100) * parseFloat(temp);
        totalTax = parseFloat(tempTotalTax) + totalTax;

        i++;
    }
    otherTax = ((otherTax / 100) * total);
    totalTax = totalTax + otherTax;

    $('#TotalBeforeTax').val(total.toFixed(2));
    $('#TotalTax').val(totalTax.toFixed(2));
    var tempGrandTotal = total + totalTax;
    $('#GrandTotal').val(tempGrandTotal.toFixed(2));

    createJson();
}

function OnChangeWasteQty(value, id) {

    $('#btnSave').prop("disabled", false);
    var rowNo = id.split('_')[1];
    var cell = document.getElementById("POQty_" + rowNo);
    var tempPOQty_ = cell.innerHTML.split(' ');
    value = parseFloat(value);

    if (value > tempPOQty_) {
        $('#spanRemark_' + rowNo).text('Wastage quantity cannot be greater then SO quantity!');
        document.getElementById('spanRemark_' + rowNo).setAttribute('style', 'color:red;');
        document.getElementById("txtRemarks_" + rowNo).focus();
        $('#btnSave').prop("disabled", true);
        return;
    }

    else if (value <= 0) {
        $('#spanRemark_' + rowNo).text('Wastage quantity can not be zero!');
        document.getElementById('spanRemark_' + rowNo).setAttribute('style', 'color:red;');
        document.getElementById("txtRemarks_" + rowNo).focus();
        $('#btnSave').prop("disabled", true);
        return;
    }
    else {
        $('#btnSave').prop("disabled", false);
        $('#spanRemark_' + rowNo).hide();
    }
    document.getElementById(id).setAttribute("style", "background-color: #9999994d;border-radius: 5px;");
}

function createJson() {

    var table = document.getElementById('submissionTable');
    var rowCount = table.rows.length;
    var i = 1;
    TxtItemDetails = "[";
    for (i = 1; i <= rowCount - 1; i++) {
        var ItemCode = (document.getElementById("ItemCode_" + i)).innerHTML;
        var ItemID = (document.getElementById("ItemID_" + i)).innerHTML;
        var ItemName = (document.getElementById("ItemName_" + i)).innerHTML;
        var POQty = (document.getElementById("POQty_" + i)).innerHTML.split(" ")[0];
        var RejectedQty = (document.getElementById("RejectedQuantity_" + i)).value;
        var Unit = (document.getElementById("ItemUnit_" + i)).innerHTML;

        var PricePerUnit = (document.getElementById("ItemUnitPrice_" + i)).innerHTML.split(" ")[0];
        PricePerUnit = (PricePerUnit == null || PricePerUnit == '') ? 0 : PricePerUnit;

        var CurrencyName = (document.getElementById("CurrencyName_" + i)).innerHTML;
        var Tax = (document.getElementById("ItemTax_" + i)).innerHTML.split(" ")[0];
        var TotalItemCost = (document.getElementById("TotalItemCost_" + i)).innerHTML.split(" ")[0];
        TotalItemCost = (TotalItemCost == null || TotalItemCost == '') ? 0 : TotalItemCost;
        // var Remarks = (document.getElementById("Remarks_" + i)).innerHTML;

        TxtItemDetails = TxtItemDetails + "{\"Item_Code\":\"" + ItemCode + "\", \"ItemId\":" + ItemID +
            ", \"ItemName\": \"" + ItemName + "\", \"POQty\": " + POQty + ", \"RejectedQty\": " + RejectedQty +
            ", \"ItemUnit\": \"" + Unit + "\", \"ItemUnitPrice\": " + PricePerUnit + ", \"CurrencyName\": \"" + CurrencyName + "\",\"ItemTaxValue\": " + Tax +
            ", \"TotalItemCost\": " + TotalItemCost; /*+",\"Remarks\": \"" + Remarks + "\""*/

        if (i == (rowCount - 1))
            TxtItemDetails = TxtItemDetails + "}";
        else
            TxtItemDetails = TxtItemDetails + "},";
    }
    TxtItemDetails = TxtItemDetails + "]"
    $('#TxtItemDetails').val(TxtItemDetails);
}