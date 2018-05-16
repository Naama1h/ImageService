using ImageService.Enums;
using ImageService.Logging.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageServiceGUI.Communication;
using System.IO.Ports;
using ImageServiceCommunication;
using System.Diagnostics;
using ImageServiceCommunication.Event;

namespace ImageServiceGUI.Models
{
    class LogModel : INotifyPropertyChanged
    {
        #region Notify Changed
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// On Property Changed
        /// </summary>
        /// <param name="name">The Name of the property that changes</param>
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
                OnPropertyChanged("messages");
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public LogModel()
        {
            this.m_Messages = new ObservableCollection<MessageRecievedEventArgs>();
            CommunicationServer.Instance.DataReceived += addMessageToLog;
        }

        /// <summary>
        /// Add Message To Log
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">The Message</param>
        public void addMessageToLog(object sender, DataRecivedEventArgs e)
        {
            CommandMessage cm = CommandMessage.ParseJSon(e.Data);
            // check if this id log command
            if (cm.CommandID == (int)CommandEnum.LogCommand)
            {
                // check the status
                if (cm.CommandArgs[0].Equals(MessageTypeEnum.FAIL.ToString()))
                {
                    MessageRecievedEventArgs message = new MessageRecievedEventArgs(MessageTypeEnum.FAIL, cm.CommandArgs[1]);
                    App.Current.Dispatcher.Invoke((System.Action)delegate
                    {
                        this.m_Messages.Add(message);
                    });
                }
                else if (cm.CommandArgs[0].Equals(MessageTypeEnum.INFO.ToString()))
                {
                    MessageRecievedEventArgs message = new MessageRecievedEventArgs(MessageTypeEnum.INFO, cm.CommandArgs[1]);
                    App.Current.Dispatcher.Invoke((System.Action)delegate
                    {
                        this.m_Messages.Add(message);
                    });
                }
                else
                {
                    MessageRecievedEventArgs message = new MessageRecievedEventArgs(MessageTypeEnum.WARNING, cm.CommandArgs[1]);
                    App.Current.Dispatcher.Invoke((System.Action)delegate
                    {
                        this.m_Messages.Add(message);
                    });
                }
                // send back that the message added:
                string[] args = { "add to log" };
                CommandMessage message1 = new CommandMessage(4, args);
                CommunicationServer.Instance.sendmessage(message1.ToJSON());
            }
        }
    }
}