using ImageServiceGUI.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
//using Microsoft.Practices.Prism.Commands;

namespace ImageServiceGUI.ViewModels
{
    class AppConfigViewModel : INotifyPropertyChanged
    {
        private AppConfigModel m_AppConfigModel;
        //public ICommand RemoveCommand { get; private set; }
        public IEnumerable<string> AllHandlers { get; private set; }
        #region Notify Changed
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        public AppConfigModel appConfigModel
        {
            get { return this.m_AppConfigModel; }
            set
            {
                this.m_AppConfigModel = value;
            }
        }

        public AppConfigViewModel()
        {
            this.m_AppConfigModel = new AppConfigModel();
            m_AppConfigModel.PropertyChanged += delegate (Object sender, PropertyChangedEventArgs e) {
                NotifyPropertyChanged(e.PropertyName);
            };
            //this.RemoveCommand = new DelegateCommand<object>(this.OnRemove, this.CanRemove);
            this.AllHandlers = new[] { "Red", "Blue", "Green" };
        }

        protected void NotifyPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

    }
}
