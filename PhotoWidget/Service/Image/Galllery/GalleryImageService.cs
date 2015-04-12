using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using Ninject;
using PhotoWidget.Models;
using PhotoWidget.Service.Factory;
using PhotoWidget.Service.Helper;
using PhotoWidget.Service.Image.Storage;
using PhotoWidget.Service.Repository;

namespace PhotoWidget.Service.Image.Galllery
{
    public class GalleryImageService : IGalleryImageService
    {
        [Inject, Named("FS")]
        public IGalleryImageStorage GalleryImageStorage { get; set; }

        [Inject]
        public IGalleryImageRepository<GalleryImage, string> GalleryImageRepository { get; set; }

        public GalleryImageThumb CreateThumb(GalleryImage galleryImage, Size thumbSize)
        {
            var image = GalleryImageStorage.Read(galleryImage);
            var resizedImage = ImageHelper.Resize(image, thumbSize);
            var storedThumb = GalleryImageStorage.StoreThumb(galleryImage.GalleryId, galleryImage.Id, resizedImage, image.RawFormat);
            var newThumb = new GalleryImageThumb(storedThumb.Path, ImageSizeFactory.Create(resizedImage));
            return newThumb;
        }

        public GalleryImage SaveImageWithNewThumb(GalleryImage image, GalleryImageThumb thumb)
        {
            image.AddThumb(thumb);
            GalleryImageRepository.Save(ref image);
            return image;
        }

        public GalleryImage SaveImage(GalleryImage image)
        {
            GalleryImageRepository.Save(ref image);
            return image;
        }

        public GalleryImage[] FindAllImages()
        {
            return GalleryImageRepository.Get().ToArray();
        }

        public GalleryImage[] FindImagesForGallery(uint galleryId)
        {
            return GalleryImageRepository.GetForGallery(galleryId).ToArray();
        }

        public GalleryImage FindImage(string imageId)
        {
            return GalleryImageRepository.Get(imageId);
        }

        public bool DeleteImage(string id)
        {
            GalleryImageRepository.Delete(id);
            return true;
        }
    }
}