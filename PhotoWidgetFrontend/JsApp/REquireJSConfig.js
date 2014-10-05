requirejs.config({
    baseUrl: '/Scripts',
    paths: {
        'apps': '/JsApp/Apps',
        'modules': '/JsApp/Modules',

        'jquery': 'jquery-1.10.2.min'
    },
    shim: {
        'backbone': {
            deps: ['underscore', 'jquery'],
            exports: 'Backbone'
        },
        'backbone.marionette': {
            deps: ['backbone'],
            exports: 'Marionette'
        },
        'underscore': {
            exports: '_'
        },
        'bootstrap': {
            deps: ['jquery']
        }
    }
});
