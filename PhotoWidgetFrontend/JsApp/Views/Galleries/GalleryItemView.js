define(['backbone.marionette', 'holder'], function (Marionette, Holder) {
    return Marionette.ItemView.extend({
        template: '#Template_Galleries_GalleryItemView',
        attributes: {
            'class': 'col-md-3'
        },

        events: {
            'click [data-role="save"]': 'onSaveBtnClick',
            'click [data-role="delete"]': 'onDeleteBtnClick',
            'click [data-role="images"]': 'onImagesBtnClick'
        },

        ui: {
            name: '[data-name="Name"]',
            description: '[data-name="Description"]',
            holderJsImage: '[data-src^="holder"]'
        },

        initialize: function(options) {
            this.bindUIElements();
        },

        onSaveBtnClick: function(e) {
            e.preventDefault();
            this.save();
        },

        onDeleteBtnClick: function (e) {
            e.preventDefault();
            this.del();
        },

        onImagesBtnClick: function() {

        },

        save: function () {
            this.model.save({
                Name: this.ui.name.html(),
                Description: this.ui.description.html()
            }, { wait: true });
        },

        del: function() {
            this.model.destroy({ wait: true });
        }
    });
});