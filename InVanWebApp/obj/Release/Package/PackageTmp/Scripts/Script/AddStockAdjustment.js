//==================Set value in txtItemDetails onCick of Save/Update button======--------
function SaveBtnClick() {
    var tableLength = document.getElementById('submissionTable').rows.length;
    var flag = 0, i = 0;
    if (tableLength > 1) {
        while (i < tableLength - 1) {
            var PhyQty = document.getElementById("txtPhysicalStock_" + i).value;
            PhyQty = parseFloat(PhyQty);

            if (PhyQty != 0) {
                flag = 1;
                var Comments = document.getElementById("txtRemarks_" + i).value;
                if (Comments == '' || Comments == null) {
                    $('#spanRemark_' + i).text('Comment is mandatory!');
                    document.getElementById('spanRemark_' + i).setAttribute('style', 'color:red;');
                    document.getElementById("txtRemarks_" + i).focus();

                    event.preventDefault();
                    return;
                }
                document.getElementById("txtRemarks_" + i).setAttribute("required", "required");
            }
            $('#spanRemark_' + i).text('');

            i++;
        }

        if (flag != 1) {
            alert("adjustment is not done! Cannot adjust the stock!");
            $('#btnSave').prop('disabled', true);
            return;
        }
        else
            $('#btnSave').prop('disabled', false);
    }
    var LocationName = $("#LocationId option:selected").text();
    $('#LocationName').val(LocationName);
    createJson();
    // alert(TxtItemDetails);
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
                        t5.id = "txtPhysicalStock_" + j;
                        t5.setAttribute("onchange", "OnChangeQty($(this).val(),id)");
                        t5.setAttribute("value", "0");
                        //t5.setAttribute("type", "number");
                        t5.setAttribute("onkeypress", "return isNumberKey(event,id)");
                        t5.setAttribute("class", "form-control form-control-sm");
                        cell.appendChild(t5);
                    }
                    else if (i == 8) {
                        var t5 = document.createElement("input");
                        t5.id = "txtDifference_" + j;
                        t5.setAttribute("readonly", "readonly");
                        t5.setAttribute("value", "0");
                        t5.setAttribute("class", "form-control form-control-sm");
                        cell.appendChild(t5);
                    }
                    else if (i == 9) {
                        var t5 = document.createElement("input");
                        t5.id = "txtTransferPrice_" + j;
                        t5.setAttribute("readonly", "readonly");
                        t5.setAttribute("value", "0");
                        //t5.setAttribute("type", "number");
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
                        t5.id = "txtPhysicalStock_" + j;
                        t5.setAttribute("onchange", "OnChangeQty($(this).val(),id)");
                        t5.setAttribute("value", "0");
                        //t5.setAttribute("type", "number");
                        t5.setAttribute("onkeypress", "return isNumberKey(event,id)");
                        t5.setAttribute("class", "form-control form-control-sm");
                        cell.appendChild(t5);
                    }
                    else if (i == 8) {
                        var t5 = document.createElement("input");
                        t5.id = "txtDifference_" + j;
                        t5.setAttribute("readonly", "readonly");
                        t5.setAttribute("value", "0");
                        t5.setAttribute("class", "form-control form-control-sm");
                        cell.appendChild(t5);
                    }
                    else if (i == 9) {
                        var t5 = document.createElement("input");
                        t5.id = "txtTransferPrice_" + j;
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
    var rowNo = id.split('_')[1];
    var avalQty = document.getElementById("AvailableStock_" + rowNo).innerHTML;
    value = parseFloat(value);
    avalQty = parseFloat(avalQty);

    var UnitPrice = document.getElementById("ItemUnitPrice_" + rowNo).innerHTML;
    var DiffQty = 0;

    if (value > avalQty) {
        DiffQty = value - avalQty;
        DiffQty = parseFloat(DiffQty);
        document.getElementById("txtDifference_" + rowNo).value = "+" + DiffQty;
        document.getElementById("txtDifference_" + rowNo).setAttribute("style", "color:none;");

    }
    else if (value < avalQty) {
        DiffQty = avalQty - value;
        DiffQty = parseFloat(DiffQty);
        document.getElementById("txtDifference_" + rowNo).value = "-" + DiffQty;
        document.getElementById("txtDifference_" + rowNo).setAttribute("style", "color:red;");
    }
    else {
        DiffQty = avalQty - value;
        DiffQty = parseFloat(DiffQty);
        document.getElementById("txtDifference_" + rowNo).value = DiffQty;
        document.getElementById("txtDifference_" + rowNo).setAttribute("style", "color:none;");
    }

    var TransferPrice = (UnitPrice * DiffQty);
    document.getElementById("txtTransferPrice_" + rowNo).value = TransferPrice;

    $('#spanRemark_' + rowNo).text('Comment is mandatory!');
    document.getElementById('spanRemark_' + rowNo).setAttribute('style', 'color:red;');
    document.getElementById("txtRemarks_" + rowNo).focus();
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

        var PhyQty = (document.getElementById("txtPhysicalStock_" + i)).value;
        var DiffQty = (document.getElementById("txtDifference_" + i)).value;
        var TransPrice = (document.getElementById("txtTransferPrice_" + i)).value;

        var Remarks = (document.getElementById("txtRemarks_" + i)).value;

        if (PhyQty == 0) {
            i++;
            continue;
        }

        TxtItemDetails = TxtItemDetails + "{\"Item_Code\":\"" + ItemCode + "\", \"ItemId\":" + ItemID +
            ", \"ItemName\": \"" + ItemName + "\", \"ItemUnitPrice\": " + PricePerUnit +
            ", \"CurrencyName\": \"" + CurrencyName + "\", \"AvlStock\": " + AvailableStock + ", \"ItemUnit\": \"" + Unit +
            "\", \"PhyQty\": " + PhyQty + ",\"DiffQty\": " + DiffQty +
            ", \"TransPrice\": " + TransPrice + ",\"Remarks\": \"" + Remarks + "\"";

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

    $('#TxtItemDetails').val(TxtItemDetails);
}

function isNumberKey(evt, id) {
    var len = $('#' + id).val().length;
    len = parseFloat(len);
    if (len > 8)
        return false;
    else
        return true;

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