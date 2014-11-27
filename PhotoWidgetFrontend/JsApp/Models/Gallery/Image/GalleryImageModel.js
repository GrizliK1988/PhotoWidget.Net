define(['backbone'], function(Backbone) {
    return Backbone.Model.extend({
        idAttribute: 'Id',

        defaults: {
            Source: null
        }
    });
});
