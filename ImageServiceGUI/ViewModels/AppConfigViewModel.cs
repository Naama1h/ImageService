using ImageService.Enums;
using ImageServiceCommunication;
using ImageServiceCommunication.Event;
using ImageServiceGUI.Communication;
using ImageServiceGUI.Models;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ImageServiceGUI.ViewModels
{
    class AppConfigViewModel : INotifyPropertyChanged
    {
        private AppConfigModel m_AppConfigModel;                    // The App Config Model
        #region Notify Changed
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        // the Remove Command member:
        public ICommand removeCommand { get; set; }

        // the Handler member:
        private string m_Handler;
        public string Handler
        {
            get { return m_Handler; }
            set
            {
                m_Handler = value;
                var command = this.removeCommand as DelegateCommand<object>;
                command.RaiseCanExecuteChanged();
            }
        }

        // the Handlers member:
        public ObservableCollection<string> Handlers
        {
            get
            {
                return this.appConfigModel.Handlers;
            }
            set
            {
                this.m_AppConfigModel.Handlers = value;
            }
        }

        // the OutputDir member:
        public string OutputDir
        {
            get
            {
                return this.appConfigModel.OutputDir;
            }
            //set
            //{
            //    this.appConfigModel.OutputDir = value;
            //}
        }

        // the SourceName member:
        public string SourceName
        {
            get
            {
                return this.appConfigModel.SourceName;
            }
            set
            {
                this.appConfigModel.SourceName = value;
            }
        }

        // the LogName member:
        public string LogName
        {
            get
            {
                return this.appConfigModel.LogName;
            }
            set
            {
                this.appConfigModel.LogName = value;
            }
        }

        // the ThumbnailSize member:
        public string ThumbnailSize
        {
            get
            {
                return this.appConfigModel.ThumbnailSize;
            }
            set
            {
               this.appConfigModel.ThumbnailSize = value;
            }
        }

        // the AppConfigModel member:
        public AppConfigModel appConfigModel
        {
            get
            {
                return this.m_AppConfigModel;
            }
            set
            {
                this.m_AppConfigModel = value;
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public AppConfigViewModel()
        {
            this.m_AppConfigModel = new AppConfigModel();
            m_AppConfigModel.PropertyChanged += delegate (Object sender, PropertyChangedEventArgs e) {
                NotifyPropertyChanged(e.PropertyName);
            };
            this.PropertyChanged += PropertyChangedF;
            this.removeCommand = new DelegateCommand<object>(this.OnRemove, this.CanRemove);
            this.appConfigModel.Handlers = new ObservableCollection<string>();
            CommunicationServer.Instance.DataReceived += settingsMessage;
        }

        /// <summary>
        /// Notify Property Changed
        /// </summary>
        /// <param name="name">The property that changes</param>
        protected void NotifyPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        /// <summary>
        /// Build Result String
        /// </summary>
        /// <returns>The Result String</returns>
        private string BuildResultString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(this.Handler);
            return builder.ToString();
        }

        /// <summary>
        /// Return if we can remove
        /// </summary>
        /// <param name="obj">The object</param>
        /// <returns>if we can remove him or not</returns>
        private bool CanRemove(object obj)
        {
            if (string.IsNullOrEmpty(this.Handler))
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// On Remove
        /// </summary>
        /// <param name="obj">The object</param>
        private void OnRemove(object obj)
        {
            string[] args = { this.Handler };
            CommandMessage message1 = new CommandMessage((int)CommandEnum.CloseHandler, args);
            Communication.CommunicationServer.Instance.sendmessage(message1.ToJSON());
        }

        /// <summary>
        /// Property Changed Func
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">The property</param>
        private void PropertyChangedF(object sender, PropertyChangedEventArgs e)
        {
            var command = this.removeCommand as DelegateCommand<object>;
            command.RaiseCanExecuteChanged();
        }

        /// <summary>
        /// Get The Settings Message
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">The message</param>
        public void settingsMessage(object sender, DataRecivedEventArgs e)
        {
            if (e.Data == null)
            {
                return;
            }
            CommandMessage cm = CommandMessage.ParseJSon(e.Data);
            // check if this is a settings message
            if (cm.CommandID == (int)CommandEnum.GetConfigCommand)
            {
                int i = 0;
                // update the handlers
                while (cm.CommandArgs[i] != null)
                {
                    App.Current.Dispatcher.Invoke((System.Action)delegate
                    {
                        this.Handlers.Add(cm.CommandArgs[i]);
                    });
                    i++;
                }
                // update the rest members
                App.Current.Dispatcher.Invoke((System.Action)delegate
                {
                    i++;
                    this.appConfigModel.OutputDir = cm.CommandArgs[i];
                    i++;
                    this.SourceName = cm.CommandArgs[i];
                    i++;
                    this.LogName = cm.CommandArgs[i];
                    i++;
                    this.ThumbnailSize = cm.CommandArgs[i];
                });
                // send back that we add the settings
                string[] args = { "add to setting" };
                CommandMessage message = new CommandMessage((int)CommandEnum.TcpMessage, args);
                CommunicationServer.Instance.sendmessage(message.ToJSON());
            }
            // check if we need to remove handler from the list
            else if (cm.CommandID == (int)CommandEnum.CloseHandler)
            {
                App.Current.Dispatcher.Invoke((System.Action)delegate
                {
                    this.Handlers.Remove(cm.CommandArgs[0]);
                });
            }
        }
    }
}