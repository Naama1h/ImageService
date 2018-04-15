using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Modal.Event
{
    // close event
    public class DirectoryCloseEventArgs : EventArgs
    {
        public string DirectoryPath { get; set; }       // The Path

        public string Message { get; set; }             // The Message That goes to the logger

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="dirPath">The Path of the Image from the file</param>
        /// <param name="message">The message of the event</param>
        public DirectoryCloseEventArgs(string dirPath, string message)
        {
            DirectoryPath = dirPath;                    // Setting the Directory Name
            Message = message;                          // Storing the String
        }

    }
}
