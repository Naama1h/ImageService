using ImageService.Commands;
using ImageService.Controller;
using ImageServiceCommunication.Enums;
using ImageService.Model;
using ImageService.Model.Event;
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
        private IImageServiceModel m_model;                    // The Modal Object
        private Dictionary<int, ICommand> commands;            // Dictionary of commands

        /// <summary>
        /// Return The Model
        /// </summary>
        /// <returns>the model</returns>
        public IImageServiceModel Model
        {
            get { return this.m_model; }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="modal">Image Service Madel</param>
        public ImageController(IImageServiceModel model)
        {
            m_model = model;                                    // Storing the Modal Of The System
            commands = new Dictionary<int, ICommand>()
            {
                {(int)CommandEnum.NewFileCommand, new NewFileCommand(m_model)}
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

        /// <summary>
        /// Add delegate
        /// </summary>
        /// <param name="func">The function that we add</param>
        public void addDelegate(EventHandler<DirectoryCloseEventArgs> func)
        {
            this.m_model.closeHandler += func;
        }
    }
}