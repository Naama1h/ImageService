using ImageService.Model.Event;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageService.Enums;
using ImageService.Logging;
using ImageService.Controller;
using ImageService.Logging.Model;
using System.Text.RegularExpressions;

namespace ImageService.Handler
{
    // handler.
    public class DirectoyHandler : IDirectoryHandler
    {
        #region Members
        private IImageController m_controller;              // The Image Processing Controller
        private ILoggingService m_logging;                  // Logging Service
        private FileSystemWatcher m_dirWatcher;             // The Watcher of the Dir
        private string m_path;                              // The Path of directory
        #endregion

        public event EventHandler<DirectoryCloseEventArgs> DirectoryClose;              // The Event That Notifies that the Directory is being closed

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="imc">Image controller</param>
        /// <param name="ls">Logging service</param>
        /// <param name="p">Path of the directory</param>
        public DirectoyHandler(IImageController imc, ILoggingService ls, string p)
        {
            this.m_controller = imc;
            this.m_dirWatcher = new FileSystemWatcher(p);
            this.m_logging = ls;
            this.m_path = p;
        }

        /// <summary>
        /// The Function Recieves the directory to Handle
        /// </summary>
        /// <param name="dirPath">Path of the directory</param>
        public void StartHandleDirectory(string dirPath)
        {
            this.m_dirWatcher.Path = dirPath;
            this.m_dirWatcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
                                   | NotifyFilters.FileName | NotifyFilters.DirectoryName;
            this.m_dirWatcher.Filter = "*.*";
            this.m_dirWatcher.Changed += new FileSystemEventHandler(OnChanged);
            this.m_dirWatcher.EnableRaisingEvents = true;
            this.m_logging.Log("start handle directory" + dirPath, MessageTypeEnum.INFO);
        }
        /// <summary>
        /// The function will heppend with the event
        /// </summary>
        /// <param name="source">the source</param>
        /// <param name="e">file system event args</param>
        private void OnChanged(object source, FileSystemEventArgs e)
        {
            this.m_logging.Log("in OnChanged", MessageTypeEnum.INFO);
            string[] args1 = { e.FullPath };
            CommandRecievedEventArgs e1 = new CommandRecievedEventArgs((int)CommandEnum.NewFileCommand, args1 ,"");
            OnCommandRecieved(source, e1);
        }

        /// <summary>
        /// The Event that will be activated upon new Command
        /// </summary>
        /// <param name="sender">the source</param>
        /// <param name="e">commend recived event args</param>
        public void OnCommandRecieved(object sender, CommandRecievedEventArgs e)
        {
            bool result;
            // if the commend is for a new file, write to the log and execute the command.
            // else, invoke the event that will clode the handlers
            if (e.CommandID == (int)CommandEnum.NewFileCommand)
            {
                string msg = this.m_controller.ExecuteCommand(e.CommandID, e.Args, out result);
                if (result)
                {
                    this.m_logging.Log(msg, MessageTypeEnum.INFO);
                }
                else
                {
                    this.m_logging.Log(msg, MessageTypeEnum.WARNING);
                }
            } else
            {
                this.DirectoryClose?.Invoke(this, new DirectoryCloseEventArgs(e.RequestDirPath, "close the handlers"));
                this.m_dirWatcher.EnableRaisingEvents = false;
            }
        }
    }
}