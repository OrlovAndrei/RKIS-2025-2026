using System.Windows;
using TodoList.Data;
using TodoListDesktop.Services;
using TodoListDesktop.ViewModels;
using TodoListDesktop.Views;

namespace TodoListDesktop;

public partial class App : Application
{
    protected override async void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        try
        {
            await DatabaseInitializer.InitializeAsync();
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                $"Не удалось подготовить базу данных: {ex.Message}",
                "Ошибка запуска",
                MessageBoxButton.OK,
                MessageBoxImage.Error);

            Shutdown();
            return;
        }

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
