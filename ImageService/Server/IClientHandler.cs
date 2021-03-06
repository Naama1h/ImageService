﻿using ImageServiceCommunication.Event;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Server
{
    public interface IClientHandler
    {
        event EventHandler<DataRecivedEventArgs> DataReceived;          // The event when data recieved
        void recivedmessage(TcpClient client);                          // received message
        void sendmessage(TcpClient client, string message);             // send message
    }
}
