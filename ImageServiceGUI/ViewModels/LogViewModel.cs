using ImageServiceGUI.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageService.Logging.Model;

namespace ImageServiceGUI.ViewModels
{
    class LogViewModel : INotifyPropertyChanged
    {
        #region Notify Changed
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion
        /// <summary>
        /// Notify Property Changed
        /// </summary>
        /// <param name="name">The property that changes</param>
        protected void NotifyPropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

        // The LogModel member:
        private LogModel m_LogModel;
        public LogModel logModel
        {
            get { return this.m_LogModel; }
            set
            {
                this.m_LogModel = value;
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public LogViewModel()
        {
            this.m_LogModel = new LogModel();
            m_LogModel.PropertyChanged +=
            delegate (Object sender, PropertyChangedEventArgs e) {
                NotifyPropertyChanged(e.PropertyName);
            };
        }

        // The messages member:
        public ObservableCollection<MessageRecievedEventArgs> Messages
        {
            get { return this.logModel.Messages; }
        }
    }
}
