using ImageService.Commands;
using ImageService.Controller;
using ImageService.Enums;
using ImageService.Modal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ImageService.Controller
{
    // image controller
    public class ImageController : IImageController
    {
        private IImageServiceModal m_modal;                      // The Modal Object
        private Dictionary<int, ICommand> commands;              // Dictionary of commands

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="modal">Image Service Madel</param>
        public ImageController(IImageServiceModal modal)
        {
            m_modal = modal;                                    // Storing the Modal Of The System
            commands = new Dictionary<int, ICommand>()
            {
                {(int)CommandEnum.NewFileCommand, new NewFileCommand(m_modal)}
            };
        }

        /// <summary>
        /// Execute The Command
        /// </summary>
        /// <param name="commandID">The Command ID</param>
        /// <param name="args">The Args</param>
        /// <param name="resultSuccesful">The Result Succesful</param>
        /// <returns>result of execute</returns>
        public string ExecuteCommand(int commandID, string[] args, out bool resultSuccesful)
        {
            bool status = true;
            Task<Tuple<string, bool>> t = Task<Tuple<string, bool>>.Run(() =>
            {
                return Tuple.Create(commands[commandID].Execute(args, out status), status);
            });
            resultSuccesful = t.Result.Item2;
            return t.Result.Item1;
        }
    }
}