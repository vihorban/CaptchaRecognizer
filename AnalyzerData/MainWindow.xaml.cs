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

using AnalyzerData.Objects;
using AnalyzerData.Methods;
using System.Collections.ObjectModel;

namespace AnalyzerData
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        AnotherClass datacontext;
        bool everChanged;

        public MainWindow()
        {
            InitializeComponent();
            datacontext = new AnotherClass();
            this.DataContext = datacontext;
            everChanged = false;
        }

        private void ButtonSource_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog OpenDialog1 = new OpenFileDialog();
            OpenDialog1.Filter = "Файли зображень(*.BMP;*.JPG;*.GIF; *.JPEG)|*.BMP;*.JPG;*.GIF;*.JPEG|Всі файли (*.*)|*.*";
            if (OpenDialog1.ShowDialog() == true)
            {
                datacontext.FileName = OpenDialog1.FileName;
            }
        }

        private void ButtonShow_Click(object sender, RoutedEventArgs e)
        {
            if(everChanged)
                WriteToFile();
            datacontext.oldnumoffiles = datacontext.NumOfFiles;
            datacontext.datagridobjects.Clear();
            datacontext.decoded.Clear();
            string fullpath = TextBoxSource.Text;
            int indOfPoint = fullpath.IndexOf(".jpeg");
            int indOfPict = fullpath.LastIndexOf("\\p");
            int indOfFolder = fullpath.LastIndexOf("examples");
            string path1 = fullpath.Substring(0, indOfPict + 2);
            string path2 = fullpath.Substring(indOfPoint);
            string number = fullpath.Substring(indOfPict + 2, indOfPoint - indOfPict - 2);
            datacontext.infoFileName = fullpath.Substring(0, indOfFolder + 10) + "info.txt";
            string temporary;
            datacontext.decoded = Stream.loadFromFile(datacontext.infoFileName, out temporary);
            datacontext.info = temporary;
            int temp = datacontext.decoded.Count;
            if (temp < 10000)
                for (int i = 0; i < 10000 - temp;i++ )
                    datacontext.decoded.Add("000000");
            everChanged = true;
            datacontext.startindex = int.Parse(number);
            for (int i = datacontext.startindex; i < datacontext.startindex + datacontext.NumOfFiles; i++)
            {
                datacontext.datagridobjects.Add(new DataGridObject(datacontext.decoded[i - 1], path1 + i.ToString() + path2));
            }
            datacontext.DataGridObjects = new ObservableCollection<DataGridObject>(datacontext.datagridobjects);
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            WriteToFile();
        }

        private void WriteToFile()
        {
            if (datacontext.datagridobjects.Count > 0)
            {
                for (int i = datacontext.startindex, k = 0; i < datacontext.oldnumoffiles + datacontext.startindex; i++, k++)
                {
                    datacontext.decoded[i - 1] = datacontext.datagridobjects[k].Value;
                }
            }
            Stream.writeToFile(datacontext.infoFileName, datacontext.info, datacontext.decoded);
        }

    }

}
