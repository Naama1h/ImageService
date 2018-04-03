using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
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
                Directory.CreateDirectory(this.m_OutputFolder);
                Directory.CreateDirectory(this.m_OutputFolder + "\\Thumbnails");
            }
            else
            {
                if (!Directory.Exists(this.m_OutputFolder + "\\Thumbnails"))
                {
                    Directory.CreateDirectory(this.m_OutputFolder + "\\Thumbnails");
                }
            }
            string year = File.GetCreationTime(path).Year.ToString();
            string month = File.GetCreationTime(path).Month.ToString();
            if (!Directory.Exists(this.m_OutputFolder + "\\" + year))
            {
                Directory.CreateDirectory(this.m_OutputFolder + "\\" + year);
                Directory.CreateDirectory(this.m_OutputFolder + "Thumbnails\\" + year);
                Directory.CreateDirectory(this.m_OutputFolder + "\\" + year + "\\" + month);
                Directory.CreateDirectory(this.m_OutputFolder + "\\Thumbnails\\" + year + "\\" + month);
                File.Copy(path, this.m_OutputFolder + "\\" + year + "\\" + month + "\\" + Path.GetFileName(path));
                // copy to Thumbnails:
                Image image = Image.FromFile(this.m_OutputFolder + "\\Thumbnails\\" + year + "\\" + month + "\\" + Path.GetFileName(path));
                Image thumb = image.GetThumbnailImage(this.m_thumbnailSize, this.m_thumbnailSize, () => false, IntPtr.Zero);
                thumb.Save(Path.ChangeExtension(this.m_OutputFolder + "\\Thumbnails\\" + year + "\\" + month + "\\" + Path.GetFileName(path), "thumb"));
            }
            else
            {
                if (!Directory.Exists(this.m_OutputFolder + "\\" + year + "\\" + month))
                {
                    Directory.CreateDirectory(this.m_OutputFolder + "\\" + year + "\\" + month);
                    Directory.CreateDirectory(this.m_OutputFolder + "\\Thumbnails\\" + year + "\\" + month);
                    File.Copy(path, this.m_OutputFolder + "\\" + year + "\\" + month + "\\" + Path.GetFileName(path));
                    // copy to Thumbnails:
                    Image image = Image.FromFile(this.m_OutputFolder + "\\Thumbnails\\" + year + "\\" + month + "\\" + Path.GetFileName(path));
                    Image thumb = image.GetThumbnailImage(this.m_thumbnailSize, this.m_thumbnailSize, () => false, IntPtr.Zero);
                    thumb.Save(Path.ChangeExtension(this.m_OutputFolder + "\\Thumbnails\\" + year + "\\" + month + "\\" + Path.GetFileName(path), "thumb"));
                }
                else
                {
                    if (!File.Exists(this.m_OutputFolder + "\\" + year + "\\" + month + "\\" + Path.GetFileName(path)))
                    {
                        File.Copy(path, this.m_OutputFolder + "\\" + year + "\\" + month + "\\" + Path.GetFileName(path));
                        // copy to Thumbnails:
                        Image image = Image.FromFile(this.m_OutputFolder + "\\Thumbnails\\" + year + "\\" + month + "\\" + Path.GetFileName(path));
                        Image thumb = image.GetThumbnailImage(this.m_thumbnailSize, this.m_thumbnailSize, () => false, IntPtr.Zero);
                        thumb.Save(Path.ChangeExtension(this.m_OutputFolder + "\\Thumbnails\\" + year + "\\" + month + "\\" + Path.GetFileName(path), "thumb"));
                    }
                }
                
            }
            result = true;
            return String.Empty;
        }
    }
}
