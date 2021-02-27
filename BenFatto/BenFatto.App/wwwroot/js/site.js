$(document).ready(function () {
    $("[data-behavior~=\"menu-toggle\"]").on("click", function () {
        $("body").toggleClass("menu-expanded");
    });
    $("input[type=\"submit\"]").on("click", function (e) {
        e.preventDefault();
        e.stopPropagation();
        var form = $(this).closest("form");
        form.find("input,button,select").removeAttr("disabled");
        form.submit();
    });
    $("a[data-behavior~=\"display-modal\"]").on("click", function (e) {
        $("#ModalDetailTitle").html($(this).attr("data-arg"));
        $("#ModalDetailContent").load($(this).attr("href"));
    });
    $("[data-behavior=\"navigate-page\"]").on("click", function () {
        $("#page").val(parseInt($("#page").val()) + parseInt($(this).attr("data-arg")));
        if (parseInt($("#page").val()) < 0) {
            $("#page").val(0);
            return;
        }
        $("#page").closest("form").submit();
    });
    $("[data-behavior~=\"confirm-delete\"]").on("click", function (e) {
        e.preventDefault();
        e.stopPropagation();
        $("#ConfirmDeleteModalTitle").html($(this).attr("data-arg"));
        $("#ConfirmDelete").attr("href", $(this).attr("href"));
        $('#ConfirmDeleteModal').modal('show');
        $('#ConfirmDeleteModalBody').load($($(this).attr("data-target")).attr("href"));
    });
    $("[data-behavior~=\"clear-delete-modal\"]").on("click", function () {
        $("#ConfirmDeleteModalBody").html("");
        $("#ConfirmDeleteModalTitle").html("");
        $("#ConfirmDelete").attr("href", "");
    });
    $("[data-behavior~=\"edit-element\"]").on("click", function (e) {
        $("#ModalDetailTitle").html($(this).attr("data-arg"));
        var href = $(this).attr("href");
        $("#ModalDetailContent").load($(this).attr("href"), function (data) {
            $("#EditElementForm").attr("action", href);
        });
    });
});

