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

        public MainWindowViewModel()
        {
            
        }

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

        private void PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            
        }
    }
}
