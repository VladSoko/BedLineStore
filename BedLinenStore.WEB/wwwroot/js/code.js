$(document).ready(function () {
    jQuery.validator.addMethod(
        "regex",
        function (value, element, regexp) {
            if (regexp.constructor != RegExp)
                regexp = new RegExp(regexp);
            else if (regexp.global)
                regexp.lastIndex = 0;
            return this.optional(element) || regexp.test(value);
        }, "erreur expression reguliere"
    );

    var userLang = navigator.language || navigator.userLanguage;

    var dateFormat;
    var dateRegexFormat;

    switch (userLang) {
        case 'en-US':
            dateFormat = "mm/dd/yy"
            dateRegexFormat = "^(0[1-9]|1[012])[- \\/.](0[1-9]|[12][0-9]|3[01])[- \\/.]((?:19|20)\\d\\d)$"
            break;
        default:
            dateFormat = "dd/mm/yy";
            dateRegexFormat = "^(?:(?:31(\\/|-|\\.)(?:0?[13578]|1[02]))\\1|(?:(?:29|30)(\\/|-|\\.)(?:0?[13-9]|1[0-2])\\2))(?:(?:1[6-9]|[2-9]\\d)?\\d{2})$|^(?:29(\\/|-|\\.)0?2\\3(?:(?:(?:1[6-9]|[2-9]\\d)?(?:0[48]|[2468][048]|[13579][26])|(?:(?:16|[2468][048]|[3579][26])00))))$|^(?:0?[1-9]|1\\d|2[0-8])(\\/|-|\\.)(?:(?:0?[1-9])|(?:1[0-2]))\\4(?:(?:1[6-9]|[2-9]\\d)?\\d{2})$"
    }

    function ShowSpinner() {
        console.log("showSpinner")
        $(".spinner").removeClass("hideSpinner")
        $(".spinner").addClass("showSpinner")
    }

    function HideSpinner() {
        console.log("hideSpinner")
        $(".spinner").removeClass("showSpinner")
        $(".spinner").addClass("hideSpinner")
    }

    function zeroPadded(val) {
        if (val >= 10)
            return val;
        else
            return '0' + val;
    }

    $("#inputPhoneNumber").inputmask("+375 (99) 99-99-999",
        {
            "clearIncomplete": true,
        });

    $("#inputFromDate, #inputToDate, #inputDate").inputmask('99/99/9999',
        {
            "clearIncomplete": true,
        });

    var dlg = $('#dialog').dialog({
        modal: false,
        autoOpen: false,
        draggable: false,
        resizable: false,
        width: 350,
    });

    $(".box").validate({
        rules: {
            Name: {
                required: true,
                minlength: 3,
            },
            Surname: {
                required: true,
                minlength: 3,
            },
            PhoneNumber: {
                required: true,
            },
            Email: {
                required: true,
                regex: "^[^@\\s]+@([^@\\s]+\\.)+[^@\\s]+$",
            },
            Password: {
                required: true,
                minlength: 4
            },
            ConfirmPassword: {
                required: true,
                minlength: 4,
                equalTo: "#inputPassword"
            },
            Date: {
                required: true,
                date: false,
                regex: dateRegexFormat
            },
            ConsultationsNumber: {
                required: true
            },
            FromDate: {
                required: true,
                regex: dateRegexFormat
            },
            ToDate: {
                required: true,
                regex: dateRegexFormat
            },
            Country: {
                required: true,
                minlength: 4,
            },
            City: {
                required: true,
                minlength: 4,
            },
            Street: {
                required: true,
                minlength: 4,
            },
            Zip: {
                required: true,
                digits: true,
                minlength: 4,
            },
        },
        messages: {
            Email: {
                required: "Поле должно быть заполнено",
                regex: "Ваш адрес электронной почты должен быть в формате name@domain.com",
            },
            Name: {
                required: "Поле должно быть заполнено",
                minlength: "Минимальная длина 3",
            },
            Surname: {
                required: "Поле должно быть заполнено",
                minlength: "Минимальная длина 3",
            },
            PhoneNumber: {
                required: "Поле должно быть заполнено",
            },
            Password: {
                required: "Поле должно быть заполнено",
                minlength: "Минимальная длина 4"
            },
            ConfirmPassword: {
                required: "Поле должно быть заполнено",
                minlength: "Минимальная длина 4",
                equalTo: "Пароли должны совпадать"
            },
            Date: {
                required: "Поле должно быть заполнено",
                regex: "Введите правильную дату"
            },
            ConsultationsNumber: {
                required: "Поле должно быть заполнено"
            },
            FromDate: {
                required: "Поле должно быть заполнено",
                regex: "Введите правильную дату"
            },
            ToDate: {
                required: "Поле должно быть заполнено",
                regex: "Введите правильную дату"
            },
            Country: {
                required: "Поле должно быть заполнен",
                minlength: "Минимальная длина 4",
            },
            City: {
                required: "Поле должно быть заполнен",
                minlength: "Минимальная длина 4",
            },
            Street: {
                required: "Поле должно быть заполнен",
                minlength: "Минимальная длина 4",
            },
            Zip: {
                required: "Поле должно быть заполнен",
                digits: "Только цифры",
                minlength: "Минимальная длина 4",
            },
        },
    });
    
    function workWithData(url, method, e){
        if ($(".box").valid()) {
            e.preventDefault();

            ShowSpinner();
            $.ajax({
                url: url,
                method: method,

                data: $(".box").serialize(),
                success: function (data) {
                    HideSpinner();

                    if (data.status !== 'validationerror') {
                        $('#dialogContent').html(data);
                        $('#modDialog').modal('show');
                    } else {
                        var $summaryUl = $(".validation-summary-errors");
                        $summaryUl.empty();

                        $summaryUl
                            .append($("<ul>")
                                .append($("<li>")
                                    .text(data.message)))
                    }
                }
            });
        }
    }

    $(".freelance-sewing-button").on("click", function (e) {
        workWithData("Main/SendRequest", 'post', e)
    });

    $(".place-order").on("click", function (e) {
        let date = new Date();
        $("input[name='CreatedDate']").val(date.getFullYear()+"-"
            +zeroPadded(date.getMonth() + 1)+"-"
            +zeroPadded(date.getDate()));

        workWithData("Cart/Checkout", 'post', e)
    });

    $(".addDate").on("click", function (e) {
        workWithData("/Consultation/CreateConsultationDate", 'post', e)
    })
    
    $(".createConsultation").on("click", function (e) {
        workWithData("/Consultation/CreateUserConsultation", 'post', e)
    })

    $(".forgotPassword").on("click", function(e) {
        workWithData("/ResetPassword/ForgotPassword", 'post', e)
    })

    $(".register").on("click", function(e) {
        workWithData("/Account/Register", 'post', e)
    })
    
    
    $('#inputDate').datepicker({
        altFormat: dateFormat,
        dateFormat: dateFormat,
        minDate: 1,

    }).on('change', function () {
        dlg.dialog("close");
        $(this).valid();
    }).datepicker('setDate', new Date());

    $('#inputFromDate, #inputToDate').datepicker({
        altFormat: dateFormat,
        dateFormat: dateFormat,
        onSelect: function () {
            $(".box").valid();
        }
    });
    
    $("html").on("mouseenter", ".ui-datepicker-calendar td", function () {

        let day = parseInt($(this).text(), 10);
        let month = parseInt($(this).attr('data-month'));
        let year = $(this).attr('data-year');

        let date;
        if (!!$(this).attr('data-month')) {
            date = new Date(year, month, day);
            let SendDate = {
                "date": date.getFullYear() + "-"
                    + zeroPadded(date.getMonth() + 1) + "-"
                    + zeroPadded(date.getDate())
            };

            $('.dialog-content').html("")
            dlg.dialog("option", "position", {
                my: "left-20 top+40",
                of: event,
                offset: "50 50"
            })
            $(".spinner-modal").removeClass("hide-spinner")
            $(".spinner-modal").addClass("show-modal-spinner")
            dlg.dialog("open");

            $.ajax({
                type: "GET",
                url: "/Consultation/GetNumberConsultationByDate",
                data: SendDate,
                dataType: 'json',
                contentType: "application/json; charset=utf-8",
                success: function (data) {

                    $(".spinner-modal").removeClass("show-modal-spinner")
                    $(".spinner-modal").addClass("hide-spinner")

                    if (!!data) {
                        console.log(data.maxConsultationNumber)
                        $('.dialog-content').html(
                            `<p>Максимальное количество консультаций: ${data.maxConsultationNumber}</p>` +
                            `<p>Зарегистрировано: ${data.consultationNumber}</p>`
                        )
                    } else {
                        $('.dialog-content').html("Эта дата не зарегистрированна")
                    }


                },
                error: function (error) {
                    $(".spinner-modal").removeClass("show-modal-spinner")
                    $(".spinner-modal").addClass("hide-spinner")
                    $('.dialog-content').html("Ошибка на сервере")
                }
            });
        }


    });

    $("html").on("mouseout", ".ui-state-default", function () {
        dlg.dialog("close");
    });
});