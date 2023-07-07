
    //Vedant added 'function createCustomDropdown()' 03-07-2023. start
    $(document).ready(function () {
        createCustomDropdown_SO_Id();
    });

    function createCustomDropdown_SO_Id() {
        $('select#SO_Id').each(function (i, select) {

            if (!$(this).next().hasClass('dropdown-select')) {

                $('#SO_Id').removeClass('form-control');
                $(this).after('<div id="divSO_Id" class="dropdown-select wide ' + ($(this).attr('class') || '') + '" tabindex="0"><span class="current"></span><div class="list"><ul></ul></div></div>');
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
    $('#divSO_Id.dropdown-select ul').before('<div class="dd-search"><input id="txtSearchValueSO_Id" autocomplete="off" onkeyup="filterSO_Id()" class="dd-searchbox" type="text" placeholder="Search for list" ><br />&nbsp;<span id="faSearch"><i class="fas fa-search"></i></span></div>');
    }
        function filterSO_Id() {
        var valThis = $('#txtSearchValueSO_Id').val();
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


//==================Set value in txtItemDetails onCick of Save/Update button======--------
function SaveBtnClick() {
    var SONumber = $("#SO_Id option:selected").text();
    $("#SONumber").val(SONumber);

    var tableLength = document.getElementById('submissionTable').rows.length;
    var flag = 0, i = 1, balQtyFlag = 0;
    if (tableLength > 1) {
        while (i <= tableLength - 1) {
            var PhyQty = document.getElementById("txtShippingQty_" + i).value;
            PhyQty = parseFloat(PhyQty);

            var BalQty = document.getElementById("txtBalQty_" + i).value;
            BalQty = parseFloat(BalQty);

            if (BalQty == 0)
                balQtyFlag = 1;

            if (PhyQty != 0) {
                flag = 1;
            }

            i++;
        }

        if (balQtyFlag == 1) {
            alert("All items shipped, Cannot create its delivery challan!");
            $('#btnSave').prop('disabled', true);
            return;
        }
        else
            $('#btnSave').prop('disabled', false);

        if (flag != 1) {
            alert("No item shipped, Cannot create delivery challan!");
            $('#btnSave').prop('disabled', true);
            return;
        }
        else
            $('#btnSave').prop('disabled', false);
    }


    createJson();
};
//==========end===============

//=====================Onchange of SO===========================
function SelectedIndexChangedSO(id) {

    $('#btnSave').prop('disabled', false);

    //For deleting the rows of Item table if exist.

    var table = document.getElementById('submissionTable');
    var rowCount = table.rows.length;
    while (rowCount != '1') {
        var row = table.deleteRow(rowCount - 1);
        rowCount--;
    }

    var SO_Id = $("#SO_Id").val();

    $.ajax({
        url: '/DeliveryChallan/GetSODetails',
        type: "POST",
        data: { id: SO_Id },
        success: function (result) {
            $("#SONumber").val(result[0].SONo);
            $("#CurrencyID").val(result[0].CurrencyID);
            $("#CurrencyName").val(result[0].CurrencyName);
            $("#CurrencyPrice").val(result[0].CurrencyPrice);
            $("#LocationId").val(result[0].LocationId);
            $("#LocationName").val(result[0].LocationName);
            $("#VendorsID").val(result[0].ClientID);
            $("#CompanyName").val(result[0].CompanyName);
            $('#ShippingAddress').val(result[0].DeliveryAddress);
            $('#SupplierAddress').val(result[0].SupplierAddress);
            $('#TermsAndCondition_ID').val(result[0].TermsAndConditionID);
            $('#Terms').val(result[0].Terms);

            var flag = 0;

            for (var j = 1; j < result.length; j++) {
                if (result[j].FinishedGoodQuantity != 0) {
                    flag = 1;
                    break;
                }
            }
            if (flag == 0) {
                alert('Please create FGS for the selected Sales Order!');
                window.location.href='/FinishedGoodSeries';
            }
            else
                $('#btnSave').prop('disabled', false);

            var table = document.getElementById('submissionTable');
            for (var j = 1; j < result.length; j++) {
                var rowCount = table.rows.length;
                var cellCount = table.rows[0].cells.length;
                var row = table.insertRow(rowCount);
                if (j % 2 == 0) {
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
                        cell.innerHTML = result[j].Item_ID;
                        cell.setAttribute("class", "d-none");
                        cell.setAttribute("id", "ItemID_" + j);
                    }
                    else if (i == 2) {
                        cell.innerHTML = result[j].ItemName;
                        cell.setAttribute("id", "ItemName_" + j);
                    }
                    else if (i == 3) {
                        cell.innerHTML = result[j].ItemQuantity + " " + result[j].ItemUnit;
                        cell.setAttribute("id", "SOQty_" + j);

                    }
                    else if (i == 4) {
                        cell.innerHTML = result[j].ItemTaxValue + " %";
                        cell.setAttribute("id", "ItemTaxValue_" + j);
                    }
                    else if (i == 5) {
                        cell.innerHTML = result[j].ItemUnitPrice + " " + result[j].CurrencyName;
                        cell.setAttribute("id", "ItemUnitPrice_" + j);
                    }
                    else if (i == 6) {
                        cell.innerHTML = result[j].FinishedGoodQuantity + " " + result[j].ItemUnit;
                        cell.setAttribute("id", "FinishedGoodQty_" + j);
                    }
                    else if (i == 7) {
                        cell.innerHTML = result[j].OutwardQuantity + " " + result[j].ItemUnit;
                        cell.setAttribute("id", "OutwardQty_" + j);
                    }
                    else if (i == 8) {
                        var t5 = document.createElement("input");
                        t5.id = "txtShippingQty_" + j;
                        t5.removeAttribute("disabled", "false");
                        t5.removeAttribute("disabled", "true");

                        if (parseFloat(result[j].ItemQuantity) == parseFloat(result[j].OutwardQuantity)) {
                            t5.setAttribute("disabled", "true");
                        }
                        else {
                            t5.removeAttribute("disabled", "false");
                            t5.removeAttribute("disabled", "true");
                            t5.setAttribute("onchange", "OnChangeQty($(this).val(),id)");
                        }

                       /* t5.setAttribute("type", "number");*/
                        t5.setAttribute("onkeypress", "return isNumberKey(event,id)");
                        t5.setAttribute("maxlength", "8");
                        t5.setAttribute("class", "form-control form-control-sm");

                        cell.appendChild(t5);

                        var t6 = document.createElement('span');
                        t6.id = "spanShippingQty_" + j;
                        t6.setAttribute("class", "text-wrap");
                        cell.appendChild(t6);

                    }
                    else if (i == 9) {
                        cell.innerHTML = result[j].BalanceQuantity + " " + result[j].ItemUnit;
                        cell.setAttribute("id", "BalQty_" + j);
                    }
                    else if (i == 10) {
                        cell.innerHTML = result[j].ItemUnit;
                        cell.setAttribute("id", "ItemUnit_" + j);
                    }

                    else if (i == 11) {
                        var t5 = document.createElement("input");
                        t5.id = "txtTotalItemCost_" + j;
                        t5.setAttribute("class", "form-control form-control-sm text-wrap");
                        t5.setAttribute("readonly", "readonly");
                        t5.setAttribute("value", "0");
                        cell.appendChild(t5);
                    }
                    else if (i == 12) {
                        cell.innerHTML = result[j].CurrencyName;
                        cell.setAttribute("id", "CurrencyName_" + j);
                    }
                    else if (i == 13) {
                        cell.innerHTML = result[j].BalanceQuantity;
                        cell.setAttribute("id", "txtBalQty_" + j);
                        cell.setAttribute("class", "d-none");
                    }
                }

            }

        },
        error: function (err) {
            alert('Not able to fetch SO item details!');

        }

    });
}
//=============End==============

function OnChangeQty(value, id) {
    $('#btnSave').prop('disabled', false);
    $('#spanShippingQty_' + rowNo).text('');
    var rowNo = id.split('_')[1];
    var BalQty = document.getElementById("txtBalQty_" + rowNo).innerHTML;
    if (BalQty == '' || BalQty == null)
        BalQty = 0;
    else
        BalQty = parseFloat(BalQty);

    var unitPrice = document.getElementById("ItemUnitPrice_" + rowNo).innerHTML;
    if (unitPrice == '' || unitPrice == null)
        unitPrice = 0;
    else
        unitPrice = parseFloat(unitPrice);

    var SOQty = document.getElementById("SOQty_" + rowNo).innerHTML;
    if (SOQty == '' || SOQty == null)
        SOQty = 0;
    else
        SOQty = parseFloat(SOQty);


    var outwardQty = document.getElementById("OutwardQty_" + rowNo).innerHTML;
    if (outwardQty == '' || outwardQty == null)
        outwardQty = 0;
    else
        outwardQty = parseFloat(outwardQty);

    if (value == '')
        value = 0;

    var ShippingQty = parseFloat(value);

    var AvlQtyOfFGS = document.getElementById('FinishedGoodQty_' + rowNo).innerHTML;
    if (AvlQtyOfFGS == '' || AvlQtyOfFGS == null)
        AvlQtyOfFGS = 0;
    else
        AvlQtyOfFGS = parseFloat(AvlQtyOfFGS);

    var DiffQty = 0;

    if (ShippingQty > AvlQtyOfFGS) {
        $('#spanShippingQty_' + rowNo).text('Shipped quantity cannot be greater than available quantity!');
        document.getElementById('spanShippingQty_' + rowNo).setAttribute('style', 'color:red;');
        document.getElementById(id).focus();
        $('#btnSave').prop("disabled", true);
        return;
    }
    else if (ShippingQty > BalQty) {
        $('#spanShippingQty_' + rowNo).text('Shipped quantity cannot be greater than balance quantity!');
        document.getElementById('spanShippingQty_' + rowNo).setAttribute('style', 'color:red;');
        document.getElementById(id).focus();
        $('#btnSave').prop("disabled", true);
        return;
    }
    else {
        $('#spanShippingQty_' + rowNo).text('');
        DiffQty = BalQty - ShippingQty;
        DiffQty = parseFloat(DiffQty);
        document.getElementById("BalQty_" + rowNo).innerHTML = DiffQty;

        document.getElementById("txtTotalItemCost_" + rowNo).value = Math.round(ShippingQty * unitPrice);

    }
    CalculateTotalBeforeTax();

}

function CalculateTotalBeforeTax() {
    $('#TotalBeforeTax').val('');
    $('#TotalTax').val('');
    var length = document.getElementById("submissionTable").rows.length;
    length = parseFloat(length) - 1;
    var total = 0;
    var totalTax = 0;
    var OtherTax = document.getElementById("OtherTax").value;

    if (OtherTax == '')
        OtherTax = 0;

    OtherTax = parseFloat(OtherTax);

    var DiscountVal = document.getElementById("DiscountPercentage").value;

    if (DiscountVal == '')
        DiscountVal = 0;

    DiscountVal = parseFloat(DiscountVal);

    var i = 1;
    while (i <= length) {
        var temp = document.getElementById("txtTotalItemCost_" + i).value;
        if (temp == '' || temp == null) {
            temp = 0;
        }
        else {
            temp = parseFloat(temp);
        }

        var tempTotalTax = ((document.getElementById("ItemTaxValue_" + i)).innerHTML).split(" %")[0];
        total = temp + total;

        if (tempTotalTax == '' || tempTotalTax == null) {
            tempTotalTax = 0;
        }
        else {
            tempTotalTax = parseFloat(tempTotalTax);
        }

        tempTotalTax = (parseFloat(tempTotalTax) / 100) * parseFloat(temp);
        totalTax = parseFloat(tempTotalTax) + totalTax;

        i++;
    }

    var tempDiscountVal = total * (DiscountVal / 100);

    $('#TotalBeforeTax').val(total.toFixed(2));
    $('#TotalTax').val(totalTax.toFixed(2));
    total = total - tempDiscountVal;
    var tempGrandTotal = total + totalTax + OtherTax;
    tempGrandTotal = Math.round(tempGrandTotal);
    $('#TotalAfterTax').val(tempGrandTotal);
    $('#GrandTotal').val(tempGrandTotal);

}

var TxtItemDetails = "";

function createJson() {

    var table = document.getElementById('submissionTable');
    var rowCount = table.rows.length;
    var i = 1;
    TxtItemDetails = "[";
    for (i = 1; i <= rowCount - 1; i++) {
        var ItemCode = (document.getElementById("ItemCode_" + i)).innerHTML;
        var ItemID = (document.getElementById("ItemID_" + i)).innerHTML;
        var ItemName = (document.getElementById("ItemName_" + i)).innerHTML;
        var SOQty = (document.getElementById("SOQty_" + i)).innerHTML.split(' ')[0];
        SOQty = (SOQty == null || SOQty == '') ? 0 : SOQty;

        var Tax = (document.getElementById("ItemTaxValue_" + i)).innerHTML.split(" %")[0];
        Tax = (Tax == null || Tax == '') ? 0 : Tax;

        var PricePerUnit = (document.getElementById("ItemUnitPrice_" + i)).innerHTML.split(' ')[0];
        PricePerUnit = (PricePerUnit == null || PricePerUnit == '') ? 0 : PricePerUnit;

        var OutwardQty = document.getElementById("OutwardQty_" + i).value;
        var ShippingQty = document.getElementById("txtShippingQty_" + i).value;
        var BalQty = (document.getElementById("BalQty_" + i)).innerHTML;
        var Unit = (document.getElementById("ItemUnit_" + i)).innerHTML;

        var TotalItemCost = (document.getElementById("txtTotalItemCost_" + i)).value;
        TotalItemCost = (TotalItemCost == null || TotalItemCost == '') ? 0 : TotalItemCost;

        var CurrencyName = (document.getElementById("CurrencyName_" + i)).innerHTML;
        var AvlQty = (document.getElementById("FinishedGoodQty_" + i)).innerHTML.split(' ')[0];

        TxtItemDetails = TxtItemDetails + "{\"Item_Code\":\"" + ItemCode + "\", \"ItemId\":" + ItemID +
            ", \"ItemName\": \"" + ItemName + "\", \"SOQty\": " + SOQty+ ",\"ItemTaxValue\": " + Tax +
            ", \"ItemUnitPrice\": " + PricePerUnit /*+ ", \"OutwardQty\": " + OutwardQty*/
            + ", \"ShippingQty\": " + ShippingQty + ", \"BalQty\": " + BalQty +
            ", \"ItemUnit\": \"" + Unit +  "\", \"CurrencyName\": \""
            + CurrencyName + "\", \"TotalItemCost\": " + TotalItemCost+",\"AvlQty\": "+AvlQty;

        if (i == (rowCount - 1))
            TxtItemDetails = TxtItemDetails + "}";
        else
            TxtItemDetails = TxtItemDetails + "},";
    }
    TxtItemDetails = TxtItemDetails + "]"
    $('#txtItemDetails').val(TxtItemDetails);
}

function isNumberKey(evt, id) {
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

function minmax(value, min, max) {

    //value = Math.round((value + Number.EPSILON) * 100) / 100;
    min = parseFloat(min);
    max = parseFloat(max);
    if (parseFloat(value) < min || isNaN(parseFloat(value)))
        return min;
    else if (parseFloat(value) > max)
        return min;
    else {
        return value;
    }
}