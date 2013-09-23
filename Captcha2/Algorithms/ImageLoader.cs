using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Captcha1.Algorithms
{
    public class ImageLoader
    {
        /// <summary>
        /// Загрузка изображения в массив байт
        /// </summary>
        /// <param name="path">путь к файлу</param>
        /// <returns></returns>
        public static byte[,] LoadImage(string path)
        {
            Uri myUri = new Uri(path, UriKind.RelativeOrAbsolute);
            PngBitmapDecoder decoder = new PngBitmapDecoder(myUri, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
            BitmapSource bs = decoder.Frames[0];
            //Конвертируем изображение в оттенки серого
            FormatConvertedBitmap fcb = new FormatConvertedBitmap(bs, PixelFormats.Gray8, BitmapPalettes.BlackAndWhite, 1);
            bs = fcb;
            byte[] arr = new byte[(int)(bs.Width * bs.Height)];
            //Извлекаем пиксели
            bs.CopyPixels(arr, (int)(8 * bs.Width) / 8, 0);
            int count = 0;
            byte[,] img = new byte[(int)bs.Height, (int)bs.Width];
            //формируем двумерный массив
            for (int i = 0; i < bs.Height; ++i)
            {
                for (int j = 0; j < bs.Width; ++j)
                {
                    img[i, j] = arr[count++];
                }
            }
            return img;
        }

        /// <summary>
        /// Преобразование массива байт в изображение
        /// </summary>
        /// <param name="img">массив байт</param>
        /// <returns></returns>
        public static BitmapSource BitmapFromByteArr(byte[,] img)
        {
            byte[] arr = new byte[(img.GetUpperBound(1) + 1) * (img.GetUpperBound(0) + 1)];
            int count = 0;
            for (int i = 0; i <= img.GetUpperBound(0); ++i)
            {
                for (int j = 0; j <= img.GetUpperBound(1); ++j)
                {
                    arr[count++] = img[i, j];
                }
            }
            return BitmapSource.Create(img.GetUpperBound(1), img.GetUpperBound(0), 96, 96, PixelFormats.Gray8, BitmapPalettes.BlackAndWhite, arr, (int)(8 * (img.GetUpperBound(1) + 1)) / 8);
        }
    }
}
