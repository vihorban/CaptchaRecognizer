using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Captcha1.Algorithms; 

namespace Captcha1.Objects
{
    public class Perceptron
    {
        public int width;
        public int height;
        public int limit;
        public int[,] input;
        public int numNeurons;
        public Neuron[] neurons;

        public Perceptron(int sizeY, int sizeX)
        {
            numNeurons = 10;
            limit = 0;
            height = sizeY;
            width = sizeX;
            neurons = new Neuron[numNeurons];
            for(int i=0; i < numNeurons; i++)
                neurons[i] = new Neuron(sizeY, sizeX);
            input = new int[sizeY, sizeX];
        }

        public int recognize(byte [,] image)
        {
            int result=-1;
            for (int i = 0; i < height; i++)
                for (int j = 0; j < width; j++)
                    input[i, j] = image[i, j];
            foreach (Neuron n in neurons)
            {
                n.countSum(input);
                n.countOutput(limit);
            }
            result = findMaximum();
            return result;
        }

        public void learn()
        {
            int numOfFilesToLearn = 100;
            int numOfProcessions = 3;
            string initialFileName = "C:\\captcha\\examples1\\p";
            string exstension = ".jpeg";
            List<string> learninigStrings = Stream.loadFromFile("C:\\captcha\\examples1\\info.txt");
            for (int k = 1; k <= numOfFilesToLearn; k++)
            {
                string temporaryFileName = initialFileName + k.ToString() + exstension;
                int[] desiredIntegers = new int[6];
                for (int i = 0; i < 6; i++)
                    desiredIntegers[i] = learninigStrings[k][i] - 48;
                byte[,] imageLoaded = ImageLoader.LoadImage(temporaryFileName);
                for (int i = 0; i < numOfProcessions; i++)
                {
                    imageLoaded = NoiseReduction.ReduceByAverage(imageLoaded);
                }
                for (int i = 0; i < 6; i++)
                {
                    byte[,] imageCropped = ImageCroper.CropToSpecificImage(imageLoaded,i);
                    while (recognize(imageCropped) != desiredIntegers[i])
                    {
                        addWeight(imageCropped, desiredIntegers[i]);
                    }
                }
            }
        }

        public void saveWeight()
        {
            int[, , ] temporaryArray = new int[numNeurons, height, width];
            for (int i = 0; i < numNeurons; i++)
                for (int y = 0; i < height; y++)
                    for (int x = 0; x < width; x++)
                        temporaryArray[i, y, x] = neurons[i].weight[y, x];
            Stream.writeToFile("weight.txt", temporaryArray);
        }

        private void addWeight(byte[,] image, int number)
        {
            for (int i = 0; i < height; i++)
                for (int j = 0; j < width; j++)
                    neurons[number].weight[i, j] += image[i, j] / 255;
        }

        private int findMaximum()
        {
            int maxIndex = -1, maxOutput = 0;
            for (int k = 0; k < numNeurons; k++)
            {
                if (neurons[k].output > maxOutput)
                {
                    maxIndex = k;
                    maxOutput = neurons[k].output;
                }
            }
            return maxIndex;
        }
    }
}
