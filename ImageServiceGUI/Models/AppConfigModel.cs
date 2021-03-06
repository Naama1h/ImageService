﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Collections.ObjectModel;
using ImageServiceGUI.Communication;
using System.IO.Ports;
using System.Diagnostics;
using ImageServiceCommunication;
using ImageServiceCommunication.Enums;
using ImageServiceCommunication.Event;

namespace ImageServiceGUI.Models
{
    class AppConfigModel : INotifyPropertyChanged
    {
        #region Notify Changed
        public event PropertyChangedEventHandler PropertyChanged;

        public AppConfigModel()
        {
        }

        protected void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
        #endregion

        // the Handler member:
        private string m_Handler;
        public string Handler
        {
            get { return m_Handler; }
            set
            {
                m_Handler = value;
                this.OnPropertyChanged("Handler");
            }
        }

        // the list of handlers
        private ObservableCollection<string> handlers;
        public ObservableCollection<string> Handlers
        {
            get
            {
                return handlers;
            }
            set
            {
                handlers = value;
                this.OnPropertyChanged("Handlers");
            }
        }

        // the OutputDir member:
        private string m_OutputDir;
        public string OutputDir
        {
            get { return m_OutputDir; }
            set
            {
                m_OutputDir = value;
                this.OnPropertyChanged("OutputDir");
            }
        }

        // the SourceName member:
        private string m_SourceName;
        public string SourceName
        {
            get { return m_SourceName; }
            set
            {
                m_SourceName = value;
                this.OnPropertyChanged("SourceName");
            }
        }

        // the LogName member:
        private string m_LogName;
        public string LogName
        {
            get { return m_LogName; }
            set
            {
                m_LogName = value;
                this.OnPropertyChanged("LogName");
            }
        }

        // the ThumbnailSize member:
        private string m_ThumbnailSize;
        public string ThumbnailSize
        {
            get { return m_ThumbnailSize; }
            set
            {
                m_ThumbnailSize = value;
                this.OnPropertyChanged("ThumbnailSize");
            }
        }
    }
}
