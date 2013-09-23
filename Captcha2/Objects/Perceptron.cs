using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Captcha1.Algorithms;

namespace Captcha1.Objects
{
    public class Perceptron
    {
        public int[,,] mul; // Тут будем хранить отмасштабированные сигналы
        public int[,,] weight; // Массив для хранения весов
        public int[,] input; // Входная информация
        public int limit = 9; // Порог - выбран экспериментально, для быстрого обучения
        public int[] sum; // Тут сохраним сумму масштабированных сигналов
        public int width;
        public int height;
        public int numNeurons;

        public Perceptron(int sizey, int sizex, int[,] inP) // Задаем свойства при создании объекта
        {
            numNeurons = 10;
            weight = new int[numNeurons, sizey, sizex]; // Определяемся с размером массива (число входов)
            mul = new int[numNeurons, sizey, sizex];
            sum= new int[numNeurons];
            input = new int[sizey, sizex];
            input = inP; // Получаем входные данные
            height = sizey;
            width = sizex;
        }

        public Perceptron(int sizey, int sizex)
        {
            numNeurons = 10;
            weight = new int[numNeurons, sizey, sizex]; // Определяемся с размером массива (число входов)
            mul = new int[numNeurons, sizey, sizex];
            sum = new int[numNeurons];
            input = new int[sizey, sizex];
            height = sizey;
            width = sizex;
        }

        public void mul_w()
        {
            for (int k = 0; k < numNeurons; k++)
            {
                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++) // Пробегаем по каждому аксону
                    {
                        mul[k, y, x] = input[y, x] * weight[k, y, x] / 255;
                    }
                }
            }
        }

        public void Sum()
        {
            for (int k = 0; k < numNeurons; k++)
            {
                sum[k] = 0;
                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        sum[k] += mul[k, y, x];
                    }
                }
            }
        }

        public int Rez()
        {
            int max=-1,maxsum=0;
            for (int k = 0; k < numNeurons; k++)
            {
                if (sum[k] > maxsum)
                {
                    max = k;
                    maxsum = sum[k];
                }
            }
            return max;
        }

        public int Recognize(byte [,] image)
        {
            int result=-1;
            for (int i = 0; i < height; i++)
                for (int j = 0; j < width; j++)
                    input[i, j] = image[i, j];
            mul_w();
            Sum();
            result=Rez();
            return result;
        }

        public void Learn()
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
                    while (Recognize(imageCropped) != desiredIntegers[i])
                    {
                        AddWeight(imageCropped, desiredIntegers[i]);
                    }
                }
            }
        }

        public void AddWeight(byte[,] image, int number)
        {
            for (int i = 0; i < height; i++)
                for (int j = 0; j < width; j++)
                    weight[number, i, j] += image[i, j]/255;
        }

        public void saveWeight()
        {
            Stream.writeToFile("weight.txt", weight);
        }
    }
}
