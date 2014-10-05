define(['backbone.marionette', 'views/Galleries/GalleryCollectionView', 'models/Gallery/GalleryCollection'], function (Marionette, GalleryCollectionView, GalleryCollection) {
    return Marionette.Module.extend({
        initialize: function (module, app, options) {
            galleryCollection = new GalleryCollection();
            galleryCollection.url = _.apiUrl('getGalleries');

            var galleryCollectionView = new GalleryCollectionView({
                collection: galleryCollection
            });

            galleryCollection.fetch();
        }
    });
});
