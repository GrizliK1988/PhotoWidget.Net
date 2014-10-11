define(['backbone.marionette', 'models/Gallery/GalleryModel'], function (Marionette, GalleryModel) {
    return Marionette.ItemView.extend({
        el: '#El_Galleries_GalleryAddItemView',

        ui: {
            add: '[data-role="add"]'
        },

        events: {
            'click @ui.add': 'add'
        },

        initialize: function(options) {
            this.bindUIElements();
        },

        add: function () {
            var newGallery = new GalleryModel({});
            this.collection.add(newGallery);
            this.$el.hide();

            newGallery.on('change', (function() {
                this.$el.show();
            }).bind(this));
        }
    });
});