using ImageServiceGUI.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageService.Logging.Modal;

namespace ImageServiceGUI.ViewModels
{
    class LogViewModel : INotifyPropertyChanged
    {
        #region Notify Changed
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion
        protected void NotifyPropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

        private LogModel m_LogModel;

        public LogViewModel()
        {
            this.m_LogModel = new LogModel();
            m_LogModel.PropertyChanged +=
       delegate (Object sender, PropertyChangedEventArgs e) {
           NotifyPropertyChanged(e.PropertyName);
       };

        }

        public LogModel logModel
        {
            get { return this.m_LogModel; }
            set
            {
                this.m_LogModel = value;
            }
        }

        public ObservableCollection<MessageRecievedEventArgs> Messages
        {
            get { return this.logModel.Messages; }
        }
    }
}
