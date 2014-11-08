requirejs.config({
    baseUrl: '/Scripts',
    paths: {
        'apps': '/JsApp/Apps',
        'modules': '/JsApp/Modules',
        'views': '/JsApp/Views',
        'models': '/JsApp/Models',
        'routers': '/JsApp/Router',

        'jquery': 'jquery-1.10.2.min',
        'moment': 'moment-with-locales'
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
            deps: ['jquery', 'holder']
        }
    }
});
