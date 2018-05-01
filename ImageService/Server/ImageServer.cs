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
using System.Net;
using System.Net.Sockets;

namespace ImageService.Server
{
    // Image Server
    public class ImageServer
    {
        #region Members
        private IImageController m_controller;          // Image Controller
        private ILoggingService m_logging;              // Logging Service

        private int port;
        private TcpListener listener;
        private IClientHandler ch;
        #endregion

        #region Properties
        public event EventHandler<CommandRecievedEventArgs> CommandRecieved;          // The event that notifies about a new Command being recieved
        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="controller">Image Controller</param>
        /// <param name="loggingService">Logging Service</param>
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

        /// <summary>
        /// Create handler
        /// </summary>
        /// <param name="directory">The Path Of The Directory</param>
        public void createHandler(string directory)
        {
            DirectoyHandler h = new DirectoyHandler(this.m_controller, this.m_logging, directory);
            CommandRecieved += h.OnCommandRecieved;
            h.DirectoryClose += onCloseServer;
            h.StartHandleDirectory(directory);
        }

        /// <summary>
        /// Close Handler
        /// </summary>
        /// <param name="sender">the handler</param>
        /// <param name="e">the event</param>
        public void onCloseServer(object sender, DirectoryCloseEventArgs e)
        {
            DirectoyHandler h = (DirectoyHandler)sender;
            CommandRecieved -= h.OnCommandRecieved;
            h.DirectoryClose -= onCloseServer;
        }

        /// <summary>
        /// Invoke the command that close the handlers
        /// </summary>
        public void closingServer()
        {
            this.CommandRecieved?.Invoke(this, new CommandRecievedEventArgs((int)CommandEnum.CloseCommand, null, ""));
        }

        public void Start()
        {
            IPEndPoint ep = new
            IPEndPoint(IPAddress.Parse("127.0.0.1"), port);
            listener = new TcpListener(ep);

            listener.Start();
            Console.WriteLine("Waiting for connections...");

            Task task = new Task(() => {
                while (true)
                {
                    try
                    {
                        TcpClient client = listener.AcceptTcpClient();
                        Console.WriteLine("Got new connection");
                        ch.HandleClient(client);
                    }
                    catch (SocketException)
                    {
                        break;
                    }
                }
                Console.WriteLine("Server stopped");
            });
            task.Start();
        }
    }
}
