//===============On change function for purpose of issuing the note=============//
function OnChangePurpose(id) {
    var purpose = $('#' + id).val();
    $('.dvProduction').hide();
    $('.dvQC').hide();
    $('.dvDispatch').hide();
    $('.dvOthers').hide();

    if (purpose == 'Production')
        $('.dvProduction').show();
    else if (purpose == 'QC')
        $('.dvQC').show();
    else if (purpose == 'Dispatch')
        $('.dvDispatch').show();
    else if (purpose == 'Others')
        $('.dvOthers').show();
}
//===================end============================//

//==================Set value in txtItemDetails onCick of Save/Update button======--------
function SaveBtnClick() {
    var tableLength = document.getElementById('submissionTable').rows.length;

    //======Check whether the pupose number is written or not=======//
    var Purpose = $('#Purpose').val();
    if (Purpose == 'Production') {
        var tempNo = $('#WorkOrderNumber').val();
        if (tempNo == '' || tempNo == null) {
            $('#valMsgWorkOrder').text('Enter the work order number!');
            $('#valMsgWorkOrder').css('display', 'contents');
            $('#valMsgWorkOrder').css('color', 'red');
            event.preventDefault();
            return;
        }
        else
            $('#valMsgWorkOrder').css('display', 'none');
    }
    else if (Purpose == 'QC') {
        var tempNo = $('#QCNumber').val();
        if (tempNo == '' || tempNo == null) {
            $('#valMsgQC').text('Enter the QC number!');
            $('#valMsgQC').css('display', 'contents');
            $('#valMsgQC').css('color', 'red');
            event.preventDefault();
            return;
        }
        else
            $('#valMsgQC').css('display', 'none');
    }
    else if (Purpose == 'Dispatch') {
        var tempNo = $('#SONumber').val();
        if (tempNo == '' || tempNo == null) {
            $('#valMsgSONo').text('Enter the sales order number!');
            $('#valMsgSONo').css('display', 'contents');
            $('#valMsgSONo').css('color', 'red');
            event.preventDefault();
            return;
        }
        else
            $('#valMsgSONo').css('display', 'none');
    }
    else if (Purpose == 'Others') {
        var tempNo = $('#OtherPurpose').val();
        if (tempNo == '' || tempNo == null) {
            $('#valMsgOther').text('Enter the other document number!');
            $('#valMsgOther').css('display', 'contents');
            $('#valMsgOther').css('color', 'red');
            event.preventDefault();
            return;
        }
        else
            $('#valMsgOther').css('display', 'none');
    }

    var flag = 0, i = 0;
    if (tableLength > 1) {
        while (i < tableLength - 1) {
            var PhyQty = document.getElementById("txtIssuedQty_" + i).value; //Change this with Requested qty
            PhyQty = parseFloat(PhyQty);
            var AvlQty = document.getElementById("AvailableStock_" + i).innerHTML;
            AvlQty = parseFloat(AvlQty);

            if (PhyQty != 0) {
                flag = 1;
                var Comments = document.getElementById("txtRemarks_" + i).value;
                var IssueQty = document.getElementById("txtIssuedQty_" + i).value;
                IssueQty = parseFloat(IssueQty);

                if (Comments == '' || Comments == null) {
                    $('#spanRemark_' + i).text('Comment is mandatory!');
                    document.getElementById('spanRemark_' + i).setAttribute('style', 'color:red;');
                    document.getElementById("txtRemarks_" + i).focus();

                    event.preventDefault();
                    return;
                }
                else if (IssueQty > AvlQty) {
                    document.getElementById("txtIssuedQty_" + i).value = 0;
                    $('#spanIssueQty_' + i).text('');
                    document.getElementById("txtIssuedQty_" + i).focus();
                    event.preventDefault();
                    return;
                }
                document.getElementById("txtRemarks_" + i).setAttribute("required", "required");
            }
            $('#spanRemark_' + i).text('');

            i++;
        }

        if (flag != 1) {
            alert("No item issued, Cannot create issue note!");
            $('#btnSave').prop('disabled', true);
            return;
        }
        else
            $('#btnSave').prop('disabled', false);
    }

    var LocationName = $("#LocationId option:selected").text();
    $('#LocationName').val(LocationName);

    var IssueByName = $("#IssueBy option:selected").text();
    $('#IssueByName').val(IssueByName);
    createJson();
};
//==========end===============

