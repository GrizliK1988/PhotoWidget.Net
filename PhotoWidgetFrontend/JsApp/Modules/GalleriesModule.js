define([
    'backbone.marionette',
    'views/Galleries/GalleryCollectionView',
    'views/Galleries/GalleryAddItemView',
    'models/Gallery/GalleryCollection'
    ], function (Marionette, GalleryCollectionView, GalleryAddItemView, GalleryCollection) {
    return Marionette.Module.extend({
        initialize: function (module, app, options) {
            var galleryCollection = new GalleryCollection();
            galleryCollection.url = _.apiUrl('getGalleries');

            var galleryCollectionView = new GalleryCollectionView({
                collection: galleryCollection
            });

            var addItemView = new GalleryAddItemView({
                collection: galleryCollection
            });

            galleryCollection.fetch();
        }
    });
});
