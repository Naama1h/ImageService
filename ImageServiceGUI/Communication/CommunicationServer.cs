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

namespace ImageServiceGUI.Communication
{
    class CommunicationServer
    {
        private static CommunicationServer instance;                    // Instance
        private TcpClient client;                                       // client
        private bool clientConnected;                                   // check if the client Connected
        public event EventHandler<DataRecivedEventArgs> DataReceived;   // the event that work when data recieved
        private NetworkStream stream;                                   // The stream
        private StreamWriter writer;                                    // The writer
        private StreamReader reader;                                    // The reader
        private bool isConnected;                                       // Check if the client connected

        /// <summary>
        /// Constructor
        /// </summary>
        private CommunicationServer()
        {
            try
            {
                // try connected to server:
                IPEndPoint ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8000);
                this.client = new TcpClient();
                this.client.Connect(ep);
                this.isConnected = true;
                Console.WriteLine("connected");
                this.clientConnected = true;
                new Task(() =>
                {
                    // wait for recieved message
                    while (this.clientConnected)
                    {
                        try
                        {
                            recivedmessage();
                        }
                        catch (SocketException)
                        {
                            break;
                        }
                    }
                }).Start();
            }
            catch (SocketException)
            {
                this.isConnected = false;
            }
        }

        /// <summary>
        /// Instance - make it singleton
        /// </summary>
        public static CommunicationServer Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new CommunicationServer();
                }
                return instance;
            }
        }

        /// <summary>
        /// Send message
        /// </summary>
        /// <param name="message">The message</param>
        public void sendmessage(string message)
        {
            try
            {
                Console.WriteLine("You are connected");
                this.stream = this.client.GetStream();
                this.writer = new StreamWriter(this.stream);
                // Send message to the server
                this.writer.WriteLine(message);
                this.writer.Flush();
            }
            catch(IOException)
            {
                closeConnection();
            }
        }

        /// <summary>
        /// recieved message
        /// </summary>
        public void recivedmessage()
        {
            try
            {
                this.stream = this.client.GetStream();
                this.reader = new StreamReader(this.stream);
                // Get result from server
                string message = this.reader.ReadLine();
                this.DataReceived?.Invoke(this, new DataRecivedEventArgs(message));
            }
            catch (IOException)
            {
                closeConnection();
            }
        }

        /// <summary>
        /// close the connection
        /// </summary>
        public void closeConnection()
        {
            if(clientConnected)
            {
                this.client.Close();
                this.clientConnected = false;
            }
        }

        // The isConnected member:
        public bool IsConnected
        {
            get { return isConnected; }
            set
            {
                isConnected = value;
            }
        }
    }
}
