$(document).ready(function () {

    function ShowSpinner(){
        console.log("showSpinner")
        $(".spinner").removeClass("hideSpinner")
        $(".spinner").addClass("showSpinner")
    }

    function HideSpinner(){
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
    
    //$("#inputDate").inputmask();

    

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
            PhoneNumber:{
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
            Date:{
                required: true,
                date: true
            },
            ConsultationsNumber:{
                required: true
            }
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
            PhoneNumber:{
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
            Date:{
                required: "Поле должно быть заполнено",
                date: "Введите правильную дату"
            },
            ConsultationsNumber:{
                required: "Поле должно быть заполнено"
            }
        },
    });
    
    
    $('#inputDate').inputmask('99/99/9999');

    $(".addDate").on("click", function(e) {

        if($(".box").valid()){
            e.preventDefault();

            ShowSpinner();
            $.ajax({
                url: "/Consultation/CreateConsultationDate",
                method: 'post',

                data: $(".box").serialize(),
                success: function (data) {
                    HideSpinner();
                    console.log(data)

                    var $summaryUl = $(".validation-summary-errors");
                    
                    console.log($summaryUl)
                    $summaryUl.empty();

                    if (data.status !== 'validationerror') {
                        $('#dialogContent').html(data);
                        $('#modDialog').modal('show');
                    }
                    else {
                        $summaryUl
                            .append($("<ul>")
                                .append($("<li>")
                                    .text(data.message)))
                    }
                }
            });
        }
        
    })


    $(".createConsultation").on("click", function(e) {

        if($(".box").valid()){
            e.preventDefault();

            ShowSpinner();
            $.ajax({
                url: "/Consultation/CreateUserConsultation",
                method: 'post',

                data: $(".box").serialize(),
                success: function (data) {
                    HideSpinner();
                    console.log(data)

                    var $summaryUl = $(".validation-summary-errors");

                    console.log($summaryUl)
                    $summaryUl.empty();

                    if (data.status !== 'validationerror') {
                        $('#dialogContent').html(data);
                        $('#modDialog').modal('show');
                    }
                    else {
                        $summaryUl
                            .append($("<ul>")
                                .append($("<li>")
                                    .text(data.message)))
                    }
                }
            });
        }

    })

    var dlg = $('#dialog').dialog({
        modal: false,
        autoOpen: false,
        draggable: false,
        resizable: false,
        width: 350,
    });

    $('#inputDate').datepicker({
        altFormat: "dd/mm/yy",
        dateFormat: "dd/mm/yy",
        minDate: 1,

    }).on('change', function () {
        dlg.dialog("close");
        $(this).valid();
    });


    $("html").on("mouseenter", ".ui-datepicker-calendar td", function () {

        var day = parseInt($(this).text(), 10);
        var month = parseInt($(this).attr('data-month'));
        var year = $(this).attr('data-year');
        
        if(!!$(this).attr('data-month')){
            date = new Date(year, month, day);
            var SendDate = { "date": date.getFullYear()+"-"
                    +zeroPadded(date.getMonth() + 1)+"-"
                    +zeroPadded(date.getDate())};

            console.log(SendDate)

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
                    
                    if(!!data){
                        console.log(data.maxConsultationNumber)
                        $('.dialog-content').html(
                            `<p>Максимальное количество консультаций: ${data.maxConsultationNumber}</p>` + 
                            `<p>Зарегистрировано: ${data.consultationNumber}</p>`
                        )
                    }
                    else {
                        $('.dialog-content').html("Эта дата не зарегистрированна")
                    }
                    

                },
                error: function (error){
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
