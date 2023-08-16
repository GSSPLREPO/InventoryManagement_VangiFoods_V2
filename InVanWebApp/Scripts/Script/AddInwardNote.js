/*Shweta added 'function create_custom_dropdowns()' 22-06-2023. start*/

$(document).ready(function () {
    create_custom_dropdowns_PO_Id();
});


function create_custom_dropdowns_PO_Id() {
    $('select#PO_Id').each(function (i, select) {

        if (!$(this).next().hasClass('dropdown-select')) {

            $('#PO_Id').removeClass('form-control');
            $(this).after('<div id="divPO_Id" class="dropdown-select wide d-flex align-items-center' + ($(this).attr('class') || '') + '" tabindex="0"><span class="current"></span><div class="list"><ul></ul></div></div>');
            var dropdown = $(this).next();
            var options = $(select).find('option');
            var selected = $(this).find('option:selected');
            dropdown.find('.current').html(selected.data('display-text') || selected.text());
            options.each(function (j, o) {
                var display = $(o).data('display-text') || '';
                dropdown.find('ul').append('<li class="option ' + ($(o).is(':selected') ? 'selected' : '') + '" data-value="' + $(o).val() + '" data-display-text="' + display + '">' + $(o).text() + '</li>');
            });
        }
    });
    $('#divPO_Id.dropdown-select ul').before('<div class="dd-search"><input id="txtSearchValuePO_Id" autocomplete="off" onkeyup="filterPO_Id()" class="dd-searchbox" type="text" placeholder="Search for list" ><br />&nbsp;<span id="faSearch"><i class="fas fa-search"></i></span></div>');
}
function filterPO_Id() {
    var valThis = $('#txtSearchValuePO_Id').val();
    $('.dropdown-select ul > li').each(function () {
        var text = $(this).text();
        (text.toLowerCase().indexOf(valThis.toLowerCase()) > -1) ? $(this).show() : $(this).hide();
    });
};

// Event listeners

// Open/close
$(document).on('click', '.dropdown-select', function (event) {
    if ($(event.target).hasClass('dd-searchbox')) {
        return;
    }
    $('.dropdown-select').not($(this)).removeClass('open');
    $(this).toggleClass('open');
    if ($(this).hasClass('open')) {
        $(this).find('.option').attr('tabindex', 0);
        $(this).find('.selected').focus();
    } else {
        $(this).find('.option').removeAttr('tabindex');
        $(this).focus();
    }
});

// Close when clicking outside
$(document).on('click', function (event) {
    if ($(event.target).closest('.dropdown-select').length === 0) {
        $('.dropdown-select').removeClass('open');
        $('.dropdown-select .option').removeAttr('tabindex');
    }
    event.stopPropagation();
});
// Option click
$(document).on('click', '.dropdown-select .option', function (event) {
    $(this).closest('.list').find('.selected').removeClass('selected');
    $(this).addClass('selected');
    var text = $(this).data('display-text') || $(this).text();
    $(this).closest('.dropdown-select').find('.current').text(text);
    $(this).closest('.dropdown-select').prev('select').val($(this).data('value')).trigger('change');
});

// Keyboard events
$(document).on('keydown', '.dropdown-select', function (event) {
    var focused_option = $($(this).find('.list .option:focus')[0] || $(this).find('.list .option.selected')[0]);
    // Space or Enter
    //if (event.keyCode == 32 || event.keyCode == 13) {
    if (event.keyCode == 13) {
        if ($(this).hasClass('open')) {
            focused_option.trigger('click');
        } else {
            $(this).trigger('click');
        }
        return false;
        // Down
    } else if (event.keyCode == 40) {
        if (!$(this).hasClass('open')) {
            $(this).trigger('click');
        } else {
            focused_option.next().focus();
        }
        return false;
        // Up
    } else if (event.keyCode == 38) {
        if (!$(this).hasClass('open')) {
            $(this).trigger('click');
        } else {
            var focused_option = $($(this).find('.list .option:focus')[0] || $(this).find('.list .option.selected')[0]);
            focused_option.prev().focus();
        }
        return false;
        // Esc
    } else if (event.keyCode == 27) {
        if ($(this).hasClass('open')) {
            $(this).trigger('click');
        }
        return false;
    }
});

