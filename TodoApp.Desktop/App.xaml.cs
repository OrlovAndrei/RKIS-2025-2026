using System.Windows;
using TodoApp.Data;
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
            
            using (var context = new AppDbContext())
            {
                context.Database.EnsureCreated();
            }
            
            var mainViewModel = new MainViewModel();
            var navigationService = new NavigationService(mainViewModel);
            mainViewModel.Initialize(navigationService);
            navigationService.NavigateTo<LoginViewModel>();
            
            var mainWindow = new MainWindow
            {
                DataContext = mainViewModel
            };
            mainWindow.Show();
        }
    }
}