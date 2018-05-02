using ImageService.Modal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Commands
{
	class AppConfigCommand : ICommand
	{
		private IImageServiceModal m_modal;         // Image Service Model

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="modal">The Image Service Model</param>
		public AppConfigCommand(IImageServiceModal modal)
		{
			m_modal = modal;            // Storing the Modal
		}

		/// <summary>
		/// Execute The Command
		/// </summary>
		/// <param name="args">The Args Of The Command</param>
		/// <param name="result">Result</param>
		/// <returns>the new path or false if there is problem</returns>
		public string Execute(string[] args, out bool result)
		{
			return m_modal.removeHandler(args[0], out result);
		}
	}
}
