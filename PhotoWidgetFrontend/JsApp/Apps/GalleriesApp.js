define([
    'bootstrap',
    'backbone.marionette',
    'modules/GalleriesModule',
    'modules/ImagesGalleryModule',
    'modules/ApiRouterModule'], function (Bootstrap, Marionette, GalleriesModule, ImagesGalleryModule, ApiRouter) {

    var GalleriesApp = new Marionette.Application();

    GalleriesApp.addInitializer(function () {
        this.module('Galleries', GalleriesModule);
        this.module('ImagesGallery', ImagesGalleryModule);
        this.module('ApiRouter', ApiRouter);
    });

    GalleriesApp.start();

    $('.carousel').carousel({
        interval: 0
    });
});