using System.Net.Http;
using System.Windows;
using TodoListDesktop.Services;
using TodoListDesktop.ViewModels;
using TodoListDesktop.Views;

namespace TodoListDesktop;

public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        var httpClient = new HttpClient
        {
            BaseAddress = new Uri("http://localhost:5088/")
        };
        var apiClient = new TodoApiClient(httpClient);
        var taskService = new TodoTaskService(apiClient);

        var window = new MainWindow
        {
            DataContext = new MainViewModel(taskService)
        };

        window.Show();
    }
}
