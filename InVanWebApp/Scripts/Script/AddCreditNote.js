
        //Vedant added 'function createCustomDropdown()' 03/07/23. start
    $(document).ready(function () {
        createCustomDropdown_PO_ID();
    });

    function createCustomDropdown_PO_ID() {
        $('select#PO_ID').each(function (i, select) {

            if (!$(this).next().hasClass('dropdown-select')) {

                $('#PO_ID').removeClass('form-control');
                $(this).after('<div id="divPO_ID" class="dropdown-select wide d-flex align-items-center' + ($(this).attr('class') || '') + '" tabindex="0"><span class="current"></span><div class="list"><ul></ul></div></div>');
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
    $('#divPO_ID.dropdown-select ul').before('<div class="dd-search"><input id="txtSearchValuePO_ID" autocomplete="off" onkeyup="filterPO_ID()" class="dd-searchbox" type="text" placeholder="Search for list" ><br />&nbsp;<span id="faSearch"><i class="fas fa-search"></i></span></div>');

    }
        function filterPO_ID() {
        var valThis = $('#txtSearchValuePO_ID').val();
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

            //Vedant added 'function createCustomDropdown()' 03/07/23. start

//==================Set value in txtItemDetails onCick of Save/Update button======--------
function SaveBtnClick() {
    var PONumber = $("#PO_ID option:selected").text();
    $("#PO_Number").val(PONumber);

    var grandTotal = $('#GrandTotal').val();
    if (grandTotal == '' || grandTotal == null) {
        grandTotal = 0;
    }
    else {
        grandTotal = parseFloat(grandTotal);
    }
    if (grandTotal == 0) {
        alert('Wastage of all listed items are zero, Cannot create its credit note!');
        $('#btnSave').prop('disabled', true);
        event.preventDefault();
        return;
    }
    else {
        $('#btnSave').prop('disabled', false);
    }
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
                        //cell.innerHTML = result[j].TotalItemCost + " " + result[j].CurrencyName;
                        //cell.setAttribute("id", "TotalItemCost_" + j);

                        var t9 = document.createElement("input");
                        t9.id = "TotalItemCost_" + j;
                        t9.setAttribute("class", "form-control form-control-sm");
                        t9.setAttribute("readonly", "readonly");
                        cell.appendChild(t9);
                    }
                }

            }

            //CalculateTotalBeforeTax();
            //var grandTotal = $('#GrandTotal').val()
            //grandTotal = parseFloat(grandTotal);
            //if (grandTotal == 0) {
            //    alert('Wastage of all listed items are zero, Cannot create its credit note!');
            //    $('#btnSave').prop('disabled', true);
            //}
            //else {
            //    $('#btnSave').prop('disabled', false);
            //}
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
        var temp = ((document.getElementById("TotalItemCost_" + i)).value);
        if (temp == '' || temp == null) {
            temp = 0;
        }
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

        var unitPrice = document.getElementById('ItemUnitPrice_' + rowNo).innerText.split(' ')[0];
        if (unitPrice == null || unitPrice == '') {
            unitPrice = 0;
        }
        else {
            unitPrice = parseFloat(unitPrice);
        }
        var totalItrmCost = unitPrice * value;
        $('#TotalItemCost_' + rowNo).val(totalItrmCost);

        CalculateTotalBeforeTax();
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
        var TotalItemCost = (document.getElementById("TotalItemCost_" + i)).value;
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