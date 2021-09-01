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
        Password: {
            required: true,
            minlength: 4
        },
        ConfirmPassword: {
            required: true,
            minlength: 4,
            equalTo: "#inputPassword"
        }
    },
    messages: {
        Email: {
            required: "Поле должно быть заполнено",
            regex: "Ваш адрес электронной почты должен быть в формате name@domain.com",
        },
        Password: {
            required: "Поле должно быть заполнено",
            minlength: "Минимальная длина 4"
        },
        ConfirmPassword: {
            required: "Поле должно быть заполнено",
            minlength: "Минимальная длина 4",
            equalTo: "Пароли должны совпадать"
        }
    },
});