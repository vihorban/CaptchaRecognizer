using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Captcha1.Algorithms
{
    public class NoiseReduction
    {
        public static byte[,] ReduceByAverage(byte[,] img)
        {
            bool usePriorityPixels = false;    //true - середнє вагове
            int height = img.GetUpperBound(0)+1;
            int width = img.GetUpperBound(1)+1;
            byte[,] result = new byte[height, width];
            for (int i = 1; i < height - 1; i++)
                for (int j = 1; j < width - 1; j++)
                {
                    int avg = 0;
                    if (usePriorityPixels)
                    {
                        int sum = img[i - 1, j - 1] + 2 * img[i - 1, j] + img[i - 1, j + 1] +
                                  img[i + 1, j - 1] + 2 * img[i + 1, j] + img[i - 1, j + 1] +
                                  2 * img[i, j - 1] + 4 * img[i, j] + 2 * img[i, j + 1];
                        avg = sum / 16;
                    }
                    else
                    {
                        int sum = img[i - 1, j - 1] + img[i - 1, j] + img[i - 1, j + 1] +
                                  img[i + 1, j - 1] + img[i + 1, j] + img[i - 1, j + 1] +
                                   img[i, j - 1] + img[i, j] + img[i, j + 1];
                        avg = sum / 9;
                    }
                    if (avg > 127)
                        result[i, j] = 255;
                    else
                        result[i, j] = 0;
                }
            for (int i = 0; i < height; i++)
            {
                result[i, 0] = 255;
                result[i, width - 1] = 255;
            }
            for (int j = 0; j < width; j++)
            {
                result[0, j] = 255;
                result[height-1, j] = 255;
            }
            return result;
        }
        public static byte[,] ReduceShifts(byte[,] img)
        {
            int height = img.GetUpperBound(0) + 1;
            int outputHeight = 20;
            int width = img.GetUpperBound(1) + 1;
            byte[,] result = new byte[outputHeight, width];
            byte[,] tempImage = new byte[35, width];
            int[,] maxpoints = findMaxPoints(img);
            int[,] minpoints = findMinPoints(img);
            for (int i = 0; i < width; i++)
            {
                int maxValue = linearInterpolation(maxpoints, i) + 2;
                int minValue = linearInterpolation(minpoints, i) - 1;
                int difValue = maxValue - minValue;
                for (int j = 0; j < difValue; j++)
                {
                    tempImage[j, i] = img[minValue + j, i];
                }
                for (int j = difValue; j < 35; j++)
                {
                    tempImage[j, i] = 255;
                }
            }
            Stream.saveImageToFile("beforeLeveling.jpg",ImageLoader.BitmapFromByteArr(tempImage));
            for (int i = 0; i < width; i++)
            {
                int maxValue = linearInterpolation(maxpoints, i) + 2;
                int minValue = linearInterpolation(minpoints, i) - 1;
                int difValue = maxValue - minValue;
                double koef = (double)outputHeight / difValue;
                double nextPixel = 1;
                double nextBorder = koef;
                int numofNextBorder=0;
                for (int j = 0; j < outputHeight; j++,nextPixel+=1)
                {
                    double byteValue = 0;
                    double kurPosition=j;
                    while (kurPosition < nextPixel)
                    {
                        if (nextBorder <= nextPixel)
                        {
                            byteValue += (nextBorder - kurPosition) * img[minValue + numofNextBorder, i];
                            kurPosition = nextBorder;
                            nextBorder += koef;
                            numofNextBorder++;
                        }
                        else
                        {
                            byteValue += (nextPixel - kurPosition) * img[minValue + numofNextBorder, i];
                            kurPosition = nextPixel;
                        }
                    }
                    result[j, i] = (byte)byteValue;
                }
            }
            //for (int i = 0; i < 5; i++)
            //{
            //    result[maxpoints[i, 0], maxpoints[i, 1]] = 255;
            //    result[minpoints[i, 0], minpoints[i, 1]] = 255;
            //}
            Stream.saveImageToFile("afterLeveling.jpg", ImageLoader.BitmapFromByteArr(result));
            return result;
        }

        public static int[,] findMaxPoints(byte[,] img)
        {
            int[,] result = new int[5,2];
            int width = 20;
            int height = 45;
            int startY = 3;
            for (int i = 0; i < 5; i++)
            {
                int startX = 33 + 15 * i;
                bool found=false;
                int bound = 100;
                int maxValue = 0;
                int helpValue = 0;
                while (!found)
                {
                    for (int y = startY; y < height + startY; y++)
                        for (int x = startX; x < startX + width; x++)
                        {
                            if (img[y, x] < bound)
                            {
                                bool ok = true;
                                if (i > 0)
                                {
                                    if (Math.Abs(y - result[i - 1, 0]) > 10 || Math.Abs(x - result[i - 1, 1]) < 10)
                                        ok = false;
                                }
                                if (y > maxValue && ok)
                                {
                                    maxValue = y;
                                    helpValue = x;
                                }
                            }
                        }
                    if (maxValue != 0)
                    {
                        result[i, 0] = maxValue;
                        result[i, 1] = helpValue;
                        found = true;
                    }
                    else
                    {
                        bound += 5;
                    }
                }
            }
            return result;
        }
        public static int[,] findMinPoints(byte[,] img)
        {
            int[,] result = new int[5, 2];
            int width = 20;
            int height = 45;
            int startY = 3;
            for (int i = 0; i < 5; i++)
            {
                int startX = 33 + 15 * i;
                bool found = false;
                int bound = 100;
                int minValue = 50;
                int helpValue = 0;
                while(!found)
                {
                    for (int y = startY; y < height + startY; y++)
                        for (int x = startX; x < startX + width; x++)
                        {
                            if (img[y, x] < bound)
                            {
                                bool ok = true;
                                if (i > 0)
                                {
                                    if (Math.Abs(y - result[i - 1, 0]) > 10 || Math.Abs(x - result[i - 1, 1]) < 10)
                                        ok = false;
                                }
                                if (y < minValue && ok)
                                {
                                    minValue = y;
                                    helpValue = x;
                                }
                            }
                        }
                    if(minValue!=50)
                    {
                        result[i, 0] = minValue;
                        result[i, 1] = helpValue;
                        found=true;
                    }
                    else
                    {
                        bound+=5;
                    }
                }
            }
            return result;
        }
        public static int linearInterpolation(int[,] data, int x)
        {
            int result = -1;
            if (x <= data[0, 1])
                return data[0, 0];
            if (x > data[4, 1])
                return data[4, 0];
            for (int i = 0; i < 4; i++)
                if (x <= data[i + 1, 1] && x > data[i, 1])
                    return data[i, 0] + (x - data[i, 1]) * (data[i + 1, 0] - data[i, 0]) / (data[i + 1, 1] - data[i, 1]);
            return result;
        }
    }
}
