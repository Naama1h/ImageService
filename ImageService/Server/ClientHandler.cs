using ImageServiceCommunication;
using ImageServiceCommunication.Enums;
using ImageServiceCommunication.Event;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Server
{
    class ClientHandler : IClientHandler
    {
        private static ClientHandler instance;
        private bool clientConnected;
        public event EventHandler<DataRecivedEventArgs> DataReceived;
        private NetworkStream stream;
        private StreamWriter writer;
        private StreamReader reader;

        /// <summary>
        /// Constructor
        /// </summary>
        private ClientHandler()
        {
            this.clientConnected = true;
        }

        /// <summary>
        /// Instant - make the ClientHandler a singlton
        /// </summary>
        public static ClientHandler Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ClientHandler();
                }
                return instance;
            }
        }

        /// <summary>
        /// Send the message
        /// </summary>
        /// <param name="client">The client that we need to send him the message</param>
        /// <param name="message">The message</param>
        public void sendmessage(TcpClient client, string message)
        {
            try
            {
                this.stream = client.GetStream();
                this.writer = new StreamWriter(this.stream);
                // Send message to the server
                this.writer.WriteLine(message);
                this.writer.Flush();
            }
            catch (IOException)
            {
                string[] args = { "client disconnected" };
                CommandMessage message1 = new CommandMessage((int)CommandEnum.CloseCommand, args);
                string message2 = message1.ToJSON();
                this.DataReceived?.Invoke(client, new DataRecivedEventArgs(message2));
            }
            catch (ObjectDisposedException)
            {
                string[] args = { "client disconnected" };
                CommandMessage message1 = new CommandMessage((int)CommandEnum.CloseCommand, args);
                string message2 = message1.ToJSON();
                this.DataReceived?.Invoke(client, new DataRecivedEventArgs(message2));
            }
        }

        /// <summary>
        /// Recieved message
        /// </summary>
        /// <param name="client">The client that recieved the message</param>
        /// <returns>the new path or false if there is problem</returns>
        public void recivedmessage(TcpClient client)
        {
            try
            {
                this.stream = client.GetStream();
                this.reader = new StreamReader(this.stream);
                // Get result from server
                string message = reader.ReadLine();
                this.DataReceived?.Invoke(client, new DataRecivedEventArgs(message));
            }
            catch (IOException)
            {
                string[] args = { "client disconnected" };
                CommandMessage message1 = new CommandMessage((int)CommandEnum.CloseCommand, args);
                string message = message1.ToJSON();
                this.DataReceived?.Invoke(client, new DataRecivedEventArgs(message));
            }
            catch (ObjectDisposedException)
            {
                string[] args = { "client disconnected" };
                CommandMessage message1 = new CommandMessage((int)CommandEnum.CloseCommand, args);
                string message2 = message1.ToJSON();
                this.DataReceived?.Invoke(client, new DataRecivedEventArgs(message2));
            }
        }

        public bool ClientConnected
        {
            get
            {
                return this.clientConnected;
            }
            set
            {
                this.clientConnected = value;
            }
        }
    }
}
