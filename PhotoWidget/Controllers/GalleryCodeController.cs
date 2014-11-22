using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.Http;
using Ninject;
using PhotoWidget.Models;
using PhotoWidget.Service.Repository;
using PhotoWidget.Service.Serializer;
using GalleryPublic = PhotoWidget.Models.Public.Gallery;
using GalleryPrivate = PhotoWidget.Models.Gallery;
using GalleryImagePublic = PhotoWidget.Models.Public.GalleryImage;
using GalleryImagePrivate = PhotoWidget.Models.GalleryImage;

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
            var templatePath = HttpContext.Current.Server.MapPath("~/App_Data/Resources/Template/SimpleGalleryTemplate.js");
            var stream = File.OpenRead(templatePath);
            var reader = new StreamReader(stream);
            var templateContents = reader.ReadToEnd();

            var galleryPrivate = _galleryRepository.Get(id);
            var galleryPrivateImages = _galleryImageRepository.GetForGallery(id);
            var galleryImages = galleryPrivateImages.Select(s => new GalleryImagePublic()
            {
                SourceUrl =
                    Url.Link("ControllerActionApi", new {Controller = "GalleryImage", Action = "Image", id = s.Id})
            });

            var gallery = new GalleryPublic()
            {
                Name = galleryPrivate.Name,
                Images = galleryImages.ToArray()
            };

            var galleryCode = _jsonSerializer.Serialize(gallery);
            var script = templateContents.Replace("[+ImagesData+]", galleryCode);

            var response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(script, Encoding.UTF8, "text/javascript");

            return response;
        }
    }
}