//=====================Onchange of Location===========================
function SelectedIndexChangedLocation(id) {
    $('#ddlItem').val(0);
    $("#ddlItem option").remove();
    $('#btnSave').prop('disabled', false);

    //For deleting the rows of Item table if exist.

    var table = document.getElementById('submissionTable');
    var rowCount = table.rows.length;
    while (rowCount != '1') {
        var row = table.deleteRow(rowCount - 1);
        rowCount--;
    }

    var Location_ID = $("#LocationId").val();

    ///This call is for binding the item list on that location.

    $.ajax({
        url: '/StockAdjustment/GetItemList',
        type: "POST",
        data: { id: Location_ID },
        success: function (result) {
            var i = 1;
            $("#ddlItem").append($("<option></option>").val(result[0].ID).html("--Select--"));
            while (i < result.length) {
                $("#ddlItem").append($("<option></option>").val(result[i].ID).html(result[i].Item_Name));
                i++;
            }
        },
        error: function (err) {
            alert('Not able to fetch item list of that warehouse!');

        }
    });

    ///This call is for binding item details of that location
    $.ajax({
        url: '/StockAdjustment/GetLocationStocksDetails',
        type: "POST",
        data: { id: Location_ID },
        success: function (result) {

            var table = document.getElementById('submissionTable');
            for (var j = 0; j < result.length; j++) {
                var rowCount = table.rows.length;
                var cellCount = table.rows[0].cells.length;
                var row = table.insertRow(rowCount);
                if (j % 2 != 0) {
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
                        cell.innerHTML = result[j].ItemId;
                        cell.setAttribute("class", "d-none");
                        cell.setAttribute("id", "ItemID_" + j);
                    }
                    else if (i == 2) {
                        cell.innerHTML = result[j].Item_Name;
                        cell.setAttribute("id", "ItemName_" + j);

                    }
                    else if (i == 3) {
                        cell.innerHTML = result[j].ItemUnitPrice;
                        cell.setAttribute("id", "ItemUnitPrice_" + j);
                    }
                    else if (i == 4) {
                        cell.innerHTML = result[j].CurrencyName;
                        cell.setAttribute("id", "CurrencyName_" + j);
                    }
                    else if (i == 5) {
                        cell.innerHTML = result[j].AvailableStock;
                        cell.setAttribute("id", "AvailableStock_" + j);
                    }
                    else if (i == 6) {

                        cell.innerHTML = result[j].ItemUnit;
                        cell.setAttribute("id", "ItemUnit_" + j);
                    }
                    else if (i == 7) {
                        var t5 = document.createElement("input");
                        t5.id = "txtRequestedQty_" + j;
                        t5.setAttribute("value", "0");
                        t5.setAttribute("type", "text");
                        t5.setAttribute("maxlength", "8");
                        t5.setAttribute("onkeypress", "return isNumberKey(event)");
                        t5.setAttribute("class", "form-control form-control-sm");
                        cell.appendChild(t5);
                    }
                    else if (i == 8) {
                        var t5 = document.createElement("input");
                        t5.id = "txtIssuedQty_" + j;
                        t5.setAttribute("value", "0");
                        t5.setAttribute("onchange", "OnChangeQty($(this).val(),id)");
                        t5.setAttribute("maxlength", "8");
                        t5.setAttribute("onkeypress", "return isNumberKey(event)");
                        t5.setAttribute("class", "form-control form-control-sm");
                        cell.appendChild(t5);
                        var t6 = document.createElement('span');
                        t6.id = "spanIssueQty_" + j;
                        t6.setAttribute("class", "text-wrap");
                        cell.appendChild(t6);
                    }
                    else if (i == 9) {
                        var t5 = document.createElement("input");
                        t5.id = "txtFinalStock_" + j;
                        t5.setAttribute("readonly", "readonly");
                        t5.setAttribute("value", "0");
                        t5.setAttribute("type", "number");
                        t5.setAttribute("class", "form-control form-control-sm");
                        cell.appendChild(t5);
                    }
                    else if (i == 10) {
                        var t5 = document.createElement("input");
                        t5.id = "txtRemarks_" + j;
                        t5.setAttribute("maxlength", "90");
                        t5.setAttribute("style", "width:auto;");
                        t5.setAttribute("class", "form-control form-control-sm");
                        t5.setAttribute("onkeyup", "OnChangeComment($(this).val(),id)");
                        cell.appendChild(t5);
                        var t6 = document.createElement('span');
                        t6.id = "spanRemark_" + j;
                        cell.appendChild(t6);
                    }
                }

            }
        },
        error: function (err) {
            alert('Not able to fetch item details!');

        }

    });

}
//=============End==============

