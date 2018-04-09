using ImageService.Controller;
using ImageService.Commands;
using ImageService.Handler;
using ImageService.Enums;
using ImageService.Logging;
using ImageService.Modal.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace ImageService.Server
{
    public class ImageServer
    {
        #region Members
        private IImageController m_controller;
        private ILoggingService m_logging;
        #endregion

        #region Properties
        public event EventHandler<CommandRecievedEventArgs> CommandRecieved;          // The event that notifies about a new Command being recieved
        #endregion

        // constructor
        public ImageServer(IImageController controller, ILoggingService loggingService)
        {
            this.m_logging = loggingService;
            this.m_controller = controller;

            // creating handlers to all of the dir
            string handlersList = ConfigurationManager.AppSettings["Handlers"];
            string[] handlers = handlersList.Split(';');
            for(int i = 0; i < handlers.Length; i++)
            {
                this.createHandler(handlers[i]);
            }   
        }

        public void createHandler(string directory)
        {
            DirectoyHandler h = new DirectoyHandler(this.m_controller, this.m_logging, directory);
            CommandRecieved += h.OnCommandRecieved;
            h.DirectoryClose += onCloseServer;
            h.StartHandleDirectory(directory);
        }

        public void onCloseServer(object sender, DirectoryCloseEventArgs e)
        {
            DirectoyHandler h = (DirectoyHandler)sender;
            CommandRecieved -= h.OnCommandRecieved;
            h.DirectoryClose -= onCloseServer;
        }

        public void sendCommand(CommandRecievedEventArgs e)
        {
            CommandRecieved?.Invoke(this, e);
        }

    }
}
