define([
    'backbone.marionette',
    'models/Gallery/Image/GalleryImageCollection',
    'views/GalleryImages/GalleryImageCollectionView',
    'views/GalleryImages/GalleryImageUploadItemView'
    ], function (Marionette, GalleryImageCollection, GalleryImageCollectionView, GalleryImageUploadItemView) {

    return Marionette.Module.extend({
        galleryId: null,

        initialize: function () {
            this.galleryImages = new GalleryImageCollection([]);
            new GalleryImageCollectionView({
                collection: this.galleryImages
            });

            this.galleryImageUploadView = new GalleryImageUploadItemView({
                collection: this.galleryImages
            });
        },

        openGallery: function(id) {
            this.galleryId = id;
            this.galleryImageUploadView.setGalleryId(id);
            this.getGalleryImages();
        },

        getGalleryImages: function() {
            this.galleryImages.url = _.apiUrl('galleryImages', {id: this.galleryId});
            this.galleryImages.fetch();
        }
    });
});
