using System.Windows;
using Microsoft.EntityFrameworkCore;
using TodoApp.Data;
using TodoApp.Desktop.Services;
using TodoApp.Desktop.ViewModels;
using TodoApp.Desktop.Views;

namespace TodoApp.Desktop;

public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        using (var context = new AppDbContext())
        {
            context.Database.Migrate();
        }

        var navigation = new NavigationService();
        var profileRepository = new ProfileRepository();
        var todoRepository = new TodoRepository();
        var session = new ProfileSessionService();

        var window = new MainWindow
        {
            DataContext = new MainViewModel(navigation, profileRepository, todoRepository, session)
        };

        window.Show();
    }
}
