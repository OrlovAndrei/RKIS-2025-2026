using System.Threading.Tasks;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TodoApp.Data;
using TodoApp.Desktop.Services;

namespace TodoApp.Desktop.ViewModels
{
    public partial class LoginViewModel : ObservableObject
    {
        private readonly ProfileRepository _profileRepo = new();

        [ObservableProperty]
        private string _login = string.Empty;

        [ObservableProperty]
        private string _password = string.Empty;

        [ObservableProperty]
        private string _errorMessage = string.Empty;

        private INavigationService? Navigation =>
            (Application.Current.MainWindow.DataContext as MainViewModel)?.Navigation;

        [RelayCommand]
        private async Task LoginAsync()
        {
            ErrorMessage = string.Empty;
            
            if (string.IsNullOrWhiteSpace(Login) || string.IsNullOrWhiteSpace(Password))
            {
                ErrorMessage = "Логин и пароль обязательны.";
                return;
            }

            var profile = await _profileRepo.GetByLoginAsync(Login);
            if (profile == null || !profile.CheckPassword(Password))
            {
                ErrorMessage = "Неверный логин или пароль.";
                return;
            }

            var mainVm = Application.Current.MainWindow.DataContext as MainViewModel;
            mainVm?.NavigateToTasks(profile.Id);
        }

        [RelayCommand]
        private void GoToRegister()
        {
            Navigation?.NavigateTo<RegisterViewModel>();
        }
    }
}