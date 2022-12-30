
//<script type="text/javascript">

var InwardQuantities = "";
var BalanceQuantities = "";

//===========This function will create a json format of the item details
function createJson() {
    //var table = document.getElementById('ItemTable');
    //var rowCount = table.rows.length;
    //var i = 1;
    //InwardQuantities = "";
    //BalanceQuantities ="";
    //for (i = 1; i < rowCount; i++) {
    //    var value = $('#txtInwardQty' + i).val();
    //    var BalQty = $('#txtBalanceQty' + i).val();
    //    var UnitPrice = $('#UnitPrice_' + i).val();
    //    var ItemID = $('#ItemID_' + i).val();
    //    InwardQuantities = InwardQuantities + "txtInwardQty" + i + "*" + value + ",";
    //    BalanceQuantities = BalanceQuantities + "txtBalanceQty" + i + "*" + BalQty + ",";
    //    alert(InwardQuantities);
    //    //JSON.stringify(res)
    //}

    var table = document.getElementById('ItemTable');
    var rowCount = table.rows.length;
    var i = 1;
    InwardQuantities = "[";
    BalanceQuantities = "";
    for (i = 1; i < rowCount; i++) {
        var ItemName = $('#ItemName_' + i).val();
        var ItemCode = $('#ItemCode_' + i).val();
        var cellData = document.getElementById("ItemQty" + i);
        var POQty = cellData.innerHTML.split(' ');
        var Unit = POQty[1]; POQty = POQty[0];
        var Tax = $('#ItemTaxValue' + i).val(); Tax = Tax.split(" "); Tax = Tax[0];
        var value = $('#txtInwardQty' + i).val();
        if (value == null || value == '')
            value = 0;
        var BalQty = $('#txtBalanceQty' + i).val();
        var UnitPrice = $('#UnitPrice_' + i).val(); UnitPrice = UnitPrice.split(" ");
        var CurrencyName = UnitPrice[1];
        UnitPrice = UnitPrice[0];
        var ItemID = $('#ItemID_' + i).val();
        InwardQuantities = InwardQuantities + "{\"InwardQuantity\":" + value + ", \"ItemId\":" + ItemID +
            ", \"ItemUnitPrice\": " + UnitPrice + ", \"BalanceQuantity\": " + BalQty +
            ", \"Item_Name\": \"" + ItemName + "\", \"Item_Code\": \"" + ItemCode + "\",\"POQuantity\": " + POQty +
            ", \"ItemUnit\": \"" + Unit + "\", \"ItemTaxValue\": " + Tax + ", \"CurrencyName\": \"" + CurrencyName + "\"";

        if (i == (rowCount - 1))
            InwardQuantities = InwardQuantities + "}";
        else
            InwardQuantities = InwardQuantities + "},";
    }
    InwardQuantities = InwardQuantities + "]"
}

