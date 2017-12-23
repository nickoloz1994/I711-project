// Datepicker functions
$('#datePicker').datepicker({
    todayBtn: "linked",
    clearBtn: true,
    autoclose: true,
    todayHighlight: true,
    startDate: new Date()
}).on('changeDate', showStartDate);

$('#datePicker2').datepicker({
    todayBtn: "linked",
    clearBtn: true,
    autoclose: true,
    todayHighlight: true,
    startDate: new Date()
}).on('changeDate', showEndDate);

function showStartDate() {
    var value = $('#datePicker').datepicker('getFormattedDate');
    $('#StartDate').val(value);
    if (value) {
        $('#datePicker2').datepicker('setStartDate', value);
    } else {
        var date = new Date();
        $('#datePicker2').datepicker('setStartDate', date);
    }
}

function showEndDate() {
    var value = $('#datePicker2').datepicker('getFormattedDate');
    $('#EndDate').val(value);
}