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
        $("#brand").removeAttr("style");
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


    function httpPost() {

        var jsonData = {
            firstName: $("#first_name").val(),
            lastName: $("#last_name").val(),
            mobileNo: $("#mobile_number").val(),
            email: $("#email").val(),
            brand: $('ul#brand > li.selected').text(),
            branch: $("#branch").val(),
            position: $("#position").val(),
        };

        //axios.post('https://7u7btuuitcqwgjfh2rnnisf4we0kvrtb.lambda-url.ap-southeast-1.on.aws/attendees',
        //    jsonData)
        //    .then(function (response) {
        //        console.log(response);
        //    })
        //    .catch(function (error) {
        //        console.log(error);
        //    });
    }

    $('#reset').on('click', function () {
        $('#register-form').reset();
    });

    $('#submit').on('click', function () {
        if ($('ul#brand > li.selected').text() == '') {
            $("#brand").css({ 'border-color': 'red' });
        }        
    });

    $('#register-form').validate({
        rules: {
            first_name: {
                required: true,
            },
            last_name: {
                required: true,
            },
            brand: {
                required: true
            },
            email: {
                required: true,
                email: true
            },
            mobile_number: {
                required: true,
            },
            position: {
                required: true,
            },
            branch: {
                required: true,
            }
        },
        submitHandler: function (form) {
            httpPost();
            return false;
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