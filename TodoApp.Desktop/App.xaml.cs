using System.Windows;
using TodoApp.Data;
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
            
            var mainWindow = new MainWindow();
            mainWindow.Show();
        }
    }
}