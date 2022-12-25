
$(document).ready(function () {
    if ($.isFunction($.fn.select2)) {
        $("select.select2").select2();
    }
});

$(".popupform").unbind('click');
$(".popupform").click(function (e) {
    e.preventDefault();
    var link = this;
    $.get(link.href, function (data) {
        $('#dialogContent').html(data);
        $('#modDialog').modal({ backdrop: true });
        if (link.dataset['onsuccess']) {
            $('#modDialog .btn-primary, #modDialog .btn-gss-primary').on('click', function () { eval(link.dataset['onsuccess']) } );
        }
    });
});

$(".popupform-lg").unbind('click');
$(".popupform-lg").click(function (e) {
    e.preventDefault();
    var link = this;
    $.get(link.href, function (data) {
        $('#dialogContentLg').html(data);
        $('#modDialogLg').modal({ backdrop: true });
        if (link.dataset['onsuccess']) {
            $('#modDialogLg .btn-primary').on('click', function () { eval(link.dataset['onsuccess']) });
        }
    });
});

// delete-confirm
function InitDeleteConfirm() {
    //console.log('InitDeleteConfirm');
    $deleteconfirm = $(".delete-confirm:visible");
    $(".delete-confirm:visible").each(function (idx, el) {
        $deleteconfirm = $(el);
        $newitem = $deleteconfirm.clone();
        $newitem.removeClass("delete-confirm");
        $newitem.unbind("click");
        $deleteconfirm.after($newitem);
        $deleteconfirm.hide();

        // установим привязку к исходному элементу
        if (!el.id) el.id = 'delete-confirm-' + uuid();
        $newitem.data('target', el.id);

        // open modal dialog
        $newitem.click(function (e) {
            e.preventDefault();
            //console.log('delete-confirm', $(this).data('target'));
            $('#modDeleteConfirmDialog').data('target',$(this).data('target'))
            $('#modDeleteConfirmDialog').modal('show');
        });

    });
    // click OK in modal dilog
    $('#modDeleteConfirmDialog').find('.delete-confirm-action').unbind("click");
    $('#modDeleteConfirmDialog').find('.delete-confirm-action').click(function (e) {
        e.preventDefault();
        //console.log('InitDeleteConfirm - click', $('#modDeleteConfirmDialog').data('target'));
        if ($('#modDeleteConfirmDialog').data('target'))
            document.getElementById($('#modDeleteConfirmDialog').data('target')).click();
    });
}
InitDeleteConfirm();


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

