using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using PhotoWidget.Models;
using PhotoWidget.Models.Service;
using PhotoWidget.Service.Helper;

namespace PhotoWidget.Service.Image.Storage
{
    public class FileSystemGalleryImageStorage : IGalleryImageStorage
    {
        private const string UserId = "UserId";
        private readonly string _basePath;

        public FileSystemGalleryImageStorage(string basePath)
        {
            _basePath = basePath;
        }

        public StoredGalleryImage Store(uint galleryId, string imageId, string imagePathOnServer)
        {
            var image = new Bitmap(imagePathOnServer);
            return Store(galleryId, imageId, image);
        }

        public StoredGalleryImage Store(uint galleryId, string imageId, System.Drawing.Image image)
        {
            var ext = ImageHelper.GetImageFormatExtension(image.RawFormat);
            var imageFileName = MakeImageFileName(imageId, ext);

            var imagePath = GenerateGalleryDirectoryPath(galleryId);
            CreateDirectoryIfMissed(imagePath);
            imagePath.GoToFile(imageFileName);

            var path = imagePath.Absolute();
            image.Save(path);

            return new StoredGalleryImage(imagePath.Web(), StorageType.FileSystem);
        }

        public StoredGalleryImage StoreThumb(uint galleryId, string imageId, System.Drawing.Image image, ImageFormat imageFormat)
        {
            var ext = ImageHelper.GetImageFormatExtension(imageFormat);
            var imageFileName = MakeImageThumbFileName(imageId, ext, image.Size);

            var imagePath = GenerateGalleryDirectoryPath(galleryId);
            imagePath.GoToFile(imageFileName);

            image.Save(imagePath.Absolute());

            return new StoredGalleryImage(imagePath.Web(), StorageType.FileSystem);
        }

        public System.Drawing.Image Read(GalleryImage galleryImage)
        {
            var imageFileSystemPath = new FileSystemPath(_basePath, galleryImage.Source);
            return new Bitmap(imageFileSystemPath.Absolute());
        }

        public Stream ReadToStream(GalleryImage galleryImage)
        {
            var imageFileSystemPath = new FileSystemPath(_basePath, galleryImage.Source);
            var stream = File.OpenRead(imageFileSystemPath.Absolute());
            return stream;
        }

        public Stream ReadToStream(GalleryImageThumb thumb)
        {
            var imageFileSystemPath = new FileSystemPath(_basePath, thumb.Source);
            var stream = File.OpenRead(imageFileSystemPath.Absolute());
            return stream;
        }

        private FileSystemPath GenerateGalleryDirectoryPath(uint galleryId)
        {
            var imagePath = UserId + Path.DirectorySeparatorChar + galleryId;
            return new FileSystemPath(_basePath, imagePath);
        }

        private static string MakeImageFileName(string imageId, string ext)
        {
            return imageId + ext;
        }

        private static string MakeImageThumbFileName(string imageId, string ext, Size imageSize)
        {
            return imageId + "_" + imageSize.Width + "_" + imageSize.Height + ext;
        }

        private static void CreateDirectoryIfMissed(FileSystemPath path)
        {
            var absolutePath = path.Absolute();
            if (!Directory.Exists(absolutePath))
            {
                Directory.CreateDirectory(absolutePath);
            }
        }
    }
}