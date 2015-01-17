using PhotoWidget.Models;

namespace PhotoWidget.Service.Factory
{
    public class ImageSizeFactory
    {
        public static ImageSize Create(System.Drawing.Image image)
        {
            return new ImageSize() { Width = image.Width, Height = image.Height };
        }

        public static ImageSize Create(int width, int height)
        {
            return new ImageSize() { Width = width, Height = height };
        }
    }
}