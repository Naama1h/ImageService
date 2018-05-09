using ImageService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Commands
{
	class AppConfigCommand : ICommand
	{
		private IImageServiceModel m_model;         // Image Service Model

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="model">The Image Service Model</param>
		public AppConfigCommand(IImageServiceModel model)
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
			return m_model.settingsMessage(args[0], out result);
		}
	}
}
