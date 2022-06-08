using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Text;

namespace Rn.Core.Helpers
{
    public static class ImageHelper
    {
        public static Image ResizeImage(this Image img, int maxWidth, int maxHeight)
        {
            var width = img.Width;
            var height = img.Height;

            if (width > maxWidth || height > maxHeight)
            {
                float ratio;
                if (width > height)
                {
                    ratio = width / (float)height;
                    width = maxWidth;
                    height = Convert.ToInt32(Math.Round(width / ratio));
                }
                else
                {
                    ratio = height / (float)width;
                    height = maxHeight;
                    width = Convert.ToInt32(Math.Round(height / ratio));
                }

                return img.GetThumbnailImage(width, height, null, IntPtr.Zero);
            }
            
            return img;
        }
    }
}
