using ImageServiceCommunication.Enums;
using ImageServiceCommunication;
using ImageServiceCommunication.Event;
using ImageServiceWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.IO;
using System.Threading;

namespace ImageServiceWeb.Controllers
{
    public class FirstController : Controller
    {
        // members:
        static ImageWebModel imageWebModel = new ImageWebModel();
        public static ConfigModel configModel = new ConfigModel();
        public static AskIfRemoveModel askIfRemoveModel = new AskIfRemoveModel();
        public static PhotosModel photoModel = new PhotosModel();
        public static List<LogMessage> logs = new List<LogMessage>();
        public static List<LogMessage> viewLogs = new List<LogMessage>();
        public static string filterType = "";
        public static string outputDir;
        private static bool waitForDeleteHandler = false;

        /// <summary>
        /// constructor
        /// </summary>
        static FirstController()
        {
            ClientSingleton.Instance.DataReceived += getMessageFromServer;
        }

        /// <summary>
        /// view the ImageWeb window
        /// </summary>
        /// <returns>ImageWeb window</returns>
        public ActionResult ImageWeb()
        {
            outputDir = configModel.outputDir;
            imageWebModel.numOfImages = countImages(outputDir);
            return View(imageWebModel);
        }

        /// <summary>
        /// view the Config window
        /// </summary>
        /// <returns>Config window</returns>
        public ActionResult Config()
        {
            while (waitForDeleteHandler) {}
            return View(configModel);
        }

        /// <summary>
        /// view the Logs window
        /// </summary>
        /// <returns>Logs window</returns>
        public ActionResult Logs()
        {
            return View(viewLogs);
        }

        /// <summary>
        /// view the Photos window
        /// </summary>
        /// <returns>Photos window</returns>
        public ActionResult Photos()
        {
            outputDir = configModel.outputDir;
            if (outputDir != null)
            {
                getPhotos(outputDir);
            }
            return View(photoModel);
        }

        /// <summary>
        /// view the askIfRemove window
        /// </summary>
        /// <param name="h">handler</param>
        /// <returns>askIfRemove Window</returns>
        public ActionResult askIfRemove(string h)
        {
            askIfRemoveModel.handler = h;
            return View(askIfRemoveModel);
        }

        /// <summary>
        /// view the askIfDelete window
        /// </summary>
        /// <param name="idOfPhoto">the id of the photo</param>
        /// <returns>askIfDelete Window</returns>
        public ActionResult askIfDelete(int idOfPhoto)
        {
            foreach (ThumbnailPhoto photo in photoModel.photos)
            {
                if (photo.ID.Equals(idOfPhoto))
                {
                    return View(photo);
                }
            }
            return View("Error");
        }

        /// <summary>
        /// view the View window
        /// </summary>
        /// <param name="idOfPhoto">the id of the photo</param>
        /// <returns>View Window</returns>
        public ActionResult View(int idOfPhoto)
        {
            foreach (ThumbnailPhoto photo in photoModel.photos)
            {
                if (photo.ID.Equals(idOfPhoto))
                {
                    return View(photo);
                }
            }
            return View("Error");
        }

        /// <summary>
        /// filter the logs
        /// </summary>
        /// <param name="type">log to show</param>
        [HttpPost]
        public void FilterLogs(string type)
        {
            filterType = type;
            viewLogs.Clear();
            foreach (LogMessage log in logs)
            {
                if (log.Type.Equals(type))
                {
                    viewLogs.Add(log);
                } else if (type.Equals(""))
                {
                    viewLogs.Add(log);
                }
            }
        }

        /// <summary>
        /// remove handler
        /// </summary>
        /// <param name="handler">the path of the handler</param>
        [HttpPost]
        public void removeHandler(string handler)
        {
            waitForDeleteHandler = true;
            string[] args = { handler };
            CommandMessage message1 = new CommandMessage((int)CommandEnum.CloseHandler, args);
            ClientSingleton.Instance.sendmessage(message1.ToJSON());
            while (configModel.handlers.Contains(handler)) {}
            waitForDeleteHandler = false;
        }

