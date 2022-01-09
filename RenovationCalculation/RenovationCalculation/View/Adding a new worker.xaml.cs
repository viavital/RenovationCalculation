﻿using RenovationCalculation.ApplictionViewModel;
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


namespace RenovationCalculation.View
{
    /// <summary>
    /// Логика взаимодействия для Adding_a_new_worker.xaml
    /// </summary>
    public partial class Adding_a_new_worker : Window
    {
        private AddingWorkerViewModel addingWorkerViewModel = new();

        public Adding_a_new_worker()
        {
            this.DataContext = addingWorkerViewModel;
            InitializeComponent();
            addingWorkerViewModel.CloseAddWorkerWindowEvent += CloseAddWorkerWindowHandler;
        }

        private void CloseAddWorkerWindowHandler()
        {
            this.Close();
            addingWorkerViewModel.CloseAddWorkerWindowEvent -= CloseAddWorkerWindowHandler;
        }
    }
}
