using System.Collections.Generic;
using PhotoWidget.Models;

namespace PhotoWidget.Service.Repository
{
    public interface IGalleryImageRepository<T, in TId> : IRepository<T, TId>
    {
        IEnumerable<GalleryImage> GetForGallery(uint galleryId);
    }
}
