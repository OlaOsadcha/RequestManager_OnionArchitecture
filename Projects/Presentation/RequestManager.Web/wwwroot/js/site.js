
$(function ()
{
    var PlaceHolderRequest = $('#PlaceHolderRequest');
    $('button[data-toggle="ajax-modal"]').click(function (event) {

        var url = $(this).data('url');
        $.get(url).done(function (data) {
            PlaceHolderRequest.html(data);
            PlaceHolderRequest.find('.modal').modal('show');
        })
    })

    PlaceHolderRequest.on('click', '[data-save="modal"]', function (event)
    {
        var form = $(this).parents('.modal').find('form');
        var actionUrl = form.attr('action');
        var sendData = form.serialize();

        $.post(actionUrl, sendData).done(function (data) {
            location.reload(true);  
            PlaceHolderRequest.find('.modal').modal('hide');
        })
    })
})


//function OpenDetailsModal(id) {
//    var data = id;
//    $.ajax(
//        {
//            type: 'GET',
//            url: '/Request/Details',
//            contentType: 'application/json; charset=utf=8',
//            data: data,
//            success: function (result) {
//                $('#modal-details-content').html(result);
//                $('#modal-details').modal('show');
//            },
//            error: function (er) {
//                alert(er);
//            }
//        });
//}