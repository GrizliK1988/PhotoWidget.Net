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

        galleryId: null,

        initialize: function() {
            this.bindUIElements();
        },

        setGalleryId: function(id) {
            this.galleryId = id;
            this.collection.reset([]);
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
            data.append('galleryId', this.galleryId);

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
            var that = this;

            $.get(_.apiUrl('galleryImages', {id: this.galleryId}))
                .success(function(images) {
                    _.each(images, function(image) {
                        image.Source = _.apiUrl('image', {
                            id: image.Id
                        });
                        that.galleryImages.add(image);
                    });
                });
        }
    });
});
