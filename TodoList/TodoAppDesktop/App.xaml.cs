using System.Windows;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TodoApp.Data;
using TodoApp.Desktop.Services;
using TodoApp.Desktop.ViewModels;

namespace TodoApp.Desktop
{
    public partial class DesktopApp : Application
    {
        private ServiceProvider? _serviceProvider;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var services = new ServiceCollection();

            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlite("Data Source=todos.db"));

            services.AddScoped<ITodoRepository, TodoRepository>();
            services.AddScoped<IProfileRepository, ProfileRepository>();

            services.AddSingleton<INavigationService, NavigationService>();
            services.AddSingleton<IDialogService, DialogService>();

            services.AddTransient<LoginViewModel>();
            services.AddTransient<RegisterViewModel>();
            services.AddTransient<TodoListViewModel>();
            services.AddTransient<AddEditTodoViewModel>();

            services.AddSingleton<MainViewModel>();

            _serviceProvider = services.BuildServiceProvider();

            using (var scope = _serviceProvider.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                db.Database.EnsureCreated();
            }

            var nav = _serviceProvider.GetRequiredService<INavigationService>();
            var dialog = _serviceProvider.GetRequiredService<IDialogService>();
            var profileRepo = _serviceProvider.GetRequiredService<IProfileRepository>();
            var todoRepo = _serviceProvider.GetRequiredService<ITodoRepository>();

            var loginVm = new LoginViewModel(todoRepo, profileRepo, nav, dialog);
            nav.CurrentViewModel = loginVm;

            var mainVm = _serviceProvider.GetRequiredService<MainViewModel>();
            var mainWindow = new MainWindow { DataContext = mainVm };
            mainWindow.Show();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _serviceProvider?.Dispose();
            base.OnExit(e);
        }
    }
}
