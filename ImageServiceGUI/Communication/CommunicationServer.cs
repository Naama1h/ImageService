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
        private static CommunicationServer instance;
        private TcpClient client;
        private bool clientConnected;
        public event EventHandler<DataRecivedEventArgs> DataReceived;
        private NetworkStream stream;
        private StreamWriter writer;
        private StreamReader reader;

        private CommunicationServer()
        {
            try
            {
                IPEndPoint ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8000);
                this.client = new TcpClient();
                this.client.Connect(ep);
                Console.WriteLine("connected");
                this.clientConnected = true;
                new Task(() =>
                {
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
            }
        }

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

        public void closeConnection()
        {
            if(clientConnected)
            {
                this.client.Close();
                this.clientConnected = false;
            }
        }
    }
}
