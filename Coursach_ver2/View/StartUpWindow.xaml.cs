﻿using Coursach_ver2.ViewModel;
using System;
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
using System.Windows.Shapes;

namespace Coursach_ver2.View
{
    /// <summary>
    /// Логика взаимодействия для StartUpWindow.xaml
    /// </summary>
    public partial class StartUpWindow : Window
    {
        /// <summary>
        /// Инициализирует новый экземпляр окна запуска приложения.
        /// </summary>
        public StartUpWindow()
        {
            InitializeComponent();
            DataContext = new StartUpWindowViewModel();
        }
    }
}