//<script type="text/javascript">

var InwardQuantities = "";
var BalanceQuantities = "";

//===========This function will create a json format of the item details
function createJson() {
    var table = document.getElementById('ItemTable');
    var rowCount = table.rows.length;
    var i = 1, flag = 0;;
    InwardQuantities = "[";
    BalanceQuantities = "";
    while (i < rowCount) {
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

        //if (value == 0) {
        //    i++;
        //    continue;
        //}

        InwardQuantities = InwardQuantities + "{\"InwardQuantity\":" + value + ", \"ItemId\":" + ItemID +
            ", \"ItemUnitPrice\": " + UnitPrice + ", \"BalanceQuantity\": " + BalQty +
            ", \"Item_Name\": \"" + ItemName + "\", \"Item_Code\": \"" + ItemCode + "\",\"POQuantity\": " + POQty +
            ", \"ItemUnit\": \"" + Unit + "\", \"ItemTaxValue\": " + Tax + ", \"CurrencyName\": \"" + CurrencyName + "\"";

        if (i == (rowCount - 1)) {
            InwardQuantities = InwardQuantities + "}";
            flag = 1;
        }
        else
            InwardQuantities = InwardQuantities + "},";

        i++;
    }
    InwardQuantities = InwardQuantities + "]";

    var tempTxt = InwardQuantities.split(',]')[0];

    if (flag == 0)
        InwardQuantities = tempTxt + "]";

}

function SelectedIndexChanged(id) {
    $('#btnSave').prop('disabled', false);

    $('#ValMsgPONumber').hide();
    $('#ValMsgShippingDetails').hide();
    $('#ValMsgSupplierDetails').hide();

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
            var flag = 0;
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
                        t6.setAttribute("maxlength", "8");
                        t6.setAttribute("onkeypress", "return isNumberKey(event)");

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
                            flag = 1;
                        }
                        //t6.setAttribute("type", "number");
                        t6.setAttribute("style", "background-color: #9999994d;border-radius: 5px;");
                        cell.appendChild(t6);
                    }
                    else if (i == 7) {
                        var t7 = document.createElement("input");
                        t7.id = "txtBalanceQty" + j;
                        t7.setAttribute("disabled", "true");
                        t7.setAttribute("value", result[j].BalanceQuantity != 0 ? result[j].BalanceQuantity : '0');
                        t7.setAttribute("style", "background-color: #9999994d;border-radius: 5px;");
                        cell.appendChild(t7);
                    }
                    else if (i == 8) {
                        var t8 = document.createElement("input");
                        t8.id = "ItemID_" + j;
                        t8.setAttribute("disabled", "true");
                        t8.setAttribute("class", "d-none");
                        t8.setAttribute("value", result[j].Item_ID);
                        cell.setAttribute("class", "d-none");
                        cell.appendChild(t8);
                        //cell.innerHTML = result[j].Item_ID;
                        // cell.setAttribute("id", "ItemID_" + j);
                    }
                }

            }
            if (flag == 0) {
                $('#btnSave').prop('disabled', 'true');
                alert('The Inward is done for the selected PO!');
            }
            else
                $('#btnSave').prop('disabled', false);

        }
    });
}

