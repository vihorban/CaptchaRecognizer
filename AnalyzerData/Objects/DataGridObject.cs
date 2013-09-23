using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.ComponentModel;

namespace AnalyzerData.Objects 
{
    public class DataGridObject : INotifyPropertyChanged
    {
        private string _value;
        private string _image;

        public string Image
        {
            get { return _image; }
            set
            {
                _image = value;
                NotifyPropertyChanged("Image");
            }
        }
        public string Value
        {
            get { return _value; }
            set
            {
                _value = value;
                NotifyPropertyChanged("Value");
            }
        }

        public DataGridObject(string value, string path)
        {
            Value = value;
            Image = path;
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
    };      
}
