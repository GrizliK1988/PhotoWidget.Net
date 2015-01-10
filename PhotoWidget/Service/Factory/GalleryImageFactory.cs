using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using PhotoWidget.Models;

namespace PhotoWidget.Service.Factory
{
    public class GalleryImageFactory
    {
        public static GalleryImage Create(uint galleryId, HttpPostedFile postedFile)
        {
            return new GalleryImage
            {
                MimeType = postedFile.ContentType,
                Extension = Path.GetExtension(postedFile.FileName),
                Name = postedFile.FileName,
                GalleryId = galleryId
            };
        }
    }
}