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



// action-confirm
$actionconfirm = $(".confirm-action");
$newitemAc = $actionconfirm.clone();
$newitemAc.removeClass("confirm-action");
$newitemAc.unbind("click");
$actionconfirm.after($newitemAc);
$actionconfirm.hide();

$('#modActionConfirmDialog').find('.confirm-action-primary').click(function (e) {
    e.preventDefault();
    $actionconfirm.trigger('click');
});
$newitemAc.click(function (e) {
    e.preventDefault();
    console.log('action-confirm', e);
    var el = e.target;
    if ($(el).data("text"))
        $(modActionConfirmDialogText).html($(el).data("text"));
    if ($(el).data("submit-text"))
        $(".confirm-action-primary").html($(el).data("submit-text"));
    $('#modActionConfirmDialog').modal('show');
});
