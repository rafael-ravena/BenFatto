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
        ClearDetailModal();
        $("#DetailModalTitle").html($(this).attr("data-arg"));
        $("#DetailModalBody").load($(this).attr("href"), function (response, status, xhr) {
            if ("error" == status) {
                DisplayErrorMessage("#DetailModalBody");
                $("#DetailModalFooter").removeClass("d-none");
            }
        });
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
        var deleteUrl = $(this).attr("href");
        ClearDeleteModal();
        $("#ConfirmDeleteModalTitle").html($(this).attr("data-arg"));
        $('#ConfirmDeleteModal').modal('show');
        $('#ConfirmDeleteModalBody').load($($(this).attr("data-target")).attr("href"), function (response, status, xhr) {
            if ("error" == status) {
                DisplayErrorMessage("#ConfirmDeleteModalBody");
                $("#ConfirmDelete").addClass("d-none");
            } else if ("success" == status) {
                $('#ConfirmDeleteModalBody').prepend(GetDeleteWarning());
                $("#Action").addClass("d-none");
                $("#ConfirmDelete").attr("href", deleteUrl);
            }
        });
    });
    $("[data-behavior~=\"clear-delete-modal\"]").on("click", function () {
        ClearDeleteModal();
    });

    $("[data-behavior~=\"edit-element\"]").on("click", function (e) {
        ClearDetailModal();
        $("#DetailModalTitle").html($(this).attr("data-arg"));
        var href = $(this).attr("href");
        $("#DetailModalBody").load($(this).attr("href"), function (response, status, xhr) {
            if ("error" == status) {
                DisplayErrorMessage("#DetailModalBody");
                $("#DetailModalFooter").removeClass("d-none");
            } else if ("success" == status) {
                $("#EditElementForm").attr("action", href);
            }
        });
    });
    $('.table').stickyTableHeaders();
});

function GetDeleteWarning() {
    return $("#DeleteWarning").html();
}
function GetLoadingContent() {
    return $("#LoadingSign").html()
}
function ClearDetailModal() {
    $("#DetailModalFooter").addClass("d-none");
    $("#DetailModalBody").html(GetLoadingContent());
    $("#DetailModalTitle").html("");
}
function ClearDeleteModal() {
    $("#ConfirmDeleteModalBody").html(GetLoadingContent());
    $("#ConfirmDeleteModalTitle").html("");
    $("#ConfirmDelete").attr("href", "");
    $("#ConfirmDelete").removeClass("d-none");
}
function DisplayErrorMessage(modalElement) {
    $(modalElement).html($("#DeleteFail").html());
}