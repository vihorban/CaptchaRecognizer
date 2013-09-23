using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.ComponentModel;
using System.Windows.Data;
using System.Collections.ObjectModel;

using AnalyzerData.Methods;

namespace AnalyzerData.Objects
{
    public class AnotherClass : INotifyPropertyChanged
    {
        private int scrollnumber;
        private int numoffiles;
        public int oldnumoffiles;
        private string fullpath;

        public List<DataGridObject> datagridobjects;
        public List<string> decoded { get; set; }

        public int startindex { get; set; }
        public string info { get; set; }
        public string infoFileName { get; set; }

        public AnotherClass()
        {
            numoffiles = 100;
            oldnumoffiles = numoffiles;
            MaxScroll = 10000;
            scrollnumber = MaxScroll - numoffiles;
            datagridobjects = new List<DataGridObject>();
            decoded = new List<string>();
            fullpath = "C:\\captcha\\examples1\\p1.jpeg";
            int indOfFolder = fullpath.LastIndexOf("examples");
            infoFileName= fullpath.Substring(0,indOfFolder+10)+"info.txt";
        }

        public string FileName
        {
            get
            {
                return fullpath;
            }
            set
            {
                fullpath = value;
                NotifyPropertyChanged("FileName");
            }
        }

        public ObservableCollection<DataGridObject> DataGridObjects
        {
            get
            {
                return new ObservableCollection<DataGridObject>(datagridobjects);
            }
            set
            {
                NotifyPropertyChanged("DataGridObjects");
                NotifyPropertyChanged("Image");
                NotifyPropertyChanged("Value");
            }
        }

        public int MaxScroll { get; set; }

        public int NumOfFiles
        {
            get
            {
                return numoffiles;
            }
            set
            {
                numoffiles = value;
                scrollnumber = MaxScroll - value;
                NotifyPropertyChanged("ScrollNumber");
                NotifyPropertyChanged("NumOfFiles");
            }
        }

        public int ScrollNumber
        {
            get
            {
                return scrollnumber;
            }
            set
            {
                scrollnumber = value;
                numoffiles = MaxScroll - value;
                NotifyPropertyChanged("ScrollNumber");
                NotifyPropertyChanged("NumOfFiles");
            }
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
