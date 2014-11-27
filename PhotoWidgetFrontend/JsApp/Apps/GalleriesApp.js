define([
    'bootstrap',
    'backbone.marionette',
    'modules/GalleriesModule',
    'modules/GallerySettingsModule',
    'modules/ImagesGalleryModule',
    'modules/ApiRouterModule',
    'routers/GalleriesRouter'], function (Bootstrap, Marionette, GalleriesModule, GallerySettingsModule, ImagesGalleryModule, ApiRouter, GalleriesRouter) {

    var GalleriesApp = new Marionette.Application();

    GalleriesApp.addInitializer(function () {
        this.module('Galleries', GalleriesModule);
        this.module('GallerySettingsModule', GallerySettingsModule);
        this.module('ImagesGallery', ImagesGalleryModule);
        this.module('ApiRouter', ApiRouter);
    });

    GalleriesApp.start();

    var carousel = $('.carousel');
    carousel.carousel({
        interval: 0
    });

    GalleriesApp.vent.on('gallery:list:show', function(args) {
        carousel.carousel(0);
    });

    GalleriesApp.vent.on('gallery:images_page:show', function(args) {
        carousel.carousel(1);
        GalleriesApp.module('ImagesGallery').openGallery(args.id);
    });

    GalleriesApp.vent.on('gallery:settings_page:show', function(args) {
        carousel.carousel(2);
        GalleriesApp.module('GallerySettingsModule').openGallerySettings(args.id);
    });

    new GalleriesRouter(GalleriesApp);

    Backbone.history.start({});
});