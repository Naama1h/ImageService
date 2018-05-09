using ImageService.Model.Event;
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

namespace ImageService.Model
{
    public class ImageServiceModel : IImageServiceModel
    {
        #region Members
        private string m_OutputFolder;            // The Output Folder
        private int m_thumbnailSize;              // The Size Of The Thumbnail Size

        public event EventHandler<DirectoryCloseEventArgs> closeHandler;
        #endregion

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="outputFolder">The Path of the output folder of the images</param>
        /// <param name="thumbnailSize">The Thumbnail Size</param>
        public ImageServiceModel(string outputFolder, int thumbnailSize)
        {
            this.m_OutputFolder = outputFolder;
            this.m_thumbnailSize = thumbnailSize;
        }

        /// <summary>
        /// The Function Addes A file to the system
        /// </summary>
        /// <param name="path">The Path of the Image from the file</param>
        /// <returns>Indication if the Addition Was Successful</returns>
        public string AddFile(string path, out bool result)
        {
            //check if outputDir exists, if not – create it.  naama did it in desktop!!
            //Check date of picture from path, 
            //move picture from path to directory outputDir in relevant year and month directories.
            //Result – false / true
            //Return value – string – specific errorth
            try
            {
                if (!Directory.Exists(this.m_OutputFolder))
                {
                    DirectoryInfo d = Directory.CreateDirectory(this.m_OutputFolder);
                    d.Attributes = FileAttributes.Directory | FileAttributes.Hidden;
                    Directory.CreateDirectory(this.m_OutputFolder + "\\Thumbnails");
                }
                else
                {
                    if (!Directory.Exists(this.m_OutputFolder + "\\Thumbnails"))
                    {
                        Directory.CreateDirectory(this.m_OutputFolder + "\\Thumbnails");
                    }
                }
                string year = getDate(path).Year.ToString();
                string month = getDate(path).Month.ToString();
                if (!Directory.Exists(this.m_OutputFolder + "\\" + year))
                {
                    Directory.CreateDirectory(this.m_OutputFolder + "\\" + year);
                    Directory.CreateDirectory(this.m_OutputFolder + "\\Thumbnails\\" + year);
                    Directory.CreateDirectory(this.m_OutputFolder + "\\" + year + "\\" + month);
                    Directory.CreateDirectory(this.m_OutputFolder + "\\Thumbnails\\" + year + "\\" + month);
                    // copy to Thumbnails:
                    Image image = Image.FromFile(path);
                    Image thumb = image.GetThumbnailImage(this.m_thumbnailSize, this.m_thumbnailSize, () => false, IntPtr.Zero);
                    thumb.Save(Path.ChangeExtension(this.m_OutputFolder + "\\Thumbnails\\" + year + "\\" + month + "\\" + Path.GetFileName(path), "jpg"));
                    image.Dispose();
                    // move the image:
                    System.Threading.Thread.Sleep(100);
                    File.Move(path, this.m_OutputFolder + "\\" + year + "\\" + month + "\\" + Path.GetFileName(path));
                }
                else
                {
                    if (!Directory.Exists(this.m_OutputFolder + "\\" + year + "\\" + month))
                    {
                        Directory.CreateDirectory(this.m_OutputFolder + "\\" + year + "\\" + month);
                        Directory.CreateDirectory(this.m_OutputFolder + "\\Thumbnails\\" + year + "\\" + month);
                        // copy to Thumbnails:
                        Image image = Image.FromFile(path);
                        Image thumb = image.GetThumbnailImage(this.m_thumbnailSize, this.m_thumbnailSize, () => false, IntPtr.Zero);
                        thumb.Save(Path.ChangeExtension(this.m_OutputFolder + "\\Thumbnails\\" + year + "\\" + month + "\\" + Path.GetFileName(path), "jpg"));
                        image.Dispose();
                        // move the image:
                        System.Threading.Thread.Sleep(100);
                        File.Move(path, this.m_OutputFolder + "\\" + year + "\\" + month + "\\" + Path.GetFileName(path));
                    }
                    else
                    {
                        if (!File.Exists(this.m_OutputFolder + "\\" + year + "\\" + month + "\\" + Path.GetFileName(path)))
                        {
                            // copy to Thumbnails:
                            Image image = Image.FromFile(path);
                            Image thumb = image.GetThumbnailImage(this.m_thumbnailSize, this.m_thumbnailSize, () => false, IntPtr.Zero);
                            thumb.Save(Path.ChangeExtension(this.m_OutputFolder + "\\Thumbnails\\" + year + "\\" + month + "\\" + Path.GetFileName(path), "jpg"));
                            image.Dispose();
                            // move the image:
                            System.Threading.Thread.Sleep(100);
                            File.Move(path, this.m_OutputFolder + "\\" + year + "\\" + month + "\\" + Path.GetFileName(path));
                        }
                        else
                        {
                            int num = 1;
                            while (File.Exists(this.m_OutputFolder + "\\" + year + "\\" + month + "\\" + "(" + num.ToString() + ")" + Path.GetFileName(path)))
                            {
                                num++;
                            }
                            // copy to Thumbnails:
                            Image image = Image.FromFile(path);
                            Image thumb = image.GetThumbnailImage(this.m_thumbnailSize, this.m_thumbnailSize, () => false, IntPtr.Zero);
                            thumb.Save(Path.ChangeExtension(this.m_OutputFolder + "\\Thumbnails\\" + year + "\\" + month + "\\" + "(" + num.ToString() + ")" + Path.GetFileName(path), "jpg"));
                            image.Dispose();
                            // move the image:
                            System.Threading.Thread.Sleep(100);
                            File.Move(path, this.m_OutputFolder + "\\" + year + "\\" + month + "\\" + "(" + num.ToString() + ")" + Path.GetFileName(path));
                        }
                    }

                }
            } catch (Exception e)
            {
                result = false;
                return e.Message;
            }
            result = true;
            return "file was added";
        }

        /// <summary>
        /// The Function get the date from the image.
        /// </summary>
        /// <param name="file">The Image</param>
        /// <returns>Date Time</returns>
        public DateTime getDate(string file)
        {
            DateTime now = DateTime.Now;
            TimeSpan t = now - now.ToUniversalTime();
            return File.GetLastWriteTimeUtc(file) + t;
        }

        /// <summary>
        /// The Function remove the handler.
        /// </summary>
        /// <param name="path">The Handler</param>
        /// <returns>The outcomes</returns>
        public string settingsMessage(string path, out bool result)
        {
            this.closeHandler?.Invoke(this, new DirectoryCloseEventArgs(path, "closing handler"));
            result = true;
            return "handler has removed";
        }

    }
}
