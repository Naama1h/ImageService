using System;
using ImageService.Enums;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Logging.Model
{
    // message recived event args
    public class MessageRecievedEventArgs : EventArgs
    {
        public MessageTypeEnum Status { get; set; }     // the status of the message
        public string Message { get; set; }             // The message that will be in the logger 

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="message">The message that will be in the logger</param>
        /// <param name="type">The type of the message of the event</param>
        public MessageRecievedEventArgs(MessageTypeEnum type, string message)
        {
            this.Message = message;
            this.Status = type;
        }
    }
}
