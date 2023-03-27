
//==================Set value in txtItemDetails onCick of Save/Update button======--------
function SaveBtnClick() {
    var tableLength = document.getElementById('submissionTable').rows.length;

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
                        var t5 = document.createElement("input");
                        t5.id = "IssuingQty_" + j;
                        t5.setAttribute("value", result[j].IssuingQty);
                        t5.setAttribute("type", "text");
                        t5.setAttribute("maxlength", "8");
                        t5.setAttribute("onkeypress", "return isNumberKey(event)")
                        t5.setAttribute("onchange", "OnChangeQty($(this).val(),id)")
                        t5.setAttribute("class", "form-control form-control-sm");
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
                    //else if (i == 11) {
                    //    var t5 = document.createElement("input");
                    //    t5.id = "txtRequestedQty_" + j;
                    //    t5.setAttribute("value", "0");
                    //    t5.setAttribute("type", "text");
                    //    t5.setAttribute("maxlength", "8");
                    //    t5.setAttribute("onkeypress", "return isNumberKey(event)");
                    //    t5.setAttribute("class", "form-control form-control-sm");
                    //    cell.appendChild(t5);
                    //}
                    //else if (i == 8) {
                    //    var t5 = document.createElement("input");
                    //    t5.id = "txtIssuedQty_" + j;
                    //    t5.setAttribute("value", "0");
                    //    t5.setAttribute("onchange", "OnChangeQty($(this).val(),id)");
                    //    t5.setAttribute("maxlength", "8");
                    //    t5.setAttribute("onkeypress", "return isNumberKey(event)");
                    //    t5.setAttribute("class", "form-control form-control-sm");
                    //    cell.appendChild(t5);
                    //    var t6 = document.createElement('span');
                    //    t6.id = "spanIssueQty_" + j;
                    //    t6.setAttribute("class", "text-wrap");
                    //    cell.appendChild(t6);
                    //}
                    //else if (i == 9) {
                    //    var t5 = document.createElement("input");
                    //    t5.id = "txtFinalStock_" + j;
                    //    t5.setAttribute("readonly", "readonly");
                    //    t5.setAttribute("value", "0");
                    //    t5.setAttribute("type", "number");
                    //    t5.setAttribute("class", "form-control form-control-sm");
                    //    cell.appendChild(t5);
                    //}

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
    $('#btnSave').prop('disabled', false);

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
        return;
    }
    else if (value > AvailableStock) {
        $('#spanMsg_' + rowNo).text('You cannot issue more than available stock!');
        document.getElementById('spanMsg_' + rowNo).setAttribute('style', 'color:red;');
        document.getElementById(id).focus();
        return;
    }
    else if (value > ActualBalanceQty) {
        $('#spanMsg_' + rowNo).text('You cannot issue more than balance stock!');
        document.getElementById('spanMsg_' + rowNo).setAttribute('style', 'color:red;');
        document.getElementById(id).focus();
        return;
    }
    else {
        $('#spanMsg_' + rowNo).text('');
        DiffQty = (RequestedQty + IssuedQty) - value;
        DiffQty = parseFloat(DiffQty);
        finalStockQty = AvailableStock - value;
        finalStockQty = parseFloat(finalStockQty);

        document.getElementById("BalanceQty_" + rowNo).innerHTML = DiffQty;
        document.getElementById("FinalStock_" + rowNo).innerHTML = finalStockQty;
    }
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