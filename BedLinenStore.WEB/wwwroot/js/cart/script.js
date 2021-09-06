﻿function zeroPadded(val) {
    if (val >= 10)
        return val;
    else
        return '0' + val;
}

function myfunction() {
    let date = new Date();
    $("input[name='CreatedDate']").val(date.getFullYear()+"-"
        +zeroPadded(date.getMonth() + 1)+"-"
        +zeroPadded(date.getDate()));
    
    $.ajax({
        url: "Cart/Checkout",
        method: 'post',

        data: $("#form").serialize(),
        dataType: 'html',
        success: function (data) {
            $('#dialogContent').html(data);
            $('#modDialog').modal('show');
        }
    });
}


$(".box").validate({
    rules: {
        Name: {
            required: true,
            minlength: 4,
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
        Name: {
            required: "Поле должно быть заполнен",
            minlength: "Минимальная длина 4",
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
    submitHandler: function (e) {
        myfunction();
    },
});