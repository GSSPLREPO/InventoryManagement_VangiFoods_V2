//Vedant added 'function createCustomDropdown()' 04-07-2023. start
$(document).ready(function () {
    //createCustomDropdown_LocationId();
    createCustomDropdown_ddlItem();
});

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
    //Vedant added 'function createCustomDropdown()' 04-07-2023. end



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
            ///Rahul commented extrea code from 'function SelectedIndexChangedLocation(id)' 'while (i < result.length)' start 31-07-23.
            // Display the result array as a JSON string
        //    alert('result: ' + JSON.stringify(result));
        //    console.log(result)
        //    alert('result :' + result);
        //    var i = 1;
        //    $("#ddlItem").append($("<option></option>").val(result[0].ID).html("--Select--"));
        //    while (i < result.length) {
        //        $("#ddlItem").append($("<option></option>").val(result[i].ID).html(result[i].Item_Name));
        //        i++;
        //    }
            ///Rahul commented extrea code from 'function SelectedIndexChangedLocation(id)' 'while (i < result.length)' end 31-07-23.
            /*///Rahul added code in 'function SelectedIndexChangedLocation(id)' ' $("#ddlItem").empty();' start 31-07-23.*/
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
            /*///Rahul  added code in 'function SelectedIndexChangedLocation(id)' ' $("#ddlItem").empty();' end 31-07-23.*/
        },
        error: function (err) {
            alert('Not able to fetch item list of that warehouse!');

        }
    });

        ///This call is for binding item details of that location
        $.ajax({
        url: '/StockAdjustment/GetLocationStocksDetails',
        type: "POST",
        data: { id: Location_ID},
        success: function (result) {
            alert('result: ' + JSON.stringify(result));
            debugger
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
                    /// @*///Rahul added 'if (i == 0)' in 'function SelectedIndexChangedLocation(id)' start 31-0723.*@
                    if (i == 0) {
                        var t0 = document.createElement("input");
                        t0.type = "checkbox";
                        t0.id = "SelectedItem_" + j;
                        t0.name = "ItemSelected";
                        t0.setAttribute("value", true);
                        t0.setAttribute("class", "form-control-sm");
                        cell.appendChild(t0);
                    }   /// @*///Rahul added 'if (i == 0)' in 'function SelectedIndexChangedLocation(id)' end 31-0723.*@
                    else if (i == 1) {
                        cell.innerHTML = result[j].Item_Code;
                        cell.setAttribute("id", "ItemCode_" + j);
                    }
                    else if (i == 2) {
                        cell.innerHTML = result[j].ItemId;
                        cell.setAttribute("class", "d-none");
                        cell.setAttribute("id", "ItemID_" + j);
                    }
                    else if (i == 3) {
                        cell.innerHTML = result[j].Item_Name;
                        cell.setAttribute("id", "ItemName_" + j);

                    }
                    else if (i == 4) {
                        cell.innerHTML = result[j].ItemUnitPrice;
                        cell.setAttribute("id", "ItemUnitPrice_" + j);
                    }
                    else if (i == 5) {
                        cell.innerHTML = result[j].CurrencyName;
                        cell.setAttribute("id", "CurrencyName_" + j);
                    }
                    else if (i == 6) {
                        cell.innerHTML = result[j].AvailableStock;
                        cell.setAttribute("id", "AvailableStock_" + j);
                    }
                    else if (i == 7) {

                        cell.innerHTML = result[j].ItemUnit;
                        cell.setAttribute("id", "ItemUnit_" + j);
                    }
                    else if (i == 8) {
                        var t8 = document.createElement("input");
                        t8.id = "txtPhysicalStock_" + j;
                        t8.setAttribute("onchange", "OnChangeQty($(this).val(),id)");
                        t8.setAttribute("value", "0");
                        //t8.setAttribute("type", "number");
                        t8.setAttribute("onkeypress", "return isNumberKey(event)");
                        t8.setAttribute("maxlength", "8");
                        t8.setAttribute("class", "form-control form-control-sm");
                        cell.appendChild(t8);
                    }
                    else if (i == 9) {
                        var t9 = document.createElement("input");
                        t9.id = "txtDifference_" + j;
                        t9.setAttribute("readonly", "readonly");
                        t9.setAttribute("value", "0");
                        t9.setAttribute("class", "form-control form-control-sm");
                        cell.appendChild(t9);
                    }
                    else if (i == 10) {
                        var t10 = document.createElement("input");
                        t10.id = "txtTransferPrice_" + j;
                        t10.setAttribute("readonly", "readonly");
                        t10.setAttribute("value", "0");
                        //t10.setAttribute("type", "number");
                        t10.setAttribute("class", "form-control form-control-sm");
                        cell.appendChild(t10);
                    }
                    else if (i == 11) {
                        var t11 = document.createElement("input");
                        t11.id = "txtRemarks_" + j;
                        t11.setAttribute("maxlength", "90");
                        t11.setAttribute("style", "width:auto;");
                        t11.setAttribute("class", "form-control form-control-sm");
                        t11.setAttribute("onkeyup", "OnChangeComment($(this).val(),id)");
                        cell.appendChild(t11);
                        var t11 = document.createElement('span');
                        t11.id = "spanRemark_" + j;
                        cell.appendChild(t11);
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
    debugger
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
    /*///Rahul added code in 'function SelectedIndexChangedItem(id)' 'if (Item_ID == '' || Item_ID == undefined || Item_ID == null)' start 31-07-23.*/
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
    /*///Rahul added code in 'function SelectedIndexChangedItem(id)' 'if (Item_ID == '' || Item_ID == undefined || Item_ID == null)' end 31-07-23.*/
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
                        t0.setAttribute("value", true);
                        t0.setAttribute("class", "form-control-sm");
                        cell.appendChild(t0);
                    }
                    else if (i == 1) {
                        cell.innerHTML = result[j].Item_Code;
                        cell.setAttribute("id", "ItemCode_" + j);
                    }
                    else if (i == 2) {
                        cell.innerHTML = result[j].ItemId;
                        cell.setAttribute("class", "d-none");
                        cell.setAttribute("id", "ItemID_" + j);
                    }
                    else if (i == 3) {
                        cell.innerHTML = result[j].Item_Name;
                        cell.setAttribute("id", "ItemName_" + j);

                    }
                    else if (i == 4) {
                        cell.innerHTML = result[j].ItemUnitPrice;
                        cell.setAttribute("id", "ItemUnitPrice_" + j);
                    }
                    else if (i == 5) {
                        cell.innerHTML = result[j].CurrencyName;
                        cell.setAttribute("id", "CurrencyName_" + j);
                    }
                    else if (i == 6) {
                        cell.innerHTML = result[j].AvailableStock;
                        cell.setAttribute("id", "AvailableStock_" + j);
                    }
                    else if (i == 7) {
                        cell.innerHTML = result[j].ItemUnit;
                        cell.setAttribute("id", "ItemUnit_" + j);
                    }
                    else if (i == 8) {
                        var t8 = document.createElement("input");
                        t8.id = "txtPhysicalStock_" + j;
                        t8.setAttribute("onchange", "OnChangeQty($(this).val(),id)");
                        t8.setAttribute("value", "0");
                        //t8.setAttribute("type", "number");
                        t8.setAttribute("onkeypress", "return isNumberKey(event)");
                        t8.setAttribute("maxlength", "8");
                        t8.setAttribute("class", "form-control form-control-sm");
                        cell.appendChild(t8);
                    }
                    else if (i == 9) {
                        var t9 = document.createElement("input");
                        t9.id = "txtDifference_" + j;
                        t9.setAttribute("readonly", "readonly");
                        t9.setAttribute("value", "0");
                        t9.setAttribute("class", "form-control form-control-sm");
                        cell.appendChild(t9);
                    }
                    else if (i == 10) {
                        var t10 = document.createElement("input");
                        t10.id = "txtTransferPrice_" + j;
                        t10.setAttribute("readonly", "readonly");
                        t10.setAttribute("value", "0");
                        t10.setAttribute("type", "number");
                        t10.setAttribute("class", "form-control form-control-sm");
                        cell.appendChild(t10);
                    }
                    else if (i == 11) {
                        var t11 = document.createElement("input");
                        t11.id = "txtRemarks_" + j;
                        t11.setAttribute("maxlength", "90");
                        t11.setAttribute("style", "width:auto;");
                        t11.setAttribute("class", "form-control form-control-sm");
                        t11.setAttribute("onkeyup", "OnChangeComment($(this).val(),id)");
                        cell.appendChild(t11);
                        var t11 = document.createElement('span');
                        t11.id = "spanRemark_" + j;
                        cell.appendChild(t11);
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
        document.getElementById("txtTransferPrice_" + rowNo).setAttribute("style", "color:none;");

    }
    else if (value < avalQty) {
        DiffQty = avalQty - value;
        DiffQty = parseFloat(DiffQty);
        document.getElementById("txtDifference_" + rowNo).value = "-" + DiffQty;
        document.getElementById("txtDifference_" + rowNo).setAttribute("style", "color:red;");
        document.getElementById("txtTransferPrice_" + rowNo).setAttribute("style", "color:red;");
    }
    else {
        DiffQty = avalQty - value;
        DiffQty = parseFloat(DiffQty);
        document.getElementById("txtDifference_" + rowNo).value = DiffQty;
        document.getElementById("txtDifference_" + rowNo).setAttribute("style", "color:none;");
        document.getElementById("txtTransferPrice_" + rowNo).setAttribute("style", "color:none;");
    }

    var TransferPrice = (UnitPrice * DiffQty);
    TransferPrice = Math.round(TransferPrice * 100) / 100;
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
        ///Rahul added 'var ItemSelected' start 31-07-23.
        var ItemSelected = false;
        if ($('#SelectedItem_' + i).is(":checked")) {
            ItemSelected = true;
        }
        ///Rahul added 'var ItemSelected' end 31-07-23.
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
        ///Rahul added 'var ItemSelected' start 31-07-23.
        if (ItemSelected == false) {
            i++;
            continue;
        }
        ///Rahul added 'var ItemSelected' end 31-07-23.
        ///Rahul added 'var ItemSelected' in 'TxtItemDetails' start 31-07-23.
        TxtItemDetails = TxtItemDetails + "{\"Item_Code\":\"" + ItemCode + "\", \"ItemId\":" + ItemID +
            ", \"ItemName\": \"" + ItemName + "\", \"ItemUnitPrice\": " + PricePerUnit +
            ", \"CurrencyName\": \"" + CurrencyName + "\", \"AvlStock\": " + AvailableStock + ", \"ItemUnit\": \"" + Unit +
            "\", \"PhyQty\": " + PhyQty + ",\"DiffQty\": " + DiffQty +
            ", \"TransPrice\": " + TransPrice + ",\"Remarks\": \"" + Remarks + "\", \"ItemSelected\":" + ItemSelected;
        ///Rahul added 'var ItemSelected' in 'TxtItemDetails' end 31-07-23.
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