define(['backbone'], function (Backbone) {
    return Backbone.Router.extend({
        routes: {
            "": "galleryList",
            "gallery/:id": "gallery"
        },

        initialize: function(app) {
            this.app  = app;
        },

        galleryList: function() {
            this.app.vent.trigger('gallery:list:show');
        },

        gallery: function(id) {
            this.app.vent.trigger('gallery:images_page:show', {id: id});
        }
    });
});