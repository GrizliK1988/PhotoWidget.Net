define(['bootstrap', 'backbone.marionette', 'modules/GalleriesModule'], function (Bootstrap, Marionette, GalleriesModule) {
    var GalleriesApp = new Marionette.Application();

    GalleriesApp.addInitializer(function () {
        this.module('Galleries', GalleriesModule);
    });

    GalleriesApp.start();
});