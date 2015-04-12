using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ninject;
using Ninject.Modules;
using PhotoWidget.Models;
using PhotoWidget.Service.Repository;
using PhotoWidget.Service.Storage;

namespace ServiceUnitTests.Integrational.Repository
{
    [TestClass]
    public class GalleryRepositoryUnitTest
    {
        private StandardKernel _kernel;

        [TestInitialize]
        public void Initialize()
        {
            _kernel = new StandardKernel(new DependencyInjectionModule());
        }

        [TestMethod]
        public void TestGet()
        {
            var repository = _kernel.Get<IGalleryRepository<Gallery, int>>();

            var galleries = repository.Get();
            Assert.IsTrue(galleries.Any());
        }

        [TestMethod]
        public void TestCrud()
        {
            var repository = _kernel.Get<IGalleryRepository<Gallery, int>>();

            var gallery = new Gallery() 
            {
                Name = "Test gallery",
                Description = "Test gallery description"
            };
            repository.Save(ref gallery);
            Assert.IsTrue(gallery.Id != 0);
            Assert.IsTrue(gallery.CreatedDate == gallery.UpdatedDate);

            gallery.Name = "New Test Name";
            repository.Save(ref gallery);
            Assert.IsTrue(gallery.CreatedDate < gallery.UpdatedDate);

            var foundGallery = repository.Get(gallery.Id);
            Assert.AreEqual(gallery.UpdatedDate, foundGallery.UpdatedDate);

            repository.Delete(gallery.Id);

            var foundGalleryAfterDelete = repository.Get(gallery.Id);
            Assert.IsNull(foundGalleryAfterDelete);
        }
    }

    public class DependencyInjectionModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IConnectionManager>().To<ConnectionManager>();
            Bind<IGalleryRepository<Gallery, int>>().To<GalleryRepository>();
        }
    }
}
