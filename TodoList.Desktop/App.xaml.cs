using System.Windows;
using TodoList.Data;
using TodoListDesktop.Services;
using TodoListDesktop.ViewModels;
using TodoListDesktop.Views;

namespace TodoListDesktop;

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
            DataContext = new MainViewModel(taskService)
        };

        window.Show();
    }
}
