function create() {
    var id = $("#id").val();
    var parentId = $("#parentId").val();

    $.ajax({
        type: 'POST',
        url: "api/values?id=" + id + "&parentId=" + parentId,
        complete: function (data) {}
    });
}

function update() {
    var id = $("#id").val();
    var parentId = $("#parentId").val();

    $.ajax({
        type: 'PUT',
        url: "api/values?id=" + id + "&parentId=" + parentId,
        complete: function (data) {}
    });
}

function deleteFunc() {
    var Id = $('#Id').val();

    $.ajax({
        type: 'DELETE',
        url: "api/values/" + Id,
        complete: function (data) {}
    });
}

function get() {
    var Id = $('#Id').val();

    $.ajax({
        type: 'GET',
        url: "api/values/" + Id,
        complete: function (data) {}
    });
}

$(function () {
    $('#create').click(function () {
        create();
    });
    $('#update').click(function () {
        update();
    });
    $('#delete').click(function () {
        deleteFunc();
    });
    $('#get').click(function () {
        get();
    });
});
