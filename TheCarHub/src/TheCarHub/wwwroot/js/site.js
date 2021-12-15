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
        $carousels.carousel();
    }
});