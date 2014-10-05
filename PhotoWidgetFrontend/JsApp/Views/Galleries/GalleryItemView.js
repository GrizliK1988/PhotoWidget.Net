define(['backbone.marionette', 'holder'], function (Marionette, Holder) {
    return Marionette.ItemView.extend({
        template: '#Template_Galleries_GalleryItemView',
        attributes: {
            'class': 'col-md-3'
        },

        events: {
            'click [data-role="save"]': 'onSaveBtnClick',
            'click [data-role="delete"]': 'onDeleteBtnClick'
        },

        ui: {
            name: '[data-name="Name"]',
            description: '[data-name="Description"]'
        },

        initialize: function() {
            this.bindUIElements();
        },

        onRender: function () {
            _.delay((function() {
                Holder.run({ images: "#gallery-image-" + this.model.id });
            }).bind(this), 0);
        },

        onSaveBtnClick: function(e) {
            e.preventDefault();
            this.save();
        },

        onDeleteBtnClick: function (e) {
            e.preventDefault();
            this.del();
        },

        save: function () {
            this.model.save({
                Name: this.ui.name.html(),
                Description: this.ui.description.html()
            });
        },

        del: function() {
            this.model.destroy({ wait: true });
        }
    });
});