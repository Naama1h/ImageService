using ImageServiceGUI.Models;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Text;
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
        }
    

        public string OutputDirectory
        {
            get
            {
                return this.appConfigModel.OutputDir;
            }
        }
        public string SourceName
        {
            get
            {
                return this.appConfigModel.SourceName;
            }
        }
        public string LogName
        {
            get
            {
                return this.appConfigModel.LogName;
            }
        }
        public string ThumbnailSize
        {
            get
            {
                return this.appConfigModel.ThumbnailSize;
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
            string handlersList = ConfigurationManager.AppSettings["Handlers"]; // change to handlers from tcp
            string[] handlers = handlersList.Split(';');
            for (int i = 0; i < handlers.Length; i++)
            {
                this.Handlers.Add(handlers[i]);
            }
        }

        protected void NotifyPropertyChanged(string name)
        {
            //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
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
            //
            // send the server!!!
            //
            this.Handlers.Remove(this.Handler);
            this.appConfigModel = new AppConfigModel();
            Console.Write("remove\n");
        }

        private void PropertyChangedF(object sender, PropertyChangedEventArgs e)
        {
            var command = this.removeCommand as DelegateCommand<object>;
            command.RaiseCanExecuteChanged();
        }

    }
}
