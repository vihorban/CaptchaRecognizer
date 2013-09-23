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
            bool usePriorityPixels = true;    //true - середнє вагове
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
    }
}
