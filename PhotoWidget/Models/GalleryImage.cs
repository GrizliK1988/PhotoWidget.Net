using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace PhotoWidget.Models
{
    public class GalleryImage
    {
        public string Id { get; set; }

        public uint GalleryId { get; set; }

        public string Name { get; set; }

        public string MimeType { get; set; }

        public string Extension { get; set; }

        public string Source { get; set; }

        public Size Size { get; set; }

        public DateTime CreatedDate { get; set; }

        public GalleryImageThumb[] Thumbs { get; set; }

        public GalleryImageThumb FindSuitableThumb(Size thumbSize)
        {
            return
                Thumbs.Where(s => SizesDiffMultiplication(s.Size, thumbSize) > 0)
                    .OrderBy(t => SizesDiffMultiplication(t.Size, thumbSize))
                    .FirstOrDefault();
        }

        private static float SizesDiffMultiplication(Size size1, Size size2)
        {
            return (size1 - size2).Height * (size1 - size2).Width;
        }
    }

    public class GalleryImageThumb
    {
        public Size Size { get; set; }

        public string Source { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}