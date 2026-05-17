using System.Windows;
using TodoApp.Desktop.ViewModels;

namespace TodoApp.Desktop.Views;

public partial class LoginWindow : Window
{
    public LoginWindow()
    {
        InitializeComponent();
        var vm = new LoginViewModel();
        DataContext = vm;
        vm.LoginCompleted += (success, profile) =>
        {
            if (success && profile != null)
            {
                var mainWindow = new MainWindow(profile);
                Application.Current.MainWindow = mainWindow;
                mainWindow.Show();
                Close();
            }
            else
            {
                var registerWindow = new RegisterWindow();
                registerWindow.ShowDialog();
            }
        };
        PasswordBox.PasswordChanged += (s, e) => vm.Password = PasswordBox.Password;
    }
}