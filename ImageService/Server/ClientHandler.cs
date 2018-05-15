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

        private ClientHandler()
        {
            this.clientConnected = true;
        }

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

        public void sendmessage(TcpClient client, string message)
        {
            Console.WriteLine("You are connected");
            this.stream = client.GetStream();
            this.writer = new StreamWriter(this.stream);
            // Send message to the server
            try
            {
                this.writer.WriteLine(message);
                this.writer.Flush();
            }
            catch (IOException)
            {
                closeConnection(client);
            }
        }

        public void recivedmessage(TcpClient client)
        {
            Console.WriteLine("You are connected");
            this.stream = client.GetStream();
            this.reader = new StreamReader(this.stream);
            // Get result from server
            try
            {
                string message = reader.ReadLine();
                //this.DataReceived?.Invoke(this, new DataRecivedEventArgs(message));
            }
            catch (IOException)
            {
                closeConnection(client);
            }
        }

        public void closeConnection(TcpClient client)
        {
            if (clientConnected)
            {
                client.Close();
                this.clientConnected = false;
            }
        }
    }
}
