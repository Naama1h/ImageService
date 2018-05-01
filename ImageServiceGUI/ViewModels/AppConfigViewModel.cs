using ImageServiceGUI.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// ??
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
//

namespace ImageServiceGUI.ViewModels
{
    class AppConfigViewModel : INotifyPropertyChanged
    {
        private AppConfigModel m_AppConfigModel;
        #region Notify Changed
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        public AppConfigViewModel()
        {
            this.m_AppConfigModel = new AppConfigModel();
            m_AppConfigModel.PropertyChanged += delegate (Object sender, PropertyChangedEventArgs e) {
                NotifyPropertyChanged(e.PropertyName);
            };
            this.RemoveCommand = new DelegateCommand<object>(this.OnRemove, this.CanRemove);
        }

        protected void NotifyPropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

        /*
        private void PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var command = this.SubmitCommand as DelegateCommand<object>;
            command.RaiseCanExecuteChanged();
        }
        */

        public ICommand RemoveCommand { get; private set; }

        private void OnRemove(object obj)
        {
            Debug.WriteLine(this.BuildResultString());
        }

        private bool CanRemove(object obj)
        {
            /**
            if (string.IsNullOrEmpty(this.QuestionnaireViewModel.Questionnaire.Name))
            {
                return false;
            }
            */
            return true;
        }

        public AppConfigModel appConfigModel
        {
            get { return this.m_AppConfigModel; }
            set
            {
                this.m_AppConfigModel = value;
            }
        }
    }
}
