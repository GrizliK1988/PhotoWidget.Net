using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Ninject;
using PhotoWidget.Models;
using PhotoWidget.Service.Repository;
using PhotoWidget.Service.Serializer;
using GalleryPublic = PhotoWidget.Models.Public.Gallery;
using GalleryPrivate = PhotoWidget.Models.Gallery;

namespace PhotoWidget.Controllers
{
    public class GalleryCodeController : ApiController
    {
        private readonly IGalleryRepository<GalleryPrivate, uint> _galleryRepository;
        private readonly IGalleryImageRepository<GalleryImage, string> _galleryImageRepository;
        private readonly ISerializer<GalleryPublic> _jsonSerializer;

        public GalleryCodeController(IGalleryRepository<GalleryPrivate, uint> galleryRepository,
            IGalleryImageRepository<GalleryImage, string> galleryImageRepository,
            [Named("JsonSerializer")] ISerializer<GalleryPublic> jsonSerializer)
        {
            _galleryRepository = galleryRepository;
            _galleryImageRepository = galleryImageRepository;
            _jsonSerializer = jsonSerializer;
        }

        public HttpResponseMessage Get(uint id)
        {
            var galleryPrivate = _galleryRepository.Get(id);
            var galleryPrivateImages = _galleryImageRepository.GetForGallery(id);

            var gallery = new GalleryPublic()
            {
                Name = galleryPrivate.Name,
                Images = galleryPrivateImages.ToArray()
            };

            var galleryCode = _jsonSerializer.Serialize(gallery);
            var response = Request.CreateResponse(HttpStatusCode.OK, galleryCode);

            return response;
        }
    }
}
