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

    $(".modal-content").on("change", "[id]:not([data-behavior~=\"search-object\"])", function (e) {
        e.stopPropagation();
        ClearElementValidation($(this));
        var theForm = $(this).closest("form");
        var apiUrl = theForm.attr("data-search-row");
        var body = JSON.stringify(GetFormSerializedData(theForm));
        AjaxPost(apiUrl, body, function (response) {
            $("#OriginalLine").val(response);
        }, function (response) {
            toastr["warning"](response.responseText, response.statusText);
        });
    });

    $(".modal-content").on("change", "[data-behavior~=\"search-object\"]", function (e) {
        e.stopPropagation();
        ClearElementValidation($(this));
        var apiUrl = $(this).attr("data-search-object");
        var importId = $("[name=\"ImportId\"]").val();
        var rowNumber = $("[name=\"RowNumber\"]").val()
        var value = $(this).val();
        if ("" == rowNumber) {
            rowNumber = 0;
        }
        var reqBody = JSON.stringify({ ImportId: importId, RowNumber: rowNumber, Row: value });
        AjaxPost(apiUrl, reqBody, function (response) {
            BindSerializedDataToForm(response);
        }, function (response) {
            toastr["warning"](response.responseText, response.statusText);
        });
    });

    $(".modal-content").on("submit", "form", function (e) {
        var elements = $(this).find("input,select");
        $(".error-message").remove()
        $(elements).removeClass("border-danger");
        var valid = true;
        for (var i = 0; i < elements.length; i++) {
            if (undefined !== $(elements[i]).attr("data-required") && "" == $(elements[i]).val()) {
                valid = false;
                $(elements[i]).addClass("border-danger");
                $(elements[i]).after("<b class=\"error-message text-danger\">This field is required!</b>");
            }
        }
        if (!valid) {
            e.preventDefault();
            e.stopPropagation();
        } else {
            AddLoadingDisplay($(this));
        }
    });

    $('.table').stickyTableHeaders();
    ShowToarstrMessage();
});
function AddLoadingDisplay(element) {
    $backdrop = $("<div class=\"loader-container\"/>")
    element.append($backdrop);
    $loader = $("<div class=\"loader\"/>");
    $backdrop.append($loader);
}
function ClearElementValidation(element) {
    element.parent().find(".error-message").remove()
    element.removeClass("border-danger");
}

function AjaxPost(theUrl, body, successHandler, errorHandler) {
    $.ajax({
        url: theUrl,
        contentType: "application/json; charset=utf-8",
        accepts: "text/plain",
        type: "post",
        data: body,
        crossDomain: true,
        success: successHandler,
        error: errorHandler
    });
}
function BindSerializedDataToForm(data) {
    $.each(data, function (name, value) {
        var $element = $("[name=\"" + name + "\"]");
        $element.val(value);
    });
}
function GetFormSerializedData($element) {
    var elementData = $element.serializeArray();
    var indexedElementData = {};
    $.map(elementData, function (i, j) {
        indexedElementData[i["name"]] = i["value"]
    });
    return indexedElementData;
}
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
function ShowToarstrMessage() {
    var msgType = $("#ToastrType").html();
    var message = $("#ToastrMsg").html();
    if (msgType && message) {
        toastr[msgType](message);
    }
}
toastr.options = {
    "closeButton": false,
    "debug": false,
    "newestOnTop": true,
    "progressBar": true,
    "positionClass": "toast-top-full-width",
    "preventDuplicates": false,
    "onclick": null,
    "showDuration": "300",
    "hideDuration": "1000",
    "timeOut": "15000",
    "extendedTimeOut": "15000",
    "showEasing": "swing",
    "hideEasing": "linear",
    "showMethod": "fadeIn",
    "hideMethod": "fadeOut"
}
