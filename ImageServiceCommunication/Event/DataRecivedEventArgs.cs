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
        public string Data { get; set; }

        public DataRecivedEventArgs(string _Data)
        {
            this.Data = _Data;
        }

        public DataRecivedEventArgs(DataRecivedEventArgs Source)
        {
            this.Data = Source.Data;
        }
    }
}
