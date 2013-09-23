using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Captcha1.Objects
{
    public class Neuron
    {
        public int width;
        public int height;
        public int[,] weight;
        public int output;
        public int sum;

        public Neuron(int sizeY, int sizeX)
        {
            height = sizeY;
            width = sizeX;
            output = 0;
            sum = 0;
            weight = new int[sizeY, sizeX];
        }

        public void countSum(int[,] inputImage)
        {
            sum = 0;
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    sum += inputImage[y, x] * weight[y, x] / 255;
                }
            }
        }

        private int activisationFuncrion(int limit)
        {
            if (sum <= limit)
                return 0;
            else
                return sum - limit;
        }

        public void countOutput(int limit)
        {
            output = activisationFuncrion(limit);
        }
    }
}
