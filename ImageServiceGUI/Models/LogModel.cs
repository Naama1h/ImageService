using ImageService.Logging.Modal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageServiceGUI.Models
{
    class LogModel : INotifyPropertyChanged
    {
        #region Notify Changed
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
        #endregion

        // the SourceName member:
        private MessageRecievedEventArgs[] m_Massages;
        public MessageRecievedEventArgs[] Massages
        {
            get { return m_Massages; }
            set
            {
                m_Massages = value;
                OnPropertyChanged("Massages");
            }
        }
    }
}
