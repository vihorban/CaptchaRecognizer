using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using Microsoft.Win32;
using System.ComponentModel;

using Captcha1.Objects;
using Captcha1.Algorithms;

namespace Captcha1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MyDataContext myDataContext;
        Perceptron perceptron;
        OpenFileDialog OpenFileDialogPath;
        BackgroundWorker BackgroundWorkerCheckAccuracy;
        private int numOfProcessions;

        public MainWindow()
        {
            InitializeComponent();

            OpenFileDialogPath = new OpenFileDialog();
            OpenFileDialogPath.Filter = "Файли зображень(*.BMP;*.JPG;*.GIF; *.JPEG)|*.BMP;*.JPG;*.GIF;*.JPEG|Всі файли (*.*)|*.*";

            myDataContext=new MyDataContext();
            this.DataContext = myDataContext;
            perceptron = new Perceptron(24, 12);
            perceptron.learn();

            numOfProcessions = 3;

            BackgroundWorkerCheckAccuracy = new BackgroundWorker();
            BackgroundWorkerCheckAccuracy.DoWork+=new DoWorkEventHandler(BackgroundWorkerCheckAccuracy_DoWork);
        }

        private void ButtonFilePath_Click(object sender, RoutedEventArgs e)
        {
            if (OpenFileDialogPath.ShowDialog() == true)
            {
                myDataContext.FileName = OpenFileDialogPath.FileName;
            }
        }

        private void ButtonCrop_Click(object sender, RoutedEventArgs e)
        {
            byte[,] imageLoaded = ImageLoader.LoadImage(myDataContext.FileName);
            for (int i = 0; i < numOfProcessions; i++)
            {
                imageLoaded = NoiseReduction.ReduceByAverage(imageLoaded);
            }
            myDataContext.ImageConverted = ImageLoader.BitmapFromByteArr(imageLoaded);
            Stream.saveImageToFile("temp.jpg", myDataContext.ImageConverted);
            myDataContext.BitmapSources = ImageCroper.CropByPixels(imageLoaded);
            for(int i=0;i<6;i++)
            {
                myDataContext.Recognized[i]=perceptron.recognize(ImageCroper.CropToSpecificImage(imageLoaded,i));
            }
            myDataContext.Recognized = myDataContext.Recognized;
        }

        private void ButtonAccuracy_Click(object sender, RoutedEventArgs e)
        {
            BackgroundWorkerCheckAccuracy.RunWorkerAsync();
        }

        private void BackgroundWorkerCheckAccuracy_DoWork(object Sender, DoWorkEventArgs arg)
        {
            string info;
            string infoFileName = "C:\\captcha\\examples1\\info.txt";
            List<string> neededStrings = Stream.loadFromFile(infoFileName, out info);
            int startIndex = 1001;
            int numberOfChecks = 2000;
            int size = neededStrings.Count;
            int[,] neededIntegers = new int[size, 6];
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    neededIntegers[i, j] = neededStrings[i][j] - 48;
                }
            }
            string initialFileName = "C:\\captcha\\examples1\\p";
            string exstension = ".jpeg";
            int totalErrors = 0;
            int symbolErrors = 0;
            for (int k = startIndex; k < startIndex + numberOfChecks; k++)
            {
                myDataContext.ProgressStatus = (double)(k - startIndex) / numberOfChecks * 100;
                string temporaryFileName = initialFileName + k.ToString() + exstension;
                byte[,] imageLoaded = ImageLoader.LoadImage(temporaryFileName);
                for (int i = 0; i < numOfProcessions; i++)
                {
                    imageLoaded = NoiseReduction.ReduceByAverage(imageLoaded);
                }
                bool totalError = false;
                string resultstring = "";
                for (int i = 0; i < 6; i++)
                {
                    byte[,] imageCropped = ImageCroper.CropToSpecificImage(imageLoaded, i);
                    int recognized = perceptron.recognize(imageCropped);
                    resultstring += recognized.ToString();
                    if (neededIntegers[k - 1, i] != recognized)
                    {
                        symbolErrors++;
                        totalError = true;
                    }
                }
                //neededStrings[k - 1] = resultstring;
                if (totalError) totalErrors++;
            }
            myDataContext.AccuracyTotal = (double)(numberOfChecks - totalErrors) / numberOfChecks * 100;
            myDataContext.AccuracySymbol = (double)(numberOfChecks * 6 - symbolErrors) / numberOfChecks / 6 * 100;
            //perceptron.saveWeight();
            //Stream.writeToFile(infoFileName, info, neededStrings);
        }
    }
}
