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

        private CommunicationServer()
        {
            try
            {
                IPEndPoint ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8000);
                this.client = new TcpClient();
                this.client.Connect(ep);
                this.clientConnected = true;
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
            Console.WriteLine("You are connected");
            using (NetworkStream stream = this.client.GetStream())
            using (BinaryWriter writer = new BinaryWriter(stream))
            {
                // Send message to the server
                writer.Write(message);
            }
        }

        public void recivedmessage()
        {
            Console.WriteLine("You are connected");
            using (NetworkStream stream = this.client.GetStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                // Get result from server
                string message = reader.ReadLine();
                this.DataReceived?.Invoke(this, new DataRecivedEventArgs(message));
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
