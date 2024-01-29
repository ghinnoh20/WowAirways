(function ($) {
    $('#brand').parent().append('<ul class="list-item" id="newbrand" name="brand"></ul>');
    $('#brand option').each(function () {
        $('#newbrand').append('<li value="' + $(this).val() + '">' + $(this).text() + '</li>');
    });
    $('#brand').remove();
    $('#newbrand').attr('id', 'brand');
    $('#brand li').first().addClass('init');
    $("#brand").on("click", ".init", function () {
        $(this).closest("#brand").children('li:not(.init)').toggle();
    });
    var allOptions = $("#brand").children('li:not(.init)');
    $("#brand").on("click", "li:not(.init)", function () {
        allOptions.removeClass('selected');
        $(this).addClass('selected');
        $("#brand").children('.init').html($(this).html());
        allOptions.toggle();
    });
    var marginSlider = document.getElementById('slider-margin');
    if (marginSlider != undefined) {
        noUiSlider.create(marginSlider, {
            start: [500],
            step: 10,
            connect: [true, false],
            tooltips: [true],
            range: {
                'min': 0,
                'max': 1000
            },
            format: wNumb({
                decimals: 0,
                thousand: ',',
                prefix: '$ ',
            })
        });
    }
    $('#reset').on('click', function () {
        $('#register-form').reset();
    });
    $('#register-form').validate({
        rules: {
            first_name: {
                required: true,
            },
            last_name: {
                required: true,
            },
            company: {
                required: true
            },
            email: {
                required: true,
                email: true
            },
            phone_number: {
                required: true,
            }
        },
        onfocusout: function (element) {
            $(element).valid();
        },
    });
    jQuery.extend(jQuery.validator.messages, {
        required: "",
        remote: "",
        email: "",
        url: "",
        date: "",
        dateISO: "",
        number: "",
        digits: "",
        creditcard: "",
        equalTo: ""
    });
})(jQuery);