define(['underscore'], function (_) {
    var baseUrl = 'http://localhost:2368/api/';

    _.mixin({
        apiUrl: function(action, params) {
            switch (action) {
                case 'getGalleries':
                    return baseUrl + 'gallery';
                case 'getGallery':
                    return baseUrl + 'gallery/' + params.id;
                case 'uploadImages':
                    return baseUrl + 'galleryimage';
                default:
                    return action;
            }
        }
    });
});