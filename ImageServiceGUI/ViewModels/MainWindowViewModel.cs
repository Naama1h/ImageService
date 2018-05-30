using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Prism.Commands;
using ImageServiceCommunication;
using ImageServiceCommunication.Enums;

namespace ImageServiceGUI.ViewModels
{
    class MainWindowViewModel : INotifyPropertyChanged
    {
        public ICommand WindowClosing { get; private set; }

        private bool isConnected;                // check if the client connect to the server
        /// <summary>
        /// The isConnect member
        /// </summary>
        public bool IsConnected
        {
            get
            {
                return this.isConnected;
            }
        }
        /// <summary>
        /// Constructor
        /// </summary>
        public MainWindowViewModel()
        {
            this.isConnected = Communication.CommunicationServer.Instance.IsConnected;
            this.WindowClosing = new DelegateCommand<object>(this.OnWindowClosing);
        }

        /// <summary>
        /// Property Changed Event Handler
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        /**
        event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
        {
            add
            {
                throw new NotImplementedException();
            }

            remove
            {
                throw new NotImplementedException();
            }
        }
    */

        /// <summary>
        /// Property Changed
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">The property that changes</param>
        private void fPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            
        }

        private void OnWindowClosing(object obj)
        {
            string[] args = { "client disconnect" };
            CommandMessage message1 = new CommandMessage((int)CommandEnum.CloseCommand, args);
            Communication.CommunicationServer.Instance.sendmessage(message1.ToJSON());
        }
    }
}
