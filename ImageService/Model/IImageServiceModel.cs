using ImageService.Model.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Model
{
    public interface IImageServiceModel
    {
        event EventHandler<DirectoryCloseEventArgs> closeHandler;

        /// <summary>
        /// The Function Addes A file to the system
        /// </summary>
        /// <param name="path">The Path of the Image from the file</param>
        /// <returns>Indication if the Addition Was Successful</returns>
        string AddFile(string path, out bool result);

        /// <summary>
        /// The Function remove the handler.
        /// </summary>
        /// <param name="path">The Handler</param>
        /// <returns>The outcomes</returns>
        string settingsMessage(string path, out bool result);
    }
}
