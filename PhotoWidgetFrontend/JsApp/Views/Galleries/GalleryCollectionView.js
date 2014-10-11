define(['views/Galleries/GalleryItemView'], function (GalleryItemView) {
    return Marionette.CollectionView.extend({
        el: '#El_Galleries_GalleryCollectionView',
        itemView: GalleryItemView
    });
});