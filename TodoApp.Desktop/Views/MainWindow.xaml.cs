using System.Windows;
using TodoApp.Desktop.ViewModels;
using TodoApp.Models;

namespace TodoApp.Desktop.Views;

public partial class MainWindow : Window
{
    public MainWindow(Profile profile)
    {
        InitializeComponent();
        var vm = new MainViewModel(profile);
        DataContext = vm;
        vm.LogoutRequested += () =>
        {
            var loginWindow = new LoginWindow();
            Application.Current.MainWindow = loginWindow;
            loginWindow.Show();
            Close();
        };
    }
}