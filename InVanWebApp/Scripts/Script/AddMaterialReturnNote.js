//Rahul added 'function createCustomDropdown()'  start 

$(document).ready(function () {
    createCustomDropdown_ReturnBy();
    createCustomDropdown_LocationId();    
    createCustomDropdown_ddlItem();
})

function createCustomDropdown_ReturnBy() {
    $('select#ReturnBy').each(function (i, select) {

        if (!$(this).next().hasClass('dropdown-select')) {

            $('#ReturnBy').removeClass('form-control');
            $(this).after('<div id="divReturnBy" class="dropdown-select wide ' + ($(this).attr('class') || '') + '" tabindex="0"><span class="current"></span><div class="list"><ul></ul></div></div>');
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
    $('#divReturnBy.dropdown-select ul').before('<div class="dd-search"><input id="txtSearchValueReturnBy" autocomplete="off" onkeyup="filterReturnBy()" class="dd-searchbox" type="text" placeholder="Search for list" ><br />&nbsp;<span id="faSearch"><i class="fas fa-search"></i></span></div>');
}
function filterReturnBy() {
    var valThis = $('#txtSearchValueReturnBy').val();
    $('.dropdown-select ul > li').each(function () {
        var text = $(this).text();
        (text.toLowerCase().indexOf(valThis.toLowerCase()) > -1) ? $(this).show() : $(this).hide();
    });
};

function createCustomDropdown_LocationId() {
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

function createCustomDropdown_ddlItem() {
    $('select#ddlItem').each(function (i, select) {
        if (!$(this).next().hasClass('dropdown-select')) {
            $('#ddlItem').removeClass('form-control');
            $(this).after('<div id="divddlItem" class="dropdown-select wide ' + ($(this).attr('class') || '') + '" tabindex="0"><span class="current"></span><div class="list"><ul></ul></div></div>');
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
    $('#divddlItem.dropdown-select ul').before('<div class="dd-search"><input id="txtSearchValueddlItem" autocomplete="off" onkeyup="filterddlItem()" class="dd-searchbox" type="text" placeholder="Search for list" ><br />&nbsp;<span id="faSearch"><i class="fas fa-search"></i></span></div>');
}
function filterddlItem() {
    var valThis = $('#txtSearchValueddlItem').val();
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

    //Rahul added 'function createCustomDropdown()' end

//=====================Onchange of Issue By===========================
function SelectedIndexChangedReturnBy(id) {  
    var SelectedReturnByNameVal = $("#divReturnBy .option.selected").attr('data-value');
    if (SelectedReturnByNameVal != '' || SelectedReturnByNameVal != "") {
        $('#ReturnByErrMsg').text('');
        document.getElementById('ReturnByErrMsg').setAttribute('style', 'color:#000;');
        $('#ReturnByErrMsg').hide;
        $('#btnSave').prop('disabled', false);
    }
}
//==================Set value in txtItemDetails onCick of Save/Update button======--------
function SaveBtnClick() {
    //debugger
    var ReturnByName = $("#divReturnBy .selected").text();
    if (ReturnByName == '---Select---') {
        $('#ReturnByErrMsg').text('Select return by!');
        document.getElementById('ReturnByErrMsg').setAttribute('style', 'color:red;');
        $('#divReturnBy').focus();
        document.getElementById('divReturnBy').focus();
        $('#btnSave').prop('disabled', true);
        event.preventDefault();
        return;
    }
    else {
        $('#ReturnByName').val(ReturnByName);
    }

    var ReturnByNameVal = $("#divReturnBy .option.selected").attr('data-value');
    if (ReturnByNameVal == '' || ReturnByNameVal == "") {
        $('#ReturnByErrMsg').text('Select return by!');
        document.getElementById('ReturnByErrMsg').setAttribute('style', 'color:red;');
        $('#divReturnBy').focus();
        document.getElementById('divReturnBy').focus();
        $('#btnSave').prop('disabled', true);
        event.preventDefault();
        return;
    }

    var LocationName = $("#divLocationId .option.selected").text();
    if (LocationName == '---Select---') {
        $('#LocationIdErrMsg ').text('Select location!');
        document.getElementById('LocationIdErrMsg').setAttribute('style', 'color:red;');
        $('#divLocationId').focus();
        document.getElementById('divLocationId').focus();
        $('#btnSave').prop('disabled', true);
        event.preventDefault();
        return;
    }
    else {
        $('#LocationName').val(LocationName);
    }

    var LocationNameVal = $("#divLocationId .option.selected").attr('data-value');
    if (LocationNameVal == '' || LocationNameVal == "") {
        $('#LocationIdErrMsg').text('Select location!');
        document.getElementById('LocationIdErrMsg').setAttribute('style', 'color:red;');
        $('#divLocationId').focus();
        document.getElementById('divLocationId').focus();
        $('#btnSave').prop('disabled', true);
        event.preventDefault();
        return;
    }


    var tableLength = document.getElementById('submissionTable').rows.length;
    debugger
    if (tableLength == 1) {
        alert("No item rejected, Cannot create intermediate rejection note!");
        $('#btnSave').prop('disabled', true);
        event.preventDefault();
        return;
    }
    var flag = 0, i = 0;
    if (tableLength > 1) {
        while (i < tableLength - 1) {
            var AvlQty = document.getElementById("CurrentStockQuantity_" + i).innerHTML;
            AvlQty = parseFloat(AvlQty);
            var ReturnQty = document.getElementById("txtReturnQuantity_" + i).value; //Change this with Rejected qty 
            ReturnQty = parseFloat(ReturnQty);

            $('#spanReturnQty_' + i).text('');
            if (ReturnQty == 0 || ReturnQty == '') {
                $('#spanReturnQty_' + i).text('You can not enter rejected quantity value zero or null!');
                document.getElementById('spanReturnQty_' + i).setAttribute('style', 'color:red;');
                document.getElementById("txtReturnQuantity_" + i).focus();
                if (i == 0) {
                    $('#spanReturnQty_' + i).text('');
                    i++;
                    continue;
                }
                //$('#btnSave').prop('disabled', true);
                //event.preventDefault();
                //return;
            }

            if (ReturnQty != 0) {
                flag = 1;
                var Comments = document.getElementById("txtRemarks_" + i).value;
                var RejectQty = document.getElementById("txtReturnQuantity_" + i).value;
                RejectQty = parseFloat(RejectQty);

                $('#spanRemark_' + i).text('');
                if (Comments == '' || Comments == null) {
                    $('#spanRemark_' + i).text('Comment is mandatory!');
                    document.getElementById('spanRemark_' + i).setAttribute('style', 'color:red;');
                    document.getElementById("txtRemarks_" + i).focus();
                    $('#btnSave').prop('disabled', true);
                    event.preventDefault();
                    return;
                }
                else if (RejectQty > AvlQty) {
                    document.getElementById("txtReturnQuantity_" + i).value = 0;
                    $('#spanReturnQty_' + i).text('');
                    document.getElementById("txtReturnQuantity_" + i).focus();
                    $('#btnSave').prop('disabled', true);
                    event.preventDefault();
                    return;
                }
                document.getElementById("txtRemarks_" + i).setAttribute("required", "required");
            }
            $('#spanRemark_' + i).text('');

            i++;
        }

        if (flag != 1) {
            alert("No item rejected, Cannot create intermediate rejection note!");
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

//=====================Onchange of Location===========================
function SelectedIndexChangedLocation(id) {

    var SelectedLocationNameVal = $("#divLocationId .option.selected").attr('data-value');
    if (SelectedLocationNameVal != '' || SelectedLocationNameVal != "") {
        $('#LocationIdErrMsg').text('');
        document.getElementById('LocationIdErrMsg').setAttribute('style', 'color:#000;');
        $('#LocationIdErrMsg').hide;
        $('#btnSave').prop('disabled', false);
    }

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
        //    var i = 1;
        //    $("#ddlItem").append($("<option></option>").val(result[0].ID).html("--Select--"));
        //    while (i < result.length) {
        //        $("#ddlItem").append($("<option></option>").val(result[i].ID).html(result[i].Item_Name));
        //        i++;
        //    }
            // Clear existing options from the dropdown list
            $("#ddlItem").empty();

            // Add a default option "--Select--"
            $("#ddlItem").append($("<option></option>").val("0").html("--Select--"));

            // Loop through the result array and add each item to the dropdown list
            for (var i = 0; i < result.length; i++) {
                //$('#divddlItem.dropdown-select ul').append($("<li></li>").val(result[i].ID).html(result[i].Item_Name));
                var display = result[i].Item_Name;
                var option = $('<li class="option ' + (i === 0 ? 'selected' : '') +
                    '" data-value="' + result[i].ID +
                    '" data-display-text="' + display + '">' +
                    result[i].Item_Name + '</li>');
                $('#divddlItem.dropdown-select ul').append(option);
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
            //alert('result: ' + JSON.stringify(result));
            //debugger
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
                        var t0 = document.createElement("input");
                        t0.type = "checkbox";
                        t0.id = "SelectedItem_" + j;
                        t0.name = "ItemSelected";
                        t0.setAttribute("value", false);
                        t0.setAttribute("class", "form-control-sm");
                        cell.appendChild(t0);
                    }   
                    else  if (i == 1) {
                        cell.innerHTML = result[j].Item_Code;
                        cell.setAttribute("class", "d-none");
                        cell.setAttribute("id", "ItemCode_" + j);
                    }
                    else if (i == 2) {
                        cell.innerHTML = result[j].ItemId;
                        cell.setAttribute("class", "d-none");
                        cell.setAttribute("id", "ItemID_" + j);
                    }
                    else if (i == 3) {                        
                        cell.innerHTML = result[j].Item_Code + ' ('+result[j].Item_Name+')';
                        cell.setAttribute("id", "ItemName_" + j);
                    }
                    else if (i == 4) {
                        cell.innerHTML = result[j].ItemUnitPrice;
                        cell.setAttribute("id", "ItemUnitPrice_" + j);
                    }
                    else if (i == 5) {
                        cell.innerHTML = result[j].CurrencyName;
                        cell.setAttribute("class", "d-none");
                        cell.setAttribute("id", "CurrencyName_" + j);
                    }
                    else if (i == 6) {
                        cell.innerHTML = result[j].ItemUnit;
                        cell.setAttribute("id", "ItemUnit_" + j);
                    }
                    else if (i == 7) {
                        cell.innerHTML = result[j].AvailableStock;
                        cell.setAttribute("id", "CurrentStockQuantity_" + j);
                    }
                    else if (i == 8) {
                        var t8 = document.createElement("input");
                        t8.id = "txtReturnQuantity_" + j;
                        t8.setAttribute("value", "0");
                        t8.setAttribute("type", "text");
                        t8.setAttribute("maxlength", "8");
                        t8.setAttribute("onkeypress", "return isNumberKey(event)");
                        t8.setAttribute("onchange", "OnChangeQty($(this).val(),id)");
                        t8.setAttribute("class", "form-control form-control-sm");
                        cell.appendChild(t8);
                        var t8 = document.createElement('span');
                        t8.id = "spanReturnQty_" + j; 
                        cell.appendChild(t8);
                    }
                    else if (i == 9) {
                        var t9 = document.createElement("input");
                        t9.id = "txtFinalQuantity_" + j;
                        t9.setAttribute("readonly", "readonly");
                        t9.setAttribute("value", "0");                       
                        t9.setAttribute("class", "form-control form-control-sm");
                        cell.appendChild(t9);
                    }
                    else if (i == 10) {
                        var t10 = document.createElement("input");
                        t10.id = "txtRemarks_" + j;
                        t10.setAttribute("maxlength", "90");
                        t10.setAttribute("style", "width:auto;");
                        t10.setAttribute("class", "form-control form-control-sm");
                        t10.setAttribute("onkeyup", "OnChangeComment($(this).val(),id)");
                        cell.appendChild(t10);
                        var t10 = document.createElement('span');
                        t10.id = "spanRemark_" + j;
                        cell.appendChild(t10);
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

    if (Item_ID == '' || Item_ID == undefined || Item_ID == null) {

        // Get the selected option element with class "option"
        var selectedOption = $('#divddlItem.dropdown-select ul li.option.selected');

        // Get the data-value and data-display-text attributes of the selected option
        var dataValue = selectedOption.data('value');
        var Item_ID = dataValue;

        var dataDisplayText = selectedOption.data('display-text');

        // Get the text content of the selected option
        var textContent = selectedOption.text();

        // Output the values
        console.log('data-value:', dataValue);
        console.log('data-display-text:', dataDisplayText);
        console.log('text content:', textContent);
    }

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
                        var t0 = document.createElement("input");
                        t0.type = "checkbox";
                        t0.id = "SelectedItem_" + j;
                        t0.name = "ItemSelected";
                        t0.setAttribute("value", false);
                        t0.setAttribute("class", "form-control-sm");
                        cell.appendChild(t0);
                    }
                    else  if (i == 1) {
                        cell.innerHTML = result[j].Item_Code;
                        cell.setAttribute("class", "d-none");
                        cell.setAttribute("id", "ItemCode_" + j);
                    }
                    else if (i == 2) {
                        cell.innerHTML = result[j].ItemId;
                        cell.setAttribute("class", "d-none");
                        cell.setAttribute("id", "ItemID_" + j);
                    }
                    else if (i == 3) {
                        cell.innerHTML = result[j].Item_Code + ' (' + result[j].Item_Name + ')';
                        cell.setAttribute("id", "ItemName_" + j);
                    }
                    else if (i == 4) {
                        cell.innerHTML = result[j].ItemUnitPrice;
                        cell.setAttribute("id", "ItemUnitPrice_" + j);
                    }
                    else if (i == 5) {
                        cell.innerHTML = result[j].CurrencyName;
                        cell.setAttribute("class", "d-none");
                        cell.setAttribute("id", "CurrencyName_" + j);
                    }
                    else if (i == 6) {
                        cell.innerHTML = result[j].ItemUnit;
                        cell.setAttribute("id", "ItemUnit_" + j);
                    }
                    else if (i == 7) {
                        cell.innerHTML = result[j].AvailableStock;
                        cell.setAttribute("id", "CurrentStockQuantity_" + j);
                    }
                    else if (i == 8) {
                        var t8 = document.createElement("input");
                        t8.id = "txtReturnQuantity_" + j;
                        t8.setAttribute("value", "0");
                        t8.setAttribute("type", "text");
                        t8.setAttribute("maxlength", "8");
                        t8.setAttribute("onkeypress", "return isNumberKey(event)");
                        t8.setAttribute("onchange", "OnChangeQty($(this).val(),id)");
                        t8.setAttribute("class", "form-control form-control-sm");
                        cell.appendChild(t8);
                        var t8 = document.createElement('span');
                        t8.id = "spanReturnQty_" + j;
                        cell.appendChild(t8);
                    }
                    else if (i == 9) {
                        var t9 = document.createElement("input");
                        t9.id = "txtFinalQuantity_" + j;
                        t9.setAttribute("readonly", "readonly");
                        t9.setAttribute("value", "0");                        
                        t9.setAttribute("class", "form-control form-control-sm");
                        cell.appendChild(t9);
                    }
                    else if (i == 10) {
                        var t10 = document.createElement("input");
                        t10.id = "txtRemarks_" + j;
                        t10.setAttribute("maxlength", "90");
                        t10.setAttribute("style", "width:auto;");
                        t10.setAttribute("class", "form-control form-control-sm");
                        t10.setAttribute("onkeyup", "OnChangeComment($(this).val(),id)");
                        cell.appendChild(t10);
                        var t10 = document.createElement('span');
                        t10.id = "spanRemark_" + j;
                        cell.appendChild(t10);
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
    $('#btnSave').prop('disabled', false);
}

function OnChangeQty(value, id) {

    $('#btnSave').prop('disabled', false);
    var rowNo = id.split('_')[1];
    $('#spanReturnQty_' + rowNo).text('');
    var ReturnQty = document.getElementById("txtReturnQuantity_" + rowNo).value;
    if (ReturnQty == 0 || ReturnQty == '') {
        $('#spanReturnQty_' + rowNo).text('You can not enter Return quantity value zero or null!');
        document.getElementById('spanReturnQty_' + rowNo).setAttribute('style', 'color:red;');
        document.getElementById("txtReturnQuantity_" + rowNo).focus();
        $('#btnSave').prop('disabled', true);
        event.preventDefault();
        return;
    }

    var CurrentQty = document.getElementById("CurrentStockQuantity_" + rowNo).innerHTML;
    CurrentQty = parseFloat(CurrentQty);
    var ReturnQty = parseFloat(value);
    ReturnQty = parseFloat(ReturnQty);

    var FinalQty = 0;

    if (ReturnQty > CurrentQty) {
        $('#spanReturnQty_' + rowNo).text('You can not issue more than current stock quantity!');
        document.getElementById('spanReturnQty_' + rowNo).setAttribute('style', 'color:red;');
        document.getElementById(id).focus();
        $('#btnSave').prop('disabled', true);
        event.preventDefault();
        return;
    }
    else {
        $('#spanReturnQty_' + rowNo).text('');
        FinalQty = CurrentQty + ReturnQty; 
        FinalQty = parseFloat(FinalQty);
        document.getElementById("txtFinalQuantity_" + rowNo).value = FinalQty;
    }
    document.getElementById("txtRemarks_" + rowNo).focus();

    $('#spanRemark_' + rowNo).text('');
    var remarks = $('#txtRemarks_' + rowNo).val();
    if (remarks == '' && remarks == null) {
        $('#spanRemark_' + rowNo).text('Comment is mandatory!');
        document.getElementById('spanRemark_' + rowNo).setAttribute('style', 'color:red;');
        document.getElementById("txtRemarks_" + rowNo).focus();
        $('#btnSave').prop('disabled', true);
        event.preventDefault();
    }
    else
        $('#spanRemark_' + rowNo).text('');
        $('#btnSave').prop('disabled', false);
}

var TxtItemDetails = "";

function createJson() {
    debugger
    var table = document.getElementById('submissionTable');
    var rowCount = table.rows.length;
    var i = 0, flag = 0;
    TxtItemDetails = "[";
    while (i < rowCount - 1) {

        var ItemSelected = false;
        if ($('#SelectedItem_' + i).is(":checked")) {debugger
            ItemSelected = true;
        }

        var ItemCode = (document.getElementById("ItemCode_" + i)).innerHTML;
        var ItemID = (document.getElementById("ItemID_" + i)).innerHTML;
        var ItemName = (document.getElementById("ItemName_" + i)).innerHTML;
        var PricePerUnit = (document.getElementById("ItemUnitPrice_" + i)).innerHTML;
        var CurrencyName = (document.getElementById("CurrencyName_" + i)).innerHTML;
        var CurrentStock = (document.getElementById("CurrentStockQuantity_" + i)).innerHTML;
        var Unit = (document.getElementById("ItemUnit_" + i)).innerHTML;

        var ReturnQty = (document.getElementById("txtReturnQuantity_" + i)).value;        
        var FinalQty = (document.getElementById("txtFinalQuantity_" + i)).value;

        var Remarks = (document.getElementById("txtRemarks_" + i)).value;

        if (ItemSelected == false) {debugger
            i++;
            continue;
        }
        TxtItemDetails = TxtItemDetails + "{\"Item_Code\":\"" + ItemCode + "\", \"ItemId\":" + ItemID +
            ", \"ItemName\": \"" + ItemName + "\", \"ItemUnitPrice\": " + PricePerUnit +
            ", \"CurrencyName\": \"" + CurrencyName + "\", \"CurrentStock\": " + CurrentStock + ", \"ItemUnit\": \"" + Unit +
            "\", \"ReturnQty\": " + ReturnQty + ",\"FinalQty\": " + FinalQty +
            ",\"Remarks\": \"" + Remarks + "\", \"ItemSelected\": \"" + ItemSelected + "\"";

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

    alert(TxtItemDetails);
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