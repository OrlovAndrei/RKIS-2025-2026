using System.Windows;
using TodoApp.Desktop.Services;
using TodoApp.Desktop.ViewModels;
using TodoApp.Desktop.Views;

namespace TodoApp.Desktop
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var mainViewModel = new MainViewModel();
            var navigationService = new NavigationService(mainViewModel);

            mainViewModel.Initialize(navigationService);

            navigationService.NavigateTo(new LoginViewModel(mainViewModel));

            var mainWindow = new MainWindow
            {
                DataContext = mainViewModel
            };

            mainWindow.Show();
        }
    }
}