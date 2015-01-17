using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;
using Ninject;
using PhotoWidget.Models;
using PhotoWidget.Service.Factory;
using PhotoWidget.Service.Helper;
using PhotoWidget.Service.Image.Galllery;
using PhotoWidget.Service.Image.Storage;
using PhotoWidget.Service.Repository;
using DrawingImage = System.Drawing.Image;

namespace PhotoWidget.Controllers.Public
{
    public class ImageController : ApiController
    {
        [Inject]
        public IGalleryRepository<Gallery, uint> GalleryRepository { get; set; }

        [Inject, Named("FS")]
        public IGalleryImageStorage GalleryImageStorage { get; set; }

        [Inject]
        public IGalleryImageService GalleryImageService { get; set; }

        [AcceptVerbs("GET"), HttpGet, Route("image/{id}", Name = "ImagesPublicRoute")]
        public HttpResponseMessage ImagePublic(string id)
        {
            var galleryImage = GalleryImageService.FindImage(id);

            var gallery = GalleryRepository.Get(galleryImage.GalleryId);
            var existedThumb = galleryImage.FindSuitableThumb(gallery.Settings.ImagesSize);

            GalleryImageThumb thumb;
            if (existedThumb == null)
            {
                thumb = GalleryImageService.CreateThumb(galleryImage, gallery.Settings.ImagesSize.ToSize());
                GalleryImageService.SaveImageWithNewThumb(galleryImage, thumb);
            }
            else
            {
                thumb = existedThumb;
            }

            var stream = GalleryImageStorage.ReadToStream(thumb);
            var result = new HttpResponseMessage(HttpStatusCode.OK) { Content = new StreamContent(stream) };
            result.Content.Headers.ContentType = new MediaTypeHeaderValue(galleryImage.MimeType);

            return result;
        }

    }
}
