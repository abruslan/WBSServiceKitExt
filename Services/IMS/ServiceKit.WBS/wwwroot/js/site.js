
$(document).ready(function () {
    if ($.isFunction($.fn.select2)) {
        $("select.select2").select2();
    }
});

// Отправляем DIV на печать в том же окне. 
// После печати обновляем страницу для восстановления event, которые могли поломаться
function printDiv(divId) {
    var printContent = document.getElementById(divId).innerHTML;
    var originalContent = document.body.innerHTML;

    document.body.innerHTML = printContent;
    window.print();
    //document.body.innerHTML = originalContent;
    location.reload();
}

// отправляем DIV на печать в новом окне
function printDivExtra(divId) {
    var printWindow = window.open("", "");
    var doc = printWindow.document;
    var head = document.getElementsByTagName('head')[0].innerHTML;

    doc.open();
    doc.write('<html><head>' + head + '</head><body>' + document.getElementById(divId).innerHTML + '</body></html>');
    doc.close();

    printWindow.print();
    printWindow.close();
}

function uuid() {
    return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
        var r = Math.random() * 16 | 0, v = c == 'x' ? r : (r & 0x3 | 0x8);
        return v.toString(16);
    });
}

