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
            this.displayGalleryCode();
        },

        getGalleryImages: function() {
            this.galleryImages.url = _.apiUrl('galleryImages', {id: this.galleryId});
            this.galleryImages.fetch();
        },

        displayGalleryCode: function() {
            var url = _.apiUrl('galleryCode', {id: this.galleryId});
            $('#gallery-code').val('<div id="photo-widget-slider"></div>' + "\n"
                                    + '<script src="' + url + '"></script>');
        }
    });
});
