define(['backbone', 'models/Gallery/GallerySettingsModel'], function(Backbone, GallerySettingsModel) {
    return Backbone.Model.extend({
        idAttribute: "Id",
        defaults: {
            Id: null,
            Name: null,
            Description: null,
            Settings: null,
            CreatedDate: null,
            UpdatedDate: null
        },

        initialize: function(args) {
            if (_.isUndefined(args))
                return ;

            this.set('Settings', new GallerySettingsModel(args.Settings || {}));
        },

        parse: function(resp, options) {
            var data = resp;
            data.Settings = new GallerySettingsModel(resp.Settings || {});
            return data;
        }
    });
});