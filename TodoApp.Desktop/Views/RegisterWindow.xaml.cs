using System.Windows;
using TodoApp.Desktop.ViewModels;

namespace TodoApp.Desktop.Views;

public partial class RegisterWindow : Window
{
    public RegisterWindow()
    {
        InitializeComponent();
        var vm = new RegisterViewModel();
        DataContext = vm;
        vm.RegisterCompleted += (success, profile) =>
        {
            if (success && profile != null)
            {
                var mainWindow = new MainWindow(profile);
                mainWindow.Show();
                Close();
            }
            else
            {
                var loginWindow = new LoginWindow();
                loginWindow.Show();
                Close();
            }
        };
        PasswordBox.PasswordChanged += (s, e) => vm.Password = PasswordBox.Password;
    }
}