define(['bootstrap', 'backbone.marionette', 'modules/GalleriesModule', 'modules/ApiRouterModule'], function(Bootstrap, Marionette, GalleriesModule, ApiRouter) {
    var GalleriesApp = new Marionette.Application();

    GalleriesApp.addInitializer(function() {
        this.module('Galleries', GalleriesModule);
        this.module('ApiRouter', ApiRouter);
    });

    GalleriesApp.start();
});