using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using ImageService.Controller;

namespace ImageService.Server
{
    class ClientHandler : IClientHandler
    {
        private IImageController contrlr;

        public ClientHandler(IImageController controller)
        {
            this.contrlr = controller;
        }

        public void HandleClient(TcpClient client)
        {
            new Task(() =>
            {
                using (NetworkStream stream = client.GetStream())
                using (StreamReader reader = new StreamReader(stream))
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    string commandLine = reader.ReadLine();
                    Console.WriteLine("Got command: {0}", commandLine);
                    //string result = this.contrlr.ExecuteCommand(commandLine, client);
                    //writer.Write(result);
                }
                client.Close();
            }).Start();
        }
    }
}