//=====================Onchange of Item===========================
function SelectedIndexChangedItem(id) {

    $('#btnSave').prop('disabled', false);

    //For deleting the rows of Item table if exist.

    var table = document.getElementById('submissionTable');
    var rowCount = table.rows.length;
    while (rowCount != '1') {
        var row = table.deleteRow(rowCount - 1);
        rowCount--;
    }

    var Location_ID = $("#LocationId").val();
    var Item_ID = $('#' + id).val();
    ///This call is for binding item details of that location
    $.ajax({
        url: '/StockAdjustment/GetLocationStocksDetails',
        type: "POST",
        data: { id: Location_ID, itemId: Item_ID },
        success: function (result) {
            var table = document.getElementById('submissionTable');
            for (var j = 0; j < result.length; j++) {
                var rowCount = table.rows.length;
                var cellCount = table.rows[0].cells.length;
                var row = table.insertRow(rowCount);
                if (j % 2 != 0) {
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
                        cell.innerHTML = result[j].ItemId;
                        cell.setAttribute("class", "d-none");
                        cell.setAttribute("id", "ItemID_" + j);
                    }
                    else if (i == 2) {
                        cell.innerHTML = result[j].Item_Name;
                        cell.setAttribute("id", "ItemName_" + j);

                    }
                    else if (i == 3) {
                        cell.innerHTML = result[j].ItemUnitPrice;
                        cell.setAttribute("id", "ItemUnitPrice_" + j);
                    }
                    else if (i == 4) {
                        cell.innerHTML = result[j].CurrencyName;
                        cell.setAttribute("id", "CurrencyName_" + j);
                    }
                    else if (i == 5) {
                        cell.innerHTML = result[j].AvailableStock;
                        cell.setAttribute("id", "AvailableStock_" + j);
                    }
                    else if (i == 6) {

                        cell.innerHTML = result[j].ItemUnit;
                        cell.setAttribute("id", "ItemUnit_" + j);
                    }
                    else if (i == 7) {
                        var t5 = document.createElement("input");
                        t5.id = "txtRequestedQty_" + j;
                        t5.setAttribute("value", "0");
                        t5.setAttribute("type", "text");
                        t5.setAttribute("maxlength", "8");
                        t5.setAttribute("onkeypress", "return isNumberKey(event)");
                        t5.setAttribute("class", "form-control form-control-sm");
                        cell.appendChild(t5);
                    }
                    else if (i == 8) {
                        var t5 = document.createElement("input");
                        t5.id = "txtIssuedQty_" + j;
                        t5.setAttribute("value", "0");
                        t5.setAttribute("maxlength", "8");
                        t5.setAttribute("type", "text");
                        t5.setAttribute("onkeypress", "return isNumberKey(event)");
                        t5.setAttribute("onchange", "OnChangeQty($(this).val(),id)");
                        t5.setAttribute("class", "form-control form-control-sm");
                        cell.appendChild(t5);
                        var t6 = document.createElement('span');
                        t6.id = "spanIssueQty_" + j;
                        t6.setAttribute("class", "text-wrap");
                        cell.appendChild(t6);
                    }
                    else if (i == 9) {
                        var t5 = document.createElement("input");
                        t5.id = "txtFinalStock_" + j;
                        t5.setAttribute("readonly", "readonly");
                        t5.setAttribute("value", "0");
                        t5.setAttribute("type", "number");
                        t5.setAttribute("class", "form-control form-control-sm");
                        cell.appendChild(t5);
                    }
                    else if (i == 10) {
                        var t5 = document.createElement("input");
                        t5.id = "txtRemarks_" + j;
                        t5.setAttribute("maxlength", "90");
                        t5.setAttribute("style", "width:auto;");
                        t5.setAttribute("class", "form-control form-control-sm");
                        t5.setAttribute("onkeyup", "OnChangeComment($(this).val(),id)");
                        cell.appendChild(t5);
                        var t6 = document.createElement('span');
                        t6.id = "spanRemark_" + j;
                        cell.appendChild(t6);
                    }
                }

            }
        },
        error: function (err) {
            alert('Not able to fetch item details!');

        }

    });

}
//=============End==============

