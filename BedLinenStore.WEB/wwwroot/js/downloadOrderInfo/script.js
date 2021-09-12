$(document).ready(function () {

    function zeroPadded(val) {
        if (val >= 10)
            return val;
        else
            return '0' + val;
    }


    // $(".box").validate({
    //     rules: {
    //         Date:{
    //             required: true,
    //             date: true
    //         },
    //     },
    //     messages: {
    //         Date:{
    //             required: "Поле должно быть заполнено",
    //             date: "Введите правильную дату"
    //         }
    //     },
    // });

    $('#inputEmail, #inputPassword').inputmask('99/99/9999');

    $('#inputPassword, #inputEmail').datepicker({
        altFormat: "dd/mm/yy",
        dateFormat: "dd/mm/yy",
    });
    
})