        /// <summary>
        /// delete Image
        /// </summary>
        /// <param name="path">the path</param>
        [HttpPost]
        public void deleteImage(string path)
        {
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
            if (System.IO.File.Exists(path.Replace("\\Thumbnails", string.Empty)))
            {
                System.IO.File.Delete(path.Replace("\\Thumbnails", string.Empty));
            }
            if (System.IO.File.Exists(path.Replace("/Thumbnails", string.Empty)))
            {
                System.IO.File.Delete(path.Replace("/Thumbnails", string.Empty));
            }
        }

        /// <summary>
        /// Get The Settings Message
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">The message</param>
        public static void getMessageFromServer(object sender, DataRecivedEventArgs e)
        {
            if (e.Data == null)
            {
                return;
            }
            CommandMessage cm = CommandMessage.ParseJSon(e.Data);
            // check if this is a settings message
            if (cm.CommandID == (int)CommandEnum.GetConfigCommand)
            {
                int i = 0;
                // update the handlers
                while (cm.CommandArgs[i] != null)
                {
                    configModel.handlers.Add(cm.CommandArgs[i]);
                    i++;
                }
                // update the rest members
                i++;
                configModel.outputDir = cm.CommandArgs[i];
                outputDir = cm.CommandArgs[i];
                i++;
                configModel.sourceName = cm.CommandArgs[i];
                i++;
                configModel.logName = cm.CommandArgs[i];
                i++;
                configModel.thumbnailSize = cm.CommandArgs[i];
                // send back that we add the settings
                string[] args = { "add to setting" };
                CommandMessage message = new CommandMessage((int)CommandEnum.TcpMessage, args);
                ClientSingleton.Instance.sendmessage(message.ToJSON());
            }
            // check if we need to remove handler from the list
            else if (cm.CommandID == (int)CommandEnum.CloseHandler)
            {
                configModel.handlers.Remove(cm.CommandArgs[0]);
            } else if(cm.CommandID == (int)CommandEnum.LogCommand)
            {
                // check the status
                if (cm.CommandArgs[0].Equals(MessageTypeEnum.FAIL.ToString()))
                {
                    LogMessage message = new LogMessage(MessageTypeEnum.FAIL, cm.CommandArgs[1]);
                    logs.Add(message);
                    if (filterType.Equals(message.Type) || filterType.Equals(""))
                    {
                        viewLogs.Add(message);
                    }
                }
                else if (cm.CommandArgs[0].Equals(MessageTypeEnum.INFO.ToString()))
                {
                    LogMessage message = new LogMessage(MessageTypeEnum.INFO, cm.CommandArgs[1]);
                    logs.Add(message);
                    if (filterType.Equals(message.Type) || filterType.Equals(""))
                    {
                        viewLogs.Add(message);
                    }
                }
                else
                {
                    LogMessage message = new LogMessage(MessageTypeEnum.WARNING, cm.CommandArgs[1]);
                    logs.Add(message);
                    if (filterType.Equals(message.Type) || filterType.Equals(""))
                    {
                        viewLogs.Add(message);
                    }
                }
                // send back that the message added:
                string[] args = { "add to log" };
                CommandMessage message1 = new CommandMessage((int)CommandEnum.TcpMessage, args);
                ClientSingleton.Instance.sendmessage(message1.ToJSON());
            }
        }

        /// <summary>
        /// count Images
        /// </summary>
        /// <param name="outputDir">the output directory</param>
        /// <returns>num of images</returns>
        public int countImages(string outputDir)
        {
            int fileCount = 0;
            if (Directory.Exists(outputDir))
            {
                fileCount = Directory.EnumerateFiles(outputDir, "*.jpg", SearchOption.AllDirectories).Count();
            }
            return fileCount;
        }

        /// <summary>
        /// update the list of the photos
        /// </summary>
        /// <param name="outputDir">the output directory path</param>
        public void getPhotos(string outputDir)
        {
            photoModel.photos.Clear();
            if (Directory.Exists(outputDir + "\\Thumbnails"))
            {
                string[] photos = Directory.GetFiles(outputDir + "\\Thumbnails", "*.jpg", SearchOption.AllDirectories);
                foreach (string photo in photos)
                {
                    photoModel.photos.Add(new ThumbnailPhoto(Path.GetFileName(photo), new DirectoryInfo(Path.GetDirectoryName(Path.GetDirectoryName(photo))).Name,
                        new DirectoryInfo(Path.GetDirectoryName(photo)).Name, photo));
                }
            }
        }
    }
}