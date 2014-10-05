define(['backbone', 'models/Gallery/GalleryModel'], function (Backbone, GalleryModel) {
    return Backbone.Collection.extend({
        model: GalleryModel
    });
});