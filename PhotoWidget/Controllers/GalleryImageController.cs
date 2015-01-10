using System;
using System.Collections.Generic;
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
using PhotoWidget.Service.Image.Storage;
using PhotoWidget.Service.Repository;
using DrawingImage = System.Drawing.Image;

namespace PhotoWidget.Controllers
{
    public class GalleryImageController : ApiController
    {
        const string UploadsBasePath = "~/App_Data/Resources/Uploads/";

        [Inject]
        public IGalleryImageRepository<GalleryImage, string> GalleryImageRepository { get; set; }

        [Inject]
        public IGalleryRepository<Gallery, uint> GalleryRepository { get; set; }

        [Inject, Named("FS")]
        public IGalleryImageStorage GalleryImageStorage { get; set; }

        public IEnumerable<GalleryImage> Get()
        {
            if (!string.IsNullOrEmpty(HttpContext.Current.Request.QueryString["gallery"]))
            {
                return GalleryImageRepository.GetForGallery(Convert.ToUInt32(HttpContext.Current.Request.QueryString["gallery"]));
            }

            return GalleryImageRepository.Get();
        }

        public GalleryImage Get(string id)
        {
            var galleryImage = GalleryImageRepository.Get(id);
            return galleryImage;
        }

        [AcceptVerbs("GET"), HttpGet]
        public HttpResponseMessage Image(string id)
        {
            var result = new HttpResponseMessage(HttpStatusCode.OK);

            var galleryImage = GalleryImageRepository.Get(id);

            var path = HttpContext.Current.Server.MapPath(UploadsBasePath + "/" + galleryImage.Source);
            var stream = File.OpenRead(path);

            result.Content = new StreamContent(stream);
            result.Content.Headers.ContentType = new MediaTypeHeaderValue(galleryImage.MimeType);

            return result;
        }

        [AcceptVerbs("GET"), HttpGet, Route("image/{id}", Name="ImagesPublicRoute")]
        public HttpResponseMessage ImagePublic(string id)
        {
            var result = new HttpResponseMessage(HttpStatusCode.OK);

            var galleryImage = GalleryImageRepository.Get(id);

            var gallery = GalleryRepository.Get(galleryImage.GalleryId);
            var thumb = galleryImage.FindSuitableThumb(gallery.Settings.ImagesSize);

            var path = HttpContext.Current.Server.MapPath(UploadsBasePath + "/" + galleryImage.Source);

            if (thumb == null)
            {
                var image = new Bitmap(path);
                var newThumb = ImageHelper.Resize(image, gallery.Settings.ImagesSize.ToSize());

                var thumbPath = HttpContext.Current.Server.MapPath(UploadsBasePath + "/" + galleryImage.Source.Replace(".jpg", "_thump.jpg"));
                ImageHelper.SaveJpeg(thumbPath, new Bitmap(newThumb), 100);

                var thumbs = galleryImage.Thumbs.ToList();
                thumbs.Add(new GalleryImageThumb()
                {
                    CreatedDate = DateTime.Now,
                    Size = new ImageSize()
                    {
                        Width = newThumb.Width,
                        Height = newThumb.Height
                    },
                    Source = galleryImage.Source.Replace(".jpg", "_thump.jpg")
                });
                galleryImage.Thumbs = thumbs.ToArray();
                GalleryImageRepository.Save(galleryImage);

                var imageStream = new MemoryStream();
                newThumb.Save(imageStream, ImageFormat.Jpeg);
                imageStream.Position = 0;

                result.Content = new StreamContent(imageStream);
                result.Content.Headers.ContentType = new MediaTypeHeaderValue(galleryImage.MimeType);

                return result;
            }
            else
            {
                var stream = File.OpenRead(path);

                result.Content = new StreamContent(stream);
                result.Content.Headers.ContentType = new MediaTypeHeaderValue(galleryImage.MimeType);

                return result;
            }

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
            var galleryId = HttpContext.Current.Request["galleryId"];
            var galleryIdUint = Convert.ToUInt32(galleryId);

            var newGalleryImage = GalleryImageFactory.Create(galleryIdUint, postedFile);
            var savedImage = GalleryImageRepository.Save(newGalleryImage);

            var storedImage = GalleryImageStorage.Store(galleryIdUint, savedImage.Id, new Bitmap(postedFile.InputStream));
            savedImage.Source = storedImage.Path;

            GalleryImageRepository.Save(savedImage);

            return savedImage;
        }

        public void Delete(string id)
        {
            GalleryImageRepository.Delete(id);
        }
    }
}