function OnChangeIWQty(value, id) {debugger 
    $('#btnSave').prop("disabled", false);
    var rowNo = id.split('y')[1];
    var cell = document.getElementById("ItemQty" + rowNo);
    var temp_itemQty = cell.innerHTML.split(' ');
    cell = document.getElementById("DeliveredQty" + rowNo);
    var deliveredQty = cell.innerHTML;
    var itemQty = parseFloat(temp_itemQty[0]) - parseFloat(deliveredQty);
    value = parseFloat(value);

    if (value > itemQty) {
        alert("Delivered quantity cannot be greater then balanced quantity!");
        document.getElementById(id).focus();
        document.getElementById(id).setAttribute("class", "border border-1 border-danger");
        $('#btnSave').prop("disabled", true);
        event.preventDefault();
        return;
    }
    else {
        $('#btnSave').prop("disabled", false);
        var tempInwQty = document.getElementById("txtInwardQty" + rowNo).value;
        if (tempInwQty == '' || tempInwQty == null) {
            //tempInwQty = 0; //Rahul commented 'tempInwQty = 0;' 08-08-23.
            alert("Delivered quantity is zero or null! Cannot create inward note as!");
            $('#btnSave').prop('disabled', true);
            event.preventDefault();
            return;
        }
        else {
            $('#btnSave').prop('disabled', false);
        }

        document.getElementById("txtBalanceQty" + rowNo).value = parseFloat(temp_itemQty[0]) - (parseFloat(deliveredQty) + parseFloat(tempInwQty));
        InwardQuantities = InwardQuantities + "txtInwardQty" + rowNo + "*" + value + ",";

        var BalQty = document.getElementById("txtBalanceQty" + rowNo).value;
        BalanceQuantities = BalanceQuantities + "txtBalanceQty" + rowNo + "*" + BalQty + ",";
        document.getElementById(id).removeAttribute("class", "border border-1 border-danger");
    }
    document.getElementById(id).setAttribute("style", "background-color: #9999994d;border-radius: 5px;");
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

    //==================Set value in txtItemDetails onCick of Save/Update button======--------

    var tableLength = document.getElementById('ItemTable').rows.length;
    var flag = 0, i = 1;

    if (tableLength > 1) {
        while (i < tableLength) {
            var PhyQty = document.getElementById("txtInwardQty" + i).value;
            var DelvQty = document.getElementById("DeliveredQty" + i).innerHTML;
            var POQty = document.getElementById("ItemQty" + i).innerHTML; debugger
            var BalQty = document.getElementById("txtBalanceQty" + i).value; ///Rahul added 'txtBalanceQty' 08-08-23 start.
            var PhyQtyId = $("#txtInwardQty" + i);
            if (BalQty != 0) {
                alert("Delivered quantity cannot be greater then balanced quantity!");
                document.getElementById(PhyQtyId[0].id).focus();
                document.getElementById(PhyQtyId[0].id).setAttribute("class", "border border-1 border-danger");
                $('#btnSave').prop("disabled", true);
                event.preventDefault();
                i++;
                flag = 0;
                //continue;
                //return;
            }            
            
            ///Rahul added 'txtBalanceQty' 08-08-23 start.
            DelvQty = DelvQty.split(' ')[0];
            DelvQty = parseFloat(DelvQty);

            POQty = POQty.split(' ')[0];
            POQty = parseFloat(POQty);

            PhyQty = (PhyQty == '' || PhyQty == null) ? 0 : PhyQty;
            PhyQty = parseFloat(PhyQty);

            //if (PhyQty == 0) {
            //    if (DelvQty != POQty) {
            //        alert("Delivered quantity is zero or null! Cannot create inward note!");
            //        $('#btnSave').prop('disabled', true);
            //        return;
            //    }
            //    else
            //        flag = 0;
            //}

            if (PhyQty != 0) {
                flag = 1;
                break;
            }
            i++;

        }
        if (flag != 1) {
            alert("Delivered quantity is zero or null! Cannot create inward note as!");
            $('#btnSave').prop('disabled', true);
            return;
        }
        else
            $('#btnSave').prop('disabled', false);
    }

    createJson();
    $('#InwardQuantities').val(InwardQuantities);
}

function isAlphaNumericKey(evt) {
    var keycode = (evt.which) ? evt.which : evt.keyCode;
    if (!((keycode > 46 && keycode < 58) || (keycode > 64 && keycode < 91) || (keycode > 96 && keycode < 123) || (keycode == 45) || (keycode == 95))) {
        $('#ValChallanNo').text('Only \"/, _, -\" are allowed!');
        $('#ValChallanNo').css('display', 'contents');

        return false;
    }
    else {
        $('#ValChallanNo').css('display', 'none');
        return true;
    }
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


//</script>