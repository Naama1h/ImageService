﻿
using ImageService.Logging.Model;
using ImageServiceCommunication.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Logging
{
    // logging service
    public class LoggingService : ILoggingService
    {
        public event EventHandler<MessageRecievedEventArgs> MessageRecieved;

        /// <summary>
        /// invoke the messageRecievedEventArgs to write the message to the logger.
        /// </summary>
        /// <param name="message">The message that will be in the logger</param>
        /// <param name="type">The type of the message of the event</param>
        public void Log(string message, MessageTypeEnum type)
        {
            MessageRecieved?.Invoke(this, new MessageRecievedEventArgs(type, message));
        }
    }
}
