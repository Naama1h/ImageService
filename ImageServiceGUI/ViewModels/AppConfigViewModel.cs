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
        private AppConfigModel m_AppConfigModel;
        #region Notify Changed
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        public ICommand removeCommand { get; private set; }

        // the Handlers member:
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
    

        public string OutputDir
        {
            get
            {
                return this.appConfigModel.OutputDir;
            }
            set
            {
                this.appConfigModel.OutputDir = value;
            }
        }
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

            /**
            string handlersList = ConfigurationManager.AppSettings["Handlers"]; // change to handlers from tcp
            string[] handlers = handlersList.Split(';');
            for (int i = 0; i < handlers.Length; i++)
            {
                this.Handlers.Add(handlers[i]);
            }
            */
        }

        protected void NotifyPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private string BuildResultString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(this.Handler);
            return builder.ToString();
        }

        private bool CanRemove(object obj)
        {
            if (string.IsNullOrEmpty(this.Handler))
            {
                return false;
            }
            return true;
        }

        private void OnRemove(object obj)
        {
            string[] args = { this.Handler };
            CommandMessage message1 = new CommandMessage((int)CommandEnum.CloseCommand, args);
            Communication.CommunicationServer.Instance.sendmessage(message1.ToJSON());
            this.Handlers.Remove(this.Handler);
            this.appConfigModel = new AppConfigModel();
            Console.Write("remove\n");
        }

        private void PropertyChangedF(object sender, PropertyChangedEventArgs e)
        {
            var command = this.removeCommand as DelegateCommand<object>;
            command.RaiseCanExecuteChanged();
        }

        public void settingsMessage(object sender, DataRecivedEventArgs e)
        {
            CommandMessage cm = CommandMessage.ParseJSon(e.Data);
            if (cm.CommandID == (int)CommandEnum.GetConfigCommand)
            {
                int i = 0;
                while (cm.CommandArgs[i] != null)
                {
                    App.Current.Dispatcher.Invoke((System.Action)delegate
                    {
                        this.Handlers.Add(cm.CommandArgs[i]);
                    });
                    i++;
                }
                App.Current.Dispatcher.Invoke((System.Action)delegate
                {
                    i++;
                    this.OutputDir = cm.CommandArgs[i];
                    i++;
                    this.SourceName = cm.CommandArgs[i];
                    i++;
                    this.LogName = cm.CommandArgs[i];
                    i++;
                    this.ThumbnailSize = cm.CommandArgs[i];
                });
                this.m_AppConfigModel = new AppConfigModel();
                string[] args = { "add to setting" };
                CommandMessage message = new CommandMessage(4, args);
                CommunicationServer.Instance.sendmessage(message.ToJSON());
            }
            else if (cm.CommandID == (int)CommandEnum.CloseCommand)
            {
                // remove the handler
                string[] args = { cm.CommandArgs[0] + " removed" };
                CommandMessage message = new CommandMessage(4, args);
                CommunicationServer.Instance.sendmessage(message.ToJSON());
            }
        }

    }
}
