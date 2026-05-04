using System.Windows;
using TodoApp.Desktop.Services;
using TodoApp.Desktop.ViewModels;

namespace TodoApp.Desktop.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
            var mainViewModel = new MainViewModel();
            var navigationService = new NavigationService(mainViewModel);
            mainViewModel.Initialize(navigationService);
            navigationService.NavigateTo<LoginViewModel>();
            
            DataContext = mainViewModel;
        }

        private void HelpAbout_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("TodoApp - приложение для управления задачами\nВерсия 2.0\n© 2024", 
                "О программе", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}