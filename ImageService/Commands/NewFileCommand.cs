using ImageService.Model;
using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Commands
{
    // new file command
	public class NewFileCommand : ICommand
	{
		private IImageServiceModel m_model;         // Image Service Model

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="modal">The Image Service Model</param>
		public NewFileCommand(IImageServiceModel model)
		{
			m_model = model;            // Storing the Model
		}

        /// <summary>
        /// Execute The Command
        /// </summary>
        /// <param name="args">The Args Of The Command</param>
        /// <param name="result">Result</param>
        /// <returns>the new path or false if there is problem</returns>
		public string Execute(string[] args, out bool result)
		{
			// The String Will Return the New Path if result = true, and will return the error message
			if (File.Exists(args[0]))
			{
				return m_model.AddFile(args[0], out result);
			}
			else
			{
				result = false;
				return "error: path is not exists";
			}
		}
	}
}
