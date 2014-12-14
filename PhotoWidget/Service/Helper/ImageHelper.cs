using System;
using System.Drawing;
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

        public static Image Resize(Image imageToResize, Size newSize)
        {
            var sourceWidth = imageToResize.Width;
            var sourceHeight = imageToResize.Height;

            var relativeWidth = (float)newSize.Width / sourceWidth;
            var relativeHeight = (float)newSize.Height / sourceHeight;
            var ratio = relativeWidth < relativeHeight ? relativeWidth : relativeHeight;

            var destinationWidth = (int)(sourceWidth * ratio);
            if (Math.Abs(destinationWidth - newSize.Width) < 2)
            {
                destinationWidth = newSize.Width;
            }

            var destinationHeight = (int)(sourceHeight * ratio);
            if (Math.Abs(destinationHeight - newSize.Height) < 2)
            {
                destinationHeight = newSize.Height;
            }

            var b = new Bitmap(destinationWidth, destinationHeight);
            var g = Graphics.FromImage(b);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;

            g.DrawImage(imageToResize, 0, 0, destinationWidth, destinationHeight);
            g.Dispose();

            return b;
        }
    }
}