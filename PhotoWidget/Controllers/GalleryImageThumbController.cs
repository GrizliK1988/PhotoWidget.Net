using System.Drawing;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using Ninject;
using PhotoWidget.Models;
using PhotoWidget.Service.Factory;
using PhotoWidget.Service.Image.Galllery;
using PhotoWidget.Service.Image.Storage;

namespace PhotoWidget.Controllers
{
    public class GalleryImageThumbController : ApiController
    {
        [Inject]
        public IGalleryImageService GalleryImageService { get; set; }

        [Inject, Named("FS")]
        public IGalleryImageStorage GalleryImageStorage { get; set; }

        [AcceptVerbs("GET"), HttpGet, Route("api/galleryimagethumb/{width}/{height}/{id}", Name = "ImageThumb")]
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
            var result = new HttpResponseMessage(HttpStatusCode.OK) { Content = new StreamContent(stream) };
            result.Content.Headers.ContentType = new MediaTypeHeaderValue(galleryImage.MimeType);

            return result;
        }
    }
}
