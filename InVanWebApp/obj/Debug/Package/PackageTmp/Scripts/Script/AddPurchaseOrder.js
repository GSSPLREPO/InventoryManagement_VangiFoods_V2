﻿/*Shweta added 'function create_custom_dropdowns()' 22-06-2023. start*/

$(document).ready(function () {
    create_custom_dropdowns_IndentID();
    create_custom_dropdowns_LocationId();
    create_custom_dropdowns_VendorsID();
});


function create_custom_dropdowns_IndentID() {
    $('select#IndentID').each(function (i, select) {

        if (!$(this).next().hasClass('dropdown-select')) {

            $('#IndentID').removeClass('form-control');
            $(this).after('<div id="divIndentID" class="dropdown-select wide ' + ($(this).attr('class') || '') + '" tabindex="0"><span class="current"></span><div class="list"><ul></ul></div></div>');
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
    $('#divIndentID.dropdown-select ul').before('<div class="dd-search"><input id="txtSearchValueIndentID" autocomplete="off" onkeyup="filterIndentID()" class="dd-searchbox" type="text" placeholder="Search for list" ><br />&nbsp;<span id="faSearch"><i class="fas fa-search"></i></span></div>');
}
function filterIndentID() {
    var valThis = $('#txtSearchValueIndentID').val();
    $('.dropdown-select ul > li').each(function () {
        var text = $(this).text();
        (text.toLowerCase().indexOf(valThis.toLowerCase()) > -1) ? $(this).show() : $(this).hide();
    });
};

function create_custom_dropdowns_LocationId() {
    $('select#LocationId').each(function (i, select) {

        if (!$(this).next().hasClass('dropdown-select')) {

            $('#LocationId').removeClass('form-control');
            $(this).after('<div id="divLocationId" class="dropdown-select wide ' + ($(this).attr('class') || '') + '" tabindex="0"><span class="current"></span><div class="list"><ul></ul></div></div>');
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
    $('#divLocationId.dropdown-select ul').before('<div class="dd-search"><input id="txtSearchValueLocationId" autocomplete="off" onkeyup="filterLocationId()" class="dd-searchbox" type="text" placeholder="Search for list" ><br />&nbsp;<span id="faSearch"><i class="fas fa-search"></i></span></div>');
}
function filterLocationId() {
    var valThis = $('#txtSearchValueLocationId').val();
    $('.dropdown-select ul > li').each(function () {
        var text = $(this).text();
        (text.toLowerCase().indexOf(valThis.toLowerCase()) > -1) ? $(this).show() : $(this).hide();
    });
};

function create_custom_dropdowns_VendorsID() {
    $('select#VendorsID').each(function (i, select) {

        if (!$(this).next().hasClass('dropdown-select')) {

            $('#VendorsID').removeClass('form-control');
            $(this).after('<div id="divVendorsID" class="dropdown-select wide ' + ($(this).attr('class') || '') + '" tabindex="0"><span class="current"></span><div class="list"><ul></ul></div></div>');
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
    $('#divVendorsID.dropdown-select ul').before('<div class="dd-search"><input id="txtSearchValueVendorsID" autocomplete="off" onkeyup="filterVendorsID()" class="dd-searchbox" type="text" placeholder="Search for list" ><br />&nbsp;<span id="faSearch"><i class="fas fa-search"></i></span></div>');
}
function filterVendorsID() {
    var valThis = $('#txtSearchValueVendorsID').val();
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

/*Shweta added 'function create_custom_dropdowns()' 22-06-2023. end*/

//==================Set value in txtItemDetails onCick of Save/Update button======--------
function SaveBtnClick() {
    ////Rahul added 'if (quantity == 0)' start 19-07-23.
    var tableLength = document.getElementById('submissionTable').rows.length;    
    var i = 0;
    while (i <= tableLength - 1) {
        var temp = document.getElementById("txtItemQty_" + i);
        var tempIdValue = $("#txtItemQty_" + i).val();
        if (tempIdValue == null || tempIdValue == '' || tempIdValue == "" || tempIdValue == 'undefined') {
            var tempId = temp.id;
            alert("Order quantity is zero or null! Cannot create PO!");
            $('#btnSave').prop('disabled', true);
            $('#btn_SaveDraft').prop('disabled', true);
            document.getElementById(tempId).focus();
            document.getElementById(tempId).setAttribute("style", "border-color:red;");
            event.preventDefault();

            i++;
        }
        else {
            var tempId = temp.id;
            var temp = document.getElementById("txtItemQty_" + i).value;
            $('#btnSave').prop('disabled', false);
            $('#btn_SaveDraft').prop('disabled', false);
            document.getElementById(tempId).setAttribute("style", "border-color:none;");
            i++;
        }
    }
    ////Rahul added 'if (quantity == 0)' start 19-07-23.
    var CurrencyName = $("#CurrencyID option:selected").text();
    $("#CurrencyName").val(CurrencyName);
    createJson();
};
//==========end===============

//=====================Onchange of Terms and Condition===========================
function SelectedIndexChangedTerms(id) {
    $.ajax({
        url: '/PurchaseOrder/GetTermsDescription',
        type: "POST",
        data: { id: id },
        success: function (result) {
            $('#Terms').val(result.TermDescription);
        },
        error: function (err) {
            alert('Not able to get the selected terms and condition value!');

        }
    });
}

//=============End==============

//=====================Onchange of Indent description===========================
function SelectedIndexChangedIndent(id) {
    
    //Check whether the currency dropdown is selected or not
 
    var CurrencyIDCheck = $('#CurrencyID').val();
    if (CurrencyIDCheck == '' || CurrencyIDCheck == null) {
        $('#CurrencyID').focus();
        document.getElementById('IndentID').selectedIndex = 0;
        return;
    }
    else {
        $("#tempCurrencyID").val(CurrencyIDCheck);
        $("#CurrencyID").prop("disabled", "true");
    }
        
    //For deleting the rows of Item table if exist.

    var table = document.getElementById('submissionTable');
    var rowCount = table.rows.length;
    while (rowCount != '1') {
        var row = table.deleteRow(rowCount - 1);
        rowCount--;
    }

    var IdentNumber = $("#IndentID option:selected").text();
    $('#IndentNumber').val(IdentNumber);
    var CurrencyName = $("#CurrencyID option:selected").text();

    $.ajax({
        url: '/PurchaseOrder/GetIndentDescription',
        type: "POST",
        data: { id: id, tempCurrencyId: CurrencyIDCheck },
        success: function (result) {

            var table = document.getElementById('submissionTable');
            for (var j = 0; j < result.length; j++) {
                var rowCount = table.rows.length;
                var cellCount = table.rows[0].cells.length;
                var row = table.insertRow(rowCount);
                //Rahul added ' id="tablerow"' 30-05-2023. 
                row.setAttribute("id", "tablerow" + j);

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
                        cell.innerHTML = result[j].RequiredQuantity + " " + result[j].ItemUnit;
                        cell.setAttribute("id", "RequiredQuantity_" + j);

                    }
                    else if (i == 4) {
                        var t6 = document.createElement("input");
                        t6.id = "txtItemQty_" + j;
                        t6.removeAttribute("disabled", "false");
                        t6.removeAttribute("disabled", "true");
                        t6.setAttribute("maxlength", "8");
                        t6.setAttribute("onkeypress", "return isNumberKey(event)");

                        if (parseFloat(result[j].BalanceQuantity) == 0) {
                            t6.setAttribute("disabled", "true");
                        }
                        else {
                            t6.removeAttribute("disabled", "false");
                            t6.removeAttribute("disabled", "true");
                            t6.setAttribute("onchange", "OnChangeQty($(this).val(),id)");
                        }
                        //t6.setAttribute("type", "number");
                        t6.setAttribute("class", "form-control form-control-sm");
                        cell.appendChild(t6);
                    }
                    else if (i == 5) {
                        var t4 = document.createElement("input");
                        t4.id = "BalanceQuantity_" + j;
                        t4.setAttribute("value", result[j].BalanceQuantity);
                        t4.setAttribute("readonly", "readonly");
                        t4.setAttribute("class", "form-control form-control-sm");
                        cell.appendChild(t4);

                    }
                    else if (i == 6) {
                        cell.innerHTML = result[j].ItemUnit;
                        cell.setAttribute("id", "ItemUnit_" + j);
                    }
                    else if (i == 7) {
                        var t4 = document.createElement("input");
                        t4.id = "ItemUnitPrice_" + j;
                        t4.setAttribute("value", result[j].ItemUnitPrice);
                        t4.setAttribute("class", "form-control form-control-sm");
                        t4.setAttribute("maxlength", "8");
                        t4.setAttribute("onchange", "OnChangeUnitPrice($(this).val(),id)");
                        if (parseFloat(result[j].BalanceQuantity) == 0) {
                            t4.setAttribute("disabled", "true");
                        }
                        cell.appendChild(t4);
                    }

                    else if (i == 8) {
                        cell.innerHTML = CurrencyName
                        cell.setAttribute("id", "CurrencyName_" + j);
                    }
                    else if (i == 9) {
                        cell.innerHTML = result[j].ItemTax + " %";
                        cell.setAttribute("id", "ItemTax_" + j);
                    }
                    else if (i == 10) {
                        var t4 = document.createElement("input");
                        t4.id = "TotalItemCost_" + j;
                        t4.setAttribute("class", "form-control form-control-sm");
                        t4.setAttribute("readonly", "readonly");
                        t4.setAttribute("value", 0);
                        cell.appendChild(t4);
                    }
                    else if (i == 11) {
                        cell.innerHTML = result[j].BalanceQuantity;
                        cell.setAttribute("class", "d-none");
                        cell.setAttribute("id", "ActualBalanceQuantity_" + j);
                    }
                    /*Rahul : Add Javascript 'else if (i == 12)' for 'function removeTr(index)' start on 30-05-2023.*/
                    else if (i == 12) { 
                        var t4 = document.createElement("button");
                        t4.id = "btnSave_" + j;
                        t4.setAttribute("class", "btn btn-sm btn-primary");
                        t4.setAttribute("style", "background: linear-gradient(85deg, #392c70, #6a005b);");
                        t4.setAttribute("type", "submit");
                        t4.innerHTML = "Delete";
                        t4.setAttribute("onclick", "removeTr('"+j+"');");
                        cell.appendChild(t4);
                    }
                    /*Rahul : Add Javascript 'else if (i == 12)' for 'function removeTr(index)' end on 30-05-2023.*/
                }
            }
        },
        error: function (err) {
            alert('Not able to fetch indent item details!');

        }
    });
}

//=============End==============

var TxtItemDetails = "";


function OnChangeUnitPrice(value, id) {
    var rowNo = id.split('_')[1];
    if (value == '')
        value = 0;

    var UnitPrice = parseFloat(value);
    if (UnitPrice <= 0 || value == null || value == '') {
        alert("Price cannot be negative, null or zero!");
        document.getElementById(id).focus();
        document.getElementById(id).setAttribute("style", "border-color:red;");
        return;
    }
    else {

        //Set total price in item grid.
        var quantity = $("#txtItemQty_" + rowNo).val();
        var totalPrice = quantity * UnitPrice;
        totalPrice = Math.round(totalPrice);
        $("#TotalItemCost_" + rowNo).val(totalPrice);
        CalculateTotalBeforeTax();
        document.getElementById(id).setAttribute("style", "none");
    }
}

function SelectedIndexChanged(id) {
    var selectedOption, companyName;
    selectedOption = document.getElementById("VendorsID").selectedIndex;
    companyName = document.getElementById("VendorsID").options[selectedOption].innerText;
    document.getElementById("CompanyName").value = companyName;

    $.ajax({
        type: "POST",
        url: `/PurchaseOrder/BindCompanyAddress?id=` + id,
        data: "{}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            //console.log(result);
            var suplierAdd = result[0].SupplierAddress;
            $("#SupplierAddress").val(suplierAdd);
        }
    });
}

function SelectedIndexChangedLocation(id) {
    var selectedOptionLocationName, locationName;
    selectedOptionLocationName = document.getElementById("LocationId").selectedIndex;
    locationName = document.getElementById("LocationId").options[selectedOptionLocationName].innerText;
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
            $("#DeliveryAddress").val(deliveryAdd);
        }
    });
}

function CalculateTotalBeforeTax() {
    
    $('#TotalBeforeTax').val('');
    $('#TotalTax').val('');
    var length = document.getElementById("submissionTable").rows.length;
    length = parseFloat(length) - 1;
    var total = 0;
    var totalTax = 0;
    totalTax = Math.round(totalTax);

    var OtherTax = document.getElementById("OtherTax").value;

    if (OtherTax == '')
        OtherTax = 0;

    OtherTax = parseFloat(OtherTax);

    var i = 0;
    while (i <= length) {
        var temp = document.getElementById("TotalItemCost_" + i);
        if (temp == null || temp == '' || temp == 'undefined')
            i++;
        else {
            var temp = document.getElementById("TotalItemCost_" + i).value;
            var tempTotalTax = ((document.getElementById("ItemTax_" + i)).innerHTML).split(" %")[0];
            total = parseFloat(temp) + total;

            tempTotalTax = (parseFloat(tempTotalTax) / 100) * parseFloat(temp);
            totalTax = parseFloat(tempTotalTax) + totalTax;

            i++;
        }
    }

    //var rowNo = 0;  //Rahul added 'rowNo' 31-05-2023.
    //while (rowNo <= length) {debugger
    //    var temp = document.getElementById("TotalItemCost_" + rowNo).value;
    //    //Rahul added 'temp'  30-05-2023.
    //    if (temp == '' || temp == null) {
    //        temp = 0;
    //    }
    //    else {
    //        temp = parseFloat(temp);
    //    }

    //    var tempTotalTax = ((document.getElementById("ItemTax_" + rowNo)).innerHTML).split(" %")[0];
    //    total = parseFloat(temp) + total;
    //    //Rahul added 'tempTotalTax'30-05-2023.
    //    if (tempTotalTax == '' || tempTotalTax == null) {
    //        tempTotalTax = 0;
    //    }
    //    else {
    //        tempTotalTax = parseFloat(tempTotalTax);
    //    }

    //    tempTotalTax = (parseFloat(tempTotalTax) / 100) * parseFloat(temp);
    //    totalTax = parseFloat(tempTotalTax) + totalTax;

    //    rowNo++;
    //}

    $('#TotalBeforeTax').val(total.toFixed(2));
    $('#TotalTax').val(totalTax.toFixed(2));
    var tempGrandTotal = total + totalTax + OtherTax;
    tempGrandTotal = Math.round(tempGrandTotal);
    //var tempGrandTotal = total + totalTax;
    $('#TotalAfterTax').val(tempGrandTotal);
    $('#GrandTotal').val(tempGrandTotal);

    createJson();
}

function createJson() { 
    //let res = [...document.getElementById("myTableBody").children].map(tr =>
    //    Object.fromEntries([...tr.querySelectorAll("input,select")].map(el =>
    //        [el.name, el.value])));
    //console.log(res);
    //var TxtItemDetails = JSON.stringify(res);
    //console.log(TxtItemDetails);
    //$('#TxtItemDetails').val(TxtItemDetails);

    var table = document.getElementById('submissionTable');
    var rowCount = table.rows.length;
    //rowCount = parseFloat(length); //added 17-06-23
    var i = 0;
    TxtItemDetails = "[";
    /*for (i = 0; i < rowCount-2; i++) {*/  //added 'while' 17-06-23
    while (i <= rowCount-1) {
        var temp = document.getElementById("ItemCode_" + i);
        if (temp == null || temp == '' || temp == 'undefined')
            i++;
        else {
            var ItemCode = (document.getElementById("ItemCode_" + i)).innerHTML;
            var ItemID = (document.getElementById("ItemID_" + i)).innerHTML;
            var ItemName = (document.getElementById("ItemName_" + i)).innerHTML;
            var RequiredQty = (document.getElementById("RequiredQuantity_" + i)).innerHTML.split(" ")[0];
            var Unit = (document.getElementById("ItemUnit_" + i)).innerHTML;
            var OrderQty = $("#txtItemQty_" + i).val();
            OrderQty = (OrderQty == null || OrderQty == '') ? 0 : OrderQty;
            var BalanceQty = $("#BalanceQuantity_" + i).val();
            var PricePerUnit = $("#ItemUnitPrice_" + i).val();
            PricePerUnit = (PricePerUnit == null || PricePerUnit == '') ? 0 : PricePerUnit;
            var Tax = (document.getElementById("ItemTax_" + i)).innerHTML.split(" ")[0];
            var TotalItemCost = $("#TotalItemCost_" + i).val();
            TotalItemCost = (TotalItemCost == null || TotalItemCost == '') ? 0 : TotalItemCost;
            var CurrencyName = $("#CurrencyID option:selected").text();

            TxtItemDetails = TxtItemDetails + "{\"Item_Code\":\"" + ItemCode + "\", \"ItemId\":" + ItemID +
                ", \"ItemName\": \"" + ItemName + "\", \"RequiredQty\": " + RequiredQty + ", \"OrderQty\": " + OrderQty + ", \"BalanceQty\": " + BalanceQty
                + ", \"ItemUnit\": \"" + Unit + "\", \"ItemUnitPrice\": " + PricePerUnit + ", \"CurrencyName\": \"" + CurrencyName + "\",\"ItemTaxValue\": " + Tax +
                ", \"TotalItemCost\": " + TotalItemCost;

            //if (i == (rowCount - 1))
            if (i == (rowCount-2))
                TxtItemDetails = TxtItemDetails + "}";                 
            else
                TxtItemDetails = TxtItemDetails + "},";
            i++;  //added 17-06-23
        }
    }
    TxtItemDetails = TxtItemDetails + "]"
    $('#TxtItemDetails').val(TxtItemDetails);
        console.log(TxtItemDetails);
}

//======Common function which get called on change of Item ordered quantity.==========================

function OnChangeQty(value, id) {
    $('#btnSave').prop('disabled', false);
    $('#btn_SaveDraft').prop('disabled', false);
    debugger    
    var rowNo = id.split('_')[1];
    var quantity = value;
    var BalanceQty = $("#BalanceQuantity_" + rowNo).val();
    BalanceQty = parseFloat(BalanceQty);
    var ActualBalQty = $('#ActualBalanceQuantity_' + rowNo).text();
    ActualBalQty = parseFloat(ActualBalQty);

    //if (quantity == '')
        //quantity = 0;
    ////Rahul added 'if (quantity == 0)' start 19-07-23.
    if (quantity == '' || quantity == 0) {
        alert("Order quantity cannot be null or zero(0)!");
        document.getElementById(id).focus();
        document.getElementById(id).setAttribute("style", "border-color:red;");
        $('#btnSave').prop('disabled', true);
        $('#btn_SaveDraft').prop('disabled', true);
        return;
    }
    else {
        $('#btnSave').prop('disabled', false);
        $('#btn_SaveDraft').prop('disabled', false);
        document.getElementById(id).setAttribute("style", "border-color:none;");
    }
    ////Rahul added 'if (quantity == 0)' start 19-07-23 end.

    quantity = parseFloat(quantity);
    var RequiredQty = (document.getElementById("RequiredQuantity_" + rowNo)).innerHTML.split(" ")[0];
    RequiredQty = parseFloat(RequiredQty);

    if (quantity > ActualBalQty) {
        alert("Order quantity cannot be greater then indent balance quantity!");
        document.getElementById(id).focus();
        document.getElementById(id).setAttribute("style", "border-color:red;");
        $('#btnSave').prop('disabled', true);
        $('#btn_SaveDraft').prop('disabled', true);
        return;
    }
    else {
        $('#btnSave').prop('disabled', false);
        $('#btn_SaveDraft').prop('disabled', false);

        var tempBalanceQty = ActualBalQty - quantity ;
        if (quantity == 0) {
            $("#BalanceQuantity_" + rowNo).val(ActualBalQty);
        }
        else {
            $("#BalanceQuantity_" + rowNo).val(tempBalanceQty);
        }

        var price = $("#ItemUnitPrice_" + rowNo).val();
        var tempTax = ((document.getElementById("ItemTax_" + rowNo)).innerHTML).split(" %");
        var tax = tempTax[0];

        //Set total price in item grid.
        var totalPrice = quantity * price;
        totalPrice = Math.round(totalPrice);
        $("#TotalItemCost_" + rowNo).val(totalPrice);

        var totalPriceAfterTax = (totalPrice) + ((totalPrice * tax) / 100);
        $("#TotalBeforeTax").val(totalPrice);

        var itemTaxVal = tax / 100 * totalPrice;
        itemTaxVal = Math.round(itemTaxVal);
        totalPriceAfterTax = Math.round(totalPriceAfterTax);

        $("#TotalTax").val(itemTaxVal);
        $("#TotalAfterTax").val(totalPriceAfterTax);
        $("#GrandTotal").val(totalPriceAfterTax);

        CalculateTotalBeforeTax();
        document.getElementById(id).setAttribute("style", "border-color:none;");
    }


};

//=======================End======================================================//

function ValidateAdvancePayment(value, id) {
    var GrandTotal = parseFloat($("#GrandTotal").val());
    value = parseFloat(value);
    if (value > GrandTotal) {
        $("#ValMsgAdvancePayment").text("Advance payment cannot be greater than Grand total!");
        $('#ValMsgAdvancePayment').show();
        document.getElementById(id).focus();
        return;
    }
    else
        $('#ValMsgAdvancePayment').hide();

}

$("#btn_SaveDraft").click(function () {
    var CurrencyName = $("#CurrencyID option:selected").text();
    $("#CurrencyName").val(CurrencyName);
    $("#DraftFlag").val('true');
    createJson();
});

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

$('#uploadexcel').show();

$('#uploadexcel').attr("disabled", true);

$('#uploadexcel').click(function () {

    // Checking whether FormData is available in browser
    if (window.FormData !== undefined) {


        var fileUpload = $("#fileupload").get(0);
        var files = fileUpload.files;

        // Create FormData object
        var fileData = new FormData();

        // Looping over all files and add it to FormData object
        for (var i = 0; i < files.length; i++) {
            fileData.append(files[i].name, files[i]);
        }

        // Adding one more key to FormData object
        //fileData.append('username', ‘Manas’);

        $.ajax({
            url: '/PurchaseOrder/UploadSignature',
            type: "POST",
            contentType: false, // Not to set any content header
            processData: false, // Not to process data
            data: fileData,
            success: function (result) {
                alert(result);
                window.location.href = "/PurchaseOrder/AddPurchaseOrder";
            },
            error: function (err) {
                alert('Format of data uploaded is incorrect.');

            }
        });
    } else {
        alert("FormData is not supported.");
    }
});

$('#fileupload').change(function () {
    var fileExtension = ['jpg', 'jpeg', 'png'];
    if ($.inArray($(this).val().split('.').pop().toLowerCase(), fileExtension) == -1) {
        // $('#uploadexcel').attr("disabled", true);
        alert("Only '.jpg','.jpeg','png' formats are allowed.");
        $('#fileupload').val('');
        //$('#<%= myLabel.ClientID %>').html("Only '.jpeg','.jpg' formats are allowed.");
    }
    else {
        $('#uploadexcel').attr("disabled", false);
    }
});

/*Rahul : Add Javascript validation on 12 Oct 2022.*/
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
/*Rahul : Add Javascript 'function removeTr(index)' start on 30-05-2023.*/
function removeTr(index) {  
    var length = document.getElementById("submissionTable").rows.length;
    length = parseFloat(length) - 1;
    id = index;
    if (length == 1) {
        CalculateTotalBeforeTax();
        event.preventDefault();
        return false;
    }
    else if (length >= index) {
        $('#tablerow' + index).remove();
        //CalculateTotalBeforeTax();
    }
    //else {
    //    CalculateTotalBeforeTax();
    //}
    CalculateTotalBeforeTax();
     return false;
}
/*Rahul : Add Javascript 'function removeTr(index)' end on 30-05-2023.*/