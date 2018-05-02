using ImageServiceGUI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageServiceGUI.Command
{
    class RemoveCommand : ICommand
    {
        private MainWindowViewModel viewModel;
        public event EventHandler CanExecuteChanged;

        public RemoveCommand(MainWindowViewModel vm)
        {
            this.viewModel = vm;
        }

        public bool CanExacute()
        {
            return this.viewModel.CanRemove();
        }

        public void ExacuteCommand()
        {
            this.viewModel.OnRemove();
        }

        public void RaiseCanExecuteChanged()
        {
            // do something
        }
    }
}
