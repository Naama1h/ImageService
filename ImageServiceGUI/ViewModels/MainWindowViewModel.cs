using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Prism.Commands;


namespace ImageServiceGUI.ViewModels
{
    class MainWindowViewModel : INotifyPropertyChanged
    {
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
    }
}
