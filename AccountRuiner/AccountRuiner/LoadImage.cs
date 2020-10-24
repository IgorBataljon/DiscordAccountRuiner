using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace AccountRuiner
{
    class LoadImage
    {
        public Image image { get; set; }

        public override string ToString()
        {
            if (image == null)
                return null;

            string type;

            if (ImageFormat.Jpeg.Equals(image.RawFormat))
                type = "jpeg";
            else if (ImageFormat.Png.Equals(image.RawFormat))
                type = "png";
            else if (ImageFormat.Gif.Equals(image.RawFormat))
                type = "gif";
            else
                throw new NotSupportedException("File extension not supported");

            return $"data:image/{type};base64,{Convert.ToBase64String((byte[])new ImageConverter().ConvertTo(image, typeof(byte[])))}";
        }
    }
}