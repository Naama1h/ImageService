using ImageServiceGUI.Command;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ImageServiceGUI.ViewModels
{
    class MainWindowViewModel
    {
        public AppConfigViewModel AppConfigViewModel { get; set; }
        public ICommand removeCommand { get; private set; }

        public MainWindowViewModel()
        {
            this.AppConfigViewModel = new AppConfigViewModel();
            this.AppConfigViewModel.PropertyChanged += PropertyChanged;
            //this.removeCommand = new DelegateCommand<object>(this.OnRemove, this.CanRemove);
            this.removeCommand = new RemoveCommand(this);
        }

        private void PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var command = this.removeCommand as RemoveCommand;
            command.RaiseCanExecuteChanged();
        }

        private string BuildResultString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(this.AppConfigViewModel.appConfigModel.Handler);
            return builder.ToString();
        }

        public bool CanRemove()
        {
            if (string.IsNullOrEmpty(this.AppConfigViewModel.appConfigModel.Handler))
            {
                return false;
            }
            return true;
        }

        public void OnRemove()
        {
            if (string.IsNullOrEmpty(this.AppConfigViewModel.appConfigModel.Handler))
            {
                //this.AllHandlers
            }
            Console.Write("remove\n");
        }
    }
}
