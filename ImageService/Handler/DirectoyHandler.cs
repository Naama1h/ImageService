using ImageService.Modal.Event;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageService.Enums;
using ImageService.Logging;
using ImageService.Controller;
using ImageService.Logging.Modal;
using System.Text.RegularExpressions;

namespace ImageService.Handler
{
    public class DirectoyHandler : IDirectoryHandler
    {
        #region Members
        private IImageController m_controller;              // The Image Processing Controller
        private ILoggingService m_logging;
        private FileSystemWatcher m_dirWatcher;             // The Watcher of the Dir
        private string m_path;                              // The Path of directory
        #endregion

        public event EventHandler<DirectoryCloseEventArgs> DirectoryClose;              // The Event That Notifies that the Directory is being closed

        // constructor
        public DirectoyHandler(IImageController imc, ILoggingService ls, string p)
        {
            this.m_controller = imc;
            this.m_dirWatcher = new FileSystemWatcher(p);
            this.m_logging = ls;
            this.m_path = p;
        }
        
        // The Function Recieves the directory to Handle
        public void StartHandleDirectory(string dirPath)
        {
            this.m_dirWatcher.Path = dirPath;
            this.m_dirWatcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
                                   | NotifyFilters.FileName | NotifyFilters.DirectoryName;
            this.m_dirWatcher.Filter = "*.*";
            this.m_dirWatcher.Changed += new FileSystemEventHandler(OnChanged);
            this.m_dirWatcher.EnableRaisingEvents = true;
        }

        private void OnChanged(object source, FileSystemEventArgs e)
        {
            string[] args1 = { System.Configuration.ConfigurationManager.AppSettings["SourceName"] };
            CommandRecievedEventArgs e1 = new CommandRecievedEventArgs(1, args1 ,e.FullPath);
            OnCommandRecieved(source, e1);
        }

        // The Event that will be activated upon new Command
        public void OnCommandRecieved(object sender, CommandRecievedEventArgs e)
        {
            bool result;
            if (e.RequestDirPath.Equals(this.m_path))
            {
                this.m_controller.ExecuteCommand(e.CommandID, e.Args, out result);
            }
        }
    }
}