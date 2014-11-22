using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Web;
using System.Web.Http;
using Ninject;
using PhotoWidget.Models;
using PhotoWidget.Service.Repository;

namespace PhotoWidget.Controllers
{
    public class GalleryImageController : ApiController
    {
        const string UploadsBasePath = "~/App_Data/Resources/Uploads/";

        [Inject]
        public IGalleryImageRepository<GalleryImage, string> GalleryImageRepository { get; set; }

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

            return savedImage;
        }

        public void Delete(string id)
        {
            GalleryImageRepository.Delete(id);
        }
    }
}
