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
using System.Windows.Navigation;
using System.Windows.Shapes;
using RenovationCalculation.Model;
using System.Data.Entity;
using Microsoft.EntityFrameworkCore;
using RenovationCalculation.ApplictionViewModel;
using RenovationCalculation.View;

namespace RenovationCalculation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        StackOfAddingWorksViewModel stackOfAddingWorksViewModel = new();
        Adding_a_new_worker adding_A_New_Worker = new();
        public MainWindow()
        {

            DataContext = stackOfAddingWorksViewModel;
            InitializeComponent();
            stackOfAddingWorksViewModel.AddNewWorkerEvent += AddNewWorkerEventHandler;
            adding_A_New_Worker.Closing += Adding_A_New_Worker_Closing;

        }

        private void Adding_A_New_Worker_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            stackOfAddingWorksViewModel.isEnabledMainWindow = true;
            NameOfSelectedWorker.SelectedItem = null;
            adding_A_New_Worker = new();
        }

        private void AddNewWorkerEventHandler()
        {
            {
                stackOfAddingWorksViewModel.isEnabledMainWindow = false;
                adding_A_New_Worker.Show();
            }
        }

    }
}
