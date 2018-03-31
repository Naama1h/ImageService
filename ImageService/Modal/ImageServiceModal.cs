using System;
using System.Collections.Generic;
using System.Drawing;
//using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace ImageService.Modal
{
    public class ImageServiceModal : IImageServiceModal
    {
        #region Members
        private string m_OutputFolder;            // The Output Folder
        private int m_thumbnailSize;              // The Size Of The Thumbnail Size

        #endregion
        string AddFile(string path, out bool result)
        {
            //check if outputDir exists, if not – create it.  in desktop!!
            //Check date of picture from path, 
            //move picture from path to directory outputDir in relevant year and month directories.
            //Result – false / true
            //Return value – string – specific errorth
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            if (!Directory.Exists(desktopPath + "\\OutputDir")) { 
                System.IO.Directory.CreateDirectory(desktopPath + "\\OutputDir");
            }

        }
    }
}
