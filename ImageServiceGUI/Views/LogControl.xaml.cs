﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ImageServiceGUI.ViewModels;

namespace ImageServiceGUI
{
    /// <summary>
    /// Interaction logic for LogControl.xaml
    /// </summary>
    public partial class LogControl : UserControl
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public LogControl()
        {
            InitializeComponent();
            this.DataContext = new LogViewModel();
        }
    }
}