function OnChangeComment(value, id) {
    var rowNo = id.split('_')[1];
    $('#spanRemark_' + rowNo).text('');

}

function OnChangeQty(value, id) {
    $('#btnSave').prop('disabled', false);
    $('#spanIssueQty_' + rowNo).text('');
    var rowNo = id.split('_')[1];
    var RequestedQty = document.getElementById("txtRequestedQty_" + rowNo).value;
    if (RequestedQty == 0 || RequestedQty == '') {
        document.getElementById("txtRequestedQty_" + rowNo).focus();
        document.getElementById('txtIssuedQty_' + rowNo).value = 0;
        return;
    }

    var avalQty = document.getElementById("AvailableStock_" + rowNo).innerHTML;
    var IssueValue = parseFloat(value);
    avalQty = parseFloat(avalQty);
    RequestedQty = parseFloat(RequestedQty);

    var DiffQty = 0;

    if (IssueValue > avalQty) {
        $('#spanIssueQty_' + rowNo).text('You cannot issue more than available stock!');
        document.getElementById('spanIssueQty_' + rowNo).setAttribute('style', 'color:red;');
        document.getElementById(id).focus();
        return;
    }
    else if (IssueValue > RequestedQty) {
        $('#spanIssueQty_' + rowNo).text('You cannot issue more than requested quantity!');
        document.getElementById('spanIssueQty_' + rowNo).setAttribute('style', 'color:red;');
        document.getElementById(id).focus();
        return;
    }
    else {
        $('#spanIssueQty_' + rowNo).text('');
        DiffQty = avalQty - IssueValue;
        DiffQty = parseFloat(DiffQty);
        document.getElementById("txtFinalStock_" + rowNo).value = DiffQty;
    }
    document.getElementById("txtRemarks_" + rowNo).focus();

    var remarks = $('#txtRemarks_' + rowNo).val();
    if (remarks == '' && remarks == null) {
        $('#spanRemark_' + rowNo).text('Comment is mandatory!');
        document.getElementById('spanRemark_' + rowNo).setAttribute('style', 'color:red;');
    }
    else
        $('#spanRemark_' + rowNo).text('');

}

var TxtItemDetails = "";

