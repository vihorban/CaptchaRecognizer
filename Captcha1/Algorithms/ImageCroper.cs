using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Captcha1.Algorithms
{
    public class ImageCroper
    {
        public static BitmapSource[] CropByPixels(byte[,] imageLoaded)
        {
            BitmapSource[] result = new BitmapSource[6];
            int width = 12;
            int height = 24;
            byte[,] imageCropped = new byte[height, width];
            for (int k = 0; k < 6; k++)
            {
                for (int i = 0; i < height; i++)
                    for (int j = 0; j < width; j++)
                        imageCropped[i, j] = imageLoaded[10 + i, 14 + 16 * k + j];
                result[k] = ImageLoader.BitmapFromByteArr(imageCropped);
            }
            return result;
        }

        public static byte[,] CropToSpecificImage(byte[,] imageLoaded, int number)
        {
            int width = 12;
            int height = 24;
            byte[,] result = new byte[height,width];
            for (int i = 0; i < height; i++)
                for (int j = 0; j < width; j++)
                    result[i, j] = imageLoaded[10 + i, 14 + 16 * number + j];
            return result;
        }
    }
}
