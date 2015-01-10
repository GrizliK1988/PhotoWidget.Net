using System;
using System.Diagnostics;
using System.Drawing;
using DrawingImage = System.Drawing.Image;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;

namespace PhotoWidget.Service.Helper
{
    public class ImageHelper
    {
        public static void SaveJpeg(string path, Bitmap image, long quality)
        {
            var qualityParam = new EncoderParameter(Encoder.Quality, quality);
            var jpegCodecInfo = GetImageCodecInfo("image/jpeg");

            if (jpegCodecInfo == null)
                return;

            var encoderParameters = new EncoderParameters(1);
            encoderParameters.Param[0] = qualityParam;

            image.Save(path, jpegCodecInfo, encoderParameters);
        }

        private static ImageCodecInfo GetImageCodecInfo(string mimeType)
        {
            var codecs = ImageCodecInfo.GetImageEncoders();
            return codecs.FirstOrDefault(c => c.MimeType == mimeType);
        }

        public static DrawingImage Resize(DrawingImage imageToResize, Size newSize)
        {
            var thumbSize = CalculateThumbSize(imageToResize, newSize);

            var b = new Bitmap(thumbSize.Width, thumbSize.Height);
            var g = Graphics.FromImage(b);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;

            g.DrawImage(imageToResize, 0, 0, thumbSize.Width, thumbSize.Height);
            g.Dispose();

            return b;
        }

        private static Size CalculateThumbSize(DrawingImage imageToResize, Size newSize)
        {
            var sourceWidth = imageToResize.Width;
            var sourceHeight = imageToResize.Height;

            var relativeWidth = (float)newSize.Width / sourceWidth;
            var relativeHeight = (float)newSize.Height / sourceHeight;
            var ratio = relativeWidth < relativeHeight ? relativeWidth : relativeHeight;

            var destinationWidth = CalculateDestinationDimension(sourceWidth, ratio, newSize.Width);
            var destinationHeight = CalculateDestinationDimension(sourceHeight, ratio, newSize.Height);

            return new Size(destinationWidth, destinationHeight);
        }

        private static int CalculateDestinationDimension(float sourceDimension, float ratio, int desiredDimension)
        {
            var dimension = (int)(sourceDimension * ratio);
            return Math.Abs(dimension - desiredDimension) < 2 ? desiredDimension : dimension;
        }

        public static string GetImageExtension(DrawingImage image)
        {
            var encoder = ImageCodecInfo.GetImageEncoders().FirstOrDefault(x => x.FormatID == image.RawFormat.Guid);
            if (encoder == null)
            {
                return null;
            }

            var ext = encoder.FilenameExtension.Split(';').FirstOrDefault();
            if (ext == null)
            {
                return null;
            }

            return ext.ToLower().Replace("*.", ".");
        }
    }
}