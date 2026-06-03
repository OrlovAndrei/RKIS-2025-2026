using System.Windows;
using TodoApp.Desktop.ViewModels;

namespace TodoApp.Desktop.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
        }
    }
}
