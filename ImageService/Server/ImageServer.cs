using ImageService.Controller;
using ImageService.Commands;
using ImageService.Handler;
using ImageService.Enums;
using ImageService.Logging;
using ImageService.Model.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;
using ImageServiceCommunication;
using ImageServiceCommunication.Event;
using System.Collections;
using ImageService.Logging.Model;

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
        private ArrayList clients;

        private ArrayList logMessage;

        private Dictionary<string, IDirectoryHandler> handlers;
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
            this.handlers = new Dictionary<string, IDirectoryHandler>();

            // creating handlers to all of the dir
            string handlersList = ConfigurationManager.AppSettings["Handlers"];
            string[] handlers = handlersList.Split(';');
            for(int i = 0; i < handlers.Length; i++)
            {
                this.createHandler(handlers[i]);
            }
            this.clients = new ArrayList();
            this.m_logging.MessageRecieved += saveLogMessage;
            //StartServer();
        }

        /// <summary>
        /// Create handler
        /// </summary>
        /// <param name="directory">The Path Of The Directory</param>
        public void createHandler(string directory)
        {
            DirectoyHandler h = new DirectoyHandler(this.m_controller, this.m_logging, directory);
            handlers.Add(directory, h);
            this.m_controller.addDelegate(onCloseServer);
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
            this.handlers.Remove(e.DirectoryPath);
        }

        /// <summary>
        /// Invoke the command that close the handlers
        /// </summary>
        public void closingServer()
        {
            this.CommandRecieved?.Invoke(this, new CommandRecievedEventArgs((int)CommandEnum.CloseCommand, null, ""));
        }

        public void StartServer()
        {
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), port);
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
                        ch.DataReceived += removeHandler;
                        sendSettings(client);
                        this.clients.Add(client);
                        if(this.clients.Count == 1)
                        {
                            this.m_logging.MessageRecieved += sendLogMessage;
                            foreach (MessageRecievedEventArgs message in this.logMessage)
                            {
                                new Task(() =>
                                {
                                    string[] args = { message.Status.ToString(), message.Message };
                                    CommandMessage message1 = new CommandMessage((int)CommandEnum.LogCommand, args);
                                    ch.sendmessage(client, message1.ToJSON());
                                    ch.recivedmessage(client);
                                }).Start();
                            }
                        }
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

        public void removeHandler(object sender, DataRecivedEventArgs e)
        {
            CommandMessage cm = CommandMessage.ParseJSon(e.Data);
            if (cm.CommandID.Equals(CommandEnum.CloseCommand))
            {
                DirectoyHandler h = (DirectoyHandler)this.handlers[cm.CommandArgs[1]];
                CommandRecieved -= h.OnCommandRecieved;
                h.DirectoryClose -= onCloseServer;
                this.handlers.Remove(cm.CommandArgs[1]);
            }
        }

        public void sendSettings(TcpClient client)
        {
            new Task(() =>
            {
                string[] args = { "OutPutDir", ConfigurationManager.AppSettings["OutPutDir"] };
                CommandMessage message = new CommandMessage((int)CommandEnum.GetConfigCommand, args);
                ch.sendmessage(client, message.ToJSON());
                ch.recivedmessage(client);
            }).Start();
            new Task(() =>
            {
                string[] args = { "SourceName", ConfigurationManager.AppSettings["SourceName"] };
                CommandMessage message = new CommandMessage((int)CommandEnum.GetConfigCommand, args);
                ch.sendmessage(client, message.ToJSON());
                ch.recivedmessage(client);
            }).Start();
            new Task(() =>
            {
                string[] args = { "LogName", ConfigurationManager.AppSettings["LogName"] };
                CommandMessage message = new CommandMessage((int)CommandEnum.GetConfigCommand, args);
                ch.sendmessage(client, message.ToJSON());
                ch.recivedmessage(client);
            }).Start();
            new Task(() =>
            {
                string[] args = { "ThumbnailSize", ConfigurationManager.AppSettings["ThumbnailSize"] };
                CommandMessage message = new CommandMessage((int)CommandEnum.GetConfigCommand, args);
                ch.sendmessage(client, message.ToJSON());
                ch.recivedmessage(client);
            }).Start();
            new Task(() =>
            {
                string handlersList = ConfigurationManager.AppSettings["Handlers"];
                string[] handlers = handlersList.Split(';');
                string[] args = { "Handlers" };
                for (int i = 0; i < handlers.Length; i++)
                {
                    args[i+1] = handlers[i];
                }
                args[handlers.Length+2] = null;
                CommandMessage message = new CommandMessage((int)CommandEnum.GetConfigCommand, args);
                ch.sendmessage(client, message.ToJSON());
                ch.recivedmessage(client);
            }).Start();
        }

        public void sendLogMessage(Object sender, MessageRecievedEventArgs message)
        {
            foreach (TcpClient client in this.clients)
            {
                new Task(() =>
                {
                    string[] args = { message.Status.ToString(), message.Message };
                    CommandMessage message1 = new CommandMessage((int)CommandEnum.LogCommand, args);
                    ch.sendmessage(client, message1.ToJSON());
                    ch.recivedmessage(client);
                }).Start();
            }
        }

        public void saveLogMessage(Object sender, MessageRecievedEventArgs message)
        {
            this.logMessage.Add(message);
        }
    }
}
