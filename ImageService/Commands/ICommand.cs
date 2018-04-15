using ImageService.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Commands
{
    public interface ICommand
    {
        /// <summary>
        /// The Function That will Execute The command.
        /// </summary>
        /// <param name="args">The Args Of The Command</param>
        /// <param name="result">Result</param>
        /// <returns>the new path or false if there is problem</returns>
        string Execute(string[] args, out bool result);
    }
}
