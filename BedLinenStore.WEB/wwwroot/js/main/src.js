$(document).ready(function ($) {
    function myfunction() {
        $.ajax({
            url: "Main/SendRequest",
            method: 'post',

            data: $(".box").serialize(),
            dataType: 'html',
            success: function (data) {
                $('#dialogContent').html(data);
                $('#modDialog').modal('show');
            }
        });
    }

    $("#phoneNumber").inputmask("+375 (99) 99-99-999",
        {
            "clearIncomplete": true,
        });

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
            Email: {
                required: true,
                regex: "^[^@\\s]+@([^@\\s]+\\.)+[^@\\s]+$",
            },
            Name: {
                required: true,
                minlength: 4
            },
            PhoneNumber: {
                required: true,
            }
        },
        messages: {
            Email: {
                required: "Поле должно быть заполнено",
                regex: "Ваш адрес электронной почты должен быть в формате name@domain.com",
            },
            Name: {
                required: "Поле должно быть заполнено",
                minlength: "Минимальная длина 4"
            },
            PhoneNumber: {
                required: "Поле должно быть заполнено",
            }
        },
        submitHandler: function (e) {
            myfunction();
        },
    });
});



