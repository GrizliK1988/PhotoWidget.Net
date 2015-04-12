using System.Net;
using System.Net.Http;
using System.Web.Http;
using Ninject;
using PhotoWidget.Models;
using PhotoWidget.Service.Image.Galllery;
using PhotoWidget.Service.Image.Storage;
using PhotoWidget.Service.Repository;

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

        public HttpResponseMessage ImagePublic(string id)
        {
            var result = new HttpResponseMessage(HttpStatusCode.OK);
            return result;
        }

    }
}
