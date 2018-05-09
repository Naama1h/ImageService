using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Model.Event
{
    // recieved event
    public class CommandRecievedEventArgs : EventArgs
    {
        public int CommandID { get; set; }          // The Command ID
        public string[] Args { get; set; }          // The args
        public string RequestDirPath { get; set; }  // The Request Directory

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="id">The Command ID</param>
        /// <param name="args">The args</param>
        /// <param name="path">The Request Directory</param>
        public CommandRecievedEventArgs(int id, string[] args, string path)
        {
            CommandID = id;
            Args = args;
            RequestDirPath = path;
        }

        /// <summary>
        /// inplicit the operator of Command Recieved Event Args.
        /// </summary>
        /// <param name="v"></param>
        public static implicit operator CommandRecievedEventArgs(int v)
        {
            throw new NotImplementedException();
        }
    }
}
