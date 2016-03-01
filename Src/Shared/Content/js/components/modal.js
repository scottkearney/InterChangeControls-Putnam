$(document).ready(function () {

    callPreview();

});


function callPreview()
{
    $('.interAction .ia-modal-inline-trigger-upload').magnificPopup({
        type: 'inline',
        preloader: false,
        closeBtnInside: true,
        showCloseBtn: true

    });
    $('.interAction .ia-modal-inline-trigger-taxonomy-grid').magnificPopup({
        type: 'inline',
        preloader: false,
        closeBtnInside: true,
        showCloseBtn: true

    });
    $('.interAction .ia-modal-inline-trigger-taxonomy').magnificPopup({
        type: 'inline',
        preloader: false,
        closeBtnInside: true,
        showCloseBtn: true

    });

    $('.interAction .ia-modal-inline-trigger').magnificPopup({
        type: 'inline',
        preloader: false,
        closeBtnInside: true,
        showCloseBtn: true,
        fixedBgPos: true//,
        //callbacks: {
        //    close: function () {
        //        unCheckSelections();
        //        UndoSelection();
        //    }
        //    // e.t.c.
        //}
    });

    $('.interAction .ia-modal-inline-trigger-preview').magnificPopup({
        type: 'inline',
        preloader: false,
        closeBtnInside: false,
        showCloseBtn: true,
        fixedBgPos: true//,
        //callbacks: {
        //    close: function () {
        //        unCheckSelections();               
        //    }
        //    // e.t.c.
        //}

    });

    $(document).on('click', '.interAction .ia-modal-dismiss', function (e) {
        e.preventDefault();
        $.magnificPopup.close();

    });

}