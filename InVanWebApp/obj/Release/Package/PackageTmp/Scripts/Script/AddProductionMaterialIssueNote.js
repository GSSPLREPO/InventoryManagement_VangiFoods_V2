
//==================Set value in txtItemDetails onCick of Save/Update button======--------
function SaveBtnClick() {
    var tableLength = document.getElementById('submissionTable').rows.length;

    var flag = 0, i = 0;
    if (tableLength > 1) {
        while (i < tableLength - 1) {
            var IssuingQty = document.getElementById("IssuingQty_" + i).value;
            var IssuedQty = document.getElementById("IssuedQty_" + i).innerHTML;
            var RequestedQty = document.getElementById("RequestedQty_" + i).innerHTML;
            var AvailableStock = document.getElementById("AvailableStock_" + i).innerHTML;


            if (AvailableStock == null || AvailableStock == '')
                AvailableStock = 0;
            else
                AvailableStock = parseFloat(AvailableStock);

            if (RequestedQty == null || RequestedQty == '')
                RequestedQty = 0;
            else
                RequestedQty = parseFloat(RequestedQty);

            if (IssuedQty == null || IssuedQty == '')
                IssuedQty = 0;
            else
                IssuedQty = parseFloat(IssuedQty);


            if (IssuingQty == null || IssuingQty == '')
                IssuingQty = 0;
            else
                IssuingQty = parseFloat(IssuingQty);

            if (IssuingQty == 0) {
                if (IssuedQty == RequestedQty) {
                    flag = 0;
                }
                else if (AvailableStock == 0) {
                    flag = 0;
                }
                else {
                    flag = 1;
                    break;
                }
            }

            i++;
        }

        if (flag == 1) {
            alert("No item issued, Cannot create production material issue note!");
            $('#btnSave').prop('disabled', true);
            event.preventDefault();
            return;
        }
        else
            $('#btnSave').prop('disabled', false);
    }

    createJson();
};
//==========end===============

