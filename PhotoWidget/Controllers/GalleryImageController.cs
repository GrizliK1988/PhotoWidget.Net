using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;
using Ninject;
using PhotoWidget.Models;
using PhotoWidget.Service.Factory;
using PhotoWidget.Service.Image.Galllery;
using PhotoWidget.Service.Image.Storage;
using PhotoWidget.Service.Repository;
using DrawingImage = System.Drawing.Image;

namespace PhotoWidget.Controllers
{
    public class GalleryImageController : ApiController
    {
        [Inject]
        public IGalleryImageService GalleryImageService { get; set; }

        [Inject, Named("FS")]
        public IGalleryImageStorage GalleryImageStorage { get; set; }

        public IEnumerable<GalleryImage> Get()
        {
            var galleryIdString = HttpContext.Current.Request.QueryString["gallery"];

            if (string.IsNullOrEmpty(galleryIdString))
            {
                return GalleryImageService.FindAllImages();
            }

            var galleryId = Convert.ToUInt32(galleryIdString);
            return GalleryImageService.FindImagesForGallery(galleryId);
        }

        public GalleryImage Get(string id)
        {
            return GalleryImageService.FindImage(id);
        }

        [AcceptVerbs("GET"), HttpGet]
        public HttpResponseMessage Image(string id)
        {
            var result = new HttpResponseMessage(HttpStatusCode.OK);

            var galleryImage = GalleryImageService.FindImage(id);
            var stream = GalleryImageStorage.ReadToStream(galleryImage);

            result.Content = new StreamContent(stream);
            result.Content.Headers.ContentType = new MediaTypeHeaderValue(galleryImage.MimeType);

            return result;
        }

        [AcceptVerbs("GET"), HttpGet, Route("api/galleryimage/image_thumb/{width}/{height}/{id}", Name = "ImageThumb")]
        public HttpResponseMessage ImageThumb(int width, int height, string id)
        {
            var galleryImage = GalleryImageService.FindImage(id);
            var existedThumb = galleryImage.FindSuitableThumb(ImageSizeFactory.Create(width, height));

            GalleryImageThumb thumb;
            if (existedThumb == null)
            {
                thumb = GalleryImageService.CreateThumb(galleryImage, new Size(width, height));
                GalleryImageService.SaveImageWithNewThumb(galleryImage, thumb);
            }
            else
            {
                thumb = existedThumb;
            }

            var stream = GalleryImageStorage.ReadToStream(thumb);
            var result = new HttpResponseMessage(HttpStatusCode.OK) {Content = new StreamContent(stream)};
            result.Content.Headers.ContentType = new MediaTypeHeaderValue(galleryImage.MimeType);

            return result;
        }

        public GalleryImage[] Post()
        {
            var httpRequest = HttpContext.Current.Request;
            if (httpRequest.Files.Count == 0)
            {
                return new GalleryImage[]{};
            }

            return (from string file in httpRequest.Files
                select httpRequest.Files[file]
                into postedFile
                where postedFile != null
                select UploadImage(postedFile)).ToArray();
        }

        private GalleryImage UploadImage(HttpPostedFile postedFile)
        {
            var galleryIdString = HttpContext.Current.Request["galleryId"];
            var galleryId = Convert.ToUInt32(galleryIdString);

            var newGalleryImage = GalleryImageFactory.Create(galleryId, postedFile);
            var galleryImage = GalleryImageService.SaveImage(newGalleryImage);

            var storedImage = GalleryImageStorage.Store(galleryId, galleryImage.Id, new Bitmap(postedFile.InputStream));
            galleryImage.Source = storedImage.Path;

            GalleryImageService.SaveImage(galleryImage);

            return galleryImage;
        }

        public bool Delete(string id)
        {
            return GalleryImageService.DeleteImage(id);
        }
    }
}
