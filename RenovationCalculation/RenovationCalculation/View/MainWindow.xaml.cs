using RenovationCalculation.ApplictionViewModel;
using System.Windows;

namespace RenovationCalculation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            var context = new StackOfAddingWorksViewModel();
            DataContext = context;
            InitializeComponent();
            Closing += (s, e) => context.Dispose();
        }
    }
}
