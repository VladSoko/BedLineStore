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

$(".sendEmail").on("click", function(e) {
    e.preventDefault(); // Отменяет стандартное действие ссылки
    
    ShowSpinner();
    $.ajax({
        "type": "GET",
        "url": $(this).attr('href'),
        "dataType": "html",
        "success": function (data) {
            HideSpinner();
            $('#dialogContent').html(data);
            $('#modDialog').modal('show');
        }
    });
})


$(".forgotPassword").on("click", function(e) {
    e.preventDefault();
    
    ShowSpinner();
    $.ajax({
        url: "/ResetPassword/ForgotPassword",
        method: 'post',

        data: $(".box").serialize(),
        dataType: 'html',
        success: function (data) {
            HideSpinner();
            $('#dialogContent').html(data);
            $('#modDialog').modal('show');
        }
    });
})

$(".submit").on("click", function(e) {
    e.preventDefault();

    ShowSpinner();
    $.ajax({
        url: "/Account/Register",
        method: 'post',

        data: $(".box").serialize(),
        dataType: 'html',
        success: function (data) {
            HideSpinner();
            
            $('#dialogContent').html(data);
            $('#modDialog').modal('show');
        }
    });
})

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