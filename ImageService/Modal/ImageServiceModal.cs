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

        public ImageServiceModal(string outputFolder, int thumbnailSize)
        {
            this.m_OutputFolder = outputFolder;
            this.m_thumbnailSize = thumbnailSize;
        }

        public string AddFile(string path, out bool result)
        {
            //check if outputDir exists, if not – create it.  naama did it in desktop!!
            //Check date of picture from path, 
            //move picture from path to directory outputDir in relevant year and month directories.
            //Result – false / true
            //Return value – string – specific errorth
            if (!Directory.Exists(this.m_OutputFolder)) { 
                System.IO.Directory.CreateDirectory(this.m_OutputFolder);
                System.IO.Directory.CreateDirectory(this.m_OutputFolder + "\\Thumbnails");
            }
            else
            {
                if (!Directory.Exists(this.m_OutputFolder + "\\Thumbnails"))
                {
                    System.IO.Directory.CreateDirectory(this.m_OutputFolder + "\\Thumbnails");
                }
            }
            string year = File.GetCreationTime(path).Year.ToString();
            string month = File.GetCreationTime(path).Month.ToString();
            if (!Directory.Exists(this.m_OutputFolder + "\\" + year))
            {
                System.IO.Directory.CreateDirectory(this.m_OutputFolder + "\\" + year);
                System.IO.Directory.CreateDirectory(this.m_OutputFolder + "Thumbnails\\" + year);
                System.IO.Directory.CreateDirectory(this.m_OutputFolder + "\\" + year + "\\" + month);
                System.IO.Directory.CreateDirectory(this.m_OutputFolder + "\\Thumbnails\\" + year + "\\" + month);
                File.Copy(path, this.m_OutputFolder + "\\" + year + "\\" + month + "\\" + Path.GetFileName(path));
                // copy to Thumbnails!!!
            }
            else
            {
                if (!Directory.Exists(this.m_OutputFolder + "\\" + year + "\\" + month))
                {
                    System.IO.Directory.CreateDirectory(this.m_OutputFolder + "\\" + year + "\\" + month);
                    System.IO.Directory.CreateDirectory(this.m_OutputFolder + "\\Thumbnails\\" + year + "\\" + month);
                    File.Copy(path, this.m_OutputFolder + "\\" + year + "\\" + month + "\\" + Path.GetFileName(path));
                    // copy to Thumbnails!!!
                }
                else
                {
                    if (!File.Exists(this.m_OutputFolder + "\\" + year + "\\" + month + "\\" + Path.GetFileName(path)))
                    {
                        File.Copy(path, this.m_OutputFolder + "\\" + year + "\\" + month + "\\" + Path.GetFileName(path));
                        // copy to Thumbnails!!!
                    }
                }
                
            }
            result = true;
            return String.Empty;
        }
    }
}
