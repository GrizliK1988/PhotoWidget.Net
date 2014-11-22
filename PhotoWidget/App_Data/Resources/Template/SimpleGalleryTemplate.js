$(function () {
    $('head')
            .append($('<link>').attr({ rel: 'stylesheet', type: 'text/css', href: 'flexslider.css' }))
            .append($('<link>').attr({ rel: 'stylesheet', type: 'text/css', href: 'custom.css' }));

    var data = '[+ImagesData+]';
    var galleryData = JSON.parse(data);
    galleryData.Images = galleryData.Images || [];

    $('#photo-widget-slider').hide();

    $('#photo-widget-slider').css('position', 'relative').append('<ul class="slides"></ul>');
    $.each(galleryData.Images, function (key, image) {
        $('#photo-widget-slider ul').append(
                $('<li></li>').append(
                        $('<a></a>').attr('href', image.SourceUrl).append(
                                $('<img>').attr('src', image.SourceUrl).css({ width: '200px' })
                        )
                )
        );
    });

    $.getScript("http://project.dimpost.com/flexslider-basic/js/jquery.flexslider-min.js", function () {
        $('#photo-widget-slider').show().flexslider({
            animation: "slide",
            controlNav: false,
            directionNav: true,
            easing: "swing",
            slideshowSpeed: 3000,
            animationSpeed: 600
        });
    });
});
