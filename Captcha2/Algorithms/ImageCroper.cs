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
            BitmapSource[] result = new BitmapSource[5];
            int width = 15;
            int height = 20;
            byte[,] imageCropped = new byte[height, width];
            for (int k = 0; k < 5; k++)
            {
                for (int i = 0; i < height; i++)
                    for (int j = 0; j < width; j++)
                        imageCropped[i, j] = imageLoaded[i, 40 + 15 * k + j];
                result[k] = ImageLoader.BitmapFromByteArr(imageCropped);
            }
            return result;
        }

        public static byte[,] CropToSpecificImage(byte[,] imageLoaded, int number)
        {
            int width = 12;
            int height = 20;
            byte[,] result = new byte[height,width];
            for (int i = 0; i < height; i++)
                for (int j = 0; j < width; j++)
                    result[i, j] = imageLoaded[i, 14 + 16 * number + j];
            return result;
        }

        public static BitmapSource[] CropByContent(byte[,] imageLoaded)
        {
            int sizeOfArray=7;
            BitmapSource[] result = new BitmapSource[sizeOfArray];
            int[] width = new int[sizeOfArray];
            int height = imageLoaded.GetUpperBound(0)+1;
            int imageLoadedWidth=imageLoaded.GetUpperBound(1)+1;
            int minwidth = 5;
            int sensity=50;
            int currentWidth=0;
            int startIndex=0;
            int indexOfImage = 0;
            for (int i = 0; i < imageLoadedWidth; i++)
            {
                bool isRowWhite = true;
                for (int j = 0; j < height&&isRowWhite; j++)
                {
                    if (imageLoaded[j, i] < sensity)
                        isRowWhite = false;
                }
                if (!isRowWhite)
                {
                    if (currentWidth == 0)
                        startIndex = i;
                    currentWidth++;
                }
                if (currentWidth > 0 && isRowWhite == true)
                {
                    if (currentWidth < minwidth)
                    {
                        currentWidth = 0;
                    }
                    else
                    {
                        currentWidth++;
                        width[indexOfImage] = currentWidth;
                        byte[,] temporaryImage = new byte[height, currentWidth];
                        for (int k = startIndex; k < startIndex + currentWidth; k++)
                            for (int j = 0; j < height; j++)
                                temporaryImage[j, k - startIndex] = imageLoaded[j, k];
                        result[indexOfImage] = ImageLoader.BitmapFromByteArr(temporaryImage);
                        indexOfImage++;
                        currentWidth = 0;
                    }
                }
            }
            return result;
        }

        public static BitmapSource[] CropByBindingComponent(byte[,] imageLoaded)
        {
            int sizeOfArray = 7;
            BitmapSource[] result = new BitmapSource[sizeOfArray];
            int imageLoadedHeight = imageLoaded.GetUpperBound(0) + 1;
            int imageLoadedWidth = imageLoaded.GetUpperBound(1) + 1;
            int width=0, height=0;
            int minwidth = 5;
            int sensity = 50;
            int currentWidth = 0;
            int startIndex = 0;
            int indexOfImage = 0;
            for (int i = 0; i < imageLoadedWidth; i++)
            {
                bool isRowWhite = true;
                for (int j = 0; j < height && isRowWhite; j++)
                {
                    if (imageLoaded[j, i] < sensity)
                        isRowWhite = false;
                }
                if (!isRowWhite)
                {
                    if (currentWidth == 0)
                        startIndex = i;
                    currentWidth++;
                }
                if (currentWidth > 0 && isRowWhite == true)
                {
                    if (currentWidth < minwidth)
                    {
                        currentWidth = 0;
                    }
                    else
                    {
                        currentWidth++;
                        width = currentWidth;
                        byte[,] temporaryImage = new byte[height, currentWidth];
                        for (int k = startIndex; k < startIndex + currentWidth; k++)
                            for (int j = 0; j < height; j++)
                                temporaryImage[j, k - startIndex] = imageLoaded[j, k];
                        result[indexOfImage] = ImageLoader.BitmapFromByteArr(temporaryImage);
                        indexOfImage++;
                        currentWidth = 0;
                    }
                }
            }
            return result;
        }
    }
}
