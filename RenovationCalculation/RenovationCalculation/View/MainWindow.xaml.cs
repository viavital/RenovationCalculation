using RenovationCalculation.ApplictionViewModel;
using System.Windows;

namespace RenovationCalculation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //v: тут код не пишемо, якщо виникає потреба щось тут писати - треба шукати проблему.
        public MainWindow()
        {
            DataContext = new StackOfAddingWorksViewModel();
            InitializeComponent();
        }
    }
}
