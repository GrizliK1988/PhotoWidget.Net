using System.Configuration;
using System.Web.Configuration;
using System.Web.Http;
using PhotoWidget.Models;
using PhotoWidget.Service.Image.Galllery;
using PhotoWidget.Service.Image.Storage;
using PhotoWidget.Service.Repository;
using PhotoWidget.Service.Serializer;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(PhotoWidget.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(PhotoWidget.App_Start.NinjectWebCommon), "Stop")]

namespace PhotoWidget.App_Start
{
    using System;
    using System.Web;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;

    public static class NinjectWebCommon 
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }
        
        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }
        
        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                RegisterServices(kernel);

                GlobalConfiguration.Configuration.DependencyResolver = new NinjectDependencyResolver(kernel);

                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            var imagesBaseServerPath = ConfigurationManager.AppSettings["imagesBaseServerPath"];

            kernel.Bind<IGalleryRepository<Gallery, uint>>().To<GalleryRepository>();
            kernel.Bind<IGalleryImageRepository<GalleryImage, string>>().To<GalleryImageRepository>();
            kernel.Bind(typeof (ISerializer<>)).To(typeof (JsonSerializer<>)).Named("JsonSerializer");
            kernel.Bind<IGalleryImageStorage>().To<FileSystemGalleryImageStorage>().Named("FS").WithConstructorArgument("basePath", imagesBaseServerPath);
            kernel.Bind<IGalleryImageService>().To<GalleryImageService>();
        }
    }
}
