$(document).ready(function () {
    $(html).append("<style>#preloader{position: fixed;top: 0;left: 0;right: 0;bottom: 0;background-color: #ffffff;z-index: 99999;}#status{width: 200px;height: 200px;position: absolute;left: 50%;top: 50%;background-image: url('load.gif');background-repeat: no-repeat;background-position: center;margin: -80px 0 0 -100px;}</style>");

    $("#load").ajaxStart(function () {//ajax json çalışmadan çalışır
        //$(".div").css("opacity", "0.8");
        $(body).append("<div id='preloader'><div id='status'>&nbsp;</div></div>");
        $(this).fadeIn();
    }).ajaxComplete(function () {//  her şey tamamlandığında çalışır
        $(this).fadeOut();

    });
});

var Core =
{
    Ajax: function (url, type, entity, dataType, successFunc) {
        var model = {
            Id: entity.Id,
            Name: entity.Name,
            FirstName: entity.FirstName
        };

        jQuery.ajaxSetup({
            url: url,
            type: type,
            data: JSON.stringify(model),
            dataType: dataType,
            contentType: 'application/json; charset=utf-8',
            error: function () {
                alert("Ajax TimeOut aştı");
            },
            statusCode: {
                404: function () {
                    alert("Ajax Dosyası Bulunamadı");
                }
            },
        });

        jQuery.ajax({
            success: function (data) {
                successFunc(data);
            },
        });

        //$.ajaxSetup({
        //    url: '/Home/Get',
        //    dataType: 'json',
        //    contentType: 'application/json; charset=utf-8',
        //    error: function () {
        //        alert("Ajax TimeOut aştı");
        //    },
        //    statusCode: {
        //        404: function () {
        //            alert("Ajax Dosyası Bulunamadı");
        //        }
        //    },
        //});
        //jQuery.ajax({
        //    url: url,
        //    type: type,
        //    data: JSON.stringify(model),
        //    dataType: dataType,
        //    contentType: 'application/json; charset=utf-8',
        //    success: function (data) {
        //        successFunc(data);
        //    },

        //    error: function (error) {
        //        alert("Hata: " + ' ' + error, { autoclose: 3000, position: "top-right", type: "st-error" });
        //    },
        //});
    }
};