function createJson() {
    var table = document.getElementById('submissionTable');
    var rowCount = table.rows.length;
    var i = 0, flag = 0;
    TxtItemDetails = "[";
    while (i < rowCount - 1) {
        var ItemCode = (document.getElementById("ItemCode_" + i)).innerHTML;
        var ItemID = (document.getElementById("ItemID_" + i)).innerHTML;
        var ItemName = (document.getElementById("ItemName_" + i)).innerHTML;
        var PricePerUnit = (document.getElementById("ItemUnitPrice_" + i)).innerHTML;
        var CurrencyName = (document.getElementById("CurrencyName_" + i)).innerHTML;
        var AvailableStock = (document.getElementById("AvailableStock_" + i)).innerHTML;
        var Unit = (document.getElementById("ItemUnit_" + i)).innerHTML;

        var ReqQty = (document.getElementById("txtRequestedQty_" + i)).value;
        var IssueQty = (document.getElementById("txtIssuedQty_" + i)).value;
        var DiffQty = (document.getElementById("txtFinalStock_" + i)).value;

        var Remarks = (document.getElementById("txtRemarks_" + i)).value;

        if (IssueQty == 0) {
            i++;
            continue;
        }

        TxtItemDetails = TxtItemDetails + "{\"Item_Code\":\"" + ItemCode + "\", \"ItemId\":" + ItemID +
            ", \"ItemName\": \"" + ItemName + "\", \"ItemUnitPrice\": " + PricePerUnit +
            ", \"CurrencyName\": \"" + CurrencyName + "\", \"AvlStock\": " + AvailableStock + ", \"ItemUnit\": \"" + Unit +
            "\", \"ReqQty\": " + ReqQty + ",\"FinalQty\": " + DiffQty + ", \"IssueQty\": " + IssueQty +
            ",\"Remarks\": \"" + Remarks + "\"";

        if (i == (rowCount - 1)) {
            TxtItemDetails = TxtItemDetails + "}";
            flag = 1;
        }
        else
            TxtItemDetails = TxtItemDetails + "},";

        i++;
    }
    TxtItemDetails = TxtItemDetails + "]"
    var tempTxt = TxtItemDetails.split(',]')[0];

    if (flag == 0)
        TxtItemDetails = tempTxt + "]";

    $('#txtItemDetails').val(TxtItemDetails);

    //alert(TxtItemDetails);
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

//function isNumberKey(evt, id) {
//    //var len = $('#' + id).val().length;
//    //len = parseFloat(len);
//    //if (len > 8)
//    //    return false;
//    //else
//    //    return true;

//    var keycode = (evt.which) ? evt.which : evt.keyCode;
//    if (!(keycode == 8 || keycode == 46) && (keycode < 48 || keycode > 57)) {
//        return false;
//    }
//    else {
//        var parts = evt.srcElement.value.split('.');
//        if (parts.length > 1 && keycode == 46)
//            return false;
//        else
//            return true;

//    }
//    return true;
//}

function isAlphaNumeric(evt, id) {
    var keycode = (evt.which) ? evt.which : evt.keyCode;
    if (!((keycode > 46 && keycode < 58) || (keycode > 64 && keycode < 91) || (keycode > 96 && keycode < 123) || (keycode == 45) || (keycode == 95))) {
        var valMsg = 'Only \"/, -,_\" are allowed!';

        if (id == 1) {
            $('#valMsgWorkOrder').text(valMsg);
            $('#valMsgWorkOrder').css('display', 'contents');
            $('#valMsgWorkOrder').css('color', 'red');
        }
        else if (id == 2) {
            $('#valMsgQC').text(valMsg);
            $('#valMsgQC').css('display', 'contents');
            $('#valMsgQC').css('color', 'red');
        }
        else if (id == 3) {
            $('#valMsgSONo').text(valMsg);
            $('#valMsgSONo').css('display', 'contents');
            $('#valMsgSONo').css('color', 'red');
        }
        else if (id == 4) {
            $('#valMsgOther').text(valMsg);
            $('#valMsgOther').css('display', 'contents');
            $('#valMsgOther').css('color', 'red');
        }

        return false;
    }
    else {
        if (id == 1)
            $('#valMsgWorkOrder').css('display', 'none');

        else if (id == 2)
            $('#valMsgQC').css('display', 'none');

        else if (id == 3)
            $('#valMsgSONo').css('display', 'none');

        else if (id == 4)
            $('#valMsgOther').css('display', 'none');

        return true;

    }
}