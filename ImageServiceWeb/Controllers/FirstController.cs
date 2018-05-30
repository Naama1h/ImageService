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

namespace ImageServiceWeb.Controllers
{
    public class FirstController : Controller
    {
        static ImageWebModel imageWebModel = new ImageWebModel();
        static ConfigModel configModel = new ConfigModel();
        public static List<LogMessage> logs = new List<LogMessage>();
        public static List<LogMessage> viewLogs = new List<LogMessage>();
        public string filterType = "";

        public FirstController()
        {
            ClientSingleton.Instance.DataReceived += getMessageFromServer;
        }

       /*
        //GET: First
        public ActionResult Index()
        {
            return View();
        }
        */
        
        public ActionResult ImageWeb()
        {
            return View(imageWebModel);
        }
        
        public ActionResult Config()
        {
            return View(configModel);
        }

        public ActionResult Logs()
        {
            return View(viewLogs);
        }
        
        [HttpPost]
        public void FilterLogs(string type)
        {
            filterType = type;
            viewLogs.Clear();
            foreach (LogMessage log in logs)
            {
                if (!log.Type.Equals(type))
                {
                    viewLogs.Add(log);
                }
            }
        }

        /// <summary>
        /// Get The Settings Message
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">The message</param>
        public void getMessageFromServer(object sender, DataRecivedEventArgs e)
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
                    if (!filterType.Equals(message.Type))
                    {
                        viewLogs.Add(message);
                    }
                }
                else if (cm.CommandArgs[0].Equals(MessageTypeEnum.INFO.ToString()))
                {
                    LogMessage message = new LogMessage(MessageTypeEnum.INFO, cm.CommandArgs[1]);
                    logs.Add(message);
                    if (!filterType.Equals(message.Type))
                    {
                        viewLogs.Add(message);
                    }
                }
                else
                {
                    LogMessage message = new LogMessage(MessageTypeEnum.WARNING, cm.CommandArgs[1]);
                    logs.Add(message);
                    if (!filterType.Equals(message.Type))
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
    }
}