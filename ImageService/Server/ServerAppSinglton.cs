using ImageServiceCommunication;
using ImageServiceCommunication.Enums;
using ImageServiceCommunication.Event;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Server
{
    class ServerAppSinglton
    {
        private static ServerAppSinglton instance;
        private bool clientConnected;
        private NetworkStream stream;
        private BinaryReader reader;

        /// <summary>
        /// Constructor
        /// </summary>
        private ServerAppSinglton()
        {
            this.clientConnected = true;
        }

        /// <summary>
        /// Instant - make the ClientHandler a singlton
        /// </summary>
        public static ServerAppSinglton Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ServerAppSinglton();
                }
                return instance;
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
                this.reader = new BinaryReader(this.stream);
                // Get result from server
                byte[] sizeInBytes = reader.ReadBytes(4);
                if (BitConverter.IsLittleEndian)
                    Array.Reverse(sizeInBytes);
                if (sizeInBytes == null)
                {
                    return;
                }
                int size = BitConverter.ToInt32(sizeInBytes, 0);
                byte[] message = reader.ReadBytes(size);
                Image image = (Bitmap)((new ImageConverter()).ConvertFrom(message));
                sizeInBytes = reader.ReadBytes(4);
                if (BitConverter.IsLittleEndian)
                    Array.Reverse(sizeInBytes);
                size = BitConverter.ToInt32(sizeInBytes, 0);
                byte[] imageNameInBytes = reader.ReadBytes(size);
                string imageName = Encoding.UTF8.GetString(imageNameInBytes, 0, imageNameInBytes.Length);
                string handlersList = ConfigurationManager.AppSettings["Handlers"];
                string[] handlers = handlersList.Split(';');
                for(int i = 0; i < handlers.Length; i++)
                {
                    if (Directory.Exists(handlers[i]))
                    {
                        image.Save(handlers[0] + "/" + imageName + ".jpg");
                        break;
                    }
                }
            }
            catch (IOException)
            {
                this.clientConnected = false;
            }
            catch (ObjectDisposedException)
            {
                this.clientConnected = false;
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