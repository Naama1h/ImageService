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
    class MainWindowViewModel
    {
        public AppConfigViewModel AppConfigViewModel { get; set; }
        public ICommand removeCommand { get; private set; }

        // the Handlers member:
        private string m_Handler;
        public string Handler
        {
            get { return m_Handler; }
            set
            {
                m_Handler = value;
                var command = this.removeCommand as DelegateCommand<object>;
                command.RaiseCanExecuteChanged();
                // maybe invoking the event of AppConfigModel
            }
        }

        public MainWindowViewModel()
        {
            this.AppConfigViewModel = new AppConfigViewModel();
            this.AppConfigViewModel.PropertyChanged += PropertyChanged;
            this.removeCommand = new DelegateCommand<object>(this.OnRemove, this.CanRemove);
        }

        private void PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var command = this.removeCommand as DelegateCommand<object>;
            command.RaiseCanExecuteChanged();
        }

        private string BuildResultString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(this.Handler);
            return builder.ToString();
        }

        private bool CanRemove(object obj)
        {
            if (string.IsNullOrEmpty(this.Handler))
            {
                return false;
            }
            return true;
        }

        private void OnRemove(object obj)
        {
            // sendig tcp
            this.AppConfigViewModel.Handlers.Remove(this.Handler); 
            Console.Write("remove\n");
        }
    }
}
