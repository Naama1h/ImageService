using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageServiceCommunication.Event
{
    public class DataRecivedEventArgs : EventArgs
    {
        public string Data { get; set; }                                // The Data

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="_Data">The sata</param>
        public DataRecivedEventArgs(string _Data)
        {
            this.Data = _Data;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Source">The previuse DataRecivedEventArgs</param>
        public DataRecivedEventArgs(DataRecivedEventArgs Source)
        {
            this.Data = Source.Data;
        }
    }
}
