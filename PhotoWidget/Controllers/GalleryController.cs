using System;
using System.Collections.Generic;
using System.Web.Http;
using PhotoWidget.Models;
using PhotoWidget.Service.Repository;

namespace PhotoWidget.Controllers
{
    public class GalleryController : ApiController
    {
        public IGalleryRepository<Gallery, uint> galleryRepository  = new GalleryRepository();

        public IEnumerable<Gallery> Get()
        {
            return galleryRepository.Get();
        }

        public Gallery Get(uint id)
        {
            return galleryRepository.Get(id);
        }

        public Gallery Post(Gallery gallery)
        {
            gallery.CreatedDate = DateTime.Now;
            return galleryRepository.Save(gallery);
        }

        public void Delete(uint id)
        {
            galleryRepository.Delete(id);
        }
    }
}
