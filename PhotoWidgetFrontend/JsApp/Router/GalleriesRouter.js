define(['backbone'], function (Backbone) {
    return Backbone.Router.extend({
        routes: {
            "gallery/:id": "gallery"
        },

        initialize: function(app) {
            this.app  = app;
        },

        gallery: function(id) {
            this.app.vent.trigger('gallery:images_page:show', {id: id});
        }
    });
});