function SelectedIndexChanged(id) {

    //For deleting the rows of Item table if exist.

    var table = document.getElementById('ItemTable');
    var rowCount = table.rows.length;
    while (rowCount != '1') {
        var row = table.deleteRow(rowCount - 1);
        rowCount--;
    }

    //Clearing the address textarea
    $('#ShippingDetails').val(' ');
    $('#SupplierDetails').val(' ');

    $.ajax({
        type: "POST",
        url: `/InwardNote/BindPODetails?id=` + id,
        data: "{ }",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            //console.log(result);
            //$('#PODate').val(result[0].PODate);
            $('#ShippingDetails').val(result[0].DeliveryAddress);
            $('#SupplierDetails').val(result[0].SupplierAddress);
            var ColCount = result.length

            //===================Create dynamic table for binding Item details====================//
            var table = document.getElementById('ItemTable');
            for (var j = 1; j < result.length; j++) {
                var rowCount = table.rows.length;
                var cellCount = table.rows[0].cells.length;
                var row = table.insertRow(rowCount);
                for (var i = 0; i < cellCount; i++) {
                    var cell = 'cell' + i;
                    cell = row.insertCell(i);
                    if (i == 0) {
                        var t0 = document.createElement("input");
                        t0.id = "ItemName_" + j;
                        t0.setAttribute("disabled", "true");
                        t0.setAttribute("style", "background:transparent;border:none;");
                        t0.setAttribute("value", result[j].ItemName);
                        cell.appendChild(t0);
                        //cell.innerHTML = result[j].ItemName;
                    }
                    else if (i == 1) {
                        var t1 = document.createElement("input");
                        t1.id = "ItemCode_" + j;
                        t1.setAttribute("disabled", "true");
                        t1.setAttribute("style", "background:transparent;border:none;");
                        t1.setAttribute("value", result[j].Item_Code);
                        cell.appendChild(t1);
                        //cell.innerHTML = result[j].Item_Code;
                    }
                    else if (i == 2) {
                        cell.innerHTML = result[j].ItemQuantity + " " + result[j].ItemUnit;
                        cell.setAttribute("id", "ItemQty" + j);
                    }
                    else if (i == 3) {
                        var t3 = document.createElement("input");
                        t3.id = "ItemTaxValue" + j;
                        t3.setAttribute("disabled", "true");
                        t3.setAttribute("style", "background:transparent;border:none;");
                        t3.setAttribute("value", result[j].ItemTaxValue + " %");
                        cell.appendChild(t3);
                        //cell.innerHTML = result[j].ItemTaxValue + " %";
                    }
                    else if (i == 4) {
                        var t4 = document.createElement("input");
                        t4.id = "UnitPrice_" + j;
                        t4.setAttribute("disabled", "true");
                        t4.setAttribute("style", "background:transparent;border:none;");
                        t4.setAttribute("value", result[j].ItemUnitPrice + " " + result[j].CurrencyName);
                        cell.appendChild(t4);

                        //cell.innerHTML = result[j].ItemUnitPrice + " " + result[j].CurrencyName;
                    }
                    else if (i == 5) {
                        cell.innerHTML = result[j].InwardQuantity + " " + result[j].ItemUnit;
                        cell.setAttribute("id", "DeliveredQty" + j);
                    }
                    else if (i == 6) {

                        var t6 = document.createElement("input");
                        t6.id = "txtInwardQty" + j;
                        t6.removeAttribute("disabled", "false");
                        t6.removeAttribute("disabled", "true");

                        var cellData = document.getElementById("ItemQty" + j);
                        var temp_itemQty = cellData.innerHTML.split(' ');
                        cellData = document.getElementById("DeliveredQty" + j);
                        var deliveredQty = cellData.innerHTML;
                        if (parseFloat(temp_itemQty[0]) == parseFloat(deliveredQty)) {
                            t6.setAttribute("disabled", "true");
                        }
                        else {
                            t6.removeAttribute("disabled", "false");
                            t6.removeAttribute("disabled", "true");
                            t6.setAttribute("onchange", "OnChangeIWQty($(this).val(),id)");
                        }
                        t6.setAttribute("type", "number");
                        cell.appendChild(t6);
                    }
                    else if (i == 7) {
                        var t7 = document.createElement("input");
                        t7.id = "txtBalanceQty" + j;
                        t7.setAttribute("disabled", "true");
                        t7.setAttribute("value", result[j].BalanceQuantity != 0 ? result[j].BalanceQuantity : '0');
                        cell.appendChild(t7);
                    }
                    else if (i == 8) {
                        var t8 = document.createElement("input");
                        t8.id = "ItemID_" + j;
                        t8.setAttribute("disabled", "true");
                        t8.setAttribute("class", "d-none");
                        t8.setAttribute("value", result[j].Item_ID);
                        cell.appendChild(t8);
                        //cell.innerHTML = result[j].Item_ID;
                        // cell.setAttribute("id", "ItemID_" + j);
                    }
                }

            }
        }
    });
}

function OnChangeIWQty(value, id) {

    var rowNo = id.split('y')[1];
    var cell = document.getElementById("ItemQty" + rowNo);
    var temp_itemQty = cell.innerHTML.split(' ');
    cell = document.getElementById("DeliveredQty" + rowNo);
    var deliveredQty = cell.innerHTML;
    //console.log(deliveredQty);
    var itemQty = parseFloat(temp_itemQty[0]) - parseFloat(deliveredQty);
    value = parseFloat(value);

    if (value > itemQty) {
        alert("Inwarding quantity cannot be greater then balanced quantity!");
        document.getElementById(id).focus();
        document.getElementById(id).setAttribute("style", "border-color:red;");
        return;
    }
    else {
        var tempInwQty = document.getElementById("txtInwardQty" + rowNo).value;
        //console.log(tempInwQty + " " + temp_itemQty[0] + " " + deliveredQty);
        document.getElementById("txtBalanceQty" + rowNo).value = parseFloat(temp_itemQty[0]) - (parseFloat(deliveredQty) + parseFloat(tempInwQty));
        InwardQuantities = InwardQuantities + "txtInwardQty" + rowNo + "*" + value + ",";

        var BalQty = document.getElementById("txtBalanceQty" + rowNo).value;
        BalanceQuantities = BalanceQuantities + "txtBalanceQty" + rowNo + "*" + BalQty + ",";
        document.getElementById(id).setAttribute("style", "border-color:none;");
    }
}


function fileValidation() {
    var fileInput =
        document.getElementById('fileupload');

    var filePath = fileInput.value;

    // Allowing file type
    var allowedExtensions =
        /(\.jpg|\.jpeg|\.png)$/i;

    if (!allowedExtensions.exec(filePath)) {
        alert('Invalid file type');
        fileInput.value = '';
        return false;
    }
}

function SetInwardQty() {
    createJson();
    $('#InwardQuantities').val(InwardQuantities);
    //$('#BalanceQuantities').val(BalanceQuantities);
    //alert($('#InwardQuantities').val());
    //alert($('#BalanceQuantities').val());
}

function isAlphaNumericKey(evt) {
    var keycode = (evt.which) ? evt.which : evt.keyCode;
    if (!((keycode > 46 && keycode < 58) || (keycode > 64 && keycode < 91) || (keycode > 96 && keycode < 123) || (keycode == 45) || (keycode == 95) )) {
        $('#ValChallanNo').text('Only \"/, _, -\" are allowed!');
        $('#ValChallanNo').css('display', 'contents');

        return false;
    }
    else {
        $('#ValChallanNo').css('display', 'none');
        return true;
    }
}

//</script>