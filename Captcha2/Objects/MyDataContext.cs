using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.ComponentModel;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Captcha1.Objects
{
    public class MyDataContext: INotifyPropertyChanged
    {
        private string fileName;
        private BitmapSource[] bitmapSources;
        private BitmapSource imageConverted;
        private int[] recognized;
        double accuracySymbol;
        double accuracyTotal;
        double progressStatus;

        public string FileName
        {
            get
            {
                return fileName;
            }
            set
            {
                fileName = value;
                NotifyPropertyChanged("FileName");
            }
        }

        public BitmapSource[] BitmapSources
        {
            get
            {
                return bitmapSources;
            }
            set
            {
                bitmapSources = value;
                NotifyPropertyChanged("BitmapSources");
            }
        }

        public int[] Recognized
        {
            get
            {
                return recognized;
            }
            set
            {
                recognized = value;
                NotifyPropertyChanged("Recognized");
            }
        }

        public BitmapSource ImageConverted
        {
            get
            {
                return imageConverted;
            }
            set
            {
                imageConverted = value;
                NotifyPropertyChanged("ImageConverted");
            }
        }

        public double AccuracySymbol
        {
            get
            {
                return accuracySymbol;
            }
            set
            {
                accuracySymbol = value;
                NotifyPropertyChanged("AccuracySymbol");
            }
        }

        public double AccuracyTotal
        {
            get
            {
                return accuracyTotal;
            }
            set
            {
                accuracyTotal = value;
                NotifyPropertyChanged("AccuracyTotal");
            }
        }

        public double ProgressStatus
        {
            get
            {
                return progressStatus;
            }
            set
            {
                progressStatus = value;
                NotifyPropertyChanged("ProgressStatus");
            }
        }

        public MyDataContext()
        {
            fileName = "C:\\captcha\\examples2\\p1.png";
            bitmapSources = new BitmapSource[5];
            recognized = new int[5];
            accuracySymbol = 0;
            accuracyTotal = 0;
            progressStatus = 0;
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Private Helpers

        protected void NotifyPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        #endregion
    }
}
