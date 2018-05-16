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
using System.Threading;

namespace ImageService.Server
{
    // Image Server
    public class ImageServer
    {
        #region Members
        private IImageController m_controller;          // Image Controller
        private ILoggingService m_logging;              // Logging Service

        private int port;                               // Port
        private TcpListener listener;                   // Listener
        private ClientHandler ch;                       // Client Handler
        private ArrayList clients;                      // Client Array
        private bool firstClientConnected;              // Check if the first client connected

        private ArrayList logMessages;                  // Loges Array

        private Dictionary<string, IDirectoryHandler> handlers;                        // The Dictionary of the handlers
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
            this.logMessages = new ArrayList();
            this.m_logging.MessageRecieved += saveLogMessage;
            this.port = 8000;
            StartServer();
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
            foreach (TcpClient client in this.clients)
            {
                new Task(() =>
                {
                    string[] args = {};
                    CommandMessage message = new CommandMessage((int)CommandEnum.CloseCommand, args);
                    ClientHandler.Instance.sendmessage(client, message.ToJSON());
                    ClientHandler.Instance.recivedmessage(client);
                }).Start();
            }
        }

        /// <summary>
        /// Start TCP
        /// </summary>
        public void StartServer()
        {
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), port);
            listener = new TcpListener(ep);
            listener.Start();
            Console.WriteLine("Waiting for connections...");
            this.firstClientConnected = true;
            Task task = new Task(() => {
                while (true)
                {
                    try
                    {
                        Console.WriteLine("wait for connection");
                        TcpClient client = listener.AcceptTcpClient();
                        Console.WriteLine("Got new connection");
                        Thread.Sleep(500);
                        sendSettings(client);
                        waitForEndOfClient(client);
                        this.clients.Add(client);
                        if (this.firstClientConnected)
                        {
                            ClientHandler.Instance.DataReceived += removeHandler;
                            this.m_logging.MessageRecieved += sendLogMessage;
                            this.firstClientConnected = false;
                        }
                        // send the logs until now
                        for (int i = 0; i < this.logMessages.Count; i++)
                        {
                            string[] args = { ((MessageRecievedEventArgs)this.logMessages[i]).Status.ToString(), ((MessageRecievedEventArgs)this.logMessages[i]).Message };
                            CommandMessage message1 = new CommandMessage((int)CommandEnum.LogCommand, args);
                            ClientHandler.Instance.sendmessage(client, message1.ToJSON());
                            ClientHandler.Instance.recivedmessage(client);
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

        /// <summary>
        /// remove handler
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">The data</param>
        public void removeHandler(object sender, DataRecivedEventArgs e)
        {
            if (e.Data == null)
            {
                return;
            }
            CommandMessage cm = CommandMessage.ParseJSon(e.Data);
            if (cm.CommandID == (int)CommandEnum.CloseHandler)
            {
                DirectoyHandler h = (DirectoyHandler)this.handlers[cm.CommandArgs[0]];
                CommandRecieved -= h.OnCommandRecieved;
                h.DirectoryClose -= onCloseServer;
                h.dirWatcher.EnableRaisingEvents = false;
                this.handlers.Remove(cm.CommandArgs[0]);
                foreach (TcpClient client in this.clients)
                {
                    new Task(() =>
                    {
                        string[] args = { cm.CommandArgs[0] };
                        CommandMessage message1 = new CommandMessage((int)CommandEnum.CloseHandler, args);
                        ClientHandler.Instance.sendmessage(client, message1.ToJSON());
                        ClientHandler.Instance.recivedmessage(client);
                    }).Start();
                }
            } else if (cm.CommandID == (int)CommandEnum.CloseCommand)
            {
                if(this.clients.Contains((TcpClient)sender))
                {
                    ((TcpClient)sender).Close();
                    this.clients.Remove((TcpClient)sender);
                }
            }
        }

        /// <summary>
        /// Send the settings
        /// </summary>
        /// <param name="client">The client</param>
        public void sendSettings(TcpClient client)
        {
            new Task(() =>
            {
                // save the settings in one string array:
                string[] args = new string[handlers.Count + 5];
                for (int i = 0; i < handlers.Count; i++)
                {
                    args[i] = handlers.ElementAt(i).Key;
                }
                args[handlers.Count] = null;
                args[handlers.Count + 1] = ConfigurationManager.AppSettings["OutPutDir"];
                args[handlers.Count + 2] = ConfigurationManager.AppSettings["SourceName"];
                args[handlers.Count + 3] = ConfigurationManager.AppSettings["LogName"];
                args[handlers.Count + 4] = ConfigurationManager.AppSettings["ThumbnailSize"];
                // make command message from the args:
                CommandMessage message = new CommandMessage((int)CommandEnum.GetConfigCommand, args);
                // send the settings and wait for get a message that the client get the settings:
                ClientHandler.Instance.sendmessage(client, message.ToJSON());
                ClientHandler.Instance.recivedmessage(client);
                // send the client that he wait for handler to remove
                string[] args2 = { "add to setting" };
                CommandMessage message2 = new CommandMessage((int)CommandEnum.TcpMessage, args2);
                ClientHandler.Instance.sendmessage(client, message2.ToJSON());
                ClientHandler.Instance.recivedmessage(client);
            }).Start();
        }

        /// <summary>
        /// wait For End Of Client
        /// </summary>
        /// <param name="client">The client</param>
        public void waitForEndOfClient(TcpClient client)
        {
            new Task(() =>
            { 
                // send the client that he wait for handler to remove
                string[] args2 = { "wait for end" };
                CommandMessage message2 = new CommandMessage((int)CommandEnum.TcpMessage, args2);
                ClientHandler.Instance.sendmessage(client, message2.ToJSON());
                ClientHandler.Instance.recivedmessage(client);
            }).Start();
        }

        /// <summary>
        /// Send log message
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="message">The log message</param>
        public void sendLogMessage(Object sender, MessageRecievedEventArgs message)
        {
            // over on the clients and send them the message
            foreach (TcpClient client in this.clients)
            {
                new Task(() =>
                {
                    string[] args = { message.Status.ToString(), message.Message };
                    CommandMessage message1 = new CommandMessage((int)CommandEnum.LogCommand, args);
                    ClientHandler.Instance.sendmessage(client, message1.ToJSON());
                    ClientHandler.Instance.recivedmessage(client);
                }).Start();
            }
        }

        /// <summary>
        /// Save log message in the array of the log messages
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="message">The log message</param>
        public void saveLogMessage(Object sender, MessageRecievedEventArgs message)
        {
            this.logMessages.Add(message);
        }
    }
}