//=====================Onchange of Production indent===========================
function SelectedIndexChangedPI() {
    $('#btnSave').prop('disabled', false);

    //For deleting the rows of Item table if exist.

    var table = document.getElementById('submissionTable');
    var rowCount = table.rows.length;
    while (rowCount != '1') {
        var row = table.deleteRow(rowCount - 1);
        rowCount--;
    }

    var Location_ID = $("#LocationId").val();
    if (Location_ID == null || Location_ID == '' || Location_ID == 0) {
        $('#LocationId').focus();
        document.getElementById('ProductionIndentId').selectedIndex = 0;
        return;
    }
    else {
        $('#LocationName').val($("#LocationId option:selected").text());
        document.getElementById('LocationId').setAttribute('readonly', 'readonly');
    }

    var ProductionIndentId = $("#ProductionIndentId").val();
    $('#ProductionIndentNo').val($("#ProductionIndentId option:selected").text());


    ///This call is for binding item details on change of production indent.
    $.ajax({
        url: '/ProductionMaterialIssueNote/GetLocationStocksDetails?id=' + ProductionIndentId + '&locationId=' + Location_ID,
        type: "POST",
        data: {},
        success: function (result) {

            var table = document.getElementById('submissionTable');
            $('#WorkOrderNumber').val(result[0].WorkOrderNo);

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
                        cell.innerHTML = result[j].ItemCode;
                        cell.setAttribute("id", "ItemCode_" + j);
                    }
                    else if (i == 1) {
                        cell.innerHTML = result[j].ItemId;
                        cell.setAttribute("class", "d-none");
                        cell.setAttribute("id", "ItemID_" + j);
                    }
                    else if (i == 2) {
                        cell.innerHTML = result[j].ItemName;
                        cell.setAttribute("id", "ItemName_" + j);
                    }
                    else if (i == 3) {
                        cell.innerHTML = result[j].RequestedQty;
                        cell.setAttribute("id", "RequestedQty_" + j);
                    }
                    else if (i == 4) {
                        cell.innerHTML = result[j].IssuedQty;
                        cell.setAttribute("id", "IssuedQty_" + j);
                    }
                    else if (i == 5) {
                        var IssuedQty = parseFloat(result[j].IssuedQty);
                        var RequestedQty = parseFloat(result[j].RequestedQty);

                        var t5 = document.createElement("input");
                        t5.id = "IssuingQty_" + j;
                        t5.setAttribute("value", result[j].IssuingQty);
                        t5.setAttribute("type", "text");
                        t5.setAttribute("maxlength", "8");
                        t5.setAttribute("onkeypress", "return isNumberKey(event)")
                        t5.setAttribute("onchange", "OnChangeQty($(this).val(),id)")
                        t5.setAttribute("class", "form-control form-control-sm");
                        if (IssuedQty == RequestedQty) {
                            t5.setAttribute("readonly", "readonly");
                        }

                        cell.appendChild(t5);

                        var t6 = document.createElement('span');
                        t6.id = "spanMsg_" + j;
                        t6.setAttribute("class", "text-wrap");
                        cell.appendChild(t6);
                    }
                    else if (i == 6) {
                        cell.innerHTML = result[j].BalanceQty;
                        cell.setAttribute("id", "BalanceQty_" + j);
                    }
                    else if (i == 7) {
                        cell.innerHTML = result[j].AvailableStock;
                        cell.setAttribute("id", "AvailableStock_" + j);
                    }
                    else if (i == 8) {
                        cell.innerHTML = result[j].ItemUnit;
                        cell.setAttribute("id", "ItemUnit_" + j);
                    }
                    else if (i == 9) {
                        cell.innerHTML = result[j].ItemUnitPrice;
                        cell.setAttribute("id", "ItemUnitPrice_" + j);
                    }
                    else if (i == 10) {
                        cell.innerHTML = result[j].CurrencyName;
                        cell.setAttribute("id", "CurrencyName_" + j);
                    }
                    else if (i == 11) {
                        cell.innerHTML = result[j].FinalStock;
                        cell.setAttribute("id", "FinalStock_" + j);
                    }

                    else if (i == 12) {
                        cell.innerHTML = result[j].BalanceQty;
                        cell.setAttribute("id", "ActualBalanceQty_" + j);
                        cell.setAttribute("class", "d-none");
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

function OnChangeQty(value, id) {
    /* $('#btnSave').prop('disabled', false);*/

    var rowNo = id.split('_')[1];
    if (value == '' || value == null)
        value = 0;
    else
        value = parseFloat(value);

    $('#spanMsg_' + rowNo).text('');
    var RequestedQty = document.getElementById("RequestedQty_" + rowNo).innerHTML;
    RequestedQty = parseFloat(RequestedQty);

    var ActualBalanceQty = document.getElementById("ActualBalanceQty_" + rowNo).innerHTML;
    ActualBalanceQty = parseFloat(ActualBalanceQty);

    var AvailableStock = document.getElementById("AvailableStock_" + rowNo).innerHTML;
    AvailableStock = parseFloat(AvailableStock);

    var IssuedQty = document.getElementById("IssuedQty_" + rowNo).innerHTML;
    IssuedQty = parseFloat(IssuedQty);

    var DiffQty = 0, finalStockQty = 0;

    if (value > RequestedQty) {
        $('#spanMsg_' + rowNo).text('You cannot issue more than requested quantity!');
        document.getElementById('spanMsg_' + rowNo).setAttribute('style', 'color:red;');
        document.getElementById(id).focus();
        $('#btnSave').prop('disabled', true);
        return;
    }
    else if (value > AvailableStock) {
        $('#spanMsg_' + rowNo).text('You cannot issue more than available stock!');
        document.getElementById('spanMsg_' + rowNo).setAttribute('style', 'color:red;');
        document.getElementById(id).focus();
        $('#btnSave').prop('disabled', true);
        return;
    }
    else if (value > ActualBalanceQty) {
        $('#spanMsg_' + rowNo).text('You cannot issue more than balance stock!');
        document.getElementById('spanMsg_' + rowNo).setAttribute('style', 'color:red;');
        document.getElementById(id).focus();
        $('#btnSave').prop('disabled', true);
        return;
    }
    else {
        $('#spanMsg_' + rowNo).text('');
        DiffQty = (ActualBalanceQty) - value;
        DiffQty = parseFloat(DiffQty);
        if (value == 0) {
            finalStockQty = 0;
        }
        else {
            finalStockQty = AvailableStock - value;
        }
        finalStockQty = parseFloat(finalStockQty);

        document.getElementById("BalanceQty_" + rowNo).innerHTML = DiffQty;
        document.getElementById("FinalStock_" + rowNo).innerHTML = finalStockQty;

        $('#btnSave').prop('disabled', false);
    }
}

var TxtItemDetails = "";

function createJson() {
    var table = document.getElementById('submissionTable');
    var rowCount = table.rows.length;
    var i = 0;
    TxtItemDetails = "[";
    while (i < rowCount - 1) {
        var ItemCode = (document.getElementById("ItemCode_" + i)).innerHTML;
        var ItemID = (document.getElementById("ItemID_" + i)).innerHTML;
        var ItemName = (document.getElementById("ItemName_" + i)).innerHTML;
        var ReqQty = (document.getElementById("RequestedQty_" + i)).innerHTML;
        var IssueQty = (document.getElementById("IssuedQty_" + i)).innerHTML;
        var IssuingQty = (document.getElementById("IssuingQty_" + i)).value;
        IssuingQty = (IssuingQty == '' || IssuingQty == null ? 0 : IssuingQty);

        var BalanceQty = (document.getElementById("BalanceQty_" + i)).innerHTML;
        var AvailableStock = (document.getElementById("AvailableStock_" + i)).innerHTML;
        var ItemUnit = (document.getElementById("ItemUnit_" + i)).innerHTML;
        var PricePerUnit = (document.getElementById("ItemUnitPrice_" + i)).innerHTML;
        var CurrencyName = (document.getElementById("CurrencyName_" + i)).innerHTML;
        var FinalStock = (document.getElementById("FinalStock_" + i)).innerHTML;

        TxtItemDetails = TxtItemDetails + "{\"Item_Code\":\"" + ItemCode + "\", \"ItemId\":" + ItemID +
            ", \"ItemName\": \"" + ItemName + "\", \"ReqQty\": " + ReqQty + ", \"IssueQty\": " + IssueQty +
            ", \"IssuingQty\": " + IssuingQty + ", \"BalanceQty\": " + BalanceQty + ", \"AvlStock\": " + AvailableStock +
            ", \"ItemUnit\": \"" + ItemUnit + "\", \"PricePerUnit\": " + PricePerUnit + ", \"CurrencyName\": \"" + CurrencyName +
            "\",\"FinalQty\": " + FinalStock;

        if (i == (rowCount - 2)) {
            TxtItemDetails = TxtItemDetails + "}";
        }
        else
            TxtItemDetails = TxtItemDetails + "},";

        i++;
    }
    TxtItemDetails = TxtItemDetails + "]";

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