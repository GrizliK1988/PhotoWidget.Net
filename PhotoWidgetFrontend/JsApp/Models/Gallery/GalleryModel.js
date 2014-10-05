define(['backbone'], function(Backbone) {
    return Backbone.Model.extend({
        idAttribute: "Id",
        defaults: {
            Id: null,
            Name: null,
            Description: null,
            CreatedDate: null
        }
    });
});