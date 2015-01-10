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

        public ImageSize Size { get; set; }

        public DateTime CreatedDate { get; set; }

        private GalleryImageThumb[] _thumbs;

        public GalleryImageThumb[] Thumbs
        {
            get { return _thumbs ?? new GalleryImageThumb[] {}; }
            set { _thumbs = value; }
        }

        public GalleryImageThumb FindSuitableThumb(ImageSize thumbSize)
        {
            return Thumbs
                .OrderBy(t => SizesDiffMultiplication(t.Size, thumbSize))
                .FirstOrDefault();
        }

        private static float SizesDiffMultiplication(ImageSize size1, ImageSize size2)
        {
            return Math.Abs((size1 - size2).Height * (size1 - size2).Width);
        }
    }

    public class GalleryImageThumb
    {
        public ImageSize Size { get; set; }

        public string Source { get; set; }

        public DateTime CreatedDate { get; set; }
    }

    public class ImageSize
    {
        public float Width { get; set; }
        public float Height { get; set; }

        public static ImageSize operator +(ImageSize size1, ImageSize size2)
        {
            var size = new ImageSize()
            {
                Height = size1.Height + size2.Height,
                Width = size1.Width + size2.Width
            };
            return size;
        }

        public static ImageSize operator -(ImageSize size1, ImageSize size2)
        {
            var size = new ImageSize()
            {
                Height = size1.Height - size2.Height,
                Width = size1.Width - size2.Width
            };
            return size;
        }

        public Size ToSize()
        {
            return new Size((int)Width, (int)Height);
        }
    }
}