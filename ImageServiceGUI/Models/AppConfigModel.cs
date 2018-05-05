﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Collections.ObjectModel;

namespace ImageServiceGUI.Models
{
    class AppConfigModel : INotifyPropertyChanged
    {
        #region Notify Changed
        public event PropertyChangedEventHandler PropertyChanged;

        public AppConfigModel()
        {
            this.m_OutputDir = ConfigurationManager.AppSettings["OutPutDir"];
            this.m_SourceName = ConfigurationManager.AppSettings["SourceName"];
            this.m_LogName = ConfigurationManager.AppSettings["LogName"];
            this.m_ThumbnailSize = ConfigurationManager.AppSettings["ThumbnailSize"];
        }

        protected void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
        #endregion
        
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
                //this.OnPropertyChanged("handlers");
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
            }
        }

    }
}
