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
                case 'galleryImages':
                    return baseUrl + 'galleryimage?gallery=' + params.id;
                case 'deleteImage':
                    return baseUrl + 'galleryimage/delete';
                case 'image':
                    return baseUrl + 'galleryimage/image/' + params.id;
                case 'galleryCode':
                    return baseUrl + 'gallerycode/' + params.id;
                default:
                    return action;
            }
        }
    });
});