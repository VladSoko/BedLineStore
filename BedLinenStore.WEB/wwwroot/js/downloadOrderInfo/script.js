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

    function zeroPadded(val) {
        if (val >= 10)
            return val;
        else
            return '0' + val;
    }
    
    


    $(".box").validate({
        rules: {
            FromDate:{
                required: true,
                regex: "^(?:(?:31(\\/|-|\\.)(?:0?[13578]|1[02]))\\1|(?:(?:29|30)(\\/|-|\\.)(?:0?[13-9]|1[0-2])\\2))(?:(?:1[6-9]|[2-9]\\d)?\\d{2})$|^(?:29(\\/|-|\\.)0?2\\3(?:(?:(?:1[6-9]|[2-9]\\d)?(?:0[48]|[2468][048]|[13579][26])|(?:(?:16|[2468][048]|[3579][26])00))))$|^(?:0?[1-9]|1\\d|2[0-8])(\\/|-|\\.)(?:(?:0?[1-9])|(?:1[0-2]))\\4(?:(?:1[6-9]|[2-9]\\d)?\\d{2})$"
            },
            ToDate:{
                required: true,
                regex: "^(?:(?:31(\\/|-|\\.)(?:0?[13578]|1[02]))\\1|(?:(?:29|30)(\\/|-|\\.)(?:0?[13-9]|1[0-2])\\2))(?:(?:1[6-9]|[2-9]\\d)?\\d{2})$|^(?:29(\\/|-|\\.)0?2\\3(?:(?:(?:1[6-9]|[2-9]\\d)?(?:0[48]|[2468][048]|[13579][26])|(?:(?:16|[2468][048]|[3579][26])00))))$|^(?:0?[1-9]|1\\d|2[0-8])(\\/|-|\\.)(?:(?:0?[1-9])|(?:1[0-2]))\\4(?:(?:1[6-9]|[2-9]\\d)?\\d{2})$"
            },
        },
        messages: {
            FromDate:{
                required: "Поле должно быть заполнено",
                regex: "Введите правильную дату"
            },
            ToDate:{
                required: "Поле должно быть заполнено",
                regex: "Введите правильную дату"
            }
        },
    });

    // $('#inputEmail, #inputPassword').inputmask('99/99/9999', );

    $("#inputEmail, #inputPassword").inputmask('99/99/9999',
        {
            "clearIncomplete": true,
        });

    $('#inputPassword').datepicker({

        altFormat: "dd/mm/yy",
        dateFormat: "dd/mm/yy",
        maxDate: 365,
        onSelect: function () {
            $(".box").valid();
        }
    });

    $('#inputEmail').datepicker({
        altFormat: "dd/mm/yy",
        dateFormat: "dd/mm/yy",
        onSelect: function () {
            $(".box").valid();
        }
    });
    
})