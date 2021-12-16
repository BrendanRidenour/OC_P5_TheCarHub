$(document).ready(function () {
    var $dates = $('.datepicker');

    if ($dates.length > 0) {
        $dates.datepicker({
            format: 'yyyy/mm/dd',
            clearBtn: true,
            autoclose: true,
        });
    }

    var $carousels = $('.carousel');

    if ($carousels) {
        $carousels.each(function () {
            var $this = $(this);
            var timeout = Math.floor(Math.random() * 15000);

            setTimeout(function () {
                $this.carousel({
                    interval: 7500,
                    ride: "carousel",
                    keyboard: false,
                    wrap: true,
                });
            }, timeout);
        });
    }
});