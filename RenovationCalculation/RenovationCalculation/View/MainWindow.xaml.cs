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
using RenovationCalculation.Model;
using System.Data.Entity;
using Microsoft.EntityFrameworkCore;
using RenovationCalculation.ApplictionViewModel;

namespace RenovationCalculation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        readonly StackOfAddingWorksViewModel _stackOfAddingWorksViewModel = new();
        public MainWindow()
        {
            DataContext = _stackOfAddingWorksViewModel;
            InitializeComponent();
        }
    }
}
