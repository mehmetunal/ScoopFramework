function feedback(feedback) {


    if (feedback == "" || feedback == null || feedback == "null") return false;

    var feedbackObj = JSON.parse(feedback);

    toastr.options = {
        "closeButton": true,
        "debug": false,
        "newestOnTop": false,
        "progressBar": true,
        "positionClass": "toast-top-right",
        "preventDuplicates": false,
        "onclick": null,
        "showDuration": "300",
        "hideDuration": "1000",
        "timeOut": feedbackObj.timeout * 1000,
        "extendedTimeOut": 0,
        "showEasing": "swing",
        "hideEasing": "linear",
        "showMethod": "fadeIn",
        "hideMethod": "fadeOut",
        "tapToDismiss": false
    }


    if (feedbackObj.action != "") {
        toastr.options.onHidden = function (a) { location.href = feedbackObj.action; }
    }

    if (feedbackObj.message != "" && feedbackObj.status != "") {
        toastr[feedbackObj.status](feedbackObj.message, feedbackObj.title);
    }

}