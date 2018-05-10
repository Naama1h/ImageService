using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

using ImageService.Controller;
using ImageService.Logging.Model;
using ImageService.Model;
using ImageService.Server;
using System.Configuration;

namespace ImageService
{
    public enum ServiceState
    {
        SERVICE_STOPPED = 0x00000001,
        SERVICE_START_PENDING = 0x00000002,
        SERVICE_STOP_PENDING = 0x00000003,
        SERVICE_RUNNING = 0x00000004,
        SERVICE_CONTINUE_PENDING = 0x00000005,
        SERVICE_PAUSE_PENDING = 0x00000006,
        SERVICE_PAUSED = 0x00000007,
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct ServiceStatus
    {
        public int dwServiceType;
        public ServiceState dwCurrentState;
        public int dwControlsAccepted;
        public int dwWin32ExitCode;
        public int dwServiceSpecificExitCode;
        public int dwCheckPoint;
        public int dwWaitHint;
    };

    public partial class ImageService : ServiceBase
    {
        private System.ComponentModel.IContainer components2;
        private System.Diagnostics.EventLog eventLog2;
        private int eventId = 1;

        private Server.ImageServer m_imageServer;          // The Image Server
        private Model.IImageServiceModel model;
        private Controller.IImageController controller;
        private Logging.ILoggingService logging;

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool SetServiceStatus(IntPtr handle, ref ServiceStatus serviceStatus);

        public ImageService(string[] args)
        {
            InitializeComponent();
            string eventSourceName = ConfigurationManager.AppSettings["SourceName"];
            string logName = ConfigurationManager.AppSettings["LogName"];
            if (args.Count() > 0)
            {
                eventSourceName = args[0];
            }
            if (args.Count() > 1)
            {
                logName = args[1];
            }
            eventLog2 = new System.Diagnostics.EventLog();
            if (!System.Diagnostics.EventLog.SourceExists(eventSourceName))
            {
                System.Diagnostics.EventLog.CreateEventSource(eventSourceName, logName);
            }
            eventLog2.Source = eventSourceName;
            eventLog2.Log = logName;
            this.logging = new Logging.LoggingService();
            this.model = new ImageServiceModel(ConfigurationManager.AppSettings["OutputDir"], Int32.Parse(ConfigurationManager.AppSettings["ThumbnailSize"]));
            this.logging.MessageRecieved += OnMessage;
            this.controller = new ImageController(this.model);
            this.m_imageServer = new ImageServer(this.controller, this.logging);
        }

        protected override void OnStart(string[] args)
        {
            // Update the service state to Start Pending.  
            ServiceStatus serviceStatus = new ServiceStatus();
            serviceStatus.dwCurrentState = ServiceState.SERVICE_START_PENDING;
            serviceStatus.dwWaitHint = 100000;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);

            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Interval = 60000;
            timer.Elapsed += new System.Timers.ElapsedEventHandler(this.OnTimer);
            timer.Start();

            // Update the service state to Running.  
            serviceStatus.dwCurrentState = ServiceState.SERVICE_RUNNING;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);

            this.logging.Log("In OnStart", Enums.MessageTypeEnum.INFO);
        }

        protected override void OnStop()
        {
            this.logging.Log("In OnStop", Enums.MessageTypeEnum.INFO);
            this.m_imageServer.closingServer();
        }

        public void OnTimer(object sender, System.Timers.ElapsedEventArgs args)
        {
            this.logging.Log("Monitoring the System", Enums.MessageTypeEnum.INFO);
            eventLog2.WriteEntry("Monitoring the System", EventLogEntryType.Information, eventId++);
        }

        protected override void OnContinue()
        {
            this.logging.Log("In OnContinue", Enums.MessageTypeEnum.INFO);
        }

        public void OnMessage(object sender, MessageRecievedEventArgs e)
        {
            this.logging.Log(e.Message, e.Status);
        }
    }
}
