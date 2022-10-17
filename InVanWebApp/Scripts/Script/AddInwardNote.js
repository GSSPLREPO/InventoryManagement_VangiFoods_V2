
//<script type="text/javascript">

    var InwardQuantities = "";
    var BalanceQuantities = "";
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
        cell.innerHTML = result[j].ItemName;
                        }
    else if (i == 1) {
        cell.innerHTML = result[j].Item_Code;
                        }
    else if (i == 2) {
        cell.innerHTML = result[j].ItemQuantity + " " + result[j].ItemUnit;
    cell.setAttribute("id", "ItemQty" + j);
                        }
    else if (i == 3) {
        cell.innerHTML = result[j].ItemTaxValue + " %";
                        }
    else if (i == 4) {
        cell.innerHTML = result[j].ItemUnitPrice + " Rs";
                        }
    else if (i == 5) {

        cell.innerHTML = result[j].InwardQuantity;
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
                    }

                }
            }
        });
    }

    function OnChangeIWQty(value, id) {

        var rowNo = id.split('y')[1];
    var cell = document.getElementById("ItemQty" + rowNo);
    var temp_itemQty = cell.innerHTML.split(' ');
    cell= document.getElementById("DeliveredQty" + rowNo);
    var deliveredQty = cell.innerHTML;
    //console.log(deliveredQty);
    var itemQty = parseFloat(temp_itemQty[0])-parseFloat(deliveredQty);
    value = parseFloat(value);

        if (value > itemQty) {
        alert("Inwarding quantity cannot be greater then balanced quantity!");
    document.getElementById(id).focus();
    document.getElementById(id).setAttribute("style", "border-color:red;");
    return;
        }
    else {
            var tempInwQty = document.getElementById("txtInwardQty" + rowNo).value;
    console.log(tempInwQty + " " + temp_itemQty[0] + " " + deliveredQty);
    document.getElementById("txtBalanceQty" + rowNo).value = parseFloat(temp_itemQty[0])-(parseFloat(deliveredQty) + parseFloat(tempInwQty));
    InwardQuantities = InwardQuantities + "txtInwardQty" + rowNo + "*" + value + ",";

    var BalQty=document.getElementById("txtBalanceQty" + rowNo).value;
    BalanceQuantities = BalanceQuantities + "txtBalanceQty" + rowNo + "*" +BalQty+",";
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
        $('#InwardQuantities').val(InwardQuantities);
    $('#BalanceQuantities').val(BalanceQuantities);
        //alert($('#InwardQuantities').val());
        //alert($('#BalanceQuantities').val());
    }

//</script>