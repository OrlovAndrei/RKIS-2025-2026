using System.Windows;
using TodoList.Services;
using TodoListWpf.Services;
using TodoListWpf.ViewModels;
using TodoListWpf.Views;

namespace TodoListWpf;

public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        var profileRepository = new ProfileRepository();
        var todoRepository = new TodoRepository();
        var taskService = new TodoTaskService(profileRepository, todoRepository);

        var window = new MainWindow
        {
            DataContext = new MainWindowViewModel(taskService)
        };

        window.Show();
    }
}
