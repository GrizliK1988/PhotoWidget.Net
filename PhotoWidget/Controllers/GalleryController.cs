using System;
using System.Collections.Generic;
using System.Web.Http;
using Ninject;
using PhotoWidget.Models;
using PhotoWidget.Service.Repository;

namespace PhotoWidget.Controllers
{
    public class GalleryController : ApiController
    {
        [Inject]
        public IGalleryRepository<Gallery, uint> GalleryRepository { get; set; }

        public IEnumerable<Gallery> Get()
        {
            return GalleryRepository.Get();
        }

        public Gallery Get(uint id)
        {
            return GalleryRepository.Get(id);
        }

        public Gallery Post(Gallery gallery)
        {
            return GalleryRepository.Save(gallery);
        }

        public void Delete(uint id)
        {
            GalleryRepository.Delete(id);
        }
    }
}
