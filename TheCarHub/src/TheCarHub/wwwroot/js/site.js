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
        var timeout = 0;
        $carousels.each(function () {
            var self = this;
            var selfTimeout = timeout;

            setTimeout(function () {
                $(self).carousel({
                    interval: 7500,
                    ride: "carousel",
                    keyboard: false,
                    wrap: true,
                });
            }, selfTimeout);

            timeout += 2500;
        });

        $(".carousel-control-prev").click(function (e) {
            e.preventDefault();

            var $self = $(this);

            var carouselId = $self.attr('href');

            $(carouselId).carousel('prev');
        });

        $(".carousel-control-next").click(function (e) {
            e.preventDefault();

            var $self = $(this);

            var carouselId = $self.attr('href');

            $(carouselId).carousel('next');
        });
    }

    $("form.delete-picture").submit(function (e) {
        var self = this;

        e.preventDefault();

        if (window.confirm("Are you sure you want to delete this photo?")) {
            self.submit();
        }
    });
});