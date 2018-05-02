using ImageService.Commands;
using ImageService.Modal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageService.Modal.Event;

namespace ImageService.Controller
{
    // interface of Image Controller
    public interface IImageController
    {
        string ExecuteCommand(int commandID, string[] args, out bool result);          // Executing the Command Requet
        void addDelegate(EventHandler<DirectoryCloseEventArgs> func);
    }
}
