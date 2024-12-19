$(document).ready(function () {
    jQuery('#camera_wrap').camera({
        loader: false,
        pagination: false,
        thumbnails: false,
        height: '26.89655172413793%',
        caption: false,
        navigation: true,
        fx: 'mosaic'
    });
});


$(document).ready(function () {
    $().UItoTop({ easingType: 'easeOutQuart' });
});


$(window).load(
    function () {
        $('.carousel1').carouFredSel({
            auto: false, prev: '#prev_slide', next: '#next_slide', width: 960, items: {
                visible: {
                    min: 1,
                    max: 1
                },
                height: 'auto',
                width: 230,

            }, responsive: true,

            scroll: 1,

            mousewheel: false,

            swipe: { onMouse: true, onTouch: true }
        });

    });
jQuery(function () {
    $().UItoTop({ easingType: 'easeOutQuart' });
});