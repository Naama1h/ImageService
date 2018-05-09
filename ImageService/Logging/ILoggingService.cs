using ImageService.Logging.Model;
using System;
using ImageService.Enums;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Logging
{
    // interface of logging service
    public interface ILoggingService
    {
        event EventHandler<MessageRecievedEventArgs> MessageRecieved;

        /// <summary>
        /// invoke the messageRecievedEventArgs to write the message to the logger.
        /// </summary>
        /// <param name="message">The message that will be in the logger</param>
        /// <param name="type">The type of the message of the event</param>
        void Log(string message, MessageTypeEnum type);
    }
}
