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
using RenovationCalculation.ApplictionVewModel;

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
            InitializeComponent();
            DataContext = _stackOfAddingWorksViewModel;

           // Binding binding = new Binding();
                       
        }

        //private void Button_AddWork_Click(object sender, RoutedEventArgs e)
        //{
        //    _stackOfAddingWorksViewModel.enteredNewWork = NameOfNewWorkTextBox.Text;
        //    _stackOfAddingWorksViewModel.selectedWorker = NameOfSelectedWorker.SelectedItem.ToString();
        //    _stackOfAddingWorksViewModel.enteredQuantityOfWork = int.Parse(EnteredQuantityOfHours.Text);
        //    _stackOfAddingWorksViewModel.enteresCostOfWork = int.Parse(EnteredCostOfWork.Text);

        //    _stackOfAddingWorksViewModel.CreateNewWork();
        //}
    }
}
