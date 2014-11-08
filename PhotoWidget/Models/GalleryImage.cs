using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PhotoWidget.Models
{
    public class GalleryImage
    {
        [System.ComponentModel.DefaultValue(null)]
        public string Id { get; set; }

        public uint GalleryId { get; set; }

        public string Name { get; set; }

        public string MimeType { get; set; }

        public string Extension { get; set; }

        public string Source { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}