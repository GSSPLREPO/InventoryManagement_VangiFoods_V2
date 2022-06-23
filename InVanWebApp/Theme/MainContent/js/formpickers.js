(function($) {
  'use strict';
  if ($("#timepicker-example").length) {
    $('#timepicker-example').datetimepicker({
      format: 'LT'
    });
  }
  if ($(".color-picker").length) {
    $('.color-picker').asColorPicker();
  }
  if ($("#datepicker-popup").length) {
    $('#datepicker-popup').datepicker({
      enableOnReadonly: true,
      todayHighlight: true,
    });
  }
  if ($("#inline-datepicker").length) {
    $('#inline-datepicker').datepicker({
      enableOnReadonly: true,
      todayHighlight: true,
    });
  }
  if ($(".datepicker-autoclose").length) {
    $('.datepicker-autoclose').datepicker({
      autoclose: true
    });
  }
  if ($('input[name="date-range"]').length) {
    $('input[name="date-range"]').daterangepicker();
  }
  if ($('input[name="date-time-range"]').length) {
    $('input[name="date-time-range"]').daterangepicker({
      timePicker: true,
      timePickerIncrement: 30,
      locale: {
        format: 'MM/DD/YYYY h:mm A'
      }
    });
  }
})(jQuery);

//Written the below script for second datepicker on the same page, just need to change ID of second datepicker.

(function ($) {
  'use strict';
  if ($("#timepicker-example1").length) {
    $('#timepicker-example1').datetimepicker({
      format: 'LT'
    });
  }
  if ($(".color-picker1").length) {
    $('.color-picker1').asColorPicker();
  }
  if ($("#datepicker-popup1").length) {
    $('#datepicker-popup1').datepicker({
      enableOnReadonly: true,
        todayHighlight: true,
        //format: 'dd/mm/yyyy'
    });
  }
  if ($("#inline-datepicker1").length) {
    $('#inline-datepicker1').datepicker({
      enableOnReadonly: true,
      todayHighlight: true,
    });
  }
  if ($(".datepicker-autoclose1").length) {
    $('.datepicker-autoclose1').datepicker({
        autoclose: true        
    });
  }
  if ($('input1[name="date-range"]').length) {
    $('input1[name="date-range"]').daterangepicker();
  }
  if ($('input1[name="date-time-range"]').length) {
    $('input1[name="date-time-range"]').daterangepicker({
      timePicker: true,
      timePickerIncrement: 30,
      locale: {
        format: 'MM/DD/YYYY h:mm A'
      }
    });
  }
})(jQuery);