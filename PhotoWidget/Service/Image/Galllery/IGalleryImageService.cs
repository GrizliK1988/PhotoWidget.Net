using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhotoWidget.Models;

namespace PhotoWidget.Service.Image.Galllery
{
    public interface IGalleryImageService
    {
        GalleryImageThumb CreateThumb(GalleryImage image, Size thumbSize);

        GalleryImage SaveImageWithNewThumb(GalleryImage image, GalleryImageThumb thumb);

        GalleryImage SaveImage(GalleryImage image);

        GalleryImage[] FindAllImages();

        GalleryImage[] FindImagesForGallery(uint galleryId);

        GalleryImage FindImage(string imageId);

        bool DeleteImage(string id);
    }
}
