define([
    'backbone.marionette'
    ], function (Marionette) {

    var GalleryImageModel = Backbone.Model.extend({
        defaults: {
            Source: null
        }
    });

    var GalleryImageCollection = Backbone.Collection.extend({
        model: GalleryImageModel
    });

    var GalleryImageItemView = Marionette.ItemView.extend({
        tagName: 'div',
        attributes: {
            'class': 'col-md-3'
        },
        template: '#Template_GalleryImages_GalleryImageItemView'
    });

    var GalleryImageCollectionView = Marionette.CollectionView.extend({
        el: '#El_ImagesGallery_GalleryImageCollectionView',
        itemView: GalleryImageItemView
    });

    var GalleryImageUploadItemView = Marionette.ItemView.extend({
        el: '#El_ImagesGallery_GalleryImageUploadItemView',

        ui: {
            uploadArea: '#imagesFileUploadArea',
            uploadControl: '#imagesFileUpload'
        },

        events: {
            'click @ui.uploadArea': 'openFileDialog',
            'change @ui.uploadControl': 'onFilesSelected'
        },

        initialize: function() {
            this.bindUIElements();
        },

        openFileDialog: function() {
            this.ui.uploadControl.click();
        },

        onFilesSelected: function(e) {
            this.showPreviews(e);
            this.startUpload();
        },

        showPreviews: function (e) {
            var files = this.ui.uploadControl[0].files;
            _.each(files, (function(file) {
                var fileReader = new FileReader();
                fileReader.readAsDataURL(file);
                fileReader.onload = (function(event) {
                    this.collection.add({
                        Source: event.target.result
                    });
                }).bind(this);
            }).bind(this));
        },

        startUpload: function () {
            var data = new FormData();

            var files = this.ui.uploadControl[0].files;
            _.each(files, (function (file, key) {
                data.append(key, file);
            }).bind(this));

            $.ajax({
                url: _.apiUrl('uploadImages'),
                data: data,
                type: 'POST',
                cache: false,
                processData: false,
                contentType: false,
                dataType: 'json'
            })
                .success(function() {

                })
                .error(function() {
                    alert('Error!');
                    console.log(arguments);
                });
        }
    });

    return Marionette.Module.extend({
        initialize: function () {
            this.galleryImages = new GalleryImageCollection([]);
            this.galleryImagesView = new GalleryImageCollectionView({
                collection: this.galleryImages
            });

            this.galleryImageUploadView = new GalleryImageUploadItemView({
                collection: this.galleryImages
            });

            this.getGalleryImages();
        },

        getGalleryImages: function() {
            var that = this;

            $.get(_.apiUrl('uploadImages'))
                .success(function(images) {
                    _.each(images, function(image) {
                        //that.galleryImages.add(image);
                    });
                });
        }
    });
});
