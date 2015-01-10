using PhotoWidget.Models;
using DrawingImage = System.Drawing.Image;

namespace PhotoWidget.Service.Image.Storage
{
    public interface IGalleryImageStorage
    {
        StoredGalleryImage Store(uint galleryId, string imageId, string imagePathOnServer);

        StoredGalleryImage Store(uint galleryId, string imageId, DrawingImage image);

        StoredGalleryImage StoreThumb(uint galleryId, string imageId, DrawingImage image);

        DrawingImage Read(StoredGalleryImage galleryImage);
    }
}
