define(['backbone.marionette'], function(Marionette) {
    return Marionette.Module.extend({
        initialize: function (module, app, options) {
            console.log('I am module!');
        }
    });
});
