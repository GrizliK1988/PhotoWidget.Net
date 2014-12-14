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
using System.Web.UI;
using Ninject;
using PhotoWidget.Models;
using PhotoWidget.Service.Helper;
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

        [System.Web.Http.AcceptVerbs("GET")]
        [System.Web.Http.HttpGet]
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

        [System.Web.Http.AcceptVerbs("GET"), System.Web.Http.HttpGet, Route("image/{id}", Name="ImagesPublicRoute")]
        public HttpResponseMessage ImagePublic(string id)
        {
            var result = new HttpResponseMessage(HttpStatusCode.OK);

            var galleryImage = GalleryImageRepository.Get(id);

            var gallery = GalleryRepository.Get(galleryImage.GalleryId);
            var thumb = galleryImage.FindSuitableThumb(gallery.Settings.ImagesSize, 0);

            var path = HttpContext.Current.Server.MapPath(UploadsBasePath + "/" + galleryImage.Source);

            if (thumb == null)
            {
                var size = gallery.Settings.ImagesSize;
                var image = new Bitmap(path);
                var newThumb = ImageHelper.Resize(image, new Size((int)size.Width, (int)size.Height));

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
            var savedImage = GalleryImageRepository.Save(new GalleryImage
            {
                MimeType = postedFile.ContentType,
                Extension = Path.GetExtension(postedFile.FileName),
                Name = postedFile.FileName,
                GalleryId = Convert.ToUInt32(HttpContext.Current.Request["galleryId"])
            });
            var imageName = savedImage.Id + Path.GetExtension(postedFile.FileName);

            var imagePath = "UserId" + "/" + savedImage.GalleryId;
            var serverImagePath = HttpContext.Current.Server.MapPath(UploadsBasePath + imagePath);
            Directory.CreateDirectory(serverImagePath);

            postedFile.SaveAs(serverImagePath + "\\" + imageName);
            savedImage.Source = imagePath + "/" + imageName;
            GalleryImageRepository.Save(savedImage);

            var image = DrawingImage.FromFile(serverImagePath + "\\" + imageName);
            var resizedImage = ImageHelper.Resize(image, new Size(200, 300));
            ImageHelper.SaveJpeg(serverImagePath + "\\" + savedImage.Id + "_200_300" + Path.GetExtension(postedFile.FileName), new Bitmap(resizedImage), 100);

            return savedImage;
        }

        public void Delete(string id)
        {
            GalleryImageRepository.Delete(id);
        }
    }
}
