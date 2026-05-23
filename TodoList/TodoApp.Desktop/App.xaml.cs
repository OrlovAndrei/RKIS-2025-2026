using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
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
            // Глобальный перехват необработанных исключений
            AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
            {
                string message = args.ExceptionObject is Exception ex
                    ? ex.ToString()
                    : args.ExceptionObject?.ToString() ?? "Неизвестная ошибка";
                MessageBox.Show($"Необработанное исключение:\n{message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                Environment.Exit(1);
            };

            DispatcherUnhandledException += (sender, args) =>
            {
                MessageBox.Show($"Ошибка в UI-потоке:\n{args.Exception}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                args.Handled = true; // Продолжаем работу, но пишем ошибку
            };

            TaskScheduler.UnobservedTaskException += (sender, args) =>
            {
                MessageBox.Show($"Необработанное исключение в задаче:\n{args.Exception}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                args.SetObserved();
            };

            base.OnStartup(e);

            // Создание/миграция базы данных
            using (var context = new AppDbContext())
            {
                context.Database.EnsureCreated();
            }

            var mainViewModel = new MainViewModel();
            var navigationService = new NavigationService(mainViewModel);
            mainViewModel.Initialize(navigationService);

            // Начальный экран
            navigationService.NavigateTo<LoginViewModel>();

            var mainWindow = new MainWindow
            {
                DataContext = mainViewModel
            };
            mainWindow.Show();
        }
    }
}