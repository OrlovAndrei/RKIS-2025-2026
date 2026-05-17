using System.Windows;
using TodoApp.Desktop.Views;

namespace TodoApp.Desktop;

public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        var loginWindow = new LoginWindow();
        loginWindow.Show();
    }

    // Опционально: обработка необработанных исключений для отладки
    public App()
    {
        this.DispatcherUnhandledException += (s, args) =>
        {
            MessageBox.Show($"Ошибка: {args.Exception.Message}");
            args.Handled = true;
        };
    }
}