using ImageService.Enums;
using ImageService.Logging.Modal;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageService.Logging.Modal;

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
        private ObservableCollection<MessageRecievedEventArgs> m_Messages;
        public ObservableCollection<MessageRecievedEventArgs> Messages
        {
            get { return m_Messages; }
            set
            {
                m_Messages = value;
                OnPropertyChanged("Massages");
            }
        }

        public LogModel()
        {
            this.m_Messages = new ObservableCollection<MessageRecievedEventArgs>();
            MessageRecievedEventArgs example1 = new MessageRecievedEventArgs(MessageTypeEnum.WARNING, "try1");
            MessageRecievedEventArgs example2 = new MessageRecievedEventArgs(MessageTypeEnum.INFO, "try2");
            MessageRecievedEventArgs example3 = new MessageRecievedEventArgs(MessageTypeEnum.FAIL, "try3");
            this.m_Messages.Add(example1);
            this.m_Messages.Add(example2);
            this.m_Messages.Add(example3);
        }
    }
}
